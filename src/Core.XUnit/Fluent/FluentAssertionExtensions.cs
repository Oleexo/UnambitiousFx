using UnambitiousFx.Core.Eithers;
using UnambitiousFx.Core.Options;
using UnambitiousFx.Core.Results;
using Xunit;

namespace UnambitiousFx.Core.XUnit.Fluent;

/// <summary>
///     Provides extension methods for asserting success or failure of result objects
///     within the FluentAssertions framework.
/// </summary>
public static class FluentAssertionExtensions {
    /// <summary>
    ///     Asserts that the given result represents a successful outcome.
    /// </summary>
    /// <param name="result">The result to assert.</param>
    /// <returns>A SuccessAssertion wrapping Unit.</returns>
    public static SuccessAssertion<Unit> EnsureSuccess(this Result result) {
        if (!result.IsSuccess) {
            Assert.Fail("Expected success result but was failure.");
        }

        return new SuccessAssertion<Unit>(Unit.Value);
    }

    /// <summary>
    ///     Ensures that the specified <see cref="Result" /> represents a failure. If the result is not a failure,
    ///     the assertion will fail.
    /// </summary>
    /// <param name="result">The <see cref="Result" /> to validate as a failure.</param>
    /// <returns>A <see cref="FailureAssertion" /> containing the error details of the failure.</returns>
    public static FailureAssertion EnsureFailure(this Result result) {
        var success = result.Ok(out var error);
        if (success) {
            Assert.Fail("Expected failure result but was success.");
        }

        return new FailureAssertion(error!);
    }

    /// <summary>
    ///     Asserts that the result is successful and returns a SuccessAssertion for its value.
    /// </summary>
    /// <param name="result">The result to assert.</param>
    /// <returns>A SuccessAssertion for the contained value.</returns>
    public static SuccessAssertion<T1> EnsureSuccess<T1>(this Result<T1> result)
        where T1 : notnull {
        if (!result.Ok(out var value)) {
            Assert.Fail("Expected success result but was failure.");
        }

        return new SuccessAssertion<T1>(value);
    }

    /// <summary>
    ///     Asserts that the result represents a failure and returns a FailureAssertion for further checks.
    /// </summary>
    /// <param name="result">The result to assert.</param>
    /// <returns>A FailureAssertion for the error.</returns>
    public static FailureAssertion EnsureFailure<T1>(this Result<T1> result)
        where T1 : notnull {
        if (result.Ok(out _, out var error)) {
            Assert.Fail("Expected failure result but was success.");
        }

        return new FailureAssertion(error);
    }

    /// <summary>
    ///     Ensures that the provided <see cref="Result" /> represents a success state.
    ///     If the result is not successful, the assertion fails.
    /// </summary>
    /// <param name="result">The result instance to assert.</param>
    /// <returns>An instance of <see cref="SuccessAssertion{Unit}" /> representing the success state of the result.</returns>
    public static SuccessAssertion<(T1, T2)> EnsureSuccess<T1, T2>(this Result<T1, T2> result)
        where T1 : notnull
        where T2 : notnull {
        if (!result.Ok(out (T1, T2) tuple)) {
            Assert.Fail("Expected success result but was failure.");
        }

        return new SuccessAssertion<(T1, T2)>(tuple);
    }

    /// <summary>
    ///     Ensures that the specified result is a failure and provides fluent assertions for further validation.
    /// </summary>
    /// <typeparam name="T1">The type of the first success value contained in the result.</typeparam>
    /// <typeparam name="T2">The type of the second success value contained in the result.</typeparam>
    /// <param name="result">The result to validate.</param>
    /// <returns>A <see cref="FailureAssertion" /> that can be used to assert characteristics of the failure.</returns>
    /// <exception cref="Xunit.Sdk.XunitException">Thrown when the result is not a failure.</exception>
    public static FailureAssertion EnsureFailure<T1, T2>(this Result<T1, T2> result)
        where T1 : notnull
        where T2 : notnull {
        if (result.Ok(out (T1, T2) _, out var error)) {
            Assert.Fail("Expected failure result but was success.");
        }

        return new FailureAssertion(error);
    }

    /// <summary>
    ///     Verifies that the provided <see cref="Result" /> is successful.
    ///     If the result is not successful, the assertion will fail.
    /// </summary>
    /// <param name="result">The result instance to verify.</param>
    /// <returns>A <see cref="SuccessAssertion{TValue}" /> representing the successful result.</returns>
    public static SuccessAssertion<(T1, T2, T3)> EnsureSuccess<T1, T2, T3>(this Result<T1, T2, T3> result)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull {
        if (!result.Ok(out (T1, T2, T3) tuple)) {
            Assert.Fail("Expected success result but was failure.");
        }

        return new SuccessAssertion<(T1, T2, T3)>(tuple);
    }

    /// <summary>
    ///     Asserts that the result represents a failure and returns a FailureAssertion for further checks.
    /// </summary>
    /// <param name="result">The result to assert.</param>
    /// <returns>A FailureAssertion for the error.</returns>
    public static FailureAssertion EnsureFailure<T1, T2, T3>(this Result<T1, T2, T3> result)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull {
        if (result.Ok(out (T1, T2, T3) _, out var error)) {
            Assert.Fail("Expected failure result but was success.");
        }

        return new FailureAssertion(error);
    }

    /// <summary>
    ///     Ensures that the given <see cref="Result" /> instance represents a success state.
    ///     If the <see cref="Result" /> is not successful, the method will fail the test execution.
    /// </summary>
    /// <param name="result">The <see cref="Result" /> instance to be validated.</param>
    /// <returns>A <see cref="SuccessAssertion{TValue}" /> containing the success value.</returns>
    public static SuccessAssertion<(T1, T2, T3, T4)> EnsureSuccess<T1, T2, T3, T4>(this Result<T1, T2, T3, T4> result)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull {
        if (!result.Ok(out (T1, T2, T3, T4) tuple)) {
            Assert.Fail("Expected success result but was failure.");
        }

        return new SuccessAssertion<(T1, T2, T3, T4)>(tuple);
    }

    /// <summary>
    ///     Ensures that the provided <see cref="Result" /> represents a failure result and returns a
    ///     <see cref="FailureAssertion" /> object
    ///     for further assertions.
    /// </summary>
    /// <param name="result">The result to be inspected for failure.</param>
    /// <returns>A <see cref="FailureAssertion" /> instance to perform further validations on the failure.</returns>
    public static FailureAssertion EnsureFailure<T1, T2, T3, T4>(this Result<T1, T2, T3, T4> result)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull {
        if (result.Ok(out (T1, T2, T3, T4) _, out var error)) {
            Assert.Fail("Expected failure result but was success.");
        }

        return new FailureAssertion(error);
    }

    /// <summary>
    ///     Asserts that the result is successful and returns a SuccessAssertion for its tuple value.
    /// </summary>
    /// <param name="result">The result to assert.</param>
    /// <returns>A SuccessAssertion for the contained (T1, T2, T3, T4, T5) value.</returns>
    public static SuccessAssertion<(T1, T2, T3, T4, T5)> EnsureSuccess<T1, T2, T3, T4, T5>(this Result<T1, T2, T3, T4, T5> result)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull {
        if (!result.Ok(out (T1, T2, T3, T4, T5) tuple)) {
            Assert.Fail("Expected success result but was failure.");
        }

        return new SuccessAssertion<(T1, T2, T3, T4, T5)>(tuple);
    }

    /// Ensures that the provided result represents a failure state.
    /// If the result is successful, an assertion failure is generated.
    /// <param name="result">The result instance to be verified as a failure.</param>
    /// <returns>A FailureAssertion instance, which contains the associated error for further verification.</returns>
    public static FailureAssertion EnsureFailure<T1, T2, T3, T4, T5>(this Result<T1, T2, T3, T4, T5> result)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull {
        if (result.Ok(out (T1, T2, T3, T4, T5) _, out var error)) {
            Assert.Fail("Expected failure result but was success.");
        }

        return new FailureAssertion(error);
    }

    /// <summary>
    ///     Asserts that the result is successful and returns a SuccessAssertion for its tuple value.
    /// </summary>
    /// <param name="result">The result to assert.</param>
    /// <returns>A SuccessAssertion for the contained (T1, T2, T3, T4, T5, T6) value.</returns>
    public static SuccessAssertion<(T1, T2, T3, T4, T5, T6)> EnsureSuccess<T1, T2, T3, T4, T5, T6>(this Result<T1, T2, T3, T4, T5, T6> result)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull {
        if (!result.Ok(out (T1, T2, T3, T4, T5, T6) tuple)) {
            Assert.Fail("Expected success result but was failure.");
        }

        return new SuccessAssertion<(T1, T2, T3, T4, T5, T6)>(tuple);
    }

    /// Ensures that the given result represents a failure state.
    /// Throws an assertion failure if the result is successful.
    /// <param name="result">The result to verify for a failure state.</param>
    /// <returns>An instance of <see cref="FailureAssertion" /> to enable further fluent assertions.</returns>
    public static FailureAssertion EnsureFailure<T1, T2, T3, T4, T5, T6>(this Result<T1, T2, T3, T4, T5, T6> result)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull {
        if (result.Ok(out (T1, T2, T3, T4, T5, T6) _, out var error)) {
            Assert.Fail("Expected failure result but was success.");
        }

        return new FailureAssertion(error);
    }

    /// <summary>
    ///     Asserts that the result is successful and returns a SuccessAssertion for its tuple value.
    /// </summary>
    /// <typeparam name="T1">The first success value type.</typeparam>
    /// <typeparam name="T2">The second success value type.</typeparam>
    /// <typeparam name="T3">The third success value type.</typeparam>
    /// <typeparam name="T4">The fourth success value type.</typeparam>
    /// <typeparam name="T5">The fifth success value type.</typeparam>
    /// <typeparam name="T6">The sixth success value type.</typeparam>
    /// <typeparam name="T7">The seventh success value type.</typeparam>
    /// <param name="result">The result to assert.</param>
    /// <returns>A SuccessAssertion for the contained tuple.</returns>
    public static SuccessAssertion<(T1, T2, T3, T4, T5, T6, T7)> EnsureSuccess<T1, T2, T3, T4, T5, T6, T7>(this Result<T1, T2, T3, T4, T5, T6, T7> result)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
        where T7 : notnull {
        if (!result.Ok(out (T1, T2, T3, T4, T5, T6, T7) tuple)) {
            Assert.Fail("Expected success result but was failure.");
        }

        return new SuccessAssertion<(T1, T2, T3, T4, T5, T6, T7)>(tuple);
    }

    /// Ensures that the specified result represents a failure. If the result is a success, this method will throw an assertion failure.
    /// Returns a FailureAssertion object for further failure-specific assertions.
    /// <param name="result">The result to ensure represents a failure.</param>
    /// <returns>A FailureAssertion object for verifying the failure details.</returns>
    public static FailureAssertion EnsureFailure<T1, T2, T3, T4, T5, T6, T7>(this Result<T1, T2, T3, T4, T5, T6, T7> result)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
        where T7 : notnull {
        if (result.Ok(out (T1, T2, T3, T4, T5, T6, T7) _, out var error)) {
            Assert.Fail("Expected failure result but was success.");
        }

        return new FailureAssertion(error);
    }

    /// <summary>
    ///     Asserts that the result is successful and returns a SuccessAssertion for its tuple value.
    /// </summary>
    /// <typeparam name="T1">The first success value type.</typeparam>
    /// <typeparam name="T2">The second success value type.</typeparam>
    /// <typeparam name="T3">The third success value type.</typeparam>
    /// <typeparam name="T4">The fourth success value type.</typeparam>
    /// <typeparam name="T5">The fifth success value type.</typeparam>
    /// <typeparam name="T6">The sixth success value type.</typeparam>
    /// <typeparam name="T7">The seventh success value type.</typeparam>
    /// <typeparam name="T8">The eighth success value type.</typeparam>
    /// <param name="result">The result to assert.</param>
    /// <returns>A SuccessAssertion for the contained tuple.</returns>
    public static SuccessAssertion<(T1, T2, T3, T4, T5, T6, T7, T8)> EnsureSuccess<T1, T2, T3, T4, T5, T6, T7, T8>(this Result<T1, T2, T3, T4, T5, T6, T7, T8> result)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
        where T7 : notnull
        where T8 : notnull {
        if (!result.Ok(out (T1, T2, T3, T4, T5, T6, T7, T8) tuple)) {
            Assert.Fail("Expected success result but was failure.");
        }

        return new SuccessAssertion<(T1, T2, T3, T4, T5, T6, T7, T8)>(tuple);
    }

    /// <summary>
    ///     Asserts that the result represents a failure and returns a FailureAssertion for the error.
    /// </summary>
    /// <typeparam name="T1">The first success value type.</typeparam>
    /// <typeparam name="T2">The second success value type.</typeparam>
    /// <typeparam name="T3">The third success value type.</typeparam>
    /// <typeparam name="T4">The fourth success value type.</typeparam>
    /// <typeparam name="T5">The fifth success value type.</typeparam>
    /// <typeparam name="T6">The sixth success value type.</typeparam>
    /// <typeparam name="T7">The seventh success value type.</typeparam>
    /// <typeparam name="T8">The eighth success value type.</typeparam>
    /// <param name="result">The result to assert.</param>
    /// <returns>A FailureAssertion for the error.</returns>
    public static FailureAssertion EnsureFailure<T1, T2, T3, T4, T5, T6, T7, T8>(this Result<T1, T2, T3, T4, T5, T6, T7, T8> result)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
        where T7 : notnull
        where T8 : notnull {
        if (result.Ok(out (T1, T2, T3, T4, T5, T6, T7, T8) _, out var error)) {
            Assert.Fail("Expected failure result but was success.");
        }

        return new FailureAssertion(error);
    }

    // Option
    /// <summary>
    ///     Asserts that the option is Some and returns a SomeAssertion for its value.
    /// </summary>
    /// <typeparam name="T">The option value type.</typeparam>
    /// <param name="option">The option to assert.</param>
    /// <returns>A SomeAssertion for the contained value.</returns>
    public static SomeAssertion<T> EnsureSome<T>(this Option<T> option)
        where T : notnull {
        if (!option.Some(out var value)) {
            Assert.Fail("Expected Option.Some but was None.");
        }

        return new SomeAssertion<T>(value);
    }

    /// <summary>
    ///     Asserts that the option is None and returns a NoneAssertion.
    /// </summary>
    /// <typeparam name="T">The option value type.</typeparam>
    /// <param name="option">The option to assert.</param>
    /// <returns>A NoneAssertion to allow further chaining.</returns>
    public static NoneAssertion EnsureNone<T>(this Option<T> option)
        where T : notnull {
        if (option.IsSome) {
            Assert.Fail("Expected Option.None but was Some.");
        }

        return new NoneAssertion();
    }

    /// <summary>
    ///     Asserts that the Either is Left and returns a LeftAssertion for its value.
    /// </summary>
    /// <typeparam name="TLeft">The left type.</typeparam>
    /// <typeparam name="TRight">The right type.</typeparam>
    /// <param name="either">The either to assert.</param>
    /// <returns>A LeftAssertion over the Left value.</returns>
    public static LeftAssertion<TLeft, TRight> EnsureLeft<TLeft, TRight>(this Either<TLeft, TRight> either)
        where TLeft : notnull
        where TRight : notnull {
        if (!either.Left(out var left, out _)) {
            Assert.Fail("Expected Either.Left but was Right.");
        }

        return new LeftAssertion<TLeft, TRight>(left);
    }

    /// <summary>
    ///     Asserts that the Either is Right and returns a RightAssertion for its value.
    /// </summary>
    /// <typeparam name="TLeft">The left type.</typeparam>
    /// <typeparam name="TRight">The right type.</typeparam>
    /// <param name="either">The either to assert.</param>
    /// <returns>A RightAssertion over the Right value.</returns>
    public static RightAssertion<TLeft, TRight> EnsureRight<TLeft, TRight>(this Either<TLeft, TRight> either)
        where TLeft : notnull
        where TRight : notnull {
        if (!either.Right(out _, out var right)) {
            Assert.Fail("Expected Either.Right but was Left.");
        }

        return new RightAssertion<TLeft, TRight>(right);
    }

    /// <summary>
    ///     Asserts that the task resolves to a successful Result and returns a SuccessAssertion for Unit.
    /// </summary>
    /// <param name="task">The task producing the result.</param>
    /// <returns>A SuccessAssertion wrapping Unit.</returns>
    public static async Task<SuccessAssertion<Unit>> EnsureSuccess(this Task<Result> task) {
        return (await task.ConfigureAwait(false)).EnsureSuccess();
    }

    /// <summary>
    ///     Asserts that the task resolves to a failure Result and returns a FailureAssertion.
    /// </summary>
    /// <param name="task">The task producing the result.</param>
    /// <returns>A FailureAssertion.</returns>
    public static async Task<FailureAssertion> EnsureFailure(this Task<Result> task) {
        return (await task.ConfigureAwait(false)).EnsureFailure();
    }

    /// <summary>
    ///     Asserts that the task resolves to a successful Result and returns a SuccessAssertion for its value.
    /// </summary>
    /// <typeparam name="T1">The result value type.</typeparam>
    /// <param name="task">The task producing the result.</param>
    /// <returns>A SuccessAssertion for the contained value.</returns>
    public static async Task<SuccessAssertion<T1>> EnsureSuccess<T1>(this Task<Result<T1>> task)
        where T1 : notnull {
        return (await task.ConfigureAwait(false)).EnsureSuccess();
    }

    /// <summary>
    ///     Asserts that the task resolves to a failure Result and returns a FailureAssertion.
    /// </summary>
    /// <typeparam name="T1">The result value type.</typeparam>
    /// <param name="task">The task producing the result.</param>
    /// <returns>A FailureAssertion.</returns>
    public static async Task<FailureAssertion> EnsureFailure<T1>(this Task<Result<T1>> task)
        where T1 : notnull {
        return (await task.ConfigureAwait(false)).EnsureFailure();
    }

    /// <summary>
    ///     Asserts that the task resolves to an Option in the Some state and returns a SomeAssertion for its value.
    /// </summary>
    /// <typeparam name="T">The option value type.</typeparam>
    /// <param name="task">The task producing the option.</param>
    /// <returns>A SomeAssertion for the contained value.</returns>
    public static async Task<SomeAssertion<T>> EnsureSome<T>(this Task<Option<T>> task)
        where T : notnull {
        return (await task.ConfigureAwait(false)).EnsureSome();
    }

    /// <summary>
    ///     Asserts that the task resolves to an Option in the None state.
    /// </summary>
    /// <typeparam name="T">The option value type.</typeparam>
    /// <param name="task">The task producing the option.</param>
    /// <returns>A NoneAssertion to allow further chaining.</returns>
    public static async Task<NoneAssertion> EnsureNone<T>(this Task<Option<T>> task)
        where T : notnull {
        return (await task.ConfigureAwait(false)).EnsureNone();
    }

    /// <summary>
    ///     Asserts that the task resolves to an Either in the Left variant and returns a LeftAssertion for further checks.
    /// </summary>
    /// <typeparam name="TLeft">The left type.</typeparam>
    /// <typeparam name="TRight">The right type.</typeparam>
    /// <param name="task">The task that produces the Either.</param>
    /// <returns>A LeftAssertion over the Left value.</returns>
    public static async Task<LeftAssertion<TLeft, TRight>> EnsureLeft<TLeft, TRight>(this Task<Either<TLeft, TRight>> task)
        where TLeft : notnull
        where TRight : notnull {
        return (await task.ConfigureAwait(false)).EnsureLeft();
    }

    /// <summary>
    ///     Ensures that an Either task resolves to the Right variant and returns an assertion object to further affirm the
    ///     Right value.
    /// </summary>
    /// <typeparam name="TLeft">The type of the Left variant.</typeparam>
    /// <typeparam name="TRight">The type of the Right variant.</typeparam>
    /// <param name="task">The Task producing an Either to validate as Right.</param>
    /// <returns>A RightAssertion instance encapsulating the Right value for further verification or assertions.</returns>
    public static async Task<RightAssertion<TLeft, TRight>> EnsureRight<TLeft, TRight>(this Task<Either<TLeft, TRight>> task)
        where TLeft : notnull
        where TRight : notnull {
        return (await task.ConfigureAwait(false)).EnsureRight();
    }

    /// <summary>
    ///     Asserts that the ValueTask of Result resolves to a successful outcome.
    /// </summary>
    /// <param name="vt">The ValueTask to await.</param>
    /// <returns>A SuccessAssertion wrapping Unit.</returns>
    public static async ValueTask<SuccessAssertion<Unit>> EnsureSuccess(this ValueTask<Result> vt) {
        return (await vt.ConfigureAwait(false)).EnsureSuccess();
    }

    /// <summary>
    ///     Asserts that the ValueTask of Result resolves to a failure and returns a FailureAssertion.
    /// </summary>
    /// <param name="vt">The ValueTask to await.</param>
    /// <returns>A FailureAssertion.</returns>
    public static async ValueTask<FailureAssertion> EnsureFailure(this ValueTask<Result> vt) {
        return (await vt.ConfigureAwait(false)).EnsureFailure();
    }

    /// <summary>
    ///     Asserts that the ValueTask resolves to a successful Result and returns a SuccessAssertion for its value.
    /// </summary>
    /// <typeparam name="T1">The result value type.</typeparam>
    /// <param name="vt">The ValueTask to await.</param>
    /// <returns>A SuccessAssertion for the contained value.</returns>
    public static async ValueTask<SuccessAssertion<T1>> EnsureSuccess<T1>(this ValueTask<Result<T1>> vt)
        where T1 : notnull {
        return (await vt.ConfigureAwait(false)).EnsureSuccess();
    }

    /// <summary>
    ///     Asserts that the ValueTask resolves to a failure Result and returns a FailureAssertion.
    /// </summary>
    /// <typeparam name="T1">The result value type.</typeparam>
    /// <param name="vt">The ValueTask to await.</param>
    /// <returns>A FailureAssertion.</returns>
    public static async ValueTask<FailureAssertion> EnsureFailure<T1>(this ValueTask<Result<T1>> vt)
        where T1 : notnull {
        return (await vt.ConfigureAwait(false)).EnsureFailure();
    }

    /// <summary>
    ///     Asserts that the ValueTask resolves to an Option in the Some state and returns a SomeAssertion for its value.
    /// </summary>
    /// <typeparam name="T">The option value type.</typeparam>
    /// <param name="vt">The ValueTask to await.</param>
    /// <returns>A SomeAssertion for the contained value.</returns>
    public static async ValueTask<SomeAssertion<T>> EnsureSome<T>(this ValueTask<Option<T>> vt)
        where T : notnull {
        return (await vt.ConfigureAwait(false)).EnsureSome();
    }

    /// <summary>
    ///     Asserts that the ValueTask resolves to an Option in the None state.
    /// </summary>
    /// <typeparam name="T">The option value type.</typeparam>
    /// <param name="vt">The ValueTask to await.</param>
    /// <returns>A NoneAssertion to allow further chaining.</returns>
    public static async ValueTask<NoneAssertion> EnsureNone<T>(this ValueTask<Option<T>> vt)
        where T : notnull {
        return (await vt.ConfigureAwait(false)).EnsureNone();
    }

    /// <summary>
    ///     Asserts that the ValueTask resolves to an Either in the Left variant and returns a LeftAssertion for further
    ///     checks.
    /// </summary>
    /// <typeparam name="TLeft">The left type.</typeparam>
    /// <typeparam name="TRight">The right type.</typeparam>
    /// <param name="vt">The ValueTask to await.</param>
    /// <returns>A LeftAssertion over the Left value.</returns>
    public static async ValueTask<LeftAssertion<TLeft, TRight>> EnsureLeft<TLeft, TRight>(this ValueTask<Either<TLeft, TRight>> vt)
        where TLeft : notnull
        where TRight : notnull {
        return (await vt.ConfigureAwait(false)).EnsureLeft();
    }

    /// <summary>
    ///     Asserts that the ValueTask resolves to an Either in the Right variant and returns an assertion object for the Right
    ///     value.
    /// </summary>
    /// <typeparam name="TLeft">The type of the Left variant.</typeparam>
    /// <typeparam name="TRight">The type of the Right variant.</typeparam>
    /// <param name="vt">The ValueTask to await.</param>
    /// <returns>A RightAssertion instance encapsulating the Right value for further verification or assertions.</returns>
    public static async ValueTask<RightAssertion<TLeft, TRight>> EnsureRight<TLeft, TRight>(this ValueTask<Either<TLeft, TRight>> vt)
        where TLeft : notnull
        where TRight : notnull {
        return (await vt.ConfigureAwait(false)).EnsureRight();
    }

    /// <summary>
    ///     Applies a predicate to the success value, failing the assertion if the predicate is not satisfied.
    /// </summary>
    /// <typeparam name="T">The success value type.</typeparam>
    /// <param name="assertion">The success assertion to evaluate.</param>
    /// <param name="predicate">The predicate to test the value against.</param>
    /// <param name="because">An optional reason to include if the assertion fails.</param>
    /// <returns>The same SuccessAssertion instance.</returns>
    public static SuccessAssertion<T> Where<T>(this SuccessAssertion<T> assertion,
                                               Func<T, bool>            predicate,
                                               string?                  because = null)
        where T : notnull {
        ArgumentNullException.ThrowIfNull(predicate);
        if (!predicate(assertion.Value)) {
            Assert.Fail(because ?? $"Value '{assertion.Value}' does not satisfy predicate.");
        }

        return assertion;
    }

    /// <summary>
    ///     Applies a predicate to the error of a failure assertion, failing if the predicate is not satisfied.
    /// </summary>
    /// <param name="assertion">The failure assertion to evaluate.</param>
    /// <param name="predicate">The predicate to test the error against.</param>
    /// <param name="because">An optional reason to include if the assertion fails.</param>
    /// <returns>The same FailureAssertion instance.</returns>
    public static FailureAssertion Where(this FailureAssertion assertion,
                                         Func<Exception, bool> predicate,
                                         string?               because = null) {
        ArgumentNullException.ThrowIfNull(predicate);
        if (!predicate(assertion.Error)) {
            Assert.Fail(because ?? $"Error '{assertion.Error.Message}' does not satisfy predicate.");
        }

        return assertion;
    }

    /// <summary>
    ///     Applies a predicate to the value inside a SomeAssertion, failing if the predicate is not satisfied.
    /// </summary>
    /// <typeparam name="T">The option value type.</typeparam>
    /// <param name="assertion">The Some assertion to evaluate.</param>
    /// <param name="predicate">The predicate to test the value against.</param>
    /// <param name="because">An optional reason to include if the assertion fails.</param>
    /// <returns>The same SomeAssertion instance.</returns>
    public static SomeAssertion<T> Where<T>(this SomeAssertion<T> assertion,
                                            Func<T, bool>         predicate,
                                            string?               because = null)
        where T : notnull {
        ArgumentNullException.ThrowIfNull(predicate);
        if (!predicate(assertion.Value)) {
            Assert.Fail(because ?? $"Value '{assertion.Value}' does not satisfy predicate.");
        }

        return assertion;
    }

    /// <summary>
    ///     Filters the left value of the given <see cref="LeftAssertion{TLeft,TRight}" /> based on a predicate.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value.</typeparam>
    /// <param name="assertion">The <see cref="LeftAssertion{TLeft,TRight}" /> being filtered.</param>
    /// <param name="predicate">The predicate to evaluate the left value against.</param>
    /// <param name="because">The optional reason why the assertion is performed.</param>
    /// <returns>The same <see cref="LeftAssertion{TLeft,TRight}" /> instance if the predicate passes.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="predicate" /> is null.</exception>
    /// <exception cref="Xunit.Sdk.XunitException">Thrown if the predicate is not satisfied by the left value.</exception>
    public static LeftAssertion<TLeft, TRight> WhereLeft<TLeft, TRight>(this LeftAssertion<TLeft, TRight> assertion,
                                                                        Func<TLeft, bool>                 predicate,
                                                                        string?                           because = null)
        where TLeft : notnull
        where TRight : notnull {
        ArgumentNullException.ThrowIfNull(predicate);
        if (!predicate(assertion.Value)) {
            Assert.Fail(because ?? $"Left value '{assertion.Value}' does not satisfy predicate.");
        }

        return assertion;
    }

    /// Applies a predicate to the "Right" value of the assertion, asserting that the predicate is satisfied.
    /// If the predicate is not satisfied, the test fails with the provided message or a default message.
    /// <param name="assertion">The assertion containing the "Right" value to test.</param>
    /// <param name="predicate">The predicate function to apply to the "Right" value.</param>
    /// <param name="because">An optional explanatory reason for the assertion failure.</param>
    /// <returns>The original RightAssertion object for chaining further operations.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the predicate is null.</exception>
    /// <exception cref="Xunit.Sdk.XunitException">Thrown when the "Right" value does not satisfy the predicate.</exception>
    public static RightAssertion<TLeft, TRight> WhereRight<TLeft, TRight>(this RightAssertion<TLeft, TRight> assertion,
                                                                          Func<TRight, bool>                 predicate,
                                                                          string?                            because = null)
        where TLeft : notnull
        where TRight : notnull {
        ArgumentNullException.ThrowIfNull(predicate);
        if (!predicate(assertion.Value)) {
            Assert.Fail(because ?? $"Right value '{assertion.Value}' does not satisfy predicate.");
        }

        return assertion;
    }
}
