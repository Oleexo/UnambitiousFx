#nullable enable
namespace UnambitiousFx.Core.Results.ValueTasks;

public static partial class ResultExtensions
{
    // TraverseAsync: IEnumerable<T> with async selector returning ValueTask<Result<U>>
    public static async ValueTask<Result<List<TOut>>> TraverseAsync<TIn, TOut>(this IEnumerable<TIn> source, Func<TIn, ValueTask<Result<TOut>>> selector)
        where TOut : notnull
    {
        var list = new List<TOut>();
        foreach (var item in source)
        {
            var r = await selector(item).ConfigureAwait(false);
            if (r.Ok(out var value, out var error))
            {
                list.Add(value);
            }
            else
            {
                return Result.Failure<List<TOut>>(error);
            }
        }

        return Result.Success(list);
    }

    // SequenceAsync: IEnumerable<ValueTask<Result<T>>> -> ValueTask<Result<List<T>>>
    public static async ValueTask<Result<List<TValue>>> SequenceAsync<TValue>(this IEnumerable<ValueTask<Result<TValue>>> tasks)
        where TValue : notnull
    {
        var list = new List<TValue>();
        foreach (var t in tasks)
        {
            var r = await t.ConfigureAwait(false);
            if (r.Ok(out var value, out var error))
            {
                list.Add(value);
            }
            else
            {
                return Result.Failure<List<TValue>>(error);
            }
        }

        return Result.Success(list);
    }
}
