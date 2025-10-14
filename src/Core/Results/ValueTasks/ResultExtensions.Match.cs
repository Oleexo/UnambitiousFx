namespace UnambitiousFx.Core.Results.ValueTasks;

public static partial class ResultExtensions {
    public static async ValueTask<TOut> MatchAsync<TOut, TValue>(this ValueTask<Result<TValue>>   awaitableResult,
                                                                 Func<TValue, ValueTask<TOut>>    success,
                                                                 Func<Exception, ValueTask<TOut>> failure)
        where TValue : notnull
        where TOut : notnull {
        var result = await awaitableResult;
        return await result.Match<ValueTask<TOut>>(success, failure);
    }

    public static async ValueTask<TOut> MatchAsync<TOut, TValue1, TValue2>(this ValueTask<Result<TValue1, TValue2>> awaitableResult,
                                                                           Func<TValue1, TValue2, ValueTask<TOut>>  success,
                                                                           Func<Exception, ValueTask<TOut>>         failure)
        where TValue1 : notnull
        where TValue2 : notnull
        where TOut : notnull {
        var result = await awaitableResult;
        return await result.Match<ValueTask<TOut>>((value1,
                                                    value2) => success(value1, value2), failure);
    }

    public static async ValueTask<TOut> MatchAsync<TOut, TValue1, TValue2, TValue3>(this ValueTask<Result<TValue1, TValue2, TValue3>> awaitableResult,
                                                                                    Func<TValue1, TValue2, TValue3, ValueTask<TOut>>  success,
                                                                                    Func<Exception, ValueTask<TOut>>                  failure)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TOut : notnull {
        var result = await awaitableResult;
        return await result.Match<ValueTask<TOut>>((value1,
                                                    value2,
                                                    value3) => success(value1, value2, value3), failure);
    }

    public static async ValueTask<TOut> MatchAsync<TOut, TValue1, TValue2, TValue3, TValue4>(this ValueTask<Result<TValue1, TValue2, TValue3, TValue4>> awaitableResult,
                                                                                             Func<TValue1, TValue2, TValue3, TValue4, ValueTask<TOut>>  success,
                                                                                             Func<Exception, ValueTask<TOut>>                           failure)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TOut : notnull {
        var result = await awaitableResult;
        return await result.Match<ValueTask<TOut>>((value1,
                                                    value2,
                                                    value3,
                                                    value4) => success(value1, value2, value3, value4), failure);
    }

    public static async ValueTask<TOut> MatchAsync<TOut, TValue1, TValue2, TValue3, TValue4, TValue5>(
        this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, ValueTask<TOut>>  success,
        Func<Exception, ValueTask<TOut>>                                    failure)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TOut : notnull {
        var result = await awaitableResult;
        return await result.Match<ValueTask<TOut>>((value1,
                                                    value2,
                                                    value3,
                                                    value4,
                                                    value5) => success(value1, value2, value3, value4, value5), failure);
    }

    public static async ValueTask<TOut> MatchAsync<TOut, TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>> awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, ValueTask<TOut>>  success,
        Func<Exception, ValueTask<TOut>>                                             failure)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TOut : notnull {
        var result = await awaitableResult;
        return await result.Match<ValueTask<TOut>>((value1,
                                                    value2,
                                                    value3,
                                                    value4,
                                                    value5,
                                                    value6) => success(value1, value2, value3, value4, value5, value6), failure);
    }

    public static async ValueTask<TOut> MatchAsync<TOut, TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>> awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, ValueTask<TOut>>  success,
        Func<Exception, ValueTask<TOut>>                                                      failure)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TOut : notnull {
        var result = await awaitableResult;
        return await result.Match<ValueTask<TOut>>((value1,
                                                    value2,
                                                    value3,
                                                    value4,
                                                    value5,
                                                    value6,
                                                    value7) => success(value1, value2, value3, value4, value5, value6, value7), failure);
    }

    public static async ValueTask<TOut> MatchAsync<TOut, TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
        this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>> awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, ValueTask<TOut>>  success,
        Func<Exception, ValueTask<TOut>>                                                               failure)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull
        where TOut : notnull {
        var result = await awaitableResult;
        return await result.Match<ValueTask<TOut>>((value1,
                                                    value2,
                                                    value3,
                                                    value4,
                                                    value5,
                                                    value6,
                                                    value7,
                                                    value8) => success(value1, value2, value3, value4, value5, value6, value7, value8), failure);
    }
}
