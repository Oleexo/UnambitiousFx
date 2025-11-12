namespace UnambitiousFx.Mediator.Transports.Abstractions;

/// <summary>
///     Represents a message received from a transport with acknowledgment callbacks.
/// </summary>
public sealed class TransportMessage
{
    /// <summary>
    ///     Gets or initializes the unique message identifier.
    /// </summary>
    public required string MessageId { get; init; }

    /// <summary>
    ///     Gets or initializes the message body as a byte array.
    /// </summary>
    public required byte[] Body { get; init; }

    /// <summary>
    ///     Gets or initializes the message properties (headers, metadata).
    /// </summary>
    public required IReadOnlyDictionary<string, string> Properties { get; init; }

    /// <summary>
    ///     Gets or initializes the callback to acknowledge successful message processing.
    /// </summary>
    public required Func<CancellationToken, ValueTask> AcknowledgeAsync { get; init; }

    /// <summary>
    ///     Gets or initializes the callback to reject a message and optionally send it to dead-letter.
    /// </summary>
    public required Func<string, CancellationToken, ValueTask> RejectAsync { get; init; }
}
