using System.Diagnostics.CodeAnalysis;

namespace UnambitiousFx.Core;

/// <summary>
///     Represents an optional value which can either be in a "Some" state, containing a value,
///     or in a "None" state, indicating the absence of a value.
/// </summary>
public interface IOption {
    /// Gets a value indicating whether the current instance represents a value.
    /// When this property is true, it means the option contains a value; otherwise, the option is empty.
    bool IsSome { get; }

    /// <summary>
    ///     Gets a value indicating whether the current option instance represents a "None" state,
    ///     meaning it does not contain a value.
    /// </summary>
    /// <remarks>
    ///     This property is the logical opposite of <c>IsSome</c>. If <c>IsNone</c> is <c>true</c>,
    ///     then no value is present within the option.
    /// </remarks>
    bool IsNone { get; }

    /// <summary>
    ///     Represents the content of the option's state.
    ///     When the option is in the "Some" state, this property will return the contained value.
    ///     When the option is in the "None" state, this property will return null.
    /// </summary>
    object? Case { get; }

    /// Executes the provided action if the instance represents a "None" value.
    /// <param name="none">The action to execute when the instance is "None".</param>
    void IfNone(Action none);

    /// Executes the provided function if the current option has no value (is none).
    /// <param name="none">The function to execute when the option is "none".</param>
    /// <returns>A <see cref="ValueTask" /> representing the asynchronous operation.</returns>
    ValueTask IfNone(Func<ValueTask> none);
}

/// Represents a container that may or may not contain a value.
/// Provides methods to operate on the contained value or handle its absence.
public interface IOption<TValue> : IOption {
    /// Executes the specified action if the current instance represents a "Some" value.
    /// If the instance is a "None" value, the action will not be executed.
    /// <param name="some">
    ///     An action to execute for the "Some" value. The action receives the value as a parameter.
    /// </param>
    void IfSome(Action<TValue> some);

    /// Executes the specified asynchronous action if the current option contains a value.
    /// <param name="some">
    ///     A function to execute if the current option contains a value. The function receives the value as its parameter.
    /// </param>
    /// <returns>
    ///     A <see cref="ValueTask" /> representing the asynchronous operation.
    /// </returns>
    ValueTask IfSome(Func<TValue, ValueTask> some);

    /// <summary>
    ///     Creates an instance of <see cref="Option{TValue}" /> with a value wrapped in a "Some" result.
    /// </summary>
    /// <param name="value">The value to wrap in a "Some" result.</param>
    /// <returns>An instance of <see cref="Option{TValue}" /> representing the provided value.</returns>
    bool Some([NotNullWhen(true)] out TValue? value);

    /// Matches the current state of the option and executes the corresponding function.
    /// <typeparam name="TOut">The type of the result returned by the invoked function.</typeparam>
    /// <param name="some">Function that is executed if the option has a value (IsSome).</param>
    /// <param name="none">Function that is executed if the option does not have a value (IsNone).</param>
    /// <returns>The result of the invoked function based on the match.</returns>
    TOut Match<TOut>(Func<TValue, TOut> some,
                     Func<TOut>         none);

    /// Matches an `Option` instance by performing an action based on its state.
    /// If the `Option` contains a value, the `some` action is invoked with that value.
    /// Otherwise, the `none` action is invoked to handle the absence of a value.
    /// <param name="some">The action to execute when the `Option` contains a value.</param>
    /// <param name="none">The action to execute when the `Option` does not contain a value.</param>
    void Match(Action<TValue> some,
               Action         none);
}
