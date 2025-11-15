namespace UnambitiousFx.Mediator.Transports.Abstractions;

/// <summary>
///     Defines the interface for a registry that maps message type names to their corresponding CLR types.
///     This registry is required for NativeAOT compatibility as it avoids using Type.GetType() which relies on reflection.
/// </summary>
public interface IMessageTypeRegistry
{
    /// <summary>
    ///     Registers a message type with its type name.
    /// </summary>
    /// <typeparam name="TMessage">The message type to register.</typeparam>
    void Register<TMessage>() where TMessage : class;

    /// <summary>
    ///     Registers a message type with a custom type name.
    /// </summary>
    /// <typeparam name="TMessage">The message type to register.</typeparam>
    /// <param name="typeName">The custom type name to use for this message type.</param>
    void Register<TMessage>(string typeName) where TMessage : class;

    /// <summary>
    ///     Tries to resolve a CLR type from a message type name.
    /// </summary>
    /// <param name="typeName">The message type name.</param>
    /// <param name="messageType">When this method returns, contains the CLR type if found; otherwise, null.</param>
    /// <returns>True if the type was found; otherwise, false.</returns>
    bool TryResolveType(string typeName, out Type? messageType);

    /// <summary>
    ///     Resolves a CLR type from a message type name.
    /// </summary>
    /// <param name="typeName">The message type name.</param>
    /// <returns>The CLR type if found; otherwise, null.</returns>
    Type? ResolveType(string typeName);

    /// <summary>
    ///     Gets the type name for a given CLR type.
    /// </summary>
    /// <param name="messageType">The CLR type.</param>
    /// <returns>The type name if registered; otherwise, null.</returns>
    string? GetTypeName(Type messageType);

    /// <summary>
    ///     Gets all registered message types.
    /// </summary>
    /// <returns>An enumerable of all registered message types.</returns>
    IEnumerable<Type> GetAllTypes();
}
