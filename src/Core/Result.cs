using System.Diagnostics.CodeAnalysis;

namespace Oleexo.UnambitiousFx.Core;

public abstract class Result<TValue> : IResult<TValue> where TValue : notnull
{
    public static Result<TValue> Success(TValue value)
    {
        return new SuccessResult<TValue>(value);
    }
    
    public static Result<TValue> Failure(Error error)
    {
        return new FailureResult<TValue>(error);
    }
    /// <inheritdoc/>
    public abstract bool IsFaulted { get; }
    /// <inheritdoc/>
    public abstract bool IsSuccess { get; }
    /// <inheritdoc/>
    public abstract void Match(Action<TValue> success, Action<Error> failure);
    /// <inheritdoc/>
    public abstract TOut Match<TOut>(Func<TValue, TOut> success, Func<Error, TOut> failure);
    /// <inheritdoc/>
    public abstract bool Ok([NotNullWhen(true)] out TValue? value, [NotNullWhen(false)] out Error? error);
}