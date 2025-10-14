namespace UnambitiousFx.Core.Results;

public static partial class ResultExtensions {
    public static Result<TOut> Select<TValue1, TOut>(this Result<TValue1> source,
                                                     Func<TValue1, TOut>  selector)
        where TValue1 : notnull
        where TOut : notnull {
        return source.Bind(value1 => Result.Success(selector(value1)));
    }

    public static Result<TOut> Select<TValue1, TValue2, TOut>(this Result<TValue1, TValue2> source,
                                                              Func<TValue1, TValue2, TOut>  selector)
        where TValue1 : notnull
        where TValue2 : notnull
        where TOut : notnull {
        return source.Bind((value1,
                            value2) => Result.Success(selector(value1, value2)));
    }

    public static Result<TOut> Select<TValue1, TValue2, TValue3, TOut>(this Result<TValue1, TValue2, TValue3> source,
                                                                       Func<TValue1, TValue2, TValue3, TOut>  selector)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TOut : notnull {
        return source.Bind((value1,
                            value2,
                            value3) => Result.Success(selector(value1, value2, value3)));
    }

    public static Result<TOut> Select<TValue1, TValue2, TValue3, TValue4, TOut>(this Result<TValue1, TValue2, TValue3, TValue4> source,
                                                                                Func<TValue1, TValue2, TValue3, TValue4, TOut>  selector)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TOut : notnull {
        return source.Bind((value1,
                            value2,
                            value3,
                            value4) => Result.Success(selector(value1, value2, value3, value4)));
    }

    public static Result<TOut> Select<TValue1, TValue2, TValue3, TValue4, TValue5, TOut>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5> source,
                                                                                         Func<TValue1, TValue2, TValue3, TValue4, TValue5, TOut>  selector)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TOut : notnull {
        return source.Bind((value1,
                            value2,
                            value3,
                            value4,
                            value5) => Result.Success(selector(value1, value2, value3, value4, value5)));
    }

    public static Result<TOut> Select<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TOut>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> source,
                                                                                                  Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TOut>  selector)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TOut : notnull {
        return source.Bind((value1,
                            value2,
                            value3,
                            value4,
                            value5,
                            value6) => Result.Success(selector(value1, value2, value3, value4, value5, value6)));
    }

    public static Result<TOut> Select<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TOut>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> source,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TOut>  selector)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TOut : notnull {
        return source.Bind((value1,
                            value2,
                            value3,
                            value4,
                            value5,
                            value6,
                            value7) => Result.Success(selector(value1, value2, value3, value4, value5, value6, value7)));
    }

    public static Result<TOut> Select<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TOut>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> source,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TOut>  selector)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull
        where TOut : notnull {
        return source.Bind((value1,
                            value2,
                            value3,
                            value4,
                            value5,
                            value6,
                            value7,
                            value8) => Result.Success(selector(value1, value2, value3, value4, value5, value6, value7, value8)));
    }
}
