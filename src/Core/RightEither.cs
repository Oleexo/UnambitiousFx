using System.Diagnostics.CodeAnalysis;

namespace UnambitiousFx.Core;

internal sealed class RightEither<TLeft, TRight> : Either<TLeft, TRight>
    where TLeft : notnull
    where TRight : notnull {
    private readonly TRight _right;

    public RightEither(TRight right) {
        _right = right;
    }

    public override bool IsLeft  => false;
    public override bool IsRight => true;

    public override Either<TLeftOut, TRightOut> Bind<TLeftOut, TRightOut>(Func<TLeft, Either<TLeftOut, TRightOut>>  leftFunc,
                                                                          Func<TRight, Either<TLeftOut, TRightOut>> rightFunc) {
        return rightFunc(_right);
    }

    public override T Match<T>(Func<TLeft, T>  leftFunc,
                               Func<TRight, T> rightFunc) {
        return rightFunc(_right);
    }

    public override void Match(Action<TLeft>  leftAction,
                               Action<TRight> rightAction) {
        rightAction(_right);
    }

    public override bool Left([NotNullWhen(true)] out  TLeft?  left,
                              [NotNullWhen(false)] out TRight? right) {
        left  = default;
        right = _right;
        return false;
    }

    public override bool Right([NotNullWhen(false)] out TLeft?  left,
                               [NotNullWhen(true)] out  TRight? right) {
        left  = default;
        right = _right;
        return true;
    }
}
