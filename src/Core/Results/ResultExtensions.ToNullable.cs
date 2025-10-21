namespace UnambitiousFx.Core.Results;

public static partial class ResultExtensions {
    public static TValue1? ToNullable<TValue1>(this Result<TValue1> result)
        where TValue1 : notnull {
        return result.Ok(out var value)
                   ? (TValue1?)value
                   : default;
    }

    public static (TValue1, TValue2)? ToNullable<TValue1, TValue2>(this Result<TValue1, TValue2> result)
        where TValue1 : notnull
        where TValue2 : notnull {
        if (!result.IsSuccess) {
            return default;
        }

        return result.Ok(out (TValue1, TValue2) value)
                   ? value
                   : default;
    }

    public static (TValue1, TValue2, TValue3)? ToNullable<TValue1, TValue2, TValue3>(this Result<TValue1, TValue2, TValue3> result)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        if (!result.IsSuccess) {
            return default;
        }

        return result.Ok(out (TValue1, TValue2, TValue3) value)
                   ? value
                   : default;
    }

    public static (TValue1, TValue2, TValue3, TValue4)? ToNullable<TValue1, TValue2, TValue3, TValue4>(this Result<TValue1, TValue2, TValue3, TValue4> result)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        if (!result.IsSuccess) {
            return default;
        }

        return result.Ok(out (TValue1, TValue2, TValue3, TValue4) value)
                   ? value
                   : default;
    }

    public static (TValue1, TValue2, TValue3, TValue4, TValue5)? ToNullable<TValue1, TValue2, TValue3, TValue4, TValue5>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5> result)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        if (!result.IsSuccess) {
            return default;
        }

        return result.Ok(out (TValue1, TValue2, TValue3, TValue4, TValue5) value)
                   ? value
                   : default;
    }

    public static (TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)? ToNullable<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> result)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull {
        if (!result.IsSuccess) {
            return default;
        }

        return result.Ok(out (TValue1, TValue2, TValue3, TValue4, TValue5, TValue6) value)
                   ? value
                   : default;
    }

    public static (TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)? ToNullable<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> result)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull {
        if (!result.IsSuccess) {
            return default;
        }

        return result.Ok(out (TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7) value)
                   ? value
                   : default;
    }

    public static (TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)? ToNullable<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> result)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull {
        if (!result.IsSuccess) {
            return default;
        }

        return result.Ok(out (TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8) value)
                   ? value
                   : default;
    }
}
