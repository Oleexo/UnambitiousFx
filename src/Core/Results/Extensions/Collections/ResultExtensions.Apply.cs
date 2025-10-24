namespace UnambitiousFx.Core.Results.Extensions.Collections;

public static partial class ResultExtensions {
    public static Result<TOut> Apply<TIn, TOut>(this Result<Func<TIn, TOut>> rf,
                                                Result<TIn>                  ra)
        where TIn : notnull
        where TOut : notnull {
        if (!rf.Ok(out var f, out var fErr)) {
            return Result.Failure<TOut>(fErr);
        }

        if (!ra.Ok(out var a, out var aErr)) {
            return Result.Failure<TOut>(aErr);
        }

        return Result.Success(f(a));
    }
}