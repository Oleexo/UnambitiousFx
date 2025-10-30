#nullable enable

using System;
using System.Threading.Tasks;

namespace UnambitiousFx.Core.Results.Extensions.ValueAccess.Tasks;

public static partial class ResultValueOrExtensions
{
    /// <summary>
    /// Async ValueOr returning fallback(s) when failure.
    /// </summary>
    public static async Task<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)> ValueOrAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>> awaitableResult, TValue1 fallback1, TValue2 fallback2, TValue3 fallback3, TValue4 fallback4, TValue5 fallback5, TValue6 fallback6) where TValue1 : notnull where TValue2 : notnull where TValue3 : notnull where TValue4 : notnull where TValue5 : notnull where TValue6 : notnull {
        var result = await awaitableResult.ConfigureAwait(false);
        return result.ValueOr(fallback1, fallback2, fallback3, fallback4, fallback5, fallback6);
    }
    
    /// <summary>
    /// Async ValueOr using fallback factory when failure.
    /// </summary>
    public static async Task<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)> ValueOrAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>> awaitableResult, Func<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)> fallbackFactory) where TValue1 : notnull where TValue2 : notnull where TValue3 : notnull where TValue4 : notnull where TValue5 : notnull where TValue6 : notnull {
        var result = await awaitableResult.ConfigureAwait(false);
        return result.ValueOr(fallbackFactory);
    }
    
}
