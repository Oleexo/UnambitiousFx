namespace UnambitiousFx.Core.Results.Extensions.Transformations.Tasks;

public static class ResultFlattenExtensions {
    public static async Task<Result<TValue1>> FlattenAsync<TValue1>(this Task<Result<Result<TValue1>>> awaitable)
        where TValue1 : notnull {
        var outer = await awaitable;
        return outer.Flatten();
    }

    public static async Task<Result<TValue1, TValue2>> FlattenAsync<TValue1, TValue2>(this Task<Result<Result<TValue1, TValue2>>> awaitable)
        where TValue1 : notnull
        where TValue2 : notnull {
        var outer = await awaitable;
        return outer.Flatten();
    }

    public static async Task<Result<TValue1, TValue2, TValue3>> FlattenAsync<TValue1, TValue2, TValue3>(this Task<Result<Result<TValue1, TValue2, TValue3>>> awaitable)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        var outer = await awaitable;
        return outer.Flatten();
    }

    public static async Task<Result<TValue1, TValue2, TValue3, TValue4>> FlattenAsync<TValue1, TValue2, TValue3, TValue4>(
        this Task<Result<Result<TValue1, TValue2, TValue3, TValue4>>> awaitable)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        var outer = await awaitable;
        return outer.Flatten();
    }

    public static async Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> FlattenAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(
        this Task<Result<Result<TValue1, TValue2, TValue3, TValue4, TValue5>>> awaitable)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        var outer = await awaitable;
        return outer.Flatten();
    }

    public static async Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>> FlattenAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        this Task<Result<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>>> awaitable)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull {
        var outer = await awaitable;
        return outer.Flatten();
    }

    public static async Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>> FlattenAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        this Task<Result<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>>> awaitable)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull {
        var outer = await awaitable;
        return outer.Flatten();
    }

    public static async Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>>
        FlattenAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
            this Task<Result<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>>> awaitable)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull {
        var outer = await awaitable;
        return outer.Flatten();
    }
}
