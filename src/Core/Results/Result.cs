using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Results;

public abstract partial class Result : BaseResult {
    public static Result Success() {
        return new SuccessResult();
    }

    public static Result Failure(Exception error) {
        return new FailureResult(error);
    }

    public static Result Failure(IError error) {
        var r = new FailureResult(error.Exception ?? new Exception(error.Message), false);
        r.AddReason(error);
        foreach (var kv in error.Metadata) {
            r.AddMetadata(kv.Key, kv.Value);
        }

        return r;
    }

    public static Result Failure(string message) {
        return new FailureResult(message);
    }

    public static Result Failure(IEnumerable<IError> errors) {
        return new FailureResult(errors);
    }

    public abstract void Deconstruct(out bool       isSuccess,
                                     out IEnumerable<IError>? error);
}
