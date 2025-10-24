namespace UnambitiousFx.Core.Results.Reasons;

/// <summary>
///     Represents an error reason attached to a Result.
/// </summary>
public interface IError : IReason {
    /// <summary>
    ///     A stable machine readable code (e.g. VALIDATION, NOT_FOUND, etc.).
    /// </summary>
    string Code { get; }

    /// <summary>
    ///     Underlying exception if this error originates from one, otherwise null.
    /// </summary>
    Exception? Exception { get; }
}
