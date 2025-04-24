using System.Diagnostics.CodeAnalysis;

namespace Oleexo.UnambitiousFx.Core;

public static class Option {
    public static Option<TValue> Some<TValue>(TValue value)
        where TValue : notnull {
        return new SomeOption<TValue>(value);
    }

    public static Option<TService> None<TService>()
        where TService : class {
        return Option<TService>.None;
    }
}

public abstract class Option<TValue> : IOption<TValue>
    where TValue : notnull {
    public static readonly Option<TValue> None = new NoneOption<TValue>();

    public abstract bool    IsSome { get; }
    public abstract bool    IsNone { get; }
    public abstract object? Case   { get; }
    public abstract void IfNone(Action                        none);
    public abstract ValueTask IfNone(Func<ValueTask>          none);
    public abstract void IfSome(Action<TValue>                some);
    public abstract ValueTask IfSome(Func<TValue, ValueTask>  some);
    public abstract bool Some([NotNullWhen(true)] out TValue? value);

    public abstract TOut Match<TOut>(Func<TValue, TOut> some,
                                     Func<TOut>         none);

    public abstract void Match(Action<TValue> some,
                               Action         none);

    public static Option<TValue> Some(TValue value) {
        return new SomeOption<TValue>(value);
    }
}
