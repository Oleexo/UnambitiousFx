using System.Collections.Concurrent;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Mediator.Abstractions;
using UnambitiousFx.Mediator.Transports.Abstractions;

namespace UnambitiousFx.Mediator.Transports.Outbox;

/// <summary>
///     In-memory implementation of distributed outbox storage using concurrent collections.
///     Suitable for development and testing scenarios.
/// </summary>
/// <remarks>
///     This implementation stores outbox entries in memory and will lose data on application restart.
///     For production scenarios, use a persistent storage implementation.
/// </remarks>
public sealed class InMemoryDistributedOutboxStorage : IDistributedOutboxStorage
{
    private readonly ConcurrentDictionary<string, OutboxEntry> _entries = new();
    private readonly ConcurrentDictionary<IEvent, EventMetadata> _eventMetadata = new();

    /// <inheritdoc />
    public ValueTask AddAsync(OutboxEntry entry, CancellationToken cancellationToken = default)
    {
        _entries.TryAdd(entry.MessageId, entry);
        return ValueTask.CompletedTask;
    }

    /// <inheritdoc />
    public ValueTask<IReadOnlyList<OutboxEntry>> GetPendingAsync(
        int batchSize,
        CancellationToken cancellationToken = default)
    {
        var pending = _entries.Values
            .Where(e => e.Status == OutboxEntryStatus.Pending)
            .OrderBy(e => e.CreatedAt)
            .Take(batchSize)
            .ToList();

        return ValueTask.FromResult<IReadOnlyList<OutboxEntry>>(pending);
    }

    /// <inheritdoc />
    public ValueTask MarkCompletedAsync(string messageId, CancellationToken cancellationToken = default)
    {
        if (_entries.TryGetValue(messageId, out var entry))
        {
            entry.Status = OutboxEntryStatus.Completed;
            entry.ProcessedAt = DateTimeOffset.UtcNow;
        }

        return ValueTask.CompletedTask;
    }

    /// <inheritdoc />
    public ValueTask MarkFailedAsync(
        string messageId,
        string error,
        CancellationToken cancellationToken = default)
    {
        if (_entries.TryGetValue(messageId, out var entry))
        {
            entry.Status = OutboxEntryStatus.Failed;
            entry.LastError = error;
            entry.AttemptCount++;
        }

        return ValueTask.CompletedTask;
    }

    #region IEventOutboxStorage Implementation (for backward compatibility)

    /// <inheritdoc />
    public ValueTask<Result> AddAsync<TEvent>(
        TEvent @event,
        CancellationToken cancellationToken = default)
        where TEvent : class, IEvent
    {
        // Default to LocalOnly for backward compatibility
        return AddAsync(@event, DistributionMode.LocalOnly, cancellationToken);
    }

    /// <inheritdoc />
    public ValueTask<Result> AddAsync<TEvent>(
        TEvent @event,
        DistributionMode distributionMode,
        CancellationToken cancellationToken = default)
        where TEvent : class, IEvent
    {
        var metadata = new EventMetadata
        {
            Event = @event,
            DistributionMode = distributionMode,
            AddedAt = DateTimeOffset.UtcNow,
            Status = EventStatus.Pending,
            AttemptCount = 0
        };

        _eventMetadata.TryAdd(@event, metadata);
        return ValueTask.FromResult(Result.Success());
    }

    /// <inheritdoc />
    public ValueTask<IEnumerable<IEvent>> GetPendingEventsAsync(
        CancellationToken cancellationToken = default)
    {
        var pending = _eventMetadata
            .Where(kvp => kvp.Value.Status == EventStatus.Pending &&
                          (!kvp.Value.NextAttemptAt.HasValue || kvp.Value.NextAttemptAt <= DateTimeOffset.UtcNow))
            .Select(kvp => kvp.Key)
            .ToList();

        return ValueTask.FromResult<IEnumerable<IEvent>>(pending);
    }

    /// <inheritdoc />
    public ValueTask<Result> MarkAsProcessedAsync(
        IEvent @event,
        CancellationToken cancellationToken = default)
    {
        if (_eventMetadata.TryGetValue(@event, out var metadata))
        {
            metadata.Status = EventStatus.Processed;
            metadata.ProcessedAt = DateTimeOffset.UtcNow;
        }

        return ValueTask.FromResult(Result.Success());
    }

    /// <inheritdoc />
    public ValueTask<Result> ClearAsync(CancellationToken cancellationToken = default)
    {
        _eventMetadata.Clear();
        _entries.Clear();
        return ValueTask.FromResult(Result.Success());
    }

    /// <inheritdoc />
    public ValueTask<Result> MarkAsFailedAsync(
        IEvent @event,
        string reason,
        bool deadLetter,
        DateTimeOffset? nextAttemptAt = null,
        CancellationToken cancellationToken = default)
    {
        if (_eventMetadata.TryGetValue(@event, out var metadata))
        {
            metadata.AttemptCount++;
            metadata.LastError = reason;

            if (deadLetter)
            {
                metadata.Status = EventStatus.DeadLetter;
            }
            else
            {
                metadata.Status = EventStatus.Failed;
                metadata.NextAttemptAt = nextAttemptAt;
            }
        }

        return ValueTask.FromResult(Result.Success());
    }

    /// <inheritdoc />
    public ValueTask<IEnumerable<IEvent>> GetDeadLetterEventsAsync(
        CancellationToken cancellationToken = default)
    {
        var deadLetters = _eventMetadata
            .Where(kvp => kvp.Value.Status == EventStatus.DeadLetter)
            .Select(kvp => kvp.Key)
            .ToList();

        return ValueTask.FromResult<IEnumerable<IEvent>>(deadLetters);
    }

    /// <inheritdoc />
    public ValueTask<int?> GetAttemptCountAsync(
        IEvent @event,
        CancellationToken cancellationToken = default)
    {
        if (_eventMetadata.TryGetValue(@event, out var metadata))
        {
            return ValueTask.FromResult<int?>(metadata.AttemptCount);
        }

        return ValueTask.FromResult<int?>(null);
    }

    /// <inheritdoc />
    public ValueTask<DistributionMode> GetDistributionModeAsync(
        IEvent @event,
        CancellationToken cancellationToken = default)
    {
        if (_eventMetadata.TryGetValue(@event, out var metadata))
        {
            return ValueTask.FromResult(metadata.DistributionMode);
        }

        // Return LocalOnly as default if event not found (defensive programming)
        return ValueTask.FromResult(DistributionMode.LocalOnly);
    }

    #endregion

    #region Private Helper Classes

    private sealed class EventMetadata
    {
        public required IEvent Event { get; init; }
        public DistributionMode DistributionMode { get; init; } = DistributionMode.LocalOnly;
        public required DateTimeOffset AddedAt { get; init; }
        public EventStatus Status { get; set; }
        public DateTimeOffset? ProcessedAt { get; set; }
        public int AttemptCount { get; set; }
        public string? LastError { get; set; }
        public DateTimeOffset? NextAttemptAt { get; set; }
    }

    private enum EventStatus
    {
        Pending,
        Processed,
        Failed,
        DeadLetter
    }

    #endregion
}
