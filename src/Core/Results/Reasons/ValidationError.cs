namespace UnambitiousFx.Core.Results.Reasons;

public sealed record ValidationError(IReadOnlyList<string>                 Failures,
                                     IReadOnlyDictionary<string, object?>? Extra = null)
    : ErrorBase("VALIDATION", Failures.Count == 0
                                  ? "Validation failed."
                                  : string.Join("; ", Failures), null,
                Merge(Extra, [new KeyValuePair<string, object?>("failures", Failures)]));
