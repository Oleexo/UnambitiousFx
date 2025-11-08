namespace UnambitiousFx.Core.Results.Extensions.Collections.ValueTasks;

public static partial class ResultExtensions
{
    public static async ValueTask<Result<TOut>> ApplyAsync<TIn, TOut>(this ValueTask<Result<Func<TIn, TOut>>> rf,
                                                                      Result<TIn> ra)
        where TIn : notnull
        where TOut : notnull
    {
        var rfResolved = await rf.ConfigureAwait(false);
        if (!rfResolved.TryGet(out var f, out var fErr))
        {
            return Result.Failure<TOut>(fErr);
        }

        if (!ra.TryGet(out var a, out var aErr))
        {
            return Result.Failure<TOut>(aErr);
        }

        return Result.Success(f(a));
    }

    public static async ValueTask<Result<TOut>> ApplyAsync<TIn, TOut>(this Result<Func<TIn, TOut>> rf,
                                                                      ValueTask<Result<TIn>> ra)
        where TIn : notnull
        where TOut : notnull
    {
        if (!rf.TryGet(out var f, out var fErr))
        {
            return Result.Failure<TOut>(fErr);
        }

        var raResolved = await ra.ConfigureAwait(false);
        if (!raResolved.TryGet(out var a, out var aErr))
        {
            return Result.Failure<TOut>(aErr);
        }

        return Result.Success(f(a));
    }

    public static async ValueTask<Result<TOut>> ApplyAsync<TIn, TOut>(this ValueTask<Result<Func<TIn, TOut>>> rf,
                                                                      ValueTask<Result<TIn>> ra)
        where TIn : notnull
        where TOut : notnull
    {
        var rfResolved = await rf.ConfigureAwait(false);
        if (!rfResolved.TryGet(out var f, out var fErr))
        {
            return Result.Failure<TOut>(fErr);
        }

        var raResolved = await ra.ConfigureAwait(false);
        if (!raResolved.TryGet(out var a, out var aErr))
        {
            return Result.Failure<TOut>(aErr);
        }

        return Result.Success(f(a));
    }
}
