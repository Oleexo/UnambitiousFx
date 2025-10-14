namespace UnambitiousFx.Core.Results.Tasks;

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

    public static async Task<Result<List<TValue>>> SequenceAsync<TValue>(this IEnumerable<Task<Result<TValue>>> tasks)
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
}
