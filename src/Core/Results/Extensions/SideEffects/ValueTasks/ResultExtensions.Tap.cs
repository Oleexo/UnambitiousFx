namespace UnambitiousFx.Core.Results.Extensions.SideEffects.ValueTasks;

public static partial class ResultExtensions {
    public static async ValueTask<Result<TValue>> TapAsync<TValue>(this Result<TValue>     result,
                                                                   Func<TValue, ValueTask> tap)
        where TValue : notnull {
        if (result.Ok(out var value)) {
            await tap(value);
        }

        return result;
    }

    public static async ValueTask<Result<TValue>> TapAsync<TValue>(this ValueTask<Result<TValue>> awaitableResult,
                                                                   Func<TValue, ValueTask>        tap)
        where TValue : notnull {
        var result = await awaitableResult;
        return await result.TapAsync(tap);
    }

    public static async ValueTask<Result<TValue1, TValue2>> TapAsync<TValue1, TValue2>(this Result<TValue1, TValue2>     result,
                                                                                       Func<TValue1, TValue2, ValueTask> tap)
        where TValue1 : notnull
        where TValue2 : notnull {
        if (result.Ok(out var value1, out var value2)) {
            await tap(value1, value2);
        }

        return result;
    }

    public static async ValueTask<Result<TValue1, TValue2>> TapAsync<TValue1, TValue2>(this ValueTask<Result<TValue1, TValue2>> awaitableResult,
                                                                                       Func<TValue1, TValue2, ValueTask>        tap)
        where TValue1 : notnull
        where TValue2 : notnull {
        var result = await awaitableResult;
        return await result.TapAsync(tap);
    }

    public static async ValueTask<Result<TValue1, TValue2, TValue3>> TapAsync<TValue1, TValue2, TValue3>(this Result<TValue1, TValue2, TValue3>     result,
                                                                                                         Func<TValue1, TValue2, TValue3, ValueTask> tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        if (result.Ok(out var value1, out var value2, out var value3)) {
            await tap(value1, value2, value3);
        }

        return result;
    }

    public static async ValueTask<Result<TValue1, TValue2, TValue3>> TapAsync<TValue1, TValue2, TValue3>(this ValueTask<Result<TValue1, TValue2, TValue3>> awaitableResult,
                                                                                                         Func<TValue1, TValue2, TValue3, ValueTask>        tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        var result = await awaitableResult;
        return await result.TapAsync(tap);
    }

    public static async ValueTask<Result<TValue1, TValue2, TValue3, TValue4>> TapAsync<TValue1, TValue2, TValue3, TValue4>(this Result<TValue1, TValue2, TValue3, TValue4> result,
        Func<TValue1, TValue2, TValue3, TValue4, ValueTask>                                                                                                                tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        if (result.Ok(out var value1, out var value2, out var value3, out var value4)) {
            await tap(value1, value2, value3, value4);
        }

        return result;
    }

    public static async ValueTask<Result<TValue1, TValue2, TValue3, TValue4>> TapAsync<TValue1, TValue2, TValue3, TValue4>(
        this ValueTask<Result<TValue1, TValue2, TValue3, TValue4>> awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, ValueTask>        tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        var result = await awaitableResult;
        return await result.TapAsync(tap);
    }

    public static async ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> TapAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5>     result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, ValueTask> tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        if (result.Ok(out var value1, out var value2, out var value3, out var value4, out var value5)) {
            await tap(value1, value2, value3, value4, value5);
        }

        return result;
    }

    public static async ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> TapAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(
        this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, ValueTask>        tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        var result = await awaitableResult;
        return await result.TapAsync(tap);
    }

    public static async ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>> TapAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>     result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, ValueTask> tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull {
        if (result.Ok(out var value1, out var value2, out var value3, out var value4, out var value5, out var value6)) {
            await tap(value1, value2, value3, value4, value5, value6);
        }

        return result;
    }

    public static async ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>> TapAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>> awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, ValueTask>        tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull {
        var result = await awaitableResult;
        return await result.TapAsync(tap);
    }

    public static async ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>> TapAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>     result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, ValueTask> tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull {
        if (result.Ok(out var value1, out var value2, out var value3, out var value4, out var value5, out var value6, out var value7)) {
            await tap(value1, value2, value3, value4, value5, value6, value7);
        }

        return result;
    }

    public static async ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>> TapAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>> awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, ValueTask>        tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull {
        var result = await awaitableResult;
        return await result.TapAsync(tap);
    }

    public static async ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>>
        TapAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> result,
                                                                                         Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, ValueTask>
                                                                                             tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull {
        if (result.Ok(out var value1, out var value2, out var value3, out var value4, out var value5, out var value6, out var value7, out var value8)) {
            await tap(value1, value2, value3, value4, value5, value6, value7, value8);
        }

        return result;
    }

    public static async ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>>
        TapAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
            this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>> awaitableResult,
            Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, ValueTask>        tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull {
        var result = await awaitableResult;
        return await result.TapAsync(tap);
    }
}
