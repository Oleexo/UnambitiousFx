using System.Diagnostics;
using UnambitiousFx.Core.Maybe;
using Xunit;

namespace UnambitiousFx.Core.XUnit.Maybe;

/// <summary>
///     Provides extension methods for asserting the state of <see cref="Maybe{TValue}" /> instances
///     in xUnit tests. Includes synchronous, asynchronous, and predicate-based assertions.
/// </summary>
[DebuggerStepThrough]
public static class MaybeAssertExtensions
{
    /// <summary>
    ///     Represents the error message used by the Maybe assertion extensions when an expected
    ///     <c>Maybe</c> is of type None but was expected to be of type Some.
    /// </summary>
    private const string SomeExpected = "Expected Maybe to be Some but was None.";

    /// <summary>
    ///     Constant message used to indicate that the expected Maybe was None, but Some was found instead.
    /// </summary>
    private const string NoneExpected = "Expected Maybe to be None but was Some.";

    /// <summary>
    ///     Validates that the specified <see cref="Maybe{TValue}" /> contains a value and extracts the contained value.
    /// </summary>
    /// <typeparam name="T">The type of the value contained in the <see cref="Maybe{TValue}" />.</typeparam>
    /// <param name="maybe">The <see cref="Maybe{TValue}" /> to validate and extract the value from.</param>
    /// <param name="value">When the method returns, contains the value extracted from the <see cref="Maybe{TValue}" />.</param>
    /// <returns>The original <see cref="Maybe{TValue}" /> after the assertion.</returns>
    /// <exception cref="Xunit.Sdk.XunitException">Thrown if the <see cref="Maybe{TValue}" /> is None.</exception>
    public static Maybe<T> ShouldBeSome<T>(this Maybe<T> maybe,
                                           out T value)
        where T : notnull
    {
        if (!maybe.Some(out var tmp))
        {
            Assert.Fail(SomeExpected);
        }

        value = tmp;
        return maybe;
    }

    /// <summary>
    ///     Asserts that the Maybe contains a value and invokes a specified assertion on the value.
    /// </summary>
    /// <typeparam name="T">The type of the value contained within the Maybe.</typeparam>
    /// <param name="maybe">The Maybe instance to be asserted.</param>
    /// <param name="assert">The Action to perform on the extracted value for additional assertions.</param>
    /// <returns>The original Maybe instance after performing the assertion.</returns>
    public static Maybe<T> ShouldBeSome<T>(this Maybe<T> maybe,
                                           Action<T> assert)
        where T : notnull
    {
        maybe.ShouldBeSome(out var v);
        assert(v);
        return maybe;
    }

    /// <summary>
    ///     Verifies that the Maybe is in a None state and returns the Maybe.
    /// </summary>
    /// <param name="maybe">The Maybe to verify.</param>
    /// <returns>The same Maybe if it is in a None state; otherwise, this method will throw.</returns>
    public static Maybe<T> ShouldBeNone<T>(this Maybe<T> maybe)
        where T : notnull
    {
        if (maybe.IsSome)
        {
            Assert.Fail(NoneExpected);
        }

        return maybe;
    }

    /// <summary>
    ///     Awaits a Task returning Maybe and asserts that the result is Some, extracting the value.
    /// </summary>
    /// <param name="awaitable">The Task representing an asynchronous operation that returns an Maybe.</param>
    /// <param name="value">The extracted value if the Maybe is Some.</param>
    /// <returns>The original Task's Maybe result.</returns>
    public static Task<Maybe<T>> ShouldBeSome<T>(this Task<Maybe<T>> awaitable,
                                                 out T value)
        where T : notnull
    {
        var opt = awaitable.GetAwaiter()
                           .GetResult();
        opt.ShouldBeSome(out value);
        return Task.FromResult(opt);
    }

    /// <summary>
    ///     Asserts that the awaited Maybe is Some and applies an additional assertion on its value.
    /// </summary>
    /// <param name="awaitable">The task producing the Maybe.</param>
    /// <param name="assert">The assertion to perform on the extracted value.</param>
    /// <typeparam name="T">The type of the value contained in the Maybe.</typeparam>
    /// <returns>The original Maybe after the assertion.</returns>
    public static async Task<Maybe<T>> ShouldBeSome<T>(this Task<Maybe<T>> awaitable,
                                                       Action<T> assert)
        where T : notnull
    {
        var opt = await awaitable.ConfigureAwait(false);
        opt.ShouldBeSome(assert);
        return opt;
    }

    /// <summary>Asserts that the awaited Maybe instance is None.</summary>
    /// <param name="awaitable">The Task producing the Maybe.</param>
    /// <typeparam name="T">The type of the value contained in the Maybe.</typeparam>
    /// <returns>The original Maybe instance if the assertion passes.</returns>
    public static async Task<Maybe<T>> ShouldBeNone<T>(this Task<Maybe<T>> awaitable)
        where T : notnull
    {
        var opt = await awaitable.ConfigureAwait(false);
        opt.ShouldBeNone();
        return opt;
    }

    /// <summary>
    ///     Asserts that the given Maybe is in a Some state and extracts its value.
    /// </summary>
    /// <typeparam name="T">The type of the value contained within the Maybe.</typeparam>
    /// <param name="awaitable">The ValueTask producing the Maybe.</param>
    /// <param name="value">The extracted value if the assertion succeeds.</param>
    /// <returns>The original Maybe to allow further chaining or inspection.</returns>
    public static ValueTask<Maybe<T>> ShouldBeSome<T>(this ValueTask<Maybe<T>> awaitable,
                                                      out T value)
        where T : notnull
    {
        var opt = awaitable.GetAwaiter()
                           .GetResult();
        opt.ShouldBeSome(out value);
        return ValueTask.FromResult(opt);
    }

    /// <summary>Asserts that the Maybe is None.</summary>
    /// <typeparam name="T">The type of the value contained in the Maybe.</typeparam>
    /// <param name="awaitable">The ValueTask producing the Maybe instance to be asserted.</param>
    /// <returns>The original Maybe instance after the assertion.</returns>
    public static async ValueTask<Maybe<T>> ShouldBeNone<T>(this ValueTask<Maybe<T>> awaitable)
        where T : notnull
    {
        var opt = await awaitable.ConfigureAwait(false);
        opt.ShouldBeNone();
        return opt;
    }

    // Custom message overloads & predicate-based assertions
    /// <summary>
    ///     Asserts that the given Maybe contains a value and extracts the value if present.
    /// </summary>
    /// <param name="maybe">The Maybe instance to be checked.</param>
    /// <param name="value">The extracted value if the Maybe is Some.</param>
    /// <param name="because">The reason to include if the assertion fails.</param>
    /// <returns>The original Maybe instance.</returns>
    public static Maybe<T> ShouldBeSome<T>(this Maybe<T> maybe,
                                           out T value,
                                           string because)
        where T : notnull
    {
        if (!maybe.Some(out var tmp))
        {
            Assert.Fail(because);
        }

        value = tmp;
        return maybe;
    }

    /// <summary>
    ///     Asserts that the given <see cref="Maybe{TValue}" /> instance is None.
    /// </summary>
    /// <typeparam name="T">The type of the value encapsulated by the Maybe.</typeparam>
    /// <param name="maybe">The Maybe to be checked.</param>
    /// <param name="because">The reason to include if the assertion fails.</param>
    /// <returns>The original Maybe instance, for potential chaining or further assertions.</returns>
    public static Maybe<T> ShouldBeNone<T>(this Maybe<T> maybe,
                                           string because)
        where T : notnull
    {
        if (maybe.IsSome)
        {
            Assert.Fail(because);
        }

        return maybe;
    }

    /// <summary>
    ///     Asserts that the provided Maybe is Some, its value satisfies the given predicate, and returns the Maybe.
    /// </summary>
    /// <param name="maybe">The Maybe to assert.</param>
    /// <param name="predicate">A function that evaluates whether the value satisfies the desired condition.</param>
    /// <param name="because">An Maybeal message to include if the assertion fails.</param>
    /// <typeparam name="T">The type of the value contained in the Maybe.</typeparam>
    /// <returns>The same Maybe, if the assertion is successful.</returns>
    public static Maybe<T> ShouldBeSomeWhere<T>(this Maybe<T> maybe,
                                                Func<T, bool> predicate,
                                                string? because = null)
        where T : notnull
    {
        ArgumentNullException.ThrowIfNull(predicate);
        if (!maybe.Some(out var value))
        {
            Assert.Fail(because ?? "Expected Maybe.Some but was None.");
        }

        if (!predicate(value))
        {
            Assert.Fail(because ?? $"Value '{value}' does not satisfy predicate.");
        }

        return maybe;
    }

    /// <summary>
    ///     Asserts that the Maybe is None and that the provided predicate for the None state evaluates to true.
    /// </summary>
    /// <param name="maybe">The Maybe instance being asserted.</param>
    /// <param name="predicate">A function that defines an additional condition for the None state.</param>
    /// <param name="because">An Maybeal explanation for the assertion or failure.</param>
    /// <returns>The original Maybe instance for further chaining.</returns>
    public static Maybe<T> ShouldBeNoneWhere<T>(this Maybe<T> maybe,
                                                Func<bool> predicate,
                                                string? because = null)
        where T : notnull
    {
        // Allows attaching additional condition about None context
        if (maybe.IsSome)
        {
            Assert.Fail(because ?? "Expected Maybe.None but was Some.");
        }

        if (!predicate())
        {
            Assert.Fail(because ?? "Provided predicate for None state failed.");
        }

        return maybe;
    }

    /// <summary>
    ///     Awaits a Task returning an Maybe and asserts that it is Some and matches the provided predicate.
    /// </summary>
    /// <param name="awaitable">The Task that evaluates to an Maybe.</param>
    /// <param name="predicate">The condition that the contained value must satisfy.</param>
    /// <param name="because">An Maybeal explanation for the assertion.</param>
    /// <returns>The original Maybe if the assertion is met.</returns>
    public static async Task<Maybe<T>> ShouldBeSomeWhereAsync<T>(this Task<Maybe<T>> awaitable,
                                                                 Func<T, bool> predicate,
                                                                 string? because = null)
        where T : notnull
    {
        var opt = await awaitable.ConfigureAwait(false);
        opt.ShouldBeSomeWhere(predicate, because);
        return opt;
    }

    /// <summary>
    ///     Awaits a Task returning Maybe and asserts None if the provided predicate is true.
    /// </summary>
    /// <param name="awaitable">The Task of Maybe to be awaited and asserted.</param>
    /// <param name="predicate">A function that defines the condition to assert None.</param>
    /// <param name="because">An Maybeal explanation message for the assertion.</param>
    /// <typeparam name="T">The type of the value contained in the Maybe.</typeparam>
    /// <returns>The awaited Maybe after the assertion.</returns>
    public static async Task<Maybe<T>> ShouldBeNoneWhereAsync<T>(this Task<Maybe<T>> awaitable,
                                                                 Func<bool> predicate,
                                                                 string? because = null)
        where T : notnull
    {
        var opt = await awaitable.ConfigureAwait(false);
        opt.ShouldBeNoneWhere(predicate, because);
        return opt;
    }

    /// <summary>
    ///     Awaits a ValueTask returning an Maybe and asserts that the Maybe contains a value satisfying the specified
    ///     predicate.
    /// </summary>
    /// <param name="awaitable">The ValueTask of Maybe to be awaited and examined.</param>
    /// <param name="predicate">The predicate to validate the value within the Maybe.</param>
    /// <param name="because">An Maybeal message explaining the reason for the assertion.</param>
    /// <typeparam name="T">The type of the value contained within the Maybe.</typeparam>
    /// <returns>The awaited Maybe, after ensuring it satisfies the specified predicate.</returns>
    public static async ValueTask<Maybe<T>> ShouldBeSomeWhereAsync<T>(this ValueTask<Maybe<T>> awaitable,
                                                                      Func<T, bool> predicate,
                                                                      string? because = null)
        where T : notnull
    {
        var opt = await awaitable.ConfigureAwait(false);
        opt.ShouldBeSomeWhere(predicate, because);
        return opt;
    }

    /// <summary>
    ///     Awaits a ValueTask returning Maybe and asserts None based on a predicate.
    /// </summary>
    /// <param name="awaitable">The ValueTask containing the Maybe to be asserted.</param>
    /// <param name="predicate">A function defining the condition that must hold true for the assertion.</param>
    /// <param name="because">An Maybeal descriptive message explaining the reason for the assertion.</param>
    /// <returns>The original Maybe contained within the ValueTask after the assertion is performed.</returns>
    public static async ValueTask<Maybe<T>> ShouldBeNoneWhereAsync<T>(this ValueTask<Maybe<T>> awaitable,
                                                                      Func<bool> predicate,
                                                                      string? because = null)
        where T : notnull
    {
        var opt = await awaitable.ConfigureAwait(false);
        opt.ShouldBeNoneWhere(predicate, because);
        return opt;
    }
}
