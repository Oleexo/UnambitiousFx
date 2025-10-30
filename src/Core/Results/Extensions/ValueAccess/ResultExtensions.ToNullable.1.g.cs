#nullable enable


namespace UnambitiousFx.Core.Results.Extensions.ValueAccess;

public static partial class ResultToNullableExtensions
{
    /// <summary>
    /// Returns the value as nullable if success; otherwise default.
    /// </summary>
    /// <typeparam name="TValue1">Value type 1.</typeparam>
    /// <param name="result">The result instance.</param>
    /// <returns>The nullable value or null/default.</returns>
    public static TValue1? ToNullable<TValue1>(this Result<TValue1> result) where TValue1 : notnull {
        return result.TryGet(out var value)
                   ? (TValue1?)value
                   : default;
    }
    
}
