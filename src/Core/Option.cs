using System.Diagnostics.CodeAnalysis;

namespace Oleexo.UnambitiousFx.Core;

/// <summary>
///     Provides a static class to create and work with instances of <see cref="Option{TValue}" />,
///     which represents a value or the absence of a value.
/// </summary>
public static class Option {
    /// Creates an Option instance that represents a value.
    /// <param name="value">The value to be wrapped inside the Option. It must be a non-null value of type TValue.</param>
    /// <returns>An <see cref="Option{TValue}" /> instance containing the provided value.</returns>
    public static Option<TValue> Some<TValue>(TValue value)
        where TValue : notnull {
        return new SomeOption<TValue>(value);
    }

    /// <summary>
    ///     Creates an instance of <see cref="Option{TValue}" /> that represents no value.
    /// </summary>
    /// <typeparam name="TValue">The type of the service represented by the option.</typeparam>
    /// <returns>An <see cref="Option{TValue}" /> instance that indicates no value.</returns>
    public static Option<TValue> None<TValue>()
        where TValue : class {
        return Option<TValue>.None();
    }
}

/// <summary>
///     Provides a static class to create instances of <see cref="Option{TValue}" />, representing a value or no value.
/// </summary>
public abstract class Option<TValue> : IOption<TValue>
    where TValue : notnull {
    /// <summary>
    ///     Represents the singleton instance of <see cref="NoneOption{TValue}" />, used to indicate the absence of a value.
    /// </summary>
    private static readonly Option<TValue> NoneInstance = new NoneOption<TValue>();

    /// <inheritdoc />
    public abstract bool IsSome { get; }

    /// <inheritdoc />
    public abstract bool IsNone { get; }

    /// <inheritdoc />
    public abstract object? Case { get; }

    /// <inheritdoc />
    public abstract void IfNone(Action none);

    /// <inheritdoc />
    public abstract ValueTask IfNone(Func<ValueTask> none);

    /// <inheritdoc />
    public abstract void IfSome(Action<TValue> some);

    /// <inheritdoc />
    public abstract ValueTask IfSome(Func<TValue, ValueTask> some);

    /// <inheritdoc />
    public abstract bool Some([NotNullWhen(true)] out TValue? value);

    /// <inheritdoc />
    public abstract TOut Match<TOut>(Func<TValue, TOut> some,
                                     Func<TOut>         none);

    /// <inheritdoc />
    public abstract void Match(Action<TValue> some,
                               Action         none);

    /// Creates an Option instance that represents no value.
    /// <returns>An <see cref="Option{TValue}" /> instance representing no value.</returns>
    public static Option<TValue> None() {
        return NoneInstance;
    }

    /// <summary>
    ///     Creates a new instance of <see cref="Option{TValue}" /> with a value wrapped in a SomeOption.
    /// </summary>
    /// <param name="value">The value to wrap within the SomeOption. This value cannot be null.</param>
    /// <returns>An <see cref="Option{TValue}" /> containing the specified value.</returns>
    public static Option<TValue> Some(TValue value) {
        return new SomeOption<TValue>(value);
    }
}
