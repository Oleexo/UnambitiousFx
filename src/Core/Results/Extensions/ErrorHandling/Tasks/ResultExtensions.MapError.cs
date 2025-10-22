using UnambitiousFx.Core.Results.Extensions.ValueAccess.Tasks;

namespace UnambitiousFx.Core.Results.Extensions.ErrorHandling.Tasks;

public static partial class ResultExtensions {
    public static Task<Result<TValue>> MapErrorAsync<TValue>(this Result<TValue>              result,
                                                             Func<Exception, Task<Exception>> mapError)
        where TValue : notnull {
        return result.Match<Task<Result<TValue>>>(
            value => Task.FromResult(Result.Success(value)),
            async ex => Result.Failure<TValue>(await mapError(ex))
        );
    }

    public static Task<Result<TValue>> MapErrorAsync<TValue>(this Task<Result<TValue>>        awaitableResult,
                                                             Func<Exception, Task<Exception>> mapError)
        where TValue : notnull {
        return awaitableResult.MatchAsync<Result<TValue>, TValue>(
            value => Task.FromResult(Result.Success<TValue>(value)),
            async ex => Result.Failure<TValue>(await mapError(ex))
        );
    }

    public static Task<Result<TValue1, TValue2>> MapErrorAsync<TValue1, TValue2>(this Result<TValue1, TValue2>    result,
                                                                                 Func<Exception, Task<Exception>> mapError)
        where TValue1 : notnull
        where TValue2 : notnull {
        return result.Match<Task<Result<TValue1, TValue2>>>(
            (value1,
             value2) => Task.FromResult(Result.Success(value1, value2)),
            async ex => Result.Failure<TValue1, TValue2>(await mapError(ex))
        );
    }

    public static Task<Result<TValue1, TValue2>> MapErrorAsync<TValue1, TValue2>(this Task<Result<TValue1, TValue2>> awaitableResult,
                                                                                 Func<Exception, Task<Exception>>    mapError)
        where TValue1 : notnull
        where TValue2 : notnull {
        return awaitableResult.MatchAsync(
            (value1,
             value2) => Task.FromResult(Result.Success(value1, value2)),
            async ex => Result.Failure<TValue1, TValue2>(await mapError(ex))
        );
    }

    public static Task<Result<TValue1, TValue2, TValue3>> MapErrorAsync<TValue1, TValue2, TValue3>(this Result<TValue1, TValue2, TValue3> result,
                                                                                                   Func<Exception, Task<Exception>>       mapError)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        return result.Match<Task<Result<TValue1, TValue2, TValue3>>>(
            (value1,
             value2,
             value3) => Task.FromResult(Result.Success(value1, value2, value3)),
            async ex => Result.Failure<TValue1, TValue2, TValue3>(await mapError(ex))
        );
    }

    public static Task<Result<TValue1, TValue2, TValue3>> MapErrorAsync<TValue1, TValue2, TValue3>(this Task<Result<TValue1, TValue2, TValue3>> awaitableResult,
                                                                                                   Func<Exception, Task<Exception>>             mapError)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        return awaitableResult.MatchAsync(
            (value1,
             value2,
             value3) => Task.FromResult(Result.Success(value1, value2, value3)),
            async ex => Result.Failure<TValue1, TValue2, TValue3>(await mapError(ex))
        );
    }

    public static Task<Result<TValue1, TValue2, TValue3, TValue4>> MapErrorAsync<TValue1, TValue2, TValue3, TValue4>(this Result<TValue1, TValue2, TValue3, TValue4> result,
                                                                                                                     Func<Exception, Task<Exception>>                mapError)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        return result.Match<Task<Result<TValue1, TValue2, TValue3, TValue4>>>(
            (value1,
             value2,
             value3,
             value4) => Task.FromResult(Result.Success(value1, value2, value3, value4)),
            async ex => Result.Failure<TValue1, TValue2, TValue3, TValue4>(await mapError(ex))
        );
    }

    public static Task<Result<TValue1, TValue2, TValue3, TValue4>> MapErrorAsync<TValue1, TValue2, TValue3, TValue4>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4>> awaitableResult,
        Func<Exception, Task<Exception>>                      mapError)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        return awaitableResult.MatchAsync(
            (value1,
             value2,
             value3,
             value4) => Task.FromResult(Result.Success(value1, value2, value3, value4)),
            async ex => Result.Failure<TValue1, TValue2, TValue3, TValue4>(await mapError(ex))
        );
    }

    public static Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> MapErrorAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5> result,
        Func<Exception, Task<Exception>>                         mapError)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        return result.Match<Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5>>>(
            (value1,
             value2,
             value3,
             value4,
             value5) => Task.FromResult(
                Result.Success(value1, value2, value3, value4, value5)),
            async ex => Result.Failure<TValue1, TValue2, TValue3, TValue4, TValue5>(await mapError(ex))
        );
    }

    public static Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> MapErrorAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> awaitableResult,
        Func<Exception, Task<Exception>>                               mapError)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        return awaitableResult.MatchAsync(
            (value1,
             value2,
             value3,
             value4,
             value5) => Task.FromResult(
                Result.Success(value1, value2, value3, value4, value5)),
            async ex => Result.Failure<TValue1, TValue2, TValue3, TValue4, TValue5>(await mapError(ex))
        );
    }

    public static Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>> MapErrorAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> result,
        Func<Exception, Task<Exception>>                                  mapError)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull {
        return result.Match<Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>>>(
            (value1,
             value2,
             value3,
             value4,
             value5,
             value6) => Task.FromResult(
                Result.Success(value1, value2, value3, value4, value5, value6)),
            async ex => Result.Failure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(await mapError(ex))
        );
    }

    public static Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>> MapErrorAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>> awaitableResult,
        Func<Exception, Task<Exception>>                                        mapError)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull {
        return awaitableResult.MatchAsync(
            (value1,
             value2,
             value3,
             value4,
             value5,
             value6) => Task.FromResult(
                Result.Success(value1, value2, value3, value4, value5, value6)),
            async ex => Result.Failure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(await mapError(ex))
        );
    }

    public static Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>> MapErrorAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> result,
        Func<Exception, Task<Exception>>                                           mapError)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull {
        return result.Match<Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>>>(
            (value1,
             value2,
             value3,
             value4,
             value5,
             value6,
             value7) => Task.FromResult(
                Result.Success(value1, value2, value3, value4, value5, value6, value7)),
            async ex => Result.Failure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(await mapError(ex))
        );
    }

    public static Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>> MapErrorAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>> awaitableResult,
        Func<Exception, Task<Exception>>                                                 mapError)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull {
        return awaitableResult.MatchAsync(
            (value1,
             value2,
             value3,
             value4,
             value5,
             value6,
             value7) => Task.FromResult(
                Result.Success(value1, value2, value3, value4, value5, value6, value7)),
            async ex => Result.Failure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(await mapError(ex))
        );
    }

    public static Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>>
        MapErrorAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
            this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> result,
            Func<Exception, Task<Exception>>                                                    mapError)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull {
        return result.Match<Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>>>(
            (value1,
             value2,
             value3,
             value4,
             value5,
             value6,
             value7,
             value8) => Task.FromResult(
                Result.Success(value1, value2, value3, value4, value5, value6, value7, value8)),
            async ex => Result.Failure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(await mapError(ex))
        );
    }

    public static Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>>
        MapErrorAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
            this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>> awaitableResult,
            Func<Exception, Task<Exception>>                                                          mapError)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull {
        return awaitableResult.MatchAsync(
            (value1,
             value2,
             value3,
             value4,
             value5,
             value6,
             value7,
             value8) => Task.FromResult(
                Result.Success(value1, value2, value3, value4, value5, value6, value7, value8)),
            async ex => Result.Failure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(await mapError(ex))
        );
    }
}
