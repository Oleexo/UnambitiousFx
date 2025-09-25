using System.Diagnostics.CodeAnalysis;

namespace UnambitiousFx.Core.Results;

public abstract partial class BaseResult {
    public abstract bool IsFaulted { get; }
    public abstract bool IsSuccess { get; }

    public abstract void Match(Action            success,
                               Action<Exception> failure);

    public abstract TOut Match<TOut>(Func<TOut>            success,
                                     Func<Exception, TOut> failure);

    public abstract void IfSuccess(Action                       action);
    public abstract void IfFailure(Action<Exception>            action);
    public abstract bool Ok([NotNullWhen(false)] out Exception? error);
}
