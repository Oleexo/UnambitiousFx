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
    /// <param name="result">The result instance.</param>
    /// <returns>The value(s) or throws.</returns>
    public static TValue1 ValueOrThrow<TValue1>(this Result<TValue1> result) where TValue1 : notnull {
        return result.ValueOrThrow(errors => throw errors.ToException());
    }
    
    /// <summary>
    /// Returns contained value(s); otherwise throws exception from factory.
    /// </summary>
    /// <typeparam name="TValue1">Value type 1.</typeparam>
    /// <param name="result">The result instance.</param>
    /// <param name="exceptionFactory">Factory creating exception from errors.</param>
    /// <returns>The value(s) or throws custom exception.</returns>
    public static TValue1 ValueOrThrow<TValue1>(this Result<TValue1> result, Func<IEnumerable<IError>, Exception> exceptionFactory) where TValue1 : notnull {
        return result.Match<TValue1>(value1 => value1, e => throw exceptionFactory(e));
    }
    
}
