using System.Diagnostics.CodeAnalysis;

namespace UnambitiousFx.Core.OneOf;

internal sealed class FirstOneOf<TFirst, TSecond, TThird, TFourth> : OneOf<TFirst, TSecond, TThird, TFourth>
    where TFirst : notnull
    where TSecond : notnull
    where TThird : notnull
    where TFourth : notnull
{
    private readonly TFirst _first;
    
    public FirstOneOf(TFirst first) {
        _first = first;
    }
    
    public override bool IsFirst => true;
    public override bool IsSecond => false;
    public override bool IsThird => false;
    public override bool IsFourth => false;
    
    public override TOut Match<TOut>(Func<TFirst, TOut> firstFunc, Func<TSecond, TOut> secondFunc, Func<TThird, TOut> thirdFunc, Func<TFourth, TOut> fourthFunc) {
        return firstFunc(_first);
    }
    
    public override void Match(Action<TFirst> firstAction, Action<TSecond> secondAction, Action<TThird> thirdAction, Action<TFourth> fourthAction) {
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
    
}
