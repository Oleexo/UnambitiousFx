namespace UnambitiousFx.Mediator.Transports.Abstractions;

/// <summary>
///     Defines the interface for dispatching messages to transports.
///     Coordinates outbox and direct transport dispatch based on message traits.
/// </summary>
public interface ITransportDispatcher
{
    /// <summary>
    ///     Dispatches a message envelope to the configured transport.
    /// </summary>
    /// <param name="envelope">The message envelope to dispatch.</param>
    /// <param name="traits">The message traits defining routing and reliability policies.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A ValueTask representing the asynchronous operation.</returns>
    ValueTask DispatchAsync(
        MessageEnvelope envelope,
        MessageTraits traits,
        CancellationToken cancellationToken = default);
}
