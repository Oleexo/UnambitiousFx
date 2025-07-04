﻿using System.Diagnostics.CodeAnalysis;

namespace UnambitiousFx.Core;

internal sealed class SuccessResult : Result {
    public override bool IsFaulted => false;
    public override bool IsSuccess => true;

    public override void Match(Action         success,
                               Action<IError> failure) {
        success();
    }

    public override TOut Match<TOut>(Func<TOut>         success,
                                     Func<IError, TOut> failure) {
        return success();
    }

    public override Result Bind(Func<Result> bind) {
        return bind();
    }

    public override Result<TOut> Bind<TOut>(Func<Result<TOut>> bind) {
        return bind();
    }

    public override ValueTask<Result<TOut>> Bind<TOut>(Func<ValueTask<Result<TOut>>> bind) {
        return bind();
    }

    public override void IfSuccess(Action action) {
        action();
    }

    public override ValueTask IfSuccess(Func<ValueTask> action) {
        return action();
    }

    public override void IfFailure(Action<IError> action) {
    }

    public override ValueTask IfFailure(Func<IError, ValueTask> action) {
        return ValueTask.CompletedTask;
    }

    public override bool Ok([NotNullWhen(false)] out IError? error) {
        error = null;
        return true;
    }
}

internal sealed class SuccessResult<TValue> : Result<TValue>
    where TValue : notnull {
    private readonly TValue _value;

    public SuccessResult(TValue value) {
        _value = value;
    }

    public override bool IsFaulted => false;
    public override bool IsSuccess => true;

    public override void Match(Action         success,
                               Action<IError> failure) {
        success();
    }

    public override TOut Match<TOut>(Func<TOut>         success,
                                     Func<IError, TOut> failure) {
        return success();
    }

    public override Result Bind(Func<Result> bind) {
        return bind();
    }

    public override Result<TOut> Bind<TOut>(Func<Result<TOut>> bind) {
        return bind();
    }

    public override ValueTask<Result<TOut>> Bind<TOut>(Func<ValueTask<Result<TOut>>> bind) {
        return bind();
    }

    public override void IfSuccess(Action action) {
        action();
    }

    public override ValueTask IfSuccess(Func<ValueTask> action) {
        return action();
    }

    public override void Match(Action<TValue> success,
                               Action<IError> failure) {
        success(_value);
    }

    public override TOut Match<TOut>(Func<TValue, TOut> success,
                                     Func<IError, TOut> failure) {
        return success(_value);
    }

    public override void IfSuccess(Action<TValue> action) {
        action(_value);
    }

    public override ValueTask IfSuccess(Func<TValue, ValueTask> action) {
        return action(_value);
    }

    public override void IfFailure(Action<IError> action) {
    }

    public override ValueTask IfFailure(Func<IError, ValueTask> action) {
        return ValueTask.CompletedTask;
    }

    public override bool Ok([NotNullWhen(false)] out IError? error) {
        error = null;
        return true;
    }

    public override bool Ok([NotNullWhen(true)] out  TValue? value,
                            [NotNullWhen(false)] out IError? error) {
        value = _value;
        error = null;
        return true;
    }

    public override bool Ok([NotNullWhen(true)] out TValue? value) {
        value = _value;
        return true;
    }

    public override Result<TOut> Bind<TOut>(Func<TValue, Result<TOut>> bind) {
        return bind(_value);
    }

    public override ValueTask<Result<TOut>> Bind<TOut>(Func<TValue, ValueTask<Result<TOut>>> bind) {
        return bind(_value);
    }
}
