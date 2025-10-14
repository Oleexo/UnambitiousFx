using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Results;

/// <summary>
/// Guard helpers that convert a successful Result into a failure (ValidationError) when invariants are violated.
/// </summary>
public static partial class ResultExtensions {
    /// <summary>
    /// Ensures a projected inner reference value is not null. If null, returns a Failure Result with a ValidationError.
    /// Original failure results are passed through unchanged.
    /// </summary>
    public static Result<T> EnsureNotNull<T, TInner>(this Result<T> result,
                                                     Func<T, TInner?> selector,
                                                     string           message,
                                                     string?          field = null)
        where T : notnull
        where TInner : class {
        if (!result.Ok(out var value, out _)) {
            return result; // already faulted
        }

        if (selector == null) throw new ArgumentNullException(nameof(selector));
        var inner = selector(value);
        if (inner is not null) {
            return result;
        }

        var finalMessage = field is null ? message : $"{field}: {message}";
        return Result.Failure<T>(new ValidationError(new[] { finalMessage }));
    }

    /// <summary>
    /// Ensures a successful string result value is neither null nor empty. (Null is unlikely due to notnull constraint, but guarded defensively.)
    /// </summary>
    public static Result<string> EnsureNotEmpty(this Result<string> result,
                                                string              message = "Value must not be empty.",
                                                string?             field   = null) {
        if (!result.Ok(out var value, out _)) {
            return result; // propagate existing failure
        }

        if (string.IsNullOrEmpty(value)) {
            var finalMessage = field is null ? message : $"{field}: {message}";
            return Result.Failure<string>(new ValidationError(new[] { finalMessage }));
        }

        return result;
    }

    /// <summary>
    /// Ensures a successful enumerable result is not empty.
    /// </summary>
    public static Result<TCollection> EnsureNotEmpty<TCollection, TItem>(this Result<TCollection> result,
                                                                         string                  message = "Collection must not be empty.",
                                                                         string?                 field   = null)
        where TCollection : notnull, IEnumerable<TItem> {
        if (!result.Ok(out var collection, out _)) {
            return result;
        }

        if (!collection.Any()) {
            var finalMessage = field is null ? message : $"{field}: {message}";
            return Result.Failure<TCollection>(new ValidationError(new[] { finalMessage }));
        }

        return result;
    }
}
