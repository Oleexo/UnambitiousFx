namespace UnambitiousFx.Core.Results;

public static partial class ResultExtensions {
    public static Result TapError(this Result       result,
                                  Action<Exception> tap) {
        return result.Match(
            Result.Success,
            ex => {
                tap(ex);
                return Result.Failure(ex);
            }
        );
    }
    
    public static Result<TValue> TapError<TValue>(this Result<TValue> result,
                                                  Action<Exception>   tap)
        where TValue : notnull {
        return result.Match(
            Result.Success,
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
            Result.Success,
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
            Result.Success,
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
            Result.Success,
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
            Result.Success,
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
            Result.Success,
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
            Result.Success,
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
            Result.Success,
            ex => {
                tap(ex);
                return Result.Failure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(ex);
            }
        );
    }
}
