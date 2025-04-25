using System.Diagnostics.CodeAnalysis;

namespace UnambitiousFx.Core;

/// <summary>
///     Represents a disjoint union, encapsulating a value of one of two possible types.
/// </summary>
/// <typeparam name="TLeft">The type of the value if this instance represents the 'Left' state.</typeparam>
/// <typeparam name="TRight">The type of the value if this instance represents the 'Right' state.</typeparam>
public interface IEither<TLeft, TRight>
    where TLeft : notnull
    where TRight : notnull {
    /// <summary>
    ///     Indicates whether the current instance of <c>Either</c> represents a value of the left type.
    /// </summary>
    /// <remarks>
    ///     Returns <c>true</c> if the instance holds a value of the left type; otherwise, returns <c>false</c>.
    /// </remarks>
    bool IsLeft { get; }

    /// Indicates whether the current instance represents a "right" value in the Either type.
    /// Returns true if the instance is of type RightEither, indicating a successful or valid state.
    /// Returns false if the instance is of type LeftEither, indicating an alternate or error state.
    bool IsRight { get; }

    /// <summary>
    ///     Transforms the current instance of <see cref="Either{TLeft, TRight}" /> into a new instance of
    ///     <see cref="Either{TLeftOut, TRightOut}" />
    ///     by applying the respective transformation functions based on whether the current instance is a Left or Right.
    /// </summary>
    /// <typeparam name="TLeftOut">The type of the Left value in the resulting <see cref="Either{TLeftOut, TRightOut}" />.</typeparam>
    /// <typeparam name="TRightOut">The type of the Right value in the resulting <see cref="Either{TLeftOut, TRightOut}" />.</typeparam>
    /// <param name="leftFunc">
    ///     A function to apply to the Left value if the current instance is a Left.
    ///     The function must return a new instance of <see cref="Either{TLeftOut, TRightOut}" />.
    /// </param>
    /// <param name="rightFunc">
    ///     A function to apply to the Right value if the current instance is a Right.
    ///     The function must return a new instance of <see cref="Either{TLeftOut, TRightOut}" />.
    /// </param>
    /// <returns>
    ///     A new instance of <see cref="Either{TLeftOut, TRightOut}" /> resulting from applying
    ///     the appropriate function (either <paramref name="leftFunc" /> or <paramref name="rightFunc" />)
    ///     to the current value.
    /// </returns>
    Either<TLeftOut, TRightOut> Bind<TLeftOut, TRightOut>(Func<TLeft, Either<TLeftOut, TRightOut>>  leftFunc,
                                                          Func<TRight, Either<TLeftOut, TRightOut>> rightFunc)
        where TLeftOut : notnull
        where TRightOut : notnull;

    /// <summary>
    ///     Evaluates the Either instance and returns a value of the specified type based on whether it contains
    ///     a value of the left type or the right type.
    /// </summary>
    /// <typeparam name="T">The type of the value to return.</typeparam>
    /// <param name="leftFunc">A function to execute if the instance contains a value of the left type.</param>
    /// <param name="rightFunc">A function to execute if the instance contains a value of the right type.</param>
    /// <returns>
    ///     The result of invoking <paramref name="leftFunc" /> if the instance contains a left value,
    ///     or the result of invoking <paramref name="rightFunc" /> if it contains a right value.
    /// </returns>
    T Match<T>(Func<TLeft, T>  leftFunc,
               Func<TRight, T> rightFunc);

    /// <summary>
    ///     Executes the appropriate action based on whether the instance represents the left or right value.
    /// </summary>
    /// <param name="leftAction">The action to execute if the instance represents the left value.</param>
    /// <param name="rightAction">The action to execute if the instance represents the right value.</param>
    void Match(Action<TLeft>  leftAction,
               Action<TRight> rightAction);

    /// <summary>
    ///     Attempts to extract the 'Left' value if this instance represents the 'Left' state.
    /// </summary>
    /// <param name="left">
    ///     When this method returns, contains the 'Left' value if the instance represents the 'Left' state; otherwise, the
    ///     default value for the type of the parameter.
    /// </param>
    /// <param name="right">
    ///     When this method returns, contains null if the instance represents the 'Left' state; otherwise, contains the
    ///     'Right' value.
    /// </param>
    /// <returns>
    ///     <see langword="true" /> if this instance represents the 'Left' state; otherwise, <see langword="false" />.
    /// </returns>
    bool Left([NotNullWhen(true)] out  TLeft?  left,
              [NotNullWhen(false)] out TRight? right);

    /// <summary>
    ///     Extracts the 'Right' value from the instance, if it represents the 'Right' state.
    /// </summary>
    /// <param name="left">Contains the 'Left' value when the method returns false; otherwise, null.</param>
    /// <param name="right">Contains the 'Right' value when the method returns true; otherwise, null.</param>
    /// <returns>True if the instance represents the 'Right' state; otherwise, false.</returns>
    bool Right([NotNullWhen(false)] out TLeft?  left,
               [NotNullWhen(true)] out  TRight? right);
}
