using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Results.Extensions.Collections;

public static partial class ResultExtensions
{
    public static Result Combine(this IEnumerable<Result> results)
    {
        var errors = new List<IError>();
        foreach (var result in results)
        {
            if (!result.TryGet(out var err))
            {
                errors.AddRange(err);
            }
        }

        return errors.Count != 0
                   ? Result.Failure(errors)
                   : Result.Success();
    }
}
