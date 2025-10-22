namespace UnambitiousFx.Core.Results.Extensions.Validations.ValueTasks;

public static partial class ResultExtensions {
    /// <summary>
    ///     Ensures a projected inner reference value (from Task) is not null and returns a Task-based Result.
    /// </summary>
    public static async ValueTask<Result<T>> EnsureNotNullAsync<T, TInner>(this ValueTask<Result<T>> awaitableResult,
                                                                      Func<T, TInner?>     selector,
                                                                      string               message,
                                                                      string?              field = null)
        where T : notnull
        where TInner : class {
        var result = await awaitableResult;
        return result.EnsureNotNull(selector, message, field);
    }
}
