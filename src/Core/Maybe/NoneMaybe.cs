using System.Diagnostics.CodeAnalysis;

namespace UnambitiousFx.Core.Maybe;

internal sealed class NoneMaybe<TValue> : Maybe<TValue>
    where TValue : notnull
{
    public override bool IsSome => false;
    public override bool IsNone => true;
    public override object? Case => null;

    public override void IfNone(Action none)
    {
        none();
    }

    public override ValueTask IfNone(Func<ValueTask> none)
    {
        return none();
    }

    public override void IfSome(Action<TValue> some)
    {
    }

    public override ValueTask IfSome(Func<TValue, ValueTask> some)
    {
        return new ValueTask();
    }

    public override bool Some([NotNullWhen(true)] out TValue? value)
    {
        value = default;
        return false;
    }

    public override TOut Match<TOut>(Func<TValue, TOut> some,
                                     Func<TOut> none)
    {
        return none();
    }

    public override void Match(Action<TValue> some,
                               Action none)
    {
        none();
    }
}
