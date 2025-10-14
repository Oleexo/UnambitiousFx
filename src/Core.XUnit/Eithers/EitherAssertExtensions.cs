using System.Diagnostics;
using UnambitiousFx.Core.Eithers;
using Xunit;

namespace UnambitiousFx.Core.XUnit.Eithers;

/// <summary>
///     Provides xUnit assertion extensions for <see cref="Either{TLeft,TRight}" /> types,
///     allowing for validation of left and right values within either results and asynchronous operations.
/// </summary>
[DebuggerStepThrough]
public static class EitherAssertExtensions {
    /// <summary>
    ///     Constant string message used to indicate that an assertion expecting the Either value to be
    ///     a Left type has failed because the actual value was a Right type.
    /// </summary>
    private const string LeftExpected = "Expected Either to be Left but was Right.";

    /// <summary>
    ///     Defines a constant string message used to indicate that an <c>Either</c> value
    ///     was expected to be of the <c>Right</c> type but was actually of the <c>Left</c> type.
    /// </summary>
    private const string RightExpected = "Expected Either to be Right but was Left.";

    /// <summary>
    ///     Asserts that the Either is in the Left state and extracts the Left value.
    /// </summary>
    /// <typeparam name="TLeft">The type of the Left value.</typeparam>
    /// <typeparam name="TRight">The type of the Right value.</typeparam>
    /// <param name="either">The Either structure to be asserted.</param>
    /// <param name="left">The extracted Left value if the Either is in the Left state.</param>
    /// <returns>The original Either structure for additional assertions.</returns>
    public static Either<TLeft, TRight> ShouldBeLeft<TLeft, TRight>(this Either<TLeft, TRight> either,
                                                                    out  TLeft                 left)
        where TLeft : notnull
        where TRight : notnull {
        if (!either.Left(out var tmpLeft, out _)) {
            Assert.Fail(LeftExpected);
        }

        left = tmpLeft;
        return either;
    }

    /// <summary>
    ///     Verifies that the provided Either instance is a Left and performs the specified assertion on the Left value.
    /// </summary>
    /// <typeparam name="TLeft">The type of the Left value.</typeparam>
    /// <typeparam name="TRight">The type of the Right value.</typeparam>
    /// <param name="either">The Either instance to validate.</param>
    /// <param name="assert">An action to perform assertions on the Left value.</param>
    /// <returns>The original Either instance after validation and assertion.</returns>
    public static Either<TLeft, TRight> ShouldBeLeft<TLeft, TRight>(this Either<TLeft, TRight> either,
                                                                    Action<TLeft>              assert)
        where TLeft : notnull
        where TRight : notnull {
        either.ShouldBeLeft(out var l);
        assert(l);
        return either;
    }

    /// <summary>
    ///     Asserts that the Either instance is in a Right state and extracts the Right value.
    /// </summary>
    /// <typeparam name="TLeft">The type of the Left value.</typeparam>
    /// <typeparam name="TRight">The type of the Right value.</typeparam>
    /// <param name="either">The Either instance to evaluate.</param>
    /// <param name="right">The extracted Right value if the assertion succeeds.</param>
    /// <returns>The original Either instance for further assertions.</returns>
    public static Either<TLeft, TRight> ShouldBeRight<TLeft, TRight>(this Either<TLeft, TRight> either,
                                                                     out  TRight                right)
        where TLeft : notnull
        where TRight : notnull {
        if (!either.Right(out _, out var tmpRight)) {
            Assert.Fail(RightExpected);
        }

        right = tmpRight;
        return either;
    }

    /// <summary>
    ///     Asserts that the Either is a Right value and applies the provided assertion action to it.
    /// </summary>
    /// <typeparam name="TLeft">The type of the Left value.</typeparam>
    /// <typeparam name="TRight">The type of the Right value.</typeparam>
    /// <param name="either">The Either instance to be checked.</param>
    /// <param name="assert">The action to apply to the Right value.</param>
    /// <returns>The original Either instance.</returns>
    public static Either<TLeft, TRight> ShouldBeRight<TLeft, TRight>(this Either<TLeft, TRight> either,
                                                                     Action<TRight>             assert)
        where TLeft : notnull
        where TRight : notnull {
        either.ShouldBeRight(out var r);
        assert(r);
        return either;
    }

    /// <summary>
    ///     Verifies that the Either instance contains a Left value and extracts the Left value.
    /// </summary>
    /// <typeparam name="TLeft">The type of the Left value.</typeparam>
    /// <typeparam name="TRight">The type of the Right value.</typeparam>
    /// <param name="awaitable">The Task producing the Either instance to verify.</param>
    /// <param name="left">The extracted Left value if the instance is a Left.</param>
    /// <returns>The original Either instance.</returns>
    // Removed async + out param restriction by synchronously awaiting (acceptable in test assertion helpers)
    public static Task<Either<TLeft, TRight>> ShouldBeLeft<TLeft, TRight>(this Task<Either<TLeft, TRight>> awaitable,
                                                                          out  TLeft                       left)
        where TLeft : notnull
        where TRight : notnull {
        var e = awaitable.GetAwaiter()
                         .GetResult();
        e.ShouldBeLeft(out left);
        return Task.FromResult(e);
    }

    /// <summary>
    ///     Asserts that the Either instance is Right and returns it, extracting the Right value.
    /// </summary>
    /// <typeparam name="TLeft">The type of the Left value.</typeparam>
    /// <typeparam name="TRight">The type of the Right value.</typeparam>
    /// <param name="awaitable">The Task producing the Either instance to assert.</param>
    /// <param name="right">The extracted Right value if the assertion succeeds.</param>
    /// <returns>The original Either instance if it is Right.</returns>
    public static Task<Either<TLeft, TRight>> ShouldBeRight<TLeft, TRight>(this Task<Either<TLeft, TRight>> awaitable,
                                                                           out  TRight                      right)
        where TLeft : notnull
        where TRight : notnull {
        var e = awaitable.GetAwaiter()
                         .GetResult();
        e.ShouldBeRight(out right);
        return Task.FromResult(e);
    }

    /// <summary>
    ///     Asserts that an Either instance contains a Left value, extracting the Left value if the assertion passes.
    /// </summary>
    /// <typeparam name="TLeft">The type of the Left value.</typeparam>
    /// <typeparam name="TRight">The type of the Right value.</typeparam>
    /// <param name="awaitable">The ValueTask producing the Either instance to assert.</param>
    /// <param name="left">The extracted Left value if the assertion passes.</param>
    /// <returns>The original Either instance.</returns>
    public static ValueTask<Either<TLeft, TRight>> ShouldBeLeft<TLeft, TRight>(this ValueTask<Either<TLeft, TRight>> awaitable,
                                                                               out  TLeft                            left)
        where TLeft : notnull
        where TRight : notnull {
        var e = awaitable.GetAwaiter()
                         .GetResult();
        e.ShouldBeLeft(out left);
        return ValueTask.FromResult(e);
    }

    /// <summary>Asserts that the Either is Right and retrieves its right value.</summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value.</typeparam>
    /// <param name="awaitable">The ValueTask producing the Either value to assert.</param>
    /// <param name="right">The retrieved right value when the Either is Right.</param>
    /// <returns>The original Either instance.</returns>
    public static ValueTask<Either<TLeft, TRight>> ShouldBeRight<TLeft, TRight>(this ValueTask<Either<TLeft, TRight>> awaitable,
                                                                                out  TRight                           right)
        where TLeft : notnull
        where TRight : notnull {
        var e = awaitable.GetAwaiter()
                         .GetResult();
        e.ShouldBeRight(out right);
        return ValueTask.FromResult(e);
    }

    // Custom message overloads & predicate-based assertions
    /// <summary>
    ///     Asserts that the provided Either is a Left with the specified condition, and outputs the Left value.
    /// </summary>
    /// <param name="either">The <see cref="Either{TLeft,TRight}" /> instance to check.</param>
    /// <param name="left">When the assertion succeeds, the extracted Left value is output here.</param>
    /// <param name="because">The custom message provided, used when the assertion fails.</param>
    /// <typeparam name="TLeft">The type of the Left value.</typeparam>
    /// <typeparam name="TRight">The type of the Right value.</typeparam>
    /// <returns>The original <see cref="Either{TLeft,TRight}" /> instance if the assertion succeeds.</returns>
    public static Either<TLeft, TRight> ShouldBeLeft<TLeft, TRight>(this Either<TLeft, TRight> either,
                                                                    out  TLeft                 left,
                                                                    string                     because)
        where TLeft : notnull
        where TRight : notnull {
        if (!either.Left(out var l, out _)) {
            Assert.Fail(because);
        }

        left = l;
        return either;
    }

    /// <summary>
    ///     Asserts that the Either is Right, extracts the Right value, and provides a custom failure message if the assertion
    ///     fails.
    /// </summary>
    /// <param name="either">The Either object being asserted.</param>
    /// <param name="right">The extracted Right value if the assertion passes.</param>
    /// <param name="because">The custom failure message provided if the assertion fails.</param>
    /// <returns>The original Either object after assertion.</returns>
    public static Either<TLeft, TRight> ShouldBeRight<TLeft, TRight>(this Either<TLeft, TRight> either,
                                                                     out  TRight                right,
                                                                     string                     because)
        where TLeft : notnull
        where TRight : notnull {
        if (!either.Right(out _, out var r)) {
            Assert.Fail(because);
        }

        right = r;
        return either;
    }

    /// <summary>
    ///     Asserts that the Either is a Left value and satisfies the given predicate.
    /// </summary>
    /// <param name="either">The Either instance to be evaluated.</param>
    /// <param name="predicate">The predicate to evaluate the Left value.</param>
    /// <param name="because">Optional explanation message if the assertion fails.</param>
    /// <typeparam name="TLeft">The type of the Left value.</typeparam>
    /// <typeparam name="TRight">The type of the Right value.</typeparam>
    /// <returns>The original Either instance for further assertions or processing.</returns>
    public static Either<TLeft, TRight> ShouldBeLeftWhere<TLeft, TRight>(this Either<TLeft, TRight> either,
                                                                         Func<TLeft, bool>          predicate,
                                                                         string?                    because = null)
        where TLeft : notnull
        where TRight : notnull {
        ArgumentNullException.ThrowIfNull(predicate);
        if (!either.Left(out var l, out _)) {
            Assert.Fail(because ?? "Expected Either.Left but was Right.");
        }

        if (!predicate(l)) {
            Assert.Fail(because ?? $"Left value '{l}' does not satisfy predicate.");
        }

        return either;
    }

    /// <summary>
    ///     Verifies that the Either instance is a Right value and satisfies the specified predicate.
    /// </summary>
    /// <param name="either">The Either instance to verify.</param>
    /// <param name="predicate">The predicate that the Right value must satisfy.</param>
    /// <param name="because">An optional message providing additional context for assertion failures.</param>
    /// <returns>The original Either instance if the assertion is successful.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the predicate is null.</exception>
    /// <exception cref="Xunit.Sdk.XunitException">
    ///     Thrown if the Either is not a Right value or if the Right value does not satisfy the predicate.
    /// </exception>
    public static Either<TLeft, TRight> ShouldBeRightWhere<TLeft, TRight>(this Either<TLeft, TRight> either,
                                                                          Func<TRight, bool>         predicate,
                                                                          string?                    because = null)
        where TLeft : notnull
        where TRight : notnull {
        ArgumentNullException.ThrowIfNull(predicate);
        if (!either.Right(out _, out var r)) {
            Assert.Fail(because ?? "Expected Either.Right but was Left.");
        }

        if (!predicate(r)) {
            Assert.Fail(because ?? $"Right value '{r}' does not satisfy predicate.");
        }

        return either;
    }

    /// <summary>
    ///     Awaits a Task producing an Either and asserts it is Left, ensuring the Left value matches the provided predicate.
    /// </summary>
    /// <param name="awaitable">The task producing an Either to assert.</param>
    /// <param name="predicate">The predicate that the Left value must satisfy.</param>
    /// <param name="because">A message that explains why the assertion is performed, included in case of failure.</param>
    /// <typeparam name="TLeft">The type of the Left value.</typeparam>
    /// <typeparam name="TRight">The type of the Right value.</typeparam>
    /// <returns>The awaited Either value.</returns>
    public static async Task<Either<TLeft, TRight>> ShouldBeLeftWhereAsync<TLeft, TRight>(this Task<Either<TLeft, TRight>> awaitable,
                                                                                          Func<TLeft, bool>                predicate,
                                                                                          string?                          because = null)
        where TLeft : notnull
        where TRight : notnull {
        var e = await awaitable.ConfigureAwait(false);
        e.ShouldBeLeftWhere(predicate, because);
        return e;
    }

    /// <summary>
    ///     Awaits a Task producing an Either and asserts it is Right based on the given predicate.
    /// </summary>
    /// <param name="awaitable">The Task producing the Either instance to be evaluated.</param>
    /// <param name="predicate">A function used to test the Right value of the Either.</param>
    /// <param name="because">An optional reason why the assertion is being made, used for error messages.</param>
    /// <typeparam name="TLeft">The type of the Left value of the Either.</typeparam>
    /// <typeparam name="TRight">The type of the Right value of the Either.</typeparam>
    /// <returns>The awaited Either instance.</returns>
    public static async Task<Either<TLeft, TRight>> ShouldBeRightWhereAsync<TLeft, TRight>(this Task<Either<TLeft, TRight>> awaitable,
                                                                                           Func<TRight, bool>               predicate,
                                                                                           string?                          because = null)
        where TLeft : notnull
        where TRight : notnull {
        var e = await awaitable.ConfigureAwait(false);
        e.ShouldBeRightWhere(predicate, because);
        return e;
    }

    /// <summary>
    ///     Awaits a ValueTask producing an Either and asserts that the result is Left and satisfies the given predicate.
    /// </summary>
    /// <param name="awaitable">The ValueTask returning an Either to be validated as Left.</param>
    /// <param name="predicate">A function to assert specific conditions on the Left value of the Either.</param>
    /// <param name="because">A custom message to provide context for the assertion failure (optional).</param>
    /// <typeparam name="TLeft">The type of the Left value of the Either.</typeparam>
    /// <typeparam name="TRight">The type of the Right value of the Either.</typeparam>
    /// <returns>The original Either after validating it as Left.</returns>
    public static async ValueTask<Either<TLeft, TRight>> ShouldBeLeftWhereAsync<TLeft, TRight>(this ValueTask<Either<TLeft, TRight>> awaitable,
                                                                                               Func<TLeft, bool>                     predicate,
                                                                                               string?                               because = null)
        where TLeft : notnull
        where TRight : notnull {
        var e = await awaitable.ConfigureAwait(false);
        e.ShouldBeLeftWhere(predicate, because);
        return e;
    }

    /// <summary>
    ///     Awaits a ValueTask producing an Either and asserts it is Right based on the specified predicate.
    /// </summary>
    /// <param name="awaitable">The ValueTask producing the Either value to be asserted.</param>
    /// <param name="predicate">The predicate to evaluate on the Right value.</param>
    /// <param name="because">
    ///     An optional message specifying why the assertion is expected to be valid.
    /// </param>
    /// <typeparam name="TLeft">The type of the Left value in the Either.</typeparam>
    /// <typeparam name="TRight">The type of the Right value in the Either.</typeparam>
    /// <returns>
    ///     The original Either value after completing the assertion.
    /// </returns>
    public static async ValueTask<Either<TLeft, TRight>> ShouldBeRightWhereAsync<TLeft, TRight>(this ValueTask<Either<TLeft, TRight>> awaitable,
                                                                                                Func<TRight, bool>                    predicate,
                                                                                                string?                               because = null)
        where TLeft : notnull
        where TRight : notnull {
        var e = await awaitable.ConfigureAwait(false);
        e.ShouldBeRightWhere(predicate, because);
        return e;
    }
}
