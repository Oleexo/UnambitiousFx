namespace UnambitiousFx.Core.Results.ValueTasks;

public static partial class ResultExtensions {
    public static ValueTask<Result<TOut>> Select<TValue1, TOut>(this Result<TValue1> source,
                                                                Func<TValue1, TOut>  selector)
        where TValue1 : notnull
        where TOut : notnull {
        return source.Match<ValueTask<Result<TOut>>>(
            value1 => ValueTask.FromResult(Result.Success(selector(value1))),
            e => new ValueTask<Result<TOut>>(Result.Failure<TOut>(e))
        );
    }

    public static ValueTask<Result<TOut>> Select<TValue1, TOut>(this ValueTask<Result<TValue1>> awaitableSource,
                                                                Func<TValue1, TOut>             selector)
        where TValue1 : notnull
        where TOut : notnull {
        return awaitableSource.MatchAsync<Result<TOut>, TValue1>(
            value1 => ValueTask.FromResult(Result.Success(selector(value1))),
            e => new ValueTask<Result<TOut>>(Result.Failure<TOut>(e))
        );
    }

    public static ValueTask<Result<TOut>> Select<TValue1, TValue2, TOut>(this Result<TValue1, TValue2> source,
                                                                         Func<TValue1, TValue2, TOut>  selector)
        where TValue1 : notnull
        where TValue2 : notnull
        where TOut : notnull {
        return source.Match<ValueTask<Result<TOut>>>(
            (value1,
             value2) => ValueTask.FromResult(Result.Success(selector(value1, value2))),
            e => new ValueTask<Result<TOut>>(Result.Failure<TOut>(e))
        );
    }

    public static ValueTask<Result<TOut>> Select<TValue1, TValue2, TOut>(this ValueTask<Result<TValue1, TValue2>> awaitableSource,
                                                                         Func<TValue1, TValue2, TOut>             selector)
        where TValue1 : notnull
        where TValue2 : notnull
        where TOut : notnull {
        return awaitableSource.MatchAsync<Result<TOut>, TValue1, TValue2>(
            (value1,
             value2) => ValueTask.FromResult(Result.Success(selector(value1, value2))),
            e => new ValueTask<Result<TOut>>(Result.Failure<TOut>(e))
        );
    }

    public static ValueTask<Result<TOut>> Select<TValue1, TValue2, TValue3, TOut>(this Result<TValue1, TValue2, TValue3> source,
                                                                                  Func<TValue1, TValue2, TValue3, TOut>  selector)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TOut : notnull {
        return source.Match<ValueTask<Result<TOut>>>(
            (value1,
             value2,
             value3) => ValueTask.FromResult(Result.Success(selector(value1, value2, value3))),
            e => new ValueTask<Result<TOut>>(Result.Failure<TOut>(e))
        );
    }

    public static ValueTask<Result<TOut>> Select<TValue1, TValue2, TValue3, TOut>(this ValueTask<Result<TValue1, TValue2, TValue3>> awaitableSource,
                                                                                  Func<TValue1, TValue2, TValue3, TOut>             selector)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TOut : notnull {
        return awaitableSource.MatchAsync<Result<TOut>, TValue1, TValue2, TValue3>(
            (value1,
             value2,
             value3) => ValueTask.FromResult(Result.Success(selector(value1, value2, value3))),
            e => new ValueTask<Result<TOut>>(Result.Failure<TOut>(e))
        );
    }

    public static ValueTask<Result<TOut>> Select<TValue1, TValue2, TValue3, TValue4, TOut>(this Result<TValue1, TValue2, TValue3, TValue4> source,
                                                                                           Func<TValue1, TValue2, TValue3, TValue4, TOut>  selector)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TOut : notnull {
        return source.Match<ValueTask<Result<TOut>>>(
            (value1,
             value2,
             value3,
             value4) => ValueTask.FromResult(Result.Success(selector(value1, value2, value3, value4))),
            e => new ValueTask<Result<TOut>>(Result.Failure<TOut>(e))
        );
    }

    public static ValueTask<Result<TOut>> Select<TValue1, TValue2, TValue3, TValue4, TOut>(this ValueTask<Result<TValue1, TValue2, TValue3, TValue4>> awaitableSource,
                                                                                           Func<TValue1, TValue2, TValue3, TValue4, TOut>             selector)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TOut : notnull {
        return awaitableSource.MatchAsync<Result<TOut>, TValue1, TValue2, TValue3, TValue4>(
            (value1,
             value2,
             value3,
             value4) => ValueTask.FromResult(Result.Success(selector(value1, value2, value3, value4))),
            e => new ValueTask<Result<TOut>>(Result.Failure<TOut>(e))
        );
    }

    public static ValueTask<Result<TOut>> Select<TValue1, TValue2, TValue3, TValue4, TValue5, TOut>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5> source,
                                                                                                    Func<TValue1, TValue2, TValue3, TValue4, TValue5, TOut>  selector)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TOut : notnull {
        return source.Match<ValueTask<Result<TOut>>>(
            (value1,
             value2,
             value3,
             value4,
             value5) => ValueTask.FromResult(Result.Success(selector(value1, value2, value3, value4, value5))),
            e => new ValueTask<Result<TOut>>(Result.Failure<TOut>(e))
        );
    }

    public static ValueTask<Result<TOut>> Select<TValue1, TValue2, TValue3, TValue4, TValue5, TOut>(
        this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> awaitableSource,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TOut>             selector)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TOut : notnull {
        return awaitableSource.MatchAsync<Result<TOut>, TValue1, TValue2, TValue3, TValue4, TValue5>(
            (value1,
             value2,
             value3,
             value4,
             value5) => ValueTask.FromResult(Result.Success(selector(value1, value2, value3, value4, value5))),
            e => new ValueTask<Result<TOut>>(Result.Failure<TOut>(e))
        );
    }

    public static ValueTask<Result<TOut>> Select<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TOut>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> source,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TOut>  selector)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TOut : notnull {
        return source.Match<ValueTask<Result<TOut>>>(
            (value1,
             value2,
             value3,
             value4,
             value5,
             value6) => ValueTask.FromResult(Result.Success(selector(value1, value2, value3, value4, value5, value6))),
            e => new ValueTask<Result<TOut>>(Result.Failure<TOut>(e))
        );
    }

    public static ValueTask<Result<TOut>> Select<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TOut>(
        this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>> awaitableSource,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TOut>             selector)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TOut : notnull {
        return awaitableSource.MatchAsync<Result<TOut>, TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
            (value1,
             value2,
             value3,
             value4,
             value5,
             value6) => ValueTask.FromResult(Result.Success(selector(value1, value2, value3, value4, value5, value6))),
            e => new ValueTask<Result<TOut>>(Result.Failure<TOut>(e))
        );
    }

    public static ValueTask<Result<TOut>> Select<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TOut>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> source,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TOut>  selector)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TOut : notnull {
        return source.Match<ValueTask<Result<TOut>>>(
            (value1,
             value2,
             value3,
             value4,
             value5,
             value6,
             value7) => ValueTask.FromResult(Result.Success(selector(value1, value2, value3, value4, value5, value6, value7))),
            e => new ValueTask<Result<TOut>>(Result.Failure<TOut>(e))
        );
    }

    public static ValueTask<Result<TOut>> Select<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TOut>(
        this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>> awaitableSource,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TOut>             selector)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TOut : notnull {
        return awaitableSource.MatchAsync<Result<TOut>, TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
            (value1,
             value2,
             value3,
             value4,
             value5,
             value6,
             value7) => ValueTask.FromResult(Result.Success(selector(value1, value2, value3, value4, value5, value6, value7))),
            e => new ValueTask<Result<TOut>>(Result.Failure<TOut>(e))
        );
    }

    public static ValueTask<Result<TOut>> Select<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TOut>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> source,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TOut>  selector)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull
        where TOut : notnull {
        return source.Match<ValueTask<Result<TOut>>>(
            (value1,
             value2,
             value3,
             value4,
             value5,
             value6,
             value7,
             value8) => ValueTask.FromResult(Result.Success(selector(value1, value2, value3, value4, value5, value6, value7, value8))),
            e => new ValueTask<Result<TOut>>(Result.Failure<TOut>(e))
        );
    }

    public static ValueTask<Result<TOut>> Select<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TOut>(
        this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>> awaitableSource,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TOut>             selector)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull
        where TOut : notnull {
        return awaitableSource.MatchAsync<Result<TOut>, TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
            (value1,
             value2,
             value3,
             value4,
             value5,
             value6,
             value7,
             value8) => ValueTask.FromResult(Result.Success(selector(value1, value2, value3, value4, value5, value6, value7, value8))),
            e => new ValueTask<Result<TOut>>(Result.Failure<TOut>(e))
        );
    }
}
