namespace UnambitiousFx.Core.Results.ValueTasks;

public static partial class ResultExtensions {
    public static async ValueTask<Result<TValue>> RecoverAsync<TValue>(this Result<TValue>                result,
                                                                       Func<Exception, ValueTask<TValue>> recover)
        where TValue : notnull {
        if (result.Ok(out var value, out var error)) {
            return Result.Success(value);
        }

        var fallback = await recover(error)
                          .ConfigureAwait(false);
        return Result.Success(fallback);
    }

    public static async ValueTask<Result<TValue>> RecoverWithAsync<TValue>(this Result<TValue>                        result,
                                                                           Func<Exception, ValueTask<Result<TValue>>> recover)
        where TValue : notnull {
        if (result.Ok(out var value, out var error)) {
            return Result.Success(value);
        }

        return await recover(error)
                  .ConfigureAwait(false);
    }
}
