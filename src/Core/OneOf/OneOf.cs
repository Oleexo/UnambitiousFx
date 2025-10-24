using System.Diagnostics.CodeAnalysis;

namespace UnambitiousFx.Core.OneOf;

/// <summary>
///     Minimal discriminated union base abstraction representing a value that can be one of two types.
///     Specific semantic unions (e.g. Either&lt;TLeft,TRight&gt;) can inherit from this to express intent
///     without duplicating core shape.
/// </summary>
/// <typeparam name="TFirst">First possible contained type.</typeparam>
/// <typeparam name="TSecond">Second possible contained type.</typeparam>
public abstract class OneOf<TFirst, TSecond>
    where TFirst : notnull
    where TSecond : notnull {
    /// <summary>True when the instance currently holds a value of the first type.</summary>
    public abstract bool IsFirst { get; }

    /// <summary>True when the instance currently holds a value of the second type.</summary>
    public abstract bool IsSecond { get; }

    /// <summary>Pattern match returning a value.</summary>
    public abstract TOut Match<TOut>(Func<TFirst, TOut>  firstFunc,
                                     Func<TSecond, TOut> secondFunc);

    /// <summary>Pattern match executing side-effect actions.</summary>
    public abstract void Match(Action<TFirst>  firstAction,
                               Action<TSecond> secondAction);

    /// <summary>Attempts to extract the first value.</summary>
    public abstract bool First([NotNullWhen(true)] out TFirst? first);

    /// <summary>Attempts to extract the second value.</summary>
    public abstract bool Second([NotNullWhen(true)] out TSecond? second);

    // Factories ------------------------------------------------------------
    public static OneOf<TFirst, TSecond> FromFirst(TFirst value) {
        return new FirstOneOf<TFirst, TSecond>(value);
    }

    public static OneOf<TFirst, TSecond> FromSecond(TSecond value) {
        return new SecondOneOf<TFirst, TSecond>(value);
    }

    /// <summary>Implicit conversion from first type to OneOf.</summary>
    public static implicit operator OneOf<TFirst, TSecond>(TFirst value) {
        return FromFirst(value);
    }

    /// <summary>Implicit conversion from second type to OneOf.</summary>
    public static implicit operator OneOf<TFirst, TSecond>(TSecond value) {
        return FromSecond(value);
    }
}

internal sealed class FirstOneOf<TFirst, TSecond> : OneOf<TFirst, TSecond>
    where TFirst : notnull
    where TSecond : notnull {
    private readonly TFirst _first;

    public FirstOneOf(TFirst first) {
        _first = first;
    }

    public override bool IsFirst  => true;
    public override bool IsSecond => false;

    public override TOut Match<TOut>(Func<TFirst, TOut>  firstFunc,
                                     Func<TSecond, TOut> secondFunc) {
        return firstFunc(_first);
    }

    public override void Match(Action<TFirst>  firstAction,
                               Action<TSecond> secondAction) {
        firstAction(_first);
    }

    public override bool First([NotNullWhen(true)] out TFirst? first) {
        first = _first;
        return true;
    }

    public override bool Second([NotNullWhen(true)] out TSecond? second) {
        second = default;
        return false;
    }
}

internal sealed class SecondOneOf<TFirst, TSecond> : OneOf<TFirst, TSecond>
    where TFirst : notnull
    where TSecond : notnull {
    private readonly TSecond _second;

    public SecondOneOf(TSecond second) {
        _second = second;
    }

    public override bool IsFirst  => false;
    public override bool IsSecond => true;

    public override TOut Match<TOut>(Func<TFirst, TOut>  firstFunc,
                                     Func<TSecond, TOut> secondFunc) {
        return secondFunc(_second);
    }

    public override void Match(Action<TFirst>  firstAction,
                               Action<TSecond> secondAction) {
        secondAction(_second);
    }

    public override bool First([NotNullWhen(true)] out TFirst? first) {
        first = default;
        return false;
    }

    public override bool Second([NotNullWhen(true)] out TSecond? second) {
        second = _second;
        return true;
    }
}
