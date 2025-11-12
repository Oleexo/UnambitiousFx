using UnambitiousFx.Mediator.Transports.Abstractions;

namespace UnambitiousFx.Mediator.Transports.Outbox;

/// <summary>
///     Represents an outbox entry for distributed message delivery.
///     Contains the message envelope and metadata for reliable dispatch.
/// </summary>
public sealed class OutboxEntry
{
    /// <summary>
    ///     Gets or initializes the unique identifier for this outbox entry (same as the message ID).
    /// </summary>
    public required string MessageId { get; init; }

    /// <summary>
    ///     Gets or initializes the message envelope containing the payload and metadata.
    /// </summary>
    public required MessageEnvelope Envelope { get; init; }

    /// <summary>
    ///     Gets or initializes the name of the transport to use for dispatch.
    /// </summary>
    public string? TransportName { get; init; }

    /// <summary>
    ///     Gets or initializes the topic or queue name for dispatch.
    /// </summary>
    public string? Topic { get; init; }

    /// <summary>
    ///     Gets or initializes the timestamp when the entry was created.
    /// </summary>
    public required DateTimeOffset CreatedAt { get; init; }

    /// <summary>
    ///     Gets or sets the timestamp when the entry was successfully processed.
    /// </summary>
    public DateTimeOffset? ProcessedAt { get; set; }

    /// <summary>
    ///     Gets or sets the number of dispatch attempts made for this entry.
    /// </summary>
    public int AttemptCount { get; set; }

    /// <summary>
    ///     Gets or sets the last error message if dispatch failed.
    /// </summary>
    public string? LastError { get; set; }

    /// <summary>
    ///     Gets or sets the current processing status of this entry.
    /// </summary>
    public OutboxEntryStatus Status { get; set; } = OutboxEntryStatus.Pending;
}
