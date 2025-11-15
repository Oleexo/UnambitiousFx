namespace UnambitiousFx.Core.Results.Extensions.Collections.Tasks;

public static partial class ResultExtensions
{
    public static async Task<Result<List<TValue>>> SequenceAsync<TValue>(this IEnumerable<Task<Result<TValue>>> tasks)
        where TValue : notnull
    {
        var list = new List<TValue>();
        foreach (var t in tasks)
        {
            var r = await t.ConfigureAwait(false);
            if (r.TryGet(out var value, out var error))
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

    public static async Task<Result<List<TValue>>> SequenceAsync<TValue>(this Task<IEnumerable<Result<TValue>>> awaitableResults)
        where TValue : notnull
    {
        var results = await awaitableResults;
        return results.Sequence();
    }
}
