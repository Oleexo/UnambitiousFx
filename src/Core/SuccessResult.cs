using System.Diagnostics.CodeAnalysis;

namespace Oleexo.UnambitiousFx.Core;

internal sealed class SuccessResult<TValue> : Result<TValue> where TValue : notnull
{
    private readonly TValue _value;

    public SuccessResult(TValue value)
    {
        _value = value;
    }

    public override bool IsFaulted => false;
    public override bool IsSuccess => true;

    public override void Match(Action<TValue> success, Action<Error> failure)
    {
        success(_value);
    }

    public override TOut Match<TOut>(Func<TValue, TOut> success, Func<Error, TOut> failure)
    {
        return success(_value);
    }

    public override void IfSuccess(Action<TValue> action)
    {
        action(_value);
    }

    public override ValueTask IfSuccess(Func<TValue, ValueTask> action)
    {
        return action(_value);
    }

    public override void IfFailure(Action<Error> action)
    {
    }

    public override ValueTask IfFailure(Func<Error, ValueTask> action)
    {
        return new ValueTask();
    }

    public override bool Ok([NotNullWhen(true)] out TValue? value, [NotNullWhen(false)] out Error? error)
    {
        value = _value;
        error = null;
        return true;
    }
}