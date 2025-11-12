namespace UnambitiousFx.Mediator.Transports.Abstractions;

/// <summary>
///     Defines metadata annotations for message types that control distribution behavior, topics, and reliability
///     policies.
/// </summary>
public sealed class MessageTraits
{
    /// <summary>
    ///     Gets or initializes the message type.
    /// </summary>
    public required Type MessageType { get; init; }

    /// <summary>
    ///     Gets or initializes the distribution mode for the message.
    /// </summary>
    public DistributionMode DistributionMode { get; init; } = DistributionMode.LocalOnly;

    /// <summary>
    ///     Gets or initializes the name of the transport to use for external dispatch.
    /// </summary>
    public string? TransportName { get; init; }

    /// <summary>
    ///     Gets or initializes the topic or queue name for external dispatch.
    /// </summary>
    public string? Topic { get; init; }

    /// <summary>
    ///     Gets or initializes the partition key for message routing.
    /// </summary>
    public string? PartitionKey { get; init; }

    /// <summary>
    ///     Gets or initializes whether to fail fast on transport errors (true) or use best-effort (false).
    /// </summary>
    public bool FailFast { get; init; }

    /// <summary>
    ///     Gets or initializes whether to use the outbox pattern for reliable delivery.
    /// </summary>
    public bool UseOutbox { get; init; }

    /// <summary>
    ///     Gets or initializes the maximum number of concurrent message handlers.
    /// </summary>
    public int MaxConcurrency { get; init; } = 1;

    /// <summary>
    ///     Gets or initializes the retry policy for failed message processing.
    /// </summary>
    public RetryPolicy? RetryPolicy { get; init; }
}
