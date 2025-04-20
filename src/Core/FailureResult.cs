using System.Diagnostics.CodeAnalysis;

namespace Oleexo.UnambitiousFx.Core;

internal sealed class FailureResult<TValue> : Result<TValue> where TValue : notnull
{
    private readonly Error _error;

    public FailureResult(Error error)
    {
        _error = error;
    }

    public override bool IsFaulted => true;
    public override bool IsSuccess => false;

    public override void Match(Action<TValue> success, Action<Error> failure)
    {
        failure(_error);
    }

    public override TOut Match<TOut>(Func<TValue, TOut> success, Func<Error, TOut> failure)
    {
        return failure(_error);
    }

    public override bool Ok([NotNullWhen(true)] out TValue? value, [NotNullWhen(false)] out Error? error)
    {
        value = default;
        error = _error;
        return false;
    }
}