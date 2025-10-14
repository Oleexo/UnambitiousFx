namespace UnambitiousFx.Core.Results.ValueTasks;

public static partial class ResultExtensions {
    public static ValueTask<Result<TValue>> TapAsync<TValue>(this Result<TValue>     result,
                                                             Func<TValue, ValueTask> tap)
        where TValue : notnull {
        return result.BindAsync(async value => {
            await tap(value);
            return Result.Success(value);
        });
    }

    public static ValueTask<Result<TValue>> TapAsync<TValue>(this ValueTask<Result<TValue>> awaitableResult,
                                                             Func<TValue, ValueTask>        tap)
        where TValue : notnull {
        return awaitableResult.BindAsync(async value => {
            await tap(value);
            return Result.Success(value);
        });
    }

    public static ValueTask<Result<TValue1, TValue2>> TapAsync<TValue1, TValue2>(this Result<TValue1, TValue2>     result,
                                                                                 Func<TValue1, TValue2, ValueTask> tap)
        where TValue1 : notnull
        where TValue2 : notnull {
        return result.BindAsync(async (value1,
                                       value2) => {
            await tap(value1, value2);
            return Result.Success(value1, value2);
        });
    }

    public static ValueTask<Result<TValue1, TValue2>> TapAsync<TValue1, TValue2>(this ValueTask<Result<TValue1, TValue2>> awaitableResult,
                                                                                 Func<TValue1, TValue2, ValueTask>        tap)
        where TValue1 : notnull
        where TValue2 : notnull {
        return awaitableResult.BindAsync(async (value1,
                                                value2) => {
            var result = await awaitableResult;
            await tap(value1, value2);
            return Result.Success(value1, value2);
        });
    }

    public static ValueTask<Result<TValue1, TValue2, TValue3>> TapAsync<TValue1, TValue2, TValue3>(this Result<TValue1, TValue2, TValue3>     result,
                                                                                                   Func<TValue1, TValue2, TValue3, ValueTask> tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        return result.BindAsync(async (value1,
                                       value2,
                                       value3) => {
            await tap(value1, value2, value3);
            return Result.Success(value1, value2, value3);
        });
    }

    public static ValueTask<Result<TValue1, TValue2, TValue3>> TapAsync<TValue1, TValue2, TValue3>(this ValueTask<Result<TValue1, TValue2, TValue3>> awaitableResult,
                                                                                                   Func<TValue1, TValue2, TValue3, ValueTask>        tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        return awaitableResult.BindAsync(async (value1,
                                                value2,
                                                value3) => {
            var result = await awaitableResult;
            await tap(value1, value2, value3);
            return Result.Success(value1, value2, value3);
        });
    }

    public static ValueTask<Result<TValue1, TValue2, TValue3, TValue4>> TapAsync<TValue1, TValue2, TValue3, TValue4>(this Result<TValue1, TValue2, TValue3, TValue4>     result,
                                                                                                                     Func<TValue1, TValue2, TValue3, TValue4, ValueTask> tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        return result.BindAsync(async (value1,
                                       value2,
                                       value3,
                                       value4) => {
            await tap(value1, value2, value3, value4);
            return Result.Success(value1, value2, value3, value4);
        });
    }

    public static ValueTask<Result<TValue1, TValue2, TValue3, TValue4>> TapAsync<TValue1, TValue2, TValue3, TValue4>(
        this ValueTask<Result<TValue1, TValue2, TValue3, TValue4>> awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, ValueTask>        tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        return awaitableResult.BindAsync(async (value1,
                                                value2,
                                                value3,
                                                value4) => {
            var result = await awaitableResult;
            await tap(value1, value2, value3, value4);
            return Result.Success(value1, value2, value3, value4);
        });
    }

    public static ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> TapAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5>     result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, ValueTask> tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        return result.BindAsync(async (value1,
                                       value2,
                                       value3,
                                       value4,
                                       value5) => {
            await tap(value1, value2, value3, value4, value5);
            return Result.Success(value1, value2, value3, value4, value5);
        });
    }

    public static ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> TapAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(
        this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, ValueTask>        tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        return awaitableResult.BindAsync(async (value1,
                                                value2,
                                                value3,
                                                value4,
                                                value5) => {
            var result = await awaitableResult;
            await tap(value1, value2, value3, value4, value5);
            return Result.Success(value1, value2, value3, value4, value5);
        });
    }

    public static ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>> TapAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>     result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, ValueTask> tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull {
        return result.BindAsync(async (value1,
                                       value2,
                                       value3,
                                       value4,
                                       value5,
                                       value6) => {
            await tap(value1, value2, value3, value4, value5, value6);
            return Result.Success(value1, value2, value3, value4, value5, value6);
        });
    }

    public static ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>> TapAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>> awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, ValueTask>        tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull {
        return awaitableResult.BindAsync(async (value1,
                                                value2,
                                                value3,
                                                value4,
                                                value5,
                                                value6) => {
            var result = await awaitableResult;
            await tap(value1, value2, value3, value4, value5, value6);
            return Result.Success(value1, value2, value3, value4, value5, value6);
        });
    }

    public static ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>> TapAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>     result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, ValueTask> tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull {
        return result.BindAsync(async (value1,
                                       value2,
                                       value3,
                                       value4,
                                       value5,
                                       value6,
                                       value7) => {
            await tap(value1, value2, value3, value4, value5, value6, value7);
            return Result.Success(value1, value2, value3, value4, value5, value6, value7);
        });
    }

    public static ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>> TapAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>> awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, ValueTask>        tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull {
        return awaitableResult.BindAsync(async (value1,
                                                value2,
                                                value3,
                                                value4,
                                                value5,
                                                value6,
                                                value7) => {
            var result = await awaitableResult;
            await tap(value1, value2, value3, value4, value5, value6, value7);
            return Result.Success(value1, value2, value3, value4, value5, value6, value7);
        });
    }

    public static ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>>
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
        return result.BindAsync(async (value1,
                                       value2,
                                       value3,
                                       value4,
                                       value5,
                                       value6,
                                       value7,
                                       value8) => {
            await tap(value1, value2, value3, value4, value5, value6, value7, value8);
            return Result.Success(value1, value2, value3, value4, value5, value6, value7, value8);
        });
    }

    public static ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>>
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
        return awaitableResult.BindAsync(async (value1,
                                                value2,
                                                value3,
                                                value4,
                                                value5,
                                                value6,
                                                value7,
                                                value8) => {
            var result = await awaitableResult;
            await tap(value1, value2, value3, value4, value5, value6, value7, value8);
            return Result.Success(value1, value2, value3, value4, value5, value6, value7, value8);
        });
    }
}
