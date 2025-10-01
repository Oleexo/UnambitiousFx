using System.Diagnostics;
using UnambitiousFx.Core.Results;
using Xunit;

namespace UnambitiousFx.Core.XUnit.Results;

/// <summary>
/// Provides extension methods for asserting the state of <see cref="Result"/> and <c>Result&lt;T&gt;</c> types within xUnit tests.
/// These methods help verify success and failure conditions for synchronous, asynchronous, and generic Result instances.
/// </summary>
[DebuggerStepThrough]
public static partial class ResultAssertExtensions {
    /// <summary>
    /// Represents the default message indicating that a success result was expected
    /// but a failure result was encountered.
    /// </summary>
    private const string SuccessExpected = "Expected success result but was failure.";

    /// <summary>
    /// A constant string representing the error message emitted when a failure
    /// result is expected, but a success result is encountered.
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
    public static Result ShouldBeSuccess(this Result result,
                                         string      because) {
        if (!result.IsSuccess) Assert.Fail(because);
        return result;
    }

    /// <summary>Asserts that the result is a failure.</summary>
    /// <param name="result">The result instance to be validated.</param>
    /// <returns>The original result for further assertions or chaining.</returns>
    public static Result ShouldBeFailure(this Result result) {
        Assert.True(result.IsFaulted, FailureExpected);
        return result;
    }

    /// <summary>Asserts that the result is a failure.</summary>
    /// <param name="result">The result instance to assert.</param>
    /// <param name="because">The reason why the assertion is expected to fail.</param>
    /// <returns>The original result, enabling method chaining.</returns>
    public static Result ShouldBeFailure(this Result result,
                                         string      because) {
        if (!result.IsFaulted) Assert.Fail(because);
        return result;
    }

    /// <summary>Asserts that the result is a failure.</summary>
    /// <param name="result">The result to assert.</param>
    /// <param name="error">When the method returns, contains the failure exception.</param>
    /// <returns>The same result for further assertions.</returns>
    public static Result ShouldBeFailure(this Result    result,
                                         out  Exception error) {
        if (result.Ok(out var maybeError)) {
            Assert.Fail(FailureExpected);
            throw new UnreachableException();
        }

        error = maybeError;
        return result;
    }

    /// <summary>
    /// Asserts that the result is a failure.
    /// </summary>
    /// <param name="result">The result to assert as a failure.</param>
    /// <param name="error">When the method returns, contains the failure exception.</param>
    /// <param name="because">The reason why the assertion is expected to fail.</param>
    /// <returns>The result after ensuring it is a failure.</returns>
    public static Result ShouldBeFailure(this Result    result,
                                         out  Exception error,
                                         string         because) {
        if (result.Ok(out var maybeError)) {
            Assert.Fail(because);
            throw new UnreachableException();
        }

        error = maybeError;
        return result;
    }

    /// <summary>
    /// Asserts that the result is a failure with the provided message.
    /// </summary>
    /// <param name="result">The result instance to assert against.</param>
    /// <param name="expectedMessage">The expected failure message to be compared with the result's message.</param>
    /// <returns>The same result instance for further assertions or chaining.</returns>
    public static Result ShouldBeFailureWithMessage(this Result result,
                                                    string      expectedMessage) {
        result.ShouldBeFailure(out var ex);
        Assert.Equal(expectedMessage, ex.Message);
        return result;
    }

    /// <summary>
    /// Asserts that the result is a success.
    /// </summary>
    /// <param name="result">The result to be checked.</param>
    /// <param name="value">When the method returns, contains the success value.</param>
    /// <returns>The original result if it is a success.</returns>
    public static Result<T1> ShouldBeSuccess<T1>(this Result<T1> result,
                                                 out  T1         value)
        where T1 : notnull {
        if (!result.Ok(out var tmp)) Assert.Fail(SuccessExpected);
        value = tmp;
        return result;
    }

    /// <summary>Asserts that the result is a success.</summary>
    /// <param name="result">The result to assert.</param>
    /// <param name="value">When the method returns, contains the success value.</param>
    /// <param name="because">The reason why the assertion is expected to pass.</param>
    /// <returns>The same result instance.</returns>
    public static Result<T1> ShouldBeSuccess<T1>(this Result<T1> result,
                                                 out  T1         value,
                                                 string          because)
        where T1 : notnull {
        if (!result.Ok(out var tmp)) Assert.Fail(because);
        value = tmp;
        return result;
    }

    /// <summary>
    /// Asserts that the result is a success.
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
    /// <param name="result">The result to be checked for failure.</param>
    /// <param name="error">When the method returns, contains the failure exception.</param>
    /// <returns>The original result if the assertion passes.</returns>
    public static Result<T1> ShouldBeFailure<T1>(this Result<T1> result,
                                                 out  Exception  error)
        where T1 : notnull {
        if (result.Ok(out T1? _, out var err)) {
            Assert.Fail(FailureExpected);
            throw new UnreachableException();
        }

        error = err;
        return result;
    }

    /// <summary>Asserts that the result is a failure.</summary>
    /// <param name="result">The result to be validated as a failure.</param>
    /// <param name="error">When the method returns, contains the failure exception.</param>
    /// <param name="because">The reason why the assertion is expected to fail.</param>
    /// <returns>The same result instance for further assertions.</returns>
    public static Result<T1> ShouldBeFailure<T1>(this Result<T1> result,
                                                 out  Exception  error,
                                                 string          because)
        where T1 : notnull {
        if (result.Ok(out T1? _, out var err)) {
            Assert.Fail(because);
            throw new UnreachableException();
        }

        error = err;
        return result;
    }

    /// <summary>
    /// Asserts that the result is a failure with the specified expected message.
    /// </summary>
    /// <param name="result">The result to be asserted.</param>
    /// <param name="expectedMessage">The expected failure message.</param>
    /// <returns>The original result to support further chaining.</returns>
    public static Result<T1> ShouldBeFailureWithMessage<T1>(this Result<T1> result,
                                                            string          expectedMessage)
        where T1 : notnull {
        result.ShouldBeFailure(out var ex);
        Assert.Equal(expectedMessage, ex.Message);
        return result;
    }

    /// <summary>
    /// Asserts that the task resolves to a success <see cref="Result"/>.
    /// </summary>
    /// <param name="task">The task producing the result.</param>
    /// <returns>The resolved result for further assertions.</returns>
    public static async Task<Result> ShouldBeSuccess(this Task<Result> task) => (await task.ConfigureAwait(false)).ShouldBeSuccess();

    /// <summary>
    /// Asserts that the task resolves to a failure result and extracts the error.
    /// </summary>
    /// <param name="task">The task producing the result.</param>
    /// <param name="error">When the method returns, contains the failure exception.</param>
    /// <returns>The resolved result for further assertions.</returns>
    public static Task<Result> ShouldBeFailure(this Task<Result> task,
                                               out  Exception    error) {
        var r = task.GetAwaiter()
                    .GetResult();
        r.ShouldBeFailure(out error);
        return Task.FromResult(r);
    }

    /// <summary>
    /// Asserts that the task resolves to a success result and extracts the value.
    /// </summary>
    /// <typeparam name="T1">The success value type.</typeparam>
    /// <param name="task">The task producing the result.</param>
    /// <param name="value">When the method returns, contains the success value.</param>
    /// <returns>The resolved result for further assertions.</returns>
    public static Task<Result<T1>> ShouldBeSuccess<T1>(this Task<Result<T1>> task,
                                                       out  T1               value)
        where T1 : notnull {
        var r = task.GetAwaiter()
                    .GetResult();
        r.ShouldBeSuccess(out value);
        return Task.FromResult(r);
    }

    /// <summary>
    /// Asserts that the task resolves to a success result and applies an additional assertion on the value.
    /// </summary>
    /// <typeparam name="T1">The success value type.</typeparam>
    /// <param name="task">The task producing the result.</param>
    /// <param name="assert">An action to perform additional assertions on the success value.</param>
    /// <returns>The resolved result for further assertions.</returns>
    public static async Task<Result<T1>> ShouldBeSuccess<T1>(this Task<Result<T1>> task,
                                                             Action<T1>            assert)
        where T1 : notnull {
        var r = await task.ConfigureAwait(false);
        r.ShouldBeSuccess(assert);
        return r;
    }

    /// <summary>Asserts that the task resolves to a failure result and extracts the error.</summary>
    /// <typeparam name="T1">The success value type.</typeparam>
    /// <param name="task">The task producing the result.</param>
    /// <param name="error">When the method returns, contains the failure exception.</param>
    /// <returns>The resolved result for further assertions.</returns>
    public static Task<Result<T1>> ShouldBeFailure<T1>(this Task<Result<T1>> task,
                                                       out  Exception        error)
        where T1 : notnull {
        var r = task.GetAwaiter()
                    .GetResult();
        r.ShouldBeFailure(out error);
        return Task.FromResult(r);
    }

    /// <summary>
    /// Asserts that the value task resolves to a success result and extracts the value.
    /// </summary>
    /// <typeparam name="T1">The success value type.</typeparam>
    /// <param name="task">The value task producing the result.</param>
    /// <param name="value">When the method returns, contains the success value.</param>
    /// <returns>The resolved result for further assertions.</returns>
    public static ValueTask<Result<T1>> ShouldBeSuccess<T1>(this ValueTask<Result<T1>> task,
                                                            out  T1                    value)
        where T1 : notnull {
        var r = task.GetAwaiter()
                    .GetResult();
        r.ShouldBeSuccess(out value);
        return ValueTask.FromResult(r);
    }

    /// <summary>Asserts that the value task resolves to a failure result and extracts the error.</summary>
    /// <typeparam name="T1">The success value type.</typeparam>
    /// <param name="task">The value task producing the result.</param>
    /// <param name="error">When the method returns, contains the failure exception.</param>
    /// <returns>The resolved result for further assertions.</returns>
    public static ValueTask<Result<T1>> ShouldBeFailure<T1>(this ValueTask<Result<T1>> task,
                                                            out  Exception             error)
        where T1 : notnull {
        var r = task.GetAwaiter()
                    .GetResult();
        r.ShouldBeFailure(out error);
        return ValueTask.FromResult(r);
    }

    /// <summary>Ensures that the operation was successful; otherwise, fails the assertion with a predefined message.</summary>
    /// <param name="ok">A boolean value indicating whether the operation was successful.</param>
    private static void EnsureSuccess(bool ok) {
        if (!ok) Assert.Fail(SuccessExpected);
    }

    /// <summary>
    /// Ensures that the specified condition represents a failure.
    /// </summary>
    /// <param name="ok">A boolean indicating whether the condition is satisfied. If true, an assertion failure is raised.</param>
    private static void EnsureFailure(bool ok) {
        if (ok) Assert.Fail(FailureExpected);
    }
}
