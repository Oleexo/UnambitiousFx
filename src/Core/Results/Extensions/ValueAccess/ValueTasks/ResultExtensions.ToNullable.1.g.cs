#nullable enable

using System;
using System.Threading.Tasks;

namespace UnambitiousFx.Core.Results.Extensions.ValueAccess.ValueTasks;

public static partial class ResultToNullableExtensions
{
    /// <summary>
    /// Async ToNullable returning nullable value(s) when success, null when failure.
    /// </summary>
    public static async ValueTask<TValue1?> ToNullableAsync<TValue1>(this ValueTask<Result<TValue1>> awaitable) where TValue1 : notnull {
        var result = await awaitable.ConfigureAwait(false);
        return result.ToNullable();
    }
    
}
