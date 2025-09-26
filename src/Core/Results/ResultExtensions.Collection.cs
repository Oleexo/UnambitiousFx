namespace UnambitiousFx.Core.Results;

public static partial class ResultExtensions
{
    public static Result<List<TValue>> Sequence<TValue>(this IEnumerable<Result<TValue>> results)
        where TValue : notnull
    {
        var list = new List<TValue>();
        foreach (var r in results)
        {
            if (r.Ok(out var value, out var error))
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

    public static Result<List<TOut>> Traverse<TIn, TOut>(this IEnumerable<TIn> source, Func<TIn, Result<TOut>> selector)
        where TOut : notnull
    {
        var list = new List<TOut>();
        foreach (var item in source)
        {
            var r = selector(item);
            if (r.Ok(out var value, out var error))
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

    public static (List<TValue> oks, List<Exception> errors) Partition<TValue>(this IEnumerable<Result<TValue>> results)
        where TValue : notnull
    {
        var oks = new List<TValue>();
        var errors = new List<Exception>();
        foreach (var r in results)
        {
            if (r.Ok(out var value, out var error))
            {
                oks.Add(value);
            }
            else
            {
                errors.Add(error);
            }
        }

        return (oks, errors);
    }

    public static Result Combine(this IEnumerable<Result> results) {
        var errors = new List<Exception>();
        foreach (var result in results) {
            if (!result.Ok(out var error)) {
                errors.Add(error);
            }
        }

        return errors.Count != 0
                   ? Result.Failure(new AggregateException(errors))
                   : Result.Success();
    }
}
