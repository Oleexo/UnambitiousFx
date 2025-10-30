namespace UnambitiousFx.Core.Results.Extensions.Transformations.Tasks;

public static partial class ResultThenExtensions
{
    public static async Task<Result<T1>> ThenAsync<T1>(this Result<T1> result,
                                                       Func<T1, Task<Result<T1>>> then,
                                                       bool copyReasonsAndMetadata = true)
        where T1 : notnull
    {
        if (!result.TryGet(out var value))
        {
            return result;
        }

        var response = await then(value);
        if (copyReasonsAndMetadata)
        {
            result.CopyReasonsAndMetadata(response);
        }

        return response;
    }

    public static async Task<Result<T1>> ThenAsync<T1>(this Task<Result<T1>> awaitable,
                                                       Func<T1, Result<T1>> then,
                                                       bool copyReasonsAndMetadata = true)
        where T1 : notnull
    {
        var result = await awaitable;
        return result.Then(then, copyReasonsAndMetadata);
    }

    public static async Task<Result<T1>> ThenAsync<T1>(this Task<Result<T1>> awaitable,
                                                       Func<T1, Task<Result<T1>>> then,
                                                       bool copyReasonsAndMetadata = true)
        where T1 : notnull
    {
        var result = await awaitable;
        return await result.ThenAsync(then, copyReasonsAndMetadata);
    }

    public static async Task<Result<T1, T2>> ThenAsync<T1, T2>(this Result<T1, T2> result,
                                                               Func<T1, T2, Task<Result<T1, T2>>> then,
                                                               bool copyReasonsAndMetadata = true)
        where T1 : notnull
        where T2 : notnull
    {
        if (!result.TryGet(out var value1, out var value2))
        {
            return result;
        }

        var response = await then(value1, value2);
        if (copyReasonsAndMetadata)
        {
            result.CopyReasonsAndMetadata(response);
        }

        return response;
    }

    public static async Task<Result<T1, T2>> ThenAsync<T1, T2>(this Task<Result<T1, T2>> awaitable,
                                                               Func<T1, T2, Result<T1, T2>> then,
                                                               bool copyReasonsAndMetadata = true)
        where T1 : notnull
        where T2 : notnull
    {
        var result = await awaitable;
        return result.Then(then, copyReasonsAndMetadata);
    }

    public static async Task<Result<T1, T2>> ThenAsync<T1, T2>(this Task<Result<T1, T2>> awaitable,
                                                               Func<T1, T2, Task<Result<T1, T2>>> then,
                                                               bool copyReasonsAndMetadata = true)
        where T1 : notnull
        where T2 : notnull
    {
        var result = await awaitable;
        return await result.ThenAsync(then, copyReasonsAndMetadata);
    }

    public static async Task<Result<T1, T2, T3>> ThenAsync<T1, T2, T3>(this Result<T1, T2, T3> result,
                                                                       Func<T1, T2, T3, Task<Result<T1, T2, T3>>> then,
                                                                       bool copyReasonsAndMetadata = true)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
    {
        if (!result.TryGet(out var value1, out var value2, out var value3))
        {
            return result;
        }

        var response = await then(value1, value2, value3);
        if (copyReasonsAndMetadata)
        {
            result.CopyReasonsAndMetadata(response);
        }

        return response;
    }

    public static async Task<Result<T1, T2, T3>> ThenAsync<T1, T2, T3>(this Task<Result<T1, T2, T3>> awaitable,
                                                                       Func<T1, T2, T3, Result<T1, T2, T3>> then,
                                                                       bool copyReasonsAndMetadata = true)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
    {
        var result = await awaitable;
        return result.Then(then, copyReasonsAndMetadata);
    }

    public static async Task<Result<T1, T2, T3>> ThenAsync<T1, T2, T3>(this Task<Result<T1, T2, T3>> awaitable,
                                                                       Func<T1, T2, T3, Task<Result<T1, T2, T3>>> then,
                                                                       bool copyReasonsAndMetadata = true)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
    {
        var result = await awaitable;
        return await result.ThenAsync(then, copyReasonsAndMetadata);
    }

    public static async Task<Result<T1, T2, T3, T4>> ThenAsync<T1, T2, T3, T4>(this Result<T1, T2, T3, T4> result,
                                                                               Func<T1, T2, T3, T4, Task<Result<T1, T2, T3, T4>>> then,
                                                                               bool copyReasonsAndMetadata = true)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
    {
        if (!result.TryGet(out var value1, out var value2, out var value3, out var value4))
        {
            return result;
        }

        var response = await then(value1, value2, value3, value4);
        if (copyReasonsAndMetadata)
        {
            result.CopyReasonsAndMetadata(response);
        }

        return response;
    }

    public static async Task<Result<T1, T2, T3, T4>> ThenAsync<T1, T2, T3, T4>(this Task<Result<T1, T2, T3, T4>> awaitable,
                                                                               Func<T1, T2, T3, T4, Result<T1, T2, T3, T4>> then,
                                                                               bool copyReasonsAndMetadata = true)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
    {
        var result = await awaitable;
        return result.Then(then, copyReasonsAndMetadata);
    }

    public static async Task<Result<T1, T2, T3, T4>> ThenAsync<T1, T2, T3, T4>(this Task<Result<T1, T2, T3, T4>> awaitable,
                                                                               Func<T1, T2, T3, T4, Task<Result<T1, T2, T3, T4>>> then,
                                                                               bool copyReasonsAndMetadata = true)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
    {
        var result = await awaitable;
        return await result.ThenAsync(then, copyReasonsAndMetadata);
    }

    public static async Task<Result<T1, T2, T3, T4, T5>> ThenAsync<T1, T2, T3, T4, T5>(this Result<T1, T2, T3, T4, T5> result,
                                                                                       Func<T1, T2, T3, T4, T5, Task<Result<T1, T2, T3, T4, T5>>> then,
                                                                                       bool copyReasonsAndMetadata = true)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
    {
        if (!result.TryGet(out var value1, out var value2, out var value3, out var value4, out var value5))
        {
            return result;
        }

        var response = await then(value1, value2, value3, value4, value5);
        if (copyReasonsAndMetadata)
        {
            result.CopyReasonsAndMetadata(response);
        }

        return response;
    }

    public static async Task<Result<T1, T2, T3, T4, T5>> ThenAsync<T1, T2, T3, T4, T5>(this Task<Result<T1, T2, T3, T4, T5>> awaitable,
                                                                                       Func<T1, T2, T3, T4, T5, Result<T1, T2, T3, T4, T5>> then,
                                                                                       bool copyReasonsAndMetadata = true)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
    {
        var result = await awaitable;
        return result.Then(then, copyReasonsAndMetadata);
    }

    public static async Task<Result<T1, T2, T3, T4, T5>> ThenAsync<T1, T2, T3, T4, T5>(this Task<Result<T1, T2, T3, T4, T5>> awaitable,
                                                                                       Func<T1, T2, T3, T4, T5, Task<Result<T1, T2, T3, T4, T5>>> then,
                                                                                       bool copyReasonsAndMetadata = true)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
    {
        var result = await awaitable;
        return await result.ThenAsync(then, copyReasonsAndMetadata);
    }

    public static async Task<Result<T1, T2, T3, T4, T5, T6>> ThenAsync<T1, T2, T3, T4, T5, T6>(this Result<T1, T2, T3, T4, T5, T6> result,
                                                                                               Func<T1, T2, T3, T4, T5, T6, Task<Result<T1, T2, T3, T4, T5, T6>>> then,
                                                                                               bool copyReasonsAndMetadata = true)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
    {
        if (!result.TryGet(out var value1, out var value2, out var value3, out var value4, out var value5, out var value6))
        {
            return result;
        }

        var response = await then(value1, value2, value3, value4, value5, value6);
        if (copyReasonsAndMetadata)
        {
            result.CopyReasonsAndMetadata(response);
        }

        return response;
    }

    public static async Task<Result<T1, T2, T3, T4, T5, T6>> ThenAsync<T1, T2, T3, T4, T5, T6>(this Task<Result<T1, T2, T3, T4, T5, T6>> awaitable,
                                                                                               Func<T1, T2, T3, T4, T5, T6, Result<T1, T2, T3, T4, T5, T6>> then,
                                                                                               bool copyReasonsAndMetadata = true)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
    {
        var result = await awaitable;
        return result.Then(then, copyReasonsAndMetadata);
    }

    public static async Task<Result<T1, T2, T3, T4, T5, T6>> ThenAsync<T1, T2, T3, T4, T5, T6>(this Task<Result<T1, T2, T3, T4, T5, T6>> awaitable,
                                                                                               Func<T1, T2, T3, T4, T5, T6, Task<Result<T1, T2, T3, T4, T5, T6>>> then,
                                                                                               bool copyReasonsAndMetadata = true)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
    {
        var result = await awaitable;
        return await result.ThenAsync(then, copyReasonsAndMetadata);
    }

    public static async Task<Result<T1, T2, T3, T4, T5, T6, T7>> ThenAsync<T1, T2, T3, T4, T5, T6, T7>(this Result<T1, T2, T3, T4, T5, T6, T7> result,
                                                                                                       Func<T1, T2, T3, T4, T5, T6, T7, Task<Result<T1, T2, T3, T4, T5, T6, T7>>>
                                                                                                           then,
                                                                                                       bool copyReasonsAndMetadata = true)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
        where T7 : notnull
    {
        if (!result.TryGet(out var value1, out var value2, out var value3, out var value4, out var value5, out var value6, out var value7))
        {
            return result;
        }

        var response = await then(value1, value2, value3, value4, value5, value6, value7);
        if (copyReasonsAndMetadata)
        {
            result.CopyReasonsAndMetadata(response);
        }

        return response;
    }

    public static async Task<Result<T1, T2, T3, T4, T5, T6, T7>> ThenAsync<T1, T2, T3, T4, T5, T6, T7>(this Task<Result<T1, T2, T3, T4, T5, T6, T7>> awaitable,
                                                                                                       Func<T1, T2, T3, T4, T5, T6, T7, Result<T1, T2, T3, T4, T5, T6, T7>> then,
                                                                                                       bool copyReasonsAndMetadata = true)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
        where T7 : notnull
    {
        var result = await awaitable;
        return result.Then(then, copyReasonsAndMetadata);
    }

    public static async Task<Result<T1, T2, T3, T4, T5, T6, T7>> ThenAsync<T1, T2, T3, T4, T5, T6, T7>(this Task<Result<T1, T2, T3, T4, T5, T6, T7>> awaitable,
                                                                                                       Func<T1, T2, T3, T4, T5, T6, T7, Task<Result<T1, T2, T3, T4, T5, T6, T7>>>
                                                                                                           then,
                                                                                                       bool copyReasonsAndMetadata = true)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
        where T7 : notnull
    {
        var result = await awaitable;
        return await result.ThenAsync(then, copyReasonsAndMetadata);
    }

    public static async Task<Result<T1, T2, T3, T4, T5, T6, T7, T8>> ThenAsync<T1, T2, T3, T4, T5, T6, T7, T8>(this Result<T1, T2, T3, T4, T5, T6, T7, T8> result,
                                                                                                               Func<T1, T2, T3, T4, T5, T6, T7, T8,
                                                                                                                   Task<Result<T1, T2, T3, T4, T5, T6, T7, T8>>> then,
                                                                                                               bool copyReasonsAndMetadata = true)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
        where T7 : notnull
        where T8 : notnull
    {
        if (!result.TryGet(out var value1, out var value2, out var value3, out var value4, out var value5, out var value6, out var value7, out var value8))
        {
            return result;
        }

        var response = await then(value1, value2, value3, value4, value5, value6, value7, value8);
        if (copyReasonsAndMetadata)
        {
            result.CopyReasonsAndMetadata(response);
        }

        return response;
    }

    public static async Task<Result<T1, T2, T3, T4, T5, T6, T7, T8>> ThenAsync<T1, T2, T3, T4, T5, T6, T7, T8>(this Task<Result<T1, T2, T3, T4, T5, T6, T7, T8>> awaitable,
                                                                                                               Func<T1, T2, T3, T4, T5, T6, T7, T8,
                                                                                                                   Result<T1, T2, T3, T4, T5, T6, T7, T8>> then,
                                                                                                               bool copyReasonsAndMetadata = true)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
        where T7 : notnull
        where T8 : notnull
    {
        var result = await awaitable;
        return result.Then(then, copyReasonsAndMetadata);
    }

    public static async Task<Result<T1, T2, T3, T4, T5, T6, T7, T8>> ThenAsync<T1, T2, T3, T4, T5, T6, T7, T8>(this Task<Result<T1, T2, T3, T4, T5, T6, T7, T8>> awaitable,
                                                                                                               Func<T1, T2, T3, T4, T5, T6, T7, T8,
                                                                                                                   Task<Result<T1, T2, T3, T4, T5, T6, T7, T8>>> then,
                                                                                                               bool copyReasonsAndMetadata = true)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
        where T7 : notnull
        where T8 : notnull
    {
        var result = await awaitable;
        return await result.ThenAsync(then, copyReasonsAndMetadata);
    }
}
