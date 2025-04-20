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

    public override bool Ok([NotNullWhen(true)] out TValue? value, [NotNullWhen(false)] out Error? error)
    {
        value = _value;
        error = null;
        return true;
    }
}