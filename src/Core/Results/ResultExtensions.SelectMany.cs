namespace UnambitiousFx.Core.Results;

public static partial class ResultExtensions {
    public static Result<TResult> SelectMany<TValue1, TCollection, TResult>(this Result<TValue1>                source,
                                                                            Func<TValue1, Result<TCollection>>  collectionSelector,
                                                                            Func<TValue1, TCollection, TResult> resultSelector)
        where TValue1 : notnull
        where TCollection : notnull
        where TResult : notnull {
        return source.Bind(value1 => collectionSelector(value1)
                              .Map(c => resultSelector(value1, c)));
    }

    public static Result<TResult> SelectMany<TValue1, TValue2, TCollection, TResult>(this Result<TValue1, TValue2>                source,
                                                                                     Func<TValue1, TValue2, Result<TCollection>>  collectionSelector,
                                                                                     Func<TValue1, TValue2, TCollection, TResult> resultSelector)
        where TValue1 : notnull
        where TValue2 : notnull
        where TCollection : notnull
        where TResult : notnull {
        return source.Bind((value1,
                            value2) => collectionSelector(value1, value2)
                              .Map(c => resultSelector(value1, value2, c)));
    }

    public static Result<TResult> SelectMany<TValue1, TValue2, TValue3, TCollection, TResult>(this Result<TValue1, TValue2, TValue3>                source,
                                                                                              Func<TValue1, TValue2, TValue3, Result<TCollection>>  collectionSelector,
                                                                                              Func<TValue1, TValue2, TValue3, TCollection, TResult> resultSelector)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TCollection : notnull
        where TResult : notnull {
        return source.Bind((value1,
                            value2,
                            value3) => collectionSelector(value1, value2, value3)
                              .Map(c => resultSelector(value1, value2, value3, c)));
    }

    public static Result<TResult> SelectMany<TValue1, TValue2, TValue3, TValue4, TCollection, TResult>(this Result<TValue1, TValue2, TValue3, TValue4> source,
                                                                                                       Func<TValue1, TValue2, TValue3, TValue4, Result<TCollection>>
                                                                                                           collectionSelector,
                                                                                                       Func<TValue1, TValue2, TValue3, TValue4, TCollection, TResult>
                                                                                                           resultSelector)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TCollection : notnull
        where TResult : notnull {
        return source.Bind((value1,
                            value2,
                            value3,
                            value4) => collectionSelector(value1, value2, value3, value4)
                              .Map(c => resultSelector(value1, value2, value3, value4, c)));
    }

    public static Result<TResult> SelectMany<TValue1, TValue2, TValue3, TValue4, TValue5, TCollection, TResult>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5> source,
                                                                                                                Func<TValue1, TValue2, TValue3, TValue4, TValue5,
                                                                                                                    Result<TCollection>> collectionSelector,
                                                                                                                Func<TValue1, TValue2, TValue3, TValue4, TValue5, TCollection,
                                                                                                                    TResult> resultSelector)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TCollection : notnull
        where TResult : notnull {
        return source.Bind((value1,
                            value2,
                            value3,
                            value4,
                            value5) => collectionSelector(value1, value2, value3, value4, value5)
                              .Map(c => resultSelector(value1, value2, value3, value4, value5, c)));
    }

    public static Result<TResult> SelectMany<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TCollection, TResult>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>                source,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Result<TCollection>>  collectionSelector,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TCollection, TResult> resultSelector)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TCollection : notnull
        where TResult : notnull {
        return source.Bind((value1,
                            value2,
                            value3,
                            value4,
                            value5,
                            value6) => collectionSelector(value1, value2, value3, value4, value5, value6)
                              .Map(c => resultSelector(value1, value2, value3, value4, value5, value6, c)));
    }

    public static Result<TResult> SelectMany<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TCollection, TResult>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>                source,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Result<TCollection>>  collectionSelector,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TCollection, TResult> resultSelector)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TCollection : notnull
        where TResult : notnull {
        return source.Bind((value1,
                            value2,
                            value3,
                            value4,
                            value5,
                            value6,
                            value7) => collectionSelector(value1, value2, value3, value4, value5, value6, value7)
                              .Map(c => resultSelector(value1, value2, value3, value4, value5, value6, value7, c)));
    }

    public static Result<TResult> SelectMany<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TCollection, TResult>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>                source,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Result<TCollection>>  collectionSelector,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TCollection, TResult> resultSelector)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull
        where TCollection : notnull
        where TResult : notnull {
        return source.Bind((value1,
                            value2,
                            value3,
                            value4,
                            value5,
                            value6,
                            value7,
                            value8) => collectionSelector(value1, value2, value3, value4, value5, value6, value7, value8)
                              .Map(c => resultSelector(value1, value2, value3, value4, value5, value6, value7, value8, c)));
    }
}
