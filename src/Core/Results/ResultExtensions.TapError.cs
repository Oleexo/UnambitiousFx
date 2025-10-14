namespace UnambitiousFx.Core.Results;

public static partial class ResultExtensions {
    public static Result<TValue> TapError<TValue>(this Result<TValue> result,
                                                  Action<Exception>   tap)
        where TValue : notnull {
        return result.Match(
            value => Result.Success<TValue>(value),
            ex => {
                tap(ex);
                return Result.Failure<TValue>(ex);
            }
        );
    }

    public static Result<TValue1, TValue2> TapError<TValue1, TValue2>(this Result<TValue1, TValue2> result,
                                                                      Action<Exception>             tap)
        where TValue1 : notnull
        where TValue2 : notnull {
        return result.Match(
            (value1,
             value2) => Result.Success<TValue1, TValue2>(value1, value2),
            ex => {
                tap(ex);
                return Result.Failure<TValue1, TValue2>(ex);
            }
        );
    }

    public static Result<TValue1, TValue2, TValue3> TapError<TValue1, TValue2, TValue3>(this Result<TValue1, TValue2, TValue3> result,
                                                                                        Action<Exception>                      tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        return result.Match(
            (value1,
             value2,
             value3) => Result.Success<TValue1, TValue2, TValue3>(value1, value2, value3),
            ex => {
                tap(ex);
                return Result.Failure<TValue1, TValue2, TValue3>(ex);
            }
        );
    }

    public static Result<TValue1, TValue2, TValue3, TValue4> TapError<TValue1, TValue2, TValue3, TValue4>(this Result<TValue1, TValue2, TValue3, TValue4> result,
                                                                                                          Action<Exception>                               tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        return result.Match(
            (value1,
             value2,
             value3,
             value4) => Result.Success<TValue1, TValue2, TValue3, TValue4>(value1, value2, value3, value4),
            ex => {
                tap(ex);
                return Result.Failure<TValue1, TValue2, TValue3, TValue4>(ex);
            }
        );
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5> TapError<TValue1, TValue2, TValue3, TValue4, TValue5>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5> result,
        Action<Exception>                                        tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        return result.Match(
            (value1,
             value2,
             value3,
             value4,
             value5) => Result.Success<TValue1, TValue2, TValue3, TValue4, TValue5>(value1, value2, value3, value4, value5),
            ex => {
                tap(ex);
                return Result.Failure<TValue1, TValue2, TValue3, TValue4, TValue5>(ex);
            }
        );
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> TapError<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> result,
        Action<Exception>                                                 tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull {
        return result.Match(
            (value1,
             value2,
             value3,
             value4,
             value5,
             value6) => Result.Success<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(value1, value2, value3, value4, value5, value6),
            ex => {
                tap(ex);
                return Result.Failure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(ex);
            }
        );
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> TapError<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> result,
        Action<Exception>                                                          tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull {
        return result.Match(
            (value1,
             value2,
             value3,
             value4,
             value5,
             value6,
             value7) => Result.Success<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(value1, value2, value3, value4, value5, value6, value7),
            ex => {
                tap(ex);
                return Result.Failure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(ex);
            }
        );
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> TapError<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> result,
        Action<Exception>                                                                   tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull {
        return result.Match(
            (value1,
             value2,
             value3,
             value4,
             value5,
             value6,
             value7,
             value8) => Result.Success<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(value1, value2, value3, value4, value5, value6, value7, value8),
            ex => {
                tap(ex);
                return Result.Failure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(ex);
            }
        );
    }
}
