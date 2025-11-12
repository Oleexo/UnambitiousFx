namespace UnambitiousFx.Mediator.Transports.Abstractions;

/// <summary>
///     Defines the interface for a registry that stores and retrieves message traits.
/// </summary>
public interface IMessageTraitsRegistry
{
    /// <summary>
    ///     Registers message traits for a specific message type.
    /// </summary>
    /// <typeparam name="TMessage">The message type.</typeparam>
    /// <param name="traits">The message traits to register.</param>
    void Register<TMessage>(MessageTraits traits);

    /// <summary>
    ///     Gets the message traits for a specific message type.
    /// </summary>
    /// <param name="messageType">The message type.</param>
    /// <returns>The message traits if found; otherwise, null.</returns>
    MessageTraits? GetTraits(string messageType);

    /// <summary>
    ///     Retrieves the message traits associated with the specified message type.
    /// </summary>
    /// <typeparam name="TMessage">The type of the message to retrieve traits for.</typeparam>
    /// <returns>The message traits for the given message type if registered; otherwise, null.</returns>
    MessageTraits? GetTraits<TMessage>();

    /// <summary>
    ///     Gets all registered message traits.
    /// </summary>
    /// <returns>An enumerable of all registered message traits.</returns>
    IEnumerable<MessageTraits> GetAllTraits();
}
