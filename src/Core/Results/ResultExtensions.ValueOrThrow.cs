namespace UnambitiousFx.Core.Results;

public static partial class ResultExtensions {
    public static TValue1 ValueOrThrow<TValue1>(this Result<TValue1> result)
        where TValue1 : notnull {
        return result.Match<TValue1>(value1 => value1, e => throw e);
    }

    public static TValue1 ValueOrThrow<TValue1>(this Result<TValue1>       result,
                                                Func<Exception, Exception> exceptionFactory)
        where TValue1 : notnull {
        if (exceptionFactory is null) {
            throw new ArgumentNullException("exceptionFactory");
        }

        return result.Match<TValue1>(value1 => value1, e => throw exceptionFactory(e));
    }

    public static (TValue1, TValue2) ValueOrThrow<TValue1, TValue2>(this Result<TValue1, TValue2> result)
        where TValue1 : notnull
        where TValue2 : notnull {
        return result.Match<(TValue1, TValue2)>((value1,
                                                 value2) => (value1, value2), e => throw e);
    }

    public static (TValue1, TValue2) ValueOrThrow<TValue1, TValue2>(this Result<TValue1, TValue2> result,
                                                                    Func<Exception, Exception>    exceptionFactory)
        where TValue1 : notnull
        where TValue2 : notnull {
        if (exceptionFactory is null) {
            throw new ArgumentNullException("exceptionFactory");
        }

        return result.Match<(TValue1, TValue2)>((value1,
                                                 value2) => (value1, value2), e => throw exceptionFactory(e));
    }

    public static (TValue1, TValue2, TValue3) ValueOrThrow<TValue1, TValue2, TValue3>(this Result<TValue1, TValue2, TValue3> result)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        return result.Match<(TValue1, TValue2, TValue3)>((value1,
                                                          value2,
                                                          value3) => (value1, value2, value3), e => throw e);
    }

    public static (TValue1, TValue2, TValue3) ValueOrThrow<TValue1, TValue2, TValue3>(this Result<TValue1, TValue2, TValue3> result,
                                                                                      Func<Exception, Exception>             exceptionFactory)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        if (exceptionFactory is null) {
            throw new ArgumentNullException("exceptionFactory");
        }

        return result.Match<(TValue1, TValue2, TValue3)>((value1,
                                                          value2,
                                                          value3) => (value1, value2, value3), e => throw exceptionFactory(e));
    }

    public static (TValue1, TValue2, TValue3, TValue4) ValueOrThrow<TValue1, TValue2, TValue3, TValue4>(this Result<TValue1, TValue2, TValue3, TValue4> result)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        return result.Match<(TValue1, TValue2, TValue3, TValue4)>((value1,
                                                                   value2,
                                                                   value3,
                                                                   value4) => (value1, value2, value3, value4), e => throw e);
    }

    public static (TValue1, TValue2, TValue3, TValue4) ValueOrThrow<TValue1, TValue2, TValue3, TValue4>(this Result<TValue1, TValue2, TValue3, TValue4> result,
                                                                                                        Func<Exception, Exception>                      exceptionFactory)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        if (exceptionFactory is null) {
            throw new ArgumentNullException("exceptionFactory");
        }

        return result.Match<(TValue1, TValue2, TValue3, TValue4)>((value1,
                                                                   value2,
                                                                   value3,
                                                                   value4) => (value1, value2, value3, value4), e => throw exceptionFactory(e));
    }

    public static (TValue1, TValue2, TValue3, TValue4, TValue5) ValueOrThrow<TValue1, TValue2, TValue3, TValue4, TValue5>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5> result)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        return result.Match<(TValue1, TValue2, TValue3, TValue4, TValue5)>((value1,
                                                                            value2,
                                                                            value3,
                                                                            value4,
                                                                            value5) => (value1, value2, value3, value4, value5), e => throw e);
    }

    public static (TValue1, TValue2, TValue3, TValue4, TValue5) ValueOrThrow<TValue1, TValue2, TValue3, TValue4, TValue5>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5> result,
        Func<Exception, Exception>                               exceptionFactory)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        if (exceptionFactory is null) {
            throw new ArgumentNullException("exceptionFactory");
        }

        return result.Match<(TValue1, TValue2, TValue3, TValue4, TValue5)>((value1,
                                                                            value2,
                                                                            value3,
                                                                            value4,
                                                                            value5) => (value1, value2, value3, value4, value5), e => throw exceptionFactory(e));
    }

    public static (TValue1, TValue2, TValue3, TValue4, TValue5, TValue6) ValueOrThrow<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> result)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull {
        return result.Match<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>((value1,
                                                                                     value2,
                                                                                     value3,
                                                                                     value4,
                                                                                     value5,
                                                                                     value6) => (value1, value2, value3, value4, value5, value6), e => throw e);
    }

    public static (TValue1, TValue2, TValue3, TValue4, TValue5, TValue6) ValueOrThrow<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> result,
        Func<Exception, Exception>                                        exceptionFactory)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull {
        if (exceptionFactory is null) {
            throw new ArgumentNullException("exceptionFactory");
        }

        return result.Match<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>((value1,
                                                                                     value2,
                                                                                     value3,
                                                                                     value4,
                                                                                     value5,
                                                                                     value6) => (value1, value2, value3, value4, value5, value6), e => throw exceptionFactory(e));
    }

    public static (TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7) ValueOrThrow<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> result)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull {
        return result.Match<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>((value1,
                                                                                              value2,
                                                                                              value3,
                                                                                              value4,
                                                                                              value5,
                                                                                              value6,
                                                                                              value7) => (value1, value2, value3, value4, value5, value6, value7), e => throw e);
    }

    public static (TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7) ValueOrThrow<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> result,
        Func<Exception, Exception>                                                 exceptionFactory)
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

        return result.Match<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>((value1,
                                                                                              value2,
                                                                                              value3,
                                                                                              value4,
                                                                                              value5,
                                                                                              value6,
                                                                                              value7) => (value1, value2, value3, value4, value5, value6, value7),
                                                                                             e => throw exceptionFactory(e));
    }

    public static (TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8) ValueOrThrow<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> result)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull {
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

    public static (TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8) ValueOrThrow<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> result,
        Func<Exception, Exception>                                                          exceptionFactory)
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
