using System.Diagnostics.CodeAnalysis;

namespace Oleexo.UnambitiousFx.Core;

/// Defines a contract representing the result of an operation, indicating whether it succeeded or failed and providing state information.
public interface IResult<TValue> : IResult
    where TValue : notnull
{
    /// Matches the result state and executes the respective action based on whether the result
    /// is successful or faulted. Use this method to handle both success and failure cases directly
    /// through actions.
    /// <param name="success">
    /// The action to execute if the result is successful. It takes the successfully returned value as a parameter.
    /// </param>
    /// <param name="failure">
    /// The action to execute if the result is faulted. It takes the Error object representing the error as a parameter.
    /// </param>
    void Match(Action<TValue> success,
        Action<Error> failure);

    /// Matches the result of an operation, calling a function depending on its success or failure state.
    /// <param name="success">The function to invoke if the operation was successful. Accepts the successful value as a parameter.</param>
    /// <param name="failure">The function to invoke if the operation failed. Accepts the error as a parameter.</param>
    /// <typeparam name="TOut">The return type of the success or failure function.</typeparam>
    /// <returns>The result from either the success or failure function, depending on the operation's state.</returns>
    TOut Match<TOut>(Func<TValue, TOut> success,
        Func<Error, TOut> failure);

    /// Checks if the result indicates success or failure, and outputs the corresponding value or error.
    /// <param name="value">When the method returns true, this will contain the successful value of the result. If the method returns false, this will be null.</param>
    /// <param name="error">When the method returns false, this will contain the error associated with the result. If the method returns true, this will be null.</param>
    /// <returns>Returns true if the result is successful and contains a valid value; otherwise, returns false if the result contains an error.</returns>
    bool Ok([NotNullWhen(true)] out TValue? value, [NotNullWhen(false)] out Error? error);
}

/// Represents the result of an operation with a possible success or failure state.
public interface IResult
{
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
}