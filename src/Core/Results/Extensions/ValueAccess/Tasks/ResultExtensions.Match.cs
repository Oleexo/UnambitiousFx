namespace UnambitiousFx.Core.Results.Extensions.ValueAccess.Tasks;

public static partial class ResultExtensions {
    public static async Task<TOut> MatchAsync<TOut, TValue>(this Task<Result<TValue>>   awaitableResult,
                                                            Func<TValue, Task<TOut>>    success,
                                                            Func<Exception, Task<TOut>> failure)
        where TValue : notnull
        where TOut : notnull {
        var result = await awaitableResult.ConfigureAwait(false);
        return await result.Match(success, failure);
    }

    public static async Task<TOut> MatchAsync<TOut, TValue1, TValue2>(this Task<Result<TValue1, TValue2>> awaitableResult,
                                                                      Func<TValue1, TValue2, Task<TOut>>  success,
                                                                      Func<Exception, Task<TOut>>         failure)
        where TValue1 : notnull
        where TValue2 : notnull
        where TOut : notnull {
        var result = await awaitableResult.ConfigureAwait(false);
        return await result.Match(success, failure);
    }

    public static async Task<TOut> MatchAsync<TOut, TValue1, TValue2, TValue3>(this Task<Result<TValue1, TValue2, TValue3>> awaitableResult,
                                                                               Func<TValue1, TValue2, TValue3, Task<TOut>>  success,
                                                                               Func<Exception, Task<TOut>>                  failure)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TOut : notnull {
        var result = await awaitableResult.ConfigureAwait(false);
        return await result.Match(success, failure);
    }

    public static async Task<TOut> MatchAsync<TOut, TValue1, TValue2, TValue3, TValue4>(this Task<Result<TValue1, TValue2, TValue3, TValue4>> awaitableResult,
                                                                                        Func<TValue1, TValue2, TValue3, TValue4, Task<TOut>>  success,
                                                                                        Func<Exception, Task<TOut>>                           failure)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TOut : notnull {
        var result = await awaitableResult.ConfigureAwait(false);
        return await result.Match(success, failure);
    }

    public static async Task<TOut> MatchAsync<TOut, TValue1, TValue2, TValue3, TValue4, TValue5>(this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> awaitableResult,
                                                                                                 Func<TValue1, TValue2, TValue3, TValue4, TValue5, Task<TOut>>  success,
                                                                                                 Func<Exception, Task<TOut>>                                    failure)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TOut : notnull {
        var result = await awaitableResult.ConfigureAwait(false);
        return await result.Match(success, failure);
    }

    public static async Task<TOut> MatchAsync<TOut, TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>> awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Task<TOut>>  success,
        Func<Exception, Task<TOut>>                                             failure)
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

    public static async Task<TOut> MatchAsync<TOut, TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>> awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Task<TOut>>  success,
        Func<Exception, Task<TOut>>                                                      failure)
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

    public static async Task<TOut> MatchAsync<TOut, TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>> awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Task<TOut>>  success,
        Func<Exception, Task<TOut>>                                                               failure)
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
