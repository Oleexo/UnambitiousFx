namespace UnambitiousFx.Core.Results;

public static partial class ResultExtensions {
    public static Result MapError(this Result                result,
                                  Func<Exception, Exception> mapError) {
        return result.Match(
            Result.Success,
            ex => Result.Failure(mapError(ex))
        );
    }
    public static Result<TValue> MapError<TValue>(this Result<TValue>        result,
                                                  Func<Exception, Exception> mapError)
        where TValue : notnull {
        return result.Match(
            Result.Success,
            ex => Result.Failure<TValue>(mapError(ex))
        );
    }

    public static Result<TValue1, TValue2> MapError<TValue1, TValue2>(this Result<TValue1, TValue2> result,
                                                                      Func<Exception, Exception>    mapError)
        where TValue1 : notnull
        where TValue2 : notnull {
        return result.Match(
            Result.Success,
            ex => Result.Failure<TValue1, TValue2>(mapError(ex))
        );
    }

    public static Result<TValue1, TValue2, TValue3> MapError<TValue1, TValue2, TValue3>(this Result<TValue1, TValue2, TValue3> result,
                                                                                        Func<Exception, Exception>             mapError)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        return result.Match(
            Result.Success,
            ex => Result.Failure<TValue1, TValue2, TValue3>(mapError(ex))
        );
    }

    public static Result<TValue1, TValue2, TValue3, TValue4> MapError<TValue1, TValue2, TValue3, TValue4>(this Result<TValue1, TValue2, TValue3, TValue4> result,
                                                                                                          Func<Exception, Exception>                      mapError)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        return result.Match(
            Result.Success,
            ex => Result.Failure<TValue1, TValue2, TValue3, TValue4>(mapError(ex))
        );
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5> MapError<TValue1, TValue2, TValue3, TValue4, TValue5>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5> result,
        Func<Exception, Exception>                               mapError)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        return result.Match(
            Result.Success,
            ex => Result.Failure<TValue1, TValue2, TValue3, TValue4, TValue5>(mapError(ex))
        );
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> MapError<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> result,
        Func<Exception, Exception>                                        mapError)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull {
        return result.Match(
            Result.Success,
            ex => Result.Failure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(mapError(ex))
        );
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> MapError<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> result,
        Func<Exception, Exception>                                                 mapError)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull {
        return result.Match(
            Result.Success,
            ex => Result.Failure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(mapError(ex))
        );
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> MapError<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> result,
        Func<Exception, Exception>                                                          mapError)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull {
        return result.Match(
            Result.Success,
            ex => Result.Failure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(mapError(ex))
        );
    }
}
