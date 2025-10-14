namespace UnambitiousFx.Core.Results.ValueTasks;

public static partial class ResultExtensions {
    public static async ValueTask<TValue1> ValueOrThrow<TValue1>(this ValueTask<Result<TValue1>> awaitable)
        where TValue1 : notnull {
        var result = await awaitable.ConfigureAwait(false);
        return result.Match<TValue1>(value1 => value1, e => throw e);
    }

    public static async ValueTask<TValue1> ValueOrThrow<TValue1>(this ValueTask<Result<TValue1>> awaitable,
                                                                 Func<Exception, Exception>      exceptionFactory)
        where TValue1 : notnull {
        if (exceptionFactory is null) {
            throw new ArgumentNullException("exceptionFactory");
        }

        var result = await awaitable.ConfigureAwait(false);
        return result.Match<TValue1>(value1 => value1, e => throw exceptionFactory(e));
    }

    public static async ValueTask<(TValue1, TValue2)> ValueOrThrow<TValue1, TValue2>(this ValueTask<Result<TValue1, TValue2>> awaitable)
        where TValue1 : notnull
        where TValue2 : notnull {
        var result = await awaitable.ConfigureAwait(false);
        return result.Match<(TValue1, TValue2)>((value1,
                                                 value2) => (value1, value2), e => throw e);
    }

    public static async ValueTask<(TValue1, TValue2)> ValueOrThrow<TValue1, TValue2>(this ValueTask<Result<TValue1, TValue2>> awaitable,
                                                                                     Func<Exception, Exception>               exceptionFactory)
        where TValue1 : notnull
        where TValue2 : notnull {
        if (exceptionFactory is null) {
            throw new ArgumentNullException("exceptionFactory");
        }

        var result = await awaitable.ConfigureAwait(false);
        return result.Match<(TValue1, TValue2)>((value1,
                                                 value2) => (value1, value2), e => throw exceptionFactory(e));
    }

    public static async ValueTask<(TValue1, TValue2, TValue3)> ValueOrThrow<TValue1, TValue2, TValue3>(this ValueTask<Result<TValue1, TValue2, TValue3>> awaitable)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        var result = await awaitable.ConfigureAwait(false);
        return result.Match<(TValue1, TValue2, TValue3)>((value1,
                                                          value2,
                                                          value3) => (value1, value2, value3), e => throw e);
    }

    public static async ValueTask<(TValue1, TValue2, TValue3)> ValueOrThrow<TValue1, TValue2, TValue3>(this ValueTask<Result<TValue1, TValue2, TValue3>> awaitable,
                                                                                                       Func<Exception, Exception>                        exceptionFactory)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        if (exceptionFactory is null) {
            throw new ArgumentNullException("exceptionFactory");
        }

        var result = await awaitable.ConfigureAwait(false);
        return result.Match<(TValue1, TValue2, TValue3)>((value1,
                                                          value2,
                                                          value3) => (value1, value2, value3), e => throw exceptionFactory(e));
    }

    public static async ValueTask<(TValue1, TValue2, TValue3, TValue4)> ValueOrThrow<TValue1, TValue2, TValue3, TValue4>(
        this ValueTask<Result<TValue1, TValue2, TValue3, TValue4>> awaitable)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        var result = await awaitable.ConfigureAwait(false);
        return result.Match<(TValue1, TValue2, TValue3, TValue4)>((value1,
                                                                   value2,
                                                                   value3,
                                                                   value4) => (value1, value2, value3, value4), e => throw e);
    }

    public static async ValueTask<(TValue1, TValue2, TValue3, TValue4)> ValueOrThrow<TValue1, TValue2, TValue3, TValue4>(
        this ValueTask<Result<TValue1, TValue2, TValue3, TValue4>> awaitable,
        Func<Exception, Exception>                                 exceptionFactory)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        if (exceptionFactory is null) {
            throw new ArgumentNullException("exceptionFactory");
        }

        var result = await awaitable.ConfigureAwait(false);
        return result.Match<(TValue1, TValue2, TValue3, TValue4)>((value1,
                                                                   value2,
                                                                   value3,
                                                                   value4) => (value1, value2, value3, value4), e => throw exceptionFactory(e));
    }

    public static async ValueTask<(TValue1, TValue2, TValue3, TValue4, TValue5)> ValueOrThrow<TValue1, TValue2, TValue3, TValue4, TValue5>(
        this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> awaitable)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        var result = await awaitable.ConfigureAwait(false);
        return result.Match<(TValue1, TValue2, TValue3, TValue4, TValue5)>((value1,
                                                                            value2,
                                                                            value3,
                                                                            value4,
                                                                            value5) => (value1, value2, value3, value4, value5), e => throw e);
    }

    public static async ValueTask<(TValue1, TValue2, TValue3, TValue4, TValue5)> ValueOrThrow<TValue1, TValue2, TValue3, TValue4, TValue5>(
        this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> awaitable,
        Func<Exception, Exception>                                          exceptionFactory)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        if (exceptionFactory is null) {
            throw new ArgumentNullException("exceptionFactory");
        }

        var result = await awaitable.ConfigureAwait(false);
        return result.Match<(TValue1, TValue2, TValue3, TValue4, TValue5)>((value1,
                                                                            value2,
                                                                            value3,
                                                                            value4,
                                                                            value5) => (value1, value2, value3, value4, value5), e => throw exceptionFactory(e));
    }

    public static async ValueTask<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)> ValueOrThrow<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>> awaitable)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull {
        var result = await awaitable.ConfigureAwait(false);
        return result.Match<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>((value1,
                                                                                     value2,
                                                                                     value3,
                                                                                     value4,
                                                                                     value5,
                                                                                     value6) => (value1, value2, value3, value4, value5, value6), e => throw e);
    }

    public static async ValueTask<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)> ValueOrThrow<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>> awaitable,
        Func<Exception, Exception>                                                   exceptionFactory)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull {
        if (exceptionFactory is null) {
            throw new ArgumentNullException("exceptionFactory");
        }

        var result = await awaitable.ConfigureAwait(false);
        return result.Match<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>((value1,
                                                                                     value2,
                                                                                     value3,
                                                                                     value4,
                                                                                     value5,
                                                                                     value6) => (value1, value2, value3, value4, value5, value6), e => throw exceptionFactory(e));
    }

    public static async ValueTask<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)> ValueOrThrow<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>> awaitable)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull {
        var result = await awaitable.ConfigureAwait(false);
        return result.Match<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>((value1,
                                                                                              value2,
                                                                                              value3,
                                                                                              value4,
                                                                                              value5,
                                                                                              value6,
                                                                                              value7) => (value1, value2, value3, value4, value5, value6, value7), e => throw e);
    }

    public static async ValueTask<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)> ValueOrThrow<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>> awaitable,
        Func<Exception, Exception>                                                            exceptionFactory)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull {
        if (exceptionFactory is null) {
            throw new ArgumentNullException("exceptionFactory");
        }

        var result = await awaitable.ConfigureAwait(false);
        return result.Match<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>((value1,
                                                                                              value2,
                                                                                              value3,
                                                                                              value4,
                                                                                              value5,
                                                                                              value6,
                                                                                              value7) => (value1, value2, value3, value4, value5, value6, value7),
                                                                                             e => throw exceptionFactory(e));
    }

    public static async ValueTask<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>
        ValueOrThrow<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
            this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>> awaitable)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull {
        var result = await awaitable.ConfigureAwait(false);
        return result.Match<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>((value1,
                                                                                                       value2,
                                                                                                       value3,
                                                                                                       value4,
                                                                                                       value5,
                                                                                                       value6,
                                                                                                       value7,
                                                                                                       value8) => (value1, value2, value3, value4, value5, value6, value7, value8),
                                                                                                      e => throw e);
    }

    public static async ValueTask<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>
        ValueOrThrow<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
            this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>> awaitable,
            Func<Exception, Exception>                                                                     exceptionFactory)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull {
        if (exceptionFactory is null) {
            throw new ArgumentNullException("exceptionFactory");
        }

        var result = await awaitable.ConfigureAwait(false);
        return result.Match<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>((value1,
                                                                                                       value2,
                                                                                                       value3,
                                                                                                       value4,
                                                                                                       value5,
                                                                                                       value6,
                                                                                                       value7,
                                                                                                       value8) => (value1, value2, value3, value4, value5, value6, value7, value8),
                                                                                                      e => throw exceptionFactory(e));
    }
}
