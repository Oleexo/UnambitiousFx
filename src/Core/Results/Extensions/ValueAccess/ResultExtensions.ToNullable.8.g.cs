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
    /// <typeparam name="TValue6">Value type 6.</typeparam>
    /// <typeparam name="TValue7">Value type 7.</typeparam>
    /// <typeparam name="TValue8">Value type 8.</typeparam>
    /// <param name="result">The result instance.</param>
    /// <returns>The nullable value or null/default.</returns>
    public static (TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)? ToNullable<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> result) where TValue1 : notnull where TValue2 : notnull where TValue3 : notnull where TValue4 : notnull where TValue5 : notnull where TValue6 : notnull where TValue7 : notnull where TValue8 : notnull {
        if (!result.IsSuccess) {
            return null;
        }
        return result.TryGet(out var value1, out var value2, out var value3, out var value4, out var value5, out var value6, out var value7, out var value8)
                   ? (value1, value2, value3, value4, value5, value6, value7, value8)
                   : default;
    }
    
}
