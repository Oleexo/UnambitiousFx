using System.Diagnostics.CodeAnalysis;

namespace UnambitiousFx.Core.OneOf;

/// <summary>
///     Represents a discriminated union of eight possible value types.
/// </summary>
public abstract class OneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>
    where TFirst : notnull
    where TSecond : notnull
    where TThird : notnull
    where TFourth : notnull
    where TFifth : notnull
    where TSixth : notnull
    where TSeventh : notnull
    where TEighth : notnull {
    public abstract bool IsFirst   { get; }
    public abstract bool IsSecond  { get; }
    public abstract bool IsThird   { get; }
    public abstract bool IsFourth  { get; }
    public abstract bool IsFifth   { get; }
    public abstract bool IsSixth   { get; }
    public abstract bool IsSeventh { get; }
    public abstract bool IsEighth  { get; }

    public abstract TOut Match<TOut>(Func<TFirst, TOut>   firstFunc,
                                     Func<TSecond, TOut>  secondFunc,
                                     Func<TThird, TOut>   thirdFunc,
                                     Func<TFourth, TOut>  fourthFunc,
                                     Func<TFifth, TOut>   fifthFunc,
                                     Func<TSixth, TOut>   sixthFunc,
                                     Func<TSeventh, TOut> seventhFunc,
                                     Func<TEighth, TOut>  eighthFunc);

    public abstract void Match(Action<TFirst>   firstAction,
                               Action<TSecond>  secondAction,
                               Action<TThird>   thirdAction,
                               Action<TFourth>  fourthAction,
                               Action<TFifth>   fifthAction,
                               Action<TSixth>   sixthAction,
                               Action<TSeventh> seventhAction,
                               Action<TEighth>  eighthAction);

    public abstract bool First([NotNullWhen(  true)] out TFirst?   first);
    public abstract bool Second([NotNullWhen( true)] out TSecond?  second);
    public abstract bool Third([NotNullWhen(  true)] out TThird?   third);
    public abstract bool Fourth([NotNullWhen( true)] out TFourth?  fourth);
    public abstract bool Fifth([NotNullWhen(  true)] out TFifth?   fifth);
    public abstract bool Sixth([NotNullWhen(  true)] out TSixth?   sixth);
    public abstract bool Seventh([NotNullWhen(true)] out TSeventh? seventh);
    public abstract bool Eighth([NotNullWhen( true)] out TEighth?  eighth);

    // Factories
    public static OneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth> FromFirst(TFirst value) {
        return new FirstOneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>(value);
    }

    public static OneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth> FromSecond(TSecond value) {
        return new SecondOneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>(value);
    }

    public static OneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth> FromThird(TThird value) {
        return new ThirdOneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>(value);
    }

    public static OneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth> FromFourth(TFourth value) {
        return new FourthOneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>(value);
    }

    public static OneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth> FromFifth(TFifth value) {
        return new FifthOneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>(value);
    }

    public static OneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth> FromSixth(TSixth value) {
        return new SixthOneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>(value);
    }

    public static OneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth> FromSeventh(TSeventh value) {
        return new SeventhOneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>(value);
    }

    public static OneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth> FromEighth(TEighth value) {
        return new EighthOneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>(value);
    }
}

internal sealed class FirstOneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth> : OneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>
    where TFirst : notnull
    where TSecond : notnull
    where TThird : notnull
    where TFourth : notnull
    where TFifth : notnull
    where TSixth : notnull
    where TSeventh : notnull
    where TEighth : notnull {
    private readonly TFirst _value;

    public FirstOneOf(TFirst value) {
        _value = value;
    }

    public override bool IsFirst   => true;
    public override bool IsSecond  => false;
    public override bool IsThird   => false;
    public override bool IsFourth  => false;
    public override bool IsFifth   => false;
    public override bool IsSixth   => false;
    public override bool IsSeventh => false;
    public override bool IsEighth  => false;

    public override TOut Match<TOut>(Func<TFirst, TOut>   firstFunc,
                                     Func<TSecond, TOut>  secondFunc,
                                     Func<TThird, TOut>   thirdFunc,
                                     Func<TFourth, TOut>  fourthFunc,
                                     Func<TFifth, TOut>   fifthFunc,
                                     Func<TSixth, TOut>   sixthFunc,
                                     Func<TSeventh, TOut> seventhFunc,
                                     Func<TEighth, TOut>  eighthFunc) {
        return firstFunc(_value);
    }

    public override void Match(Action<TFirst>   firstAction,
                               Action<TSecond>  secondAction,
                               Action<TThird>   thirdAction,
                               Action<TFourth>  fourthAction,
                               Action<TFifth>   fifthAction,
                               Action<TSixth>   sixthAction,
                               Action<TSeventh> seventhAction,
                               Action<TEighth>  eighthAction) {
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

    public override bool Seventh([NotNullWhen(true)] out TSeventh? seventh) {
        seventh = default;
        return false;
    }

    public override bool Eighth([NotNullWhen(true)] out TEighth? eighth) {
        eighth = default;
        return false;
    }
}

internal sealed class SecondOneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth> : OneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>
    where TFirst : notnull
    where TSecond : notnull
    where TThird : notnull
    where TFourth : notnull
    where TFifth : notnull
    where TSixth : notnull
    where TSeventh : notnull
    where TEighth : notnull {
    private readonly TSecond _value;

    public SecondOneOf(TSecond value) {
        _value = value;
    }

    public override bool IsFirst   => false;
    public override bool IsSecond  => true;
    public override bool IsThird   => false;
    public override bool IsFourth  => false;
    public override bool IsFifth   => false;
    public override bool IsSixth   => false;
    public override bool IsSeventh => false;
    public override bool IsEighth  => false;

    public override TOut Match<TOut>(Func<TFirst, TOut>   firstFunc,
                                     Func<TSecond, TOut>  secondFunc,
                                     Func<TThird, TOut>   thirdFunc,
                                     Func<TFourth, TOut>  fourthFunc,
                                     Func<TFifth, TOut>   fifthFunc,
                                     Func<TSixth, TOut>   sixthFunc,
                                     Func<TSeventh, TOut> seventhFunc,
                                     Func<TEighth, TOut>  eighthFunc) {
        return secondFunc(_value);
    }

    public override void Match(Action<TFirst>   firstAction,
                               Action<TSecond>  secondAction,
                               Action<TThird>   thirdAction,
                               Action<TFourth>  fourthAction,
                               Action<TFifth>   fifthAction,
                               Action<TSixth>   sixthAction,
                               Action<TSeventh> seventhAction,
                               Action<TEighth>  eighthAction) {
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

    public override bool Seventh([NotNullWhen(true)] out TSeventh? seventh) {
        seventh = default;
        return false;
    }

    public override bool Eighth([NotNullWhen(true)] out TEighth? eighth) {
        eighth = default;
        return false;
    }
}

internal sealed class ThirdOneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth> : OneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>
    where TFirst : notnull
    where TSecond : notnull
    where TThird : notnull
    where TFourth : notnull
    where TFifth : notnull
    where TSixth : notnull
    where TSeventh : notnull
    where TEighth : notnull {
    private readonly TThird _value;

    public ThirdOneOf(TThird value) {
        _value = value;
    }

    public override bool IsFirst   => false;
    public override bool IsSecond  => false;
    public override bool IsThird   => true;
    public override bool IsFourth  => false;
    public override bool IsFifth   => false;
    public override bool IsSixth   => false;
    public override bool IsSeventh => false;
    public override bool IsEighth  => false;

    public override TOut Match<TOut>(Func<TFirst, TOut>   firstFunc,
                                     Func<TSecond, TOut>  secondFunc,
                                     Func<TThird, TOut>   thirdFunc,
                                     Func<TFourth, TOut>  fourthFunc,
                                     Func<TFifth, TOut>   fifthFunc,
                                     Func<TSixth, TOut>   sixthFunc,
                                     Func<TSeventh, TOut> seventhFunc,
                                     Func<TEighth, TOut>  eighthFunc) {
        return thirdFunc(_value);
    }

    public override void Match(Action<TFirst>   firstAction,
                               Action<TSecond>  secondAction,
                               Action<TThird>   thirdAction,
                               Action<TFourth>  fourthAction,
                               Action<TFifth>   fifthAction,
                               Action<TSixth>   sixthAction,
                               Action<TSeventh> seventhAction,
                               Action<TEighth>  eighthAction) {
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

    public override bool Seventh([NotNullWhen(true)] out TSeventh? seventh) {
        seventh = default;
        return false;
    }

    public override bool Eighth([NotNullWhen(true)] out TEighth? eighth) {
        eighth = default;
        return false;
    }
}

internal sealed class FourthOneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth> : OneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>
    where TFirst : notnull
    where TSecond : notnull
    where TThird : notnull
    where TFourth : notnull
    where TFifth : notnull
    where TSixth : notnull
    where TSeventh : notnull
    where TEighth : notnull {
    private readonly TFourth _value;

    public FourthOneOf(TFourth value) {
        _value = value;
    }

    public override bool IsFirst   => false;
    public override bool IsSecond  => false;
    public override bool IsThird   => false;
    public override bool IsFourth  => true;
    public override bool IsFifth   => false;
    public override bool IsSixth   => false;
    public override bool IsSeventh => false;
    public override bool IsEighth  => false;

    public override TOut Match<TOut>(Func<TFirst, TOut>   firstFunc,
                                     Func<TSecond, TOut>  secondFunc,
                                     Func<TThird, TOut>   thirdFunc,
                                     Func<TFourth, TOut>  fourthFunc,
                                     Func<TFifth, TOut>   fifthFunc,
                                     Func<TSixth, TOut>   sixthFunc,
                                     Func<TSeventh, TOut> seventhFunc,
                                     Func<TEighth, TOut>  eighthFunc) {
        return fourthFunc(_value);
    }

    public override void Match(Action<TFirst>   firstAction,
                               Action<TSecond>  secondAction,
                               Action<TThird>   thirdAction,
                               Action<TFourth>  fourthAction,
                               Action<TFifth>   fifthAction,
                               Action<TSixth>   sixthAction,
                               Action<TSeventh> seventhAction,
                               Action<TEighth>  eighthAction) {
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

    public override bool Seventh([NotNullWhen(true)] out TSeventh? seventh) {
        seventh = default;
        return false;
    }

    public override bool Eighth([NotNullWhen(true)] out TEighth? eighth) {
        eighth = default;
        return false;
    }
}

internal sealed class FifthOneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth> : OneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>
    where TFirst : notnull
    where TSecond : notnull
    where TThird : notnull
    where TFourth : notnull
    where TFifth : notnull
    where TSixth : notnull
    where TSeventh : notnull
    where TEighth : notnull {
    private readonly TFifth _value;

    public FifthOneOf(TFifth value) {
        _value = value;
    }

    public override bool IsFirst   => false;
    public override bool IsSecond  => false;
    public override bool IsThird   => false;
    public override bool IsFourth  => false;
    public override bool IsFifth   => true;
    public override bool IsSixth   => false;
    public override bool IsSeventh => false;
    public override bool IsEighth  => false;

    public override TOut Match<TOut>(Func<TFirst, TOut>   firstFunc,
                                     Func<TSecond, TOut>  secondFunc,
                                     Func<TThird, TOut>   thirdFunc,
                                     Func<TFourth, TOut>  fourthFunc,
                                     Func<TFifth, TOut>   fifthFunc,
                                     Func<TSixth, TOut>   sixthFunc,
                                     Func<TSeventh, TOut> seventhFunc,
                                     Func<TEighth, TOut>  eighthFunc) {
        return fifthFunc(_value);
    }

    public override void Match(Action<TFirst>   firstAction,
                               Action<TSecond>  secondAction,
                               Action<TThird>   thirdAction,
                               Action<TFourth>  fourthAction,
                               Action<TFifth>   fifthAction,
                               Action<TSixth>   sixthAction,
                               Action<TSeventh> seventhAction,
                               Action<TEighth>  eighthAction) {
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

    public override bool Seventh([NotNullWhen(true)] out TSeventh? seventh) {
        seventh = default;
        return false;
    }

    public override bool Eighth([NotNullWhen(true)] out TEighth? eighth) {
        eighth = default;
        return false;
    }
}

internal sealed class SixthOneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth> : OneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>
    where TFirst : notnull
    where TSecond : notnull
    where TThird : notnull
    where TFourth : notnull
    where TFifth : notnull
    where TSixth : notnull
    where TSeventh : notnull
    where TEighth : notnull {
    private readonly TSixth _value;

    public SixthOneOf(TSixth value) {
        _value = value;
    }

    public override bool IsFirst   => false;
    public override bool IsSecond  => false;
    public override bool IsThird   => false;
    public override bool IsFourth  => false;
    public override bool IsFifth   => false;
    public override bool IsSixth   => true;
    public override bool IsSeventh => false;
    public override bool IsEighth  => false;

    public override TOut Match<TOut>(Func<TFirst, TOut>   firstFunc,
                                     Func<TSecond, TOut>  secondFunc,
                                     Func<TThird, TOut>   thirdFunc,
                                     Func<TFourth, TOut>  fourthFunc,
                                     Func<TFifth, TOut>   fifthFunc,
                                     Func<TSixth, TOut>   sixthFunc,
                                     Func<TSeventh, TOut> seventhFunc,
                                     Func<TEighth, TOut>  eighthFunc) {
        return sixthFunc(_value);
    }

    public override void Match(Action<TFirst>   firstAction,
                               Action<TSecond>  secondAction,
                               Action<TThird>   thirdAction,
                               Action<TFourth>  fourthAction,
                               Action<TFifth>   fifthAction,
                               Action<TSixth>   sixthAction,
                               Action<TSeventh> seventhAction,
                               Action<TEighth>  eighthAction) {
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

    public override bool Seventh([NotNullWhen(true)] out TSeventh? seventh) {
        seventh = default;
        return false;
    }

    public override bool Eighth([NotNullWhen(true)] out TEighth? eighth) {
        eighth = default;
        return false;
    }
}

internal sealed class SeventhOneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth> : OneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>
    where TFirst : notnull
    where TSecond : notnull
    where TThird : notnull
    where TFourth : notnull
    where TFifth : notnull
    where TSixth : notnull
    where TSeventh : notnull
    where TEighth : notnull {
    private readonly TSeventh _value;

    public SeventhOneOf(TSeventh value) {
        _value = value;
    }

    public override bool IsFirst   => false;
    public override bool IsSecond  => false;
    public override bool IsThird   => false;
    public override bool IsFourth  => false;
    public override bool IsFifth   => false;
    public override bool IsSixth   => false;
    public override bool IsSeventh => true;
    public override bool IsEighth  => false;

    public override TOut Match<TOut>(Func<TFirst, TOut>   firstFunc,
                                     Func<TSecond, TOut>  secondFunc,
                                     Func<TThird, TOut>   thirdFunc,
                                     Func<TFourth, TOut>  fourthFunc,
                                     Func<TFifth, TOut>   fifthFunc,
                                     Func<TSixth, TOut>   sixthFunc,
                                     Func<TSeventh, TOut> seventhFunc,
                                     Func<TEighth, TOut>  eighthFunc) {
        return seventhFunc(_value);
    }

    public override void Match(Action<TFirst>   firstAction,
                               Action<TSecond>  secondAction,
                               Action<TThird>   thirdAction,
                               Action<TFourth>  fourthAction,
                               Action<TFifth>   fifthAction,
                               Action<TSixth>   sixthAction,
                               Action<TSeventh> seventhAction,
                               Action<TEighth>  eighthAction) {
        seventhAction(_value);
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
        sixth = default;
        return false;
    }

    public override bool Seventh([NotNullWhen(true)] out TSeventh? seventh) {
        seventh = _value;
        return true;
    }

    public override bool Eighth([NotNullWhen(true)] out TEighth? eighth) {
        eighth = default;
        return false;
    }
}

internal sealed class EighthOneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth> : OneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>
    where TFirst : notnull
    where TSecond : notnull
    where TThird : notnull
    where TFourth : notnull
    where TFifth : notnull
    where TSixth : notnull
    where TSeventh : notnull
    where TEighth : notnull {
    private readonly TEighth _value;

    public EighthOneOf(TEighth value) {
        _value = value;
    }

    public override bool IsFirst   => false;
    public override bool IsSecond  => false;
    public override bool IsThird   => false;
    public override bool IsFourth  => false;
    public override bool IsFifth   => false;
    public override bool IsSixth   => false;
    public override bool IsSeventh => false;
    public override bool IsEighth  => true;

    public override TOut Match<TOut>(Func<TFirst, TOut>   firstFunc,
                                     Func<TSecond, TOut>  secondFunc,
                                     Func<TThird, TOut>   thirdFunc,
                                     Func<TFourth, TOut>  fourthFunc,
                                     Func<TFifth, TOut>   fifthFunc,
                                     Func<TSixth, TOut>   sixthFunc,
                                     Func<TSeventh, TOut> seventhFunc,
                                     Func<TEighth, TOut>  eighthFunc) {
        return eighthFunc(_value);
    }

    public override void Match(Action<TFirst>   firstAction,
                               Action<TSecond>  secondAction,
                               Action<TThird>   thirdAction,
                               Action<TFourth>  fourthAction,
                               Action<TFifth>   fifthAction,
                               Action<TSixth>   sixthAction,
                               Action<TSeventh> seventhAction,
                               Action<TEighth>  eighthAction) {
        eighthAction(_value);
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
        sixth = default;
        return false;
    }

    public override bool Seventh([NotNullWhen(true)] out TSeventh? seventh) {
        seventh = default;
        return false;
    }

    public override bool Eighth([NotNullWhen(true)] out TEighth? eighth) {
        eighth = _value;
        return true;
    }
}
