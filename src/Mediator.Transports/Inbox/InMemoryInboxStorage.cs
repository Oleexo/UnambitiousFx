using System.Collections.Concurrent;

namespace UnambitiousFx.Mediator.Transports.Inbox;

/// <summary>
///     In-memory implementation of inbox storage for message deduplication.
/// </summary>
/// <remarks>
///     This implementation uses a ConcurrentDictionary for thread-safe storage.
///     It is suitable for development and testing but should not be used in production
///     as entries are lost on application restart.
/// </remarks>
public sealed class InMemoryInboxStorage : IInboxStorage
{
    private readonly ConcurrentDictionary<string, InboxEntry> _entries = new();

    /// <inheritdoc />
    public ValueTask<bool> ExistsAsync(string messageId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(messageId);

        var exists = _entries.ContainsKey(messageId);
        return ValueTask.FromResult(exists);
    }

    /// <inheritdoc />
    public ValueTask AddAsync(InboxEntry entry, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entry);

        _entries.TryAdd(entry.MessageId, entry);
        return ValueTask.CompletedTask;
    }

    /// <inheritdoc />
    public ValueTask CleanupExpiredAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTimeOffset.UtcNow;
        var expiredKeys = _entries
            .Where(kvp => kvp.Value.ExpiresAt < now)
            .Select(kvp => kvp.Key)
            .ToList();

        foreach (var key in expiredKeys)
        {
            _entries.TryRemove(key, out _);
        }

        return ValueTask.CompletedTask;
    }
}
