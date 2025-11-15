namespace UnambitiousFx.Core.Results.Reasons;

public sealed record SuccessReason(string Message,
                                   IReadOnlyDictionary<string, object?> Metadata) : ISuccess;
