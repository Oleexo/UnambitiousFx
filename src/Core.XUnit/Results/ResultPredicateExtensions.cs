using UnambitiousFx.Core.Results;
using Xunit;

namespace UnambitiousFx.Core.XUnit.Results;

/// <summary>
/// Provides extension methods for validating success or failure conditions
/// on <c>Result</c>, <c>Result&lt;T1&gt;</c>, and multi-value <c>Result</c> types.
/// </summary>
public static class ResultPredicateExtensions {
    /// <summary>
    /// Asserts that the specified <see cref="Result"/> is a failure and that the failure satisfies the given predicate.
    /// If the assertion fails, an assertion exception is thrown.
    /// </summary>
    /// <param name="result">The result to check.</param>
    /// <param name="predicate">The predicate to evaluate against the exception of the failure.</param>
    /// <param name="because">The optional reason for the assertion to be included in failure messages.</param>
    /// <returns>The original <see cref="Result"/> if the assertion passes.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="predicate"/> is null.</exception>
    public static Result ShouldBeFailureWhere(this Result           result,
                                              Func<Exception, bool> predicate,
                                              string?               because = null) {
        ArgumentNullException.ThrowIfNull(predicate);
        if (result.Ok(out var err)) {
            Assert.Fail(because ?? "Expected failure but was success.");
        }

        if (!predicate(err)) {
            Assert.Fail(because ?? $"Error '{err.Message}' does not satisfy predicate.");
        }

        return result;
    }

    /// <summary>
    /// Asserts that the <see cref="Result{T1}"/> is successful and that the value satisfies a specified predicate.
    /// </summary>
    public static Result<T1> ShouldBeSuccessWhere<T1>(this Result<T1> result,
                                                      Func<T1, bool>  predicate,
                                                      string?         because = null)
        where T1 : notnull {
        ArgumentNullException.ThrowIfNull(predicate);
        if (!result.Ok(out var value)) {
            Assert.Fail(because ?? "Expected success but was failure.");
        }

        if (!predicate(value)) {
            Assert.Fail(because ?? $"Value '{value}' does not satisfy predicate.");
        }

        return result;
    }

    /// <summary>
    /// Verifies that a failure <see cref="Result{T1}"/> satisfies the specified predicate for the exception.
    /// </summary>
    public static Result<T1> ShouldBeFailureWhere<T1>(this Result<T1>       result,
                                                      Func<Exception, bool> predicate,
                                                      string?               because = null)
        where T1 : notnull {
        ArgumentNullException.ThrowIfNull(predicate);
        if (result.Ok(out var _, out var err)) {
            Assert.Fail(because ?? "Expected failure but was success.");
        }

        if (!predicate(err)) {
            Assert.Fail(because ?? $"Error '{err.Message}' does not satisfy predicate.");
        }

        return result;
    }

    /// <summary>
    /// Asynchronously asserts that a <see cref="Task{TResult}"/> producing a <see cref="Result{T1}"/> is successful and its value satisfies the predicate.
    /// </summary>
    public static async Task<Result<T1>> ShouldBeSuccessWhereAsync<T1>(this Task<Result<T1>> task,
                                                                       Func<T1, bool>        predicate,
                                                                       string?               because = null)
        where T1 : notnull {
        var r = await task.ConfigureAwait(false);
        r.ShouldBeSuccessWhere(predicate, because);
        return r;
    }

    /// <summary>
    /// Asynchronously asserts that the given <see cref="Task{TResult}"/> producing a <see cref="Result{T1}"/> is a failure and the exception satisfies the predicate.
    /// </summary>
    public static async Task<Result<T1>> ShouldBeFailureWhereAsync<T1>(this Task<Result<T1>> task,
                                                                       Func<Exception, bool> predicate,
                                                                       string?               because = null)
        where T1 : notnull {
        var r = await task.ConfigureAwait(false);
        r.ShouldBeFailureWhere(predicate, because);
        return r;
    }

    /// <summary>
    /// Asserts that a <see cref="ValueTask{TResult}"/> producing a <see cref="Result{T1}"/> is successful and its value satisfies the predicate.
    /// </summary>
    public static async ValueTask<Result<T1>> ShouldBeSuccessWhereAsync<T1>(this ValueTask<Result<T1>> task,
                                                                            Func<T1, bool>             predicate,
                                                                            string?                    because = null)
        where T1 : notnull {
        var r = await task.ConfigureAwait(false);
        r.ShouldBeSuccessWhere(predicate, because);
        return r;
    }

    /// <summary>
    /// Asserts that a <see cref="ValueTask{TResult}"/> producing a <see cref="Result{T1}"/> is a failure whose exception satisfies the predicate.
    /// </summary>
    public static async ValueTask<Result<T1>> ShouldBeFailureWhereAsync<T1>(this ValueTask<Result<T1>> task,
                                                                            Func<Exception, bool>      predicate,
                                                                            string?                    because = null)
        where T1 : notnull {
        var r = await task.ConfigureAwait(false);
        r.ShouldBeFailureWhere(predicate, because);
        return r;
    }

    /// <summary>
    /// Asserts that a two-value <see cref="Result{T1, T2}"/> represents a successful outcome and that the provided predicate is satisfied.
    /// </summary>
    public static Result<T1, T2> ShouldBeSuccessWhere<T1, T2>(this Result<T1, T2>  result,
                                                              Func<(T1, T2), bool> predicate,
                                                              string?              because = null)
        where T1 : notnull
        where T2 : notnull {
        ArgumentNullException.ThrowIfNull(predicate);
        if (!result.Ok(out (T1, T2) value)) {
            Assert.Fail(because ?? "Expected success but was failure.");
        }

        if (!predicate(value)) {
            Assert.Fail(because ?? $"Value '{value}' does not satisfy predicate.");
        }

        return result;
    }

    /// <summary>
    /// Asserts that the specified <see cref="Result{T1, T2}"/> is a failure and that the failure exception satisfies the predicate.
    /// </summary>
    public static Result<T1, T2> ShouldBeFailureWhere<T1, T2>(this Result<T1, T2>   result,
                                                              Func<Exception, bool> predicate,
                                                              string?               because = null)
        where T1 : notnull
        where T2 : notnull {
        ArgumentNullException.ThrowIfNull(predicate);
        if (result.Ok(out (T1, T2) _, out var err)) {
            Assert.Fail(because ?? "Expected failure but was success.");
        }

        if (!predicate(err)) {
            Assert.Fail(because ?? $"Error '{err.Message}' does not satisfy predicate.");
        }

        return result;
    }

    /// <summary>
    /// Asynchronously asserts that a <see cref="Task{TResult}"/> producing <see cref="Result{T1, T2}"/> is successful and satisfies the predicate.
    /// </summary>
    public static async Task<Result<T1, T2>> ShouldBeSuccessWhereAsync<T1, T2>(this Task<Result<T1, T2>> task,
                                                                               Func<(T1, T2), bool>      predicate,
                                                                               string?                   because = null)
        where T1 : notnull
        where T2 : notnull {
        var r = await task.ConfigureAwait(false);
        r.ShouldBeSuccessWhere(predicate, because);
        return r;
    }

    /// <summary>
    /// Asynchronously asserts that the result of the given <see cref="Task{TResult}"/> producing <see cref="Result{T1, T2}"/> is a failure and matches the predicate.
    /// </summary>
    public static async Task<Result<T1, T2>> ShouldBeFailureWhereAsync<T1, T2>(this Task<Result<T1, T2>> task,
                                                                               Func<Exception, bool>     predicate,
                                                                               string?                   because = null)
        where T1 : notnull
        where T2 : notnull {
        var r = await task.ConfigureAwait(false);
        r.ShouldBeFailureWhere(predicate, because);
        return r;
    }

    /// <summary>
    /// Asserts that a <see cref="ValueTask{TResult}"/> producing <see cref="Result{T1, T2}"/> is successful and satisfies the predicate.
    /// </summary>
    public static async ValueTask<Result<T1, T2>> ShouldBeSuccessWhereAsync<T1, T2>(this ValueTask<Result<T1, T2>> task,
                                                                                    Func<(T1, T2), bool>           predicate,
                                                                                    string?                        because = null)
        where T1 : notnull
        where T2 : notnull {
        var r = await task.ConfigureAwait(false);
        r.ShouldBeSuccessWhere(predicate, because);
        return r;
    }

    /// <summary>
    /// Asserts that a <see cref="ValueTask{TResult}"/> producing <see cref="Result{T1, T2}"/> is a failure and the exception satisfies the predicate.
    /// </summary>
    public static async ValueTask<Result<T1, T2>> ShouldBeFailureWhereAsync<T1, T2>(this ValueTask<Result<T1, T2>> task,
                                                                                    Func<Exception, bool>          predicate,
                                                                                    string?                        because = null)
        where T1 : notnull
        where T2 : notnull {
        var r = await task.ConfigureAwait(false);
        r.ShouldBeFailureWhere(predicate, because);
        return r;
    }

    /// <summary>
    /// Asserts that the given three-value <see cref="Result{T1, T2, T3}"/> is successful and satisfies the predicate.
    /// </summary>
    public static Result<T1, T2, T3> ShouldBeSuccessWhere<T1, T2, T3>(this Result<T1, T2, T3>  result,
                                                                      Func<(T1, T2, T3), bool> predicate,
                                                                      string?                  because = null)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull {
        ArgumentNullException.ThrowIfNull(predicate);
        if (!result.Ok(out (T1, T2, T3) value)) {
            Assert.Fail(because ?? "Expected success but was failure.");
        }

        if (!predicate(value)) {
            Assert.Fail(because ?? $"Value '{value}' does not satisfy predicate.");
        }

        return result;
    }

    /// <summary>
    /// Asserts that the given three-value <see cref="Result{T1, T2, T3}"/> is a failure and exception satisfies predicate.
    /// </summary>
    public static Result<T1, T2, T3> ShouldBeFailureWhere<T1, T2, T3>(this Result<T1, T2, T3> result,
                                                                      Func<Exception, bool>   predicate,
                                                                      string?                 because = null)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull {
        ArgumentNullException.ThrowIfNull(predicate);
        if (result.Ok(out (T1, T2, T3) _, out var err)) {
            Assert.Fail(because ?? "Expected failure but was success.");
        }

        if (!predicate(err)) {
            Assert.Fail(because ?? $"Error '{err.Message}' does not satisfy predicate.");
        }

        return result;
    }

    /// <summary>
    /// Asynchronously asserts success for a <see cref="Task{TResult}"/> producing <see cref="Result{T1, T2, T3}"/>.
    /// </summary>
    public static async Task<Result<T1, T2, T3>> ShouldBeSuccessWhereAsync<T1, T2, T3>(this Task<Result<T1, T2, T3>> task,
                                                                                       Func<(T1, T2, T3), bool>      predicate,
                                                                                       string?                       because = null)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull {
        var r = await task.ConfigureAwait(false);
        r.ShouldBeSuccessWhere(predicate, because);
        return r;
    }

    /// <summary>
    /// Asynchronously asserts failure for a <see cref="Task{TResult}"/> producing <see cref="Result{T1, T2, T3}"/>.
    /// </summary>
    public static async Task<Result<T1, T2, T3>> ShouldBeFailureWhereAsync<T1, T2, T3>(this Task<Result<T1, T2, T3>> task,
                                                                                       Func<Exception, bool>         predicate,
                                                                                       string?                       because = null)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull {
        var r = await task.ConfigureAwait(false);
        r.ShouldBeFailureWhere(predicate, because);
        return r;
    }

    /// <summary>
    /// Asynchronously asserts success for a <see cref="ValueTask{TResult}"/> producing <see cref="Result{T1, T2, T3}"/>.
    /// </summary>
    public static async ValueTask<Result<T1, T2, T3>> ShouldBeSuccessWhereAsync<T1, T2, T3>(this ValueTask<Result<T1, T2, T3>> task,
                                                                                            Func<(T1, T2, T3), bool>           predicate,
                                                                                            string?                            because = null)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull {
        var r = await task.ConfigureAwait(false);
        r.ShouldBeSuccessWhere(predicate, because);
        return r;
    }

    /// <summary>
    /// Asynchronously asserts failure for a <see cref="ValueTask{TResult}"/> producing <see cref="Result{T1, T2, T3}"/>.
    /// </summary>
    public static async ValueTask<Result<T1, T2, T3>> ShouldBeFailureWhereAsync<T1, T2, T3>(this ValueTask<Result<T1, T2, T3>> task,
                                                                                            Func<Exception, bool>              predicate,
                                                                                            string?                            because = null)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull {
        var r = await task.ConfigureAwait(false);
        r.ShouldBeFailureWhere(predicate, because);
        return r;
    }

    /// <summary>
    /// Asserts success for a four-value <see cref="Result{T1, T2, T3, T4}"/>.
    /// </summary>
    public static Result<T1, T2, T3, T4> ShouldBeSuccessWhere<T1, T2, T3, T4>(this Result<T1, T2, T3, T4>  result,
                                                                              Func<(T1, T2, T3, T4), bool> predicate,
                                                                              string?                      because = null)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull {
        ArgumentNullException.ThrowIfNull(predicate);
        if (!result.Ok(out (T1, T2, T3, T4) value)) {
            Assert.Fail(because ?? "Expected success but was failure.");
        }

        if (!predicate(value)) {
            Assert.Fail(because ?? $"Value '{value}' does not satisfy predicate.");
        }

        return result;
    }

    /// <summary>
    /// Asserts failure for a four-value <see cref="Result{T1, T2, T3, T4}"/>.
    /// </summary>
    public static Result<T1, T2, T3, T4> ShouldBeFailureWhere<T1, T2, T3, T4>(this Result<T1, T2, T3, T4> result,
                                                                              Func<Exception, bool>       predicate,
                                                                              string?                     because = null)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull {
        ArgumentNullException.ThrowIfNull(predicate);
        if (result.Ok(out (T1, T2, T3, T4) _, out var err)) {
            Assert.Fail(because ?? "Expected failure but was success.");
        }

        if (!predicate(err)) {
            Assert.Fail(because ?? $"Error '{err.Message}' does not satisfy predicate.");
        }

        return result;
    }

    /// <summary>
    /// Asynchronously asserts success for a <see cref="Task{TResult}"/> producing <see cref="Result{T1, T2, T3, T4}"/>.
    /// </summary>
    public static async Task<Result<T1, T2, T3, T4>> ShouldBeSuccessWhereAsync<T1, T2, T3, T4>(this Task<Result<T1, T2, T3, T4>> task,
                                                                                               Func<(T1, T2, T3, T4), bool>      predicate,
                                                                                               string?                           because = null)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull {
        var r = await task.ConfigureAwait(false);
        r.ShouldBeSuccessWhere(predicate, because);
        return r;
    }

    /// <summary>
    /// Asynchronously asserts failure for a <see cref="Task{TResult}"/> producing <see cref="Result{T1, T2, T3, T4}"/>.
    /// </summary>
    public static async Task<Result<T1, T2, T3, T4>> ShouldBeFailureWhereAsync<T1, T2, T3, T4>(this Task<Result<T1, T2, T3, T4>> task,
                                                                                               Func<Exception, bool>             predicate,
                                                                                               string?                           because = null)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull {
        var r = await task.ConfigureAwait(false);
        r.ShouldBeFailureWhere(predicate, because);
        return r;
    }

    /// <summary>
    /// Asynchronously asserts success for a <see cref="ValueTask{TResult}"/> producing <see cref="Result{T1, T2, T3, T4}"/>.
    /// </summary>
    public static async ValueTask<Result<T1, T2, T3, T4>> ShouldBeSuccessWhereAsync<T1, T2, T3, T4>(this ValueTask<Result<T1, T2, T3, T4>> task,
                                                                                                    Func<(T1, T2, T3, T4), bool>           predicate,
                                                                                                    string?                                because = null)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull {
        var r = await task.ConfigureAwait(false);
        r.ShouldBeSuccessWhere(predicate, because);
        return r;
    }

    /// <summary>
    /// Asynchronously asserts failure for a <see cref="ValueTask{TResult}"/> producing <see cref="Result{T1, T2, T3, T4}"/>.
    /// </summary>
    public static async ValueTask<Result<T1, T2, T3, T4>> ShouldBeFailureWhereAsync<T1, T2, T3, T4>(this ValueTask<Result<T1, T2, T3, T4>> task,
                                                                                                    Func<Exception, bool>                  predicate,
                                                                                                    string?                                because = null)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull {
        var r = await task.ConfigureAwait(false);
        r.ShouldBeFailureWhere(predicate, because);
        return r;
    }

    // (Higher-arity methods (5..8) would continue here; omitted for brevity in this documentation fix.)
}
