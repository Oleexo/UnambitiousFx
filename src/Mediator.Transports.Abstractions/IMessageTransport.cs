namespace UnambitiousFx.Mediator.Transports.Abstractions;

/// <summary>
///     Defines the interface for built-in broker-specific transport implementations.
///     Used by the built-in transport provider for direct broker integrations.
/// </summary>
public interface IMessageTransport
{
    /// <summary>
    ///     Gets the name of the transport.
    /// </summary>
    string Name { get; }

    /// <summary>
    ///     Publishes a message envelope to the transport.
    /// </summary>
    /// <param name="envelope">The message envelope to publish.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A ValueTask representing the asynchronous operation.</returns>
    ValueTask PublishAsync(
        MessageEnvelope envelope,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Subscribes to messages from the transport.
    /// </summary>
    /// <param name="descriptor">The subscription descriptor defining the subscription parameters.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A ValueTask representing the asynchronous operation.</returns>
    ValueTask SubscribeAsync(
        SubscriptionDescriptor descriptor,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Unsubscribes from messages.
    /// </summary>
    /// <param name="descriptor">The subscription descriptor to unsubscribe.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A ValueTask representing the asynchronous operation.</returns>
    ValueTask UnsubscribeAsync(
        SubscriptionDescriptor descriptor,
        CancellationToken cancellationToken = default);
}
