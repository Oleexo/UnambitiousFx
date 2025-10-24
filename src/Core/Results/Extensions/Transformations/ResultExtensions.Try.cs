namespace UnambitiousFx.Core.Results.Extensions.Transformations;

public static class ResultTryExtensions {
    public static Result<TOut> Try<TValue, TOut>(this Result<TValue> result,
                                                 Func<TValue, TOut>  func)
        where TValue : notnull
        where TOut : notnull {
        return result.Bind(value => {
            try {
                var newValue = func(value);
                return Result.Success(newValue);
            }
            catch (Exception ex) {
                return Result.Failure<TOut>(ex);
            }
        });
    }

    public static Result<TOut1, TOut2> Try<TValue1, TValue2, TOut1, TOut2>(this Result<TValue1, TValue2>          result,
                                                                           Func<TValue1, TValue2, (TOut1, TOut2)> func)
        where TValue1 : notnull
        where TValue2 : notnull
        where TOut1 : notnull
        where TOut2 : notnull {
        return result.Bind((value1,
                            value2) => {
            try {
                var items = func(value1, value2);
                return Result.Success(items.Item1, items.Item2);
            }
            catch (Exception ex) {
                return Result.Failure<TOut1, TOut2>(ex);
            }
        });
    }

    public static Result<TOut1, TOut2, TOut3> Try<TValue1, TValue2, TValue3, TOut1, TOut2, TOut3>(this Result<TValue1, TValue2, TValue3>                 result,
                                                                                                  Func<TValue1, TValue2, TValue3, (TOut1, TOut2, TOut3)> func)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull {
        return result.Bind((value1,
                            value2,
                            value3) => {
            try {
                var items = func(value1, value2, value3);
                return Result.Success(items.Item1, items.Item2, items.Item3);
            }
            catch (Exception ex) {
                return Result.Failure<TOut1, TOut2, TOut3>(ex);
            }
        });
    }

    public static Result<TOut1, TOut2, TOut3, TOut4> Try<TValue1, TValue2, TValue3, TValue4, TOut1, TOut2, TOut3, TOut4>(this Result<TValue1, TValue2, TValue3, TValue4> result,
        Func<TValue1, TValue2, TValue3, TValue4, (TOut1, TOut2, TOut3, TOut4)>                                                                                           func)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull {
        return result.Bind((value1,
                            value2,
                            value3,
                            value4) => {
            try {
                var items = func(value1, value2, value3, value4);
                return Result.Success(items.Item1, items.Item2, items.Item3, items.Item4);
            }
            catch (Exception ex) {
                return Result.Failure<TOut1, TOut2, TOut3, TOut4>(ex);
            }
        });
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5> Try<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1, TOut2, TOut3, TOut4, TOut5>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5>                               result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, (TOut1, TOut2, TOut3, TOut4, TOut5)> func)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull {
        return result.Bind((value1,
                            value2,
                            value3,
                            value4,
                            value5) => {
            try {
                var items = func(value1, value2, value3, value4, value5);
                return Result.Success(items.Item1, items.Item2, items.Item3, items.Item4, items.Item5);
            }
            catch (Exception ex) {
                return Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5>(ex);
            }
        });
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6> Try<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>                                      result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, (TOut1, TOut2, TOut3, TOut4, TOut5, TOut6)> func)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull {
        return result.Bind((value1,
                            value2,
                            value3,
                            value4,
                            value5,
                            value6) => {
            try {
                var items = func(value1, value2, value3, value4, value5, value6);
                return Result.Success(items.Item1, items.Item2, items.Item3, items.Item4, items.Item5, items.Item6);
            }
            catch (Exception ex) {
                return Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(ex);
            }
        });
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7> Try<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TOut1, TOut2, TOut3, TOut4, TOut5,
                                                                              TOut6, TOut7>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> result,
                                                                                            Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, (TOut1, TOut2, TOut3
                                                                                              , TOut4, TOut5, TOut6, TOut7)> func)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull
        where TOut7 : notnull {
        return result.Bind((value1,
                            value2,
                            value3,
                            value4,
                            value5,
                            value6,
                            value7) => {
            try {
                var items = func(value1, value2, value3, value4, value5, value6, value7);
                return Result.Success(items.Item1, items.Item2, items.Item3, items.Item4, items.Item5, items.Item6, items.Item7);
            }
            catch (Exception ex) {
                return Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(ex);
            }
        });
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8> Try<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TOut1, TOut2, TOut3,
                                                                                     TOut4, TOut5, TOut6, TOut7, TOut8>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>                                                    result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, (TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8)> func)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull
        where TOut7 : notnull
        where TOut8 : notnull {
        return result.Bind((value1,
                            value2,
                            value3,
                            value4,
                            value5,
                            value6,
                            value7,
                            value8) => {
            try {
                var items = func(value1, value2, value3, value4, value5, value6, value7, value8);
                return Result.Success(items.Item1, items.Item2, items.Item3, items.Item4, items.Item5, items.Item6, items.Item7, items.Item8);
            }
            catch (Exception ex) {
                return Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(ex);
            }
        });
    }
}
