namespace UnambitiousFx.Core.Results.ValueTasks;

public static partial class ResultExtensions {
    public static ValueTask<Result<TValue>> TapErrorAsync<TValue>(this Result<TValue>        result,
                                                                  Func<Exception, ValueTask> tap)
        where TValue : notnull {
        return result.Match<ValueTask<Result<TValue>>>(
            value => new ValueTask<Result<TValue>>(Result.Success<TValue>(value)),
            async ex => {
                await tap(ex);
                return Result.Failure<TValue>(ex);
            }
        );
    }

    public static async ValueTask<Result<TValue>> TapErrorAsync<TValue>(this ValueTask<Result<TValue>> awaitableResult,
                                                                        Func<Exception, ValueTask>     tap)
        where TValue : notnull {
        var result = await awaitableResult;
        return await result.Match<ValueTask<Result<TValue>>>(
                   value => new ValueTask<Result<TValue>>(Result.Success<TValue>(value)),
                   async ex => {
                       await tap(ex);
                       return Result.Failure<TValue>(ex);
                   }
               );
    }

    public static async ValueTask<Result<TValue1, TValue2>> TapErrorAsync<TValue1, TValue2>(this Result<TValue1, TValue2> result,
                                                                                            Func<Exception, ValueTask>    tap)
        where TValue1 : notnull
        where TValue2 : notnull {
        return await result.Match<ValueTask<Result<TValue1, TValue2>>>(
                   (value1,
                    value2) => new ValueTask<Result<TValue1, TValue2>>(Result.Success<TValue1, TValue2>(value1, value2)),
                   async ex => {
                       await tap(ex);
                       return Result.Failure<TValue1, TValue2>(ex);
                   }
               );
    }

    public static async ValueTask<Result<TValue1, TValue2>> TapErrorAsync<TValue1, TValue2>(this ValueTask<Result<TValue1, TValue2>> awaitableResult,
                                                                                            Func<Exception, ValueTask>               tap)
        where TValue1 : notnull
        where TValue2 : notnull {
        var result = await awaitableResult;
        return await result.Match<ValueTask<Result<TValue1, TValue2>>>(
                   (value1,
                    value2) => new ValueTask<Result<TValue1, TValue2>>(Result.Success<TValue1, TValue2>(value1, value2)),
                   async ex => {
                       await tap(ex);
                       return Result.Failure<TValue1, TValue2>(ex);
                   }
               );
    }

    public static async ValueTask<Result<TValue1, TValue2, TValue3>> TapErrorAsync<TValue1, TValue2, TValue3>(this Result<TValue1, TValue2, TValue3> result,
                                                                                                              Func<Exception, ValueTask>             tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        return await result.Match<ValueTask<Result<TValue1, TValue2, TValue3>>>(
                   (value1,
                    value2,
                    value3) => new ValueTask<Result<TValue1, TValue2, TValue3>>(Result.Success<TValue1, TValue2, TValue3>(value1, value2, value3)),
                   async ex => {
                       await tap(ex);
                       return Result.Failure<TValue1, TValue2, TValue3>(ex);
                   }
               );
    }

    public static async ValueTask<Result<TValue1, TValue2, TValue3>> TapErrorAsync<TValue1, TValue2, TValue3>(this ValueTask<Result<TValue1, TValue2, TValue3>> awaitableResult,
                                                                                                              Func<Exception, ValueTask>                        tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        var result = await awaitableResult;
        return await result.Match<ValueTask<Result<TValue1, TValue2, TValue3>>>(
                   (value1,
                    value2,
                    value3) => new ValueTask<Result<TValue1, TValue2, TValue3>>(Result.Success<TValue1, TValue2, TValue3>(value1, value2, value3)),
                   async ex => {
                       await tap(ex);
                       return Result.Failure<TValue1, TValue2, TValue3>(ex);
                   }
               );
    }

    public static async ValueTask<Result<TValue1, TValue2, TValue3, TValue4>> TapErrorAsync<TValue1, TValue2, TValue3, TValue4>(
        this Result<TValue1, TValue2, TValue3, TValue4> result,
        Func<Exception, ValueTask>                      tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        return await result.Match<ValueTask<Result<TValue1, TValue2, TValue3, TValue4>>>(
                   (value1,
                    value2,
                    value3,
                    value4) => new ValueTask<Result<TValue1, TValue2, TValue3, TValue4>>(Result.Success<TValue1, TValue2, TValue3, TValue4>(value1, value2, value3, value4)),
                   async ex => {
                       await tap(ex);
                       return Result.Failure<TValue1, TValue2, TValue3, TValue4>(ex);
                   }
               );
    }

    public static async ValueTask<Result<TValue1, TValue2, TValue3, TValue4>> TapErrorAsync<TValue1, TValue2, TValue3, TValue4>(
        this ValueTask<Result<TValue1, TValue2, TValue3, TValue4>> awaitableResult,
        Func<Exception, ValueTask>                                 tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        var result = await awaitableResult;
        return await result.Match<ValueTask<Result<TValue1, TValue2, TValue3, TValue4>>>(
                   (value1,
                    value2,
                    value3,
                    value4) => new ValueTask<Result<TValue1, TValue2, TValue3, TValue4>>(Result.Success<TValue1, TValue2, TValue3, TValue4>(value1, value2, value3, value4)),
                   async ex => {
                       await tap(ex);
                       return Result.Failure<TValue1, TValue2, TValue3, TValue4>(ex);
                   }
               );
    }

    public static async ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> TapErrorAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5> result,
        Func<Exception, ValueTask>                               tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        return await result.Match<ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5>>>(
                   (value1,
                    value2,
                    value3,
                    value4,
                    value5) => new ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5>>(
                       Result.Success<TValue1, TValue2, TValue3, TValue4, TValue5>(value1, value2, value3, value4, value5)),
                   async ex => {
                       await tap(ex);
                       return Result.Failure<TValue1, TValue2, TValue3, TValue4, TValue5>(ex);
                   }
               );
    }

    public static async ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> TapErrorAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(
        this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> awaitableResult,
        Func<Exception, ValueTask>                                          tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        var result = await awaitableResult;
        return await result.Match<ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5>>>(
                   (value1,
                    value2,
                    value3,
                    value4,
                    value5) => new ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5>>(
                       Result.Success<TValue1, TValue2, TValue3, TValue4, TValue5>(value1, value2, value3, value4, value5)),
                   async ex => {
                       await tap(ex);
                       return Result.Failure<TValue1, TValue2, TValue3, TValue4, TValue5>(ex);
                   }
               );
    }

    public static async ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>> TapErrorAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> result,
        Func<Exception, ValueTask>                                        tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull {
        return await result.Match<ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>>>(
                   (value1,
                    value2,
                    value3,
                    value4,
                    value5,
                    value6) => new ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>>(
                       Result.Success<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(value1, value2, value3, value4, value5, value6)),
                   async ex => {
                       await tap(ex);
                       return Result.Failure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(ex);
                   }
               );
    }

    public static async ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>> TapErrorAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>> awaitableResult,
        Func<Exception, ValueTask>                                                   tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull {
        var result = await awaitableResult;
        return await result.Match<ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>>>(
                   (value1,
                    value2,
                    value3,
                    value4,
                    value5,
                    value6) => new ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>>(
                       Result.Success<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(value1, value2, value3, value4, value5, value6)),
                   async ex => {
                       await tap(ex);
                       return Result.Failure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(ex);
                   }
               );
    }

    public static async ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>>
        TapErrorAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> result,
                                                                                     Func<Exception, ValueTask>                                                 tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull {
        return await result.Match<ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>>>(
                   (value1,
                    value2,
                    value3,
                    value4,
                    value5,
                    value6,
                    value7) => new ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>>(
                       Result.Success<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(value1, value2, value3, value4, value5, value6, value7)),
                   async ex => {
                       await tap(ex);
                       return Result.Failure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(ex);
                   }
               );
    }

    public static async ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>>
        TapErrorAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
            this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>> awaitableResult,
            Func<Exception, ValueTask>                                                            tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull {
        var result = await awaitableResult;
        return await result.Match<ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>>>(
                   (value1,
                    value2,
                    value3,
                    value4,
                    value5,
                    value6,
                    value7) => new ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>>(
                       Result.Success<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(value1, value2, value3, value4, value5, value6, value7)),
                   async ex => {
                       await tap(ex);
                       return Result.Failure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(ex);
                   }
               );
    }

    public static async ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>>
        TapErrorAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
            this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> result,
            Func<Exception, ValueTask>                                                          tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull {
        return await result.Match<ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>>>(
                   (value1,
                    value2,
                    value3,
                    value4,
                    value5,
                    value6,
                    value7,
                    value8) => new ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>>(
                       Result.Success<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(value1, value2, value3, value4, value5, value6, value7, value8)),
                   async ex => {
                       await tap(ex);
                       return Result.Failure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(ex);
                   }
               );
    }

    public static async ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>>
        TapErrorAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
            this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>> awaitableResult,
            Func<Exception, ValueTask>                                                                     tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull {
        var result = await awaitableResult;
        return await result.Match<ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>>>(
                   (value1,
                    value2,
                    value3,
                    value4,
                    value5,
                    value6,
                    value7,
                    value8) => new ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>>(
                       Result.Success<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(value1, value2, value3, value4, value5, value6, value7, value8)),
                   async ex => {
                       await tap(ex);
                       return Result.Failure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(ex);
                   }
               );
    }
}
