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
    /// <param name="result">The result instance.</param>
    /// <param name="fallback1">Fallback value 1.</param>
    /// <param name="fallback2">Fallback value 2.</param>
    /// <returns>The value(s) or fallback(s).</returns>
    public static (TValue1, TValue2) ValueOr<TValue1, TValue2>(this Result<TValue1, TValue2> result, TValue1 fallback1, TValue2 fallback2) where TValue1 : notnull where TValue2 : notnull {
        return result.Match<(TValue1, TValue2)>((value1, value2) => (value1, value2), _ => (fallback1, fallback2));
    }
    
    /// <summary>
    /// Returns contained values when successful; otherwise value(s) from factory.
    /// </summary>
    /// <typeparam name="TValue1">Value type 1.</typeparam>
    /// <typeparam name="TValue2">Value type 2.</typeparam>
    /// <param name="result">The result instance.</param>
    /// <param name="fallbackFactory">Factory producing fallback value(s).</param>
    /// <returns>The value(s) or factory value(s).</returns>
    public static (TValue1, TValue2) ValueOr<TValue1, TValue2>(this Result<TValue1, TValue2> result, Func<(TValue1, TValue2)> fallbackFactory) where TValue1 : notnull where TValue2 : notnull {
        return result.Match<(TValue1, TValue2)>((value1, value2) => (value1, value2), _ => fallbackFactory());
    }
    
}
