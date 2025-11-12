namespace UnambitiousFx.Mediator.Transports.Abstractions;

/// <summary>
///     Defines the interface for building message envelopes from messages.
/// </summary>
public interface IEnvelopeBuilder
{
    /// <summary>
    ///     Builds a message envelope from a message, populating correlation identifiers,
    ///     tenant context, timestamps, and metadata from the current execution context.
    /// </summary>
    /// <typeparam name="TMessage">The type of message.</typeparam>
    /// <param name="message">The message to wrap in an envelope.</param>
    /// <returns>A message envelope containing the message and metadata.</returns>
    MessageEnvelope Build<TMessage>(TMessage message);
}
