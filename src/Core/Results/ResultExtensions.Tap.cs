namespace UnambitiousFx.Core.Results;

public static partial class ResultExtensions {
    public static Result<TValue> Tap<TValue>(this Result<TValue> result,
                                             Action<TValue>      tap)
        where TValue : notnull {
        return result.Bind(value => {
            tap(value);
            return Result.Success<TValue>(value);
        });
    }

    public static Result<TValue1, TValue2> Tap<TValue1, TValue2>(this Result<TValue1, TValue2> result,
                                                                 Action<TValue1, TValue2>      tap)
        where TValue1 : notnull
        where TValue2 : notnull {
        return result.Bind((value1,
                            value2) => {
            tap(value1, value2);
            return Result.Success<TValue1, TValue2>(value1, value2);
        });
    }

    public static Result<TValue1, TValue2, TValue3> Tap<TValue1, TValue2, TValue3>(this Result<TValue1, TValue2, TValue3> result,
                                                                                   Action<TValue1, TValue2, TValue3>      tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        return result.Bind((value1,
                            value2,
                            value3) => {
            tap(value1, value2, value3);
            return Result.Success<TValue1, TValue2, TValue3>(value1, value2, value3);
        });
    }

    public static Result<TValue1, TValue2, TValue3, TValue4> Tap<TValue1, TValue2, TValue3, TValue4>(this Result<TValue1, TValue2, TValue3, TValue4> result,
                                                                                                     Action<TValue1, TValue2, TValue3, TValue4>      tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        return result.Bind((value1,
                            value2,
                            value3,
                            value4) => {
            tap(value1, value2, value3, value4);
            return Result.Success<TValue1, TValue2, TValue3, TValue4>(value1, value2, value3, value4);
        });
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5> Tap<TValue1, TValue2, TValue3, TValue4, TValue5>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5> result,
        Action<TValue1, TValue2, TValue3, TValue4, TValue5>      tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        return result.Bind((value1,
                            value2,
                            value3,
                            value4,
                            value5) => {
            tap(value1, value2, value3, value4, value5);
            return Result.Success<TValue1, TValue2, TValue3, TValue4, TValue5>(value1, value2, value3, value4, value5);
        });
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> Tap<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> result,
        Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>      tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull {
        return result.Bind((value1,
                            value2,
                            value3,
                            value4,
                            value5,
                            value6) => {
            tap(value1, value2, value3, value4, value5, value6);
            return Result.Success<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(value1, value2, value3, value4, value5, value6);
        });
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> Tap<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> result,
        Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>      tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull {
        return result.Bind((value1,
                            value2,
                            value3,
                            value4,
                            value5,
                            value6,
                            value7) => {
            tap(value1, value2, value3, value4, value5, value6, value7);
            return Result.Success<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(value1, value2, value3, value4, value5, value6, value7);
        });
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> Tap<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> result,
        Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>      tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull {
        return result.Bind((value1,
                            value2,
                            value3,
                            value4,
                            value5,
                            value6,
                            value7,
                            value8) => {
            tap(value1, value2, value3, value4, value5, value6, value7, value8);
            return Result.Success<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(value1, value2, value3, value4, value5, value6, value7, value8);
        });
    }
}
