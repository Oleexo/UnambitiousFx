namespace UnambitiousFx.Core.Results.ValueTasks;

public static partial class ResultExtensions {
    public static ValueTask<Result<TOut>> TryAsync<TValue, TOut>(this Result<TValue>           result,
                                                                 Func<TValue, ValueTask<TOut>> func)
        where TValue : notnull
        where TOut : notnull {
        return result.BindAsync(async value => {
            try {
                var newValue = await func(value);
                return Result.Success(newValue);
            }
            catch (Exception ex) {
                return Result.Failure<TOut>(ex);
            }
        });
    }

    public static ValueTask<Result<TOut>> TryAsync<TValue, TOut>(this ValueTask<Result<TValue>> awaitableResult,
                                                                 Func<TValue, ValueTask<TOut>>  func)
        where TValue : notnull
        where TOut : notnull {
        return awaitableResult.BindAsync(async value => {
            try {
                var newValue = await func(value);
                return Result.Success(newValue);
            }
            catch (Exception ex) {
                return Result.Failure<TOut>(ex);
            }
        });
    }

    public static ValueTask<Result<TOut1, TOut2>> TryAsync<TValue1, TValue2, TOut1, TOut2>(this Result<TValue1, TValue2>                     result,
                                                                                           Func<TValue1, TValue2, ValueTask<(TOut1, TOut2)>> func)
        where TValue1 : notnull
        where TValue2 : notnull
        where TOut1 : notnull
        where TOut2 : notnull {
        return result.BindAsync(async (value1,
                                       value2) => {
            try {
                var items = await func(value1, value2);
                return Result.Success(items.Item1, items.Item2);
            }
            catch (Exception ex) {
                return Result.Failure<TOut1, TOut2>(ex);
            }
        });
    }

    public static ValueTask<Result<TOut1, TOut2>> TryAsync<TValue1, TValue2, TOut1, TOut2>(this ValueTask<Result<TValue1, TValue2>>          awaitableResult,
                                                                                           Func<TValue1, TValue2, ValueTask<(TOut1, TOut2)>> func)
        where TValue1 : notnull
        where TValue2 : notnull
        where TOut1 : notnull
        where TOut2 : notnull {
        return awaitableResult.BindAsync(async (value1,
                                                value2) => {
            try {
                var items = await func(value1, value2);
                return Result.Success(items.Item1, items.Item2);
            }
            catch (Exception ex) {
                return Result.Failure<TOut1, TOut2>(ex);
            }
        });
    }

    public static ValueTask<Result<TOut1, TOut2, TOut3>> TryAsync<TValue1, TValue2, TValue3, TOut1, TOut2, TOut3>(this Result<TValue1, TValue2, TValue3> result,
                                                                                                                  Func<TValue1, TValue2, TValue3, ValueTask<(TOut1, TOut2, TOut3)>>
                                                                                                                      func)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull {
        return result.BindAsync(async (value1,
                                       value2,
                                       value3) => {
            try {
                var items = await func(value1, value2, value3);
                return Result.Success(items.Item1, items.Item2, items.Item3);
            }
            catch (Exception ex) {
                return Result.Failure<TOut1, TOut2, TOut3>(ex);
            }
        });
    }

    public static ValueTask<Result<TOut1, TOut2, TOut3>> TryAsync<TValue1, TValue2, TValue3, TOut1, TOut2, TOut3>(this ValueTask<Result<TValue1, TValue2, TValue3>> awaitableResult,
                                                                                                                  Func<TValue1, TValue2, TValue3, ValueTask<(TOut1, TOut2, TOut3)>>
                                                                                                                      func)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull {
        return awaitableResult.BindAsync(async (value1,
                                                value2,
                                                value3) => {
            try {
                var items = await func(value1, value2, value3);
                return Result.Success(items.Item1, items.Item2, items.Item3);
            }
            catch (Exception ex) {
                return Result.Failure<TOut1, TOut2, TOut3>(ex);
            }
        });
    }

    public static ValueTask<Result<TOut1, TOut2, TOut3, TOut4>> TryAsync<TValue1, TValue2, TValue3, TValue4, TOut1, TOut2, TOut3, TOut4>(
        this Result<TValue1, TValue2, TValue3, TValue4>                                   result,
        Func<TValue1, TValue2, TValue3, TValue4, ValueTask<(TOut1, TOut2, TOut3, TOut4)>> func)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull {
        return result.BindAsync(async (value1,
                                       value2,
                                       value3,
                                       value4) => {
            try {
                var items = await func(value1, value2, value3, value4);
                return Result.Success(items.Item1, items.Item2, items.Item3, items.Item4);
            }
            catch (Exception ex) {
                return Result.Failure<TOut1, TOut2, TOut3, TOut4>(ex);
            }
        });
    }

    public static ValueTask<Result<TOut1, TOut2, TOut3, TOut4>> TryAsync<TValue1, TValue2, TValue3, TValue4, TOut1, TOut2, TOut3, TOut4>(
        this ValueTask<Result<TValue1, TValue2, TValue3, TValue4>>                        awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, ValueTask<(TOut1, TOut2, TOut3, TOut4)>> func)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull {
        return awaitableResult.BindAsync(async (value1,
                                                value2,
                                                value3,
                                                value4) => {
            try {
                var items = await func(value1, value2, value3, value4);
                return Result.Success(items.Item1, items.Item2, items.Item3, items.Item4);
            }
            catch (Exception ex) {
                return Result.Failure<TOut1, TOut2, TOut3, TOut4>(ex);
            }
        });
    }

    public static ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5>> TryAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1, TOut2, TOut3, TOut4, TOut5>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5>                                          result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, ValueTask<(TOut1, TOut2, TOut3, TOut4, TOut5)>> func)
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
        return result.BindAsync(async (value1,
                                       value2,
                                       value3,
                                       value4,
                                       value5) => {
            try {
                var items = await func(value1, value2, value3, value4, value5);
                return Result.Success(items.Item1, items.Item2, items.Item3, items.Item4, items.Item5);
            }
            catch (Exception ex) {
                return Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5>(ex);
            }
        });
    }

    public static ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5>> TryAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1, TOut2, TOut3, TOut4, TOut5>(
        this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5>>                               awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, ValueTask<(TOut1, TOut2, TOut3, TOut4, TOut5)>> func)
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
        return awaitableResult.BindAsync(async (value1,
                                                value2,
                                                value3,
                                                value4,
                                                value5) => {
            try {
                var items = await func(value1, value2, value3, value4, value5);
                return Result.Success(items.Item1, items.Item2, items.Item3, items.Item4, items.Item5);
            }
            catch (Exception ex) {
                return Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5>(ex);
            }
        });
    }

    public static ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>> TryAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TOut1, TOut2, TOut3, TOut4, TOut5,
                                                                                       TOut6>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> result,
                                                                                              Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6,
                                                                                                  ValueTask<(TOut1, TOut2, TOut3, TOut4, TOut5, TOut6)>> func)
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
        return result.BindAsync(async (value1,
                                       value2,
                                       value3,
                                       value4,
                                       value5,
                                       value6) => {
            try {
                var items = await func(value1, value2, value3, value4, value5, value6);
                return Result.Success(items.Item1, items.Item2, items.Item3, items.Item4, items.Item5, items.Item6);
            }
            catch (Exception ex) {
                return Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(ex);
            }
        });
    }

    public static ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>> TryAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TOut1, TOut2, TOut3, TOut4, TOut5,
                                                                                       TOut6>(
        this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>>                                      awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, ValueTask<(TOut1, TOut2, TOut3, TOut4, TOut5, TOut6)>> func)
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
        return awaitableResult.BindAsync(async (value1,
                                                value2,
                                                value3,
                                                value4,
                                                value5,
                                                value6) => {
            try {
                var items = await func(value1, value2, value3, value4, value5, value6);
                return Result.Success(items.Item1, items.Item2, items.Item3, items.Item4, items.Item5, items.Item6);
            }
            catch (Exception ex) {
                return Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(ex);
            }
        });
    }

    public static ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>> TryAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TOut1, TOut2, TOut3,
                                                                                              TOut4, TOut5, TOut6, TOut7>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>                                                        result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, ValueTask<(TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7)>> func)
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
        return result.BindAsync(async (value1,
                                       value2,
                                       value3,
                                       value4,
                                       value5,
                                       value6,
                                       value7) => {
            try {
                var items = await func(value1, value2, value3, value4, value5, value6, value7);
                return Result.Success(items.Item1, items.Item2, items.Item3, items.Item4, items.Item5, items.Item6, items.Item7);
            }
            catch (Exception ex) {
                return Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(ex);
            }
        });
    }

    public static ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>> TryAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TOut1, TOut2, TOut3,
                                                                                              TOut4, TOut5, TOut6, TOut7>(
        this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>>                                             awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, ValueTask<(TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7)>> func)
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
        return awaitableResult.BindAsync(async (value1,
                                                value2,
                                                value3,
                                                value4,
                                                value5,
                                                value6,
                                                value7) => {
            try {
                var items = await func(value1, value2, value3, value4, value5, value6, value7);
                return Result.Success(items.Item1, items.Item2, items.Item3, items.Item4, items.Item5, items.Item6, items.Item7);
            }
            catch (Exception ex) {
                return Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(ex);
            }
        });
    }

    public static ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>> TryAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TOut1,
                                                                                                     TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>                                                               result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, ValueTask<(TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8)>> func)
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
        return result.BindAsync(async (value1,
                                       value2,
                                       value3,
                                       value4,
                                       value5,
                                       value6,
                                       value7,
                                       value8) => {
            try {
                var items = await func(value1, value2, value3, value4, value5, value6, value7, value8);
                return Result.Success(items.Item1, items.Item2, items.Item3, items.Item4, items.Item5, items.Item6, items.Item7, items.Item8);
            }
            catch (Exception ex) {
                return Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(ex);
            }
        });
    }

    public static ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>> TryAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TOut1,
                                                                                                     TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(
        this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>>                                                    awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, ValueTask<(TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8)>> func)
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
        return awaitableResult.BindAsync(async (value1,
                                                value2,
                                                value3,
                                                value4,
                                                value5,
                                                value6,
                                                value7,
                                                value8) => {
            try {
                var items = await func(value1, value2, value3, value4, value5, value6, value7, value8);
                return Result.Success(items.Item1, items.Item2, items.Item3, items.Item4, items.Item5, items.Item6, items.Item7, items.Item8);
            }
            catch (Exception ex) {
                return Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(ex);
            }
        });
    }
}
