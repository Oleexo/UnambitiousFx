using System.Collections.Concurrent;

namespace UnambitiousFx.Mediator.Transports.Security;

/// <summary>
/// Thread-safe registry for managing sensitive field descriptors for message types.
/// </summary>
internal sealed class SensitiveDataRegistry : ISensitiveDataRegistry
{
    private readonly ConcurrentDictionary<Type, IReadOnlyList<SensitiveFieldDescriptor>> _registry = new();

    /// <inheritdoc />
    public void RegisterSensitiveFields<TMessage>(IEnumerable<SensitiveFieldDescriptor> descriptors)
    {
        var messageType = typeof(TMessage);
        var descriptorList = descriptors.ToList().AsReadOnly();
        _registry.AddOrUpdate(messageType, descriptorList, (_, _) => descriptorList);
    }

    /// <inheritdoc />
    public IReadOnlyList<SensitiveFieldDescriptor> GetSensitiveFields(Type messageType)
    {
        return _registry.TryGetValue(messageType, out var descriptors)
            ? descriptors
            : Array.Empty<SensitiveFieldDescriptor>();
    }
}
