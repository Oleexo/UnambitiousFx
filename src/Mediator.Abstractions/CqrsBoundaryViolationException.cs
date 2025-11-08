namespace UnambitiousFx.Mediator.Abstractions;

/// <summary>
///     Exception thrown when a CQRS boundary violation is detected.
/// </summary>
public sealed class CqrsBoundaryViolationException : InvalidOperationException
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="CqrsBoundaryViolationException" /> class.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public CqrsBoundaryViolationException(string message)
        : base(message)
    {
    }
}
