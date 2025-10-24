namespace UnambitiousFx.Core.Results;

public interface IFailureResult : IResult {
    Exception PrimaryException { get; }
}
