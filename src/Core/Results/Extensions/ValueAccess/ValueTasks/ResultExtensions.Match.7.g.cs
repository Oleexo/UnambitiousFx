#nullable enable

using System;
using System.Threading.Tasks;
using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Results.Extensions.ValueAccess.ValueTasks;

public static partial class ResultMatchExtensions
{
    /// <summary>
    /// Async Match for pattern matching on Result, executing success or failure handler.
    /// </summary>
    public static async ValueTask<TOut> MatchAsync<TOut, TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>> awaitableResult, Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, ValueTask<TOut>> success, Func<IEnumerable<IError>, ValueTask<TOut>> failure) where TOut : notnull where TValue1 : notnull where TValue2 : notnull where TValue3 : notnull where TValue4 : notnull where TValue5 : notnull where TValue6 : notnull where TValue7 : notnull {
        var result = await awaitableResult.ConfigureAwait(false);
        return await result.Match(success, failure);
    }
    
}
