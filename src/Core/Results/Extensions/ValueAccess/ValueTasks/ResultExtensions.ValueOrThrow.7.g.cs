#nullable enable

using System;
using System.Threading.Tasks;
using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Results.Extensions.ValueAccess.ValueTasks;

public static partial class ResultValueOrThrowExtensions
{
    /// <summary>
    /// Async ValueOrThrow throwing aggregated exception when failure.
    /// </summary>
    public static async ValueTask<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)> ValueOrThrowAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>> resultTask) where TValue1 : notnull where TValue2 : notnull where TValue3 : notnull where TValue4 : notnull where TValue5 : notnull where TValue6 : notnull where TValue7 : notnull {
        var result = await resultTask.ConfigureAwait(false);
        return result.ValueOrThrow(errors => throw errors.ToException());
    }
    
    /// <summary>
    /// Async ValueOrThrow using exception factory when failure.
    /// </summary>
    public static async ValueTask<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)> ValueOrThrowAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>> resultTask, Func<IEnumerable<IError>, Exception> exceptionFactory) where TValue1 : notnull where TValue2 : notnull where TValue3 : notnull where TValue4 : notnull where TValue5 : notnull where TValue6 : notnull where TValue7 : notnull {
        var result = await resultTask.ConfigureAwait(false);
        return result.ValueOrThrow(exceptionFactory);
    }
    
}
