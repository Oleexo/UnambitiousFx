#nullable enable

using System.Diagnostics.CodeAnalysis;

namespace UnambitiousFx.Core.OneOf;

internal sealed class SixthOneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth> : OneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>
    where TFirst : notnull
    where TSecond : notnull
    where TThird : notnull
    where TFourth : notnull
    where TFifth : notnull
    where TSixth : notnull
{
    private readonly TSixth _sixth;
    
    public SixthOneOf(TSixth sixth) {
        _sixth = sixth;
    }
    
    public override bool IsFirst => false;
    public override bool IsSecond => false;
    public override bool IsThird => false;
    public override bool IsFourth => false;
    public override bool IsFifth => false;
    public override bool IsSixth => true;
    
    public override TOut Match<TOut>(Func<TFirst, TOut> firstFunc, Func<TSecond, TOut> secondFunc, Func<TThird, TOut> thirdFunc, Func<TFourth, TOut> fourthFunc, Func<TFifth, TOut> fifthFunc, Func<TSixth, TOut> sixthFunc) {
        return sixthFunc(_sixth);
    }
    
    public override void Match(Action<TFirst> firstAction, Action<TSecond> secondAction, Action<TThird> thirdAction, Action<TFourth> fourthAction, Action<TFifth> fifthAction, Action<TSixth> sixthAction) {
        sixthAction(_sixth);
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
        sixth = _sixth;
        return true;
    }
    
}
