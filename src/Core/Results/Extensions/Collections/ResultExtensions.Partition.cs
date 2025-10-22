namespace UnambitiousFx.Core.Results.Extensions.Collections;

public static partial class ResultExtensions {
    public static (List<TValue> oks, List<Exception> errors) Partition<TValue>(this IEnumerable<Result<TValue>> results)
        where TValue : notnull {
        var oks    = new List<TValue>();
        var errors = new List<Exception>();
        foreach (var r in results) {
            if (r.Ok(out var value, out var error)) {
                oks.Add(value);
            }
            else {
                errors.Add(error);
            }
        }

        return (oks, errors);
    }
}
