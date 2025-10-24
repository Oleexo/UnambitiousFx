using UnambitiousFx.Core.Results.Extensions.Transformations;

namespace UnambitiousFx.Core.Results.Extensions.Validations;

public static class ResultEnsureExtensions {
    public static Result<TValue> Ensure<TValue>(this Result<TValue>     result,
                                                Func<TValue, bool>      predicate,
                                                Func<TValue, Exception> errorFactory)
        where TValue : notnull {
        return result.Then(value => predicate(value)
                                        ? Result.Success(value)
                                        : Result.Failure<TValue>(errorFactory(value)));
    }

    public static Result<TValue1, TValue2> Ensure<TValue1, TValue2>(this Result<TValue1, TValue2>     result,
                                                                    Func<TValue1, TValue2, bool>      predicate,
                                                                    Func<TValue1, TValue2, Exception> errorFactory)
        where TValue1 : notnull
        where TValue2 : notnull {
        return result.Then((value1,
                            value2) => predicate(value1, value2)
                                           ? Result.Success(value1, value2)
                                           : Result.Failure<TValue1, TValue2>(errorFactory(value1, value2)));
    }

    public static Result<TValue1, TValue2, TValue3> Ensure<TValue1, TValue2, TValue3>(this Result<TValue1, TValue2, TValue3>     result,
                                                                                      Func<TValue1, TValue2, TValue3, bool>      predicate,
                                                                                      Func<TValue1, TValue2, TValue3, Exception> errorFactory)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        return result.Then((value1,
                            value2,
                            value3) => predicate(value1, value2, value3)
                                           ? Result.Success(value1, value2, value3)
                                           : Result.Failure<TValue1, TValue2, TValue3>(errorFactory(value1, value2, value3)));
    }

    public static Result<TValue1, TValue2, TValue3, TValue4> Ensure<TValue1, TValue2, TValue3, TValue4>(this Result<TValue1, TValue2, TValue3, TValue4>     result,
                                                                                                        Func<TValue1, TValue2, TValue3, TValue4, bool>      predicate,
                                                                                                        Func<TValue1, TValue2, TValue3, TValue4, Exception> errorFactory)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        return result.Then((value1,
                            value2,
                            value3,
                            value4) => predicate(value1, value2, value3, value4)
                                           ? Result.Success(value1, value2, value3, value4)
                                           : Result.Failure<TValue1, TValue2, TValue3, TValue4>(errorFactory(value1, value2, value3, value4)));
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5> Ensure<TValue1, TValue2, TValue3, TValue4, TValue5>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5>     result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, bool>      predicate,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, Exception> errorFactory)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        return result.Then((value1,
                            value2,
                            value3,
                            value4,
                            value5) => predicate(value1, value2, value3, value4, value5)
                                           ? Result.Success(value1, value2, value3, value4, value5)
                                           : Result.Failure<TValue1, TValue2, TValue3, TValue4, TValue5>(errorFactory(value1, value2, value3, value4, value5)));
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> Ensure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>     result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, bool>      predicate,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Exception> errorFactory)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull {
        return result.Then((value1,
                            value2,
                            value3,
                            value4,
                            value5,
                            value6) => predicate(value1, value2, value3, value4, value5, value6)
                                           ? Result.Success(value1, value2, value3, value4, value5, value6)
                                           : Result.Failure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(errorFactory(value1, value2, value3, value4, value5, value6)));
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> Ensure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>     result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, bool>      predicate,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Exception> errorFactory)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull {
        return result.Then((value1,
                            value2,
                            value3,
                            value4,
                            value5,
                            value6,
                            value7) => predicate(value1, value2, value3, value4, value5, value6, value7)
                                           ? Result.Success(value1, value2, value3, value4, value5, value6, value7)
                                           : Result.Failure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
                                               errorFactory(value1, value2, value3, value4, value5, value6, value7)));
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> Ensure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>     result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, bool>      predicate,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Exception> errorFactory)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull {
        return result.Then((value1,
                            value2,
                            value3,
                            value4,
                            value5,
                            value6,
                            value7,
                            value8) => predicate(value1, value2, value3, value4, value5, value6, value7, value8)
                                           ? Result.Success(
                                               value1, value2, value3, value4, value5, value6, value7, value8)
                                           : Result.Failure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
                                               errorFactory(value1, value2, value3, value4, value5, value6, value7, value8)));
    }
}
