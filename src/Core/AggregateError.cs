namespace Oleexo.UnambitiousFx.Core;

public sealed record AggregateError : Error {
    public const string DefaultCode = "AGGREGATE_ERROR";

    public AggregateError(IEnumerable<IError> errors)
        : base("Multiple errors occurred. See Children for details.", DefaultCode) {
        AddChildren(errors);
    }
}
