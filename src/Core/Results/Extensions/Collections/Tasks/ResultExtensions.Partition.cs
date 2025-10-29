using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Results.Extensions.Collections.Tasks;

public static partial class ResultExtensions {
    public static async Task<(List<TValue> oks, List<IError> errors)> PartitionAsync<TValue>(this IEnumerable<Task<Result<TValue>>> tasks)
        where TValue : notnull {
        var oks    = new List<TValue>();
        var errors = new List<IError>();
        foreach (var t in tasks) {
            var r = await t.ConfigureAwait(false);
            if (r.TryGet(out var value, out var error)) {
                oks.Add(value);
            }
            else {
                errors.AddRange(error);
            }
        }

        return (oks, errors);
    }

    public static async Task<(List<TValue> oks, List<IError> errors)> PartitionAsync<TValue>(this Task<IEnumerable<Result<TValue>>> awaitableResults)
        where TValue : notnull {
        var results = await awaitableResults;
        return results.Partition();
    }
}
