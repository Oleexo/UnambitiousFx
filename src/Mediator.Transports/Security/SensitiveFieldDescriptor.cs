namespace UnambitiousFx.Mediator.Transports.Security;

/// <summary>
/// Describes a sensitive field on a message type with getter/setter delegates for NativeAOT compatibility.
/// </summary>
public sealed class SensitiveFieldDescriptor
{
    /// <summary>
    /// Gets the name of the field.
    /// </summary>
    public required string FieldName { get; init; }

    /// <summary>
    /// Gets the sensitivity level of the field.
    /// </summary>
    public SensitivityLevel Level { get; init; } = SensitivityLevel.Confidential;

    /// <summary>
    /// Gets a value indicating whether encryption is required for this field.
    /// </summary>
    public bool RequireEncryption { get; init; } = true;

    /// <summary>
    /// Gets the delegate to retrieve the field value from a message instance.
    /// </summary>
    public required Func<object, string?> Getter { get; init; }

    /// <summary>
    /// Gets the delegate to set the field value on a message instance.
    /// </summary>
    public required Action<object, string?> Setter { get; init; }
}
