namespace UnambitiousFx.Core.Results.Reasons;

public sealed record ExceptionalError(Exception Exception,
                                      string? MessageOverride = null,
                                      IReadOnlyDictionary<string, object?>? Extra = null)
    : ErrorBase("EXCEPTION", MessageOverride ?? Exception.Message, Exception,
                Merge(Extra, [
                    new KeyValuePair<string, object?>("exceptionType", Exception.GetType()
                                                                                .FullName)
                ]));
