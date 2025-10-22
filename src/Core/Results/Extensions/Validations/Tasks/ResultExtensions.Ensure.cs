using UnambitiousFx.Core.Results.Extensions.Transformations.Tasks;

namespace UnambitiousFx.Core.Results.Extensions.Validations.Tasks;

public static partial class ResultExtensions {
    public static Task<Result<TValue>> EnsureAsync<TValue>(this Result<TValue>           result,
                                                           Func<TValue, Task<bool>>      predicate,
                                                           Func<TValue, Task<Exception>> errorFactory)
        where TValue : notnull {
        return result.BindAsync(async value => {
            if (await predicate(value)) {
                return Result.Success(value);
            }

            var ex = await errorFactory(value);
            return Result.Failure<TValue>(ex);
        });
    }

    public static async Task<Result<TValue>> EnsureAsync<TValue>(this Task<Result<TValue>>     awaitableResult,
                                                                 Func<TValue, Task<bool>>      predicate,
                                                                 Func<TValue, Task<Exception>> errorFactory)
        where TValue : notnull {
        var result = await awaitableResult;
        return await result.EnsureAsync(predicate, errorFactory);
    }

    public static Task<Result<TValue1, TValue2>> EnsureAsync<TValue1, TValue2>(this Result<TValue1, TValue2>           result,
                                                                               Func<TValue1, TValue2, Task<bool>>      predicate,
                                                                               Func<TValue1, TValue2, Task<Exception>> errorFactory)
        where TValue1 : notnull
        where TValue2 : notnull {
        return result.BindAsync(async (value1,
                                       value2) => {
            if (await predicate(value1, value2)) {
                return Result.Success(value1, value2);
            }

            var ex = await errorFactory(value1, value2);
            return Result.Failure<TValue1, TValue2>(ex);
        });
    }

    public static async Task<Result<TValue1, TValue2>> EnsureAsync<TValue1, TValue2>(this Task<Result<TValue1, TValue2>>     awaitableResult,
                                                                                     Func<TValue1, TValue2, Task<bool>>      predicate,
                                                                                     Func<TValue1, TValue2, Task<Exception>> errorFactory)
        where TValue1 : notnull
        where TValue2 : notnull {
        var result = await awaitableResult;
        return await result.EnsureAsync(predicate, errorFactory);
    }

    public static Task<Result<TValue1, TValue2, TValue3>> EnsureAsync<TValue1, TValue2, TValue3>(this Result<TValue1, TValue2, TValue3>           result,
                                                                                                 Func<TValue1, TValue2, TValue3, Task<bool>>      predicate,
                                                                                                 Func<TValue1, TValue2, TValue3, Task<Exception>> errorFactory)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        return result.BindAsync(async (value1,
                                       value2,
                                       value3) => {
            if (await predicate(value1, value2, value3)) {
                return Result.Success(value1, value2, value3);
            }

            var ex = await errorFactory(value1, value2, value3);
            return Result.Failure<TValue1, TValue2, TValue3>(ex);
        });
    }

    public static async Task<Result<TValue1, TValue2, TValue3>> EnsureAsync<TValue1, TValue2, TValue3>(this Task<Result<TValue1, TValue2, TValue3>>     awaitableResult,
                                                                                                       Func<TValue1, TValue2, TValue3, Task<bool>>      predicate,
                                                                                                       Func<TValue1, TValue2, TValue3, Task<Exception>> errorFactory)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        var result = await awaitableResult;
        return await result.EnsureAsync(predicate, errorFactory);
    }

    public static Task<Result<TValue1, TValue2, TValue3, TValue4>> EnsureAsync<TValue1, TValue2, TValue3, TValue4>(this Result<TValue1, TValue2, TValue3, TValue4>      result,
                                                                                                                   Func<TValue1, TValue2, TValue3, TValue4, Task<bool>> predicate,
                                                                                                                   Func<TValue1, TValue2, TValue3, TValue4, Task<Exception>>
                                                                                                                       errorFactory)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        return result.BindAsync(async (value1,
                                       value2,
                                       value3,
                                       value4) => {
            if (await predicate(value1, value2, value3, value4)) {
                return Result.Success(value1, value2, value3, value4);
            }

            var ex = await errorFactory(value1, value2, value3, value4);
            return Result.Failure<TValue1, TValue2, TValue3, TValue4>(ex);
        });
    }

    public static async Task<Result<TValue1, TValue2, TValue3, TValue4>> EnsureAsync<TValue1, TValue2, TValue3, TValue4>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4>>     awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, Task<bool>>      predicate,
        Func<TValue1, TValue2, TValue3, TValue4, Task<Exception>> errorFactory)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        var result = await awaitableResult;
        return await result.EnsureAsync(predicate, errorFactory);
    }

    public static Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> EnsureAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5>           result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, Task<bool>>      predicate,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, Task<Exception>> errorFactory)
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
                return Result.Success(value1, value2, value3, value4, value5);
            }

            var ex = await errorFactory(value1, value2, value3, value4, value5);
            return Result.Failure<TValue1, TValue2, TValue3, TValue4, TValue5>(ex);
        });
    }

    public static async Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> EnsureAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5>>     awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, Task<bool>>      predicate,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, Task<Exception>> errorFactory)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        var result = await awaitableResult;
        return await result.EnsureAsync(predicate, errorFactory);
    }

    public static Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>> EnsureAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>           result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Task<bool>>      predicate,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Task<Exception>> errorFactory)
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
                return Result.Success(value1, value2, value3, value4, value5, value6);
            }

            var ex = await errorFactory(value1, value2, value3, value4, value5, value6);
            return Result.Failure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(ex);
        });
    }

    public static async Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>> EnsureAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>>     awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Task<bool>>      predicate,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Task<Exception>> errorFactory)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull {
        var result = await awaitableResult;
        return await result.EnsureAsync(predicate, errorFactory);
    }

    public static Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>> EnsureAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>           result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Task<bool>>      predicate,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Task<Exception>> errorFactory)
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
                return Result.Success(value1, value2, value3, value4, value5, value6, value7);
            }

            var ex = await errorFactory(value1, value2, value3, value4, value5, value6, value7);
            return Result.Failure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(ex);
        });
    }

    public static async Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>> EnsureAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>>     awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Task<bool>>      predicate,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Task<Exception>> errorFactory)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull {
        var result = await awaitableResult;
        return await result.EnsureAsync(predicate, errorFactory);
    }

    public static Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>>
        EnsureAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
            this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>           result,
            Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Task<bool>>      predicate,
            Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Task<Exception>> errorFactory)
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
                return Result.Success(value1, value2, value3, value4, value5, value6, value7, value8);
            }

            var ex = await errorFactory(value1, value2, value3, value4, value5, value6, value7, value8);
            return Result.Failure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(ex);
        });
    }

    public static async Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>>
        EnsureAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
            this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>>     awaitableResult,
            Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Task<bool>>      predicate,
            Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Task<Exception>> errorFactory)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull {
        var result = await awaitableResult;
        return await result.EnsureAsync(predicate, errorFactory);
    }
}
