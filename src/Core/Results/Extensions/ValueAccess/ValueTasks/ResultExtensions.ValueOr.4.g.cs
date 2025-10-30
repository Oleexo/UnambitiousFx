#nullable enable

using System;
using System.Threading.Tasks;

namespace UnambitiousFx.Core.Results.Extensions.ValueAccess.ValueTasks;

public static partial class ResultValueOrExtensions
{
    /// <summary>
    /// Async ValueOr returning fallback(s) when failure.
    /// </summary>
    public static async ValueTask<(TValue1, TValue2, TValue3, TValue4)> ValueOrAsync<TValue1, TValue2, TValue3, TValue4>(this ValueTask<Result<TValue1, TValue2, TValue3, TValue4>> awaitableResult, TValue1 fallback1, TValue2 fallback2, TValue3 fallback3, TValue4 fallback4) where TValue1 : notnull where TValue2 : notnull where TValue3 : notnull where TValue4 : notnull {
        var result = await awaitableResult.ConfigureAwait(false);
        return result.ValueOr(fallback1, fallback2, fallback3, fallback4);
    }
    
    /// <summary>
    /// Async ValueOr using fallback factory when failure.
    /// </summary>
    public static async ValueTask<(TValue1, TValue2, TValue3, TValue4)> ValueOrAsync<TValue1, TValue2, TValue3, TValue4>(this ValueTask<Result<TValue1, TValue2, TValue3, TValue4>> awaitableResult, Func<(TValue1, TValue2, TValue3, TValue4)> fallbackFactory) where TValue1 : notnull where TValue2 : notnull where TValue3 : notnull where TValue4 : notnull {
        var result = await awaitableResult.ConfigureAwait(false);
        return result.ValueOr(fallbackFactory);
    }
    
}
