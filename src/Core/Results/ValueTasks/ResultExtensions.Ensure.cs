namespace UnambitiousFx.Core.Results.ValueTasks;

public static partial class ResultExtensions {
    public static ValueTask<Result<TValue>> EnsureAsync<TValue>(this Result<TValue>                result,
                                                                Func<TValue, ValueTask<bool>>      predicate,
                                                                Func<TValue, ValueTask<Exception>> errorFactory)
        where TValue : notnull {
        return result.BindAsync(async value => {
            if (await predicate(value)) {
                return Result.Success<TValue>(value);
            }

            var ex = await errorFactory(value);
            return Result.Failure<TValue>(ex);
        });
    }

    public static ValueTask<Result<TValue>> EnsureAsync<TValue>(this ValueTask<Result<TValue>>     awaitableResult,
                                                                Func<TValue, ValueTask<bool>>      predicate,
                                                                Func<TValue, ValueTask<Exception>> errorFactory)
        where TValue : notnull {
        return awaitableResult.BindAsync(async value => {
            if (await predicate(value)) {
                return Result.Success<TValue>(value);
            }

            var ex = await errorFactory(value);
            return Result.Failure<TValue>(ex);
        });
    }

    public static ValueTask<Result<TValue1, TValue2>> EnsureAsync<TValue1, TValue2>(this Result<TValue1, TValue2>                result,
                                                                                    Func<TValue1, TValue2, ValueTask<bool>>      predicate,
                                                                                    Func<TValue1, TValue2, ValueTask<Exception>> errorFactory)
        where TValue1 : notnull
        where TValue2 : notnull {
        return result.BindAsync(async (value1,
                                       value2) => {
            if (await predicate(value1, value2)) {
                return Result.Success<TValue1, TValue2>(value1, value2);
            }

            var ex = await errorFactory(value1, value2);
            return Result.Failure<TValue1, TValue2>(ex);
        });
    }

    public static ValueTask<Result<TValue1, TValue2>> EnsureAsync<TValue1, TValue2>(this ValueTask<Result<TValue1, TValue2>>     awaitableResult,
                                                                                    Func<TValue1, TValue2, ValueTask<bool>>      predicate,
                                                                                    Func<TValue1, TValue2, ValueTask<Exception>> errorFactory)
        where TValue1 : notnull
        where TValue2 : notnull {
        return awaitableResult.BindAsync(async (value1,
                                                value2) => {
            if (await predicate(value1, value2)) {
                return Result.Success<TValue1, TValue2>(value1, value2);
            }

            var ex = await errorFactory(value1, value2);
            return Result.Failure<TValue1, TValue2>(ex);
        });
    }

    public static ValueTask<Result<TValue1, TValue2, TValue3>> EnsureAsync<TValue1, TValue2, TValue3>(this Result<TValue1, TValue2, TValue3>                result,
                                                                                                      Func<TValue1, TValue2, TValue3, ValueTask<bool>>      predicate,
                                                                                                      Func<TValue1, TValue2, TValue3, ValueTask<Exception>> errorFactory)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        return result.BindAsync(async (value1,
                                       value2,
                                       value3) => {
            if (await predicate(value1, value2, value3)) {
                return Result.Success<TValue1, TValue2, TValue3>(value1, value2, value3);
            }

            var ex = await errorFactory(value1, value2, value3);
            return Result.Failure<TValue1, TValue2, TValue3>(ex);
        });
    }

    public static ValueTask<Result<TValue1, TValue2, TValue3>> EnsureAsync<TValue1, TValue2, TValue3>(this ValueTask<Result<TValue1, TValue2, TValue3>>     awaitableResult,
                                                                                                      Func<TValue1, TValue2, TValue3, ValueTask<bool>>      predicate,
                                                                                                      Func<TValue1, TValue2, TValue3, ValueTask<Exception>> errorFactory)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        return awaitableResult.BindAsync(async (value1,
                                                value2,
                                                value3) => {
            if (await predicate(value1, value2, value3)) {
                return Result.Success<TValue1, TValue2, TValue3>(value1, value2, value3);
            }

            var ex = await errorFactory(value1, value2, value3);
            return Result.Failure<TValue1, TValue2, TValue3>(ex);
        });
    }

    public static ValueTask<Result<TValue1, TValue2, TValue3, TValue4>> EnsureAsync<TValue1, TValue2, TValue3, TValue4>(this Result<TValue1, TValue2, TValue3, TValue4> result,
        Func<TValue1, TValue2, TValue3, TValue4, ValueTask<bool>> predicate,
        Func<TValue1, TValue2, TValue3, TValue4, ValueTask<Exception>> errorFactory)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        return result.BindAsync(async (value1,
                                       value2,
                                       value3,
                                       value4) => {
            if (await predicate(value1, value2, value3, value4)) {
                return Result.Success<TValue1, TValue2, TValue3, TValue4>(value1, value2, value3, value4);
            }

            var ex = await errorFactory(value1, value2, value3, value4);
            return Result.Failure<TValue1, TValue2, TValue3, TValue4>(ex);
        });
    }

    public static ValueTask<Result<TValue1, TValue2, TValue3, TValue4>> EnsureAsync<TValue1, TValue2, TValue3, TValue4>(
        this ValueTask<Result<TValue1, TValue2, TValue3, TValue4>>     awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, ValueTask<bool>>      predicate,
        Func<TValue1, TValue2, TValue3, TValue4, ValueTask<Exception>> errorFactory)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        return awaitableResult.BindAsync(async (value1,
                                                value2,
                                                value3,
                                                value4) => {
            if (await predicate(value1, value2, value3, value4)) {
                return Result.Success<TValue1, TValue2, TValue3, TValue4>(value1, value2, value3, value4);
            }

            var ex = await errorFactory(value1, value2, value3, value4);
            return Result.Failure<TValue1, TValue2, TValue3, TValue4>(ex);
        });
    }

    public static ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> EnsureAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5>                result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, ValueTask<bool>>      predicate,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, ValueTask<Exception>> errorFactory)
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
            if (await predicate(value1, value2, value3, value4, value5)) {
                return Result.Success<TValue1, TValue2, TValue3, TValue4, TValue5>(value1, value2, value3, value4, value5);
            }

            var ex = await errorFactory(value1, value2, value3, value4, value5);
            return Result.Failure<TValue1, TValue2, TValue3, TValue4, TValue5>(ex);
        });
    }

    public static ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> EnsureAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(
        this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5>>     awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, ValueTask<bool>>      predicate,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, ValueTask<Exception>> errorFactory)
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
            if (await predicate(value1, value2, value3, value4, value5)) {
                return Result.Success<TValue1, TValue2, TValue3, TValue4, TValue5>(value1, value2, value3, value4, value5);
            }

            var ex = await errorFactory(value1, value2, value3, value4, value5);
            return Result.Failure<TValue1, TValue2, TValue3, TValue4, TValue5>(ex);
        });
    }

    public static ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>> EnsureAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>                result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, ValueTask<bool>>      predicate,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, ValueTask<Exception>> errorFactory)
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
            if (await predicate(value1, value2, value3, value4, value5, value6)) {
                return Result.Success<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(value1, value2, value3, value4, value5, value6);
            }

            var ex = await errorFactory(value1, value2, value3, value4, value5, value6);
            return Result.Failure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(ex);
        });
    }

    public static ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>> EnsureAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>>     awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, ValueTask<bool>>      predicate,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, ValueTask<Exception>> errorFactory)
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
            if (await predicate(value1, value2, value3, value4, value5, value6)) {
                return Result.Success<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(value1, value2, value3, value4, value5, value6);
            }

            var ex = await errorFactory(value1, value2, value3, value4, value5, value6);
            return Result.Failure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(ex);
        });
    }

    public static ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>> EnsureAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>                result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, ValueTask<bool>>      predicate,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, ValueTask<Exception>> errorFactory)
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
            if (await predicate(value1, value2, value3, value4, value5, value6, value7)) {
                return Result.Success<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(value1, value2, value3, value4, value5, value6, value7);
            }

            var ex = await errorFactory(value1, value2, value3, value4, value5, value6, value7);
            return Result.Failure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(ex);
        });
    }

    public static ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>> EnsureAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>>     awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, ValueTask<bool>>      predicate,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, ValueTask<Exception>> errorFactory)
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
            if (await predicate(value1, value2, value3, value4, value5, value6, value7)) {
                return Result.Success<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(value1, value2, value3, value4, value5, value6, value7);
            }

            var ex = await errorFactory(value1, value2, value3, value4, value5, value6, value7);
            return Result.Failure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(ex);
        });
    }

    public static ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>>
        EnsureAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
            this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>                result,
            Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, ValueTask<bool>>      predicate,
            Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, ValueTask<Exception>> errorFactory)
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
            if (await predicate(value1, value2, value3, value4, value5, value6, value7, value8)) {
                return Result.Success<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(value1, value2, value3, value4, value5, value6, value7, value8);
            }

            var ex = await errorFactory(value1, value2, value3, value4, value5, value6, value7, value8);
            return Result.Failure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(ex);
        });
    }

    public static ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>>
        EnsureAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
            this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>>     awaitableResult,
            Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, ValueTask<bool>>      predicate,
            Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, ValueTask<Exception>> errorFactory)
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
            if (await predicate(value1, value2, value3, value4, value5, value6, value7, value8)) {
                return Result.Success<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(value1, value2, value3, value4, value5, value6, value7, value8);
            }

            var ex = await errorFactory(value1, value2, value3, value4, value5, value6, value7, value8);
            return Result.Failure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(ex);
        });
    }
}
