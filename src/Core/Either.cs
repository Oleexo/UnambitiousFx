using System.Diagnostics.CodeAnalysis;

namespace UnambitiousFx.Core;

/// <summary>
///     Represents a value that can hold one of two types, either <typeparamref name="TLeft" /> or
///     <typeparamref name="TRight" />.
/// </summary>
/// <typeparam name="TLeft">The type of the value if this instance represents the 'Left' state.</typeparam>
/// <typeparam name="TRight">The type of the value if this instance represents the 'Right' state.</typeparam>
public abstract class Either<TLeft, TRight> : IEither<TLeft, TRight>
    where TLeft : notnull
    where TRight : notnull {
    /// <inheritdoc />
    public abstract bool IsLeft { get; }

    /// <inheritdoc />
    public abstract bool IsRight { get; }

    /// <inheritdoc />
    public abstract Either<TLeftOut, TRightOut> Bind<TLeftOut, TRightOut>(Func<TLeft, Either<TLeftOut, TRightOut>>  leftFunc,
                                                                          Func<TRight, Either<TLeftOut, TRightOut>> rightFunc)
        where TLeftOut : notnull
        where TRightOut : notnull;

    /// <inheritdoc />
    public abstract T Match<T>(Func<TLeft, T>  leftFunc,
                               Func<TRight, T> rightFunc);

    /// <inheritdoc />
    public abstract void Match(Action<TLeft>  leftAction,
                               Action<TRight> rightAction);

    /// <inheritdoc />
    public abstract bool Left([NotNullWhen(true)] out  TLeft?  left,
                              [NotNullWhen(false)] out TRight? right);

    /// <inheritdoc />
    public abstract bool Right([NotNullWhen(false)] out TLeft?  left,
                               [NotNullWhen(true)] out  TRight? right);

    /// <summary>
    ///     Creates an instance of <see cref="Either{TLeft, TRight}" /> that wraps a value of the left type.
    /// </summary>
    /// <param name="left">The value to encapsulate as the left value.</param>
    /// <returns>An instance of <see cref="Either{TLeft, TRight}" /> representing the left value.</returns>
    public static Either<TLeft, TRight> FromLeft(TLeft left) {
        return new LeftEither<TLeft, TRight>(left);
    }

    /// <summary>
    ///     Creates an instance of <see cref="Either{TLeft, TRight}" /> representing the right value.
    /// </summary>
    /// <param name="right">The value of type <typeparamref name="TRight" /> to assign to the right side.</param>
    /// <returns>An <see cref="Either{TLeft, TRight}" /> instance with the right value set.</returns>
    public static Either<TLeft, TRight> FromRight(TRight right) {
        return new RightEither<TLeft, TRight>(right);
    }

    /// <summary>
    ///     Defines an implicit conversion operator that allows converting a value of type TLeft to an
    ///     <see cref="Either{TLeft, TRight}" />. This operator creates an Either instance representing
    ///     the left value.
    /// </summary>
    /// <param name="left">The value to convert into the left side of the Either type.</param>
    /// <returns>An Either instance containing the provided value as its left value.</returns>
    public static implicit operator Either<TLeft, TRight>(TLeft left) {
        return FromLeft(left);
    }

    /// <summary>
    ///     Defines an implicit conversion operator that allows converting a value of type TRight to an
    ///     <see cref="Either{TLeft, TRight}" />. This operator creates an Either instance representing
    ///     the right value.
    /// </summary>
    /// <param name="right">The value to convert into the right side of the Either type.</param>
    /// <returns>An Either instance containing the provided value as its right value.</returns>
    public static implicit operator Either<TLeft, TRight>(TRight right) {
        return FromRight(right);
    }
}
