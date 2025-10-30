#nullable enable


namespace UnambitiousFx.Core.Results.Extensions.ValueAccess;

public static partial class ResultToNullableExtensions
{
    /// <summary>
    /// Returns the tuple of values as nullable if success; otherwise default.
    /// </summary>
    /// <typeparam name="TValue1">Value type 1.</typeparam>
    /// <typeparam name="TValue2">Value type 2.</typeparam>
    /// <typeparam name="TValue3">Value type 3.</typeparam>
    /// <typeparam name="TValue4">Value type 4.</typeparam>
    /// <typeparam name="TValue5">Value type 5.</typeparam>
    /// <param name="result">The result instance.</param>
    /// <returns>The nullable value or null/default.</returns>
    public static (TValue1, TValue2, TValue3, TValue4, TValue5)? ToNullable<TValue1, TValue2, TValue3, TValue4, TValue5>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5> result) where TValue1 : notnull where TValue2 : notnull where TValue3 : notnull where TValue4 : notnull where TValue5 : notnull {
        if (!result.IsSuccess) {
            return null;
        }
        return result.TryGet(out var value1, out var value2, out var value3, out var value4, out var value5)
                   ? (value1, value2, value3, value4, value5)
                   : default;
    }
    
}
