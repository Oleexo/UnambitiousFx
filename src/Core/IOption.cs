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