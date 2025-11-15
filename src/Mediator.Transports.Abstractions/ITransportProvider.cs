namespace UnambitiousFx.Mediator.Transports.Abstractions;

/// <summary>
///     Defines the top-level abstraction for pluggable transport systems.
///     Implementations can be built-in transports, third-party frameworks (MassTransit, Wolverine), or custom providers.
/// </summary>
public interface ITransportProvider
{
    /// <summary>
    ///     Gets the type of transport provider.
    /// </summary>
    TransportProviderType ProviderType { get; }

    /// <summary>
    ///     Publishes a message envelope to the external transport.
    /// </summary>
    /// <param name="envelope">The message envelope to publish.</param>
    /// <param name="traits">The message traits defining routing and reliability policies.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A ValueTask representing the asynchronous operation.</returns>
    ValueTask PublishAsync(
        MessageEnvelope envelope,
        MessageTraits traits,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Subscribes to messages and routes them back to mediator handlers.
    /// </summary>
    /// <param name="messageType">The type of message to subscribe to.</param>
    /// <param name="traits">The message traits defining routing and reliability policies.</param>
    /// <param name="handler">The handler delegate to invoke when messages are received.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A ValueTask representing the asynchronous operation.</returns>
    ValueTask SubscribeAsync(
        Type messageType,
        MessageTraits traits,
        Func<MessageEnvelope, CancellationToken, ValueTask> handler,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Unsubscribes from messages.
    /// </summary>
    /// <param name="messageType">The type of message to unsubscribe from.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A ValueTask representing the asynchronous operation.</returns>
    ValueTask UnsubscribeAsync(
        Type messageType,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Performs a health check on the transport provider.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A ValueTask containing true if healthy; otherwise, false.</returns>
    ValueTask<bool> CheckHealthAsync(CancellationToken cancellationToken = default);
}
