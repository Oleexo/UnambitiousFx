namespace UnambitiousFx.Core.Results;

public abstract partial class Result : BaseResult {
    public abstract Result Bind(Func<Result>                   bind);
    public abstract Result MapError(Func<Exception, Exception> mapError);
    public abstract Result Tap(Action action);
    public abstract Result TapError(Action<Exception> tapError);

    public static Result Success() {
        return new SuccessResult();
    }

    public static Result Failure(Exception error) {
        return new FailureResult(error);
    }

    public static Result Failure(string message) {
        return new FailureResult(message);
    }
}
