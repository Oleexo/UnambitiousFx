namespace UnambitiousFx.Core.Results.Extensions.Transformations;

public static class ResultZipExtensions {
    public static Result<T1, T2> Zip<T1, T2>(this Result<T1> r1,
                                             Result<T2>      r2)
        where T1 : notnull
        where T2 : notnull {
        if (!r1.Ok(out var v1, out var e1)) {
            return Result.Failure<T1, T2>(e1);
        }

        if (!r2.Ok(out var v2, out var e2)) {
            return Result.Failure<T1, T2>(e2);
        }

        return Result.Success(v1, v2);
    }

    public static Result<T1, T2, T3> Zip<T1, T2, T3>(this Result<T1> r1,
                                                     Result<T2>      r2,
                                                     Result<T3>      r3)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull {
        if (!r1.Ok(out var v1, out var e1)) {
            return Result.Failure<T1, T2, T3>(e1);
        }

        if (!r2.Ok(out var v2, out var e2)) {
            return Result.Failure<T1, T2, T3>(e2);
        }

        if (!r3.Ok(out var v3, out var e3)) {
            return Result.Failure<T1, T2, T3>(e3);
        }

        return Result.Success(v1, v2, v3);
    }

    public static Result<T1, T2, T3, T4> Zip<T1, T2, T3, T4>(this Result<T1> r1,
                                                             Result<T2>      r2,
                                                             Result<T3>      r3,
                                                             Result<T4>      r4)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull {
        if (!r1.Ok(out var v1, out var e1)) {
            return Result.Failure<T1, T2, T3, T4>(e1);
        }

        if (!r2.Ok(out var v2, out var e2)) {
            return Result.Failure<T1, T2, T3, T4>(e2);
        }

        if (!r3.Ok(out var v3, out var e3)) {
            return Result.Failure<T1, T2, T3, T4>(e3);
        }

        if (!r4.Ok(out var v4, out var e4)) {
            return Result.Failure<T1, T2, T3, T4>(e4);
        }

        return Result.Success(v1, v2, v3, v4);
    }

    public static Result<T1, T2, T3, T4, T5> Zip<T1, T2, T3, T4, T5>(this Result<T1> r1,
                                                                     Result<T2>      r2,
                                                                     Result<T3>      r3,
                                                                     Result<T4>      r4,
                                                                     Result<T5>      r5)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull {
        if (!r1.Ok(out var v1, out var e1)) {
            return Result.Failure<T1, T2, T3, T4, T5>(e1);
        }

        if (!r2.Ok(out var v2, out var e2)) {
            return Result.Failure<T1, T2, T3, T4, T5>(e2);
        }

        if (!r3.Ok(out var v3, out var e3)) {
            return Result.Failure<T1, T2, T3, T4, T5>(e3);
        }

        if (!r4.Ok(out var v4, out var e4)) {
            return Result.Failure<T1, T2, T3, T4, T5>(e4);
        }

        if (!r5.Ok(out var v5, out var e5)) {
            return Result.Failure<T1, T2, T3, T4, T5>(e5);
        }

        return Result.Success(v1, v2, v3, v4, v5);
    }

    public static Result<T1, T2, T3, T4, T5, T6> Zip<T1, T2, T3, T4, T5, T6>(this Result<T1> r1,
                                                                             Result<T2>      r2,
                                                                             Result<T3>      r3,
                                                                             Result<T4>      r4,
                                                                             Result<T5>      r5,
                                                                             Result<T6>      r6)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull {
        if (!r1.Ok(out var v1, out var e1)) {
            return Result.Failure<T1, T2, T3, T4, T5, T6>(e1);
        }

        if (!r2.Ok(out var v2, out var e2)) {
            return Result.Failure<T1, T2, T3, T4, T5, T6>(e2);
        }

        if (!r3.Ok(out var v3, out var e3)) {
            return Result.Failure<T1, T2, T3, T4, T5, T6>(e3);
        }

        if (!r4.Ok(out var v4, out var e4)) {
            return Result.Failure<T1, T2, T3, T4, T5, T6>(e4);
        }

        if (!r5.Ok(out var v5, out var e5)) {
            return Result.Failure<T1, T2, T3, T4, T5, T6>(e5);
        }

        if (!r6.Ok(out var v6, out var e6)) {
            return Result.Failure<T1, T2, T3, T4, T5, T6>(e6);
        }

        return Result.Success(v1, v2, v3, v4, v5, v6);
    }

    public static Result<T1, T2, T3, T4, T5, T6, T7> Zip<T1, T2, T3, T4, T5, T6, T7>(this Result<T1> r1,
                                                                                     Result<T2>      r2,
                                                                                     Result<T3>      r3,
                                                                                     Result<T4>      r4,
                                                                                     Result<T5>      r5,
                                                                                     Result<T6>      r6,
                                                                                     Result<T7>      r7)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
        where T7 : notnull {
        if (!r1.Ok(out var v1, out var e1)) {
            return Result.Failure<T1, T2, T3, T4, T5, T6, T7>(e1);
        }

        if (!r2.Ok(out var v2, out var e2)) {
            return Result.Failure<T1, T2, T3, T4, T5, T6, T7>(e2);
        }

        if (!r3.Ok(out var v3, out var e3)) {
            return Result.Failure<T1, T2, T3, T4, T5, T6, T7>(e3);
        }

        if (!r4.Ok(out var v4, out var e4)) {
            return Result.Failure<T1, T2, T3, T4, T5, T6, T7>(e4);
        }

        if (!r5.Ok(out var v5, out var e5)) {
            return Result.Failure<T1, T2, T3, T4, T5, T6, T7>(e5);
        }

        if (!r6.Ok(out var v6, out var e6)) {
            return Result.Failure<T1, T2, T3, T4, T5, T6, T7>(e6);
        }

        if (!r7.Ok(out var v7, out var e7)) {
            return Result.Failure<T1, T2, T3, T4, T5, T6, T7>(e7);
        }

        return Result.Success(v1, v2, v3, v4, v5, v6, v7);
    }

    public static Result<T1, T2, T3, T4, T5, T6, T7, T8> Zip<T1, T2, T3, T4, T5, T6, T7, T8>(this Result<T1> r1,
                                                                                             Result<T2>      r2,
                                                                                             Result<T3>      r3,
                                                                                             Result<T4>      r4,
                                                                                             Result<T5>      r5,
                                                                                             Result<T6>      r6,
                                                                                             Result<T7>      r7,
                                                                                             Result<T8>      r8)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
        where T7 : notnull
        where T8 : notnull {
        if (!r1.Ok(out var v1, out var e1)) {
            return Result.Failure<T1, T2, T3, T4, T5, T6, T7, T8>(e1);
        }

        if (!r2.Ok(out var v2, out var e2)) {
            return Result.Failure<T1, T2, T3, T4, T5, T6, T7, T8>(e2);
        }

        if (!r3.Ok(out var v3, out var e3)) {
            return Result.Failure<T1, T2, T3, T4, T5, T6, T7, T8>(e3);
        }

        if (!r4.Ok(out var v4, out var e4)) {
            return Result.Failure<T1, T2, T3, T4, T5, T6, T7, T8>(e4);
        }

        if (!r5.Ok(out var v5, out var e5)) {
            return Result.Failure<T1, T2, T3, T4, T5, T6, T7, T8>(e5);
        }

        if (!r6.Ok(out var v6, out var e6)) {
            return Result.Failure<T1, T2, T3, T4, T5, T6, T7, T8>(e6);
        }

        if (!r7.Ok(out var v7, out var e7)) {
            return Result.Failure<T1, T2, T3, T4, T5, T6, T7, T8>(e7);
        }

        if (!r8.Ok(out var v8, out var e8)) {
            return Result.Failure<T1, T2, T3, T4, T5, T6, T7, T8>(e8);
        }

        return Result.Success(v1, v2, v3, v4, v5, v6, v7, v8);
    }

    public static Result<TR> Zip<T1, T2, TR>(this Result<T1>  r1,
                                             Result<T2>       r2,
                                             Func<T1, T2, TR> projector)
        where T1 : notnull
        where T2 : notnull
        where TR : notnull {
        if (!r1.Ok(out var v1, out var e1)) {
            return Result.Failure<TR>(e1);
        }

        if (!r2.Ok(out var v2, out var e2)) {
            return Result.Failure<TR>(e2);
        }

        return Result.Success(projector(v1, v2));
    }

    public static Result<TR> Zip<T1, T2, T3, TR>(this Result<T1>      r1,
                                                 Result<T2>           r2,
                                                 Result<T3>           r3,
                                                 Func<T1, T2, T3, TR> projector)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where TR : notnull {
        if (!r1.Ok(out var v1, out var e1)) {
            return Result.Failure<TR>(e1);
        }

        if (!r2.Ok(out var v2, out var e2)) {
            return Result.Failure<TR>(e2);
        }

        if (!r3.Ok(out var v3, out var e3)) {
            return Result.Failure<TR>(e3);
        }

        return Result.Success(projector(v1, v2, v3));
    }

    public static Result<TR> Zip<T1, T2, T3, T4, TR>(this Result<T1>          r1,
                                                     Result<T2>               r2,
                                                     Result<T3>               r3,
                                                     Result<T4>               r4,
                                                     Func<T1, T2, T3, T4, TR> projector)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where TR : notnull {
        if (!r1.Ok(out var v1, out var e1)) {
            return Result.Failure<TR>(e1);
        }

        if (!r2.Ok(out var v2, out var e2)) {
            return Result.Failure<TR>(e2);
        }

        if (!r3.Ok(out var v3, out var e3)) {
            return Result.Failure<TR>(e3);
        }

        if (!r4.Ok(out var v4, out var e4)) {
            return Result.Failure<TR>(e4);
        }

        return Result.Success(projector(v1, v2, v3, v4));
    }

    public static Result<TR> Zip<T1, T2, T3, T4, T5, TR>(this Result<T1>              r1,
                                                         Result<T2>                   r2,
                                                         Result<T3>                   r3,
                                                         Result<T4>                   r4,
                                                         Result<T5>                   r5,
                                                         Func<T1, T2, T3, T4, T5, TR> projector)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where TR : notnull {
        if (!r1.Ok(out var v1, out var e1)) {
            return Result.Failure<TR>(e1);
        }

        if (!r2.Ok(out var v2, out var e2)) {
            return Result.Failure<TR>(e2);
        }

        if (!r3.Ok(out var v3, out var e3)) {
            return Result.Failure<TR>(e3);
        }

        if (!r4.Ok(out var v4, out var e4)) {
            return Result.Failure<TR>(e4);
        }

        if (!r5.Ok(out var v5, out var e5)) {
            return Result.Failure<TR>(e5);
        }

        return Result.Success(projector(v1, v2, v3, v4, v5));
    }

    public static Result<TR> Zip<T1, T2, T3, T4, T5, T6, TR>(this Result<T1>                  r1,
                                                             Result<T2>                       r2,
                                                             Result<T3>                       r3,
                                                             Result<T4>                       r4,
                                                             Result<T5>                       r5,
                                                             Result<T6>                       r6,
                                                             Func<T1, T2, T3, T4, T5, T6, TR> projector)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
        where TR : notnull {
        if (!r1.Ok(out var v1, out var e1)) {
            return Result.Failure<TR>(e1);
        }

        if (!r2.Ok(out var v2, out var e2)) {
            return Result.Failure<TR>(e2);
        }

        if (!r3.Ok(out var v3, out var e3)) {
            return Result.Failure<TR>(e3);
        }

        if (!r4.Ok(out var v4, out var e4)) {
            return Result.Failure<TR>(e4);
        }

        if (!r5.Ok(out var v5, out var e5)) {
            return Result.Failure<TR>(e5);
        }

        if (!r6.Ok(out var v6, out var e6)) {
            return Result.Failure<TR>(e6);
        }

        return Result.Success(projector(v1, v2, v3, v4, v5, v6));
    }

    public static Result<TR> Zip<T1, T2, T3, T4, T5, T6, T7, TR>(this Result<T1>                      r1,
                                                                 Result<T2>                           r2,
                                                                 Result<T3>                           r3,
                                                                 Result<T4>                           r4,
                                                                 Result<T5>                           r5,
                                                                 Result<T6>                           r6,
                                                                 Result<T7>                           r7,
                                                                 Func<T1, T2, T3, T4, T5, T6, T7, TR> projector)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
        where T7 : notnull
        where TR : notnull {
        if (!r1.Ok(out var v1, out var e1)) {
            return Result.Failure<TR>(e1);
        }

        if (!r2.Ok(out var v2, out var e2)) {
            return Result.Failure<TR>(e2);
        }

        if (!r3.Ok(out var v3, out var e3)) {
            return Result.Failure<TR>(e3);
        }

        if (!r4.Ok(out var v4, out var e4)) {
            return Result.Failure<TR>(e4);
        }

        if (!r5.Ok(out var v5, out var e5)) {
            return Result.Failure<TR>(e5);
        }

        if (!r6.Ok(out var v6, out var e6)) {
            return Result.Failure<TR>(e6);
        }

        if (!r7.Ok(out var v7, out var e7)) {
            return Result.Failure<TR>(e7);
        }

        return Result.Success(projector(v1, v2, v3, v4, v5, v6, v7));
    }

    public static Result<TR> Zip<T1, T2, T3, T4, T5, T6, T7, T8, TR>(this Result<T1>                          r1,
                                                                     Result<T2>                               r2,
                                                                     Result<T3>                               r3,
                                                                     Result<T4>                               r4,
                                                                     Result<T5>                               r5,
                                                                     Result<T6>                               r6,
                                                                     Result<T7>                               r7,
                                                                     Result<T8>                               r8,
                                                                     Func<T1, T2, T3, T4, T5, T6, T7, T8, TR> projector)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
        where T7 : notnull
        where T8 : notnull
        where TR : notnull {
        if (!r1.Ok(out var v1, out var e1)) {
            return Result.Failure<TR>(e1);
        }

        if (!r2.Ok(out var v2, out var e2)) {
            return Result.Failure<TR>(e2);
        }

        if (!r3.Ok(out var v3, out var e3)) {
            return Result.Failure<TR>(e3);
        }

        if (!r4.Ok(out var v4, out var e4)) {
            return Result.Failure<TR>(e4);
        }

        if (!r5.Ok(out var v5, out var e5)) {
            return Result.Failure<TR>(e5);
        }

        if (!r6.Ok(out var v6, out var e6)) {
            return Result.Failure<TR>(e6);
        }

        if (!r7.Ok(out var v7, out var e7)) {
            return Result.Failure<TR>(e7);
        }

        if (!r8.Ok(out var v8, out var e8)) {
            return Result.Failure<TR>(e8);
        }

        return Result.Success(projector(v1, v2, v3, v4, v5, v6, v7, v8));
    }
}
