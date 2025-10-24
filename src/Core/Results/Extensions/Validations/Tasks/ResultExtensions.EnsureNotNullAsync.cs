namespace UnambitiousFx.Core.Results.Extensions.Validations.Tasks;

public static partial class ResultExtensions {
    /// <summary>
    ///     Ensures a projected inner reference value (from Task) is not null and returns a Task-based Result.
    /// </summary>
    public static async Task<Result<T>> EnsureNotNullAsync<T, TInner>(this Task<Result<T>> awaitableResult,
                                                                      Func<T, TInner?>     selector,
                                                                      string               message,
                                                                      string?              field = null)
        where T : notnull
        where TInner : class {
        var result = await awaitableResult;
        return result.EnsureNotNull(selector, message, field);
    }
}
