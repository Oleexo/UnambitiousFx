using System.Diagnostics.CodeAnalysis;

namespace UnambitiousFx.Core.OneOf;

/// <summary>
///     Represents a discriminated union of five possible value types.
/// </summary>
public abstract class OneOf<TFirst, TSecond, TThird, TFourth, TFifth>
    where TFirst : notnull
    where TSecond : notnull
    where TThird : notnull
    where TFourth : notnull
    where TFifth : notnull {
    public abstract bool IsFirst  { get; }
    public abstract bool IsSecond { get; }
    public abstract bool IsThird  { get; }
    public abstract bool IsFourth { get; }
    public abstract bool IsFifth  { get; }

    public abstract TOut Match<TOut>(Func<TFirst, TOut>  firstFunc,
                                     Func<TSecond, TOut> secondFunc,
                                     Func<TThird, TOut>  thirdFunc,
                                     Func<TFourth, TOut> fourthFunc,
                                     Func<TFifth, TOut>  fifthFunc);

    public abstract void Match(Action<TFirst>  firstAction,
                               Action<TSecond> secondAction,
                               Action<TThird>  thirdAction,
                               Action<TFourth> fourthAction,
                               Action<TFifth>  fifthAction);

    public abstract bool First([NotNullWhen( true)] out TFirst?  first);
    public abstract bool Second([NotNullWhen(true)] out TSecond? second);
    public abstract bool Third([NotNullWhen( true)] out TThird?  third);
    public abstract bool Fourth([NotNullWhen(true)] out TFourth? fourth);
    public abstract bool Fifth([NotNullWhen( true)] out TFifth?  fifth);

    // Factories
    public static OneOf<TFirst, TSecond, TThird, TFourth, TFifth> FromFirst(TFirst value) {
        return new FirstOneOf<TFirst, TSecond, TThird, TFourth, TFifth>(value);
    }

    public static OneOf<TFirst, TSecond, TThird, TFourth, TFifth> FromSecond(TSecond value) {
        return new SecondOneOf<TFirst, TSecond, TThird, TFourth, TFifth>(value);
    }

    public static OneOf<TFirst, TSecond, TThird, TFourth, TFifth> FromThird(TThird value) {
        return new ThirdOneOf<TFirst, TSecond, TThird, TFourth, TFifth>(value);
    }

    public static OneOf<TFirst, TSecond, TThird, TFourth, TFifth> FromFourth(TFourth value) {
        return new FourthOneOf<TFirst, TSecond, TThird, TFourth, TFifth>(value);
    }

    public static OneOf<TFirst, TSecond, TThird, TFourth, TFifth> FromFifth(TFifth value) {
        return new FifthOneOf<TFirst, TSecond, TThird, TFourth, TFifth>(value);
    }
}

internal sealed class FirstOneOf<TFirst, TSecond, TThird, TFourth, TFifth> : OneOf<TFirst, TSecond, TThird, TFourth, TFifth>
    where TFirst : notnull
    where TSecond : notnull
    where TThird : notnull
    where TFourth : notnull
    where TFifth : notnull {
    private readonly TFirst _value;

    public FirstOneOf(TFirst value) {
        _value = value;
    }

    public override bool IsFirst  => true;
    public override bool IsSecond => false;
    public override bool IsThird  => false;
    public override bool IsFourth => false;
    public override bool IsFifth  => false;

    public override TOut Match<TOut>(Func<TFirst, TOut>  firstFunc,
                                     Func<TSecond, TOut> secondFunc,
                                     Func<TThird, TOut>  thirdFunc,
                                     Func<TFourth, TOut> fourthFunc,
                                     Func<TFifth, TOut>  fifthFunc) {
        return firstFunc(_value);
    }

    public override void Match(Action<TFirst>  firstAction,
                               Action<TSecond> secondAction,
                               Action<TThird>  thirdAction,
                               Action<TFourth> fourthAction,
                               Action<TFifth>  fifthAction) {
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
}

internal sealed class SecondOneOf<TFirst, TSecond, TThird, TFourth, TFifth> : OneOf<TFirst, TSecond, TThird, TFourth, TFifth>
    where TFirst : notnull
    where TSecond : notnull
    where TThird : notnull
    where TFourth : notnull
    where TFifth : notnull {
    private readonly TSecond _value;

    public SecondOneOf(TSecond value) {
        _value = value;
    }

    public override bool IsFirst  => false;
    public override bool IsSecond => true;
    public override bool IsThird  => false;
    public override bool IsFourth => false;
    public override bool IsFifth  => false;

    public override TOut Match<TOut>(Func<TFirst, TOut>  firstFunc,
                                     Func<TSecond, TOut> secondFunc,
                                     Func<TThird, TOut>  thirdFunc,
                                     Func<TFourth, TOut> fourthFunc,
                                     Func<TFifth, TOut>  fifthFunc) {
        return secondFunc(_value);
    }

    public override void Match(Action<TFirst>  firstAction,
                               Action<TSecond> secondAction,
                               Action<TThird>  thirdAction,
                               Action<TFourth> fourthAction,
                               Action<TFifth>  fifthAction) {
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
}

internal sealed class ThirdOneOf<TFirst, TSecond, TThird, TFourth, TFifth> : OneOf<TFirst, TSecond, TThird, TFourth, TFifth>
    where TFirst : notnull
    where TSecond : notnull
    where TThird : notnull
    where TFourth : notnull
    where TFifth : notnull {
    private readonly TThird _value;

    public ThirdOneOf(TThird value) {
        _value = value;
    }

    public override bool IsFirst  => false;
    public override bool IsSecond => false;
    public override bool IsThird  => true;
    public override bool IsFourth => false;
    public override bool IsFifth  => false;

    public override TOut Match<TOut>(Func<TFirst, TOut>  firstFunc,
                                     Func<TSecond, TOut> secondFunc,
                                     Func<TThird, TOut>  thirdFunc,
                                     Func<TFourth, TOut> fourthFunc,
                                     Func<TFifth, TOut>  fifthFunc) {
        return thirdFunc(_value);
    }

    public override void Match(Action<TFirst>  firstAction,
                               Action<TSecond> secondAction,
                               Action<TThird>  thirdAction,
                               Action<TFourth> fourthAction,
                               Action<TFifth>  fifthAction) {
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
}

internal sealed class FourthOneOf<TFirst, TSecond, TThird, TFourth, TFifth> : OneOf<TFirst, TSecond, TThird, TFourth, TFifth>
    where TFirst : notnull
    where TSecond : notnull
    where TThird : notnull
    where TFourth : notnull
    where TFifth : notnull {
    private readonly TFourth _value;

    public FourthOneOf(TFourth value) {
        _value = value;
    }

    public override bool IsFirst  => false;
    public override bool IsSecond => false;
    public override bool IsThird  => false;
    public override bool IsFourth => true;
    public override bool IsFifth  => false;

    public override TOut Match<TOut>(Func<TFirst, TOut>  firstFunc,
                                     Func<TSecond, TOut> secondFunc,
                                     Func<TThird, TOut>  thirdFunc,
                                     Func<TFourth, TOut> fourthFunc,
                                     Func<TFifth, TOut>  fifthFunc) {
        return fourthFunc(_value);
    }

    public override void Match(Action<TFirst>  firstAction,
                               Action<TSecond> secondAction,
                               Action<TThird>  thirdAction,
                               Action<TFourth> fourthAction,
                               Action<TFifth>  fifthAction) {
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
}

internal sealed class FifthOneOf<TFirst, TSecond, TThird, TFourth, TFifth> : OneOf<TFirst, TSecond, TThird, TFourth, TFifth>
    where TFirst : notnull
    where TSecond : notnull
    where TThird : notnull
    where TFourth : notnull
    where TFifth : notnull {
    private readonly TFifth _value;

    public FifthOneOf(TFifth value) {
        _value = value;
    }

    public override bool IsFirst  => false;
    public override bool IsSecond => false;
    public override bool IsThird  => false;
    public override bool IsFourth => false;
    public override bool IsFifth  => true;

    public override TOut Match<TOut>(Func<TFirst, TOut>  firstFunc,
                                     Func<TSecond, TOut> secondFunc,
                                     Func<TThird, TOut>  thirdFunc,
                                     Func<TFourth, TOut> fourthFunc,
                                     Func<TFifth, TOut>  fifthFunc) {
        return fifthFunc(_value);
    }

    public override void Match(Action<TFirst>  firstAction,
                               Action<TSecond> secondAction,
                               Action<TThird>  thirdAction,
                               Action<TFourth> fourthAction,
                               Action<TFifth>  fifthAction) {
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
}
