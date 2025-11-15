using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Results.Extensions.Collections.Tasks;

public static partial class ResultExtensions
{
    public static async Task<Result> CombineAsync(this IEnumerable<Task<Result>> tasks)
    {
        var errors = new List<IError>();
        foreach (var t in tasks)
        {
            var r = await t.ConfigureAwait(false);
            if (!r.TryGet(out var error))
            {
                errors.AddRange(error);
            }
        }

        return errors.Count != 0
                   ? Result.Failure(errors)
                   : Result.Success();
    }

    public static async Task<Result> CombineAsync(this Task<IEnumerable<Result>> awaitableResults)
    {
        var results = await awaitableResults.ConfigureAwait(false);
        return results.Combine();
    }
}
