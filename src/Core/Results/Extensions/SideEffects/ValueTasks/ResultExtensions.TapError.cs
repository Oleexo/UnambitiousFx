namespace UnambitiousFx.Core.Results.Extensions.SideEffects.ValueTasks;

public static partial class ResultExtensions {
    public static async ValueTask<Result<TValue>> TapErrorAsync<TValue>(this Result<TValue>        result,
                                                                        Func<Exception, ValueTask> tap)
        where TValue : notnull {
        if (!result.Ok(out Exception? err)) {
            await tap(err);
        }

        return result;
    }

    public static async ValueTask<Result<TValue>> TapErrorAsync<TValue>(this ValueTask<Result<TValue>> awaitableResult,
                                                                        Func<Exception, ValueTask>     tap)
        where TValue : notnull {
        var result = await awaitableResult;
        return await result.TapErrorAsync(tap);
    }

    public static async ValueTask<Result<TValue1, TValue2>> TapErrorAsync<TValue1, TValue2>(this Result<TValue1, TValue2> result,
                                                                                            Func<Exception, ValueTask>    tap)
        where TValue1 : notnull
        where TValue2 : notnull {
        if (!result.Ok(out Exception? err)) {
            await tap(err);
        }

        return result;
    }

    public static async ValueTask<Result<TValue1, TValue2>> TapErrorAsync<TValue1, TValue2>(this ValueTask<Result<TValue1, TValue2>> awaitableResult,
                                                                                            Func<Exception, ValueTask>               tap)
        where TValue1 : notnull
        where TValue2 : notnull {
        var result = await awaitableResult;
        return await result.TapErrorAsync(tap);
    }

    public static async ValueTask<Result<TValue1, TValue2, TValue3>> TapErrorAsync<TValue1, TValue2, TValue3>(this Result<TValue1, TValue2, TValue3> result,
                                                                                                              Func<Exception, ValueTask>             tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        if (!result.Ok(out Exception? err)) {
            await tap(err);
        }

        return result;
    }

    public static async ValueTask<Result<TValue1, TValue2, TValue3>> TapErrorAsync<TValue1, TValue2, TValue3>(this ValueTask<Result<TValue1, TValue2, TValue3>> awaitableResult,
                                                                                                              Func<Exception, ValueTask>                        tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        var result = await awaitableResult;
        return await result.TapErrorAsync(tap);
    }

    public static async ValueTask<Result<TValue1, TValue2, TValue3, TValue4>> TapErrorAsync<TValue1, TValue2, TValue3, TValue4>(
        this Result<TValue1, TValue2, TValue3, TValue4> result,
        Func<Exception, ValueTask>                      tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        if (!result.Ok(out Exception? err)) {
            await tap(err);
        }

        return result;
    }

    public static async ValueTask<Result<TValue1, TValue2, TValue3, TValue4>> TapErrorAsync<TValue1, TValue2, TValue3, TValue4>(
        this ValueTask<Result<TValue1, TValue2, TValue3, TValue4>> awaitableResult,
        Func<Exception, ValueTask>                                 tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        var result = await awaitableResult;
        return await result.TapErrorAsync(tap);
    }

    public static async ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> TapErrorAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5> result,
        Func<Exception, ValueTask>                               tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        if (!result.Ok(out Exception? err)) {
            await tap(err);
        }

        return result;
    }

    public static async ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> TapErrorAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(
        this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> awaitableResult,
        Func<Exception, ValueTask>                                          tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        var result = await awaitableResult;
        return await result.TapErrorAsync(tap);
    }

    public static async ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>> TapErrorAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> result,
        Func<Exception, ValueTask>                                        tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull {
        if (!result.Ok(out Exception? err)) {
            await tap(err);
        }

        return result;
    }

    public static async ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>> TapErrorAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>> awaitableResult,
        Func<Exception, ValueTask>                                                   tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull {
        var result = await awaitableResult;
        return await result.TapErrorAsync(tap);
    }

    public static async ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>>
        TapErrorAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> result,
                                                                                     Func<Exception, ValueTask>                                                 tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull {
        if (!result.Ok(out Exception? err)) {
            await tap(err);
        }

        return result;
    }

    public static async ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>>
        TapErrorAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
            this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>> awaitableResult,
            Func<Exception, ValueTask>                                                            tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull {
        var result = await awaitableResult;
        return await result.TapErrorAsync(tap);
    }

    public static async ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>>
        TapErrorAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
            this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> result,
            Func<Exception, ValueTask>                                                          tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull {
        if (!result.Ok(out Exception? err)) {
            await tap(err);
        }

        return result;
    }

    public static async ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>>
        TapErrorAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
            this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>> awaitableResult,
            Func<Exception, ValueTask>                                                                     tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull {
        var result = await awaitableResult;
        return await result.TapErrorAsync(tap);
    }
}
