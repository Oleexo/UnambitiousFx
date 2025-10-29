namespace UnambitiousFx.Core.Results.Reasons;

public static class ReasonExtensions {
    public static Exception ToException(this IEnumerable<IError> errors) {
        return new AggregateException(errors);
    }
}
