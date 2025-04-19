namespace Oleexo.UnambitiousFx.Core;

public interface IResult<out TValue> : IResult
{
    void Match(Action<TValue> success,
        Action<Error> failure);
    
    TOut Match<TOut>(Func<TValue, TOut> success,
        Func<Error, TOut> failure);
}
public interface IResult
{
    bool IsFaulted { get; }
    bool IsSuccess { get; }
}