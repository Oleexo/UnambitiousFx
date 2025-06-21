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

    /// Binds the current result to another operation that returns a new result. If the current
    /// result is successful, the provided function is executed, and the result of that function
    /// is returned. If the current result is faulted, the fault is propagated without executing
    /// the provided function.
    /// <param name="bind">
    ///     A function that takes no parameters and returns a new result. This function is executed
    ///     only if the current result is successful.
    /// </param>
    /// <returns>
    ///     A new result obtained from executing the provided function if the current result is
    ///     successful, or the existing faulted result if the current result is faulted.
    /// </returns>
    public abstract Result Bind(Func<Result> bind);

    /// Binds the current result to a function that returns a new `Result` instance.
    /// If the current result is successful, the function provided is executed to determine the next result.
    /// If the current result is faulted, the fault state is propagated without invoking the provided function.
    /// <param name="bind">
    ///     A function that is invoked if the current result is successful. This function returns a new `Result` instance
    ///     representing the next operation's result.
    /// </param>
    /// <returns>
    ///     Returns the new `Result` instance obtained by invoking the `bind` function if the current result is successful. If
    ///     the current result is faulted, it returns a propagated fault state.
    /// </returns>
    public abstract Result<TOut> Bind<TOut>(Func<Result<TOut>> bind)
        where TOut : notnull;

    /// Binds the current result to a provided function that returns a new `Result`.
    /// This operation is typically used for chaining multiple operations where the output of one
    /// operation becomes the input to the next.
    /// If the current result is successful, the `bind` function is executed. If the current result is
    /// faulted, it directly returns itself without executing the `bind` function.
    /// <param name="bind">
    ///     A function that takes no parameters and returns a new `Result`.
    ///     This function represents the next operation to execute if the current result is successful.
    /// </param>
    /// <returns>
    ///     A new `Result` created by the invocation of the `bind` function, or the current faulted result
    ///     if the operation was not successful.
    /// </returns>
    public abstract ValueTask<Result<TOut>> Bind<TOut>(Func<ValueTask<Result<TOut>>> bind)
        where TOut : notnull;

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

    /// <summary>
    ///     Creates a failure <see cref="Result" /> with the specified error message.
    /// </summary>
    /// <param name="message">The error message describing the reason for the failure.</param>
    /// <returns>A new instance of <see cref="Result" /> that represents a failure state.</returns>
    public static Result Failure(string message) {
        return new FailureResult(message);
    }

    /// Creates a failure result with the specified error information.
    /// <param name="error">
    ///     The error details encapsulated in an instance of <c>IError</c>. This parameter cannot be null
    ///     and provides information about the error that caused the failure.
    /// </param>
    /// <typeparam name="TValue">
    ///     The type of value associated with the operation. This type parameter must be a non-nullable type.
    /// </typeparam>
    /// <return>
    ///     A <c>Result</c> instance representing a failure case, containing the provided error details.
    /// </return>
    public static Result Failure<TValue>(IError error)
        where TValue : notnull {
        return new FailureResult<TValue>(error);
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
public abstract class Result<TValue> : Result, IResult<TValue>
    where TValue : notnull {
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
    public abstract bool Ok([NotNullWhen(true)] out  TValue? value,
                            [NotNullWhen(false)] out IError? error);

    /// <inheritdoc />
    public abstract bool Ok([NotNullWhen(true)] out TValue? value);

    /// <inheritdoc />
    IResult<TOut> IResult<TValue>.Bind<TOut>(Func<TValue, IResult<TOut>> bind) {
        return Bind(value => bind(value)
                       .Match(Result<TOut>.Success,
                              Result<TOut>.Failure));
    }

    /// <inheritdoc />
    async ValueTask<IResult<TOut>> IResult<TValue>.Bind<TOut>(Func<TValue, ValueTask<IResult<TOut>>> bind) {
        return await Bind(value => bind(value)
                             .Match(Result<TOut>.Success,
                                    Result<TOut>.Failure));
    }

    /// <summary>
    ///     Binds the current result to a new operation. If the current result is a successful result,
    ///     invokes the provided function to map the current value to a new <see cref="Result{TOut}" />
    ///     of type <typeparamref name="TOut" />. If the current result is a failure, the failure is propagated
    ///     without invoking the provided function.
    /// </summary>
    /// <typeparam name="TOut">The type of the value in the resulting <see cref="Result{TOut}" />.</typeparam>
    /// <param name="bind">
    ///     A function to be executed if the current result is successful, returning a new asynchronous result of type
    ///     <see cref="Result{TOut}" />.
    /// </param>
    /// <returns>
    ///     A new <see cref="Result{TOut}" /> obtained by applying the <paramref name="bind" /> function if the
    ///     current result is successful, or the original failure result if the current result is a failure.
    /// </returns>
    public abstract Result<TOut> Bind<TOut>(Func<TValue, Result<TOut>> bind)
        where TOut : notnull;

    /// Binds the current result to a transformation function that executes asynchronously, enabling continuation
    /// of operations on the value if the result is successful.
    /// If the result is a failure, the original failure is propagated without executing the transformation.
    /// <typeparam name="TOut">The type of the value in the resulting result from the binding operation.</typeparam>
    /// <param name="bind">
    ///     A function to be executed if the current result is successful, returning a new asynchronous result of type
    ///     <see cref="Result{TOut}" />.
    /// </param>
    /// <returns>
    ///     A new asynchronous result that is the result of the function provided, or the propagated failure in case the
    ///     current result is a failure.
    /// </returns>
    public abstract ValueTask<Result<TOut>> Bind<TOut>(Func<TValue, ValueTask<Result<TOut>>> bind)
        where TOut : notnull;

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
    public new static Result<TValue> Failure(IError error) {
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
