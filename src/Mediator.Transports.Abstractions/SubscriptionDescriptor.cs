namespace UnambitiousFx.Mediator.Transports.Abstractions;

/// <summary>
///     Describes a subscription to messages from a transport.
/// </summary>
public sealed class SubscriptionDescriptor
{
    /// <summary>
    ///     Gets or initializes the message type name.
    /// </summary>
    public required string MessageType { get; init; }

    /// <summary>
    ///     Gets or initializes the topic or queue name to subscribe to.
    /// </summary>
    public required string Topic { get; init; }

    /// <summary>
    ///     Gets or initializes an optional filter expression for message selection.
    /// </summary>
    public string? Filter { get; init; }

    /// <summary>
    ///     Gets or initializes the maximum number of concurrent message handlers.
    /// </summary>
    public int MaxConcurrency { get; init; } = 1;

    /// <summary>
    ///     Gets or initializes the retry policy for failed message processing.
    /// </summary>
    public RetryPolicy? RetryPolicy { get; init; }

    /// <summary>
    ///     Gets or initializes the handler delegate to invoke when messages are received.
    /// </summary>
    public required Func<MessageEnvelope, CancellationToken, ValueTask> Handler { get; init; }
}
