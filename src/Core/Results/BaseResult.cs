using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Results;

[DebuggerDisplay("{DebuggerDisplay,nq}")]
public abstract class BaseResult : IResult {
    private readonly Dictionary<string, object?> _metadata = new(StringComparer.OrdinalIgnoreCase);
    private readonly List<IReason>               _reasons  = new();

    public IReadOnlyList<IReason>               Reasons  => _reasons;
    public IReadOnlyDictionary<string, object?> Metadata => _metadata;

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

    public abstract void Match(Action            success,
                               Action<Exception> failure);

    public abstract TOut Match<TOut>(Func<TOut>            success,
                                     Func<Exception, TOut> failure);

    public abstract void IfSuccess(Action                       action);
    public abstract void IfFailure(Action<Exception>            action);
    public abstract bool Ok([NotNullWhen(false)] out Exception? error);

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
