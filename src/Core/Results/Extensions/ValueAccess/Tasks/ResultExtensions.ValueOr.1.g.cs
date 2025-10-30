#nullable enable

using System;
using System.Threading.Tasks;

namespace UnambitiousFx.Core.Results.Extensions.ValueAccess.Tasks;

public static partial class ResultValueOrExtensions
{
    /// <summary>
    /// Async ValueOr returning fallback(s) when failure.
    /// </summary>
    public static async Task<TValue1> ValueOrAsync<TValue1>(this Task<Result<TValue1>> awaitableResult, TValue1 fallback1) where TValue1 : notnull {
        var result = await awaitableResult.ConfigureAwait(false);
        return result.ValueOr(fallback1);
    }
    
    /// <summary>
    /// Async ValueOr using fallback factory when failure.
    /// </summary>
    public static async Task<TValue1> ValueOrAsync<TValue1>(this Task<Result<TValue1>> awaitableResult, Func<TValue1> fallbackFactory) where TValue1 : notnull {
        var result = await awaitableResult.ConfigureAwait(false);
        return result.ValueOr(fallbackFactory);
    }
    
}
