#nullable enable

using System;

namespace UnambitiousFx.Core.Results.Extensions.ValueAccess;

public static partial class ResultValueOrExtensions
{
    /// <summary>
    /// Returns contained values when successful; otherwise provided fallback(s).
    /// </summary>
    /// <typeparam name="TValue1">Value type 1.</typeparam>
    /// <typeparam name="TValue2">Value type 2.</typeparam>
    /// <typeparam name="TValue3">Value type 3.</typeparam>
    /// <param name="result">The result instance.</param>
    /// <param name="fallback1">Fallback value 1.</param>
    /// <param name="fallback2">Fallback value 2.</param>
    /// <param name="fallback3">Fallback value 3.</param>
    /// <returns>The value(s) or fallback(s).</returns>
    public static (TValue1, TValue2, TValue3) ValueOr<TValue1, TValue2, TValue3>(this Result<TValue1, TValue2, TValue3> result, TValue1 fallback1, TValue2 fallback2, TValue3 fallback3) where TValue1 : notnull where TValue2 : notnull where TValue3 : notnull {
        return result.Match<(TValue1, TValue2, TValue3)>((value1, value2, value3) => (value1, value2, value3), _ => (fallback1, fallback2, fallback3));
    }
    
    /// <summary>
    /// Returns contained values when successful; otherwise value(s) from factory.
    /// </summary>
    /// <typeparam name="TValue1">Value type 1.</typeparam>
    /// <typeparam name="TValue2">Value type 2.</typeparam>
    /// <typeparam name="TValue3">Value type 3.</typeparam>
    /// <param name="result">The result instance.</param>
    /// <param name="fallbackFactory">Factory producing fallback value(s).</param>
    /// <returns>The value(s) or factory value(s).</returns>
    public static (TValue1, TValue2, TValue3) ValueOr<TValue1, TValue2, TValue3>(this Result<TValue1, TValue2, TValue3> result, Func<(TValue1, TValue2, TValue3)> fallbackFactory) where TValue1 : notnull where TValue2 : notnull where TValue3 : notnull {
        return result.Match<(TValue1, TValue2, TValue3)>((value1, value2, value3) => (value1, value2, value3), _ => fallbackFactory());
    }
    
}
