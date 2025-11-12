using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.Json.Serialization;
using Amazon.SimpleNotificationService;
using Amazon.SQS;
using Microsoft.Extensions.Logging;
using UnambitiousFx.Mediator.Transports.Abstractions;
using SnsMessageAttributeValue = Amazon.SimpleNotificationService.Model.MessageAttributeValue;
using SnsPublishRequest = Amazon.SimpleNotificationService.Model.PublishRequest;
using SqsMessage = Amazon.SQS.Model.Message;
using SqsReceiveMessageRequest = Amazon.SQS.Model.ReceiveMessageRequest;
using SqsDeleteMessageRequest = Amazon.SQS.Model.DeleteMessageRequest;
using SqsChangeMessageVisibilityRequest = Amazon.SQS.Model.ChangeMessageVisibilityRequest;

namespace UnambitiousFx.Mediator.Transports.AwsSns;

/// <summary>
///     AWS SNS/SQS implementation of IMessageTransport.
/// </summary>
public sealed class AwsSnsTransport : IMessageTransport, IAsyncDisposable
{
    private readonly IAmazonSimpleNotificationService _snsClient;
    private readonly IAmazonSQS _sqsClient;
    private readonly IMessageSerializer _serializer;
    private readonly AwsSnsOptions _options;
    private readonly ILogger<AwsSnsTransport> _logger;
    private readonly ConcurrentDictionary<string, PollingTask> _activePollingTasks = new();
    private readonly CancellationTokenSource _disposalCts = new();

    private bool _disposed;

    /// <inheritdoc />
    public string Name => _options.TransportName ?? "awssns";

    /// <summary>
    ///     Initializes a new instance of the <see cref="AwsSnsTransport"/> class.
    /// </summary>
    /// <param name="snsClient">The AWS SNS client.</param>
    /// <param name="sqsClient">The AWS SQS client.</param>
    /// <param name="serializer">The message serializer.</param>
    /// <param name="options">The AWS SNS/SQS configuration options.</param>
    /// <param name="logger">The logger instance.</param>
    public AwsSnsTransport(
        IAmazonSimpleNotificationService snsClient,
        IAmazonSQS sqsClient,
        IMessageSerializer serializer,
        AwsSnsOptions options,
        ILogger<AwsSnsTransport> logger)
    {
        _snsClient = snsClient ?? throw new ArgumentNullException(nameof(snsClient));
        _sqsClient = sqsClient ?? throw new ArgumentNullException(nameof(sqsClient));
        _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async ValueTask PublishAsync(
        MessageEnvelope envelope,
        CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        if (string.IsNullOrEmpty(_options.TopicArn))
        {
            throw new InvalidOperationException("TopicArn must be configured for publishing messages");
        }

        // Serialize envelope
        var body = _serializer.Serialize(envelope);
        var messageBody = Convert.ToBase64String(body);

        // Create SNS publish request
        var publishRequest = new SnsPublishRequest
        {
            TopicArn = _options.TopicArn,
            Message = messageBody,
            MessageAttributes = new Dictionary<string, SnsMessageAttributeValue>
            {
                ["MessageId"] = new SnsMessageAttributeValue
                {
                    DataType = "String",
                    StringValue = envelope.MessageId
                },
                ["CorrelationId"] = new SnsMessageAttributeValue
                {
                    DataType = "String",
                    StringValue = envelope.CorrelationId
                },
                ["PayloadType"] = new SnsMessageAttributeValue
                {
                    DataType = "String",
                    StringValue = envelope.PayloadType
                },
                ["Timestamp"] = new SnsMessageAttributeValue
                {
                    DataType = "Number",
                    StringValue = envelope.Timestamp.ToUnixTimeMilliseconds().ToString()
                },
                ["ContentType"] = new SnsMessageAttributeValue
                {
                    DataType = "String",
                    StringValue = _serializer.ContentType
                }
            }
        };

        // Add optional attributes
        if (!string.IsNullOrEmpty(envelope.CausationId))
        {
            publishRequest.MessageAttributes["CausationId"] = new SnsMessageAttributeValue
            {
                DataType = "String",
                StringValue = envelope.CausationId
            };
        }

        if (!string.IsNullOrEmpty(envelope.TenantId))
        {
            publishRequest.MessageAttributes["TenantId"] = new SnsMessageAttributeValue
            {
                DataType = "String",
                StringValue = envelope.TenantId
            };
        }

        if (!string.IsNullOrEmpty(envelope.Headers.TraceParent))
        {
            publishRequest.MessageAttributes["TraceParent"] = new SnsMessageAttributeValue
            {
                DataType = "String",
                StringValue = envelope.Headers.TraceParent
            };
        }

        if (!string.IsNullOrEmpty(envelope.Headers.TraceState))
        {
            publishRequest.MessageAttributes["TraceState"] = new SnsMessageAttributeValue
            {
                DataType = "String",
                StringValue = envelope.Headers.TraceState
            };
        }

        // Publish to SNS
        var response = await _snsClient.PublishAsync(publishRequest, cancellationToken);

        _logger.LogDebug(
            "Published message {MessageId} to SNS topic {TopicArn} with message ID {SnsMessageId}",
            envelope.MessageId,
            _options.TopicArn,
            response.MessageId);
    }

    /// <inheritdoc />
    public ValueTask SubscribeAsync(
        SubscriptionDescriptor descriptor,
        CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        if (string.IsNullOrEmpty(_options.QueueUrl))
        {
            throw new InvalidOperationException("QueueUrl must be configured for receiving messages");
        }

        // Start background polling task
        var pollingCts = CancellationTokenSource.CreateLinkedTokenSource(_disposalCts.Token, cancellationToken);
        var pollingTask = Task.Run(
            () => PollMessagesAsync(descriptor, pollingCts.Token),
            pollingCts.Token);

        var registration = new PollingTask(pollingTask, pollingCts);
        _activePollingTasks.TryAdd(descriptor.Topic, registration);

        _logger.LogInformation(
            "Started polling SQS queue {QueueUrl} for topic {Topic}",
            _options.QueueUrl,
            descriptor.Topic);

        return ValueTask.CompletedTask;
    }

    /// <inheritdoc />
    public async ValueTask UnsubscribeAsync(
        SubscriptionDescriptor descriptor,
        CancellationToken cancellationToken = default)
    {
        if (_activePollingTasks.TryRemove(descriptor.Topic, out var registration))
        {
            try
            {
                // Cancel the polling task
                await registration.CancellationTokenSource.CancelAsync();

                // Wait for the task to complete with a timeout
                await registration.Task.WaitAsync(TimeSpan.FromSeconds(30), cancellationToken);

                registration.CancellationTokenSource.Dispose();

                _logger.LogInformation(
                    "Stopped polling SQS queue for topic {Topic}",
                    descriptor.Topic);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex,
                    "Error unsubscribing from topic {Topic}",
                    descriptor.Topic);
            }
        }
    }

    private async Task PollMessagesAsync(
        SubscriptionDescriptor descriptor,
        CancellationToken cancellationToken)
    {
        _logger.LogDebug("Starting SQS polling loop for topic {Topic}", descriptor.Topic);

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                // Receive messages from SQS with long polling
                var receiveRequest = new SqsReceiveMessageRequest
                {
                    QueueUrl = _options.QueueUrl,
                    MaxNumberOfMessages = Math.Min(descriptor.MaxConcurrency, _options.MaxNumberOfMessages),
                    WaitTimeSeconds = _options.WaitTimeSeconds,
                    MessageAttributeNames = new List<string> { "All" },
                    MessageSystemAttributeNames = new List<string> { "All" }
                };

                var response = await _sqsClient.ReceiveMessageAsync(receiveRequest, cancellationToken);

                if (response.Messages.Count == 0)
                {
                    continue; // No messages, continue polling
                }

                _logger.LogDebug(
                    "Received {Count} messages from SQS queue {QueueUrl}",
                    response.Messages.Count,
                    _options.QueueUrl);

                // Process messages concurrently up to MaxConcurrency
                var processingTasks = response.Messages.Select(message =>
                    ProcessMessageAsync(message, descriptor, cancellationToken));

                await Task.WhenAll(processingTasks);
            }
            catch (OperationCanceledException)
            {
                // Expected during shutdown
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error polling SQS queue {QueueUrl}",
                    _options.QueueUrl);

                // Back off on error
                await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            }
        }

        _logger.LogDebug("Stopped SQS polling loop for topic {Topic}", descriptor.Topic);
    }

    private async Task ProcessMessageAsync(
        SqsMessage sqsMessage,
        SubscriptionDescriptor descriptor,
        CancellationToken cancellationToken)
    {
        try
        {
            // Deserialize the message body (which is Base64-encoded envelope)
            var messageBody = sqsMessage.Body;
            
            // SNS wraps the message, so we need to extract the actual message
            // If the message came through SNS, it will be JSON with a "Message" field
            if (messageBody.TrimStart().StartsWith("{"))
            {
                // Try to parse as SNS notification
                try
                {
                    var snsNotification = JsonSerializer.Deserialize(messageBody, SnsNotificationContext.Default.SnsNotification);
                    if (snsNotification?.Message != null)
                    {
                        messageBody = snsNotification.Message;
                    }
                }
                catch
                {
                    // Not an SNS notification, use as-is
                }
            }

            var envelopeBytes = Convert.FromBase64String(messageBody);
            var envelope = _serializer.Deserialize(envelopeBytes, typeof(MessageEnvelope)) as MessageEnvelope;

            if (envelope == null)
            {
                _logger.LogError("Failed to deserialize message envelope from SQS message {MessageId}",
                    sqsMessage.MessageId);
                
                // Delete the message to prevent reprocessing
                await DeleteMessageAsync(sqsMessage.ReceiptHandle, cancellationToken);
                return;
            }

            // Invoke handler
            await descriptor.Handler(envelope, cancellationToken);

            // Acknowledge message by deleting it from the queue
            await DeleteMessageAsync(sqsMessage.ReceiptHandle, cancellationToken);

            _logger.LogDebug(
                "Successfully processed message {MessageId} from SQS queue",
                envelope.MessageId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error processing SQS message {MessageId}",
                sqsMessage.MessageId);

            // Apply retry policy
            var shouldRetry = ShouldRetry(sqsMessage, descriptor.RetryPolicy);
            if (shouldRetry)
            {
                // Change visibility timeout to retry later
                await ChangeMessageVisibilityAsync(
                    sqsMessage.ReceiptHandle,
                    CalculateRetryDelay(sqsMessage, descriptor.RetryPolicy),
                    cancellationToken);
            }
            else
            {
                // Max retries exceeded, delete message (it will go to DLQ if configured)
                await DeleteMessageAsync(sqsMessage.ReceiptHandle, cancellationToken);
            }
        }
    }

    private bool ShouldRetry(SqsMessage sqsMessage, RetryPolicy? retryPolicy)
    {
        if (retryPolicy == null)
            return false;

        // Get approximate receive count from message attributes
        if (sqsMessage.Attributes.TryGetValue("ApproximateReceiveCount", out var receiveCountStr) &&
            int.TryParse(receiveCountStr, out var receiveCount))
        {
            return receiveCount < retryPolicy.MaxAttempts;
        }

        return true; // Default to retry if we can't determine count
    }

    private int CalculateRetryDelay(SqsMessage sqsMessage, RetryPolicy? retryPolicy)
    {
        if (retryPolicy == null)
            return 60; // Default 1 minute

        // Get approximate receive count
        var receiveCount = 1;
        if (sqsMessage.Attributes.TryGetValue("ApproximateReceiveCount", out var receiveCountStr))
        {
            int.TryParse(receiveCountStr, out receiveCount);
        }

        var delay = retryPolicy.CalculateDelay(receiveCount);
        return (int)delay.TotalSeconds;
    }

    private async Task DeleteMessageAsync(string receiptHandle, CancellationToken cancellationToken)
    {
        try
        {
            var deleteRequest = new SqsDeleteMessageRequest
            {
                QueueUrl = _options.QueueUrl,
                ReceiptHandle = receiptHandle
            };

            await _sqsClient.DeleteMessageAsync(deleteRequest, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to delete message from SQS queue");
        }
    }

    private async Task ChangeMessageVisibilityAsync(
        string receiptHandle,
        int visibilityTimeoutSeconds,
        CancellationToken cancellationToken)
    {
        try
        {
            var changeVisibilityRequest = new SqsChangeMessageVisibilityRequest
            {
                QueueUrl = _options.QueueUrl,
                ReceiptHandle = receiptHandle,
                VisibilityTimeout = visibilityTimeoutSeconds
            };

            await _sqsClient.ChangeMessageVisibilityAsync(changeVisibilityRequest, cancellationToken);

            _logger.LogDebug(
                "Changed message visibility timeout to {Seconds} seconds for retry",
                visibilityTimeoutSeconds);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to change message visibility");
        }
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        if (_disposed)
            return;

        _disposed = true;

        // Cancel all polling tasks
        await _disposalCts.CancelAsync();

        // Wait for all polling tasks to complete
        var tasks = _activePollingTasks.Values.Select(r => r.Task).ToArray();
        try
        {
            await Task.WhenAll(tasks).WaitAsync(TimeSpan.FromSeconds(30));
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error waiting for polling tasks to complete during disposal");
        }

        // Dispose cancellation token sources
        foreach (var registration in _activePollingTasks.Values)
        {
            registration.CancellationTokenSource.Dispose();
        }

        _activePollingTasks.Clear();
        _disposalCts.Dispose();

        // Dispose AWS clients
        _snsClient.Dispose();
        _sqsClient.Dispose();

        _logger.LogInformation("AWS SNS/SQS transport disposed");
    }

    private sealed record PollingTask(
        Task Task,
        CancellationTokenSource CancellationTokenSource);
}

/// <summary>
///     SNS notification wrapper for extracting the actual message.
/// </summary>
internal sealed class SnsNotification
{
    public string? Message { get; set; }
}

/// <summary>
///     JSON serialization context for SNS notification deserialization.
/// </summary>
[JsonSerializable(typeof(SnsNotification))]
internal partial class SnsNotificationContext : JsonSerializerContext
{
}
