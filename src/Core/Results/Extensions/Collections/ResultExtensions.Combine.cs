namespace UnambitiousFx.Core.Results.Extensions.Collections;

public static partial class ResultExtensions {
    
    public static Result Combine(this IEnumerable<Result> results) {
        var errors = new List<Exception>();
        foreach (var result in results) {
            if (!result.TryGet(out var error)) {
                errors.Add(error);
            }
        }

        return errors.Count != 0
                   ? Result.Failure(new AggregateException(errors))
                   : Result.Success();
    }
}
