namespace UnambitiousFx.Core.Results.ValueTasks;

public static partial class ResultExtensions {
    public static async ValueTask<TValue1> ValueOr<TValue1>(this ValueTask<Result<TValue1>> awaitableResult,
                                                            TValue1                         fallback)
        where TValue1 : notnull {
        var result = await awaitableResult.ConfigureAwait(false);
        return result.Match<TValue1>(value1 => value1, _ => fallback);
    }

    public static async ValueTask<TValue1> ValueOr<TValue1>(this ValueTask<Result<TValue1>> awaitableResult,
                                                            Func<TValue1>                   fallbackFactory)
        where TValue1 : notnull {
        if (fallbackFactory is null) {
            throw new ArgumentNullException("fallbackFactory");
        }

        var result = await awaitableResult.ConfigureAwait(false);
        return result.Match<TValue1>(value1 => value1, _ => fallbackFactory());
    }

    public static async ValueTask<(TValue1, TValue2)> ValueOr<TValue1, TValue2>(this ValueTask<Result<TValue1, TValue2>> awaitableResult,
                                                                                (TValue1, TValue2)                       fallback)
        where TValue1 : notnull
        where TValue2 : notnull {
        var result = await awaitableResult.ConfigureAwait(false);
        return result.Match<(TValue1, TValue2)>((value1,
                                                 value2) => (value1, value2), _ => fallback);
    }

    public static async ValueTask<(TValue1, TValue2)> ValueOr<TValue1, TValue2>(this ValueTask<Result<TValue1, TValue2>> awaitableResult,
                                                                                Func<(TValue1, TValue2)>                 fallbackFactory)
        where TValue1 : notnull
        where TValue2 : notnull {
        if (fallbackFactory is null) {
            throw new ArgumentNullException("fallbackFactory");
        }

        var result = await awaitableResult.ConfigureAwait(false);
        return result.Match<(TValue1, TValue2)>((value1,
                                                 value2) => (value1, value2), _ => fallbackFactory());
    }

    public static async ValueTask<(TValue1, TValue2, TValue3)> ValueOr<TValue1, TValue2, TValue3>(this ValueTask<Result<TValue1, TValue2, TValue3>> awaitableResult,
                                                                                                  (TValue1, TValue2, TValue3)                       fallback)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        var result = await awaitableResult.ConfigureAwait(false);
        return result.Match<(TValue1, TValue2, TValue3)>((value1,
                                                          value2,
                                                          value3) => (value1, value2, value3), _ => fallback);
    }

    public static async ValueTask<(TValue1, TValue2, TValue3)> ValueOr<TValue1, TValue2, TValue3>(this ValueTask<Result<TValue1, TValue2, TValue3>> awaitableResult,
                                                                                                  Func<(TValue1, TValue2, TValue3)>                 fallbackFactory)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        if (fallbackFactory is null) {
            throw new ArgumentNullException("fallbackFactory");
        }

        var result = await awaitableResult.ConfigureAwait(false);
        return result.Match<(TValue1, TValue2, TValue3)>((value1,
                                                          value2,
                                                          value3) => (value1, value2, value3), _ => fallbackFactory());
    }

    public static async ValueTask<(TValue1, TValue2, TValue3, TValue4)> ValueOr<TValue1, TValue2, TValue3, TValue4>(
        this ValueTask<Result<TValue1, TValue2, TValue3, TValue4>> awaitableResult,
        (TValue1, TValue2, TValue3, TValue4)                       fallback)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        var result = await awaitableResult.ConfigureAwait(false);
        return result.Match<(TValue1, TValue2, TValue3, TValue4)>((value1,
                                                                   value2,
                                                                   value3,
                                                                   value4) => (value1, value2, value3, value4), _ => fallback);
    }

    public static async ValueTask<(TValue1, TValue2, TValue3, TValue4)> ValueOr<TValue1, TValue2, TValue3, TValue4>(
        this ValueTask<Result<TValue1, TValue2, TValue3, TValue4>> awaitableResult,
        Func<(TValue1, TValue2, TValue3, TValue4)>                 fallbackFactory)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        if (fallbackFactory is null) {
            throw new ArgumentNullException("fallbackFactory");
        }

        var result = await awaitableResult.ConfigureAwait(false);
        return result.Match<(TValue1, TValue2, TValue3, TValue4)>((value1,
                                                                   value2,
                                                                   value3,
                                                                   value4) => (value1, value2, value3, value4), _ => fallbackFactory());
    }

    public static async ValueTask<(TValue1, TValue2, TValue3, TValue4, TValue5)> ValueOr<TValue1, TValue2, TValue3, TValue4, TValue5>(
        this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> awaitableResult,
        (TValue1, TValue2, TValue3, TValue4, TValue5)                       fallback)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        var result = await awaitableResult.ConfigureAwait(false);
        return result.Match<(TValue1, TValue2, TValue3, TValue4, TValue5)>((value1,
                                                                            value2,
                                                                            value3,
                                                                            value4,
                                                                            value5) => (value1, value2, value3, value4, value5), _ => fallback);
    }

    public static async ValueTask<(TValue1, TValue2, TValue3, TValue4, TValue5)> ValueOr<TValue1, TValue2, TValue3, TValue4, TValue5>(
        this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> awaitableResult,
        Func<(TValue1, TValue2, TValue3, TValue4, TValue5)>                 fallbackFactory)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        if (fallbackFactory is null) {
            throw new ArgumentNullException("fallbackFactory");
        }

        var result = await awaitableResult.ConfigureAwait(false);
        return result.Match<(TValue1, TValue2, TValue3, TValue4, TValue5)>((value1,
                                                                            value2,
                                                                            value3,
                                                                            value4,
                                                                            value5) => (value1, value2, value3, value4, value5), _ => fallbackFactory());
    }

    public static async ValueTask<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)> ValueOr<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>> awaitableResult,
        (TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)                       fallback)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull {
        var result = await awaitableResult.ConfigureAwait(false);
        return result.Match<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>((value1,
                                                                                     value2,
                                                                                     value3,
                                                                                     value4,
                                                                                     value5,
                                                                                     value6) => (value1, value2, value3, value4, value5, value6), _ => fallback);
    }

    public static async ValueTask<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)> ValueOr<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>> awaitableResult,
        Func<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>                 fallbackFactory)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull {
        if (fallbackFactory is null) {
            throw new ArgumentNullException("fallbackFactory");
        }

        var result = await awaitableResult.ConfigureAwait(false);
        return result.Match<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>((value1,
                                                                                     value2,
                                                                                     value3,
                                                                                     value4,
                                                                                     value5,
                                                                                     value6) => (value1, value2, value3, value4, value5, value6), _ => fallbackFactory());
    }

    public static async ValueTask<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)> ValueOr<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>> awaitableResult,
        (TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)                       fallback)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull {
        var result = await awaitableResult.ConfigureAwait(false);
        return result.Match<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>((value1,
                                                                                              value2,
                                                                                              value3,
                                                                                              value4,
                                                                                              value5,
                                                                                              value6,
                                                                                              value7) => (value1, value2, value3, value4, value5, value6, value7), _ => fallback);
    }

    public static async ValueTask<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)> ValueOr<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>> awaitableResult,
        Func<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>                 fallbackFactory)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull {
        if (fallbackFactory is null) {
            throw new ArgumentNullException("fallbackFactory");
        }

        var result = await awaitableResult.ConfigureAwait(false);
        return result.Match<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>((value1,
                                                                                              value2,
                                                                                              value3,
                                                                                              value4,
                                                                                              value5,
                                                                                              value6,
                                                                                              value7) => (value1, value2, value3, value4, value5, value6, value7),
                                                                                             _ => fallbackFactory());
    }

    public static async ValueTask<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>
        ValueOr<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
            this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>> awaitableResult,
            (TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)                       fallback)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull {
        var result = await awaitableResult.ConfigureAwait(false);
        return result.Match<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>((value1,
                                                                                                       value2,
                                                                                                       value3,
                                                                                                       value4,
                                                                                                       value5,
                                                                                                       value6,
                                                                                                       value7,
                                                                                                       value8) => (value1, value2, value3, value4, value5, value6, value7, value8),
                                                                                                      _ => fallback);
    }

    public static async ValueTask<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>
        ValueOr<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
            this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>> awaitableResult,
            Func<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>                 fallbackFactory)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull {
        if (fallbackFactory is null) {
            throw new ArgumentNullException("fallbackFactory");
        }

        var result = await awaitableResult.ConfigureAwait(false);
        return result.Match<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>((value1,
                                                                                                       value2,
                                                                                                       value3,
                                                                                                       value4,
                                                                                                       value5,
                                                                                                       value6,
                                                                                                       value7,
                                                                                                       value8) => (value1, value2, value3, value4, value5, value6, value7, value8),
                                                                                                      _ => fallbackFactory());
    }
}
