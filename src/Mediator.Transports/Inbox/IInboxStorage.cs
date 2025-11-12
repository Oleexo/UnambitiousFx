namespace UnambitiousFx.Mediator.Transports.Inbox;

/// <summary>
///     Defines the interface for inbox storage used for message deduplication.
/// </summary>
/// <remarks>
///     This interface will be fully implemented in Phase 6.
///     It is referenced here to support optional inbox deduplication in MessageReceiverHost.
/// </remarks>
public interface IInboxStorage
{
    /// <summary>
    ///     Checks if a message with the specified ID has already been processed.
    /// </summary>
    /// <param name="messageId">The message ID to check.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if the message exists in the inbox; otherwise, false.</returns>
    ValueTask<bool> ExistsAsync(string messageId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Adds a new inbox entry for a processed message.
    /// </summary>
    /// <param name="entry">The inbox entry to add.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A ValueTask representing the asynchronous operation.</returns>
    ValueTask AddAsync(InboxEntry entry, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Removes expired inbox entries.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A ValueTask representing the asynchronous operation.</returns>
    ValueTask CleanupExpiredAsync(CancellationToken cancellationToken = default);
}
