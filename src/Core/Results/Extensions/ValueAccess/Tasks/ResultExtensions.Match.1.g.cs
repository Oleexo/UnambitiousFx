#nullable enable

using System;
using System.Threading.Tasks;
using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Results.Extensions.ValueAccess.Tasks;

public static partial class ResultMatchExtensions
{
    /// <summary>
    /// Async Match for pattern matching on Result, executing success or failure handler.
    /// </summary>
    public static async Task<TOut> MatchAsync<TOut, TValue1>(this Task<Result<TValue1>> awaitableResult, Func<TValue1, Task<TOut>> success, Func<IEnumerable<IError>, Task<TOut>> failure) where TOut : notnull where TValue1 : notnull {
        var result = await awaitableResult.ConfigureAwait(false);
        return await result.Match(success, failure);
    }
    
}
