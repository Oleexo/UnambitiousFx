using System.Diagnostics.CodeAnalysis;

namespace Oleexo.UnambitiousFx.Core;

public interface IOption
{
    bool IsSome { get; }
    bool IsNone { get; }
    object? Case { get; }
    void IfNone(Action none);
    ValueTask IfNone(Func<ValueTask> none);
}

public interface IOption<TValue> : IOption
{
    void IfSome(Action<TValue> some);
    ValueTask IfSome(Func<TValue, ValueTask> some);
    bool Some([NotNullWhen(true)] out TValue? value);
}

public abstract class Option<TValue> : IOption<TValue> where TValue : notnull
{
    public abstract bool IsSome { get; }
    public abstract bool IsNone { get; }
    public abstract object? Case { get; }
    public abstract void IfNone(Action none);
    public abstract ValueTask IfNone(Func<ValueTask> none);
    public abstract void IfSome(Action<TValue> some);
    public abstract ValueTask IfSome(Func<TValue, ValueTask> some);
    public abstract bool Some([NotNullWhen(true)] out TValue? value);
}