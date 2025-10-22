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
    /// Asserts that a <see cref="ValueTask{TResult}"/> producing a <see cref="Result{T1}"/> is a failure and the exception satisfies the predicate.
    /// </summary>
    public static async ValueTask<Result<T1>> ShouldBeFailureWhereAsync<T1>(this ValueTask<Result<T1>> task,
                                                                                                        Func<Exception, bool>                     predicate,
                                                                                                        string?                                because = null)
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
        if (!result.Ok(out var v1, out var v2)) {
            Assert.Fail(because ?? "Expected success but was failure.");
        }

        var value = (v1, v2);
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
        if (result.Ok(out _, out _, out var err)) {
            Assert.Fail(because ?? "Expected failure but was success.");
        }

        if (!predicate(err!)) {
            Assert.Fail(because ?? $"Error '{err!.Message}' does not satisfy predicate.");
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
        if (!result.Ok(out var v1, out var v2, out var v3)) {
            Assert.Fail(because ?? "Expected success but was failure.");
        }

        var value = (v1, v2, v3);
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
        if (result.Ok(out _, out _, out _, out var err)) {
            Assert.Fail(because ?? "Expected failure but was success.");
        }

        if (!predicate(err!)) {
            Assert.Fail(because ?? $"Error '{err!.Message}' does not satisfy predicate.");
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
                                                                                       Func<Exception, bool>     predicate,
                                                                                       string?                   because = null)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull {
        var r = await task.ConfigureAwait(false);
        r.ShouldBeFailureWhere(predicate, because);
        return r;
    }

    /// <summary>
    /// Asserts that a <see cref="ValueTask{TResult}"/> producing <see cref="Result{T1, T2, T3}"/> is successful and satisfies the predicate.
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
    /// Asserts that a <see cref="ValueTask{TResult}"/> producing <see cref="Result{T1, T2, T3}"/> is a failure and the exception satisfies the predicate.
    /// </summary>
    public static async ValueTask<Result<T1, T2, T3>> ShouldBeFailureWhereAsync<T1, T2, T3>(this ValueTask<Result<T1, T2, T3>> task,
                                                                                        Func<Exception, bool>                 predicate,
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
        if (!result.Ok(out var v1, out var v2, out var v3, out var v4)) {
            Assert.Fail(because ?? "Expected success but was failure.");
        }

        var value = (v1, v2, v3, v4);
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
        if (result.Ok(out _, out _, out _, out _, out var err)) {
            Assert.Fail(because ?? "Expected failure but was success.");
        }

        if (!predicate(err!)) {
            Assert.Fail(because ?? $"Error '{err!.Message}' does not satisfy predicate.");
        }

        return result;
    }

    /// <summary>
    /// Asynchronously asserts that a <see cref="Task{TResult}"/> producing <see cref="Result{T1, T2, T3, T4}"/> is successful and satisfies the predicate.
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
    /// Asynchronously asserts that the result of the given <see cref="Task{TResult}"/> producing <see cref="Result{T1, T2, T3, T4}"/> is a failure and matches the predicate.
    /// </summary>
    public static async Task<Result<T1, T2, T3, T4>> ShouldBeFailureWhereAsync<T1, T2, T3, T4>(this Task<Result<T1, T2, T3, T4>> task,
                                                                                               Func<Exception, bool>               predicate,
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
    /// Asserts that a <see cref="ValueTask{TResult}"/> producing <see cref="Result{T1, T2, T3, T4}"/> is successful and satisfies the predicate.
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
    /// Asserts that a <see cref="ValueTask{TResult}"/> producing <see cref="Result{T1, T2, T3, T4}"/> is a failure and the exception satisfies the predicate.
    /// </summary>
    public static async ValueTask<Result<T1, T2, T3, T4>> ShouldBeFailureWhereAsync<T1, T2, T3, T4>(this ValueTask<Result<T1, T2, T3, T4>> task,
                                                                                                        Func<Exception, bool>                     predicate,
                                                                                                        string?                                because = null)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull {
        var r = await task.ConfigureAwait(false);
        r.ShouldBeFailureWhere(predicate, because);
        return r;
    }

    /// <summary>
    /// Asserts success for a five-value <see cref="Result{T1,T2,T3,T4,T5}"/>.
    /// </summary>
    public static Result<T1, T2, T3, T4, T5> ShouldBeSuccessWhere<T1, T2, T3, T4, T5>(this Result<T1, T2, T3, T4, T5>  result,
                                                                                       Func<(T1, T2, T3, T4, T5), bool> predicate,
                                                                                       string?                          because = null)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull {
        ArgumentNullException.ThrowIfNull(predicate);
        if (!result.Ok(out var v1, out var v2, out var v3, out var v4, out var v5)) {
            Assert.Fail(because ?? "Expected success but was failure.");
        }

        var value = (v1, v2, v3, v4, v5);
        if (!predicate(value)) {
            Assert.Fail(because ?? $"Value '{value}' does not satisfy predicate.");
        }

        return result;
    }

    /// <summary>
    /// Asserts failure for a five-value <see cref="Result{T1,T2,T3,T4,T5}"/>.
    /// </summary>
    public static Result<T1, T2, T3, T4, T5> ShouldBeFailureWhere<T1, T2, T3, T4, T5>(this Result<T1, T2, T3, T4, T5> result,
                                                                                       Func<Exception, bool>           predicate,
                                                                                       string?                         because = null)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull {
        ArgumentNullException.ThrowIfNull(predicate);
        if (result.Ok(out _, out _, out _, out _, out _, out var err)) {
            Assert.Fail(because ?? "Expected failure but was success.");
        }

        if (!predicate(err!)) {
            Assert.Fail(because ?? $"Error '{err!.Message}' does not satisfy predicate.");
        }

        return result;
    }

    /// <summary>
    /// Asynchronously asserts that a five-value <see cref="Result{T1,T2,T3,T4,T5}"/> is successful and that its value tuple satisfies the provided predicate.
    /// </summary>
    /// <typeparam name="T1">The first value type.</typeparam>
    /// <typeparam name="T2">The second value type.</typeparam>
    /// <typeparam name="T3">The third value type.</typeparam>
    /// <typeparam name="T4">The fourth value type.</typeparam>
    /// <typeparam name="T5">The fifth value type.</typeparam>
    /// <param name="task">The task producing the result to assert.</param>
    /// <param name="predicate">Predicate evaluated against the (<typeparamref name="T1"/>, <typeparamref name="T2"/>, <typeparamref name="T3"/>, <typeparamref name="T4"/>, <typeparamref name="T5"/>) tuple.</param>
    /// <param name="because">Optional reason included in failure messages.</param>
    /// <returns>The asserted <see cref="Result{T1,T2,T3,T4,T5}"/>.</returns>
    public static async Task<Result<T1, T2, T3, T4, T5>> ShouldBeSuccessWhereAsync<T1, T2, T3, T4, T5>(this Task<Result<T1, T2, T3, T4, T5>> task,
                                                                                                       Func<(T1, T2, T3, T4, T5), bool>      predicate,
                                                                                                       string?                               because = null)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull {
        var r = await task.ConfigureAwait(false);
        r.ShouldBeSuccessWhere(predicate, because);
        return r;
    }

    /// <summary>
    /// Asynchronously asserts that a five-value <see cref="Result{T1,T2,T3,T4,T5}"/> is a failure and its exception satisfies the provided predicate.
    /// </summary>
    /// <typeparam name="T1">The first value type.</typeparam>
    /// <typeparam name="T2">The second value type.</typeparam>
    /// <typeparam name="T3">The third value type.</typeparam>
    /// <typeparam name="T4">The fourth value type.</typeparam>
    /// <typeparam name="T5">The fifth value type.</typeparam>
    /// <param name="task">The task producing the result to assert.</param>
    /// <param name="predicate">Predicate evaluated against the exception of the failure.</param>
    /// <param name="because">Optional reason included in failure messages.</param>
    /// <returns>The asserted <see cref="Result{T1,T2,T3,T4,T5}"/>.</returns>
    public static async Task<Result<T1, T2, T3, T4, T5>> ShouldBeFailureWhereAsync<T1, T2, T3, T4, T5>(this Task<Result<T1, T2, T3, T4, T5>> task,
                                                                                                       Func<Exception, bool>                 predicate,
                                                                                                       string?                               because = null)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull {
        var r = await task.ConfigureAwait(false);
        r.ShouldBeFailureWhere(predicate, because);
        return r;
    }

    /// <summary>
    /// Asynchronously asserts (ValueTask) that a five-value <see cref="Result{T1,T2,T3,T4,T5}"/> is successful and its value tuple satisfies the provided predicate.
    /// </summary>
    /// <typeparam name="T1">The first value type.</typeparam>
    /// <typeparam name="T2">The second value type.</typeparam>
    /// <typeparam name="T3">The third value type.</typeparam>
    /// <typeparam name="T4">The fourth value type.</typeparam>
    /// <typeparam name="T5">The fifth value type.</typeparam>
    /// <param name="task">The value task producing the result to assert.</param>
    /// <param name="predicate">Predicate evaluated against the value tuple.</param>
    /// <param name="because">Optional reason included in failure messages.</param>
    /// <returns>The asserted <see cref="Result{T1,T2,T3,T4,T5}"/>.</returns>
    public static async ValueTask<Result<T1, T2, T3, T4, T5>> ShouldBeSuccessWhereAsync<T1, T2, T3, T4, T5>(this ValueTask<Result<T1, T2, T3, T4, T5>> task,
                                                                                                            Func<(T1, T2, T3, T4, T5), bool>           predicate,
                                                                                                            string?                                    because = null)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull {
        var r = await task.ConfigureAwait(false);
        r.ShouldBeSuccessWhere(predicate, because);
        return r;
    }

    /// <summary>
    /// Asynchronously asserts (ValueTask) that a five-value <see cref="Result{T1,T2,T3,T4,T5}"/> is a failure and its exception satisfies the provided predicate.
    /// </summary>
    /// <typeparam name="T1">The first value type.</typeparam>
    /// <typeparam name="T2">The second value type.</typeparam>
    /// <typeparam name="T3">The third value type.</typeparam>
    /// <typeparam name="T4">The fourth value type.</typeparam>
    /// <typeparam name="T5">The fifth value type.</typeparam>
    /// <param name="task">The value task producing the result to assert.</param>
    /// <param name="predicate">Predicate evaluated against the exception.</param>
    /// <param name="because">Optional reason included in failure messages.</param>
    /// <returns>The asserted <see cref="Result{T1,T2,T3,T4,T5}"/>.</returns>
    public static async ValueTask<Result<T1, T2, T3, T4, T5>> ShouldBeFailureWhereAsync<T1, T2, T3, T4, T5>(this ValueTask<Result<T1, T2, T3, T4, T5>> task,
                                                                                                            Func<Exception, bool>                     predicate,
                                                                                                            string?                                    because = null)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull {
        var r = await task.ConfigureAwait(false);
        r.ShouldBeFailureWhere(predicate, because);
        return r;
    }

    /// <summary>
    /// Asserts success for a six-value <see cref="Result{T1,T2,T3,T4,T5,T6}"/>.
    /// </summary>
    public static Result<T1, T2, T3, T4, T5, T6> ShouldBeSuccessWhere<T1, T2, T3, T4, T5, T6>(this Result<T1, T2, T3, T4, T5, T6>  result,
                                                                                                Func<(T1, T2, T3, T4, T5, T6), bool> predicate,
                                                                                                string?                              because = null)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull {
        ArgumentNullException.ThrowIfNull(predicate);
        if (!result.Ok(out var v1, out var v2, out var v3, out var v4, out var v5, out var v6)) {
            Assert.Fail(because ?? "Expected success but was failure.");
        }

        var value = (v1, v2, v3, v4, v5, v6);
        if (!predicate(value)) {
            Assert.Fail(because ?? $"Value '{value}' does not satisfy predicate.");
        }

        return result;
    }

    /// <summary>
    /// Asserts failure for a six-value <see cref="Result{T1,T2,T3,T4,T5,T6}"/>.
    /// </summary>
    public static Result<T1, T2, T3, T4, T5, T6> ShouldBeFailureWhere<T1, T2, T3, T4, T5, T6>(this Result<T1, T2, T3, T4, T5, T6> result,
                                                                                                Func<Exception, bool>               predicate,
                                                                                                string?                            because = null)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull {
        ArgumentNullException.ThrowIfNull(predicate);
        if (result.Ok(out _, out _, out _, out _, out _, out _, out var err)) {
            Assert.Fail(because ?? "Expected failure but was success.");
        }

        if (!predicate(err!)) {
            Assert.Fail(because ?? $"Error '{err!.Message}' does not satisfy predicate.");
        }

        return result;
    }

    /// <summary>
    /// Asynchronously asserts that a six-value <see cref="Result{T1,T2,T3,T4,T5,T6}"/> is successful and its value tuple satisfies the provided predicate.
    /// </summary>
    public static async Task<Result<T1, T2, T3, T4, T5, T6>> ShouldBeSuccessWhereAsync<T1, T2, T3, T4, T5, T6>(this Task<Result<T1, T2, T3, T4, T5, T6>> task,
                                                                                                                Func<(T1, T2, T3, T4, T5, T6), bool>      predicate,
                                                                                                                string?                                   because = null)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull {
        var r = await task.ConfigureAwait(false);
        r.ShouldBeSuccessWhere(predicate, because);
        return r;
    }

    /// <summary>
    /// Asynchronously asserts that a six-value <see cref="Result{T1,T2,T3,T4,T5,T6}"/> is a failure and its exception satisfies the provided predicate.
    /// </summary>
    public static async Task<Result<T1, T2, T3, T4, T5, T6>> ShouldBeFailureWhereAsync<T1, T2, T3, T4, T5, T6>(this Task<Result<T1, T2, T3, T4, T5, T6>> task,
                                                                                                                Func<Exception, bool>                     predicate,
                                                                                                                string?                                   because = null)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull {
        var r = await task.ConfigureAwait(false);
        r.ShouldBeFailureWhere(predicate, because);
        return r;
    }

    /// <summary>
    /// Asynchronously asserts (ValueTask) that a six-value <see cref="Result{T1,T2,T3,T4,T5,T6}"/> is successful and its value tuple satisfies the provided predicate.
    /// </summary>
    public static async ValueTask<Result<T1, T2, T3, T4, T5, T6>> ShouldBeSuccessWhereAsync<T1, T2, T3, T4, T5, T6>(this ValueTask<Result<T1, T2, T3, T4, T5, T6>> task,
                                                                                                                       Func<(T1, T2, T3, T4, T5, T6), bool>           predicate,
                                                                                                                       string?                                        because = null)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull {
        var r = await task.ConfigureAwait(false);
        r.ShouldBeSuccessWhere(predicate, because);
        return r;
    }

    /// <summary>
    /// Asynchronously asserts (ValueTask) that a six-value <see cref="Result{T1,T2,T3,T4,T5,T6}"/> is a failure and its exception satisfies the provided predicate.
    /// </summary>
    public static async ValueTask<Result<T1, T2, T3, T4, T5, T6>> ShouldBeFailureWhereAsync<T1, T2, T3, T4, T5, T6>(this ValueTask<Result<T1, T2, T3, T4, T5, T6>> task,
                                                                                                                       Func<Exception, bool>                         predicate,
                                                                                                                       string?                                        because = null)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull {
        var r = await task.ConfigureAwait(false);
        r.ShouldBeFailureWhere(predicate, because);
        return r;
    }

    /// <summary>
    /// Asserts success for a seven-value <see cref="Result{T1,T2,T3,T4,T5,T6,T7}"/>.
    /// </summary>
    public static Result<T1, T2, T3, T4, T5, T6, T7> ShouldBeSuccessWhere<T1, T2, T3, T4, T5, T6, T7>(this Result<T1, T2, T3, T4, T5, T6, T7>  result,
                                                                                                        Func<(T1, T2, T3, T4, T5, T6, T7), bool> predicate,
                                                                                                        string?                                   because = null)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
        where T7 : notnull {
        ArgumentNullException.ThrowIfNull(predicate);
        if (!result.Ok(out var v1, out var v2, out var v3, out var v4, out var v5, out var v6, out var v7)) {
            Assert.Fail(because ?? "Expected success but was failure.");
        }

        var value = (v1, v2, v3, v4, v5, v6, v7);
        if (!predicate(value)) {
            Assert.Fail(because ?? $"Value '{value}' does not satisfy predicate.");
        }

        return result;
    }

    /// <summary>
    /// Asserts failure for a seven-value <see cref="Result{T1,T2,T3,T4,T5,T6,T7}"/>.
    /// </summary>
    public static Result<T1, T2, T3, T4, T5, T6, T7> ShouldBeFailureWhere<T1, T2, T3, T4, T5, T6, T7>(this Result<T1, T2, T3, T4, T5, T6, T7> result,
                                                                                                        Func<Exception, bool>                   predicate,
                                                                                                        string?                                 because = null)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
        where T7 : notnull {
        ArgumentNullException.ThrowIfNull(predicate);
        if (result.Ok(out _, out _, out _, out _, out _, out _, out _, out var err)) {
            Assert.Fail(because ?? "Expected failure but was success.");
        }

        if (!predicate(err!)) {
            Assert.Fail(because ?? $"Error '{err!.Message}' does not satisfy predicate.");
        }

        return result;
    }

    /// <summary>
    /// Asynchronously asserts that a seven-value <see cref="Result{T1,T2,T3,T4,T5,T6,T7}"/> is successful and its value tuple satisfies the provided predicate.
    /// </summary>
    public static async Task<Result<T1, T2, T3, T4, T5, T6, T7>> ShouldBeSuccessWhereAsync<T1, T2, T3, T4, T5, T6, T7>(this Task<Result<T1, T2, T3, T4, T5, T6, T7>> task,
                                                                                                                          Func<(T1, T2, T3, T4, T5, T6, T7), bool>      predicate,
                                                                                                                          string?                                      because = null)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
        where T7 : notnull {
        var r = await task.ConfigureAwait(false);
        r.ShouldBeSuccessWhere(predicate, because);
        return r;
    }

    /// <summary>
    /// Asynchronously asserts that a seven-value <see cref="Result{T1,T2,T3,T4,T5,T6,T7}"/> is a failure and its exception satisfies the provided predicate.
    /// </summary>
    public static async Task<Result<T1, T2, T3, T4, T5, T6, T7>> ShouldBeFailureWhereAsync<T1, T2, T3, T4, T5, T6, T7>(this Task<Result<T1, T2, T3, T4, T5, T6, T7>> task,
                                                                                                                          Func<Exception, bool>                         predicate,
                                                                                                                          string?                                      because = null)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
        where T7 : notnull {
        var r = await task.ConfigureAwait(false);
        r.ShouldBeFailureWhere(predicate, because);
        return r;
    }

    /// <summary>
    /// Asynchronously asserts (ValueTask) that a seven-value <see cref="Result{T1,T2,T3,T4,T5,T6,T7}"/> is successful and its value tuple satisfies the provided predicate.
    /// </summary>
    public static async ValueTask<Result<T1, T2, T3, T4, T5, T6, T7>> ShouldBeSuccessWhereAsync<T1, T2, T3, T4, T5, T6, T7>(this ValueTask<Result<T1, T2, T3, T4, T5, T6, T7>> task,
                                                                                                                               Func<(T1, T2, T3, T4, T5, T6, T7), bool>           predicate,
                                                                                                                               string?                                            because = null)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
        where T7 : notnull {
        var r = await task.ConfigureAwait(false);
        r.ShouldBeSuccessWhere(predicate, because);
        return r;
    }

    /// <summary>
    /// Asynchronously asserts (ValueTask) that a seven-value <see cref="Result{T1,T2,T3,T4,T5,T6,T7}"/> is a failure and its exception satisfies the provided predicate.
    /// </summary>
    public static async ValueTask<Result<T1, T2, T3, T4, T5, T6, T7>> ShouldBeFailureWhereAsync<T1, T2, T3, T4, T5, T6, T7>(this ValueTask<Result<T1, T2, T3, T4, T5, T6, T7>> task,
                                                                                                                               Func<Exception, bool>                             predicate,
                                                                                                                               string?                                            because = null)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
        where T7 : notnull {
        var r = await task.ConfigureAwait(false);
        r.ShouldBeFailureWhere(predicate, because);
        return r;
    }

    /// <summary>
    /// Asserts success for an eight-value <see cref="Result{T1,T2,T3,T4,T5,T6,T7,T8}"/>.
    /// </summary>
    public static Result<T1, T2, T3, T4, T5, T6, T7, T8> ShouldBeSuccessWhere<T1, T2, T3, T4, T5, T6, T7, T8>(this Result<T1, T2, T3, T4, T5, T6, T7, T8>  result,
                                                                                                                Func<(T1, T2, T3, T4, T5, T6, T7, T8), bool> predicate,
                                                                                                                string?                                      because = null)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
        where T7 : notnull
        where T8 : notnull {
        ArgumentNullException.ThrowIfNull(predicate);
        if (!result.Ok(out var v1, out var v2, out var v3, out var v4, out var v5, out var v6, out var v7, out var v8)) {
            Assert.Fail(because ?? "Expected success but was failure.");
        }

        var value = (v1, v2, v3, v4, v5, v6, v7, v8);
        if (!predicate(value)) {
            Assert.Fail(because ?? $"Value '{value}' does not satisfy predicate.");
        }

        return result;
    }

    /// <summary>
    /// Asserts failure for an eight-value <see cref="Result{T1,T2,T3,T4,T5,T6,T7,T8}"/>.
    /// </summary>
    public static Result<T1, T2, T3, T4, T5, T6, T7, T8> ShouldBeFailureWhere<T1, T2, T3, T4, T5, T6, T7, T8>(this Result<T1, T2, T3, T4, T5, T6, T7, T8> result,
                                                                                                                Func<Exception, bool>                     predicate,
                                                                                                                string?                                   because = null)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
        where T7 : notnull
        where T8 : notnull {
        ArgumentNullException.ThrowIfNull(predicate);
        if (result.Ok(out _, out _, out _, out _, out _, out _, out _, out _, out var err)) {
            Assert.Fail(because ?? "Expected failure but was success.");
        }

        if (!predicate(err!)) {
            Assert.Fail(because ?? $"Error '{err!.Message}' does not satisfy predicate.");
        }

        return result;
    }

    /// <summary>
    /// Asynchronously asserts that an eight-value <see cref="Result{T1,T2,T3,T4,T5,T6,T7,T8}"/> is successful and its value tuple satisfies the provided predicate.
    /// </summary>
    public static async Task<Result<T1, T2, T3, T4, T5, T6, T7, T8>> ShouldBeSuccessWhereAsync<T1, T2, T3, T4, T5, T6, T7, T8>(this Task<Result<T1, T2, T3, T4, T5, T6, T7, T8>> task,
                                                                                                                                    Func<(T1, T2, T3, T4, T5, T6, T7, T8), bool>      predicate,
                                                                                                                                    string?                                         because = null)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
        where T7 : notnull
        where T8 : notnull {
        var r = await task.ConfigureAwait(false);
        r.ShouldBeSuccessWhere(predicate, because);
        return r;
    }

    /// <summary>
    /// Asynchronously asserts that an eight-value <see cref="Result{T1,T2,T3,T4,T5,T6,T7,T8}"/> is a failure and its exception satisfies the provided predicate.
    /// </summary>
    public static async Task<Result<T1, T2, T3, T4, T5, T6, T7, T8>> ShouldBeFailureWhereAsync<T1, T2, T3, T4, T5, T6, T7, T8>(this Task<Result<T1, T2, T3, T4, T5, T6, T7, T8>> task,
                                                                                                                                    Func<Exception, bool>                            predicate,
                                                                                                                                    string?                                         because = null)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
        where T7 : notnull
        where T8 : notnull {
        var r = await task.ConfigureAwait(false);
        r.ShouldBeFailureWhere(predicate, because);
        return r;
    }

    /// <summary>
    /// Asynchronously asserts (ValueTask) that an eight-value <see cref="Result{T1,T2,T3,T4,T5,T6,T7,T8}"/> is successful and its value tuple satisfies the provided predicate.
    /// </summary>
    public static async ValueTask<Result<T1, T2, T3, T4, T5, T6, T7, T8>> ShouldBeSuccessWhereAsync<T1, T2, T3, T4, T5, T6, T7, T8>(this ValueTask<Result<T1, T2, T3, T4, T5, T6, T7, T8>> task,
                                                                                                                                       Func<(T1, T2, T3, T4, T5, T6, T7, T8), bool>           predicate,
                                                                                                                                       string?                                             because = null)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
        where T7 : notnull
        where T8 : notnull {
        var r = await task.ConfigureAwait(false);
        r.ShouldBeSuccessWhere(predicate, because);
        return r;
    }

    /// <summary>
    /// Asynchronously asserts (ValueTask) that an eight-value <see cref="Result{T1,T2,T3,T4,T5,T6,T7,T8}"/> is a failure and its exception satisfies the provided predicate.
    /// </summary>
    public static async ValueTask<Result<T1, T2, T3, T4, T5, T6, T7, T8>> ShouldBeFailureWhereAsync<T1, T2, T3, T4, T5, T6, T7, T8>(this ValueTask<Result<T1, T2, T3, T4, T5, T6, T7, T8>> task,
                                                                                                                                       Func<Exception, bool>                                 predicate,
                                                                                                                                       string?                                             because = null)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
        where T7 : notnull
        where T8 : notnull {
        var r = await task.ConfigureAwait(false);
        r.ShouldBeFailureWhere(predicate, because);
        return r;
    }
}
