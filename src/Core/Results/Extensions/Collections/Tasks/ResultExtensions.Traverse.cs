namespace UnambitiousFx.Core.Results.Extensions.Collections.Tasks;

public static partial class ResultExtensions {
    public static async Task<Result<List<TOut>>> TraverseAsync<TIn, TOut>(this IEnumerable<TIn>         source,
                                                                          Func<TIn, Task<Result<TOut>>> selector)
        where TOut : notnull {
        var list = new List<TOut>();
        foreach (var item in source) {
            var r = await selector(item)
                       .ConfigureAwait(false);
            if (r.Ok(out var value, out var error)) {
                list.Add(value);
            }
            else {
                return Result.Failure<List<TOut>>(error);
            }
        }

        return Result.Success(list);
    }

    public static async Task<Result<List<TOut>>> TraverseAsync<TIn, TOut>(this Task<IEnumerable<TIn>>   awaitableSource,
                                                                          Func<TIn, Task<Result<TOut>>> selector)
        where TOut : notnull {
        var source = await awaitableSource.ConfigureAwait(false);
        return await source.TraverseAsync(selector);
    }
}
