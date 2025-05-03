namespace UnambitiousFx.Core;

/// Provides extension methods for working with collections of `Result` instances.
public static class ResultExtensions {
    /// Converts a collection of `Result` objects into a single aggregated `Result`.
    /// If any of the `Result` objects in the collection contains an error, the returned `Result` will be
    /// a failure containing an `AggregateError` with all errors collected.
    /// If all `Result` objects are successful, the returned `Result` will be a success.
    /// <param name="results">The collection of `Result` objects to aggregate.</param>
    /// <returns>
    ///     A `Result` that is a success if all `Result` objects in the collection are successful,
    ///     or a failure containing an `AggregateError` if any of the `Result` objects contains an error.
    /// </returns>
    public static Result ToResult(this IEnumerable<Result> results) {
        var errors = new List<IError>();
        foreach (var result in results) {
            if (!result.Ok(out var error)) {
                errors.Add(error);
            }
        }

        return errors.Count != 0
                   ? Result.Failure(new AggregateError(errors))
                   : Result.Success();
    }
}
