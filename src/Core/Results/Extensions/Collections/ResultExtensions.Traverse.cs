namespace UnambitiousFx.Core.Results.Extensions.Collections;

public static partial class ResultExtensions
{
    public static Result<List<TOut>> Traverse<TIn, TOut>(this IEnumerable<TIn> source,
                                                         Func<TIn, Result<TOut>> selector)
        where TOut : notnull
    {
        var list = new List<TOut>();
        foreach (var item in source)
        {
            var r = selector(item);
            if (r.TryGet(out var value, out var error))
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
}
