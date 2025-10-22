namespace UnambitiousFx.Core.Results.Extensions.Validations.Tasks;

public static partial class ResultExtensions {
    /// <summary>
    ///     Ensures a successful string result value (from Task) is neither null nor empty and returns a Task-based Result.
    /// </summary>
    public static async Task<Result<string>> EnsureNotEmptyAsync(this Task<Result<string>> awaitableResult,
                                                                 string                    message = "Value must not be empty.",
                                                                 string?                   field   = null) {
        var result = await awaitableResult;
        return result.EnsureNotEmpty(message, field);
    }

    /// <summary>
    ///     Ensures a successful enumerable result (from Task) is not empty and returns a Task-based Result.
    /// </summary>
    public static async Task<Result<TCollection>> EnsureNotEmptyAsync<TCollection, TItem>(this Task<Result<TCollection>> awaitableResult,
                                                                                          string                         message = "Collection must not be empty.",
                                                                                          string?                        field   = null)
        where TCollection : IEnumerable<TItem> {

        var result = await awaitableResult;
        return result.EnsureNotEmpty<TCollection, TItem>(message, field);
    }
}
