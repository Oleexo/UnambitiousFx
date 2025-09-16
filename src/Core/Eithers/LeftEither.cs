using System.Diagnostics.CodeAnalysis;

namespace UnambitiousFx.Core.Eithers;

internal sealed class LeftEither<TLeft, TRight> : Either<TLeft, TRight>
    where TLeft : notnull
    where TRight : notnull {
    private readonly TLeft _left;

    public LeftEither(TLeft left) {
        _left = left;
    }

    public override bool IsLeft  => true;
    public override bool IsRight => false;

    public override Either<TLeftOut, TRightOut> Bind<TLeftOut, TRightOut>(Func<TLeft, Either<TLeftOut, TRightOut>>  leftFunc,
                                                                          Func<TRight, Either<TLeftOut, TRightOut>> rightFunc) {
        return leftFunc(_left);
    }

    public override T Match<T>(Func<TLeft, T>  leftFunc,
                               Func<TRight, T> rightFunc) {
        return leftFunc(_left);
    }

    public override void Match(Action<TLeft>  leftAction,
                               Action<TRight> rightAction) {
        leftAction(_left);
    }

    public override bool Left([NotNullWhen(true)] out  TLeft?  left,
                              [NotNullWhen(false)] out TRight? right) {
        left  = _left;
        right = default;
        return true;
    }

    public override bool Right([NotNullWhen(false)] out TLeft?  left,
                               [NotNullWhen(true)] out  TRight? right) {
        left  = _left;
        right = default;
        return false;
    }
}
