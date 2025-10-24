namespace UnambitiousFx.Core.Results.Extensions.ErrorHandling;

public static partial class ResultExtensions {
    public static Result<TValue> Recover<TValue>(this Result<TValue>     result,
                                                 Func<Exception, TValue> recover)
        where TValue : notnull {
        if (result.Ok(out _, out var error)) {
            return result;
        }

        var fallback = recover(error);
        return Result.Success(fallback);
    }

    public static Result<TValue> Recover<TValue>(this Result<TValue> result,
                                                 TValue              fallback)
        where TValue : notnull {
        return result.Recover(_ => fallback);
    }

    public static Result<TValue1, TValue2> Recover<TValue1, TValue2>(this Result<TValue1, TValue2>       result,
                                                                     Func<Exception, (TValue1, TValue2)> recover)
        where TValue1 : notnull
        where TValue2 : notnull {
        if (result.Ok(out _, out _, out var error)) {
            return result;
        }

        var fallback = recover(error);
        return Result.Success(fallback.Item1, fallback.Item2);
    }

    public static Result<TValue1, TValue2> Recover<TValue1, TValue2>(this Result<TValue1, TValue2> result,
                                                                     TValue1                       fallback1,
                                                                     TValue2                       fallback2)
        where TValue1 : notnull
        where TValue2 : notnull {
        return result.Recover(_ => (fallback1, fallback2));
    }

    public static Result<TValue1, TValue2, TValue3> Recover<TValue1, TValue2, TValue3>(this Result<TValue1, TValue2, TValue3>       result,
                                                                                       Func<Exception, (TValue1, TValue2, TValue3)> recover)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        if (result.Ok(out _, out _, out _, out var error)) {
            return result;
        }

        var fallback = recover(error);
        return Result.Success(fallback.Item1, fallback.Item2, fallback.Item3);
    }

    public static Result<TValue1, TValue2, TValue3> Recover<TValue1, TValue2, TValue3>(this Result<TValue1, TValue2, TValue3> result,
                                                                                       TValue1                                fallback1,
                                                                                       TValue2                                fallback2,
                                                                                       TValue3                                fallback3)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        return result.Recover(_ => (fallback1, fallback2, fallback3));
    }

    public static Result<TValue1, TValue2, TValue3, TValue4> Recover<TValue1, TValue2, TValue3, TValue4>(this Result<TValue1, TValue2, TValue3, TValue4>       result,
                                                                                                         Func<Exception, (TValue1, TValue2, TValue3, TValue4)> recover)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        if (result.Ok(out _, out _, out _, out _, out var error)) {
            return result;
        }

        var fallback = recover(error);
        return Result.Success(fallback.Item1, fallback.Item2, fallback.Item3, fallback.Item4);
    }

    public static Result<TValue1, TValue2, TValue3, TValue4> Recover<TValue1, TValue2, TValue3, TValue4>(this Result<TValue1, TValue2, TValue3, TValue4> result,
                                                                                                         TValue1                                         fallback1,
                                                                                                         TValue2                                         fallback2,
                                                                                                         TValue3                                         fallback3,
                                                                                                         TValue4                                         fallback4)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        return result.Recover(_ => (fallback1, fallback2, fallback3, fallback4));
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5> Recover<TValue1, TValue2, TValue3, TValue4, TValue5>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5>       result,
        Func<Exception, (TValue1, TValue2, TValue3, TValue4, TValue5)> recover)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        if (result.Ok(out _, out _, out _, out _, out _, out var error)) {
            return result;
        }

        var fallback = recover(error);
        return Result.Success(fallback.Item1, fallback.Item2, fallback.Item3, fallback.Item4, fallback.Item5);
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5> Recover<TValue1, TValue2, TValue3, TValue4, TValue5>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5> result,
        TValue1                                                  fallback1,
        TValue2                                                  fallback2,
        TValue3                                                  fallback3,
        TValue4                                                  fallback4,
        TValue5                                                  fallback5)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        return result.Recover(_ => (fallback1, fallback2, fallback3, fallback4, fallback5));
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> Recover<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>       result,
        Func<Exception, (TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)> recover)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull {
        if (result.Ok(out _, out _, out _, out _, out _, out _, out var error)) {
            return result;
        }

        var fallback = recover(error);
        return Result.Success(fallback.Item1, fallback.Item2, fallback.Item3, fallback.Item4, fallback.Item5, fallback.Item6);
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> Recover<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> result,
        TValue1                                                           fallback1,
        TValue2                                                           fallback2,
        TValue3                                                           fallback3,
        TValue4                                                           fallback4,
        TValue5                                                           fallback5,
        TValue6                                                           fallback6)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull {
        return result.Recover(_ => (fallback1, fallback2, fallback3, fallback4, fallback5, fallback6));
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> Recover<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>       result,
        Func<Exception, (TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)> recover)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull {
        if (result.Ok(out _, out _, out _, out _, out _, out _, out _, out var error)) {
            return result;
        }

        var fallback = recover(error);
        return Result.Success(fallback.Item1, fallback.Item2, fallback.Item3, fallback.Item4, fallback.Item5, fallback.Item6, fallback.Item7);
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> Recover<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> result,
        TValue1                                                                    fallback1,
        TValue2                                                                    fallback2,
        TValue3                                                                    fallback3,
        TValue4                                                                    fallback4,
        TValue5                                                                    fallback5,
        TValue6                                                                    fallback6,
        TValue7                                                                    fallback7)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull {
        return result.Recover(_ => (fallback1, fallback2, fallback3, fallback4, fallback5, fallback6, fallback7));
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> Recover<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>       result,
        Func<Exception, (TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)> recover)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull {
        if (result.Ok(out _, out _, out _, out _, out _, out _, out _, out _, out var error)) {
            return result;
        }

        var fallback = recover(error);
        return Result.Success(fallback.Item1, fallback.Item2, fallback.Item3, fallback.Item4, fallback.Item5, fallback.Item6, fallback.Item7, fallback.Item8);
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> Recover<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> result,
        TValue1                                                                             fallback1,
        TValue2                                                                             fallback2,
        TValue3                                                                             fallback3,
        TValue4                                                                             fallback4,
        TValue5                                                                             fallback5,
        TValue6                                                                             fallback6,
        TValue7                                                                             fallback7,
        TValue8                                                                             fallback8)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull {
        return result.Recover(_ => (fallback1, fallback2, fallback3, fallback4, fallback5, fallback6, fallback7, fallback8));
    }
}
