namespace UnambitiousFx.Core.Results.Tasks;

public static partial class ResultExtensions {
    public static async Task<Result<TValue>> TapErrorAsync<TValue>(this Result<TValue>   result,
                                                                   Func<Exception, Task> tap)
        where TValue : notnull {
        return await result.Match<Task<Result<TValue>>>(
                   value => Task.FromResult(Result.Success<TValue>(value)),
                   async ex => {
                       await tap(ex);
                       return Result.Failure<TValue>(ex);
                   }
               );
    }

    public static async Task<Result<TValue>> TapErrorAsync<TValue>(this Task<Result<TValue>> awaitableResult,
                                                                   Func<Exception, Task>     tap)
        where TValue : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TValue>>>(
                   value => Task.FromResult(Result.Success<TValue>(value)),
                   async ex => {
                       await tap(ex);
                       return Result.Failure<TValue>(ex);
                   }
               );
    }

    public static async Task<Result<TValue1, TValue2>> TapErrorAsync<TValue1, TValue2>(this Result<TValue1, TValue2> result,
                                                                                       Func<Exception, Task>         tap)
        where TValue1 : notnull
        where TValue2 : notnull {
        return await result.Match<Task<Result<TValue1, TValue2>>>(
                   (value1,
                    value2) => Task.FromResult(Result.Success<TValue1, TValue2>(value1, value2)),
                   async ex => {
                       await tap(ex);
                       return Result.Failure<TValue1, TValue2>(ex);
                   }
               );
    }

    public static async Task<Result<TValue1, TValue2>> TapErrorAsync<TValue1, TValue2>(this Task<Result<TValue1, TValue2>> awaitableResult,
                                                                                       Func<Exception, Task>               tap)
        where TValue1 : notnull
        where TValue2 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TValue1, TValue2>>>(
                   (value1,
                    value2) => Task.FromResult(Result.Success<TValue1, TValue2>(value1, value2)),
                   async ex => {
                       await tap(ex);
                       return Result.Failure<TValue1, TValue2>(ex);
                   }
               );
    }

    public static async Task<Result<TValue1, TValue2, TValue3>> TapErrorAsync<TValue1, TValue2, TValue3>(this Result<TValue1, TValue2, TValue3> result,
                                                                                                         Func<Exception, Task>                  tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        return await result.Match<Task<Result<TValue1, TValue2, TValue3>>>(
                   (value1,
                    value2,
                    value3) => Task.FromResult(Result.Success<TValue1, TValue2, TValue3>(value1, value2, value3)),
                   async ex => {
                       await tap(ex);
                       return Result.Failure<TValue1, TValue2, TValue3>(ex);
                   }
               );
    }

    public static async Task<Result<TValue1, TValue2, TValue3>> TapErrorAsync<TValue1, TValue2, TValue3>(this Task<Result<TValue1, TValue2, TValue3>> awaitableResult,
                                                                                                         Func<Exception, Task>                        tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TValue1, TValue2, TValue3>>>(
                   (value1,
                    value2,
                    value3) => Task.FromResult(Result.Success<TValue1, TValue2, TValue3>(value1, value2, value3)),
                   async ex => {
                       await tap(ex);
                       return Result.Failure<TValue1, TValue2, TValue3>(ex);
                   }
               );
    }

    public static async Task<Result<TValue1, TValue2, TValue3, TValue4>> TapErrorAsync<TValue1, TValue2, TValue3, TValue4>(this Result<TValue1, TValue2, TValue3, TValue4> result,
        Func<Exception, Task>                                                                                                                                              tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        return await result.Match<Task<Result<TValue1, TValue2, TValue3, TValue4>>>(
                   (value1,
                    value2,
                    value3,
                    value4) => Task.FromResult(Result.Success<TValue1, TValue2, TValue3, TValue4>(value1, value2, value3, value4)),
                   async ex => {
                       await tap(ex);
                       return Result.Failure<TValue1, TValue2, TValue3, TValue4>(ex);
                   }
               );
    }

    public static async Task<Result<TValue1, TValue2, TValue3, TValue4>> TapErrorAsync<TValue1, TValue2, TValue3, TValue4>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4>> awaitableResult,
        Func<Exception, Task>                                 tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TValue1, TValue2, TValue3, TValue4>>>(
                   (value1,
                    value2,
                    value3,
                    value4) => Task.FromResult(Result.Success<TValue1, TValue2, TValue3, TValue4>(value1, value2, value3, value4)),
                   async ex => {
                       await tap(ex);
                       return Result.Failure<TValue1, TValue2, TValue3, TValue4>(ex);
                   }
               );
    }

    public static async Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> TapErrorAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5> result,
        Func<Exception, Task>                                    tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        return await result.Match<Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5>>>(
                   (value1,
                    value2,
                    value3,
                    value4,
                    value5) => Task.FromResult(Result.Success<TValue1, TValue2, TValue3, TValue4, TValue5>(value1, value2, value3, value4, value5)),
                   async ex => {
                       await tap(ex);
                       return Result.Failure<TValue1, TValue2, TValue3, TValue4, TValue5>(ex);
                   }
               );
    }

    public static async Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> TapErrorAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> awaitableResult,
        Func<Exception, Task>                                          tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5>>>(
                   (value1,
                    value2,
                    value3,
                    value4,
                    value5) => Task.FromResult(Result.Success<TValue1, TValue2, TValue3, TValue4, TValue5>(value1, value2, value3, value4, value5)),
                   async ex => {
                       await tap(ex);
                       return Result.Failure<TValue1, TValue2, TValue3, TValue4, TValue5>(ex);
                   }
               );
    }

    public static async Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>> TapErrorAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> result,
        Func<Exception, Task>                                             tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull {
        return await result.Match<Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>>>(
                   (value1,
                    value2,
                    value3,
                    value4,
                    value5,
                    value6) => Task.FromResult(Result.Success<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(value1, value2, value3, value4, value5, value6)),
                   async ex => {
                       await tap(ex);
                       return Result.Failure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(ex);
                   }
               );
    }

    public static async Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>> TapErrorAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>> awaitableResult,
        Func<Exception, Task>                                                   tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>>>(
                   (value1,
                    value2,
                    value3,
                    value4,
                    value5,
                    value6) => Task.FromResult(Result.Success<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(value1, value2, value3, value4, value5, value6)),
                   async ex => {
                       await tap(ex);
                       return Result.Failure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(ex);
                   }
               );
    }

    public static async Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>> TapErrorAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> result,
        Func<Exception, Task>                                                      tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull {
        return await result.Match<Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>>>(
                   (value1,
                    value2,
                    value3,
                    value4,
                    value5,
                    value6,
                    value7) => Task.FromResult(
                       Result.Success<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(value1, value2, value3, value4, value5, value6, value7)),
                   async ex => {
                       await tap(ex);
                       return Result.Failure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(ex);
                   }
               );
    }

    public static async Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>> TapErrorAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>> awaitableResult,
        Func<Exception, Task>                                                            tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>>>(
                   (value1,
                    value2,
                    value3,
                    value4,
                    value5,
                    value6,
                    value7) => Task.FromResult(
                       Result.Success<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(value1, value2, value3, value4, value5, value6, value7)),
                   async ex => {
                       await tap(ex);
                       return Result.Failure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(ex);
                   }
               );
    }

    public static async Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>>
        TapErrorAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
            this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> result,
            Func<Exception, Task>                                                               tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull {
        return await result.Match<Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>>>(
                   (value1,
                    value2,
                    value3,
                    value4,
                    value5,
                    value6,
                    value7,
                    value8) => Task.FromResult(
                       Result.Success<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(value1, value2, value3, value4, value5, value6, value7, value8)),
                   async ex => {
                       await tap(ex);
                       return Result.Failure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(ex);
                   }
               );
    }

    public static async Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>>
        TapErrorAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
            this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>> awaitableResult,
            Func<Exception, Task>                                                                     tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>>>(
                   (value1,
                    value2,
                    value3,
                    value4,
                    value5,
                    value6,
                    value7,
                    value8) => Task.FromResult(
                       Result.Success<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(value1, value2, value3, value4, value5, value6, value7, value8)),
                   async ex => {
                       await tap(ex);
                       return Result.Failure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(ex);
                   }
               );
    }
}
