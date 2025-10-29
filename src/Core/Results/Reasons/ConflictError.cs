namespace UnambitiousFx.Core.Results.Reasons;

public sealed record ConflictError(string                                Message,
                                   IReadOnlyDictionary<string, object?>? Extra = null)
    : ErrorBase("CONFLICT", Message, null, Merge(Extra, []));