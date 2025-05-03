using System.Diagnostics.CodeAnalysis;

namespace UnambitiousFx.Core;

internal sealed class FailureResult : Result {
    private readonly IError _error;

    public FailureResult(IError error) {
        _error = error;
    }

    public override bool IsFaulted => true;
    public override bool IsSuccess => false;

    public override void Match(Action         success,
                               Action<IError> failure) {
        failure(_error);
    }

    public override TOut Match<TOut>(Func<TOut>         success,
                                     Func<IError, TOut> failure) {
        return failure(_error);
    }

    public override Result Bind(Func<Result> bind) {
        return new FailureResult(_error);
    }

    public override Result<TOut> Bind<TOut>(Func<Result<TOut>> bind) {
        return new FailureResult<TOut>(_error);
    }

    public override void IfSuccess(Action action) {
    }

    public override ValueTask IfSuccess(Func<ValueTask> action) {
        return ValueTask.CompletedTask;
    }

    public override void IfFailure(Action<IError> action) {
        action(_error);
    }

    public override ValueTask IfFailure(Func<IError, ValueTask> action) {
        return action(_error);
    }

    public override bool Ok([NotNullWhen(false)] out IError? error) {
        error = _error;
        return false;
    }
}

internal sealed class FailureResult<TValue> : Result<TValue>
    where TValue : notnull {
    private readonly IError _error;

    public FailureResult(IError error) {
        _error = error;
    }

    public override bool IsFaulted => true;
    public override bool IsSuccess => false;

    public override void Match(Action         success,
                               Action<IError> failure) {
        failure(_error);
    }

    public override TOut Match<TOut>(Func<TOut>         success,
                                     Func<IError, TOut> failure) {
        return failure(_error);
    }

    public override Result Bind(Func<Result> bind) {
        return new FailureResult(_error);
    }

    public override Result<TOut> Bind<TOut>(Func<Result<TOut>> bind) {
        return bind();
    }

    public override void IfSuccess(Action action) {
    }

    public override ValueTask IfSuccess(Func<ValueTask> action) {
        return ValueTask.CompletedTask;
    }

    public override void Match(Action<TValue> success,
                               Action<IError> failure) {
        failure(_error);
    }

    public override TOut Match<TOut>(Func<TValue, TOut> success,
                                     Func<IError, TOut> failure) {
        return failure(_error);
    }

    public override void IfSuccess(Action<TValue> action) {
    }

    public override ValueTask IfSuccess(Func<TValue, ValueTask> action) {
        return ValueTask.CompletedTask;
    }

    public override void IfFailure(Action<IError> action) {
        action(_error);
    }

    public override ValueTask IfFailure(Func<IError, ValueTask> action) {
        return action(_error);
    }

    public override bool Ok([NotNullWhen(false)] out IError? error) {
        error = _error;
        return false;
    }

    public override bool Ok([NotNullWhen(true)] out  TValue? value,
                            [NotNullWhen(false)] out IError? error) {
        value = default;
        error = _error;
        return false;
    }

    public override bool Ok([NotNullWhen(true)] out TValue? value) {
        value = default;
        return false;
    }

    public override Result<TOut> Bind<TOut>(Func<TValue, Result<TOut>> bind) {
        return new FailureResult<TOut>(_error);
    }

    public override ValueTask<Result<TOut>> Bind<TOut>(Func<TValue, ValueTask<Result<TOut>>> bind) {
        return new ValueTask<Result<TOut>>(new FailureResult<TOut>(_error));
    }
}
