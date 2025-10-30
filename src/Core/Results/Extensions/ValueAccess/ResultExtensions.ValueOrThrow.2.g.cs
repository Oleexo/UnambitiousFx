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
    /// <param name="result">The result instance.</param>
    /// <returns>The value(s) or throws.</returns>
    public static (TValue1, TValue2) ValueOrThrow<TValue1, TValue2>(this Result<TValue1, TValue2> result) where TValue1 : notnull where TValue2 : notnull {
        return result.ValueOrThrow(errors => throw errors.ToException());
    }
    
    /// <summary>
    /// Returns contained value(s); otherwise throws exception from factory.
    /// </summary>
    /// <typeparam name="TValue1">Value type 1.</typeparam>
    /// <typeparam name="TValue2">Value type 2.</typeparam>
    /// <param name="result">The result instance.</param>
    /// <param name="exceptionFactory">Factory creating exception from errors.</param>
    /// <returns>The value(s) or throws custom exception.</returns>
    public static (TValue1, TValue2) ValueOrThrow<TValue1, TValue2>(this Result<TValue1, TValue2> result, Func<IEnumerable<IError>, Exception> exceptionFactory) where TValue1 : notnull where TValue2 : notnull {
        return result.Match<(TValue1, TValue2)>((value1, value2) => (value1, value2), e => throw exceptionFactory(e));
    }
    
}
