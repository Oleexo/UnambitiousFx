namespace UnambitiousFx.Core.Results;

public static partial class ResultExtensions {
    public static bool TryGet<TValue1>(this Result<TValue1> result,
                                       out  TValue1?        value)
        where TValue1 : notnull {
        return result.Ok(out value);
    }

    public static bool TryGet<TValue1, TValue2>(this Result<TValue1, TValue2> result,
                                                out  (TValue1, TValue2)       value)
        where TValue1 : notnull
        where TValue2 : notnull {
        return result.Ok(out value);
    }

    public static bool TryGet<TValue1, TValue2, TValue3>(this Result<TValue1, TValue2, TValue3> result,
                                                         out  (TValue1, TValue2, TValue3)       value)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        return result.Ok(out value);
    }

    public static bool TryGet<TValue1, TValue2, TValue3, TValue4>(this Result<TValue1, TValue2, TValue3, TValue4> result,
                                                                  out  (TValue1, TValue2, TValue3, TValue4)       value)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        return result.Ok(out value);
    }

    public static bool TryGet<TValue1, TValue2, TValue3, TValue4, TValue5>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5> result,
                                                                           out  (TValue1, TValue2, TValue3, TValue4, TValue5)       value)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        return result.Ok(out value);
    }

    public static bool TryGet<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> result,
                                                                                    out  (TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)       value)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull {
        return result.Ok(out value);
    }

    public static bool TryGet<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> result,
                                                                                             out  (TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)       value)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull {
        return result.Ok(out value);
    }

    public static bool TryGet<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> result,
        out  (TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)       value)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull {
        return result.Ok(out value);
    }
}
