namespace UnambitiousFx.Core.Results.Extensions.Collections.ValueTasks;

public static partial class ResultExtensions {
    public static async ValueTask<(List<TValue> oks, List<Exception> errors)> PartitionAsync<TValue>(this IEnumerable<ValueTask<Result<TValue>>> tasks)
        where TValue : notnull {
        var oks    = new List<TValue>();
        var errors = new List<Exception>();
        foreach (var t in tasks) {
            var r = await t.ConfigureAwait(false);
            if (r.Ok(out var value, out var error)) {
                oks.Add(value);
            }
            else {
                errors.Add(error);
            }
        }

        return (oks, errors);
    }

    public static async ValueTask<(List<TValue> oks, List<Exception> errors)> PartitionAsync<TValue>(this ValueTask<IEnumerable<Result<TValue>>> awaitableResults)
        where TValue : notnull {
        var results = await awaitableResults.ConfigureAwait(false);
        return results.Partition();
    }
}
