using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Results;

[DebuggerDisplay("{DebuggerDisplay,nq}")]
public abstract class BaseResult : IResult {
    private readonly Dictionary<string, object?> _metadata = new(StringComparer.OrdinalIgnoreCase);
    private readonly List<IReason>               _reasons  = [];

    public IReadOnlyCollection<IReason> Reasons => _reasons.AsReadOnly();

    public IReadOnlyCollection<ISuccess> Successes => _reasons.OfType<ISuccess>()
                                                              .ToArray()
                                                              .AsReadOnly();

    public IReadOnlyCollection<IError> Errors => _reasons.OfType<IError>()
                                                         .ToArray()
                                                         .AsReadOnly();

    public IReadOnlyDictionary<string, object?> Metadata => _metadata.AsReadOnly();

    private string DebuggerDisplay => BuildDebuggerDisplay();

    public abstract bool IsFaulted { get; }
    public abstract bool IsSuccess { get; }

    internal void AddReason(IReason reason) {
        _reasons.Add(reason);
    }

    internal void AddReasons(IEnumerable<IReason> reasons) {
        _reasons.AddRange(reasons);
    }

    internal void AddMetadata(string  key,
                              object? value) {
        _metadata[key] = value;
    }

    internal void AddMetadata(IReadOnlyDictionary<string, object?> metadata) {
        foreach (var kv in metadata) {
            _metadata[kv.Key] = kv.Value;
        }
    }

    public abstract void Match(Action                      success,
                               Action<IEnumerable<IError>> failure);

    public abstract TOut Match<TOut>(Func<TOut>                      success,
                                     Func<IEnumerable<IError>, TOut> failure);

    public abstract bool TryGet([NotNullWhen(false)] out IEnumerable<IError>? errors);
    public abstract void IfSuccess(Action                                    action);
    public abstract void IfFailure(Action<IEnumerable<IError>>               action);

    private string BuildDebuggerDisplay() {
        if (IsSuccess) {
            var meta = Metadata.Count == 0
                           ? string.Empty
                           : " meta=" + string.Join(",", Metadata.Select(kv => kv.Key + ":" + (kv.Value ?? "null")));
            return $"Success reasons={Reasons.Count}{meta}";
        }

        // Prefer a non-ExceptionalError domain error if one exists; otherwise fallback to the first error (which may be the automatic ExceptionalError)
        var firstNonExceptional = Reasons.OfType<IError>()
                                         .FirstOrDefault(r => r is not ExceptionalError);
        var firstErrorAny = Reasons.OfType<IError>()
                                   .FirstOrDefault();
        var chosen      = firstNonExceptional ?? firstErrorAny;
        var msg         = chosen?.Message     ?? "error";
        var includeCode = chosen is not null && chosen is not ExceptionalError; // suppress code display for the primary ExceptionalError only
        var codePart = includeCode
                           ? " code=" + chosen!.Code
                           : string.Empty;
        var metaPart = Metadata.Count == 0
                           ? string.Empty
                           : " meta=" + string.Join(",", Metadata.Select(kv => kv.Key + ":" + (kv.Value ?? "null")));
        return $"Failure({msg}){codePart} reasons={Reasons.Count}{metaPart}";
    }
}
