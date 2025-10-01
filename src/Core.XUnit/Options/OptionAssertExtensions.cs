using System.Diagnostics;
using UnambitiousFx.Core.Options;
using Xunit;

namespace UnambitiousFx.Core.XUnit.Options;

/// <summary>
/// Provides extension methods for asserting the state of <see cref="Option{TValue}"/> instances
/// in xUnit tests. Includes synchronous, asynchronous, and predicate-based assertions.
/// </summary>
[DebuggerStepThrough]
public static class OptionAssertExtensions {
    /// <summary>
    /// Represents the error message used by the Option assertion extensions when an expected
    /// <c>Option</c> is of type None but was expected to be of type Some.
    /// </summary>
    private const string SomeExpected = "Expected Option to be Some but was None.";

    /// <summary>
    /// Constant message used to indicate that the expected Option was None, but Some was found instead.
    /// </summary>
    private const string NoneExpected = "Expected Option to be None but was Some.";

    /// <summary>
    /// Validates that the specified <see cref="Option{T}"/> contains a value and extracts the contained value.
    /// </summary>
    /// <typeparam name="T">The type of the value contained in the <see cref="Option{T}"/>.</typeparam>
    /// <param name="option">The <see cref="Option{T}"/> to validate and extract the value from.</param>
    /// <param name="value">When the method returns, contains the value extracted from the <see cref="Option{T}"/>.</param>
    /// <returns>The original <see cref="Option{T}"/> after the assertion.</returns>
    /// <exception cref="Xunit.Sdk.XunitException">Thrown if the <see cref="Option{T}"/> is None.</exception>
    public static Option<T> ShouldBeSome<T>(this Option<T> option,
                                            out  T         value)
        where T : notnull {
        if (!option.Some(out var tmp)) {
            Assert.Fail(SomeExpected);
        }

        value = tmp;
        return option;
    }

    /// <summary>
    /// Asserts that the Option contains a value and invokes a specified assertion on the value.
    /// </summary>
    /// <typeparam name="T">The type of the value contained within the Option.</typeparam>
    /// <param name="option">The Option instance to be asserted.</param>
    /// <param name="assert">The Action to perform on the extracted value for additional assertions.</param>
    /// <returns>The original Option instance after performing the assertion.</returns>
    public static Option<T> ShouldBeSome<T>(this Option<T> option,
                                            Action<T>      assert)
        where T : notnull {
        option.ShouldBeSome(out var v);
        assert(v);
        return option;
    }

    /// <summary>
    /// Verifies that the Option is in a None state and returns the Option.
    /// </summary>
    /// <param name="option">The Option to verify.</param>
    /// <returns>The same Option if it is in a None state; otherwise, this method will throw.</returns>
    public static Option<T> ShouldBeNone<T>(this Option<T> option)
        where T : notnull {
        if (option.IsSome) {
            Assert.Fail(NoneExpected);
        }

        return option;
    }

    /// <summary>
    /// Awaits a Task returning Option and asserts that the result is Some, extracting the value.
    /// </summary>
    /// <param name="awaitable">The Task representing an asynchronous operation that returns an Option.</param>
    /// <param name="value">The extracted value if the Option is Some.</param>
    /// <returns>The original Task's Option result.</returns>
    public static Task<Option<T>> ShouldBeSome<T>(this Task<Option<T>> awaitable,
                                                  out  T               value)
        where T : notnull {
        var opt = awaitable.GetAwaiter().GetResult();
        opt.ShouldBeSome(out value);
        return Task.FromResult(opt);
    }

    /// <summary>
    /// Asserts that the awaited Option is Some and applies an additional assertion on its value.
    /// </summary>
    /// <param name="awaitable">The task producing the Option.</param>
    /// <param name="assert">The assertion to perform on the extracted value.</param>
    /// <typeparam name="T">The type of the value contained in the Option.</typeparam>
    /// <returns>The original Option after the assertion.</returns>
    public static async Task<Option<T>> ShouldBeSome<T>(this Task<Option<T>> awaitable,
                                                        Action<T>            assert)
        where T : notnull {
        var opt = await awaitable.ConfigureAwait(false);
        opt.ShouldBeSome(assert);
        return opt;
    }

    /// <summary>Asserts that the awaited Option instance is None.</summary>
    /// <param name="awaitable">The Task producing the Option.</param>
    /// <typeparam name="T">The type of the value contained in the Option.</typeparam>
    /// <returns>The original Option instance if the assertion passes.</returns>
    public static async Task<Option<T>> ShouldBeNone<T>(this Task<Option<T>> awaitable)
        where T : notnull {
        var opt = await awaitable.ConfigureAwait(false);
        opt.ShouldBeNone();
        return opt;
    }

    /// <summary>
    /// Asserts that the given Option is in a Some state and extracts its value.
    /// </summary>
    /// <typeparam name="T">The type of the value contained within the Option.</typeparam>
    /// <param name="awaitable">The ValueTask producing the Option.</param>
    /// <param name="value">The extracted value if the assertion succeeds.</param>
    /// <returns>The original Option to allow further chaining or inspection.</returns>
    public static ValueTask<Option<T>> ShouldBeSome<T>(this ValueTask<Option<T>> awaitable,
                                                       out  T                    value)
        where T : notnull {
        var opt = awaitable.GetAwaiter().GetResult();
        opt.ShouldBeSome(out value);
        return ValueTask.FromResult(opt);
    }

    /// <summary>Asserts that the Option is None.</summary>
    /// <typeparam name="T">The type of the value contained in the Option.</typeparam>
    /// <param name="awaitable">The ValueTask producing the Option instance to be asserted.</param>
    /// <returns>The original Option instance after the assertion.</returns>
    public static async ValueTask<Option<T>> ShouldBeNone<T>(this ValueTask<Option<T>> awaitable)
        where T : notnull {
        var opt = await awaitable.ConfigureAwait(false);
        opt.ShouldBeNone();
        return opt;
    }

    // Custom message overloads & predicate-based assertions
    /// <summary>
    /// Asserts that the given option contains a value and extracts the value if present.
    /// </summary>
    /// <param name="option">The option instance to be checked.</param>
    /// <param name="value">The extracted value if the option is Some.</param>
    /// <param name="because">The reason to include if the assertion fails.</param>
    /// <returns>The original option instance.</returns>
    public static Option<T> ShouldBeSome<T>(this Option<T> option,
                                            out  T         value,
                                            string         because)
        where T : notnull {
        if (!option.Some(out var tmp)) Assert.Fail(because);
        value = tmp;
        return option;
    }

    /// <summary>
    /// Asserts that the given <see cref="Option{T}"/> instance is None.
    /// </summary>
    /// <typeparam name="T">The type of the value encapsulated by the option.</typeparam>
    /// <param name="option">The option to be checked.</param>
    /// <param name="because">The reason to include if the assertion fails.</param>
    /// <returns>The original option instance, for potential chaining or further assertions.</returns>
    public static Option<T> ShouldBeNone<T>(this Option<T> option,
                                            string         because)
        where T : notnull {
        if (option.IsSome) Assert.Fail(because);
        return option;
    }

    /// <summary>
    /// Asserts that the provided Option is Some, its value satisfies the given predicate, and returns the Option.
    /// </summary>
    /// <param name="option">The Option to assert.</param>
    /// <param name="predicate">A function that evaluates whether the value satisfies the desired condition.</param>
    /// <param name="because">An optional message to include if the assertion fails.</param>
    /// <typeparam name="T">The type of the value contained in the Option.</typeparam>
    /// <returns>The same Option, if the assertion is successful.</returns>
    public static Option<T> ShouldBeSomeWhere<T>(this Option<T> option,
                                                 Func<T, bool>  predicate,
                                                 string?        because = null)
        where T : notnull {
        ArgumentNullException.ThrowIfNull(predicate);
        if (!option.Some(out var value)) Assert.Fail(because ?? "Expected Option.Some but was None.");
        if (!predicate(value)) Assert.Fail(because          ?? $"Value '{value}' does not satisfy predicate.");
        return option;
    }

    /// <summary>
    /// Asserts that the Option is None and that the provided predicate for the None state evaluates to true.
    /// </summary>
    /// <param name="option">The Option instance being asserted.</param>
    /// <param name="predicate">A function that defines an additional condition for the None state.</param>
    /// <param name="because">An optional explanation for the assertion or failure.</param>
    /// <returns>The original Option instance for further chaining.</returns>
    public static Option<T> ShouldBeNoneWhere<T>(this Option<T> option,
                                                 Func<bool>     predicate,
                                                 string?        because = null)
        where T : notnull {
        // Allows attaching additional condition about None context
        if (option.IsSome) Assert.Fail(because ?? "Expected Option.None but was Some.");
        if (!predicate()) Assert.Fail(because  ?? "Provided predicate for None state failed.");
        return option;
    }

    /// <summary>
    /// Awaits a Task returning an Option and asserts that it is Some and matches the provided predicate.
    /// </summary>
    /// <param name="awaitable">The Task that evaluates to an Option.</param>
    /// <param name="predicate">The condition that the contained value must satisfy.</param>
    /// <param name="because">An optional explanation for the assertion.</param>
    /// <returns>The original Option if the assertion is met.</returns>
    public static async Task<Option<T>> ShouldBeSomeWhereAsync<T>(this Task<Option<T>> awaitable,
                                                                  Func<T, bool>        predicate,
                                                                  string?              because = null)
        where T : notnull {
        var opt = await awaitable.ConfigureAwait(false);
        opt.ShouldBeSomeWhere(predicate, because);
        return opt;
    }

    /// <summary>
    /// Awaits a Task returning Option and asserts None if the provided predicate is true.
    /// </summary>
    /// <param name="awaitable">The Task of Option to be awaited and asserted.</param>
    /// <param name="predicate">A function that defines the condition to assert None.</param>
    /// <param name="because">An optional explanation message for the assertion.</param>
    /// <typeparam name="T">The type of the value contained in the Option.</typeparam>
    /// <returns>The awaited Option after the assertion.</returns>
    public static async Task<Option<T>> ShouldBeNoneWhereAsync<T>(this Task<Option<T>> awaitable,
                                                                  Func<bool>           predicate,
                                                                  string?              because = null)
        where T : notnull {
        var opt = await awaitable.ConfigureAwait(false);
        opt.ShouldBeNoneWhere(predicate, because);
        return opt;
    }

    /// <summary>
    /// Awaits a ValueTask returning an Option and asserts that the option contains a value satisfying the specified predicate.
    /// </summary>
    /// <param name="awaitable">The ValueTask of Option to be awaited and examined.</param>
    /// <param name="predicate">The predicate to validate the value within the Option.</param>
    /// <param name="because">An optional message explaining the reason for the assertion.</param>
    /// <typeparam name="T">The type of the value contained within the Option.</typeparam>
    /// <returns>The awaited Option, after ensuring it satisfies the specified predicate.</returns>
    public static async ValueTask<Option<T>> ShouldBeSomeWhereAsync<T>(this ValueTask<Option<T>> awaitable,
                                                                       Func<T, bool>             predicate,
                                                                       string?                   because = null)
        where T : notnull {
        var opt = await awaitable.ConfigureAwait(false);
        opt.ShouldBeSomeWhere(predicate, because);
        return opt;
    }

    /// <summary>
    /// Awaits a ValueTask returning Option and asserts None based on a predicate.
    /// </summary>
    /// <param name="awaitable">The ValueTask containing the Option to be asserted.</param>
    /// <param name="predicate">A function defining the condition that must hold true for the assertion.</param>
    /// <param name="because">An optional descriptive message explaining the reason for the assertion.</param>
    /// <returns>The original Option contained within the ValueTask after the assertion is performed.</returns>
    public static async ValueTask<Option<T>> ShouldBeNoneWhereAsync<T>(this ValueTask<Option<T>> awaitable,
                                                                       Func<bool>                predicate,
                                                                       string?                   because = null)
        where T : notnull {
        var opt = await awaitable.ConfigureAwait(false);
        opt.ShouldBeNoneWhere(predicate, because);
        return opt;
    }
}
