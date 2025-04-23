namespace Oleexo.UnambitiousFx.Core;

public static class ResultExtensions {
    public static Result ToResult(this IEnumerable<Result> results) {
        var errors = new List<IError>();
        foreach (var result in results) {
            if (!result.Ok(out var error)) {
                errors.Add(error);
            }
        }

        return errors.Count != 0
                   ? Result.Failure(new AggregateError(errors))
                   : Result.Success();
    }
}
