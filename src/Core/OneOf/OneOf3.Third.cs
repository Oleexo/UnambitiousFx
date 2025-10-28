using System.Diagnostics.CodeAnalysis;

namespace UnambitiousFx.Core.OneOf;

internal sealed class ThirdOneOf<TFirst, TSecond, TThird> : OneOf<TFirst, TSecond, TThird>
    where TFirst : notnull
    where TSecond : notnull
    where TThird : notnull
{
    private readonly TThird _third;
    
    public ThirdOneOf(TThird third) {
        _third = third;
    }
    
    public override bool IsFirst => false;
    public override bool IsSecond => false;
    public override bool IsThird => true;
    
    public override TOut Match<TOut>(Func<TFirst, TOut> firstFunc, Func<TSecond, TOut> secondFunc, Func<TThird, TOut> thirdFunc) {
        return thirdFunc(_third);
    }
    
    public override void Match(Action<TFirst> firstAction, Action<TSecond> secondAction, Action<TThird> thirdAction) {
        thirdAction(_third);
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
        third = _third;
        return true;
    }
    
}
