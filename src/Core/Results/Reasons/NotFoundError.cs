namespace UnambitiousFx.Core.Results.Reasons;

public sealed record NotFoundError(string                                Resource,
                                   string                                Identifier,
                                   IReadOnlyDictionary<string, object?>? Extra = null)
    : ErrorBase("NOT_FOUND", $"Resource '{Resource}' with id '{Identifier}' was not found.", null,
                Merge(Extra, [new KeyValuePair<string, object?>("resource", Resource), new KeyValuePair<string, object?>("identifier", Identifier)]));
