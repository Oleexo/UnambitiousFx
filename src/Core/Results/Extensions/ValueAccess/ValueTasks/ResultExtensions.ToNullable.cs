namespace UnambitiousFx.Core.Results.Extensions.ValueAccess.ValueTasks;

public static class ResultToNullableExtensions {
    public static async ValueTask<TValue1?> ToNullableAsync<TValue1>(this ValueTask<Result<TValue1>> awaitable)
        where TValue1 : notnull {
        var result = await awaitable.ConfigureAwait(false);
        return result.ToNullable();
    }

    public static async ValueTask<(TValue1, TValue2)?> ToNullableAsync<TValue1, TValue2>(this ValueTask<Result<TValue1, TValue2>> awaitable)
        where TValue1 : notnull
        where TValue2 : notnull {
        var result = await awaitable.ConfigureAwait(false);
        return result.ToNullable();
    }

    public static async ValueTask<(TValue1, TValue2, TValue3)?> ToNullableAsync<TValue1, TValue2, TValue3>(this ValueTask<Result<TValue1, TValue2, TValue3>> awaitable)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        var result = await awaitable.ConfigureAwait(false);
        return result.ToNullable();
    }

    public static async ValueTask<(TValue1, TValue2, TValue3, TValue4)?> ToNullableAsync<TValue1, TValue2, TValue3, TValue4>(
        this ValueTask<Result<TValue1, TValue2, TValue3, TValue4>> awaitable)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        var result = await awaitable.ConfigureAwait(false);
        return result.ToNullable();
    }

    public static async ValueTask<(TValue1, TValue2, TValue3, TValue4, TValue5)?> ToNullableAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(
        this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> awaitable)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        var result = await awaitable.ConfigureAwait(false);
        return result.ToNullable();
    }

    public static async ValueTask<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)?> ToNullableAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>> awaitable)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull {
        var result = await awaitable.ConfigureAwait(false);
        return result.ToNullable();
    }

    public static async ValueTask<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)?> ToNullableAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>> awaitable)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull {
        var result = await awaitable.ConfigureAwait(false);
        return result.ToNullable();
    }

    public static async ValueTask<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)?>
        ToNullableAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
            this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>> awaitable)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull {
        var result = await awaitable.ConfigureAwait(false);
        return result.ToNullable();
    }
}
