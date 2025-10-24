using UnambitiousFx.Core.Results.Extensions.Transformations;
using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Results.Extensions.Validations;

public static class ResultEnsureNotNullExtensions {
    /// <summary>
    ///     Ensures a projected inner reference value is not null. If null, returns a Failure Result with a ValidationError.
    ///     Original failure results are passed through unchanged.
    /// </summary>
    public static Result<T> EnsureNotNull<T, TInner>(this Result<T>   result,
                                                     Func<T, TInner?> selector,
                                                     string           message,
                                                     string?          field = null)
        where T : notnull
        where TInner : class {
        return result.Then(value => {
            var inner = selector(value);
            if (inner is not null) {
                return Result.Success(value);
            }

            var finalMessage = field is null
                                   ? message
                                   : $"{field}: {message}";
            return Result.Failure<T>(new ValidationError([finalMessage]));
        });
    }
}
