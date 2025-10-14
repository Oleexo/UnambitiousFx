namespace UnambitiousFx.Core.Results.Reasons;

public sealed record UnauthorizedError(string?                               Reason = null,
                                       IReadOnlyDictionary<string, object?>? Extra  = null)
    : ErrorBase("UNAUTHORIZED", Reason ?? "Unauthorized.", null, Merge(Extra, []));
