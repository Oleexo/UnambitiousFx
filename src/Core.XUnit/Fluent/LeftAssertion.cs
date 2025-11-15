using System.Diagnostics;

namespace UnambitiousFx.Core.XUnit.Fluent;

/// <summary>
///     Represents a fluent assertion wrapper for a Left value of an Either type.
/// </summary>
/// <typeparam name="TLeft">The type of the Left value. Must be non-nullable.</typeparam>
/// <typeparam name="TRight">The type of the Right value. Must be non-nullable.</typeparam>
[DebuggerStepThrough]
public readonly struct LeftAssertion<TLeft, TRight>
    where TLeft : notnull
    where TRight : notnull
{
    /// <summary>
    ///     Represents the left value of an Either type in the context of assertions.
    /// </summary>
    /// <remarks>
    ///     This field holds the left value of the Either type, which is used in various
    ///     operations and assertions within the <see cref="LeftAssertion{TLeft, TRight}" /> struct.
    /// </remarks>
    private readonly TLeft _left;

    /// <summary>
    ///     Provides a fluent interface for assertions and transformations on a Left value
    ///     of an Either.
    /// </summary>
    internal LeftAssertion(TLeft left)
    {
        _left = left;
    }

    /// <summary>
    ///     Gets the left value being asserted in the context of <see cref="LeftAssertion{TLeft,TRight}" />.
    /// </summary>
    /// <remarks>
    ///     This property provides access to the encapsulated left value of the wrapped Either type,
    ///     allowing it to be inspected or used in further operations while maintaining immutability.
    /// </remarks>
    public TLeft Value => _left;

    /// <summary>
    ///     Performs an assertion on the left value of the current <see cref="LeftAssertion{TLeft,TRight}" /> instance
    ///     and returns the same instance to allow chaining of further assertions.
    /// </summary>
    /// <param name="assert">An <see cref="Action{TLeft}" /> that specifies the assertion to be performed on the left value.</param>
    /// <returns>The current <see cref="LeftAssertion{TLeft,TRight}" /> instance to allow method chaining.</returns>
    public LeftAssertion<TLeft, TRight> And(Action<TLeft> assert)
    {
        assert(_left);
        return this;
    }

    /// <summary>
    ///     Maps the current LeftAssertion instance to a new LeftAssertion with the projected Left value.
    /// </summary>
    /// <typeparam name="TOut">The type of the newly projected Left value.</typeparam>
    /// <param name="projector">The function to project the current Left value to a new type.</param>
    /// <returns>A new instance of <see cref="LeftAssertion{TOut,TRight}" /> with the projected Left value.</returns>
    public LeftAssertion<TOut, TRight> Map<TOut>(Func<TLeft, TOut> projector)
        where TOut : notnull
    {
        return new LeftAssertion<TOut, TRight>(projector(_left));
    }

    /// <summary>
    ///     Deconstructs the current instance into its <typeparamref name="TLeft" /> value.
    /// </summary>
    /// <param name="left">The deconstructed <typeparamref name="TLeft" /> value.</param>
    public void Deconstruct(out TLeft left)
    {
        left = _left;
    }
}
