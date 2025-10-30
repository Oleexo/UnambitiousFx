#nullable enable

using System;
using System.Threading.Tasks;

namespace UnambitiousFx.Core.Results.Extensions.ValueAccess.ValueTasks;

public static partial class ResultToNullableExtensions
{
    /// <summary>
    /// Async ToNullable returning nullable value(s) when success, null when failure.
    /// </summary>
    public static async ValueTask<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)?> ToNullableAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>> awaitable) where TValue1 : notnull where TValue2 : notnull where TValue3 : notnull where TValue4 : notnull where TValue5 : notnull where TValue6 : notnull where TValue7 : notnull where TValue8 : notnull {
        var result = await awaitable.ConfigureAwait(false);
        return result.ToNullable();
    }
    
}
