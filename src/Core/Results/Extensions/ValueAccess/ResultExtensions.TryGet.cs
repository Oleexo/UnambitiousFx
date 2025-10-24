namespace UnambitiousFx.Core.Results.Extensions.ValueAccess;

public static class ResultTryGetExtensions {
    public static bool TryGet<TValue1>(this Result<TValue1> result,
                                       out  TValue1?        value)
        where TValue1 : notnull {
        return result.Ok(out value);
    }

    public static bool TryGet<TValue1, TValue2>(this Result<TValue1, TValue2> result,
                                                out  (TValue1, TValue2)       value)
        where TValue1 : notnull
        where TValue2 : notnull {
        if (result.Ok(out var value1, out var value2)) {
            value = (value1, value2);
            return true;
        }
        value = default;
        return false;
    }

    public static bool TryGet<TValue1, TValue2, TValue3>(this Result<TValue1, TValue2, TValue3> result,
                                                         out  (TValue1, TValue2, TValue3)       value)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        if (result.Ok(out var value1, out var value2, out var value3)) {
            value = (value1, value2, value3);
            return true;
        }
        value = default;
        return false;
    }

    public static bool TryGet<TValue1, TValue2, TValue3, TValue4>(this Result<TValue1, TValue2, TValue3, TValue4> result,
                                                                  out  (TValue1, TValue2, TValue3, TValue4)       value)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        if (result.Ok(out var value1, out var value2, out var value3, out var value4)) {
            value = (value1, value2, value3, value4);
            return true;
        }
        value = default;
        return false;
    }

    public static bool TryGet<TValue1, TValue2, TValue3, TValue4, TValue5>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5> result,
                                                                           out  (TValue1, TValue2, TValue3, TValue4, TValue5)       value)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        if (result.Ok(out var value1, out var value2, out var value3, out var value4, out var value5)) {
            value = (value1, value2, value3, value4, value5);
            return true;
        }
        value = default;
        return false;
    }

    public static bool TryGet<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> result,
                                                                                    out  (TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)       value)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull {
        if (result.Ok(out var value1, out var value2, out var value3, out var value4, out var value5, out var value6)) {
            value = (value1, value2, value3, value4, value5, value6);
            return true;
        }
        value = default;
        return false;
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
        if (result.Ok(out var value1, out var value2, out var value3, out var value4, out var value5, out var value6, out var value7)) {
            value = (value1, value2, value3, value4, value5, value6, value7);
            return true;
        }
        value = default;
        return false;
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
        if (result.Ok(out var value1, out var value2, out var value3, out var value4, out var value5, out var value6, out var value7, out var value8)) {
            value = (value1, value2, value3, value4, value5, value6, value7, value8);
            return true;
        }
        value = default;
        return false;
    }
}
