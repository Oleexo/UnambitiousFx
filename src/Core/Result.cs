using System.Diagnostics.CodeAnalysis;
using Oleexo.UnambitiousFx.Core.Abstractions;

namespace Oleexo.UnambitiousFx.Core;

public abstract class Result : IResult {
    /// <inheritdoc />
    public abstract bool IsFaulted { get; }

    /// <inheritdoc />
    public abstract bool IsSuccess { get; }

    /// <inheritdoc />
    public abstract void Match(Action         success,
                               Action<IError> failure);

    /// <inheritdoc />
    public abstract TOut Match<TOut>(Func<TOut>         success,
                                     Func<IError, TOut> failure);

    /// <inheritdoc />
    public abstract void IfSuccess(Action action);

    /// <inheritdoc />
    public abstract ValueTask IfSuccess(Func<ValueTask> action);

    /// <inheritdoc />
    public abstract void IfFailure(Action<IError> action);

    /// <inheritdoc />
    public abstract ValueTask IfFailure(Func<IError, ValueTask> action);

    /// <inheritdoc />
    public abstract bool Ok([NotNullWhen(false)] out IError? error);

    public static Result Success() {
        return new SuccessResult();
    }

    public static Result Failure(IError error) {
        return new FailureResult(error);
    }
}

public abstract class Result<TValue> : IResult<TValue>
    where TValue : notnull {
    /// <inheritdoc />
    public abstract bool IsFaulted { get; }

    /// <inheritdoc />
    public abstract bool IsSuccess { get; }

    /// <inheritdoc />
    public abstract void Match(Action<TValue> success,
                               Action<IError> failure);

    /// <inheritdoc />
    public abstract TOut Match<TOut>(Func<TValue, TOut> success,
                                     Func<IError, TOut> failure);

    /// <inheritdoc />
    public abstract void IfSuccess(Action<TValue> action);

    /// <inheritdoc />
    public abstract ValueTask IfSuccess(Func<TValue, ValueTask> action);

    /// <inheritdoc />
    public abstract void IfFailure(Action<IError> action);

    /// <inheritdoc />
    public abstract ValueTask IfFailure(Func<IError, ValueTask> action);

    /// <inheritdoc />
    public abstract bool Ok([NotNullWhen(true)] out  TValue? value,
                            [NotNullWhen(false)] out IError? error);

    public static Result<TValue> Success(TValue value) {
        return new SuccessResult<TValue>(value);
    }

    public static Result<TValue> Failure(IError error) {
        return new FailureResult<TValue>(error);
    }
}
