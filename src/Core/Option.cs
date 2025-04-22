using System.Diagnostics.CodeAnalysis;
using Oleexo.UnambitiousFx.Core.Abstractions;

namespace Oleexo.UnambitiousFx.Core;

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

    public static Option<TValue> Some(TValue value) {
        return new SomeOption<TValue>(value);
    }
}
