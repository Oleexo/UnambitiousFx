using System.Diagnostics.CodeAnalysis;

namespace Oleexo.UnambitiousFx.Core;

internal sealed class SuccessResult<TValue> : Result<TValue>
    where TValue : notnull {
    private readonly TValue _value;

    public SuccessResult(TValue value) {
        _value = value;
    }

    public override bool IsFaulted => false;
    public override bool IsSuccess => true;

    public override void Match(Action<TValue> success,
                               Action<IError> failure) {
        success(_value);
    }

    public override TOut Match<TOut>(Func<TValue, TOut> success,
                                     Func<IError, TOut> failure) {
        return success(_value);
    }

    public override void IfSuccess(Action<TValue> action) {
        action(_value);
    }

    public override ValueTask IfSuccess(Func<TValue, ValueTask> action) {
        return action(_value);
    }

    public override void IfFailure(Action<IError> action) {
    }

    public override ValueTask IfFailure(Func<IError, ValueTask> action) {
        return new ValueTask();
    }

    public override bool Ok([NotNullWhen(true)] out  TValue? value,
                            [NotNullWhen(false)] out IError? error) {
        value = _value;
        error = null;
        return true;
    }
}
