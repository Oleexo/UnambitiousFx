using System.Diagnostics.CodeAnalysis;

namespace UnambitiousFx.Core.Maybe;

/// <summary>
///     Provides a static class to create and work with instances of <see cref="Maybe{TValue}" />,
///     which represents a value or the absence of a value.
/// </summary>
public static class Maybe
{
    /// Creates an Option instance that represents a value.
    /// <param name="value">The value to be wrapped inside the Option. It must be a non-null value of type TValue.</param>
    /// <returns>An <see cref="Maybe{TValue}" /> instance containing the provided value.</returns>
    public static Maybe<TValue> Some<TValue>(TValue value)
        where TValue : notnull
    {
        return new SomeMaybe<TValue>(value);
    }

    /// <summary>
    ///     Creates an instance of <see cref="Maybe{TValue}" /> that represents no value.
    /// </summary>
    /// <typeparam name="TValue">The type of the service represented by the option.</typeparam>
    /// <returns>An <see cref="Maybe{TValue}" /> instance that indicates no value.</returns>
    public static Maybe<TValue> None<TValue>()
        where TValue : notnull
    {
        return Maybe<TValue>.None();
    }
}

/// <summary>
///     Provides a static class to create instances of <see cref="Maybe{TValue}" />, representing a value or no value.
/// </summary>
public abstract class Maybe<TValue>
    where TValue : notnull
{
    /// <summary>
    ///     Represents the singleton instance of <see cref="NoneMaybe{TValue}" />, used to indicate the absence of a value.
    /// </summary>
    private static readonly Maybe<TValue> NoneInstance = new NoneMaybe<TValue>();

    /// <summary>
    ///     Indicates whether the current <see cref="Maybe{TValue}" /> instance contains a value.
    ///     Returns <c>true</c> if the instance represents a value, otherwise <c>false</c>.
    /// </summary>
    public abstract bool IsSome { get; }

    /// <summary>
    ///     Gets a value indicating whether the current instance represents the absence of a value.
    /// </summary>
    public abstract bool IsNone { get; }

    /// <summary>
    ///     Gets the underlying value if the instance represents a value, or returns null if the instance represents no value.
    /// </summary>
    public abstract object? Case { get; }

    /// Executes the specified action if this instance represents no value.
    /// <param name="none">An action to execute when the option represents no value.</param>
    public abstract void IfNone(Action none);

    /// Executes the provided asynchronous function if the current Option instance represents no value.
    /// <param name="none">The asynchronous function to be executed when the Option is a None instance.</param>
    /// <returns>A <see cref="ValueTask" /> that completes when the function execution is complete.</returns>
    public abstract ValueTask IfNone(Func<ValueTask> none);

    /// Executes the provided action if the Option contains a value.
    /// <param name="some">The action to execute if the Option contains a value.</param>
    public abstract void IfSome(Action<TValue> some);

    /// Executes the provided asynchronous action if the Option instance represents a value (Some).
    /// <param name="some">
    ///     The asynchronous action to execute if the instance is Some. The action receives the value as its
    ///     parameter.
    /// </param>
    /// <returns>A <see cref="ValueTask" /> that completes when the provided action has been executed or if the Option is None.</returns>
    public abstract ValueTask IfSome(Func<TValue, ValueTask> some);

    /// Determines whether the current instance represents a value and, if so, retrieves the wrapped value.
    /// <param name="value">
    ///     When the method returns, contains the value of type <typeparamref name="TValue" /> if the instance
    ///     represents a value; otherwise, the default value for the type of the <typeparamref name="TValue" /> parameter. This
    ///     parameter is passed uninitialized.
    /// </param>
    /// <returns><see langword="true" /> if the instance represents a value; otherwise, <see langword="false" />.</returns>
    public abstract bool Some([NotNullWhen(true)] out TValue? value);

    /// Evaluates the current Option instance and applies the corresponding function based on its state.
    /// <param name="some">The function to invoke if the Option contains a value.</param>
    /// <param name="none">The function to invoke if the Option does not contain a value.</param>
    /// <returns>The result of invoking the appropriate function based on the Option's state.</returns>
    public abstract TOut Match<TOut>(Func<TValue, TOut> some,
                                     Func<TOut> none);

    /// Executes one of the provided actions depending on whether the Option has a value or represents no value.
    /// <param name="some">
    ///     The action to execute if the Option contains a value. The value is passed as a parameter to the
    ///     action.
    /// </param>
    /// <param name="none">The action to execute if the Option represents no value.</param>
    public abstract void Match(Action<TValue> some,
                               Action none);

    /// Creates an Option instance that represents no value.
    /// <returns>An <see cref="Maybe{TValue}" /> instance representing no value.</returns>
    public static Maybe<TValue> None()
    {
        return NoneInstance;
    }

    /// <summary>
    ///     Creates a new instance of <see cref="Maybe{TValue}" /> with a value wrapped in a SomeOption.
    /// </summary>
    /// <param name="value">The value to wrap within the SomeOption. This value cannot be null.</param>
    /// <returns>An <see cref="Maybe{TValue}" /> containing the specified value.</returns>
    public static Maybe<TValue> Some(TValue value)
    {
        return new SomeMaybe<TValue>(value);
    }

    /// Defines an implicit conversion from a value of type
    /// <typeparamref name="TValue" />
    /// to an
    /// <see cref="Maybe{TValue}" />
    /// .
    /// <param name="value">The value to convert into an Option. The value cannot be null.</param>
    /// <returns>An <see cref="Maybe{TValue}" /> instance containing the provided value.</returns>
    public static implicit operator Maybe<TValue>(TValue value)
    {
        return Some(value);
    }
}
