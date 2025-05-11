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

    /// Matches the outcome of an asynchronous
    /// <see cref="IResult{TValue}" />
    /// instance and projects it into a value of type `TOut`.
    /// Executes the provided `success` function if the
    /// <see cref="IResult{TValue}" />
    /// represents a successful state, or the `failure` function
    /// if it represents a failure state.
    /// <param name="awaitableResult">
    ///     The `ValueTask` containing an <see cref="IResult{TValue}" /> representing the outcome of
    ///     an operation.
    /// </param>
    /// <param name="success">
    ///     A function to execute when the <see cref="IResult{TValue}" /> represents a successful outcome.
    ///     Takes the success value as input and returns a `TOut` value.
    /// </param>
    /// <param name="failure">
    ///     A function to execute when the <see cref="IResult{TValue}" /> represents a failure. Takes an
    ///     `IError` as input and returns a `TOut` value.
    /// </param>
    /// <typeparam name="TOut">
    ///     The type of the result returned by the `success` or `failure` function, which must be a
    ///     non-nullable type.
    /// </typeparam>
    /// <typeparam name="TValue">
    ///     The type of the value contained in the `IResult` when it succeeds, which must be a
    ///     non-nullable type.
    /// </typeparam>
    /// <returns>
    ///     A `TOut` value derived from either the `success` or `failure` function, depending on the state of the
    ///     <see cref="IResult{TValue}" />.
    /// </returns>
    public static async ValueTask<TOut> Match<TOut, TValue>(this ValueTask<IResult<TValue>> awaitableResult,
                                                            Func<TValue, TOut>              success,
                                                            Func<IError, TOut>              failure)
        where TOut : notnull
        where TValue : notnull {
        return (await awaitableResult).Match(success, failure);
    }
}
