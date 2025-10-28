using System.Diagnostics.CodeAnalysis;

namespace UnambitiousFx.Core.OneOf;

internal sealed class FirstOneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth> : OneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>
    where TFirst : notnull
    where TSecond : notnull
    where TThird : notnull
    where TFourth : notnull
    where TFifth : notnull
    where TSixth : notnull
    where TSeventh : notnull
    where TEighth : notnull
{
    private readonly TFirst _first;
    
    public FirstOneOf(TFirst first) {
        _first = first;
    }
    
    public override bool IsFirst => true;
    public override bool IsSecond => false;
    public override bool IsThird => false;
    public override bool IsFourth => false;
    public override bool IsFifth => false;
    public override bool IsSixth => false;
    public override bool IsSeventh => false;
    public override bool IsEighth => false;
    
    public override TOut Match<TOut>(Func<TFirst, TOut> firstFunc, Func<TSecond, TOut> secondFunc, Func<TThird, TOut> thirdFunc, Func<TFourth, TOut> fourthFunc, Func<TFifth, TOut> fifthFunc, Func<TSixth, TOut> sixthFunc, Func<TSeventh, TOut> seventhFunc, Func<TEighth, TOut> eighthFunc) {
        return firstFunc(_first);
    }
    
    public override void Match(Action<TFirst> firstAction, Action<TSecond> secondAction, Action<TThird> thirdAction, Action<TFourth> fourthAction, Action<TFifth> fifthAction, Action<TSixth> sixthAction, Action<TSeventh> seventhAction, Action<TEighth> eighthAction) {
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
