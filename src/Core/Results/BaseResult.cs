using System.Diagnostics.CodeAnalysis;
using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Results;

public abstract class BaseResult : IResult {
    private readonly Dictionary<string, object?> _metadata = new(StringComparer.OrdinalIgnoreCase);
    private readonly List<IReason>               _reasons  = new();

    public IReadOnlyCollection<IReason>         Reasons  => _reasons;
    public IReadOnlyDictionary<string, object?> Metadata => _metadata;

    public IReadOnlyCollection<IError> Errors => Reasons.OfType<IError>()
                                                        .ToList();

    public IReadOnlyCollection<ISuccess> Successes => Reasons.OfType<ISuccess>()
                                                             .ToList();

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
    public abstract void IfSuccess(Action                      action);
    public abstract void IfFailure(Action<IEnumerable<IError>> action);
}
