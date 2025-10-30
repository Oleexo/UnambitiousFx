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
    public static async Task<TValue1> ValueOrThrowAsync<TValue1>(this Task<Result<TValue1>> resultTask) where TValue1 : notnull {
        var result = await resultTask.ConfigureAwait(false);
        return result.ValueOrThrow();
    }
    
    /// <summary>
    /// Async ValueOrThrow using exception factory when failure.
    /// </summary>
    public static async Task<TValue1> ValueOrThrowAsync<TValue1>(this Task<Result<TValue1>> resultTask, Func<IEnumerable<IError>, Exception> exceptionFactory) where TValue1 : notnull {
        var result = await resultTask.ConfigureAwait(false);
        return result.ValueOrThrow(exceptionFactory);
    }
    
}
