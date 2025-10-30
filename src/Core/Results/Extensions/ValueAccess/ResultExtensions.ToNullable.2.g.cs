#nullable enable


namespace UnambitiousFx.Core.Results.Extensions.ValueAccess;

public static partial class ResultToNullableExtensions
{
    /// <summary>
    /// Returns the tuple of values as nullable if success; otherwise default.
    /// </summary>
    /// <typeparam name="TValue1">Value type 1.</typeparam>
    /// <typeparam name="TValue2">Value type 2.</typeparam>
    /// <param name="result">The result instance.</param>
    /// <returns>The nullable value or null/default.</returns>
    public static (TValue1, TValue2)? ToNullable<TValue1, TValue2>(this Result<TValue1, TValue2> result) where TValue1 : notnull where TValue2 : notnull {
        if (!result.IsSuccess) {
            return null;
        }
        return result.TryGet(out var value1, out var value2)
                   ? (value1, value2)
                   : default;
    }
    
}
