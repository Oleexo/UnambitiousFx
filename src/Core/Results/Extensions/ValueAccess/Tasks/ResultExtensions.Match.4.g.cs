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
    public static async Task<TOut> MatchAsync<TOut, TValue1, TValue2, TValue3, TValue4>(this Task<Result<TValue1, TValue2, TValue3, TValue4>> awaitableResult, Func<TValue1, TValue2, TValue3, TValue4, Task<TOut>> success, Func<IEnumerable<IError>, Task<TOut>> failure) where TOut : notnull where TValue1 : notnull where TValue2 : notnull where TValue3 : notnull where TValue4 : notnull {
        var result = await awaitableResult.ConfigureAwait(false);
        return await result.Match(success, failure);
    }
    
}
