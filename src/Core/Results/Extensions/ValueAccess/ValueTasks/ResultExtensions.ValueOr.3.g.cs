#nullable enable

using System;
using System.Threading.Tasks;

namespace UnambitiousFx.Core.Results.Extensions.ValueAccess.ValueTasks;

public static partial class ResultValueOrExtensions
{
    /// <summary>
    /// Async ValueOr returning fallback(s) when failure.
    /// </summary>
    public static async ValueTask<(TValue1, TValue2, TValue3)> ValueOrAsync<TValue1, TValue2, TValue3>(this ValueTask<Result<TValue1, TValue2, TValue3>> awaitableResult, TValue1 fallback1, TValue2 fallback2, TValue3 fallback3) where TValue1 : notnull where TValue2 : notnull where TValue3 : notnull {
        var result = await awaitableResult.ConfigureAwait(false);
        return result.ValueOr(fallback1, fallback2, fallback3);
    }
    
    /// <summary>
    /// Async ValueOr using fallback factory when failure.
    /// </summary>
    public static async ValueTask<(TValue1, TValue2, TValue3)> ValueOrAsync<TValue1, TValue2, TValue3>(this ValueTask<Result<TValue1, TValue2, TValue3>> awaitableResult, Func<(TValue1, TValue2, TValue3)> fallbackFactory) where TValue1 : notnull where TValue2 : notnull where TValue3 : notnull {
        var result = await awaitableResult.ConfigureAwait(false);
        return result.ValueOr(fallbackFactory);
    }
    
}
