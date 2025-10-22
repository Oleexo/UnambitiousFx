namespace UnambitiousFx.Core.Results.Extensions.ErrorHandling.Tasks;

public static partial class ResultExtensions {
    public static async Task<Result<TValue>> RecoverAsync<TValue>(this Result<TValue>           result,
                                                                  Func<Exception, Task<TValue>> recover)
        where TValue : notnull {
        if (result.Ok(out var value, out var error)) {
            return Result.Success(value);
        }

        var fallback = await recover(error)
                          .ConfigureAwait(false);
        return Result.Success(fallback);
    }

    public static async Task<Result<TValue>> RecoverWithAsync<TValue>(this Result<TValue>                   result,
                                                                      Func<Exception, Task<Result<TValue>>> recover)
        where TValue : notnull {
        if (result.Ok(out var value, out var error)) {
            return Result.Success(value);
        }

        return await recover(error)
                  .ConfigureAwait(false);
    }
}
