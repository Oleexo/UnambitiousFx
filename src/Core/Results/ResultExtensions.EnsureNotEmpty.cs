using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Results;

/// <summary>
///     Guard helpers that convert a successful Result into a failure (ValidationError) when invariants are violated.
/// </summary>
public static partial class ResultExtensions {
    /// <summary>
    ///     Ensures a successful string result value is neither null nor empty. (Null is unlikely due to notnull constraint,
    ///     but guarded defensively.)
    /// </summary>
    public static Result<string> EnsureNotEmpty(this Result<string> result,
                                                string              message = "Value must not be empty.",
                                                string?             field   = null) {
        if (result.IsFaulted) {
            return result;
        }

        return result.Bind(value => {
            if (string.IsNullOrEmpty(value)) {
                var finalMessage = field is null
                                       ? message
                                       : $"{field}: {message}";
                return Result.Failure<string>(new ValidationError(new[] { finalMessage }));
            }

            return Result.Success(value);
        });
    }

    /// <summary>
    ///     Ensures a successful enumerable result is not empty.
    /// </summary>
    public static Result<TCollection> EnsureNotEmpty<TCollection, TItem>(this Result<TCollection> result,
                                                                         string                   message = "Collection must not be empty.",
                                                                         string?                  field   = null)
        where TCollection : IEnumerable<TItem> {
        if (result.IsFaulted) {
            return result;
        }

        return result.Bind(collection => {
            if (!collection.Any()) {
                var finalMessage = field is null
                                       ? message
                                       : $"{field}: {message}";
                return Result.Failure<TCollection>(new ValidationError([finalMessage]));
            }

            return Result.Success(collection);
        });
    }
}
