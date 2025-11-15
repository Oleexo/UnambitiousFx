using System.Collections.Concurrent;
using UnambitiousFx.Mediator.Transports.Abstractions;

namespace UnambitiousFx.Mediator.Transports.Core;

/// <summary>
///     Thread-safe registry that maps message type names to their corresponding CLR types.
///     This registry is NativeAOT-compatible as it avoids using Type.GetType() which relies on reflection.
/// </summary>
public sealed class MessageTypeRegistry : IMessageTypeRegistry
{
    private readonly ConcurrentDictionary<string, Type> _typeNameToType = new();
    private readonly ConcurrentDictionary<Type, string> _typeToTypeName = new();

    /// <inheritdoc />
    public void Register<TMessage>() where TMessage : class
    {
        var messageType = typeof(TMessage);
        var typeName = messageType.AssemblyQualifiedName ?? messageType.FullName ?? messageType.Name;
        Register<TMessage>(typeName);
    }

    /// <inheritdoc />
    public void Register<TMessage>(string typeName) where TMessage : class
    {
        ArgumentNullException.ThrowIfNull(typeName);

        var messageType = typeof(TMessage);
        _typeNameToType.TryAdd(typeName, messageType);
        _typeToTypeName.TryAdd(messageType, typeName);
    }

    /// <inheritdoc />
    public bool TryResolveType(string typeName, out Type? messageType)
    {
        return _typeNameToType.TryGetValue(typeName, out messageType);
    }

    /// <inheritdoc />
    public Type? ResolveType(string typeName)
    {
        return _typeNameToType.TryGetValue(typeName, out var type) ? type : null;
    }

    /// <inheritdoc />
    public string? GetTypeName(Type messageType)
    {
        return _typeToTypeName.TryGetValue(messageType, out var typeName) ? typeName : null;
    }

    /// <inheritdoc />
    public IEnumerable<Type> GetAllTypes()
    {
        return _typeToTypeName.Keys;
    }
}
