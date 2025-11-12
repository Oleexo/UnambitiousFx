namespace UnambitiousFx.Mediator.Transports.Abstractions;

/// <summary>
///     Defines the interface for processing inbound transport messages.
/// </summary>
public interface ITransportMessageProcessor
{
    /// <summary>
    ///     Processes a message received from a transport.
    /// </summary>
    /// <param name="message">The transport message to process.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A ValueTask representing the asynchronous operation.</returns>
    ValueTask ProcessAsync(
        TransportMessage message,
        CancellationToken cancellationToken = default);
}
