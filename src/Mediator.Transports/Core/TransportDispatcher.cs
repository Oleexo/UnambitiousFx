using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UnambitiousFx.Mediator.Transports.Abstractions;
using UnambitiousFx.Mediator.Transports.Outbox;

namespace UnambitiousFx.Mediator.Transports.Core;

/// <summary>
///     Coordinates message dispatch to transport providers, handling both direct dispatch and outbox patterns.
/// </summary>
public sealed class TransportDispatcher : ITransportDispatcher
{
    private readonly IDistributedOutboxStorage? _distributedOutboxStorage;
    private readonly ILogger<TransportDispatcher> _logger;
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    ///     Initializes a new instance of the <see cref="TransportDispatcher" /> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider for resolving transport providers.</param>
    /// <param name="logger">The logger for diagnostic messages.</param>
    /// <param name="distributedOutboxStorage">Optional distributed outbox storage for reliable delivery.</param>
    public TransportDispatcher(
        IServiceProvider serviceProvider,
        ILogger<TransportDispatcher> logger,
        IDistributedOutboxStorage? distributedOutboxStorage = null)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _distributedOutboxStorage = distributedOutboxStorage;
    }

    /// <inheritdoc />
    public async ValueTask DispatchAsync(
        MessageEnvelope envelope,
        MessageTraits traits,
        CancellationToken cancellationToken = default)
    {
        if (traits.UseOutbox && _distributedOutboxStorage != null)
            await DispatchViaOutboxAsync(envelope, traits, cancellationToken);
        else
            await DispatchDirectlyAsync(envelope, traits, cancellationToken);
    }

    private async ValueTask DispatchViaOutboxAsync(
        MessageEnvelope envelope,
        MessageTraits traits,
        CancellationToken cancellationToken)
    {
        var entry = new OutboxEntry
        {
            MessageId = envelope.MessageId,
            Envelope = envelope,
            TransportName = traits.TransportName,
            Topic = traits.Topic,
            CreatedAt = DateTimeOffset.UtcNow,
            Status = OutboxEntryStatus.Pending
        };

        await _distributedOutboxStorage!.AddAsync(entry, cancellationToken);

        _logger.LogDebug(
            "Added message {MessageId} of type {MessageType} to outbox for later dispatch",
            envelope.MessageId,
            envelope.PayloadType);
    }

    private async ValueTask DispatchDirectlyAsync(
        MessageEnvelope envelope,
        MessageTraits traits,
        CancellationToken cancellationToken)
    {
        try
        {
            var provider = _serviceProvider.GetRequiredService<ITransportProvider>();
            await provider.PublishAsync(envelope, traits, cancellationToken);

            _logger.LogDebug(
                "Successfully dispatched message {MessageId} of type {MessageType} to transport {TransportName}",
                envelope.MessageId,
                envelope.PayloadType,
                traits.TransportName ?? "default");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Failed to dispatch message {MessageId} of type {MessageType} to transport {TransportName}",
                envelope.MessageId,
                envelope.PayloadType,
                traits.TransportName ?? "default");

            // Re-throw if fail-fast is enabled
            if (traits.FailFast) throw;

            // Otherwise, log and swallow the exception (best-effort)
        }
    }
}
