namespace UnambitiousFx.Core.Results.Reasons;

public static class ErrorExtensions {
    public static Exception ToException(this IEnumerable<IError> errors) {
        var arr = errors as IError[] ?? errors.ToArray();
        if (arr.Length == 0) {
            return new Exception("No errors");
        }

        if (arr.Length == 1) {
            var first = arr.First();
            return first.Exception ?? new Exception(first.Message);
        }

        return new AggregateException(arr.Select(e => e.Exception ?? new Exception(e.Message)));
    }
}
