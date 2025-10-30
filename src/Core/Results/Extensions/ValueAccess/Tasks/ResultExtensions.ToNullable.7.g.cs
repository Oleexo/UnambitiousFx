#nullable enable

using System;
using System.Threading.Tasks;

namespace UnambitiousFx.Core.Results.Extensions.ValueAccess.Tasks;

public static partial class ResultToNullableExtensions
{
    /// <summary>
    /// Async ToNullable returning nullable value(s) when success, null when failure.
    /// </summary>
    public static async Task<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)?> ToNullableAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>> awaitable) where TValue1 : notnull where TValue2 : notnull where TValue3 : notnull where TValue4 : notnull where TValue5 : notnull where TValue6 : notnull where TValue7 : notnull {
        var result = await awaitable.ConfigureAwait(false);
        return result.ToNullable();
    }
    
}
