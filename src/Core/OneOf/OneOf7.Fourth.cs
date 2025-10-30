#nullable enable

using System.Diagnostics.CodeAnalysis;

namespace UnambitiousFx.Core.OneOf;

internal sealed class FourthOneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh> : OneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh>
    where TFirst : notnull
    where TSecond : notnull
    where TThird : notnull
    where TFourth : notnull
    where TFifth : notnull
    where TSixth : notnull
    where TSeventh : notnull
{
    private readonly TFourth _fourth;
    
    public FourthOneOf(TFourth fourth) {
        _fourth = fourth;
    }
    
    public override bool IsFirst => false;
    public override bool IsSecond => false;
    public override bool IsThird => false;
    public override bool IsFourth => true;
    public override bool IsFifth => false;
    public override bool IsSixth => false;
    public override bool IsSeventh => false;
    
    public override TOut Match<TOut>(Func<TFirst, TOut> firstFunc, Func<TSecond, TOut> secondFunc, Func<TThird, TOut> thirdFunc, Func<TFourth, TOut> fourthFunc, Func<TFifth, TOut> fifthFunc, Func<TSixth, TOut> sixthFunc, Func<TSeventh, TOut> seventhFunc) {
        return fourthFunc(_fourth);
    }
    
    public override void Match(Action<TFirst> firstAction, Action<TSecond> secondAction, Action<TThird> thirdAction, Action<TFourth> fourthAction, Action<TFifth> fifthAction, Action<TSixth> sixthAction, Action<TSeventh> seventhAction) {
        fourthAction(_fourth);
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
        fourth = _fourth;
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
    
}
