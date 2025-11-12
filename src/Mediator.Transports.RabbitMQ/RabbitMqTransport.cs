using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using UnambitiousFx.Mediator.Transports.Abstractions;

namespace UnambitiousFx.Mediator.Transports.RabbitMQ;

/// <summary>
///     RabbitMQ implementation of IMessageTransport.
/// </summary>
public sealed class RabbitMqTransport : IMessageTransport, IAsyncDisposable
{
    private readonly ConcurrentDictionary<string, ConsumerRegistration> _activeConsumers = new();
    private readonly IConnectionFactory _connectionFactory;
    private readonly SemaphoreSlim _connectionLock = new(1, 1);
    private readonly ILogger<RabbitMqTransport> _logger;
    private readonly RabbitMqOptions _options;
    private readonly IMessageSerializer _serializer;

    private IConnection? _connection;
    private bool _disposed;
    private IChannel? _publishChannel;

    /// <summary>
    ///     Initializes a new instance of the <see cref="RabbitMqTransport" /> class.
    /// </summary>
    /// <param name="connectionFactory">The RabbitMQ connection factory.</param>
    /// <param name="serializer">The message serializer.</param>
    /// <param name="options">The RabbitMQ configuration options.</param>
    /// <param name="logger">The logger instance.</param>
    public RabbitMqTransport(
        IConnectionFactory connectionFactory,
        IMessageSerializer serializer,
        RabbitMqOptions options,
        ILogger<RabbitMqTransport> logger)
    {
        _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        if (_disposed)
            return;

        _disposed = true;

        // Cancel all consumers
        foreach (var registration in _activeConsumers.Values)
            try
            {
                await registration.Channel.BasicCancelAsync(registration.ConsumerTag);
                await registration.Channel.CloseAsync();
                registration.Channel.Dispose();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error disposing consumer");
            }

        _activeConsumers.Clear();

        // Close publish channel
        if (_publishChannel is not null)
        {
            await _publishChannel.CloseAsync();
            _publishChannel.Dispose();
        }

        // Close connection
        if (_connection is not null)
        {
            await _connection.CloseAsync();
            _connection.Dispose();
        }

        _connectionLock.Dispose();

        _logger.LogInformation("RabbitMQ transport disposed");
    }

    /// <inheritdoc />
    public string Name => _options.TransportName ?? "rabbitmq";

    /// <inheritdoc />
    public async ValueTask PublishAsync(
        MessageEnvelope envelope,
        CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        await EnsureConnectionAsync(cancellationToken);

        var channel = _publishChannel!;
        var routingKey = envelope.Headers.Get("topic") ?? envelope.PayloadType;

        // Declare exchange
        await channel.ExchangeDeclareAsync(
            _options.ExchangeName,
            _options.ExchangeType,
            _options.DurableExchange,
            cancellationToken: cancellationToken);

        // Serialize envelope
        var body = _serializer.Serialize(envelope);

        // Map envelope to RabbitMQ properties
        var properties = new BasicProperties();
        properties.MessageId = envelope.MessageId;
        properties.CorrelationId = envelope.CorrelationId;
        properties.Timestamp = new AmqpTimestamp(envelope.Timestamp.ToUnixTimeSeconds());
        properties.ContentType = _serializer.ContentType;
        properties.Persistent = true;

        // Add custom headers
        properties.Headers = new Dictionary<string, object?>();
        if (envelope.CausationId != null)
            properties.Headers["causation-id"] = envelope.CausationId;
        if (envelope.TenantId != null)
            properties.Headers["tenant-id"] = envelope.TenantId;
        if (envelope.Headers.TraceParent != null)
            properties.Headers["traceparent"] = envelope.Headers.TraceParent;
        if (envelope.Headers.TraceState != null)
            properties.Headers["tracestate"] = envelope.Headers.TraceState;

        properties.Headers["payload-type"] = envelope.PayloadType;

        // Publish message
        await channel.BasicPublishAsync(
            _options.ExchangeName,
            routingKey,
            false,
            properties,
            body,
            cancellationToken);

        _logger.LogDebug(
            "Published message {MessageId} to RabbitMQ exchange {Exchange} with routing key {RoutingKey}",
            envelope.MessageId,
            _options.ExchangeName,
            routingKey);
    }

    /// <inheritdoc />
    public async ValueTask SubscribeAsync(
        SubscriptionDescriptor descriptor,
        CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        await EnsureConnectionAsync(cancellationToken);

        var queueName = $"{descriptor.Topic}.{descriptor.MessageType}";
        var routingKey = descriptor.Topic;

        if (_connection is null || !_connection.IsOpen)
        {
            _logger.LogError("RabbitMQ connection is not open");
            return;
        }

        // Create a dedicated channel for this consumer
        var channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

        // Declare exchange
        await channel.ExchangeDeclareAsync(
            _options.ExchangeName,
            _options.ExchangeType,
            _options.DurableExchange,
            cancellationToken: cancellationToken);

        // Declare queue
        await channel.QueueDeclareAsync(
            queueName,
            _options.DurableQueues,
            false,
            _options.AutoDeleteQueues,
            cancellationToken: cancellationToken);

        // Bind queue to exchange
        await channel.QueueBindAsync(
            queueName,
            _options.ExchangeName,
            routingKey,
            cancellationToken: cancellationToken);

        // Set prefetch count for concurrency control
        await channel.BasicQosAsync(
            0,
            (ushort)descriptor.MaxConcurrency,
            false,
            cancellationToken);

        // Create consumer
        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            await HandleMessageAsync(ea, descriptor, channel, cancellationToken);
        };

        // Start consuming
        var consumerTag = await channel.BasicConsumeAsync(
            queueName,
            false,
            consumer,
            cancellationToken);

        // Store consumer registration
        var registration = new ConsumerRegistration(channel, consumer, consumerTag, queueName);
        _activeConsumers.TryAdd(descriptor.Topic, registration);

        _logger.LogInformation(
            "Subscribed to RabbitMQ queue {Queue} with routing key {RoutingKey}",
            queueName,
            routingKey);
    }

    /// <inheritdoc />
    public async ValueTask UnsubscribeAsync(
        SubscriptionDescriptor descriptor,
        CancellationToken cancellationToken = default)
    {
        if (_activeConsumers.TryRemove(descriptor.Topic, out var registration))
            try
            {
                await registration.Channel.BasicCancelAsync(registration.ConsumerTag);
                await registration.Channel.CloseAsync(cancellationToken);
                registration.Channel.Dispose();

                _logger.LogInformation(
                    "Unsubscribed from RabbitMQ queue {Queue}",
                    registration.QueueName);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex,
                    "Error unsubscribing from queue {Queue}",
                    registration.QueueName);
            }
    }

    private async Task HandleMessageAsync(
        BasicDeliverEventArgs ea,
        SubscriptionDescriptor descriptor,
        IChannel channel,
        CancellationToken cancellationToken)
    {
        try
        {
            // Deserialize envelope
            var envelope = _serializer.Deserialize(ea.Body.ToArray(), typeof(MessageEnvelope)) as MessageEnvelope;
            if (envelope == null)
            {
                _logger.LogError("Failed to deserialize message envelope");
                await channel.BasicNackAsync(ea.DeliveryTag, false, false, cancellationToken);
                return;
            }

            // Invoke handler
            await descriptor.Handler(envelope, cancellationToken);

            // Acknowledge message
            await channel.BasicAckAsync(ea.DeliveryTag, false, cancellationToken);

            _logger.LogDebug(
                "Successfully processed message {MessageId} from queue {Queue}",
                envelope.MessageId,
                descriptor.Topic);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error processing message from queue {Queue}",
                descriptor.Topic);

            // Apply retry policy
            var shouldRetry = ShouldRetry(ea, descriptor.RetryPolicy);
            if (shouldRetry)
                // Requeue for retry
                await channel.BasicNackAsync(ea.DeliveryTag, false, true, cancellationToken);
            else
                // Send to dead-letter (reject without requeue)
                await channel.BasicNackAsync(ea.DeliveryTag, false, false, cancellationToken);
        }
    }

    private bool ShouldRetry(BasicDeliverEventArgs ea, RetryPolicy? retryPolicy)
    {
        if (retryPolicy == null)
            return false;

        // Check retry count from headers
        var retryCount = 0;
        if (ea.BasicProperties.Headers != null &&
            ea.BasicProperties.Headers.TryGetValue("x-retry-count", out var retryCountObj))
            retryCount = Convert.ToInt32(retryCountObj);

        return retryCount < retryPolicy.MaxAttempts;
    }

    private async ValueTask EnsureConnectionAsync(CancellationToken cancellationToken)
    {
        if (_connection != null && _connection.IsOpen && _publishChannel != null && _publishChannel.IsOpen)
            return;

        await _connectionLock.WaitAsync(cancellationToken);
        try
        {
            if (_connection != null && _connection.IsOpen && _publishChannel != null && _publishChannel.IsOpen)
                return;

            // Close existing connection if any
            if (_publishChannel != null)
            {
                await _publishChannel.CloseAsync(cancellationToken);
                _publishChannel.Dispose();
            }

            if (_connection != null)
            {
                await _connection.CloseAsync(cancellationToken);
                _connection.Dispose();
            }

            // Create new connection
            _connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);
            _publishChannel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

            _logger.LogInformation(
                "Established RabbitMQ connection to {Host}:{Port}",
                _options.HostName,
                _options.Port);
        }
        finally
        {
            _connectionLock.Release();
        }
    }

    private sealed record ConsumerRegistration(
        IChannel Channel,
        AsyncEventingBasicConsumer Consumer,
        string ConsumerTag,
        string QueueName);
}
