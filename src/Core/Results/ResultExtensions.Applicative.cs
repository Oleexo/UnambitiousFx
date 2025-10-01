namespace UnambitiousFx.Core.Results;

public static partial class ResultExtensions
{
    public static Result<TOut> Apply<TIn, TOut>(this Result<Func<TIn, TOut>> rf, Result<TIn> ra)
        where TIn : notnull
        where TOut : notnull
    {
        if (!rf.Ok(out var f, out var fErr))
        {
            return Result.Failure<TOut>(fErr);
        }

        if (!ra.Ok(out var a, out var aErr))
        {
            return Result.Failure<TOut>(aErr);
        }

        return Result.Success(f(a));
    }

    public static Result<(T1, T2)> Zip<T1, T2>(this Result<T1> r1, Result<T2> r2)
        where T1 : notnull
        where T2 : notnull
    {
        if (!r1.Ok(out var v1, out var e1))
        {
            return Result.Failure<(T1, T2)>(e1);
        }
        if (!r2.Ok(out var v2, out var e2))
        {
            return Result.Failure<(T1, T2)>(e2);
        }

        return Result.Success((v1, v2));
    }

    public static Result<TR> Zip<T1, T2, TR>(this Result<T1> r1, Result<T2> r2, Func<T1, T2, TR> projector)
        where T1 : notnull
        where T2 : notnull
        where TR : notnull
    {
        if (!r1.Ok(out var v1, out var e1))
        {
            return Result.Failure<TR>(e1);
        }
        if (!r2.Ok(out var v2, out var e2))
        {
            return Result.Failure<TR>(e2);
        }

        return Result.Success(projector(v1, v2));
    }
}
