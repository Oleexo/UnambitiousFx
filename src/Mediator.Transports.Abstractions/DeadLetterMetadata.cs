namespace UnambitiousFx.Mediator.Transports.Abstractions;

/// <summary>
///     Contains metadata about a message that has been sent to the dead-letter queue.
/// </summary>
public sealed class DeadLetterMetadata
{
    /// <summary>
    ///     Gets or initializes the original message identifier.
    /// </summary>
    public required string OriginalMessageId { get; init; }

    /// <summary>
    ///     Gets or initializes the reason for dead-lettering the message.
    /// </summary>
    public required string FailureReason { get; init; }

    /// <summary>
    ///     Gets or initializes the number of processing attempts before dead-lettering.
    /// </summary>
    public required int AttemptCount { get; init; }

    /// <summary>
    ///     Gets or initializes the timestamp of the first processing attempt.
    /// </summary>
    public required DateTimeOffset FirstAttemptAt { get; init; }

    /// <summary>
    ///     Gets or initializes the timestamp of the last processing attempt.
    /// </summary>
    public required DateTimeOffset LastAttemptAt { get; init; }

    /// <summary>
    ///     Gets or initializes the type name of the exception that caused the failure.
    /// </summary>
    public string? ExceptionType { get; init; }

    /// <summary>
    ///     Gets or initializes the exception message.
    /// </summary>
    public string? ExceptionMessage { get; init; }

    /// <summary>
    ///     Gets or initializes the exception stack trace.
    /// </summary>
    public string? StackTrace { get; init; }
}
