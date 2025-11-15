using System.Diagnostics;

namespace UnambitiousFx.Core.XUnit.Fluent;

/// <summary>
///     Provides a fluent interface for asserting and manipulating the Right value of an Either type.
/// </summary>
[DebuggerStepThrough]
public readonly struct RightAssertion<TLeft, TRight>
    where TLeft : notnull
    where TRight : notnull
{
    /// <summary>
    ///     The internal field representing the Right value of the <see cref="RightAssertion{TLeft, TRight}" /> type.
    /// </summary>
    /// <remarks>
    ///     This field holds the value of a successful Right assertion in an Either construct. It is used
    ///     internally to store and manipulate the Right value within the fluent API for assertions.
    /// </remarks>
    private readonly TRight _right;

    /// <summary>
    ///     Represents a fluent wrapper for an Either value's "Right" side, providing
    ///     methods for validation, transformation, and extraction of the "Right" value.
    /// </summary>
    internal RightAssertion(TRight right)
    {
        _right = right;
    }

    /// <summary>
    ///     Gets the "Right" value of the current assertion.
    /// </summary>
    /// <remarks>
    ///     This property provides access to the encapsulated "Right" value in the assertion,
    ///     representing a successful outcome or value in a contextual operation. It allows for
    ///     further examination or manipulation of the "Right" value within a computation or test assertion.
    /// </remarks>
    /// <value>
    ///     The "Right" value of type <typeparamref name="TRight" />.
    /// </value>
    public TRight Value => _right;

    /// <summary>
    ///     Invokes the specified assertion action on the Right value of the current RightAssertion instance,
    ///     allowing for further fluent chaining.
    /// </summary>
    /// <param name="assert">
    ///     The action to invoke on the Right value. This action is intended to perform assertions
    ///     or validations on the provided value.
    /// </param>
    /// <returns>
    ///     The current instance of <see cref="RightAssertion{TLeft, TRight}" /> for further fluent method chaining.
    /// </returns>
    public RightAssertion<TLeft, TRight> And(Action<TRight> assert)
    {
        assert(_right);
        return this;
    }

    /// <summary>
    ///     Projects the inner value of a <see cref="RightAssertion{TLeft, TRight}" /> into a new value type using the
    ///     specified projector function.
    /// </summary>
    /// <typeparam name="TOut">The type of the resulting projected value.</typeparam>
    /// <param name="projector">
    ///     The function to transform the value of <typeparamref name="TRight" /> into a value of
    ///     <typeparamref name="TOut" />.
    /// </param>
    /// <returns>
    ///     A new <see cref="RightAssertion{TLeft, TOut}" /> containing the projected value of type
    ///     <typeparamref name="TOut" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="projector" /> function is null.</exception>
    public RightAssertion<TLeft, TOut> Map<TOut>(Func<TRight, TOut> projector)
        where TOut : notnull
    {
        return new RightAssertion<TLeft, TOut>(projector(_right));
    }

    /// <summary>
    ///     Deconstructs the RightAssertion instance to its contained value.
    /// </summary>
    /// <param name="right">The output parameter representing the contained right value.</param>
    public void Deconstruct(out TRight right)
    {
        right = _right;
    }
}
