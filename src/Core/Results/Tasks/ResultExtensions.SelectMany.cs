namespace UnambitiousFx.Core.Results.Tasks;

public static partial class ResultExtensions {
    public static Task<Result<TResult>> SelectMany<TValue1, TCollection, TResult>(this Result<TValue1>                     source,
                                                                                  Func<TValue1, Task<Result<TCollection>>> collectionSelector,
                                                                                  Func<TValue1, TCollection, TResult>      resultSelector)
        where TValue1 : notnull
        where TCollection : notnull
        where TResult : notnull {
        return source.Match<Task<Result<TResult>>>(async value1 => {
            var inner = await collectionSelector(value1);
            return inner.Map(c => resultSelector(value1, c));
        }, e => Task.FromResult<Result<TResult>>(Result.Failure<TResult>(e)));
    }

    public static Task<Result<TResult>> SelectMany<TValue1, TCollection, TResult>(this Task<Result<TValue1>>               awaitableSource,
                                                                                  Func<TValue1, Task<Result<TCollection>>> collectionSelector,
                                                                                  Func<TValue1, TCollection, TResult>      resultSelector)
        where TValue1 : notnull
        where TCollection : notnull
        where TResult : notnull {
        return awaitableSource.MatchAsync<Result<TResult>, TValue1>(async value1 => {
            var inner = await collectionSelector(value1);
            return inner.Map(c => resultSelector(value1, c));
        }, e => Task.FromResult<Result<TResult>>(Result.Failure<TResult>(e)));
    }

    public static Task<Result<TResult>> SelectMany<TValue1, TValue2, TCollection, TResult>(this Result<TValue1, TValue2>                     source,
                                                                                           Func<TValue1, TValue2, Task<Result<TCollection>>> collectionSelector,
                                                                                           Func<TValue1, TValue2, TCollection, TResult>      resultSelector)
        where TValue1 : notnull
        where TValue2 : notnull
        where TCollection : notnull
        where TResult : notnull {
        return source.Match<Task<Result<TResult>>>(async (value1,
                                                          value2) => {
            var inner = await collectionSelector(value1, value2);
            return inner.Map(c => resultSelector(value1, value2, c));
        }, e => Task.FromResult<Result<TResult>>(Result.Failure<TResult>(e)));
    }

    public static Task<Result<TResult>> SelectMany<TValue1, TValue2, TCollection, TResult>(this Task<Result<TValue1, TValue2>>               awaitableSource,
                                                                                           Func<TValue1, TValue2, Task<Result<TCollection>>> collectionSelector,
                                                                                           Func<TValue1, TValue2, TCollection, TResult>      resultSelector)
        where TValue1 : notnull
        where TValue2 : notnull
        where TCollection : notnull
        where TResult : notnull {
        return awaitableSource.MatchAsync<Result<TResult>, TValue1, TValue2>(async (value1,
                                                                                    value2) => {
            var inner = await collectionSelector(value1, value2);
            return inner.Map(c => resultSelector(value1, value2, c));
        }, e => Task.FromResult<Result<TResult>>(Result.Failure<TResult>(e)));
    }

    public static Task<Result<TResult>> SelectMany<TValue1, TValue2, TValue3, TCollection, TResult>(this Result<TValue1, TValue2, TValue3>                     source,
                                                                                                    Func<TValue1, TValue2, TValue3, Task<Result<TCollection>>> collectionSelector,
                                                                                                    Func<TValue1, TValue2, TValue3, TCollection, TResult>      resultSelector)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TCollection : notnull
        where TResult : notnull {
        return source.Match<Task<Result<TResult>>>(async (value1,
                                                          value2,
                                                          value3) => {
            var inner = await collectionSelector(value1, value2, value3);
            return inner.Map(c => resultSelector(value1, value2, value3, c));
        }, e => Task.FromResult<Result<TResult>>(Result.Failure<TResult>(e)));
    }

    public static Task<Result<TResult>> SelectMany<TValue1, TValue2, TValue3, TCollection, TResult>(this Task<Result<TValue1, TValue2, TValue3>>               awaitableSource,
                                                                                                    Func<TValue1, TValue2, TValue3, Task<Result<TCollection>>> collectionSelector,
                                                                                                    Func<TValue1, TValue2, TValue3, TCollection, TResult>      resultSelector)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TCollection : notnull
        where TResult : notnull {
        return awaitableSource.MatchAsync<Result<TResult>, TValue1, TValue2, TValue3>(async (value1,
                                                                                             value2,
                                                                                             value3) => {
            var inner = await collectionSelector(value1, value2, value3);
            return inner.Map(c => resultSelector(value1, value2, value3, c));
        }, e => Task.FromResult<Result<TResult>>(Result.Failure<TResult>(e)));
    }

    public static Task<Result<TResult>> SelectMany<TValue1, TValue2, TValue3, TValue4, TCollection, TResult>(this Result<TValue1, TValue2, TValue3, TValue4> source,
                                                                                                             Func<TValue1, TValue2, TValue3, TValue4, Task<Result<TCollection>>>
                                                                                                                 collectionSelector,
                                                                                                             Func<TValue1, TValue2, TValue3, TValue4, TCollection, TResult>
                                                                                                                 resultSelector)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TCollection : notnull
        where TResult : notnull {
        return source.Match<Task<Result<TResult>>>(async (value1,
                                                          value2,
                                                          value3,
                                                          value4) => {
            var inner = await collectionSelector(value1, value2, value3, value4);
            return inner.Map(c => resultSelector(value1, value2, value3, value4, c));
        }, e => Task.FromResult<Result<TResult>>(Result.Failure<TResult>(e)));
    }

    public static Task<Result<TResult>> SelectMany<TValue1, TValue2, TValue3, TValue4, TCollection, TResult>(this Task<Result<TValue1, TValue2, TValue3, TValue4>> awaitableSource,
                                                                                                             Func<TValue1, TValue2, TValue3, TValue4, Task<Result<TCollection>>>
                                                                                                                 collectionSelector,
                                                                                                             Func<TValue1, TValue2, TValue3, TValue4, TCollection, TResult>
                                                                                                                 resultSelector)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TCollection : notnull
        where TResult : notnull {
        return awaitableSource.MatchAsync<Result<TResult>, TValue1, TValue2, TValue3, TValue4>(async (value1,
                                                                                                      value2,
                                                                                                      value3,
                                                                                                      value4) => {
            var inner = await collectionSelector(value1, value2, value3, value4);
            return inner.Map(c => resultSelector(value1, value2, value3, value4, c));
        }, e => Task.FromResult<Result<TResult>>(Result.Failure<TResult>(e)));
    }

    public static Task<Result<TResult>> SelectMany<TValue1, TValue2, TValue3, TValue4, TValue5, TCollection, TResult>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5>                     source,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, Task<Result<TCollection>>> collectionSelector,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TCollection, TResult>      resultSelector)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TCollection : notnull
        where TResult : notnull {
        return source.Match<Task<Result<TResult>>>(async (value1,
                                                          value2,
                                                          value3,
                                                          value4,
                                                          value5) => {
            var inner = await collectionSelector(value1, value2, value3, value4, value5);
            return inner.Map(c => resultSelector(value1, value2, value3, value4, value5, c));
        }, e => Task.FromResult<Result<TResult>>(Result.Failure<TResult>(e)));
    }

    public static Task<Result<TResult>> SelectMany<TValue1, TValue2, TValue3, TValue4, TValue5, TCollection, TResult>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5>>               awaitableSource,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, Task<Result<TCollection>>> collectionSelector,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TCollection, TResult>      resultSelector)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TCollection : notnull
        where TResult : notnull {
        return awaitableSource.MatchAsync<Result<TResult>, TValue1, TValue2, TValue3, TValue4, TValue5>(async (value1,
                                                                                                               value2,
                                                                                                               value3,
                                                                                                               value4,
                                                                                                               value5) => {
            var inner = await collectionSelector(value1, value2, value3, value4, value5);
            return inner.Map(c => resultSelector(value1, value2, value3, value4, value5, c));
        }, e => Task.FromResult<Result<TResult>>(Result.Failure<TResult>(e)));
    }

    public static Task<Result<TResult>> SelectMany<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TCollection, TResult>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>                     source,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Task<Result<TCollection>>> collectionSelector,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TCollection, TResult>      resultSelector)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TCollection : notnull
        where TResult : notnull {
        return source.Match<Task<Result<TResult>>>(async (value1,
                                                          value2,
                                                          value3,
                                                          value4,
                                                          value5,
                                                          value6) => {
            var inner = await collectionSelector(value1, value2, value3, value4, value5, value6);
            return inner.Map(c => resultSelector(value1, value2, value3, value4, value5, value6, c));
        }, e => Task.FromResult<Result<TResult>>(Result.Failure<TResult>(e)));
    }

    public static Task<Result<TResult>> SelectMany<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TCollection, TResult>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>>               awaitableSource,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Task<Result<TCollection>>> collectionSelector,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TCollection, TResult>      resultSelector)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TCollection : notnull
        where TResult : notnull {
        return awaitableSource.MatchAsync<Result<TResult>, TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(async (value1,
                                                                                                                     value2,
                                                                                                                     value3,
                                                                                                                     value4,
                                                                                                                     value5,
                                                                                                                     value6) => {
                                                                                                                     var inner = await collectionSelector(value1, value2, value3,
                                                                                                                         value4, value5, value6);
                                                                                                                     return inner.Map(c => resultSelector(value1, value2, value3,
                                                                                                                         value4, value5, value6, c));
                                                                                                                 },
                                                                                                                 e => Task.FromResult<Result<TResult>>(Result.Failure<TResult>(e)));
    }

    public static Task<Result<TResult>> SelectMany<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TCollection, TResult>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>                     source,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Task<Result<TCollection>>> collectionSelector,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TCollection, TResult>      resultSelector)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TCollection : notnull
        where TResult : notnull {
        return source.Match<Task<Result<TResult>>>(async (value1,
                                                          value2,
                                                          value3,
                                                          value4,
                                                          value5,
                                                          value6,
                                                          value7) => {
            var inner = await collectionSelector(value1, value2, value3, value4, value5, value6, value7);
            return inner.Map(c => resultSelector(value1, value2, value3, value4, value5, value6, value7, c));
        }, e => Task.FromResult<Result<TResult>>(Result.Failure<TResult>(e)));
    }

    public static Task<Result<TResult>> SelectMany<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TCollection, TResult>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>>               awaitableSource,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Task<Result<TCollection>>> collectionSelector,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TCollection, TResult>      resultSelector)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TCollection : notnull
        where TResult : notnull {
        return awaitableSource.MatchAsync<Result<TResult>, TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(async (value1,
            value2,
            value3,
            value4,
            value5,
            value6,
            value7) => {
            var inner = await collectionSelector(value1, value2, value3, value4, value5, value6, value7);
            return inner.Map(c => resultSelector(value1, value2, value3, value4, value5, value6, value7, c));
        }, e => Task.FromResult<Result<TResult>>(Result.Failure<TResult>(e)));
    }

    public static Task<Result<TResult>> SelectMany<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TCollection, TResult>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>                     source,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Task<Result<TCollection>>> collectionSelector,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TCollection, TResult>      resultSelector)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull
        where TCollection : notnull
        where TResult : notnull {
        return source.Match<Task<Result<TResult>>>(async (value1,
                                                          value2,
                                                          value3,
                                                          value4,
                                                          value5,
                                                          value6,
                                                          value7,
                                                          value8) => {
            var inner = await collectionSelector(value1, value2, value3, value4, value5, value6, value7, value8);
            return inner.Map(c => resultSelector(value1, value2, value3, value4, value5, value6, value7, value8, c));
        }, e => Task.FromResult<Result<TResult>>(Result.Failure<TResult>(e)));
    }

    public static Task<Result<TResult>> SelectMany<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TCollection, TResult>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>>               awaitableSource,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Task<Result<TCollection>>> collectionSelector,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TCollection, TResult>      resultSelector)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull
        where TCollection : notnull
        where TResult : notnull {
        return awaitableSource.MatchAsync<Result<TResult>, TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(async (value1,
            value2,
            value3,
            value4,
            value5,
            value6,
            value7,
            value8) => {
            var inner = await collectionSelector(value1, value2, value3, value4, value5, value6, value7, value8);
            return inner.Map(c => resultSelector(value1, value2, value3, value4, value5, value6, value7, value8, c));
        }, e => Task.FromResult<Result<TResult>>(Result.Failure<TResult>(e)));
    }
}
