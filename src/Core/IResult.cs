using System.Diagnostics.CodeAnalysis;

namespace UnambitiousFx.Core;

/// Defines a contract representing the result of an operation, indicating whether it succeeded or failed and providing state information.
public interface IResult<TValue> : IResult
    where TValue : notnull {
    /// Matches the result state and executes the respective action based on whether the result
    /// is successful or faulted. Use this method to handle both success and failure cases directly
    /// through actions.
    /// <param name="success">
    ///     The action to execute if the result is successful. It takes the successfully returned value as a parameter.
    /// </param>
    /// <param name="failure">
    ///     The action to execute if the result is faulted. It takes the Error object representing the error as a parameter.
    /// </param>
    void Match(Action<TValue> success,
               Action<IError> failure);

    /// Matches the result of an operation, calling a function depending on its success or failure state.
    /// <param name="success">
    ///     The function to invoke if the operation was successful. Accepts the successful value as a
    ///     parameter.
    /// </param>
    /// <param name="failure">The function to invoke if the operation failed. Accepts the error as a parameter.</param>
    /// <typeparam name="TOut">The return type of the success or failure function.</typeparam>
    /// <returns>The result from either the success or failure function, depending on the operation's state.</returns>
    TOut Match<TOut>(Func<TValue, TOut> success,
                     Func<IError, TOut> failure);

    /// Executes the provided action if the result is successful.
    /// This method allows performing additional operations when the result is in a successful state.
    /// <param name="action">
    ///     The action to execute if the result is successful. It takes the successfully returned value as a parameter.
    /// </param>
    void IfSuccess(Action<TValue> action);

    /// Executes the specified action if the operation was successful.
    /// Use this method to perform side effects or additional logic when a successful result is present.
    /// <param name="action">
    ///     The action to execute if the result is successful. It takes the successfully returned value as a parameter.
    /// </param>
    ValueTask IfSuccess(Func<TValue, ValueTask> action);

    /// Checks if the result indicates success or failure, and outputs the corresponding value or error.
    /// <param name="value">
    ///     When the method returns true, this will contain the successful value of the result. If the method
    ///     returns false, this will be null.
    /// </param>
    /// <param name="error">
    ///     When the method returns false, this will contain the error associated with the result. If the
    ///     method returns true, this will be null.
    /// </param>
    /// <returns>
    ///     Returns true if the result is successful and contains a valid value; otherwise, returns false if the result
    ///     contains an error.
    /// </returns>
    bool Ok([NotNullWhen(true)] out  TValue? value,
            [NotNullWhen(false)] out IError? error);

    /// Determines whether the operation was successful and provides the resulting value if so.
    /// <param name="value">
    ///     When the operation is successful, this parameter is set to the returned value. Otherwise, it is set to null.
    /// </param>
    /// <returns>
    ///     Returns true if the operation was successful and `value` is set to the resulting data.
    ///     Returns false if the operation failed and `value` is set to null.
    /// </returns>
    bool Ok([NotNullWhen(true)] out TValue? value);

    /// <summary>
    ///     Binds the current result to a new operation. If the current result is a successful result,
    ///     invokes the provided function to map the current value to a new <see cref="IResult{TOut}" />
    ///     of type <typeparamref name="TOut" />. If the current result is a failure, the failure is propagated
    ///     without invoking the provided function.
    /// </summary>
    /// <typeparam name="TOut">The type of the value in the resulting <see cref="IResult{TOut}" />.</typeparam>
    /// <param name="bind">
    ///     A function to be executed if the current result is successful, returning a new asynchronous result of type
    ///     <see cref="IResult{TOut}" />.
    /// </param>
    /// <returns>
    ///     A new <see cref="IResult{TOut}" /> obtained by applying the <paramref name="bind" /> function if the
    ///     current result is successful, or the original failure result if the current result is a failure.
    /// </returns>
    IResult<TOut> Bind<TOut>(Func<TValue, IResult<TOut>> bind)
        where TOut : notnull;

    /// Binds the current result to a transformation function that executes asynchronously, enabling continuation
    /// of operations on the value if the result is successful.
    /// If the result is a failure, the original failure is propagated without executing the transformation.
    /// <typeparam name="TOut">The type of the value in the resulting result from the binding operation.</typeparam>
    /// <param name="bind">
    ///     A function to be executed if the current result is successful, returning a new asynchronous result of type
    ///     <see cref="IResult{TOut}" />.
    /// </param>
    /// <returns>
    ///     A new asynchronous result that is the result of the function provided, or the propagated failure in case the
    ///     current result is a failure.
    /// </returns>
    ValueTask<IResult<TOut>> Bind<TOut>(Func<TValue, ValueTask<IResult<TOut>>> bind)
        where TOut : notnull;
}

/// Represents an operation result indicating success or failure.
/// Provides methods to handle and process the result state, either successful or faulted.
/// Enables functional and callback-based handling of success and error scenarios.
public interface IResult {
    /// Indicates whether the current result represents a failure state.
    /// If the value of this property is true, it means an error or fault occurred
    /// during the operation represented by the result. Conversely, if it is false,
    /// it indicates that the result is either successful or in a non-failure state.
    bool IsFaulted { get; }

    /// Indicates whether the result represents a successful operation.
    /// This property returns true if the result signifies success, and false otherwise.
    /// It is the counterpart of the IsFaulted property, which indicates whether the
    /// result represents a failure. When IsSuccess is true, the result can be expected
    /// to hold a valid value or signify a completed operation without errors.
    bool IsSuccess { get; }

    /// Matches the result state and executes the respective action based on whether
    /// the result is successful or faulted. This method is used to handle success and
    /// error cases with respective actions.
    /// <param name="success">
    ///     The action to execute if the result is successful.
    /// </param>
    /// <param name="failure">
    ///     The action to execute if the result is faulted. It receives an IError
    ///     object detailing the fault condition.
    /// </param>
    void Match(Action         success,
               Action<IError> failure);

    /// Matches the result state and executes the respective action based on whether the result
    /// is successful or faulted. Use this method to handle both success and failure cases directly
    /// through actions.
    /// <param name="success">
    ///     The action to execute if the result is successful.
    /// </param>
    /// <param name="failure">
    ///     The action to execute if the result is faulted. It takes the Error object representing the error as a parameter.
    /// </param>
    TOut Match<TOut>(Func<TOut>         success,
                     Func<IError, TOut> failure);

    /// Executes the provided action if the result is successful. This allows handling the success scenario
    /// by performing an operation on the successful result value.
    /// <param name="action">
    ///     The action to execute if the result is successful. It takes the successfully returned value as a parameter.
    /// </param>
    void IfSuccess(Action action);

    /// Executes the provided action if the result indicates success.
    /// This method allows handling scenarios specifically when the operation has succeeded.
    /// <param name="action">
    ///     The action to execute. It takes the successfully returned value as a parameter.
    /// </param>
    ValueTask IfSuccess(Func<ValueTask> action);

    /// Executes the specified action if the result is faulted, allowing custom handling
    /// of error cases or logging. This method does not execute if the result is successful.
    /// <param name="action">
    ///     An action to execute when the result is faulted. The action receives the IError object
    ///     representing the error as a parameter, providing detailed information about the failure.
    /// </param>
    void IfFailure(Action<IError> action);

    /// Executes the provided action if the result is faulted.
    /// Use this method to handle error scenarios by processing the associated error information.
    /// <param name="action">
    ///     The action to execute if the result is faulted. It takes the IError object representing the error as a parameter.
    /// </param>
    ValueTask IfFailure(Func<IError, ValueTask> action);

    /// Evaluates whether the operation was successful. Provides the resulting error if the operation failed.
    /// <param name="error">
    ///     Outputs the error associated with the operation if it was faulted. This will be null if the operation succeeded.
    /// </param>
    /// <returns>
    ///     true if the operation succeeded, false otherwise.
    /// </returns>
    bool Ok([NotNullWhen(false)] out IError? error);
}
