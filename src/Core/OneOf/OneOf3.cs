using System.Diagnostics.CodeAnalysis;

namespace UnambitiousFx.Core.OneOf;

/// <summary>
///     Represents a discriminated union of three possible value types.
/// </summary>
public abstract class OneOf<TFirst, TSecond, TThird>
    where TFirst : notnull
    where TSecond : notnull
    where TThird : notnull {
    public abstract bool IsFirst  { get; }
    public abstract bool IsSecond { get; }
    public abstract bool IsThird  { get; }

    public abstract TOut Match<TOut>(Func<TFirst, TOut>  firstFunc,
                                     Func<TSecond, TOut> secondFunc,
                                     Func<TThird, TOut>  thirdFunc);

    public abstract void Match(Action<TFirst>  firstAction,
                               Action<TSecond> secondAction,
                               Action<TThird>  thirdAction);

    public abstract bool First([NotNullWhen( true)] out TFirst?  first);
    public abstract bool Second([NotNullWhen(true)] out TSecond? second);
    public abstract bool Third([NotNullWhen( true)] out TThird?  third);

    // Factories (no implicit operators to avoid ambiguity when duplicate types are used)
    public static OneOf<TFirst, TSecond, TThird> FromFirst(TFirst value) {
        return new FirstOneOf<TFirst, TSecond, TThird>(value);
    }

    public static OneOf<TFirst, TSecond, TThird> FromSecond(TSecond value) {
        return new SecondOneOf<TFirst, TSecond, TThird>(value);
    }

    public static OneOf<TFirst, TSecond, TThird> FromThird(TThird value) {
        return new ThirdOneOf<TFirst, TSecond, TThird>(value);
    }
}

internal sealed class FirstOneOf<TFirst, TSecond, TThird> : OneOf<TFirst, TSecond, TThird>
    where TFirst : notnull
    where TSecond : notnull
    where TThird : notnull {
    private readonly TFirst _value;

    public FirstOneOf(TFirst value) {
        _value = value;
    }

    public override bool IsFirst  => true;
    public override bool IsSecond => false;
    public override bool IsThird  => false;

    public override TOut Match<TOut>(Func<TFirst, TOut>  firstFunc,
                                     Func<TSecond, TOut> secondFunc,
                                     Func<TThird, TOut>  thirdFunc) {
        return firstFunc(_value);
    }

    public override void Match(Action<TFirst>  firstAction,
                               Action<TSecond> secondAction,
                               Action<TThird>  thirdAction) {
        firstAction(_value);
    }

    public override bool First([NotNullWhen(true)] out TFirst? first) {
        first = _value;
        return true;
    }

    public override bool Second([NotNullWhen(true)] out TSecond? second) {
        second = default;
        return false;
    }

    public override bool Third([NotNullWhen(true)] out TThird? third) {
        third = default;
        return false;
    }
}

internal sealed class SecondOneOf<TFirst, TSecond, TThird> : OneOf<TFirst, TSecond, TThird>
    where TFirst : notnull
    where TSecond : notnull
    where TThird : notnull {
    private readonly TSecond _value;

    public SecondOneOf(TSecond value) {
        _value = value;
    }

    public override bool IsFirst  => false;
    public override bool IsSecond => true;
    public override bool IsThird  => false;

    public override TOut Match<TOut>(Func<TFirst, TOut>  firstFunc,
                                     Func<TSecond, TOut> secondFunc,
                                     Func<TThird, TOut>  thirdFunc) {
        return secondFunc(_value);
    }

    public override void Match(Action<TFirst>  firstAction,
                               Action<TSecond> secondAction,
                               Action<TThird>  thirdAction) {
        secondAction(_value);
    }

    public override bool First([NotNullWhen(true)] out TFirst? first) {
        first = default;
        return false;
    }

    public override bool Second([NotNullWhen(true)] out TSecond? second) {
        second = _value;
        return true;
    }

    public override bool Third([NotNullWhen(true)] out TThird? third) {
        third = default;
        return false;
    }
}

internal sealed class ThirdOneOf<TFirst, TSecond, TThird> : OneOf<TFirst, TSecond, TThird>
    where TFirst : notnull
    where TSecond : notnull
    where TThird : notnull {
    private readonly TThird _value;

    public ThirdOneOf(TThird value) {
        _value = value;
    }

    public override bool IsFirst  => false;
    public override bool IsSecond => false;
    public override bool IsThird  => true;

    public override TOut Match<TOut>(Func<TFirst, TOut>  firstFunc,
                                     Func<TSecond, TOut> secondFunc,
                                     Func<TThird, TOut>  thirdFunc) {
        return thirdFunc(_value);
    }

    public override void Match(Action<TFirst>  firstAction,
                               Action<TSecond> secondAction,
                               Action<TThird>  thirdAction) {
        thirdAction(_value);
    }

    public override bool First([NotNullWhen(true)] out TFirst? first) {
        first = default;
        return false;
    }

    public override bool Second([NotNullWhen(true)] out TSecond? second) {
        second = default;
        return false;
    }

    public override bool Third([NotNullWhen(true)] out TThird? third) {
        third = _value;
        return true;
    }
}
