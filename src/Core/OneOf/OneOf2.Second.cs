using System.Diagnostics.CodeAnalysis;

namespace UnambitiousFx.Core.OneOf;

internal sealed class SecondOneOf<TFirst, TSecond> : OneOf<TFirst, TSecond>
    where TFirst : notnull
    where TSecond : notnull
{
    private readonly TSecond _second;
    
    public SecondOneOf(TSecond second) {
        _second = second;
    }
    
    public override bool IsFirst => false;
    public override bool IsSecond => true;
    
    public override TOut Match<TOut>(Func<TFirst, TOut> firstFunc, Func<TSecond, TOut> secondFunc) {
        return secondFunc(_second);
    }
    
    public override void Match(Action<TFirst> firstAction, Action<TSecond> secondAction) {
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
    
}
