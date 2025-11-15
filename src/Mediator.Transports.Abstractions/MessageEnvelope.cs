namespace UnambitiousFx.Mediator.Transports.Abstractions;

/// <summary>
///     Represents a canonical message envelope for cross-boundary serialization.
///     Contains payload, headers, trace metadata, and transport hints.
/// </summary>
public sealed class MessageEnvelope
{
    /// <summary>
    ///     Gets or initializes the unique identifier for this message.
    /// </summary>
    public required string MessageId { get; init; }

    /// <summary>
    ///     Gets or initializes the correlation identifier that groups related messages.
    /// </summary>
    public required string CorrelationId { get; init; }

    /// <summary>
    ///     Gets or initializes the causation identifier (the message ID that caused this message).
    /// </summary>
    public string? CausationId { get; init; }

    /// <summary>
    ///     Gets or initializes the tenant identifier for multi-tenant scenarios.
    /// </summary>
    public string? TenantId { get; init; }

    /// <summary>
    ///     Gets or initializes the timestamp when the message was created.
    /// </summary>
    public required DateTimeOffset Timestamp { get; init; }

    /// <summary>
    ///     Gets or initializes the fully qualified type name of the payload.
    /// </summary>
    public required string PayloadType { get; init; }

    /// <summary>
    ///     Gets or initializes the message payload.
    /// </summary>
    public required object Payload { get; init; }

    /// <summary>
    ///     Gets or initializes the message headers containing metadata and custom extensions.
    /// </summary>
    public required MessageHeaders Headers { get; init; }
}
