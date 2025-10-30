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
    public static async ValueTask<TOut> MatchAsync<TOut, TValue1>(this ValueTask<Result<TValue1>> awaitableResult, Func<TValue1, ValueTask<TOut>> success, Func<IEnumerable<IError>, ValueTask<TOut>> failure) where TOut : notnull where TValue1 : notnull {
        var result = await awaitableResult.ConfigureAwait(false);
        return await result.Match(success, failure);
    }
    
}
