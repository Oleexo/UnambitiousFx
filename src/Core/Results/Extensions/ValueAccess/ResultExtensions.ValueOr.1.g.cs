#nullable enable

using System;

namespace UnambitiousFx.Core.Results.Extensions.ValueAccess;

public static partial class ResultValueOrExtensions
{
    /// <summary>
    /// Returns contained values when successful; otherwise provided fallback(s).
    /// </summary>
    /// <typeparam name="TValue1">Value type 1.</typeparam>
    /// <param name="result">The result instance.</param>
    /// <param name="fallback1">Fallback value 1.</param>
    /// <returns>The value(s) or fallback(s).</returns>
    public static TValue1 ValueOr<TValue1>(this Result<TValue1> result, TValue1 fallback1) where TValue1 : notnull {
        return result.Match<TValue1>(value1 => value1, _ => fallback1);
    }
    
    /// <summary>
    /// Returns contained values when successful; otherwise value(s) from factory.
    /// </summary>
    /// <typeparam name="TValue1">Value type 1.</typeparam>
    /// <param name="result">The result instance.</param>
    /// <param name="fallbackFactory">Factory producing fallback value(s).</param>
    /// <returns>The value(s) or factory value(s).</returns>
    public static TValue1 ValueOr<TValue1>(this Result<TValue1> result, Func<TValue1> fallbackFactory) where TValue1 : notnull {
        return result.Match<TValue1>(value1 => value1, _ => fallbackFactory());
    }
    
}
