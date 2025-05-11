namespace UnambitiousFx.Mediator.Abstractions;

/// <summary>
///     Represents a context with properties and methods for managing data during a process or operation.
/// </summary>
public interface IContext {
    /// <summary>
    ///     Gets the unique identifier that represents a correlation ID for tracing or tracking purposes within the context.
    ///     This property is immutable and ensures consistent identification of a specific operation or event flow.
    /// </summary>
    Guid CorrelationId { get; }

    /// Gets the exact date and time at which the operation or context occurred.
    /// This property provides a timestamp that can be used for logging,
    /// debugging, or tracking purposes.
    DateTimeOffset OccuredAt { get; }
}
