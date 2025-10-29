namespace UnambitiousFx.Core.Results.Extensions.ValueAccess;

public static class ResultToNullableExtensions {
    public static TValue1? ToNullable<TValue1>(this Result<TValue1> result)
        where TValue1 : notnull {
        return result.TryGet(out var value)
                   ? (TValue1?)value
                   : default;
    }

    public static (TValue1, TValue2)? ToNullable<TValue1, TValue2>(this Result<TValue1, TValue2> result)
        where TValue1 : notnull
        where TValue2 : notnull {
        if (!result.IsSuccess) {
            return null;
        }

        return result.TryGet(out var value1, out var value2)
                   ? (value1, value2)
                   : default;
    }

    public static (TValue1, TValue2, TValue3)? ToNullable<TValue1, TValue2, TValue3>(this Result<TValue1, TValue2, TValue3> result)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        if (!result.IsSuccess) {
            return null;
        }

        return result.TryGet(out var value1, out var value2, out var value3)
                   ? (value1, value2, value3)
                   : default;
    }

    public static (TValue1, TValue2, TValue3, TValue4)? ToNullable<TValue1, TValue2, TValue3, TValue4>(this Result<TValue1, TValue2, TValue3, TValue4> result)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        if (!result.IsSuccess) {
            return null;
        }

        return result.TryGet(out var value1, out var value2, out var value3, out var value4)
                   ? (value1, value2, value3, value4)
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
            return null;
        }

        return result.TryGet(out var value1, out var value2, out var value3, out var value4, out var value5)
                   ? (value1, value2, value3, value4, value5)
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
            return null;
        }

        return result.TryGet(out var value1, out var value2, out var value3, out var value4, out var value5, out var value6)
                   ? (value1, value2, value3, value4, value5, value6)
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
            return null;
        }

        return result.TryGet(out var value1, out var value2, out var value3, out var value4, out var value5, out var value6, out var value7)
                   ? (value1, value2, value3, value4, value5, value6, value7)
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
            return null;
        }

        return result.TryGet(out var value1, out var value2, out var value3, out var value4, out var value5, out var value6, out var value7, out var value8)
                   ? (value1, value2, value3, value4, value5, value6, value7, value8)
                   : default;
    }
}
