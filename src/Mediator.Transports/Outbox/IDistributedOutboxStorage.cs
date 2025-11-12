using UnambitiousFx.Mediator.Abstractions;

namespace UnambitiousFx.Mediator.Transports.Outbox;

/// <summary>
///     Extends the event outbox storage with distributed messaging capabilities.
///     Provides methods for managing message envelopes in the outbox pattern.
/// </summary>
/// <remarks>
///     This interface extends <see cref="IEventOutboxStorage" /> to maintain backward compatibility
///     with the existing event-only outbox while adding support for distributed message dispatch.
/// </remarks>
public interface IDistributedOutboxStorage : IEventOutboxStorage
{
    /// <summary>
    ///     Adds an outbox entry for later processing.
    /// </summary>
    /// <param name="entry">The outbox entry to add.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    ValueTask AddAsync(OutboxEntry entry, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves pending outbox entries for batch processing.
    /// </summary>
    /// <param name="batchSize">The maximum number of entries to retrieve.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the pending entries.</returns>
    ValueTask<IReadOnlyList<OutboxEntry>> GetPendingAsync(
        int batchSize,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Marks an outbox entry as successfully completed.
    /// </summary>
    /// <param name="messageId">The message ID of the entry to mark as completed.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    ValueTask MarkCompletedAsync(string messageId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Marks an outbox entry as failed with error details.
    /// </summary>
    /// <param name="messageId">The message ID of the entry to mark as failed.</param>
    /// <param name="error">The error message describing the failure.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    ValueTask MarkFailedAsync(
        string messageId,
        string error,
        CancellationToken cancellationToken = default);
}
