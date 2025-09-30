namespace UnambitiousFx.Core.Results;

internal sealed partial class FailureResult : Result {
    private readonly Exception _error;

    public FailureResult(Exception error) {
        _error = error;
    }

    public FailureResult(string message) {
        _error = new Exception(message);
    }

    public override bool IsFaulted => true;
    public override bool IsSuccess => false;

    public override void Match(Action            success,
                               Action<Exception> failure) {
        failure(_error);
    }

    public override TOut Match<TOut>(Func<TOut>            success,
                                     Func<Exception, TOut> failure) {
        return failure(_error);
    }

    public override void IfSuccess(Action action) {
    }

    public override void IfFailure(Action<Exception> action) {
        action(_error);
    }

    public override bool Ok([System.Diagnostics.CodeAnalysis.NotNullWhen(false)] out Exception? error) {
        error = _error;
        return false;
    }

    public override Result Bind(Func<Result> bind) {
        return new FailureResult(_error);
    }

    public override Result MapError(Func<Exception, Exception> mapError) {
        return new FailureResult(mapError(_error));
    }

    public override Result Tap(Action action) {
        return new FailureResult(_error);
    }

    public override Result TapError(Action<Exception> tapError) {
        tapError(_error);
        return new FailureResult(_error);
    }

    public override void Deconstruct(out bool isSuccess, out Exception? error) {
        isSuccess = false;
        error = _error;
    }

    public override string ToString() => $"Failure({_error.GetType().Name}: {_error.Message}) reasons={Reasons.Count}";
}
