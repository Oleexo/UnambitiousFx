using System.Diagnostics;
using System.Linq;
using UnambitiousFx.Core.Results.Reasons;
using System.Diagnostics.CodeAnalysis;
using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Results;

[DebuggerDisplay("{DebuggerDisplay,nq}")]
public abstract partial class BaseResult {
    private readonly List<IReason> _reasons = new();
    private readonly Dictionary<string, object?> _metadata = new(StringComparer.OrdinalIgnoreCase);

    public IReadOnlyList<IReason> Reasons => _reasons;
    public IReadOnlyDictionary<string, object?> Metadata => _metadata;

    internal void AddReason(IReason reason) {
        _reasons.Add(reason);
    }

    internal void AddReasons(IEnumerable<IReason> reasons) {
        _reasons.AddRange(reasons);
    }

    internal void AddMetadata(string key, object? value) {
        _metadata[key] = value;
    }

    public abstract bool IsFaulted { get; }
    public abstract bool IsSuccess { get; }

    public abstract void Match(Action            success,
                               Action<Exception> failure);

    public abstract TOut Match<TOut>(Func<TOut>            success,
                                     Func<Exception, TOut> failure);

    public abstract void IfSuccess(Action                       action);
    public abstract void IfFailure(Action<Exception>            action);
    public abstract bool Ok([NotNullWhen(false)] out Exception? error);

    private string DebuggerDisplay => BuildDebuggerDisplay();

    private string BuildDebuggerDisplay() {
        // Success summary: show status, reasons count, first 2 metadata keys.
        if (IsSuccess) {
            var meta = Metadata.Count == 0 ? string.Empty : " meta=" + string.Join(",", Metadata.Take(2).Select(kv => kv.Key + ":" + (kv.Value ?? "null")));
            return $"Success reasons={Reasons.Count}{meta}";
        }

        // Failure summary: prefer first IError reason for code/message; fallback to first Exception reason or generic marker.
        var firstError = Reasons.OfType<IError>().FirstOrDefault();
        string codePart = firstError is null ? string.Empty : " code=" + firstError.Code;
        string msg = firstError?.Message ?? "error";
        var metaPart = Metadata.Count == 0 ? string.Empty : " meta=" + string.Join(",", Metadata.Take(2).Select(kv => kv.Key + ":" + (kv.Value ?? "null")));
        return $"Failure({msg}){codePart} reasons={Reasons.Count}{metaPart}";
    }
}
