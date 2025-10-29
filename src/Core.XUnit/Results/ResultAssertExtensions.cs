using System.Diagnostics;
using UnambitiousFx.Core.Results;
using Xunit;

namespace UnambitiousFx.Core.XUnit.Results;

/// <summary>
/// Provides extension methods for asserting the behavior and state of <see cref="Result"/>,
/// <c>Result&lt;T&gt;</c>, <c>Result&lt;T1, T2&gt;</c>, and related asynchronous versions
/// in the context of xUnit testing.
/// These methods enable validation of success and failure outcomes, including extracting
/// values, asserting specific error messages, and providing contextual information for test clarity.
/// </summary>
[DebuggerStepThrough]
public static class ResultAssertExtensions {
    /// <summary>
    /// Represents the default message indicating that a success result was expected
    /// but a failure result was encountered.
    /// </summary>
    private const string SuccessExpected = "Expected success result but was failure.";

    /// <summary>
    /// Represents the error message used when a failure result is expected
    /// but a success result is encountered.
    /// </summary>
    private const string FailureExpected = "Expected failure result but was success.";

    /// <summary>
    /// Asserts that the result is a success.
    /// </summary>
    /// <param name="result">The result instance to be evaluated.</param>
    /// <returns>The original result for further assertions or chaining.</returns>
    public static Result ShouldBeSuccess(this Result result) {
        Assert.True(result.IsSuccess, SuccessExpected);
        return result;
    }

    /// <summary>
    /// Asserts that the result is a success.
    /// </summary>
    /// <param name="result">The result instance to be evaluated.</param>
    /// <param name="because">A custom message explaining the context of the assertion.</param>
    /// <returns>The original result for further assertions or chaining.</returns>
    public static Result ShouldBeSuccess(this Result result,
                                         string      because) {
        if (!result.IsSuccess) {
            Assert.Fail(because);
        }

        return result;
    }

    /// <summary>
    /// Asserts that the result is a failure.
    /// </summary>
    /// <param name="result">The result instance to be validated.</param>
    /// <returns>The original result for further assertions or chaining.</returns>
    public static Result ShouldBeFailure(this Result result) {
        Assert.True(result.IsFaulted, FailureExpected);
        return result;
    }

    /// <summary>
    /// Asserts that the result is a failure.
    /// </summary>
    /// <param name="result">The result instance to assert.</param>
    /// <param name="because">A custom message explaining the context of the assertion.</param>
    /// <returns>The original result for further assertions or chaining.</returns>
    public static Result ShouldBeFailure(this Result result,
                                         string      because) {
        if (!result.IsFaulted) {
            Assert.Fail(because);
        }

        return result;
    }

    /// <summary>
    /// Asserts that the result is a failure.
    /// </summary>
    /// <param name="result">The result instance to be evaluated.</param>
    /// <param name="error">The exception output if the result is a failure.</param>
    /// <returns>The original result for further assertions or chaining.</returns>
    public static Result ShouldBeFailure(this Result    result,
                                         out  Exception error) {
        if (result.TryGet(out var maybeError)) {
            Assert.Fail(FailureExpected);
            throw new UnreachableException();
        }

        error = maybeError;
        return result;
    }

    /// <summary>
    /// Asserts that the result is a failure.
    /// </summary>
    /// <param name="result">The result instance to be evaluated.</param>
    /// <param name="error">The exception output if the result is a failure.</param>
    /// <param name="because">A custom message explaining the context of the assertion.</param>
    /// <returns>The original result for further assertions or chaining.</returns>
    public static Result ShouldBeFailure(this Result    result,
                                         out  Exception error,
                                         string         because) {
        if (result.TryGet(out var maybeError)) {
            Assert.Fail(because);
            throw new UnreachableException();
        }

        error = maybeError;
        return result;
    }

    /// <summary>
    /// Asserts that the (non-generic) result is a failure with the specified message.
    /// </summary>
    public static Result ShouldBeFailureWithMessage(this Result result,
                                                    string      expectedMessage) {
        result.ShouldBeFailure(out var ex);
        Assert.Equal(expectedMessage, ex.Message);
        return result;
    }

    // Added generic overload for Result<T1> to assert failure message on generic results.
    /// <summary>
    /// Asserts that the generic result is a failure with the specified exception message.
    /// </summary>
    /// <typeparam name="T1">The success value type.</typeparam>
    /// <param name="result">The result instance expected to be a failure.</param>
    /// <param name="expectedMessage">The expected exception message.</param>
    /// <returns>The original result for fluent chaining.</returns>
    public static Result<T1> ShouldBeFailureWithMessage<T1>(this Result<T1> result,
                                                            string          expectedMessage)
        where T1 : notnull {
        result.ShouldBeFailure(out var ex);
        Assert.Equal(expectedMessage, ex.Message);
        return result;
    }

    /// <summary>
    /// Asserts that the result is a success.
    /// </summary>
    /// <param name="result">The result instance to be evaluated.</param>
    /// <param name="value">The output value if the result is a success.</param>
    /// <returns>The original result for further assertions or chaining.</returns>
    public static Result<T1> ShouldBeSuccess<T1>(this Result<T1> result,
                                                 out  T1         value)
        where T1 : notnull {
        if (!result.TryGet(out var tmp)) {
            Assert.Fail(SuccessExpected);
        }

        value = tmp;
        return result;
    }

    /// <summary>
    /// Asserts that the result is a success.
    /// </summary>
    /// <param name="result">The result instance to be evaluated.</param>
    /// <param name="value">The output value if the result is a success.</param>
    /// <param name="because">A custom message explaining the context of the assertion.</param>
    /// <returns>The original result for further assertions or chaining.</returns>
    public static Result<T1> ShouldBeSuccess<T1>(this Result<T1> result,
                                                 out  T1         value,
                                                 string          because)
        where T1 : notnull {
        if (!result.TryGet(out var tmp)) {
            Assert.Fail(because);
        }

        value = tmp;
        return result;
    }

    /// <summary>
    ///     Asserts that the result is a success.
    /// </summary>
    /// <param name="result">The result to be checked.</param>
    /// <param name="assert">An action to perform additional assertions on the success value.</param>
    /// <returns>The original result instance if it is a success.</returns>
    public static Result<T1> ShouldBeSuccess<T1>(this Result<T1> result,
                                                 Action<T1>      assert)
        where T1 : notnull {
        result.ShouldBeSuccess(out var v);
        assert(v);
        return result;
    }

    /// <summary>
    /// Asserts that the result is a failure.
    /// </summary>
    /// <param name="result">The result instance to be evaluated as a failure.</param>
    /// <param name="error">The exception output if the result is a failure.</param>
    /// <returns>The original result for further assertions or chaining.</returns>
    public static Result<T1> ShouldBeFailure<T1>(this Result<T1> result,
                                                 out  Exception  error)
        where T1 : notnull {
        if (result.TryGet(out _, out var err)) {
            Assert.Fail(FailureExpected);
            throw new UnreachableException();
        }

        error = err;
        return result;
    }

    /// <summary>
    /// Asserts that the result is a failure.
    /// </summary>
    /// <param name="result">The result instance to be evaluated as a failure.</param>
    /// <param name="error">The exception output if the result is a failure.</param>
    /// <param name="because">A custom message explaining the context of the assertion.</param>
    /// <returns>The original result for further assertions or chaining.</returns>
    public static Result<T1> ShouldBeFailure<T1>(this Result<T1> result,
                                                 out  Exception  error,
                                                 string          because)
        where T1 : notnull {
        if (result.TryGet(out _, out var err)) {
            Assert.Fail(because);
            throw new UnreachableException();
        }

        error = err;
        return result;
    }

    /// <summary>
    /// Asserts that the result is a success.
    /// </summary>
    /// <param name="task">The asynchronous task returning a result.</param>
    /// <returns>The original result for further assertions or chaining.</returns>
    public static async Task<Result> ShouldBeSuccess(this Task<Result> task) {
        return (await task.ConfigureAwait(false)).ShouldBeSuccess();
    }

    /// <summary>
    /// Asserts that the result is a failure.
    /// </summary>
    /// <param name="task">The asynchronous task returning a result.</param>
    /// <param name="error">The exception output if the result is a failure.</param>
    /// <returns>The original result for further assertions or chaining.</returns>
    public static Task<Result> ShouldBeFailure(this Task<Result> task,
                                               out  Exception    error) {
        var r = task.GetAwaiter()
                    .GetResult();
        r.ShouldBeFailure(out error);
        return Task.FromResult(r);
    }

    /// <summary>
    /// Asserts that the result is a success.
    /// </summary>
    /// <param name="task">The asynchronous task returning a result.</param>
    /// <param name="value">The output value if the result is a success.</param>
    /// <returns>The original result for further assertions or chaining.</returns>
    public static Task<Result<T1>> ShouldBeSuccess<T1>(this Task<Result<T1>> task,
                                                       out  T1               value)
        where T1 : notnull {
        var r = task.GetAwaiter()
                    .GetResult();
        r.ShouldBeSuccess(out value);
        return Task.FromResult(r);
    }

    /// <summary>
    /// Asserts that the result is a success.
    /// </summary>
    /// <param name="task">The asynchronous task returning a result.</param>
    /// <param name="assert">An action to perform additional assertions on the success value.</param>
    /// <returns>The original result for further assertions or chaining.</returns>
    public static async Task<Result<T1>> ShouldBeSuccess<T1>(this Task<Result<T1>> task,
                                                             Action<T1>            assert)
        where T1 : notnull {
        var r = await task.ConfigureAwait(false);
        r.ShouldBeSuccess(assert);
        return r;
    }

    /// <summary>
    /// Asserts that the result is a failure.
    /// </summary>
    /// <param name="task">The asynchronous task returning a result.</param>
    /// <param name="error">The exception output if the result is a failure.</param>
    /// <returns>The original result for further assertions or chaining.</returns>
    public static Task<Result<T1>> ShouldBeFailure<T1>(this Task<Result<T1>> task,
                                                       out  Exception        error)
        where T1 : notnull {
        var r = task.GetAwaiter()
                    .GetResult();
        r.ShouldBeFailure(out error);
        return Task.FromResult(r);
    }

    /// <summary>
    /// Asserts that the result is a success.
    /// </summary>
    /// <param name="task">The asynchronous ValueTask returning a result.</param>
    /// <param name="value">The output value if the result is a success.</param>
    /// <returns>The original result for further assertions or chaining.</returns>
    public static ValueTask<Result<T1>> ShouldBeSuccess<T1>(this ValueTask<Result<T1>> task,
                                                            out  T1                    value)
        where T1 : notnull {
        var r = task.GetAwaiter()
                    .GetResult();
        r.ShouldBeSuccess(out value);
        return ValueTask.FromResult(r);
    }

    /// <summary>
    /// Asserts that the result is a failure.
    /// </summary>
    /// <param name="task">The asynchronous ValueTask returning a result.</param>
    /// <param name="error">The exception output if the result is a failure.</param>
    /// <returns>The original result for further assertions or chaining.</returns>
    public static ValueTask<Result<T1>> ShouldBeFailure<T1>(this ValueTask<Result<T1>> task,
                                                            out  Exception             error)
        where T1 : notnull {
        var r = task.GetAwaiter()
                    .GetResult();
        r.ShouldBeFailure(out error);
        return ValueTask.FromResult(r);
    }

    /// <summary>
    /// Ensures that the operation was successful; otherwise, fails the assertion with a predefined message.
    /// </summary>
    /// <param name="ok">A boolean value indicating whether the operation was successful.</param>
    private static void EnsureSuccess(bool ok) {
        if (!ok) {
            Assert.Fail(SuccessExpected);
        }
    }

    /// <summary>
    /// Ensures that the specified condition represents a failure.
    /// </summary>
    /// <param name="ok">A boolean indicating whether the condition represents failure. If true, an assertion failure is raised.</param>
    private static void EnsureFailure(bool ok) {
        if (ok) {
            Assert.Fail(FailureExpected);
        }
    }

    /// <summary>
    /// Asserts that the result is a success.
    /// </summary>
    /// <param name="result">The result instance to evaluate.</param>
    /// <param name="values">The output tuple of values if the result is a success.</param>
    /// <returns>The original result for additional operations or assertions.</returns>
    public static Result<T1, T2> ShouldBeSuccess<T1, T2>(this Result<T1, T2> result,
                                                         out  (T1, T2)       values)
        where T1 : notnull
        where T2 : notnull {
        if (!result.TryGet(out var value1, out var value2)) {
            Assert.Fail(SuccessExpected);
            throw new UnreachableException();
        }

        values = (value1, value2);
        return result;
    }

    /// <summary>
    /// Asserts that the result is a success.
    /// </summary>
    /// <param name="result">The result instance to be evaluated.</param>
    /// <param name="assert">An action to perform additional assertions on the success values.</param>
    /// <returns>The original result for further assertions or chaining.</returns>
    public static Result<T1, T2> ShouldBeSuccess<T1, T2>(this Result<T1, T2> result,
                                                         Action<T1, T2>      assert)
        where T1 : notnull
        where T2 : notnull {
        result.ShouldBeSuccess(out var v);
        assert(v.Item1, v.Item2);
        return result;
    }

    /// <summary>
    /// Asserts that the result is a failure.
    /// </summary>
    /// <param name="result">The result instance to be evaluated.</param>
    /// <param name="error">The exception output if the result is a failure.</param>
    /// <returns>The original result for further assertions or chaining.</returns>
    public static Result<T1, T2> ShouldBeFailure<T1, T2>(this Result<T1, T2> result,
                                                         out  Exception      error)
        where T1 : notnull
        where T2 : notnull {
        if (result.TryGet(out _, out _, out var tmpError)) {
            Assert.Fail(FailureExpected);
            throw new UnreachableException();
        }

        error = tmpError;
        return result;
    }

    /// <summary>
    /// Asserts that the result is a failure.
    /// </summary>
    /// <param name="result">The result instance to be evaluated.</param>
    /// <param name="error">The exception output if the result is a failure.</param>
    /// <param name="because">A custom message explaining the context of the assertion.</param>
    /// <returns>The original result for further assertions or chaining.</returns>
    public static Result<T1, T2> ShouldBeFailure<T1, T2>(this Result<T1, T2> result,
                                                         out  Exception      error,
                                                         string          because)
        where T1 : notnull
        where T2 : notnull {
        if (result.TryGet(out _, out _, out var tmpError)) {
            Assert.Fail(because);
            throw new UnreachableException();
        }

        error = tmpError;
        return result;
    }

    /// <summary>
    /// Asserts that the result is a success.
    /// </summary>
    /// <param name="result">The result instance to be evaluated.</param>
    /// <param name="values">The output tuple of values if the result is a success.</param>
    /// <returns>The original result for further assertions or chaining.</returns>
    public static Result<T1, T2, T3> ShouldBeSuccess<T1, T2, T3>(this Result<T1, T2, T3> result,
                                                                 out  (T1, T2, T3)       values)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull {
        if (!result.TryGet(out var v1, out var v2, out var v3)) {
            Assert.Fail(SuccessExpected);
            throw new UnreachableException();
        }

        values = (v1, v2, v3);
        return result;
    }

    /// <summary>
    /// Asserts that the result is a success.
    /// </summary>
    /// <param name="result">The result instance to be evaluated.</param>
    /// <param name="assert">An action to perform additional assertions on the success values.</param>
    /// <returns>The original result for further assertions or chaining.</returns>
    public static Result<T1, T2, T3> ShouldBeSuccess<T1, T2, T3>(this Result<T1, T2, T3> result,
                                                                 Action<T1, T2, T3>      assert)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull {
        result.ShouldBeSuccess(out var v);
        assert(v.Item1, v.Item2, v.Item3);
        return result;
    }

    /// <summary>
    /// Asserts that the result is a failure.
    /// </summary>
    /// <param name="result">The result instance to be evaluated.</param>
    /// <param name="error">The exception output if the result is a failure.</param>
    /// <returns>The original result for further assertions or chaining.</returns>
    public static Result<T1, T2, T3> ShouldBeFailure<T1, T2, T3>(this Result<T1, T2, T3> result,
                                                                 out  Exception          error)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull {
        if (result.TryGet(out _, out _, out _, out var tmpError)) {
            Assert.Fail(FailureExpected);
            throw new UnreachableException();
        }

        error = tmpError;
        return result;
    }

    /// <summary>
    /// Asserts that the result is a failure.
    /// </summary>
    /// <param name="result">The result instance to be evaluated.</param>
    /// <param name="error">The exception output if the result is a failure.</param>
    /// <param name="because">A custom message explaining the context of the assertion.</param>
    /// <returns>The original result for further assertions or chaining.</returns>
    public static Result<T1, T2, T3> ShouldBeFailure<T1, T2, T3>(this Result<T1, T2, T3> result,
                                                                 out  Exception          error,
                                                                 string          because)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull {
        if (result.TryGet(out _, out _, out _, out var tmpError)) {
            Assert.Fail(because);
            throw new UnreachableException();
        }

        error = tmpError;
        return result;
    }

    /// <summary>
    /// Asserts that the result is a success.
    /// </summary>
    /// <param name="result">The result instance to be evaluated.</param>
    /// <param name="values">The output tuple of values if the result is a success.</param>
    /// <returns>The original result for further assertions or chaining.</returns>
    public static Result<T1, T2, T3, T4> ShouldBeSuccess<T1, T2, T3, T4>(this Result<T1, T2, T3, T4> result,
                                                                         out  (T1, T2, T3, T4)       values)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull {
        if (!result.TryGet(out var v1, out var v2, out var v3, out var v4)) {
            Assert.Fail(SuccessExpected);
            throw new UnreachableException();
        }

        values = (v1, v2, v3, v4);
        return result;
    }

    /// <summary>
    /// Asserts that the result is a success.
    /// </summary>
    /// <param name="result">The result instance to be evaluated.</param>
    /// <param name="assert">An action to perform additional assertions on the success values.</param>
    /// <returns>The original result for further assertions or chaining.</returns>
    public static Result<T1, T2, T3, T4> ShouldBeSuccess<T1, T2, T3, T4>(this Result<T1, T2, T3, T4> result,
                                                                         Action<T1, T2, T3, T4>      assert)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull {
        result.ShouldBeSuccess(out var v);
        assert(v.Item1, v.Item2, v.Item3, v.Item4);
        return result;
    }

    /// <summary>
    /// Asserts that the result is a failure.
    /// </summary>
    /// <param name="result">The result instance to be evaluated.</param>
    /// <param name="error">The exception output if the result is a failure.</param>
    /// <returns>The original result for further assertions or chaining.</returns>
    public static Result<T1, T2, T3, T4> ShouldBeFailure<T1, T2, T3, T4>(this Result<T1, T2, T3, T4> result,
                                                                         out  Exception              error)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull {
        if (result.TryGet(out _, out _, out _, out _, out var tmpError)) {
            Assert.Fail(FailureExpected);
            throw new UnreachableException();
        }

        error = tmpError;
        return result;
    }

    /// <summary>
    /// Asserts that the result is a failure.
    /// </summary>
    /// <param name="result">The result instance to be evaluated.</param>
    /// <param name="error">The exception output if the result is a failure.</param>
    /// <param name="because">A custom message explaining the context of the assertion.</param>
    /// <returns>The original result for further assertions or chaining.</returns>
    public static Result<T1, T2, T3, T4> ShouldBeFailure<T1, T2, T3, T4>(this Result<T1, T2, T3, T4> result,
                                                                         out  Exception              error,
                                                                         string          because)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull {
        if (result.TryGet(out _, out _, out _, out _, out var tmpError)) {
            Assert.Fail(because);
            throw new UnreachableException();
        }

        error = tmpError;
        return result;
    }

    /// <summary>
    /// Asserts that the result is a success.
    /// </summary>
    /// <param name="result">The result instance to be evaluated.</param>
    /// <param name="values">The output tuple of values if the result is a success.</param>
    /// <returns>The original result for further assertions or chaining.</returns>
    public static Result<T1, T2, T3, T4, T5> ShouldBeSuccess<T1, T2, T3, T4, T5>(this Result<T1, T2, T3, T4, T5> result,
                                                                                 out  (T1, T2, T3, T4, T5)       values)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull {
        if (!result.TryGet(out var v1, out var v2, out var v3, out var v4, out var v5)) {
            Assert.Fail(SuccessExpected);
            throw new UnreachableException();
        }

        values = (v1, v2, v3, v4, v5);
        return result;
    }

    /// <summary>
    /// Asserts that the result is a success.
    /// </summary>
    /// <param name="result">The result instance to be evaluated.</param>
    /// <param name="assert">An action to perform additional assertions on the success values.</param>
    /// <returns>The original result for further assertions or chaining.</returns>
    public static Result<T1, T2, T3, T4, T5> ShouldBeSuccess<T1, T2, T3, T4, T5>(this Result<T1, T2, T3, T4, T5> result,
                                                                                 Action<T1, T2, T3, T4, T5>      assert)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull {
        result.ShouldBeSuccess(out var v);
        assert(v.Item1, v.Item2, v.Item3, v.Item4, v.Item5);
        return result;
    }

    /// <summary>
    /// Asserts that the result is a failure.
    /// </summary>
    /// <param name="result">The result instance to be evaluated.</param>
    /// <param name="error">The exception output if the result is a failure.</param>
    /// <returns>The original result for further assertions or chaining.</returns>
    public static Result<T1, T2, T3, T4, T5> ShouldBeFailure<T1, T2, T3, T4, T5>(this Result<T1, T2, T3, T4, T5> result,
                                                                                 out  Exception                  error)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull {
        if (result.TryGet(out _, out _, out _, out _, out _, out var tmpError)) {
            Assert.Fail(FailureExpected);
            throw new UnreachableException();
        }

        error = tmpError;
        return result;
    }

    /// <summary>
    /// Asserts that the result is a success.
    /// </summary>
    /// <param name="result">The result instance to be evaluated.</param>
    /// <param name="values">The output tuple of values if the result is a success.</param>
    /// <returns>The original result for further assertions or chaining.</returns>
    public static Result<T1, T2, T3, T4, T5, T6> ShouldBeSuccess<T1, T2, T3, T4, T5, T6>(this Result<T1, T2, T3, T4, T5, T6> result,
                                                                                         out  (T1, T2, T3, T4, T5, T6)       values)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull {
        if (!result.TryGet(out var v1, out var v2, out var v3, out var v4, out var v5, out var v6)) {
            Assert.Fail(SuccessExpected);
            throw new UnreachableException();
        }

        values = (v1, v2, v3, v4, v5, v6);
        return result;
    }

    /// <summary>
    /// Asserts that the result is a success.
    /// </summary>
    /// <param name="result">The result instance to be evaluated.</param>
    /// <param name="assert">An action to perform additional assertions on the success values.</param>
    /// <returns>The original result for further assertions or chaining.</returns>
    public static Result<T1, T2, T3, T4, T5, T6> ShouldBeSuccess<T1, T2, T3, T4, T5, T6>(this Result<T1, T2, T3, T4, T5, T6> result,
                                                                                         Action<T1, T2, T3, T4, T5, T6>      assert)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull {
        result.ShouldBeSuccess(out var v);
        assert(v.Item1, v.Item2, v.Item3, v.Item4, v.Item5, v.Item6);
        return result;
    }

    /// <summary>
    /// Asserts that the result is a failure.
    /// </summary>
    /// <param name="result">The result instance to be evaluated.</param>
    /// <param name="error">The exception output if the result is a failure.</param>
    /// <returns>The original result for further assertions or chaining.</returns>
    public static Result<T1, T2, T3, T4, T5, T6> ShouldBeFailure<T1, T2, T3, T4, T5, T6>(this Result<T1, T2, T3, T4, T5, T6> result,
                                                                                         out  Exception                      error)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull {
        if (result.TryGet(out _, out _, out _, out _, out _, out _, out var tmpError)) {
            Assert.Fail(FailureExpected);
            throw new UnreachableException();
        }

        error = tmpError;
        return result;
    }

    /// <summary>
    /// Ensures that the given result represents a successful operation.
    /// </summary>
    /// <param name="result">The operation result to evaluate for success.</param>
    /// <param name="values">The output tuple of values if the result is a success.</param>
    /// <returns>The same result instance, allowing for method chaining or additional assertions.</returns>
    public static Result<T1, T2, T3, T4, T5, T6, T7> ShouldBeSuccess<T1, T2, T3, T4, T5, T6, T7>(this Result<T1, T2, T3, T4, T5, T6, T7> result,
                                                                                                 out  (T1, T2, T3, T4, T5, T6, T7)       values)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
        where T7 : notnull {
        if (!result.TryGet(out var v1, out var v2, out var v3, out var v4, out var v5, out var v6, out var v7)) {
            Assert.Fail(SuccessExpected);
            throw new UnreachableException();
        }

        values = (v1, v2, v3, v4, v5, v6, v7);
        return result;
    }

    /// <summary>
    /// Asserts that the result is a success.
    /// </summary>
    /// <param name="result">The result instance to be evaluated.</param>
    /// <param name="assert">An action to perform additional assertions on the success values.</param>
    /// <returns>The original result for further assertions or chaining.</returns>
    public static Result<T1, T2, T3, T4, T5, T6, T7> ShouldBeSuccess<T1, T2, T3, T4, T5, T6, T7>(this Result<T1, T2, T3, T4, T5, T6, T7> result,
                                                                                                 Action<T1, T2, T3, T4, T5, T6, T7>      assert)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
        where T7 : notnull {
        result.ShouldBeSuccess(out var v);
        assert(v.Item1, v.Item2, v.Item3, v.Item4, v.Item5, v.Item6, v.Item7);
        return result;
    }

    /// <summary>
    /// Asserts that the result is a failure.
    /// </summary>
    /// <param name="result">The result instance to be evaluated.</param>
    /// <param name="error">The exception output if the result is a failure.</param>
    /// <returns>The original result for further assertions or chaining.</returns>
    public static Result<T1, T2, T3, T4, T5, T6, T7> ShouldBeFailure<T1, T2, T3, T4, T5, T6, T7>(this Result<T1, T2, T3, T4, T5, T6, T7> result,
                                                                                                 out  Exception                          error)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
        where T7 : notnull {
        if (result.TryGet(out _, out _, out _, out _, out _, out _, out _, out var tmpError)) {
            Assert.Fail(FailureExpected);
            throw new UnreachableException();
        }

        error = tmpError;
        return result;
    }

    /// <summary>
    /// Asserts that the result is a success.
    /// </summary>
    /// <param name="result">The result instance to be evaluated.</param>
    /// <param name="values">The output tuple of values if the result is a success.</param>
    /// <returns>The original result for further assertions or chaining.</returns>
    public static Result<T1, T2, T3, T4, T5, T6, T7, T8> ShouldBeSuccess<T1, T2, T3, T4, T5, T6, T7, T8>(this Result<T1, T2, T3, T4, T5, T6, T7, T8> result,
                                                                                                         out  (T1, T2, T3, T4, T5, T6, T7, T8)       values)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
        where T7 : notnull
        where T8 : notnull {
        if (!result.TryGet(out var v1, out var v2, out var v3, out var v4, out var v5, out var v6, out var v7, out var v8)) {
            Assert.Fail(SuccessExpected);
            throw new UnreachableException();
        }

        values = (v1, v2, v3, v4, v5, v6, v7, v8);
        return result;
    }

    /// <summary>
    /// Asserts that the result is a success.
    /// </summary>
    /// <param name="result">The result instance to be evaluated.</param>
    /// <param name="assert">An action to perform additional assertions on the success values.</param>
    /// <returns>The original result for further assertions or chaining.</returns>
    public static Result<T1, T2, T3, T4, T5, T6, T7, T8> ShouldBeSuccess<T1, T2, T3, T4, T5, T6, T7, T8>(this Result<T1, T2, T3, T4, T5, T6, T7, T8> result,
                                                                                                         Action<T1, T2, T3, T4, T5, T6, T7, T8>      assert)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
        where T7 : notnull
        where T8 : notnull {
        result.ShouldBeSuccess(out var v);
        assert(v.Item1, v.Item2, v.Item3, v.Item4, v.Item5, v.Item6, v.Item7, v.Item8);
        return result;
    }

    /// <summary>
    /// Asserts that the result is a failure.
    /// </summary>
    /// <param name="result">The result instance to be evaluated.</param>
    /// <param name="error">The exception output if the result is a failure.</param>
    /// <returns>The original result for further assertions or chaining.</returns>
    public static Result<T1, T2, T3, T4, T5, T6, T7, T8> ShouldBeFailure<T1, T2, T3, T4, T5, T6, T7, T8>(this Result<T1, T2, T3, T4, T5, T6, T7, T8> result,
                                                                                                         out  Exception                              error)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
        where T7 : notnull
        where T8 : notnull {
        if (result.TryGet(out _, out _, out _, out _, out _, out _, out _, out _, out var tmpError)) {
            Assert.Fail(FailureExpected);
            throw new UnreachableException();
        }

        error = tmpError;
        return result;
    }
}
