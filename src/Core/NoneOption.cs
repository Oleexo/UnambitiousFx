using System.Diagnostics.CodeAnalysis;

namespace UnambitiousFx.Core;

internal sealed class NoneOption<TValue> : Option<TValue>
    where TValue : notnull {
    public override bool    IsSome => false;
    public override bool    IsNone => true;
    public override object? Case   => null;

    public override void IfNone(Action none) {
        none();
    }

    public override ValueTask IfNone(Func<ValueTask> none) {
        return none();
    }

    public override void IfSome(Action<TValue> some) {
    }

    public override ValueTask IfSome(Func<TValue, ValueTask> some) {
        return new ValueTask();
    }

    public override bool Some([NotNullWhen(true)] out TValue? value) {
        value = default;
        return false;
    }

    public override TOut Match<TOut>(Func<TValue, TOut> some,
                                     Func<TOut>         none) {
        return none();
    }

    public override void Match(Action<TValue> some,
                               Action         none) {
        none();
    }

    public override Option<TOut> Bind<TOut>(Func<TValue, Option<TOut>> someFunc) {
        return new NoneOption<TOut>();
    }

    public override ValueTask<Option<TOut>> Bind<TOut>(Func<TValue, ValueTask<Option<TOut>>> someFunc) {
        return new ValueTask<Option<TOut>>();
    }
}
