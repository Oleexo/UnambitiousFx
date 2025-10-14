namespace UnambitiousFx.Core.Results.Tasks;

public static partial class ResultExtensions {
    public static async Task<Result> FromTask(Task task) {
        try {
            await task;
            return Result.Success();
        }
        catch (Exception ex) {
            return Result.Failure(ex);
        }
    }

    public static Task<Result> ToTask(this Result result) {
        return Task.FromResult<Result>(result);
    }

    public static async Task<Result<TValue>> FromTask<TValue>(Task<TValue> task)
        where TValue : notnull {
        try {
            var value = await task;
            return Result.Success(value);
        }
        catch (Exception ex) {
            return Result.Failure<TValue>(ex);
        }
    }

    public static Task<Result<TValue>> ToTask<TValue>(this Result<TValue> result)
        where TValue : notnull {
        return Task.FromResult<Result<TValue>>(result);
    }

    public static async Task<Result<TValue1, TValue2>> FromTask<TValue1, TValue2>(Task<(TValue1, TValue2)> task)
        where TValue1 : notnull
        where TValue2 : notnull {
        try {
            var items = await task;
            return Result.Success(items.Item1, items.Item2);
        }
        catch (Exception ex) {
            return Result.Failure<TValue1, TValue2>(ex);
        }
    }

    public static Task<Result<TValue1, TValue2>> ToTask<TValue1, TValue2>(this Result<TValue1, TValue2> result)
        where TValue1 : notnull
        where TValue2 : notnull {
        return Task.FromResult<Result<TValue1, TValue2>>(result);
    }

    public static async Task<Result<TValue1, TValue2, TValue3>> FromTask<TValue1, TValue2, TValue3>(Task<(TValue1, TValue2, TValue3)> task)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        try {
            var items = await task;
            return Result.Success(items.Item1, items.Item2, items.Item3);
        }
        catch (Exception ex) {
            return Result.Failure<TValue1, TValue2, TValue3>(ex);
        }
    }

    public static Task<Result<TValue1, TValue2, TValue3>> ToTask<TValue1, TValue2, TValue3>(this Result<TValue1, TValue2, TValue3> result)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        return Task.FromResult<Result<TValue1, TValue2, TValue3>>(result);
    }

    public static async Task<Result<TValue1, TValue2, TValue3, TValue4>> FromTask<TValue1, TValue2, TValue3, TValue4>(Task<(TValue1, TValue2, TValue3, TValue4)> task)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        try {
            var items = await task;
            return Result.Success(items.Item1, items.Item2, items.Item3, items.Item4);
        }
        catch (Exception ex) {
            return Result.Failure<TValue1, TValue2, TValue3, TValue4>(ex);
        }
    }

    public static Task<Result<TValue1, TValue2, TValue3, TValue4>> ToTask<TValue1, TValue2, TValue3, TValue4>(this Result<TValue1, TValue2, TValue3, TValue4> result)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        return Task.FromResult<Result<TValue1, TValue2, TValue3, TValue4>>(result);
    }

    public static async Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> FromTask<TValue1, TValue2, TValue3, TValue4, TValue5>(
        Task<(TValue1, TValue2, TValue3, TValue4, TValue5)> task)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        try {
            var items = await task;
            return Result.Success(items.Item1, items.Item2, items.Item3, items.Item4, items.Item5);
        }
        catch (Exception ex) {
            return Result.Failure<TValue1, TValue2, TValue3, TValue4, TValue5>(ex);
        }
    }

    public static Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> ToTask<TValue1, TValue2, TValue3, TValue4, TValue5>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5> result)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        return Task.FromResult<Result<TValue1, TValue2, TValue3, TValue4, TValue5>>(result);
    }

    public static async Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>> FromTask<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        Task<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)> task)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull {
        try {
            var items = await task;
            return Result.Success(items.Item1, items.Item2, items.Item3, items.Item4, items.Item5, items.Item6);
        }
        catch (Exception ex) {
            return Result.Failure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(ex);
        }
    }

    public static Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>> ToTask<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> result)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull {
        return Task.FromResult<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>>(result);
    }

    public static async Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>> FromTask<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        Task<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)> task)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull {
        try {
            var items = await task;
            return Result.Success(items.Item1, items.Item2, items.Item3, items.Item4, items.Item5, items.Item6, items.Item7);
        }
        catch (Exception ex) {
            return Result.Failure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(ex);
        }
    }

    public static Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>> ToTask<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> result)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull {
        return Task.FromResult<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>>(result);
    }

    public static async Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>>
        FromTask<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(Task<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)> task)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull {
        try {
            var items = await task;
            return Result.Success(items.Item1, items.Item2, items.Item3, items.Item4, items.Item5, items.Item6, items.Item7, items.Item8);
        }
        catch (Exception ex) {
            return Result.Failure<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(ex);
        }
    }

    public static Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>>
        ToTask<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> result)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull {
        return Task.FromResult<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>>(result);
    }
}
