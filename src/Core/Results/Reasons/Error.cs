// filepath: /Users/maxime.charlesn2f.com/dev/oleexo/UnambitiousFx/src/Core/Results/Reasons/Error.cs
namespace UnambitiousFx.Core.Results.Reasons;

/// <summary>
///     Default concrete error implementation used in tests and simple failures.
/// </summary>
public sealed record Error : ErrorBase
{
    /// <summary>
    ///     Creates an error with default code "ERROR" and provided message.
    /// </summary>
    public Error(string message) : base("ERROR", message) { }

    /// <summary>
    ///     Creates an error with explicit code and message.
    /// </summary>
    public Error(string code, string message, Exception? exception = null, IReadOnlyDictionary<string, object?>? metadata = null)
        : base(code, message, exception, metadata) { }
}

