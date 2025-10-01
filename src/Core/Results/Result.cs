namespace UnambitiousFx.Core.Results;

using UnambitiousFx.Core.Results.Reasons;

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

    public static Result Failure(IError error) {
        // Use internal constructor without attaching a primary ExceptionalError reason; caller-provided error becomes canonical.
        var r = new FailureResult(error.Exception ?? new Exception(error.Message), attachPrimaryExceptionalReason: false);
        r.AddReason(error);
        foreach (var kv in error.Metadata) r.AddMetadata(kv.Key, kv.Value);
        return r;
    }

    public static Result Failure(string message) {
        return new FailureResult(message);
    }

    public abstract void Deconstruct(out bool isSuccess, out Exception? error);
    public override string ToString() => $"{(IsSuccess ? "Success" : "Failure")} reasons={Reasons.Count}";
}
