using System.Diagnostics.CodeAnalysis;

namespace UnambitiousFx.Core;

/// Represents an abstract base class for handling operation results. This class provides methods
/// and properties to indicate whether an operation was successful or faulted, along with mechanisms
/// to handle and access detailed error information.
/// Subclasses of this type should define the specific details for success and failure results.
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

    /// Represents a successful result.
    /// <return>
    ///     A new instance of a successful result.
    /// </return>
    public static Result Success() {
        return new SuccessResult();
    }

    /// <summary>
    ///     Creates a success result instance.
    /// </summary>
    /// <typeparam name="TValue">The type of the value associated with the success result.</typeparam>
    /// <param name="value">The value to associate with the success result.</param>
    /// <returns>A success result instance containing the specified value.</returns>
    public static Result<TValue> Success<TValue>(TValue value)
        where TValue : notnull {
        return new SuccessResult<TValue>(value);
    }

    /// <summary>
    ///     Creates a failure result containing the specified error.
    /// </summary>
    /// <param name="error">The error that describes why the operation failed.</param>
    /// <returns>A <see cref="Result" /> representing a failed operation with the provided error.</returns>
    public static Result Failure(IError error) {
        return new FailureResult(error);
    }

    /// Defines an implicit conversion operator that allows converting an instance of the
    /// <see cref="Error" />
    /// class to a
    /// <see cref="Result" />
    /// . This operator is used to
    /// simplify the process of creating a failure result from an error.
    public static implicit operator Result(Error error) {
        return Failure(error);
    }
}

/// Represents an abstract base class for operation results with a defined value. This class provides mechanisms
/// to evaluate the outcome of operations through success or failure states and enables specific handling for each case.
/// This class uses generic typing to define operations that return a specific result type. Implementations
/// must handle operations for both success (returning a valid value) and failure (returning an error).
/// Subclasses or implementing classes are expected to define the behavior for success and failure scenarios and provide
/// detailed error information when operations do not succeed.
/// Designed with flexibility in mind, this class provides methods to act upon success or failure, match results, and
/// retrieve values or errors, ensuring clean and clear separation between operational contexts.
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

    /// <inheritdoc />
    public abstract bool Ok([NotNullWhen(true)] out TValue? value);

    /// <summary>
    ///     Creates a success result.
    /// </summary>
    /// <param name="value">The value to associate with the success result.</param>
    /// <returns>A success result containing the specified value.</returns>
    public static Result<TValue> Success(TValue value) {
        return new SuccessResult<TValue>(value);
    }

    /// Creates a new failure result containing the specified error.
    /// <param name="error">The error associated with the failure result.</param>
    /// <returns>A failure result containing the provided error.</returns>
    public static Result<TValue> Failure(IError error) {
        return new FailureResult<TValue>(error);
    }

    /// Allows implicit conversion from a value of type TValue to a
    /// <see cref="Result{TValue}" />
    /// ,
    /// automatically wrapping the value in a successful result.
    /// This operator simplifies the creation of results by enabling direct assignment
    /// of a value to a
    /// <see cref="Result{TValue}" />
    /// , which will be interpreted as a success state.
    public static implicit operator Result<TValue>(TValue value) {
        return Success(value);
    }

    /// Defines the implicit conversion operation from an Error instance to a
    /// <see cref="Result{TValue}" />
    /// .
    /// This conversion treats the provided Error as a failure result.
    /// The resulting
    /// <see cref="Result{TValue}" />
    /// represents an unsuccessful operation containing the provided error information.
    public static implicit operator Result<TValue>(Error error) {
        return Failure(error);
    }
}
