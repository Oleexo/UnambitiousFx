using System.Diagnostics;
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

    internal string DebuggerDisplay => ToString() ?? "Unknown";
}
