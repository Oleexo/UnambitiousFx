namespace UnambitiousFx.Mediator.Transports.Abstractions;

/// <summary>
///     Attribute for annotating message types with schema versioning information.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class MessageSchemaAttribute : Attribute
{
    /// <summary>
    ///     Gets or initializes the schema version.
    /// </summary>
    public required string Version { get; init; }

    /// <summary>
    ///     Gets or initializes an optional description of the schema.
    /// </summary>
    public string? Description { get; init; }
}
