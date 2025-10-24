namespace UnambitiousFx.Core.Results.Extensions.ValueAccess.ValueTasks;

public static class ResultTryGetExtensions {
    public static async ValueTask<(bool ok, TValue1? value)> TryGetAsync<TValue1>(this ValueTask<Result<TValue1>> awaitable)
        where TValue1 : notnull {
        var result = await awaitable.ConfigureAwait(false);
        return (result.Ok(out var value), value);
    }

    public static async ValueTask<(bool ok, (TValue1, TValue2) value)> TryGetAsync<TValue1, TValue2>(this ValueTask<Result<TValue1, TValue2>> awaitable)
        where TValue1 : notnull
        where TValue2 : notnull {
        var result = await awaitable.ConfigureAwait(false);
        return result.Ok(out var v1, out var v2)
                   ? (true, (v1, v2))
                   : (false, default);
    }

    public static async ValueTask<(bool ok, (TValue1, TValue2, TValue3) value)> TryGetAsync<TValue1, TValue2, TValue3>(this ValueTask<Result<TValue1, TValue2, TValue3>> awaitable)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        var result = await awaitable.ConfigureAwait(false);
        return result.Ok(out var v1, out var v2, out var v3)
                   ? (true, (v1, v2, v3))
                   : (false, default);
    }

    public static async ValueTask<(bool ok, (TValue1, TValue2, TValue3, TValue4) value)> TryGetAsync<TValue1, TValue2, TValue3, TValue4>(
        this ValueTask<Result<TValue1, TValue2, TValue3, TValue4>> awaitable)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        var result = await awaitable.ConfigureAwait(false);
        return result.Ok(out var v1, out var v2, out var v3, out var v4)
                   ? (true, (v1, v2, v3, v4))
                   : (false, default);
    }

    public static async ValueTask<(bool ok, (TValue1, TValue2, TValue3, TValue4, TValue5) value)> TryGetAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(
        this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> awaitable)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        var result = await awaitable.ConfigureAwait(false);
        return result.Ok(out var v1, out var v2, out var v3, out var v4, out var v5)
                   ? (true, (v1, v2, v3, v4, v5))
                   : (false, default);
    }

    public static async ValueTask<(bool ok, (TValue1, TValue2, TValue3, TValue4, TValue5, TValue6) value)> TryGetAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>> awaitable)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull {
        var result = await awaitable.ConfigureAwait(false);
        return result.Ok(out var v1, out var v2, out var v3, out var v4, out var v5, out var v6)
                   ? (true, (v1, v2, v3, v4, v5, v6))
                   : (false, default);
    }

    public static async ValueTask<(bool ok, (TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7) value)>
        TryGetAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>> awaitable)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull {
        var result = await awaitable.ConfigureAwait(false);
        return result.Ok(out var v1, out var v2, out var v3, out var v4, out var v5, out var v6, out var v7)
                   ? (true, (v1, v2, v3, v4, v5, v6, v7))
                   : (false, default);
    }

    public static async ValueTask<(bool ok, (TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8) value)>
        TryGetAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
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
        return result.Ok(out var v1, out var v2, out var v3, out var v4, out var v5, out var v6, out var v7, out var v8)
                   ? (true, (v1, v2, v3, v4, v5, v6, v7, v8))
                   : (false, default);
    }
}
