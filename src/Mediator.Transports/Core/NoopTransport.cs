using UnambitiousFx.Mediator.Transports.Abstractions;

namespace UnambitiousFx.Mediator.Transports.Core;

/// <summary>
///     Default in-memory transport for local-only operation and testing.
///     This transport performs no actual message delivery and is used when distributed messaging is not enabled.
/// </summary>
public sealed class NoopTransport : IMessageTransport
{
    /// <inheritdoc />
    public string Name => "noop";

    /// <inheritdoc />
    public ValueTask PublishAsync(
        MessageEnvelope envelope,
        CancellationToken cancellationToken = default)
    {
        // No-op: messages stay in-process
        return ValueTask.CompletedTask;
    }

    /// <inheritdoc />
    public ValueTask SubscribeAsync(
        SubscriptionDescriptor descriptor,
        CancellationToken cancellationToken = default)
    {
        // No-op: no external subscriptions
        return ValueTask.CompletedTask;
    }

    /// <inheritdoc />
    public ValueTask UnsubscribeAsync(
        SubscriptionDescriptor descriptor,
        CancellationToken cancellationToken = default)
    {
        // No-op: no external subscriptions
        return ValueTask.CompletedTask;
    }
}
