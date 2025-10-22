namespace UnambitiousFx.Core.Results.Extensions.ErrorHandling;

public static partial class ResultExtensions {
    public static Result MapError(this Result                result,
                                  Func<Exception, Exception> mapError) {
        return result.Match(
            Result.Success,
            ex => Result.Failure(mapError(ex))
        );
    }

    public static Result MapError(this Result                result,
                                  Func<Exception, Exception> mapError,
                                  MapErrorChainPolicy        policy) {
        if (result.IsSuccess) {
            return result;
        }

        return policy switch {
            MapErrorChainPolicy.ShortCircuit => result.MapError(mapError),
            MapErrorChainPolicy.Accumulate   => Accumulate(result, mapError),
            _                                => throw new ArgumentOutOfRangeException(nameof(policy), policy, null)
        };
    }

    public static Result<TValue> MapError<TValue>(this Result<TValue>        result,
                                                  Func<Exception, Exception> mapError)
        where TValue : notnull {
        return result.Match(
            Result.Success,
            ex => Result.Failure<TValue>(mapError(ex))
        );
    }

    public static Result<T1> MapError<T1>(this Result<T1>            result,
                                          Func<Exception, Exception> mapError,
                                          MapErrorChainPolicy        policy)
        where T1 : notnull {
        if (result.IsSuccess) {
            return result; // no-op
        }

        return policy switch {
            MapErrorChainPolicy.ShortCircuit => result.MapError(mapError),
            MapErrorChainPolicy.Accumulate   => Accumulate(result, mapError),
            _                                => throw new ArgumentOutOfRangeException(nameof(policy), policy, null)
        };
    }

    public static Result<TValue1, TValue2> MapError<TValue1, TValue2>(this Result<TValue1, TValue2> result,
                                                                      Func<Exception, Exception>    mapError)
        where TValue1 : notnull
        where TValue2 : notnull {
        return result.Match(
            Result.Success,
            ex => Result.Failure<TValue1, TValue2>(mapError(ex))
        );
    }

    public static Result<T1, T2> MapError<T1, T2>(this Result<T1, T2>        result,
                                                  Func<Exception, Exception> mapError,
                                                  MapErrorChainPolicy        policy)
        where T1 : notnull
        where T2 : notnull {
        if (result.IsSuccess) {
            return result; // no-op
        }

        return policy switch {
            MapErrorChainPolicy.ShortCircuit => result.MapError(mapError),
            MapErrorChainPolicy.Accumulate   => Accumulate(result, mapError),
            _                                => throw new ArgumentOutOfRangeException(nameof(policy), policy, null)
        };
    }

    public static Result<TValue1, TValue2, TValue3> MapError<TValue1, TValue2, TValue3>(this Result<TValue1, TValue2, TValue3> result,
                                                                                        Func<Exception, Exception>             mapError)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        return result.Match(
            Result.Success,
            ex => Result.Failure<TValue1, TValue2, TValue3>(mapError(ex))
        );
    }

    public static Result<T1, T2, T3> MapError<T1, T2, T3>(this Result<T1, T2, T3>    result,
                                                          Func<Exception, Exception> mapError,
                                                          MapErrorChainPolicy        policy)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull {
        if (result.IsSuccess) {
            return result; // no-op
        }

        return policy switch {
            MapErrorChainPolicy.ShortCircuit => result.MapError(mapError),
            MapErrorChainPolicy.Accumulate   => Accumulate(result, mapError),
            _                                => throw new ArgumentOutOfRangeException(nameof(policy), policy, null)
        };
    }

    public static Result<TValue1, TValue2, TValue3, TValue4> MapError<TValue1, TValue2, TValue3, TValue4>(this Result<TValue1, TValue2, TValue3, TValue4> result,
                                                                                                          Func<Exception, Exception>                      mapError)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        return result.Match(
            Result.Success,
            ex => Result.Failure<TValue1, TValue2, TValue3, TValue4>(mapError(ex))
        );
    }

    public static Result<T1, T2, T3, T4> MapError<T1, T2, T3, T4>(this Result<T1, T2, T3, T4> result,
                                                                  Func<Exception, Exception>  mapError,
                                                                  MapErrorChainPolicy         policy)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull {
        if (result.IsSuccess) {
            return result; // no-op
        }

        return policy switch {
            MapErrorChainPolicy.ShortCircuit => result.MapError(mapError),
            MapErrorChainPolicy.Accumulate   => Accumulate(result, mapError),
            _                                => throw new ArgumentOutOfRangeException(nameof(policy), policy, null)
        };
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5> MapError<TValue1, TValue2, TValue3, TValue4, TValue5>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5> result,
        Func<Exception, Exception>                               mapError)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        return result.Match(
            Result.Success,
            ex => Result.Failure<TValue1, TValue2, TValue3, TValue4, TValue5>(mapError(ex))
        );
    }

    public static Result<T1, T2, T3, T4, T5> MapError<T1, T2, T3, T4, T5>(this Result<T1, T2, T3, T4, T5> result,
                                                                          Func<Exception, Exception>      mapError,
                                                                          MapErrorChainPolicy             policy)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull {
        if (result.IsSuccess) {
            return result; // no-op
        }

        return policy switch {
            MapErrorChainPolicy.ShortCircuit => result.MapError(mapError),
            MapErrorChainPolicy.Accumulate   => Accumulate(result, mapError),
            _                                => throw new ArgumentOutOfRangeException(nameof(policy), policy, null)
        };
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> MapError<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> result,
        Func<Exception, Exception>                                        mapError)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull {
        return result.Match(
            Result.Success,
            ex => Result.Failure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(mapError(ex))
        );
    }

    public static Result<T1, T2, T3, T4, T5, T6> MapError<T1, T2, T3, T4, T5, T6>(this Result<T1, T2, T3, T4, T5, T6> result,
                                                                                  Func<Exception, Exception>          mapError,
                                                                                  MapErrorChainPolicy                 policy)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull {
        if (result.IsSuccess) {
            return result; // no-op
        }

        return policy switch {
            MapErrorChainPolicy.ShortCircuit => result.MapError(mapError),
            MapErrorChainPolicy.Accumulate   => Accumulate(result, mapError),
            _                                => throw new ArgumentOutOfRangeException(nameof(policy), policy, null)
        };
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> MapError<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> result,
        Func<Exception, Exception>                                                 mapError)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull {
        return result.Match(
            Result.Success,
            ex => Result.Failure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(mapError(ex))
        );
    }

    public static Result<T1, T2, T3, T4, T5, T6, T7> MapError<T1, T2, T3, T4, T5, T6, T7>(this Result<T1, T2, T3, T4, T5, T6, T7> result,
                                                                                          Func<Exception, Exception>              mapError,
                                                                                          MapErrorChainPolicy                     policy)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
        where T7 : notnull {
        if (result.IsSuccess) {
            return result; // no-op
        }

        return policy switch {
            MapErrorChainPolicy.ShortCircuit => result.MapError(mapError),
            MapErrorChainPolicy.Accumulate   => Accumulate(result, mapError),
            _                                => throw new ArgumentOutOfRangeException(nameof(policy), policy, null)
        };
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> MapError<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> result,
        Func<Exception, Exception>                                                          mapError)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull {
        return result.Match(
            Result.Success,
            ex => Result.Failure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(mapError(ex))
        );
    }

    public static Result<T1, T2, T3, T4, T5, T6, T7, T8> MapError<T1, T2, T3, T4, T5, T6, T7, T8>(this Result<T1, T2, T3, T4, T5, T6, T7, T8> result,
                                                                                                  Func<Exception, Exception>                  mapError,
                                                                                                  MapErrorChainPolicy                         policy)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
        where T7 : notnull
        where T8 : notnull {
        if (result.IsSuccess) {
            return result; // no-op
        }

        return policy switch {
            MapErrorChainPolicy.ShortCircuit => result.MapError(mapError),
            MapErrorChainPolicy.Accumulate   => Accumulate(result, mapError),
            _                                => throw new ArgumentOutOfRangeException(nameof(policy), policy, null)
        };
    }
}
