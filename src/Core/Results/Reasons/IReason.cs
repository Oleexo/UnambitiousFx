namespace UnambitiousFx.Core.Results.Reasons;

/// <summary>
/// A reason attached to a Result. Can represent either a success enrichment or an error.
/// </summary>
public interface IReason {
    /// <summary>
    /// Human readable message describing the reason.
    /// </summary>
    string Message { get; }

    /// <summary>
    /// Arbitrary metadata bag for structured inspection / logging.
    /// </summary>
    IReadOnlyDictionary<string, object?> Metadata { get; }
}
