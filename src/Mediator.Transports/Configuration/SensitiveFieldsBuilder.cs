using UnambitiousFx.Mediator.Transports.Security;

namespace UnambitiousFx.Mediator.Transports.Configuration;

/// <summary>
/// Builder implementation for configuring sensitive fields on a message type.
/// </summary>
/// <typeparam name="TMessage">The message type being configured.</typeparam>
internal sealed class SensitiveFieldsBuilder<TMessage> : ISensitiveFieldsBuilder<TMessage>
    where TMessage : class
{
    private readonly List<SensitiveFieldDescriptor> _descriptors = new();

    public ISensitiveFieldsBuilder<TMessage> Add(
        string fieldName,
        Func<TMessage, string?> getter,
        Action<TMessage, string?> setter,
        SensitivityLevel level = SensitivityLevel.Confidential,
        bool requireEncryption = true)
    {
        _descriptors.Add(new SensitiveFieldDescriptor
        {
            FieldName = fieldName,
            Level = level,
            RequireEncryption = requireEncryption,
            Getter = obj => getter((TMessage)obj),
            Setter = (obj, value) => setter((TMessage)obj, value)
        });

        return this;
    }

    internal IReadOnlyList<SensitiveFieldDescriptor> Build() => _descriptors.AsReadOnly();
}
