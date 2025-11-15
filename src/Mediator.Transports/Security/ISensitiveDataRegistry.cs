namespace UnambitiousFx.Mediator.Transports.Security;

/// <summary>
/// Registry for managing sensitive field descriptors for message types.
/// Provides NativeAOT-compatible field access through delegates instead of reflection.
/// </summary>
public interface ISensitiveDataRegistry
{
    /// <summary>
    /// Registers sensitive fields for a specific message type.
    /// </summary>
    /// <typeparam name="TMessage">The message type.</typeparam>
    /// <param name="descriptors">The collection of sensitive field descriptors.</param>
    void RegisterSensitiveFields<TMessage>(IEnumerable<SensitiveFieldDescriptor> descriptors);

    /// <summary>
    /// Gets the sensitive field descriptors for a specific message type.
    /// </summary>
    /// <param name="messageType">The message type.</param>
    /// <returns>The collection of sensitive field descriptors, or an empty collection if none are registered.</returns>
    IReadOnlyList<SensitiveFieldDescriptor> GetSensitiveFields(Type messageType);
}
