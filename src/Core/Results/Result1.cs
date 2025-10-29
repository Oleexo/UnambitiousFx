using System.Diagnostics.CodeAnalysis;
using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Results;

/// <summary>
/// Represents the result of an operation that can succeed with 1 value(s) or fail with an exception.
/// </summary>
/// <typeparam name="TValue1">The type of the first value.</typeparam>
public abstract class Result<TValue1> : BaseResult
    where TValue1 : notnull
{
    /// <summary>
    /// Pattern matches the result, executing the appropriate action.
    /// </summary>
    /// <param name="success">Action to execute if the result is successful</param>
    /// <param name="failure">Action to execute if the result is a failure</param>
    public abstract void Match(Action<TValue1> success, Action<IEnumerable<IError>> failure);
    
    /// <summary>
    /// Pattern matches the result, returning a value from the appropriate function.
    /// </summary>
    /// <typeparam name="TOut">The type of value to return</typeparam>
    /// <param name="success">Function to invoke if the result is successful</param>
    /// <param name="failure">Function to invoke if the result is a failure</param>
    /// <returns>The result of invoking the appropriate function</returns>
    public abstract TOut Match<TOut>(Func<TValue1, TOut> success, Func<IEnumerable<IError>, TOut> failure);
    
    /// <summary>
    /// Executes the action if the result is successful.
    /// </summary>
    /// <param name="action">Action to execute with the success values</param>
    public abstract void IfSuccess(Action<TValue1> action);
    
    /// <summary>
    /// Attempts to extract the success values and error.
    /// </summary>
    /// <param name="value1">The first value if successful</param>
    /// <param name="error">The exception if failed</param>
    /// <returns>True if successful, false otherwise</returns>
    public abstract bool TryGet([NotNullWhen(true)] out TValue1? value1, [NotNullWhen(false)] out IEnumerable<IError>? error);
    
    /// <summary>
    /// Attempts to extract the success values.
    /// </summary>
    /// <param name="value1">The first value if successful</param>
    /// <returns>True if successful, false otherwise</returns>
    public abstract bool TryGet([NotNullWhen(true)] out TValue1? value1);
    
    /// <summary>
    /// Deconstructs the result into its components.
    /// </summary>
    /// <param name="isSuccess">Whether the result is successful</param>
    /// <param name="value">The success value(s) if successful</param>
    /// <param name="error">The exception if failed</param>
    public abstract void Deconstruct(out bool isSuccess, out TValue1? value, out IEnumerable<IError>? error);
    
}
