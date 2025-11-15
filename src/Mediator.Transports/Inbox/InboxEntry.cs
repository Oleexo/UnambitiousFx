namespace UnambitiousFx.Mediator.Transports.Inbox;

/// <summary>
///     Represents an entry in the inbox storage for message deduplication.
/// </summary>
public sealed class InboxEntry
{
    /// <summary>
    ///     Gets or initializes the unique message identifier.
    /// </summary>
    public required string MessageId { get; init; }

    /// <summary>
    ///     Gets or initializes the name of the source transport.
    /// </summary>
    public required string SourceTransport { get; init; }

    /// <summary>
    ///     Gets or initializes the timestamp when the message was processed.
    /// </summary>
    public required DateTimeOffset ProcessedAt { get; init; }

    /// <summary>
    ///     Gets or initializes the timestamp when this entry expires and can be cleaned up.
    /// </summary>
    public required DateTimeOffset ExpiresAt { get; init; }
}
