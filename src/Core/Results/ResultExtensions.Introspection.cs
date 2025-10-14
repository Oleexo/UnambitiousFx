namespace UnambitiousFx.Core.Results;

public static partial class ResultExtensions {
    public static bool HasException<TException>(this Result result)
        where TException : Exception {
        return !result.Ok(out var err) && err is TException;
    }

    public static bool HasError<TError>(this Result result) {
        return result.Reasons.OfType<TError>()
                     .Any() ||
               (!result.Ok(out var err) && err is TError);
    }

    public static bool HasException<TException, TValue1>(this Result<TValue1> result)
        where TException : Exception
        where TValue1 : notnull {
        return !result.Ok(out Exception? err) && err is TException;
    }

    public static bool HasError<TError, TValue1>(this Result<TValue1> result)
        where TValue1 : notnull {
        return result.Reasons.OfType<TError>()
                     .Any() ||
               (!result.Ok(out Exception? err) && err is TError);
    }

    public static TValue1? ToNullable<TValue1>(this Result<TValue1> result)
        where TValue1 : notnull {
        return result.Ok(out var value)
                   ? value
                   : default;
    }

    public static bool HasException<TException, TValue1, TValue2>(this Result<TValue1, TValue2> result)
        where TException : Exception
        where TValue1 : notnull
        where TValue2 : notnull {
        return !result.Ok(out Exception? err) && err is TException;
    }

    public static bool HasError<TError, TValue1, TValue2>(this Result<TValue1, TValue2> result)
        where TValue1 : notnull
        where TValue2 : notnull {
        return result.Reasons.OfType<TError>()
                     .Any() ||
               (!result.Ok(out Exception? err) && err is TError);
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

    public static bool HasException<TException, TValue1, TValue2, TValue3>(this Result<TValue1, TValue2, TValue3> result)
        where TException : Exception
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        return !result.Ok(out Exception? err) && err is TException;
    }

    public static bool HasError<TError, TValue1, TValue2, TValue3>(this Result<TValue1, TValue2, TValue3> result)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        return result.Reasons.OfType<TError>()
                     .Any() ||
               (!result.Ok(out Exception? err) && err is TError);
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

    public static bool HasException<TException, TValue1, TValue2, TValue3, TValue4>(this Result<TValue1, TValue2, TValue3, TValue4> result)
        where TException : Exception
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        return !result.Ok(out Exception? err) && err is TException;
    }

    public static bool HasError<TError, TValue1, TValue2, TValue3, TValue4>(this Result<TValue1, TValue2, TValue3, TValue4> result)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        return result.Reasons.OfType<TError>()
                     .Any() ||
               (!result.Ok(out Exception? err) && err is TError);
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

    public static bool HasException<TException, TValue1, TValue2, TValue3, TValue4, TValue5>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5> result)
        where TException : Exception
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        return !result.Ok(out Exception? err) && err is TException;
    }

    public static bool HasError<TError, TValue1, TValue2, TValue3, TValue4, TValue5>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5> result)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        return result.Reasons.OfType<TError>()
                     .Any() ||
               (!result.Ok(out Exception? err) && err is TError);
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

    public static bool HasException<TException, TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> result)
        where TException : Exception
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull {
        return !result.Ok(out Exception? err) && err is TException;
    }

    public static bool HasError<TError, TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> result)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull {
        return result.Reasons.OfType<TError>()
                     .Any() ||
               (!result.Ok(out Exception? err) && err is TError);
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

    public static bool HasException<TException, TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> result)
        where TException : Exception
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull {
        return !result.Ok(out Exception? err) && err is TException;
    }

    public static bool HasError<TError, TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> result)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull {
        return result.Reasons.OfType<TError>()
                     .Any() ||
               (!result.Ok(out Exception? err) && err is TError);
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

    public static bool HasException<TException, TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> result)
        where TException : Exception
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull {
        return !result.Ok(out Exception? err) && err is TException;
    }

    public static bool HasError<TError, TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> result)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull {
        return result.Reasons.OfType<TError>()
                     .Any() ||
               (!result.Ok(out Exception? err) && err is TError);
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
