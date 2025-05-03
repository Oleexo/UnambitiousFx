namespace UnambitiousFx.Mediator;

/// <summary>
///     Represents an exception that is thrown when no handler is found for a specified request type.
/// </summary>
public sealed class MissingHandlerException : Exception {
    /// <summary>
    ///     Represents an exception that is thrown when a handler for a specific request type is not found.
    /// </summary>
    public MissingHandlerException(Type requestType)
        : base($"Missing handler for {requestType.Name}") {
    }
}
