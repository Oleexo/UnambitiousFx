using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UnambitiousFx.Mediator.Transports.Abstractions;

namespace UnambitiousFx.Mediator.Transports.Outbox;

/// <summary>
///     Background service that periodically processes pending outbox entries and dispatches them to transports.
/// </summary>
/// <remarks>
///     This service polls the outbox storage at configurable intervals, retrieves pending entries in batches,
///     and dispatches them using the transport dispatcher. Failed dispatches are retried with exponential backoff.
/// </remarks>
public sealed class OutboxBackgroundService : BackgroundService
{
    private readonly ILogger<OutboxBackgroundService> _logger;
    private readonly OutboxOptions _options;
    private readonly IDistributedOutboxStorage _outboxStorage;
    private readonly IMessageTraitsRegistry _traitsRegistry;
    private readonly ITransportDispatcher _transportDispatcher;

    /// <summary>
    ///     Initializes a new instance of the <see cref="OutboxBackgroundService" /> class.
    /// </summary>
    /// <param name="outboxStorage">The distributed outbox storage.</param>
    /// <param name="transportDispatcher">The transport dispatcher for sending messages.</param>
    /// <param name="traitsRegistry">The message traits registry for retrieving message configuration.</param>
    /// <param name="logger">The logger for diagnostic messages.</param>
    /// <param name="options">The outbox configuration options.</param>
    public OutboxBackgroundService(
        IDistributedOutboxStorage outboxStorage,
        ITransportDispatcher transportDispatcher,
        IMessageTraitsRegistry traitsRegistry,
        ILogger<OutboxBackgroundService> logger,
        IOptions<OutboxOptions> options)
    {
        _outboxStorage = outboxStorage;
        _transportDispatcher = transportDispatcher;
        _traitsRegistry = traitsRegistry;
        _logger = logger;
        _options = options.Value;
    }

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Outbox background service started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessOutboxEntriesAsync(stoppingToken);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogError(ex, "Error processing outbox entries");
            }

            // Wait for the next polling interval
            try
            {
                await Task.Delay(_options.PollingInterval, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                // Expected when stopping
                break;
            }
        }

        _logger.LogInformation("Outbox background service stopped");
    }

    private async Task ProcessOutboxEntriesAsync(CancellationToken cancellationToken)
    {
        var batchSize = _options.BatchSize ?? 100;
        var entries = await _outboxStorage.GetPendingAsync(batchSize, cancellationToken);

        if (entries.Count == 0) return;

        _logger.LogDebug("Processing {Count} outbox entries", entries.Count);

        var startTime = DateTimeOffset.UtcNow;

        foreach (var entry in entries)
        {
            if (cancellationToken.IsCancellationRequested) break;

            await ProcessEntryAsync(entry, cancellationToken);
        }

        var duration = DateTimeOffset.UtcNow - startTime;
        _logger.LogDebug(
            "Completed processing {Count} outbox entries in {Duration}ms",
            entries.Count,
            duration.TotalMilliseconds);
    }

    private async Task ProcessEntryAsync(OutboxEntry entry, CancellationToken cancellationToken)
    {
        try
        {
            var traits = _traitsRegistry.GetTraits(entry.Envelope.PayloadType);
            if (traits == null)
                throw new InvalidOperationException(
                    $"No message traits found for message type {entry.Envelope.PayloadType}");

            // Check if we should retry based on retry policy
            if (entry.AttemptCount > 0 && traits.RetryPolicy != null)
            {
                if (entry.AttemptCount >= traits.RetryPolicy.MaxAttempts)
                {
                    _logger.LogWarning(
                        "Message {MessageId} exceeded max retry attempts ({MaxAttempts}), marking as failed",
                        entry.MessageId,
                        traits.RetryPolicy.MaxAttempts);

                    await _outboxStorage.MarkFailedAsync(
                        entry.MessageId,
                        $"Exceeded max retry attempts: {traits.RetryPolicy.MaxAttempts}",
                        cancellationToken);

                    return;
                }

                // Apply exponential backoff delay
                var delay = traits.RetryPolicy.CalculateDelay(entry.AttemptCount);
                var timeSinceLastAttempt = DateTimeOffset.UtcNow - entry.CreatedAt;

                if (timeSinceLastAttempt < delay)
                    // Not ready to retry yet
                    return;
            }

            // Attempt dispatch
            await _transportDispatcher.DispatchAsync(entry.Envelope, traits, cancellationToken);

            // Mark as completed
            await _outboxStorage.MarkCompletedAsync(entry.MessageId, cancellationToken);

            _logger.LogDebug(
                "Successfully dispatched outbox entry {MessageId} of type {MessageType}",
                entry.MessageId,
                entry.Envelope.PayloadType);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Failed to dispatch outbox entry {MessageId} (attempt {AttemptCount})",
                entry.MessageId,
                entry.AttemptCount + 1);

            // Mark as failed with error details
            await _outboxStorage.MarkFailedAsync(
                entry.MessageId,
                ex.Message,
                cancellationToken);

            // Update attempt count
            entry.AttemptCount++;
        }
    }

    /// <inheritdoc />
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Outbox background service is stopping, completing in-flight dispatches");

        // Allow graceful shutdown by calling base implementation
        await base.StopAsync(cancellationToken);

        _logger.LogInformation("Outbox background service stopped gracefully");
    }
}
