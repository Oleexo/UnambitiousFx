#nullable enable

using System;
using System.Threading.Tasks;

namespace UnambitiousFx.Core.Results.Extensions.ValueAccess.ValueTasks;

public static partial class ResultToNullableExtensions
{
    /// <summary>
    /// Async ToNullable returning nullable value(s) when success, null when failure.
    /// </summary>
    public static async ValueTask<(TValue1, TValue2, TValue3, TValue4)?> ToNullableAsync<TValue1, TValue2, TValue3, TValue4>(this ValueTask<Result<TValue1, TValue2, TValue3, TValue4>> awaitable) where TValue1 : notnull where TValue2 : notnull where TValue3 : notnull where TValue4 : notnull {
        var result = await awaitable.ConfigureAwait(false);
        return result.ToNullable();
    }
    
}
