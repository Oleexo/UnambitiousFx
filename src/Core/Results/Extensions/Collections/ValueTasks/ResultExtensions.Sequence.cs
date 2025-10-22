namespace UnambitiousFx.Core.Results.Extensions.Collections.ValueTasks;

public static partial class ResultExtensions {
    public static async ValueTask<Result<List<TValue>>> SequenceAsync<TValue>(this IEnumerable<ValueTask<Result<TValue>>> tasks)
        where TValue : notnull {
        var list = new List<TValue>();
        foreach (var t in tasks) {
            var r = await t.ConfigureAwait(false);
            if (r.Ok(out var value, out var error)) {
                list.Add(value);
            }
            else {
                return Result.Failure<List<TValue>>(error);
            }
        }

        return Result.Success(list);
    }

    public static async ValueTask<Result<List<TValue>>> SequenceAsync<TValue>(this ValueTask<IEnumerable<Result<TValue>>> awaitableResults)
        where TValue : notnull {
        var results = await awaitableResults.ConfigureAwait(false);
        return results.Sequence();
    }
}
