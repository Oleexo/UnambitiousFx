namespace Oleexo.UnambitiousFx.Core;

internal sealed record AggregateError : Error {
    /// <summary>
    ///     A constant representing the default error code for <see cref="AggregateError" /> instances.
    /// </summary>
    public const string DefaultCode = "AGGREGATE_ERROR";

    /// <summary>
    ///     Represents an error that aggregates multiple child errors into a single entity.
    /// </summary>
    /// <remarks>
    ///     This type is useful for scenarios where multiple errors need to be grouped together.
    ///     The collection of errors can be accessed via the <see cref="IError.Children" /> property.
    /// </remarks>
    public AggregateError(IEnumerable<IError> errors)
        : base("Multiple errors occurred. See Children for details.", DefaultCode) {
        AddChildren(errors);
    }
}
