namespace UnambitiousFx.Core.Results.Extensions.Collections.ValueTasks;

public static partial class ResultExtensions {
    public static async ValueTask<Result> CombineAsync(this IEnumerable<ValueTask<Result>> tasks) {
        var errors = new List<Exception>();
        foreach (var t in tasks) {
            var r = await t.ConfigureAwait(false);
            if (!r.Ok(out var error)) {
                errors.Add(error);
            }
        }

        return errors.Count != 0
                   ? Result.Failure(new AggregateException(errors))
                   : Result.Success();
    }

    public static async ValueTask<Result> CombineAsync(this ValueTask<IEnumerable<Result>> awaitableResults) {
        var results = await awaitableResults.ConfigureAwait(false);
        return results.Combine();
    }
}
