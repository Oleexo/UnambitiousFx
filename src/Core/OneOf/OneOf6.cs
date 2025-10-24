using System.Diagnostics.CodeAnalysis;

namespace UnambitiousFx.Core.OneOf;

/// <summary>
///     Represents a discriminated union of six possible value types.
/// </summary>
public abstract class OneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>
    where TFirst : notnull
    where TSecond : notnull
    where TThird : notnull
    where TFourth : notnull
    where TFifth : notnull
    where TSixth : notnull {
    public abstract bool IsFirst  { get; }
    public abstract bool IsSecond { get; }
    public abstract bool IsThird  { get; }
    public abstract bool IsFourth { get; }
    public abstract bool IsFifth  { get; }
    public abstract bool IsSixth  { get; }

    public abstract TOut Match<TOut>(Func<TFirst, TOut>  firstFunc,
                                     Func<TSecond, TOut> secondFunc,
                                     Func<TThird, TOut>  thirdFunc,
                                     Func<TFourth, TOut> fourthFunc,
                                     Func<TFifth, TOut>  fifthFunc,
                                     Func<TSixth, TOut>  sixthFunc);

    public abstract void Match(Action<TFirst>  firstAction,
                               Action<TSecond> secondAction,
                               Action<TThird>  thirdAction,
                               Action<TFourth> fourthAction,
                               Action<TFifth>  fifthAction,
                               Action<TSixth>  sixthAction);

    public abstract bool First([NotNullWhen( true)] out TFirst?  first);
    public abstract bool Second([NotNullWhen(true)] out TSecond? second);
    public abstract bool Third([NotNullWhen( true)] out TThird?  third);
    public abstract bool Fourth([NotNullWhen(true)] out TFourth? fourth);
    public abstract bool Fifth([NotNullWhen( true)] out TFifth?  fifth);
    public abstract bool Sixth([NotNullWhen( true)] out TSixth?  sixth);

    // Factories
    public static OneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth> FromFirst(TFirst value) {
        return new FirstOneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>(value);
    }

    public static OneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth> FromSecond(TSecond value) {
        return new SecondOneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>(value);
    }

    public static OneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth> FromThird(TThird value) {
        return new ThirdOneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>(value);
    }

    public static OneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth> FromFourth(TFourth value) {
        return new FourthOneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>(value);
    }

    public static OneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth> FromFifth(TFifth value) {
        return new FifthOneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>(value);
    }

    public static OneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth> FromSixth(TSixth value) {
        return new SixthOneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>(value);
    }
}

internal sealed class FirstOneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth> : OneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>
    where TFirst : notnull
    where TSecond : notnull
    where TThird : notnull
    where TFourth : notnull
    where TFifth : notnull
    where TSixth : notnull {
    private readonly TFirst _value;

    public FirstOneOf(TFirst value) {
        _value = value;
    }

    public override bool IsFirst  => true;
    public override bool IsSecond => false;
    public override bool IsThird  => false;
    public override bool IsFourth => false;
    public override bool IsFifth  => false;
    public override bool IsSixth  => false;

    public override TOut Match<TOut>(Func<TFirst, TOut>  firstFunc,
                                     Func<TSecond, TOut> secondFunc,
                                     Func<TThird, TOut>  thirdFunc,
                                     Func<TFourth, TOut> fourthFunc,
                                     Func<TFifth, TOut>  fifthFunc,
                                     Func<TSixth, TOut>  sixthFunc) {
        return firstFunc(_value);
    }

    public override void Match(Action<TFirst>  firstAction,
                               Action<TSecond> secondAction,
                               Action<TThird>  thirdAction,
                               Action<TFourth> fourthAction,
                               Action<TFifth>  fifthAction,
                               Action<TSixth>  sixthAction) {
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

    public override bool Fourth([NotNullWhen(true)] out TFourth? fourth) {
        fourth = default;
        return false;
    }

    public override bool Fifth([NotNullWhen(true)] out TFifth? fifth) {
        fifth = default;
        return false;
    }

    public override bool Sixth([NotNullWhen(true)] out TSixth? sixth) {
        sixth = default;
        return false;
    }
}

internal sealed class SecondOneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth> : OneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>
    where TFirst : notnull
    where TSecond : notnull
    where TThird : notnull
    where TFourth : notnull
    where TFifth : notnull
    where TSixth : notnull {
    private readonly TSecond _value;

    public SecondOneOf(TSecond value) {
        _value = value;
    }

    public override bool IsFirst  => false;
    public override bool IsSecond => true;
    public override bool IsThird  => false;
    public override bool IsFourth => false;
    public override bool IsFifth  => false;
    public override bool IsSixth  => false;

    public override TOut Match<TOut>(Func<TFirst, TOut>  firstFunc,
                                     Func<TSecond, TOut> secondFunc,
                                     Func<TThird, TOut>  thirdFunc,
                                     Func<TFourth, TOut> fourthFunc,
                                     Func<TFifth, TOut>  fifthFunc,
                                     Func<TSixth, TOut>  sixthFunc) {
        return secondFunc(_value);
    }

    public override void Match(Action<TFirst>  firstAction,
                               Action<TSecond> secondAction,
                               Action<TThird>  thirdAction,
                               Action<TFourth> fourthAction,
                               Action<TFifth>  fifthAction,
                               Action<TSixth>  sixthAction) {
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

    public override bool Fourth([NotNullWhen(true)] out TFourth? fourth) {
        fourth = default;
        return false;
    }

    public override bool Fifth([NotNullWhen(true)] out TFifth? fifth) {
        fifth = default;
        return false;
    }

    public override bool Sixth([NotNullWhen(true)] out TSixth? sixth) {
        sixth = default;
        return false;
    }
}

internal sealed class ThirdOneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth> : OneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>
    where TFirst : notnull
    where TSecond : notnull
    where TThird : notnull
    where TFourth : notnull
    where TFifth : notnull
    where TSixth : notnull {
    private readonly TThird _value;

    public ThirdOneOf(TThird value) {
        _value = value;
    }

    public override bool IsFirst  => false;
    public override bool IsSecond => false;
    public override bool IsThird  => true;
    public override bool IsFourth => false;
    public override bool IsFifth  => false;
    public override bool IsSixth  => false;

    public override TOut Match<TOut>(Func<TFirst, TOut>  firstFunc,
                                     Func<TSecond, TOut> secondFunc,
                                     Func<TThird, TOut>  thirdFunc,
                                     Func<TFourth, TOut> fourthFunc,
                                     Func<TFifth, TOut>  fifthFunc,
                                     Func<TSixth, TOut>  sixthFunc) {
        return thirdFunc(_value);
    }

    public override void Match(Action<TFirst>  firstAction,
                               Action<TSecond> secondAction,
                               Action<TThird>  thirdAction,
                               Action<TFourth> fourthAction,
                               Action<TFifth>  fifthAction,
                               Action<TSixth>  sixthAction) {
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

    public override bool Fourth([NotNullWhen(true)] out TFourth? fourth) {
        fourth = default;
        return false;
    }

    public override bool Fifth([NotNullWhen(true)] out TFifth? fifth) {
        fifth = default;
        return false;
    }

    public override bool Sixth([NotNullWhen(true)] out TSixth? sixth) {
        sixth = default;
        return false;
    }
}

internal sealed class FourthOneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth> : OneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>
    where TFirst : notnull
    where TSecond : notnull
    where TThird : notnull
    where TFourth : notnull
    where TFifth : notnull
    where TSixth : notnull {
    private readonly TFourth _value;

    public FourthOneOf(TFourth value) {
        _value = value;
    }

    public override bool IsFirst  => false;
    public override bool IsSecond => false;
    public override bool IsThird  => false;
    public override bool IsFourth => true;
    public override bool IsFifth  => false;
    public override bool IsSixth  => false;

    public override TOut Match<TOut>(Func<TFirst, TOut>  firstFunc,
                                     Func<TSecond, TOut> secondFunc,
                                     Func<TThird, TOut>  thirdFunc,
                                     Func<TFourth, TOut> fourthFunc,
                                     Func<TFifth, TOut>  fifthFunc,
                                     Func<TSixth, TOut>  sixthFunc) {
        return fourthFunc(_value);
    }

    public override void Match(Action<TFirst>  firstAction,
                               Action<TSecond> secondAction,
                               Action<TThird>  thirdAction,
                               Action<TFourth> fourthAction,
                               Action<TFifth>  fifthAction,
                               Action<TSixth>  sixthAction) {
        fourthAction(_value);
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
        third = default;
        return false;
    }

    public override bool Fourth([NotNullWhen(true)] out TFourth? fourth) {
        fourth = _value;
        return true;
    }

    public override bool Fifth([NotNullWhen(true)] out TFifth? fifth) {
        fifth = default;
        return false;
    }

    public override bool Sixth([NotNullWhen(true)] out TSixth? sixth) {
        sixth = default;
        return false;
    }
}

internal sealed class FifthOneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth> : OneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>
    where TFirst : notnull
    where TSecond : notnull
    where TThird : notnull
    where TFourth : notnull
    where TFifth : notnull
    where TSixth : notnull {
    private readonly TFifth _value;

    public FifthOneOf(TFifth value) {
        _value = value;
    }

    public override bool IsFirst  => false;
    public override bool IsSecond => false;
    public override bool IsThird  => false;
    public override bool IsFourth => false;
    public override bool IsFifth  => true;
    public override bool IsSixth  => false;

    public override TOut Match<TOut>(Func<TFirst, TOut>  firstFunc,
                                     Func<TSecond, TOut> secondFunc,
                                     Func<TThird, TOut>  thirdFunc,
                                     Func<TFourth, TOut> fourthFunc,
                                     Func<TFifth, TOut>  fifthFunc,
                                     Func<TSixth, TOut>  sixthFunc) {
        return fifthFunc(_value);
    }

    public override void Match(Action<TFirst>  firstAction,
                               Action<TSecond> secondAction,
                               Action<TThird>  thirdAction,
                               Action<TFourth> fourthAction,
                               Action<TFifth>  fifthAction,
                               Action<TSixth>  sixthAction) {
        fifthAction(_value);
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
        third = default;
        return false;
    }

    public override bool Fourth([NotNullWhen(true)] out TFourth? fourth) {
        fourth = default;
        return false;
    }

    public override bool Fifth([NotNullWhen(true)] out TFifth? fifth) {
        fifth = _value;
        return true;
    }

    public override bool Sixth([NotNullWhen(true)] out TSixth? sixth) {
        sixth = default;
        return false;
    }
}

internal sealed class SixthOneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth> : OneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>
    where TFirst : notnull
    where TSecond : notnull
    where TThird : notnull
    where TFourth : notnull
    where TFifth : notnull
    where TSixth : notnull {
    private readonly TSixth _value;

    public SixthOneOf(TSixth value) {
        _value = value;
    }

    public override bool IsFirst  => false;
    public override bool IsSecond => false;
    public override bool IsThird  => false;
    public override bool IsFourth => false;
    public override bool IsFifth  => false;
    public override bool IsSixth  => true;

    public override TOut Match<TOut>(Func<TFirst, TOut>  firstFunc,
                                     Func<TSecond, TOut> secondFunc,
                                     Func<TThird, TOut>  thirdFunc,
                                     Func<TFourth, TOut> fourthFunc,
                                     Func<TFifth, TOut>  fifthFunc,
                                     Func<TSixth, TOut>  sixthFunc) {
        return sixthFunc(_value);
    }

    public override void Match(Action<TFirst>  firstAction,
                               Action<TSecond> secondAction,
                               Action<TThird>  thirdAction,
                               Action<TFourth> fourthAction,
                               Action<TFifth>  fifthAction,
                               Action<TSixth>  sixthAction) {
        sixthAction(_value);
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
        third = default;
        return false;
    }

    public override bool Fourth([NotNullWhen(true)] out TFourth? fourth) {
        fourth = default;
        return false;
    }

    public override bool Fifth([NotNullWhen(true)] out TFifth? fifth) {
        fifth = default;
        return false;
    }

    public override bool Sixth([NotNullWhen(true)] out TSixth? sixth) {
        sixth = _value;
        return true;
    }
}
