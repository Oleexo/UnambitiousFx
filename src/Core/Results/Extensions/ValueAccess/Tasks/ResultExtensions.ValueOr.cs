namespace UnambitiousFx.Core.Results.Extensions.ValueAccess.Tasks;

public static class ResultValueOrExtensions {
    public static async Task<TValue1> ValueOrAsync<TValue1>(this Task<Result<TValue1>> awaitableResult,
                                                            TValue1                    fallback)
        where TValue1 : notnull {
        var result = await awaitableResult.ConfigureAwait(false);
        return result.ValueOr(fallback);
    }

    public static async Task<TValue1> ValueOrAsync<TValue1>(this Task<Result<TValue1>> awaitableResult,
                                                            Func<TValue1>              fallbackFactory)
        where TValue1 : notnull {
        var result = await awaitableResult.ConfigureAwait(false);
        return result.ValueOr(fallbackFactory);
    }

    public static async Task<(TValue1, TValue2)> ValueOrAsync<TValue1, TValue2>(this Task<Result<TValue1, TValue2>> awaitableResult,
                                                                                TValue1                             fallback1,
                                                                                TValue2                             fallback2)
        where TValue1 : notnull
        where TValue2 : notnull {
        var result = await awaitableResult.ConfigureAwait(false);
        return result.ValueOr(fallback1, fallback2);
    }

    public static async Task<(TValue1, TValue2)> ValueOrAsync<TValue1, TValue2>(this Task<Result<TValue1, TValue2>> awaitableResult,
                                                                                Func<(TValue1, TValue2)>            fallbackFactory)
        where TValue1 : notnull
        where TValue2 : notnull {
        var result = await awaitableResult.ConfigureAwait(false);
        return result.ValueOr(fallbackFactory);
    }

    public static async Task<(TValue1, TValue2, TValue3)> ValueOrAsync<TValue1, TValue2, TValue3>(this Task<Result<TValue1, TValue2, TValue3>> awaitableResult,
                                                                                                  TValue1                                      fallback1,
                                                                                                  TValue2                                      fallback2,
                                                                                                  TValue3                                      fallback3)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        var result = await awaitableResult.ConfigureAwait(false);
        return result.ValueOr(fallback1, fallback2, fallback3);
    }

    public static async Task<(TValue1, TValue2, TValue3)> ValueOrAsync<TValue1, TValue2, TValue3>(this Task<Result<TValue1, TValue2, TValue3>> awaitableResult,
                                                                                                  Func<(TValue1, TValue2, TValue3)>            fallbackFactory)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        var result = await awaitableResult.ConfigureAwait(false);
        return result.ValueOr(fallbackFactory);
    }

    public static async Task<(TValue1, TValue2, TValue3, TValue4)> ValueOrAsync<TValue1, TValue2, TValue3, TValue4>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4>> awaitableResult,
        TValue1                                               fallback1,
        TValue2                                               fallback2,
        TValue3                                               fallback3,
        TValue4                                               fallback4)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        var result = await awaitableResult.ConfigureAwait(false);
        return result.ValueOr(fallback1, fallback2, fallback3, fallback4);
    }

    public static async Task<(TValue1, TValue2, TValue3, TValue4)> ValueOrAsync<TValue1, TValue2, TValue3, TValue4>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4>> awaitableResult,
        Func<(TValue1, TValue2, TValue3, TValue4)>            fallbackFactory)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        var result = await awaitableResult.ConfigureAwait(false);
        return result.ValueOr(fallbackFactory);
    }

    public static async Task<(TValue1, TValue2, TValue3, TValue4, TValue5)> ValueOrAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> awaitableResult,
        TValue1                                                        fallback1,
        TValue2                                                        fallback2,
        TValue3                                                        fallback3,
        TValue4                                                        fallback4,
        TValue5                                                        fallback5)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        var result = await awaitableResult.ConfigureAwait(false);
        return result.ValueOr(fallback1, fallback2, fallback3, fallback4, fallback5);
    }

    public static async Task<(TValue1, TValue2, TValue3, TValue4, TValue5)> ValueOrAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> awaitableResult,
        Func<(TValue1, TValue2, TValue3, TValue4, TValue5)>            fallbackFactory)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        var result = await awaitableResult.ConfigureAwait(false);
        return result.ValueOr(fallbackFactory);
    }

    public static async Task<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)> ValueOrAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>> awaitableResult,
        TValue1                                                                 fallback1,
        TValue2                                                                 fallback2,
        TValue3                                                                 fallback3,
        TValue4                                                                 fallback4,
        TValue5                                                                 fallback5,
        TValue6                                                                 fallback6)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull {
        var result = await awaitableResult.ConfigureAwait(false);
        return result.ValueOr(fallback1, fallback2, fallback3, fallback4, fallback5, fallback6);
    }

    public static async Task<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)> ValueOrAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>> awaitableResult,
        Func<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>            fallbackFactory)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull {
        var result = await awaitableResult.ConfigureAwait(false);
        return result.ValueOr(fallbackFactory);
    }

    public static async Task<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)> ValueOrAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>> awaitableResult,
        TValue1                                                                          fallback1,
        TValue2                                                                          fallback2,
        TValue3                                                                          fallback3,
        TValue4                                                                          fallback4,
        TValue5                                                                          fallback5,
        TValue6                                                                          fallback6,
        TValue7                                                                          fallback7)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull {
        var result = await awaitableResult.ConfigureAwait(false);
        return result.ValueOr(fallback1, fallback2, fallback3, fallback4, fallback5, fallback6, fallback7);
    }

    public static async Task<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)> ValueOrAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>> awaitableResult,
        Func<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>            fallbackFactory)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull {
        var result = await awaitableResult.ConfigureAwait(false);
        return result.ValueOr(fallbackFactory);
    }

    public static async Task<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>
        ValueOrAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
            this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>> awaitableResult,
            TValue1                                                                                   fallback1,
            TValue2                                                                                   fallback2,
            TValue3                                                                                   fallback3,
            TValue4                                                                                   fallback4,
            TValue5                                                                                   fallback5,
            TValue6                                                                                   fallback6,
            TValue7                                                                                   fallback7,
            TValue8                                                                                   fallback8)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull {
        var result = await awaitableResult.ConfigureAwait(false);
        return result.ValueOr(fallback1, fallback2, fallback3, fallback4, fallback5, fallback6, fallback7, fallback8);
    }

    public static async Task<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>
        ValueOrAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
            this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>> awaitableResult,
            Func<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>            fallbackFactory)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull {
        var result = await awaitableResult.ConfigureAwait(false);
        return result.ValueOr(fallbackFactory);
    }
}
