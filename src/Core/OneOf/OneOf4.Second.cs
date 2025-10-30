#nullable enable

using System.Diagnostics.CodeAnalysis;

namespace UnambitiousFx.Core.OneOf;

internal sealed class SecondOneOf<TFirst, TSecond, TThird, TFourth> : OneOf<TFirst, TSecond, TThird, TFourth>
    where TFirst : notnull
    where TSecond : notnull
    where TThird : notnull
    where TFourth : notnull
{
    private readonly TSecond _second;
    
    public SecondOneOf(TSecond second) {
        _second = second;
    }
    
    public override bool IsFirst => false;
    public override bool IsSecond => true;
    public override bool IsThird => false;
    public override bool IsFourth => false;
    
    public override TOut Match<TOut>(Func<TFirst, TOut> firstFunc, Func<TSecond, TOut> secondFunc, Func<TThird, TOut> thirdFunc, Func<TFourth, TOut> fourthFunc) {
        return secondFunc(_second);
    }
    
    public override void Match(Action<TFirst> firstAction, Action<TSecond> secondAction, Action<TThird> thirdAction, Action<TFourth> fourthAction) {
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
    
    public override bool Third([NotNullWhen(true)] out TThird? third) {
        third = default;
        return false;
    }
    
    public override bool Fourth([NotNullWhen(true)] out TFourth? fourth) {
        fourth = default;
        return false;
    }
    
}
