namespace UnambitiousFx.Core.Results.Reasons;

public static class ReasonExtensions {
    public static Exception ToException(this IEnumerable<IError> errors) {
        var arr = errors as IError[] ?? errors.ToArray();
        if (arr.Length == 0) {
            return new Exception("No errors.");
        }

        if (arr.Length == 1) {
            return arr.First()
                      .Exception ??
                   new Exception(arr.First()
                                    .Message);
        }

        return new AggregateException(arr.Select(e => e.Exception ?? new Exception(e.Message)));
    }
}
