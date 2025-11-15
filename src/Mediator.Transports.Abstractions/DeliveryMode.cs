namespace UnambitiousFx.Mediator.Transports.Abstractions;

/// <summary>
///     Defines the delivery guarantee semantics for message transport.
/// </summary>
public enum DeliveryMode
{
    /// <summary>
    ///     Best-effort delivery with no guarantees. Messages may be lost.
    /// </summary>
    BestEffort = 0,

    /// <summary>
    ///     At-least-once delivery. Messages may be delivered multiple times.
    /// </summary>
    AtLeastOnce = 1,

    /// <summary>
    ///     Exactly-once delivery. Messages are delivered exactly once (requires deduplication).
    /// </summary>
    ExactlyOnce = 2
}
