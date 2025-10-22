namespace UnambitiousFx.Core.Results.Extensions.ErrorHandling;

public static partial class ResultExtensions {
    public static bool HasException<TException>(this Result result)
        where TException : Exception {
        return !result.Ok(out var err) && err is TException;
    }

    public static bool HasException<TException, TValue1>(this Result<TValue1> result)
        where TException : Exception
        where TValue1 : notnull {
        return !result.Ok(out Exception? err) && err is TException;
    }

    public static bool HasException<TException, TValue1, TValue2>(this Result<TValue1, TValue2> result)
        where TException : Exception
        where TValue1 : notnull
        where TValue2 : notnull {
        return !result.Ok(out Exception? err) && err is TException;
    }

    public static bool HasException<TException, TValue1, TValue2, TValue3>(this Result<TValue1, TValue2, TValue3> result)
        where TException : Exception
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        return !result.Ok(out Exception? err) && err is TException;
    }

    public static bool HasException<TException, TValue1, TValue2, TValue3, TValue4>(this Result<TValue1, TValue2, TValue3, TValue4> result)
        where TException : Exception
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        return !result.Ok(out Exception? err) && err is TException;
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
}
