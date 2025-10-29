using System.Diagnostics.CodeAnalysis;
using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Results;

public partial class Result
{
    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> Success<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(TValue1 value1, TValue2 value2, TValue3 value3, TValue4 value4, TValue5 value5, TValue6 value6, TValue7 value7, TValue8 value8) where TValue1 : notnull where TValue2 : notnull where TValue3 : notnull where TValue4 : notnull where TValue5 : notnull where TValue6 : notnull where TValue7 : notnull where TValue8 : notnull {
        return new SuccessResult<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(value1, value2, value3, value4, value5, value6, value7, value8);
    }
    
    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> Failure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(Exception error) where TValue1 : notnull where TValue2 : notnull where TValue3 : notnull where TValue4 : notnull where TValue5 : notnull where TValue6 : notnull where TValue7 : notnull where TValue8 : notnull {
        return new FailureResult<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(error);
    }
    
    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> Failure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(IError error) where TValue1 : notnull where TValue2 : notnull where TValue3 : notnull where TValue4 : notnull where TValue5 : notnull where TValue6 : notnull where TValue7 : notnull where TValue8 : notnull {
        var r = new FailureResult<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(error.Exception ?? new Exception(error.Message), false);
        r.AddReason(error);
        foreach (var kv in error.Metadata) {
            r.AddMetadata(kv.Key, kv.Value);
        }
        return r;
    }
    
    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> Failure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(string message) where TValue1 : notnull where TValue2 : notnull where TValue3 : notnull where TValue4 : notnull where TValue5 : notnull where TValue6 : notnull where TValue7 : notnull where TValue8 : notnull {
        return new FailureResult<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(new Exception(message));
    }
    
    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> Failure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(IEnumerable<IError> errors) where TValue1 : notnull where TValue2 : notnull where TValue3 : notnull where TValue4 : notnull where TValue5 : notnull where TValue6 : notnull where TValue7 : notnull where TValue8 : notnull {
        return new FailureResult<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(errors);
    }
    
}
/// <summary>
/// Represents the result of an operation that can succeed with 8 value(s) or fail with an exception.
/// </summary>
/// <typeparam name="TValue1">The type of the first value.</typeparam>
/// <typeparam name="TValue2">The type of the second value.</typeparam>
/// <typeparam name="TValue3">The type of the third value.</typeparam>
/// <typeparam name="TValue4">The type of the fourth value.</typeparam>
/// <typeparam name="TValue5">The type of the fifth value.</typeparam>
/// <typeparam name="TValue6">The type of the sixth value.</typeparam>
/// <typeparam name="TValue7">The type of the seventh value.</typeparam>
/// <typeparam name="TValue8">The type of the eighth value.</typeparam>
public abstract class Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> : BaseResult
    where TValue1 : notnull
    where TValue2 : notnull
    where TValue3 : notnull
    where TValue4 : notnull
    where TValue5 : notnull
    where TValue6 : notnull
    where TValue7 : notnull
    where TValue8 : notnull
{
    /// <summary>
    /// Pattern matches the result, executing the appropriate action.
    /// </summary>
    /// <param name="success">Action to execute if the result is successful</param>
    /// <param name="failure">Action to execute if the result is a failure</param>
    public abstract void Match(Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> success, Action<IEnumerable<IError>> failure);
    
    /// <summary>
    /// Pattern matches the result, returning a value from the appropriate function.
    /// </summary>
    /// <typeparam name="TOut">The type of value to return</typeparam>
    /// <param name="success">Function to invoke if the result is successful</param>
    /// <param name="failure">Function to invoke if the result is a failure</param>
    /// <returns>The result of invoking the appropriate function</returns>
    public abstract TOut Match<TOut>(Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TOut> success, Func<IEnumerable<IError>, TOut> failure);
    
    /// <summary>
    /// Executes the action if the result is successful.
    /// </summary>
    /// <param name="action">Action to execute with the success values</param>
    public abstract void IfSuccess(Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> action);
    
    /// <summary>
    /// Attempts to extract the success values and error.
    /// </summary>
    /// <param name="value1">The first value if successful</param>
    /// <param name="value2">The second value if successful</param>
    /// <param name="value3">The third value if successful</param>
    /// <param name="value4">The fourth value if successful</param>
    /// <param name="value5">The fifth value if successful</param>
    /// <param name="value6">The sixth value if successful</param>
    /// <param name="value7">The seventh value if successful</param>
    /// <param name="value8">The eighth value if successful</param>
    /// <param name="error">The exception if failed</param>
    /// <returns>True if successful, false otherwise</returns>
    public abstract bool TryGet([NotNullWhen(true)] out TValue1? value1, [NotNullWhen(true)] out TValue2? value2, [NotNullWhen(true)] out TValue3? value3, [NotNullWhen(true)] out TValue4? value4, [NotNullWhen(true)] out TValue5? value5, [NotNullWhen(true)] out TValue6? value6, [NotNullWhen(true)] out TValue7? value7, [NotNullWhen(true)] out TValue8? value8, [NotNullWhen(false)] out IEnumerable<IError>? error);
    
    /// <summary>
    /// Attempts to extract the success values.
    /// </summary>
    /// <param name="value1">The first value if successful</param>
    /// <param name="value2">The second value if successful</param>
    /// <param name="value3">The third value if successful</param>
    /// <param name="value4">The fourth value if successful</param>
    /// <param name="value5">The fifth value if successful</param>
    /// <param name="value6">The sixth value if successful</param>
    /// <param name="value7">The seventh value if successful</param>
    /// <param name="value8">The eighth value if successful</param>
    /// <returns>True if successful, false otherwise</returns>
    public abstract bool TryGet([NotNullWhen(true)] out TValue1? value1, [NotNullWhen(true)] out TValue2? value2, [NotNullWhen(true)] out TValue3? value3, [NotNullWhen(true)] out TValue4? value4, [NotNullWhen(true)] out TValue5? value5, [NotNullWhen(true)] out TValue6? value6, [NotNullWhen(true)] out TValue7? value7, [NotNullWhen(true)] out TValue8? value8);
    
    /// <summary>
    /// Deconstructs the result into its components.
    /// </summary>
    /// <param name="isSuccess">Whether the result is successful</param>
    /// <param name="value">The success value(s) if successful</param>
    /// <param name="error">The exception if failed</param>
    public abstract void Deconstruct(out bool isSuccess, out (TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)? value, out IEnumerable<IError>? error);
    
}
