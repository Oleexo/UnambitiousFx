namespace UnambitiousFx.Core.Results.Extensions.Collections;

public static partial class ResultExtensions
{
    public static Result<List<TValue>> Sequence<TValue>(this IEnumerable<Result<TValue>> results)
        where TValue : notnull
    {
        var list = new List<TValue>();
        foreach (var r in results)
        {
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


}
