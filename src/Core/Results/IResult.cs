namespace UnambitiousFx.Core.Results;

public interface IResult
{
    bool IsFaulted { get; }
    bool IsSuccess { get; }
}
