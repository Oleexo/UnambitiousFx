using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Results.Extensions.SideEffects.Tasks;

public static class ResultTapBothExtensions {
    public static async Task<Result<TValue>> TapBothAsync<TValue>(this Result<TValue>             result,
                                                                  Func<TValue, Task>              onSuccess,
                                                                  Func<IEnumerable<IError>, Task> onFailure)
        where TValue : notnull {
        if (result.TryGet(out var value, out var err)) {
            await onSuccess(value);
        }
        else {
            await onFailure(err);
        }

        return result;
    }

    public static async Task<Result<TValue>> TapBothAsync<TValue>(this Task<Result<TValue>>       awaitableResult,
                                                                  Func<TValue, Task>              onSuccess,
                                                                  Func<IEnumerable<IError>, Task> onFailure)
        where TValue : notnull {
        var result = await awaitableResult;
        return await result.TapBothAsync(onSuccess, onFailure);
    }

    public static async Task<Result<TValue1, TValue2>> TapBothAsync<TValue1, TValue2>(this Result<TValue1, TValue2>   result,
                                                                                      Func<TValue1, TValue2, Task>    onSuccess,
                                                                                      Func<IEnumerable<IError>, Task> onFailure)
        where TValue1 : notnull
        where TValue2 : notnull {
        if (result.TryGet(out var value1, out var value2, out var err)) {
            await onSuccess(value1, value2);
        }
        else {
            await onFailure(err);
        }

        return result;
    }

    public static async Task<Result<TValue1, TValue2>> TapBothAsync<TValue1, TValue2>(this Task<Result<TValue1, TValue2>> awaitableResult,
                                                                                      Func<TValue1, TValue2, Task>        onSuccess,
                                                                                      Func<IEnumerable<IError>, Task>     onFailure)
        where TValue1 : notnull
        where TValue2 : notnull {
        var result = await awaitableResult;
        return await result.TapBothAsync(onSuccess, onFailure);
    }

    public static async Task<Result<TValue1, TValue2, TValue3>> TapBothAsync<TValue1, TValue2, TValue3>(this Result<TValue1, TValue2, TValue3> result,
                                                                                                        Func<TValue1, TValue2, TValue3, Task>  onSuccess,
                                                                                                        Func<IEnumerable<IError>, Task>        onFailure)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        if (result.TryGet(out var value1, out var value2, out var value3, out var err)) {
            await onSuccess(value1, value2, value3);
        }
        else {
            await onFailure(err);
        }

        return result;
    }

    public static async Task<Result<TValue1, TValue2, TValue3>> TapBothAsync<TValue1, TValue2, TValue3>(this Task<Result<TValue1, TValue2, TValue3>> awaitableResult,
                                                                                                        Func<TValue1, TValue2, TValue3, Task>        onSuccess,
                                                                                                        Func<IEnumerable<IError>, Task>              onFailure)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        var result = await awaitableResult;
        return await result.TapBothAsync(onSuccess, onFailure);
    }

    public static async Task<Result<TValue1, TValue2, TValue3, TValue4>> TapBothAsync<TValue1, TValue2, TValue3, TValue4>(this Result<TValue1, TValue2, TValue3, TValue4> result,
        Func<TValue1, TValue2, TValue3, TValue4, Task>                                                                                                                    onSuccess,
        Func<IEnumerable<IError>, Task>                                                                                                                                   onFailure)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        if (result.TryGet(out var value1, out var value2, out var value3, out var value4, out var err)) {
            await onSuccess(value1, value2, value3, value4);
        }
        else {
            await onFailure(err);
        }

        return result;
    }

    public static async Task<Result<TValue1, TValue2, TValue3, TValue4>> TapBothAsync<TValue1, TValue2, TValue3, TValue4>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4>> awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, Task>        onSuccess,
        Func<IEnumerable<IError>, Task>                       onFailure)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        var result = await awaitableResult;
        return await result.TapBothAsync(onSuccess, onFailure);
    }

    public static async Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> TapBothAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5> result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, Task>  onSuccess,
        Func<IEnumerable<IError>, Task>                          onFailure)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        if (result.TryGet(out var value1, out var value2, out var value3, out var value4, out var value5, out var err)) {
            await onSuccess(value1, value2, value3, value4, value5);
        }
        else {
            await onFailure(err);
        }

        return result;
    }

    public static async Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> TapBothAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, Task>        onSuccess,
        Func<IEnumerable<IError>, Task>                                onFailure)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        var result = await awaitableResult;
        return await result.TapBothAsync(onSuccess, onFailure);
    }

    public static async Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>> TapBothAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Task>  onSuccess,
        Func<IEnumerable<IError>, Task>                                   onFailure)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull {
        if (result.TryGet(out var value1, out var value2, out var value3, out var value4, out var value5, out var value6, out var err)) {
            await onSuccess(value1, value2, value3, value4, value5, value6);
        }
        else {
            await onFailure(err);
        }

        return result;
    }

    public static async Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>> TapBothAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>> awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Task>        onSuccess,
        Func<IEnumerable<IError>, Task>                                         onFailure)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull {
        var result = await awaitableResult;
        return await result.TapBothAsync(onSuccess, onFailure);
    }

    public static async Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>> TapBothAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Task>  onSuccess,
        Func<IEnumerable<IError>, Task>                                            onFailure)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull {
        if (result.TryGet(out var value1, out var value2, out var value3, out var value4, out var value5, out var value6, out var value7, out var err)) {
            await onSuccess(value1, value2, value3, value4, value5, value6, value7);
        }
        else {
            await onFailure(err);
        }

        return result;
    }

    public static async Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>> TapBothAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>> awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Task>        onSuccess,
        Func<IEnumerable<IError>, Task>                                                  onFailure)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull {
        var result = await awaitableResult;
        return await result.TapBothAsync(onSuccess, onFailure);
    }

    public static async Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>>
        TapBothAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
            this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> result,
            Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Task>  onSuccess,
            Func<IEnumerable<IError>, Task>                                                     onFailure)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull {
        if (result.TryGet(out var value1, out var value2, out var value3, out var value4, out var value5, out var value6, out var value7, out var value8, out var err)) {
            await onSuccess(value1, value2, value3, value4, value5, value6, value7, value8);
        }
        else {
            await onFailure(err);
        }

        return result;
    }

    public static async Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>>
        TapBothAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
            this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>> awaitableResult,
            Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Task>        onSuccess,
            Func<IEnumerable<IError>, Task>                                                           onFailure)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull {
        var result = await awaitableResult;
        return await result.TapBothAsync(onSuccess, onFailure);
    }
}
