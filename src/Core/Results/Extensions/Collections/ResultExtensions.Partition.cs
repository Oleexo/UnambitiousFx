using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Results.Extensions.Collections;

public static partial class ResultExtensions
{
    public static (List<TValue> oks, List<IError> errors) Partition<TValue>(this IEnumerable<Result<TValue>> results)
        where TValue : notnull
    {
        var oks = new List<TValue>();
        var errors = new List<IError>();
        foreach (var r in results)
        {
            if (r.TryGet(out var value, out var error))
            {
                oks.Add(value);
            }
            else
            {
                errors.AddRange(error);
            }
        }

        return (oks, errors);
    }
}
