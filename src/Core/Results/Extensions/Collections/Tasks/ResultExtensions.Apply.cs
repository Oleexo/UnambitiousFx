namespace UnambitiousFx.Core.Results.Extensions.Collections.Tasks;

public static partial class ResultExtensions {
    public static async Task<Result<TOut>> ApplyAsync<TIn, TOut>(this Task<Result<Func<TIn, TOut>>> awaitableResult,
                                                                 Result<TIn>                        result2)
        where TIn : notnull
        where TOut : notnull {
        var result1 = await awaitableResult.ConfigureAwait(false);
        return result1.Apply(result2);
    }

    public static async Task<Result<TOut>> ApplyAsync<TIn, TOut>(this Result<Func<TIn, TOut>> resultFunc,
                                                                 Task<Result<TIn>>            awaitableResult2)
        where TIn : notnull
        where TOut : notnull {
        var result2 = await awaitableResult2.ConfigureAwait(false);
        return resultFunc.Apply(result2);
    }

    public static async Task<Result<TOut>> ApplyAsync<TIn, TOut>(this Task<Result<Func<TIn, TOut>>> awaitableResultFunc,
                                                                 Task<Result<TIn>>                  awaitableResult2)
        where TIn : notnull
        where TOut : notnull {
        var resultFunc = await awaitableResultFunc.ConfigureAwait(false);
        var result2    = await awaitableResult2.ConfigureAwait(false);
        return resultFunc.Apply(result2);
    }
}
