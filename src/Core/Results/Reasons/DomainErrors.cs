// filepath: /Users/maxime.charlesn2f.com/dev/oleexo/UnambitiousFx/src/Core/Results/Reasons/DomainErrors.cs

namespace UnambitiousFx.Core.Results.Reasons;

public sealed record NotFoundError(string Resource, string Identifier, IReadOnlyDictionary<string, object?>? Extra = null)
    : ErrorBase("NOT_FOUND", $"Resource '{Resource}' with id '{Identifier}' was not found.", null,
        Merge(Extra, new [] { new KeyValuePair<string, object?>("resource", Resource), new("identifier", Identifier) }));

public sealed record ValidationError(IReadOnlyList<string> Failures, IReadOnlyDictionary<string, object?>? Extra = null)
    : ErrorBase("VALIDATION", Failures.Count == 0 ? "Validation failed." : string.Join("; ", Failures), null,
        Merge(Extra, new [] { new KeyValuePair<string, object?>("failures", Failures) }));

public sealed record ConflictError(string Message, IReadOnlyDictionary<string, object?>? Extra = null)
    : ErrorBase("CONFLICT", Message, null, Merge(Extra, Array.Empty<KeyValuePair<string, object?>>()));

public sealed record UnauthorizedError(string? Reason = null, IReadOnlyDictionary<string, object?>? Extra = null)
    : ErrorBase("UNAUTHORIZED", Reason ?? "Unauthorized.", null, Merge(Extra, Array.Empty<KeyValuePair<string, object?>>()));

public sealed record ExceptionalError(Exception Exception, string? MessageOverride = null, IReadOnlyDictionary<string, object?>? Extra = null)
    : ErrorBase("EXCEPTION", MessageOverride ?? Exception.Message, Exception,
        Merge(Extra, new [] { new KeyValuePair<string, object?>("exceptionType", Exception.GetType().FullName) }));

public sealed record SuccessReason(string Message, IReadOnlyDictionary<string, object?> Metadata) : ISuccess;
