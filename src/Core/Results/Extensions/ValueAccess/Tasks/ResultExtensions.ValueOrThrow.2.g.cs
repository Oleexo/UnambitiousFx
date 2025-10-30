#nullable enable

using System;
using System.Threading.Tasks;
using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Results.Extensions.ValueAccess.Tasks;

public static partial class ResultValueOrThrowExtensions
{
    /// <summary>
    /// Async ValueOrThrow throwing aggregated exception when failure.
    /// </summary>
    public static async Task<(TValue1, TValue2)> ValueOrThrowAsync<TValue1, TValue2>(this Task<Result<TValue1, TValue2>> resultTask) where TValue1 : notnull where TValue2 : notnull {
        var result = await resultTask.ConfigureAwait(false);
        return result.ValueOrThrow(errors => throw errors.ToException());
    }
    
    /// <summary>
    /// Async ValueOrThrow using exception factory when failure.
    /// </summary>
    public static async Task<(TValue1, TValue2)> ValueOrThrowAsync<TValue1, TValue2>(this Task<Result<TValue1, TValue2>> resultTask, Func<IEnumerable<IError>, Exception> exceptionFactory) where TValue1 : notnull where TValue2 : notnull {
        var result = await resultTask.ConfigureAwait(false);
        return result.ValueOrThrow(exceptionFactory);
    }
    
}
