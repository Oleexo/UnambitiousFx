using System.Diagnostics;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UnambitiousFx.Mediator.Abstractions;
using UnambitiousFx.Mediator.Transports.Abstractions;
using UnambitiousFx.Mediator.Transports.Inbox;
using UnambitiousFx.Mediator.Transports.Observability;

namespace UnambitiousFx.Mediator.Transports.Hosting;

/// <summary>
///     Background service that manages inbound message processing from external transports.
///     Subscribes to messages based on the message traits registry and routes them to mediator handlers.
/// </summary>
/// <remarks>
///     This service supports optional inbox deduplication through IInboxStorage.
///     If IInboxStorage is registered, duplicate messages will be detected and skipped.
/// </remarks>
public sealed class MessageReceiverHost : BackgroundService
{
    private readonly ILogger<MessageReceiverHost> _logger;
    private readonly IMessageSerializer _serializer;
    private readonly IServiceProvider _serviceProvider;
    private readonly SemaphoreSlim _shutdownSignal = new(0, 1);
    private readonly IMessageTraitsRegistry _traitsRegistry;
    private readonly ITransportProvider _transportProvider;
    private readonly IMessageTypeRegistry _typeRegistry;

    /// <summary>
    ///     Initializes a new instance of the <see cref="MessageReceiverHost" /> class.
    /// </summary>
    /// <param name="traitsRegistry">The message traits registry.</param>
    /// <param name="typeRegistry">The message type registry for NativeAOT-compatible type resolution.</param>
    /// <param name="serviceProvider">The service provider for creating scopes.</param>
    /// <param name="serializer">The message serializer.</param>
    /// <param name="transportProvider">The transport provider.</param>
    /// <param name="logger">The logger instance.</param>
    /// <remarks>
    ///     IInboxStorage is optional and will be resolved from the service provider if available.
    /// </remarks>
    public MessageReceiverHost(
        IMessageTraitsRegistry traitsRegistry,
        IMessageTypeRegistry typeRegistry,
        IServiceProvider serviceProvider,
        IMessageSerializer serializer,
        ITransportProvider transportProvider,
        ILogger<MessageReceiverHost> logger)
    {
        _traitsRegistry = traitsRegistry;
        _typeRegistry = typeRegistry;
        _serviceProvider = serviceProvider;
        _serializer = serializer;
        _transportProvider = transportProvider;
        _logger = logger;
    }

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            // Get all traits that should be subscribed to external transports
            var traits = _traitsRegistry.GetAllTraits()
                .Where(t => t.DistributionMode != DistributionMode.LocalOnly)
                .ToList();

            if (traits.Count == 0)
            {
                _logger.LogInformation(
                    "No messages configured for external distribution. MessageReceiverHost will not subscribe to any transports.");
                await _shutdownSignal.WaitAsync(stoppingToken);
                return;
            }

            _logger.LogInformation(
                "MessageReceiverHost starting. Subscribing to {Count} message types.",
                traits.Count);

            // Subscribe to each message type
            foreach (var trait in traits) await SubscribeAsync(trait, stoppingToken);

            _logger.LogInformation("MessageReceiverHost subscriptions completed. Waiting for messages...");

            // Keep running until cancellation
            await _shutdownSignal.WaitAsync(stoppingToken);
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            // Expected during shutdown
            _logger.LogInformation("MessageReceiverHost is shutting down.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "MessageReceiverHost encountered an error during execution.");
            throw;
        }
    }

    /// <inheritdoc />
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("MessageReceiverHost stop requested. Completing in-flight messages...");

        // Signal the ExecuteAsync to complete
        _shutdownSignal.Release();

        // Wait for graceful shutdown
        await base.StopAsync(cancellationToken);

        _logger.LogInformation("MessageReceiverHost stopped.");
    }

    private async Task SubscribeAsync(MessageTraits traits, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation(
                "Subscribing to message type {MessageType} on transport {TransportName} with topic {Topic}",
                traits.MessageType.Name,
                traits.TransportName ?? "default",
                traits.Topic ?? traits.MessageType.Name);

            await _transportProvider.SubscribeAsync(
                traits.MessageType,
                traits,
                async (envelope, ct) => await ProcessMessageAsync(envelope, traits, ct),
                cancellationToken);

            _logger.LogInformation(
                "Successfully subscribed to message type {MessageType}",
                traits.MessageType.Name);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Failed to subscribe to message type {MessageType} on transport {TransportName}",
                traits.MessageType.Name,
                traits.TransportName ?? "default");
            throw;
        }
    }

    private async ValueTask ProcessMessageAsync(
        MessageEnvelope envelope,
        MessageTraits traits,
        CancellationToken cancellationToken)
    {
        var attemptCount = 0;
        var maxAttempts = traits.RetryPolicy?.MaxAttempts ?? 1;

        while (attemptCount < maxAttempts)
        {
            attemptCount++;

            try
            {
                await ProcessMessageWithRetryAsync(envelope, traits, attemptCount, cancellationToken);
                return; // Success
            }
            catch (Exception) when (attemptCount < maxAttempts)
            {
                // Log retry
                LogMessages.LogRetry(
                    _logger,
                    envelope.PayloadType,
                    envelope.MessageId,
                    attemptCount);

                // Calculate delay
                if (traits.RetryPolicy != null)
                {
                    var delay = traits.RetryPolicy.CalculateDelay(attemptCount);
                    await Task.Delay(delay, cancellationToken);
                }
            }
            catch (Exception ex) when (attemptCount >= maxAttempts)
            {
                // Max retries exceeded - dead letter
                LogMessages.LogDeadLetter(
                    _logger,
                    envelope.PayloadType,
                    envelope.MessageId,
                    $"Max retry attempts ({maxAttempts}) exceeded: {ex.Message}");

                // Note: Actual dead-lettering would be handled by the transport
                // via the TransportMessage.RejectAsync callback
                throw;
            }
        }
    }

    private async ValueTask ProcessMessageWithRetryAsync(
        MessageEnvelope envelope,
        MessageTraits traits,
        int attemptCount,
        CancellationToken cancellationToken)
    {
        // Log received message
        LogMessages.LogReceived(
            _logger,
            envelope.PayloadType,
            envelope.MessageId,
            traits.TransportName ?? "default");

        // Create a new service scope for handler invocation
        await using var scope = _serviceProvider.CreateAsyncScope();

        // Check inbox for duplicate messages (if IInboxStorage is configured)
        // Note: IInboxStorage will be implemented in Phase 6
        // For now, we check if the service is available and use it if present
        var inboxStorage = scope.ServiceProvider.GetService<IInboxStorage>();
        if (inboxStorage != null)
        {
            var isDuplicate = await inboxStorage.ExistsAsync(envelope.MessageId, cancellationToken);
            if (isDuplicate)
            {
                _logger.LogInformation(
                    "Duplicate message {MessageId} detected in inbox. Skipping processing.",
                    envelope.MessageId);
                return;
            }

            // Add to inbox before processing
            await inboxStorage.AddAsync(new InboxEntry
            {
                MessageId = envelope.MessageId,
                SourceTransport = traits.TransportName ?? "default",
                ProcessedAt = DateTimeOffset.UtcNow,
                ExpiresAt = DateTimeOffset.UtcNow.AddDays(1) // Default 24 hour retention
            }, cancellationToken);
        }

        // Restore context from envelope
        var context = RestoreContext(envelope, scope.ServiceProvider);
        var contextAccessor = scope.ServiceProvider.GetRequiredService<IContextAccessor>();
        contextAccessor.Context = context;

        // Restore trace context for distributed tracing
        RestoreTraceContext(envelope);

        // Resolve message type from registry (NativeAOT-compatible)
        var messageType = _typeRegistry.ResolveType(envelope.PayloadType);
        if (messageType == null)
        {
            _logger.LogError(
                "Failed to resolve message type {PayloadType} for message {MessageId}. Type is not registered in IMessageTypeRegistry.",
                envelope.PayloadType,
                envelope.MessageId);
            throw new InvalidOperationException(
                $"Failed to resolve message type {envelope.PayloadType}. Ensure the type is registered in IMessageTypeRegistry during configuration.");
        }

        object message;
        try
        {
            // Serialize the payload to bytes first if it's not already
            byte[] payloadBytes;
            if (envelope.Payload is byte[] bytes)
            {
                payloadBytes = bytes;
            }
            else if (envelope.Payload is string json)
            {
                payloadBytes = Encoding.UTF8.GetBytes(json);
            }
            else
            {
                // Payload is already deserialized object (in-memory transport scenario)
                message = envelope.Payload;
                goto ProcessMessage;
            }

            message = _serializer.Deserialize(payloadBytes, messageType);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Failed to deserialize message {MessageId} of type {PayloadType}",
                envelope.MessageId,
                envelope.PayloadType);
            throw;
        }

        ProcessMessage:
        // Only IEvent messages are supported for inbound processing
        if (message is not IEvent evt)
        {
            _logger.LogWarning(
                "Received non-event message {MessageType} with ID {MessageId}. Only IEvent messages are supported for inbound processing. Message will be skipped.",
                messageType.Name,
                envelope.MessageId);
            return;
        }

        // Route to IPublisher
        var publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();
        var result = await publisher.PublishAsync(evt, cancellationToken);

        if (!result.IsSuccess)
        {
            _logger.LogError(
                "Failed to publish event {MessageType} with ID {MessageId}: {Error}",
                messageType.Name,
                envelope.MessageId,
                result.ToString());
            throw new InvalidOperationException(
                $"Failed to publish event {messageType.Name}: {result}");
        }

        _logger.LogDebug(
            "Successfully processed message {MessageType} with ID {MessageId}",
            messageType.Name,
            envelope.MessageId);
    }

    private IContext RestoreContext(MessageEnvelope envelope, IServiceProvider scopedProvider)
    {
        var contextBuilder = scopedProvider.GetRequiredService<IContextBuilder>();

        // Create a new context with restored metadata
        var metadata = new Dictionary<string, object>();

        // Restore standard metadata
        if (envelope.CausationId != null) metadata["CausationId"] = envelope.CausationId;

        if (envelope.TenantId != null) metadata["TenantId"] = envelope.TenantId;

        metadata["MessageId"] = envelope.MessageId;
        metadata["Timestamp"] = envelope.Timestamp;

        // Restore custom headers as metadata
        foreach (var header in envelope.Headers.GetAll())
            if (!metadata.ContainsKey(header.Key))
                metadata[header.Key] = header.Value;

        return contextBuilder
            .WithCorrelationId(envelope.CorrelationId)
            .WithMetadata(metadata)
            .Build();
    }

    private void RestoreTraceContext(MessageEnvelope envelope)
    {
        // Restore W3C Trace Context from envelope headers
        if (envelope.Headers.TraceParent != null)
            try
            {
                // Create a new activity with the parent trace context
                var activity = new Activity("MessageReceiverHost.ProcessMessage");
                activity.SetParentId(envelope.Headers.TraceParent);

                if (envelope.Headers.TraceState != null) activity.TraceStateString = envelope.Headers.TraceState;

                // Set activity tags
                activity.SetTag("messaging.system", "mediator");
                activity.SetTag("messaging.message_id", envelope.MessageId);
                activity.SetTag("messaging.correlation_id", envelope.CorrelationId);
                activity.SetTag("messaging.payload_type", envelope.PayloadType);

                activity.Start();
                Activity.Current = activity;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex,
                    "Failed to restore trace context for message {MessageId}",
                    envelope.MessageId);
            }
    }

    /// <inheritdoc />
    public override void Dispose()
    {
        _shutdownSignal.Dispose();
        base.Dispose();
    }
}
