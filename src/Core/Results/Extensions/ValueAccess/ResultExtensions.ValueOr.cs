namespace UnambitiousFx.Core.Results.Extensions.ValueAccess;

public static partial class ResultExtensions {
    public static TValue1 ValueOr<TValue1>(this Result<TValue1> result,
                                           TValue1              fallback)
        where TValue1 : notnull {
        return result.Match<TValue1>(value1 => value1, _ => fallback);
    }

    public static TValue1 ValueOr<TValue1>(this Result<TValue1> result,
                                           Func<TValue1>        fallbackFactory)
        where TValue1 : notnull {
        return result.Match<TValue1>(value1 => value1, _ => fallbackFactory());
    }

    public static (TValue1, TValue2) ValueOr<TValue1, TValue2>(this Result<TValue1, TValue2> result,
                                                               TValue1                       fallback1,
                                                               TValue2                       fallback2)
        where TValue1 : notnull
        where TValue2 : notnull {
        return result.Match<(TValue1, TValue2)>((value1,
                                                 value2) => (value1, value2), _ => (fallback1, fallback2));
    }

    public static (TValue1, TValue2) ValueOr<TValue1, TValue2>(this Result<TValue1, TValue2> result,
                                                               Func<(TValue1, TValue2)>      fallbackFactory)
        where TValue1 : notnull
        where TValue2 : notnull {
        return result.Match<(TValue1, TValue2)>((value1,
                                                 value2) => (value1, value2), _ => fallbackFactory());
    }

    public static (TValue1, TValue2, TValue3) ValueOr<TValue1, TValue2, TValue3>(this Result<TValue1, TValue2, TValue3> result,
                                                                                 TValue1                                fallback1,
                                                                                 TValue2                                fallback2,
                                                                                 TValue3                                fallback3)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        return result.Match<(TValue1, TValue2, TValue3)>((value1,
                                                          value2,
                                                          value3) => (value1, value2, value3), _ => (fallback1, fallback2, fallback3));
    }

    public static (TValue1, TValue2, TValue3) ValueOr<TValue1, TValue2, TValue3>(this Result<TValue1, TValue2, TValue3> result,
                                                                                 Func<(TValue1, TValue2, TValue3)>      fallbackFactory)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        return result.Match<(TValue1, TValue2, TValue3)>((value1,
                                                          value2,
                                                          value3) => (value1, value2, value3), _ => fallbackFactory());
    }

    public static (TValue1, TValue2, TValue3, TValue4) ValueOr<TValue1, TValue2, TValue3, TValue4>(this Result<TValue1, TValue2, TValue3, TValue4> result,
                                                                                                   TValue1                                         fallback1,
                                                                                                   TValue2                                         fallback2,
                                                                                                   TValue3                                         fallback3,
                                                                                                   TValue4                                         fallback4)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        return result.Match<(TValue1, TValue2, TValue3, TValue4)>((value1,
                                                                   value2,
                                                                   value3,
                                                                   value4) => (value1, value2, value3, value4), _ => (fallback1, fallback2, fallback3, fallback4));
    }

    public static (TValue1, TValue2, TValue3, TValue4) ValueOr<TValue1, TValue2, TValue3, TValue4>(this Result<TValue1, TValue2, TValue3, TValue4> result,
                                                                                                   Func<(TValue1, TValue2, TValue3, TValue4)>      fallbackFactory)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        return result.Match<(TValue1, TValue2, TValue3, TValue4)>((value1,
                                                                   value2,
                                                                   value3,
                                                                   value4) => (value1, value2, value3, value4), _ => fallbackFactory());
    }

    public static (TValue1, TValue2, TValue3, TValue4, TValue5) ValueOr<TValue1, TValue2, TValue3, TValue4, TValue5>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5> result,
        TValue1                                                  fallback1,
        TValue2                                                  fallback2,
        TValue3                                                  fallback3,
        TValue4                                                  fallback4,
        TValue5                                                  fallback5)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        return result.Match<(TValue1, TValue2, TValue3, TValue4, TValue5)>((value1,
                                                                            value2,
                                                                            value3,
                                                                            value4,
                                                                            value5) => (value1, value2, value3, value4, value5),
                                                                           _ => (fallback1, fallback2, fallback3, fallback4, fallback5));
    }

    public static (TValue1, TValue2, TValue3, TValue4, TValue5) ValueOr<TValue1, TValue2, TValue3, TValue4, TValue5>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5> result,
        Func<(TValue1, TValue2, TValue3, TValue4, TValue5)>      fallbackFactory)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        return result.Match<(TValue1, TValue2, TValue3, TValue4, TValue5)>((value1,
                                                                            value2,
                                                                            value3,
                                                                            value4,
                                                                            value5) => (value1, value2, value3, value4, value5), _ => fallbackFactory());
    }

    public static (TValue1, TValue2, TValue3, TValue4, TValue5, TValue6) ValueOr<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> result,
        TValue1                                                           fallback1,
        TValue2                                                           fallback2,
        TValue3                                                           fallback3,
        TValue4                                                           fallback4,
        TValue5                                                           fallback5,
        TValue6                                                           fallback6)
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
                                                                                     value6) => (value1, value2, value3, value4, value5, value6),
                                                                                    _ => (fallback1, fallback2, fallback3, fallback4, fallback5, fallback6));
    }

    public static (TValue1, TValue2, TValue3, TValue4, TValue5, TValue6) ValueOr<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> result,
        Func<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>      fallbackFactory)
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
                                                                                     value6) => (value1, value2, value3, value4, value5, value6), _ => fallbackFactory());
    }

    public static (TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7) ValueOr<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> result,
        TValue1                                                                    fallback1,
        TValue2                                                                    fallback2,
        TValue3                                                                    fallback3,
        TValue4                                                                    fallback4,
        TValue5                                                                    fallback5,
        TValue6                                                                    fallback6,
        TValue7                                                                    fallback7)
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
                                                                                              value7) => (value1, value2, value3, value4, value5, value6, value7),
                                                                                             _ => (fallback1, fallback2, fallback3, fallback4, fallback5, fallback6, fallback7));
    }

    public static (TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7) ValueOr<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> result,
        Func<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>      fallbackFactory)
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
                                                                                              value7) => (value1, value2, value3, value4, value5, value6, value7),
                                                                                             _ => fallbackFactory());
    }

    public static (TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8) ValueOr<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> result,
        TValue1                                                                             fallback1,
        TValue2                                                                             fallback2,
        TValue3                                                                             fallback3,
        TValue4                                                                             fallback4,
        TValue5                                                                             fallback5,
        TValue6                                                                             fallback6,
        TValue7                                                                             fallback7,
        TValue8                                                                             fallback8)
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
                                                                                                      _ => (fallback1, fallback2, fallback3, fallback4, fallback5, fallback6,
                                                                                                            fallback7, fallback8));
    }

    public static (TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8) ValueOr<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> result,
        Func<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>      fallbackFactory)
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
                                                                                                      _ => fallbackFactory());
    }
}
