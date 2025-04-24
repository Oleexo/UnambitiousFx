using System.Diagnostics.CodeAnalysis;

namespace Oleexo.UnambitiousFx.Core;

internal sealed class SomeOption<TValue> : Option<TValue>
    where TValue : notnull {
    private readonly TValue _value;

    public SomeOption(TValue value) {
        _value = value;
    }

    public override bool   IsSome => true;
    public override bool   IsNone => false;
    public override object Case   => _value;

    public override void IfNone(Action none) {
    }

    public override ValueTask IfNone(Func<ValueTask> none) {
        return new ValueTask();
    }

    public override void IfSome(Action<TValue> some) {
        some(_value);
    }

    public override ValueTask IfSome(Func<TValue, ValueTask> some) {
        return some(_value);
    }

    public override bool Some([NotNullWhen(true)] out TValue? value) {
        value = _value;
        return true;
    }

    public override TOut Match<TOut>(Func<TValue, TOut> some,
                                     Func<TOut>         none) {
        return some(_value);
    }

    public override void Match(Action<TValue> some,
                               Action         none) {
        some(_value);
    }
}
