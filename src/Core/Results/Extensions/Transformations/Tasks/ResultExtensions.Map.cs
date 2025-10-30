namespace UnambitiousFx.Core.Results.Extensions.Transformations.Tasks;

public static partial class ResultMapExtensions
{
    public static Task<Result<TOut>> MapAsync<TValue, TOut>(this Result<TValue> result,
                                                            Func<TValue, Task<TOut>> map)
        where TValue : notnull
        where TOut : notnull
    {
        return result.BindAsync(async value =>
        {
            var newValue = await map(value);
            return Result.Success(newValue);
        });
    }

    public static Task<Result<TOut>> MapAsync<TValue, TOut>(this Task<Result<TValue>> awaitableResult,
                                                            Func<TValue, Task<TOut>> map)
        where TValue : notnull
        where TOut : notnull
    {
        return awaitableResult.BindAsync(async value =>
        {
            var newValue = await map(value);
            return Result.Success(newValue);
        });
    }

    public static Task<Result<TOut>> MapAsync<TValue, TOut>(this Task<Result<TValue>> awaitableResult,
                                                            Func<TValue, TOut> map)
        where TValue : notnull
        where TOut : notnull
    {
        return awaitableResult.BindAsync(value =>
        {
            var newValue = map(value);
            return Result.Success(newValue);
        });
    }

    public static Task<Result<TOut1, TOut2>> MapAsync<TValue1, TValue2, TOut1, TOut2>(this Result<TValue1, TValue2> result,
                                                                                      Func<TValue1, TValue2, Task<(TOut1, TOut2)>> map)
        where TValue1 : notnull
        where TValue2 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
    {
        return result.BindAsync(async (value1,
                                       value2) =>
        {
            var items = await map(value1, value2);
            return Result.Success(items.Item1, items.Item2);
        });
    }

    public static Task<Result<TOut1, TOut2>> MapAsync<TValue1, TValue2, TOut1, TOut2>(this Task<Result<TValue1, TValue2>> awaitableResult,
                                                                                      Func<TValue1, TValue2, Task<(TOut1, TOut2)>> map)
        where TValue1 : notnull
        where TValue2 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
    {
        return awaitableResult.BindAsync(async (value1,
                                                value2) =>
        {
            var items = await map(value1, value2);
            return Result.Success(items.Item1, items.Item2);
        });
    }

    public static Task<Result<TOut1, TOut2>> MapAsync<TValue1, TValue2, TOut1, TOut2>(this Task<Result<TValue1, TValue2>> awaitableResult,
                                                                                      Func<TValue1, TValue2, (TOut1, TOut2)> map)
        where TValue1 : notnull
        where TValue2 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
    {
        return awaitableResult.BindAsync((value1,
                                          value2) =>
        {
            var items = map(value1, value2);
            return Result.Success(items.Item1, items.Item2);
        });
    }

    public static Task<Result<TOut1, TOut2, TOut3>> MapAsync<TValue1, TValue2, TValue3, TOut1, TOut2, TOut3>(this Result<TValue1, TValue2, TValue3> result,
                                                                                                             Func<TValue1, TValue2, TValue3, Task<(TOut1, TOut2, TOut3)>> map)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
    {
        return result.BindAsync(async (value1,
                                       value2,
                                       value3) =>
        {
            var items = await map(value1, value2, value3);
            return Result.Success(items.Item1, items.Item2, items.Item3);
        });
    }

    public static Task<Result<TOut1, TOut2, TOut3>> MapAsync<TValue1, TValue2, TValue3, TOut1, TOut2, TOut3>(this Task<Result<TValue1, TValue2, TValue3>> awaitableResult,
                                                                                                             Func<TValue1, TValue2, TValue3, Task<(TOut1, TOut2, TOut3)>> map)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
    {
        return awaitableResult.BindAsync(async (value1,
                                                value2,
                                                value3) =>
        {
            var items = await map(value1, value2, value3);
            return Result.Success(items.Item1, items.Item2, items.Item3);
        });
    }

    public static Task<Result<TOut1, TOut2, TOut3>> MapAsync<TValue1, TValue2, TValue3, TOut1, TOut2, TOut3>(this Task<Result<TValue1, TValue2, TValue3>> awaitableResult,
                                                                                                             Func<TValue1, TValue2, TValue3, (TOut1, TOut2, TOut3)> map)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
    {
        return awaitableResult.BindAsync((value1,
                                          value2,
                                          value3) =>
        {
            var items = map(value1, value2, value3);
            return Result.Success(items.Item1, items.Item2, items.Item3);
        });
    }

    public static Task<Result<TOut1, TOut2, TOut3, TOut4>> MapAsync<TValue1, TValue2, TValue3, TValue4, TOut1, TOut2, TOut3, TOut4>(
        this Result<TValue1, TValue2, TValue3, TValue4> result,
        Func<TValue1, TValue2, TValue3, TValue4, Task<(TOut1, TOut2, TOut3, TOut4)>> map)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
    {
        return result.BindAsync(async (value1,
                                       value2,
                                       value3,
                                       value4) =>
        {
            var items = await map(value1, value2, value3, value4);
            return Result.Success(items.Item1, items.Item2, items.Item3, items.Item4);
        });
    }

    public static Task<Result<TOut1, TOut2, TOut3, TOut4>> MapAsync<TValue1, TValue2, TValue3, TValue4, TOut1, TOut2, TOut3, TOut4>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4>> awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, Task<(TOut1, TOut2, TOut3, TOut4)>> map)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
    {
        return awaitableResult.BindAsync(async (value1,
                                                value2,
                                                value3,
                                                value4) =>
        {
            var items = await map(value1, value2, value3, value4);
            return Result.Success(items.Item1, items.Item2, items.Item3, items.Item4);
        });
    }

    public static Task<Result<TOut1, TOut2, TOut3, TOut4>> MapAsync<TValue1, TValue2, TValue3, TValue4, TOut1, TOut2, TOut3, TOut4>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4>> awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, (TOut1, TOut2, TOut3, TOut4)> map)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
    {
        return awaitableResult.BindAsync((value1,
                                          value2,
                                          value3,
                                          value4) =>
        {
            var items = map(value1, value2, value3, value4);
            return Result.Success(items.Item1, items.Item2, items.Item3, items.Item4);
        });
    }

    public static Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5>> MapAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1, TOut2, TOut3, TOut4, TOut5>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5> result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, Task<(TOut1, TOut2, TOut3, TOut4, TOut5)>> map)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
    {
        return result.BindAsync(async (value1,
                                       value2,
                                       value3,
                                       value4,
                                       value5) =>
        {
            var items = await map(value1, value2, value3, value4, value5);
            return Result.Success(items.Item1, items.Item2, items.Item3, items.Item4, items.Item5);
        });
    }

    public static Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5>> MapAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1, TOut2, TOut3, TOut4, TOut5>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, Task<(TOut1, TOut2, TOut3, TOut4, TOut5)>> map)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
    {
        return awaitableResult.BindAsync(async (value1,
                                                value2,
                                                value3,
                                                value4,
                                                value5) =>
        {
            var items = await map(value1, value2, value3, value4, value5);
            return Result.Success(items.Item1, items.Item2, items.Item3, items.Item4, items.Item5);
        });
    }

    public static Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5>> MapAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1, TOut2, TOut3, TOut4, TOut5>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, (TOut1, TOut2, TOut3, TOut4, TOut5)> map)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
    {
        return awaitableResult.BindAsync((value1,
                                          value2,
                                          value3,
                                          value4,
                                          value5) =>
        {
            var items = map(value1, value2, value3, value4, value5);
            return Result.Success(items.Item1, items.Item2, items.Item3, items.Item4, items.Item5);
        });
    }

    public static Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>> MapAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Task<(TOut1, TOut2, TOut3, TOut4, TOut5, TOut6)>> map)
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
        where TOut6 : notnull
    {
        return result.BindAsync(async (value1,
                                       value2,
                                       value3,
                                       value4,
                                       value5,
                                       value6) =>
        {
            var items = await map(value1, value2, value3, value4, value5, value6);
            return Result.Success(items.Item1, items.Item2, items.Item3, items.Item4, items.Item5, items.Item6);
        });
    }

    public static Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>> MapAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>> awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Task<(TOut1, TOut2, TOut3, TOut4, TOut5, TOut6)>> map)
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
        where TOut6 : notnull
    {
        return awaitableResult.BindAsync(async (value1,
                                                value2,
                                                value3,
                                                value4,
                                                value5,
                                                value6) =>
        {
            var items = await map(value1, value2, value3, value4, value5, value6);
            return Result.Success(items.Item1, items.Item2, items.Item3, items.Item4, items.Item5, items.Item6);
        });
    }

    public static Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>> MapAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>> awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, (TOut1, TOut2, TOut3, TOut4, TOut5, TOut6)> map)
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
        where TOut6 : notnull
    {
        return awaitableResult.BindAsync((value1,
                                          value2,
                                          value3,
                                          value4,
                                          value5,
                                          value6) =>
        {
            var items = map(value1, value2, value3, value4, value5, value6);
            return Result.Success(items.Item1, items.Item2, items.Item3, items.Item4, items.Item5, items.Item6);
        });
    }

    public static Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>> MapAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TOut1, TOut2, TOut3, TOut4,
                                                                                         TOut5, TOut6, TOut7>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Task<(TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7)>> map)
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
        where TOut7 : notnull
    {
        return result.BindAsync(async (value1,
                                       value2,
                                       value3,
                                       value4,
                                       value5,
                                       value6,
                                       value7) =>
        {
            var items = await map(value1, value2, value3, value4, value5, value6, value7);
            return Result.Success(items.Item1, items.Item2, items.Item3, items.Item4, items.Item5, items.Item6, items.Item7);
        });
    }

    public static Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>> MapAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TOut1, TOut2, TOut3, TOut4,
                                                                                         TOut5, TOut6, TOut7>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>> awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Task<(TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7)>> map)
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
        where TOut7 : notnull
    {
        return awaitableResult.BindAsync(async (value1,
                                                value2,
                                                value3,
                                                value4,
                                                value5,
                                                value6,
                                                value7) =>
        {
            var items = await map(value1, value2, value3, value4, value5, value6, value7);
            return Result.Success(items.Item1, items.Item2, items.Item3, items.Item4, items.Item5, items.Item6, items.Item7);
        });
    }

    public static Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>> MapAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TOut1, TOut2, TOut3, TOut4,
                                                                                         TOut5, TOut6, TOut7>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>> awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, (TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7)> map)
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
        where TOut7 : notnull
    {
        return awaitableResult.BindAsync((value1,
                                          value2,
                                          value3,
                                          value4,
                                          value5,
                                          value6,
                                          value7) =>
        {
            var items = map(value1, value2, value3, value4, value5, value6, value7);
            return Result.Success(items.Item1, items.Item2, items.Item3, items.Item4, items.Item5, items.Item6, items.Item7);
        });
    }

    public static Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>> MapAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TOut1,
                                                                                                TOut2,
                                                                                                TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Task<(TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8)>> map)
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
        where TOut8 : notnull
    {
        return result.BindAsync(async (value1,
                                       value2,
                                       value3,
                                       value4,
                                       value5,
                                       value6,
                                       value7,
                                       value8) =>
        {
            var items = await map(value1, value2, value3, value4, value5, value6, value7, value8);
            return Result.Success(items.Item1, items.Item2, items.Item3, items.Item4, items.Item5, items.Item6, items.Item7, items.Item8);
        });
    }

    public static Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>> MapAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TOut1,
                                                                                                TOut2,
                                                                                                TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>> awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Task<(TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8)>> map)
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
        where TOut8 : notnull
    {
        return awaitableResult.BindAsync(async (value1,
                                                value2,
                                                value3,
                                                value4,
                                                value5,
                                                value6,
                                                value7,
                                                value8) =>
        {
            var items = await map(value1, value2, value3, value4, value5, value6, value7, value8);
            return Result.Success(items.Item1, items.Item2, items.Item3, items.Item4, items.Item5, items.Item6, items.Item7, items.Item8);
        });
    }

    public static Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>> MapAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TOut1,
                                                                                                TOut2,
                                                                                                TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>> awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, (TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8)> map)
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
        where TOut8 : notnull
    {
        return awaitableResult.BindAsync((value1,
                                          value2,
                                          value3,
                                          value4,
                                          value5,
                                          value6,
                                          value7,
                                          value8) =>
        {
            var items = map(value1, value2, value3, value4, value5, value6, value7, value8);
            return Result.Success(items.Item1, items.Item2, items.Item3, items.Item4, items.Item5, items.Item6, items.Item7, items.Item8);
        });
    }
}
