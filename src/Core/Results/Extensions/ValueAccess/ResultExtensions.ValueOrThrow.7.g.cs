#nullable enable

using System;
using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Results.Extensions.ValueAccess;

public static partial class ResultValueOrThrowExtensions
{
    /// <summary>
    /// Returns contained value(s); throws aggregated exception when failure.
    /// </summary>
    /// <typeparam name="TValue1">Value type 1.</typeparam>
    /// <typeparam name="TValue2">Value type 2.</typeparam>
    /// <typeparam name="TValue3">Value type 3.</typeparam>
    /// <typeparam name="TValue4">Value type 4.</typeparam>
    /// <typeparam name="TValue5">Value type 5.</typeparam>
    /// <typeparam name="TValue6">Value type 6.</typeparam>
    /// <typeparam name="TValue7">Value type 7.</typeparam>
    /// <param name="result">The result instance.</param>
    /// <returns>The value(s) or throws.</returns>
    public static (TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7) ValueOrThrow<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> result) where TValue1 : notnull where TValue2 : notnull where TValue3 : notnull where TValue4 : notnull where TValue5 : notnull where TValue6 : notnull where TValue7 : notnull {
        return result.ValueOrThrow(errors => throw errors.ToException());
    }
    
    /// <summary>
    /// Returns contained value(s); otherwise throws exception from factory.
    /// </summary>
    /// <typeparam name="TValue1">Value type 1.</typeparam>
    /// <typeparam name="TValue2">Value type 2.</typeparam>
    /// <typeparam name="TValue3">Value type 3.</typeparam>
    /// <typeparam name="TValue4">Value type 4.</typeparam>
    /// <typeparam name="TValue5">Value type 5.</typeparam>
    /// <typeparam name="TValue6">Value type 6.</typeparam>
    /// <typeparam name="TValue7">Value type 7.</typeparam>
    /// <param name="result">The result instance.</param>
    /// <param name="exceptionFactory">Factory creating exception from errors.</param>
    /// <returns>The value(s) or throws custom exception.</returns>
    public static (TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7) ValueOrThrow<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> result, Func<IEnumerable<IError>, Exception> exceptionFactory) where TValue1 : notnull where TValue2 : notnull where TValue3 : notnull where TValue4 : notnull where TValue5 : notnull where TValue6 : notnull where TValue7 : notnull {
        return result.Match<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>((value1, value2, value3, value4, value5, value6, value7) => (value1, value2, value3, value4, value5, value6, value7), e => throw exceptionFactory(e));
    }
    
}
