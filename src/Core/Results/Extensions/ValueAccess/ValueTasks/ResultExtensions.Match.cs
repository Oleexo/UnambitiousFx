namespace UnambitiousFx.Core.Results.Extensions.ValueAccess.ValueTasks;

public static class ResultMatchExtensions {
    public static async ValueTask<TOut> MatchAsync<TOut, TValue>(this ValueTask<Result<TValue>>   awaitableResult,
                                                                 Func<TValue, ValueTask<TOut>>    success,
                                                                 Func<Exception, ValueTask<TOut>> failure)
        where TValue : notnull
        where TOut : notnull {
        var result = await awaitableResult.ConfigureAwait(false);
        return await result.Match(success, failure);
    }

    public static async ValueTask<TOut> MatchAsync<TOut, TValue1, TValue2>(this ValueTask<Result<TValue1, TValue2>> awaitableResult,
                                                                           Func<TValue1, TValue2, ValueTask<TOut>>  success,
                                                                           Func<Exception, ValueTask<TOut>>         failure)
        where TValue1 : notnull
        where TValue2 : notnull
        where TOut : notnull {
        var result = await awaitableResult.ConfigureAwait(false);
        return await result.Match(success, failure);
    }

    public static async ValueTask<TOut> MatchAsync<TOut, TValue1, TValue2, TValue3>(this ValueTask<Result<TValue1, TValue2, TValue3>> awaitableResult,
                                                                                    Func<TValue1, TValue2, TValue3, ValueTask<TOut>>  success,
                                                                                    Func<Exception, ValueTask<TOut>>                  failure)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TOut : notnull {
        var result = await awaitableResult.ConfigureAwait(false);
        return await result.Match(success, failure);
    }

    public static async ValueTask<TOut> MatchAsync<TOut, TValue1, TValue2, TValue3, TValue4>(this ValueTask<Result<TValue1, TValue2, TValue3, TValue4>> awaitableResult,
                                                                                             Func<TValue1, TValue2, TValue3, TValue4, ValueTask<TOut>>  success,
                                                                                             Func<Exception, ValueTask<TOut>>                           failure)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TOut : notnull {
        var result = await awaitableResult.ConfigureAwait(false);
        return await result.Match(success, failure);
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
        var result = await awaitableResult.ConfigureAwait(false);
        return await result.Match(success, failure);
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
        var result = await awaitableResult.ConfigureAwait(false);
        return await result.Match(success, failure);
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
        var result = await awaitableResult.ConfigureAwait(false);
        return await result.Match(success, failure);
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
        var result = await awaitableResult.ConfigureAwait(false);
        return await result.Match(success, failure);
    }
}
