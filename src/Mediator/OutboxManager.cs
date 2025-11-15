using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions.Collections;
using UnambitiousFx.Mediator.Abstractions;
using UnambitiousFx.Mediator.Orchestrators;
using UnambitiousFx.Mediator.Transports.Abstractions;
using UnambitiousFx.Mediator.Transports.Observability;

namespace UnambitiousFx.Mediator;

/// <summary>
///     Manages outbox storage and dispatch strategies for both local and external events.
///     Coordinates event storage, retry logic, and distribution mode-based dispatch.
/// </summary>
internal sealed class OutboxManager
{
    private readonly IEnvelopeBuilder _envelopeBuilder;
    private readonly IEventDispatcher _eventDispatcher;
    private readonly IEventOrchestrator _eventOrchestrator;
    private readonly ILogger<OutboxManager> _logger;
    private readonly EventDispatcherOptions _options;
    private readonly OutboxOptions _outboxOptions;
    private readonly IEventOutboxStorage _outboxStorage;
    private readonly ITransportDispatcher _transportDispatcher;
    private readonly MediatorMetrics _metrics;

    public OutboxManager(
        IEventOutboxStorage outboxStorage,
        IEnvelopeBuilder envelopeBuilder,
        ITransportDispatcher transportDispatcher,
        IEventOrchestrator eventOrchestrator,
        IEventDispatcher eventDispatcher,
        IOptions<EventDispatcherOptions> options,
        IOptions<OutboxOptions> outboxOptions,
        ILogger<OutboxManager> logger,
        MediatorMetrics metrics)
    {
        _outboxStorage = outboxStorage;
        _envelopeBuilder = envelopeBuilder;
        _transportDispatcher = transportDispatcher;
        _eventOrchestrator = eventOrchestrator;
        _eventDispatcher = eventDispatcher;
        _options = options.Value;
        _outboxOptions = outboxOptions.Value;
        _logger = logger;
        _metrics = metrics;
    }

    /// <summary>
    ///     Stores an event in the outbox and dispatches it based on the configured dispatch strategy.
    /// </summary>
    /// <typeparam name="TEvent">The type of event to store and dispatch.</typeparam>
    /// <param name="event">The event to store and dispatch.</param>
    /// <param name="handlers">The local event handlers for this event type.</param>
    /// <param name="distributionMode">The distribution mode determining how the event should be processed.</param>
    /// <param name="traits">Optional message traits for external dispatch configuration.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result indicating success or failure of the operation.</returns>
    public async ValueTask<Result> StoreAndDispatchAsync<TEvent>(
        TEvent @event,
        IEventHandler<TEvent>[] handlers,
        DistributionMode distributionMode,
        MessageTraits? traits,
        CancellationToken cancellationToken)
        where TEvent : class, IEvent
    {
        var eventType = typeof(TEvent).Name;
        
        _logger.LogDebug(
            "Storing event {EventType} in outbox with distribution mode {DistributionMode}",
            eventType, distributionMode);
        
        // Always store in outbox first for transactional guarantees
        var storeResult = await _outboxStorage.AddAsync(@event, distributionMode, cancellationToken);
        if (storeResult.IsFaulted)
        {
            _logger.LogError(
                "Failed to store event {EventType} in outbox with distribution mode {DistributionMode}: {Error}",
                eventType, distributionMode, storeResult.ToString());
            return storeResult;
        }

        _logger.LogInformation(
            "Event {EventType} stored in outbox successfully with distribution mode {DistributionMode}, applying dispatch strategy {DispatchStrategy}",
            eventType, distributionMode, _options.DispatchStrategy);

        // Apply dispatch strategy
        return _options.DispatchStrategy switch
        {
            DispatchStrategy.Immediate => await DispatchImmediateAsync(@event, handlers, distributionMode, traits,
                cancellationToken),
            DispatchStrategy.Deferred => HandleDeferredDispatch(eventType),
            DispatchStrategy.Batched => await DispatchBatchedAsync(@event, handlers, distributionMode, traits,
                cancellationToken),
            _ => Result.Failure($"Unknown dispatch strategy: {_options.DispatchStrategy}")
        };
    }

    private Result HandleDeferredDispatch(string eventType)
    {
        _logger.LogDebug(
            "Event {EventType} stored in outbox for deferred processing by background worker",
            eventType);
        return Result.Success(); // Background processing will handle
    }

    private async ValueTask<Result> DispatchImmediateAsync<TEvent>(
        TEvent @event,
        IEventHandler<TEvent>[] handlers,
        DistributionMode distributionMode,
        MessageTraits? traits,
        CancellationToken cancellationToken)
        where TEvent : class, IEvent
    {
        var eventType = typeof(TEvent).Name;
        
        _logger.LogDebug(
            "Dispatching event {EventType} immediately with distribution mode {DistributionMode}",
            eventType, distributionMode);
        
        try
        {
            var result = distributionMode switch
            {
                DistributionMode.LocalOnly => await ExecuteLocalAsync(@event, handlers, cancellationToken),
                DistributionMode.ExternalOnly => await ExecuteExternalAsync(@event, traits, cancellationToken),
                DistributionMode.Hybrid => await ExecuteHybridAsync(@event, handlers, traits, cancellationToken),
                _ => Result.Failure($"Unknown distribution mode: {distributionMode}")
            };

            if (result.IsSuccess)
            {
                _logger.LogDebug(
                    "Event {EventType} dispatched successfully, marking as processed in outbox",
                    eventType);
                await _outboxStorage.MarkAsProcessedAsync(@event, cancellationToken);
            }
            else
            {
                _logger.LogWarning(
                    "Event {EventType} dispatch failed: {Error}",
                    eventType, result.ToString());
                await HandleDispatchFailureAsync(@event, result.ToString(), cancellationToken);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Exception occurred while dispatching event {EventType} with distribution mode {DistributionMode}",
                eventType, distributionMode);
            
            await HandleDispatchFailureAsync(@event, ex.Message, cancellationToken);

            // Return success if not fail-fast (best-effort)
            if (traits?.FailFast == true)
                throw;

            return Result.Success();
        }
    }

    private ValueTask<Result> DispatchBatchedAsync<TEvent>(
        TEvent @event,
        IEventHandler<TEvent>[] handlers,
        DistributionMode distributionMode,
        MessageTraits? traits,
        CancellationToken cancellationToken)
        where TEvent : class, IEvent
    {
        var eventType = typeof(TEvent).Name;
        _logger.LogDebug(
            "Event {EventType} added to batch queue for batched processing (batch size: {BatchSize}, flush interval: {FlushInterval})",
            eventType, _options.BatchSize, _options.BatchFlushInterval);
        
        // Add to batch queue (implementation depends on batching mechanism)
        // For now, defer to background processing
        return ValueTask.FromResult(Result.Success());
    }

    private ValueTask<Result> ExecuteLocalAsync<TEvent>(
        TEvent @event,
        IEventHandler<TEvent>[] handlers,
        CancellationToken cancellationToken)
        where TEvent : class, IEvent
    {
        var eventType = typeof(TEvent).Name;
        _logger.LogDebug(
            "Executing local handlers for event {EventType} ({HandlerCount} handlers)",
            eventType, handlers.Length);
        
        return _eventOrchestrator.RunAsync(handlers, @event, cancellationToken);
    }

    private async ValueTask<Result> ExecuteExternalAsync<TEvent>(
        TEvent @event,
        MessageTraits? traits,
        CancellationToken cancellationToken)
        where TEvent : class, IEvent
    {
        var eventType = typeof(TEvent).Name;
        _logger.LogDebug(
            "Dispatching event {EventType} to external transport",
            eventType);
        
        var envelope = _envelopeBuilder.Build(@event);
        await _transportDispatcher.DispatchAsync(envelope, traits ?? new MessageTraits { MessageType = typeof(TEvent) },
            cancellationToken);
        
        _logger.LogDebug(
            "Event {EventType} dispatched to external transport successfully",
            eventType);
        
        return Result.Success();
    }

    private async ValueTask<Result> ExecuteHybridAsync<TEvent>(
        TEvent @event,
        IEventHandler<TEvent>[] handlers,
        MessageTraits? traits,
        CancellationToken cancellationToken)
        where TEvent : class, IEvent
    {
        var eventType = typeof(TEvent).Name;
        _logger.LogDebug(
            "Executing hybrid dispatch for event {EventType} (local handlers: {HandlerCount})",
            eventType, handlers.Length);
        
        // Execute local and external in parallel
        var localTask = ExecuteLocalAsync(@event, handlers, cancellationToken).AsTask();
        var externalTask = ExecuteExternalAsync(@event, traits, cancellationToken).AsTask();

        await Task.WhenAll(localTask, externalTask);

        var localResult = await localTask;
        var externalResult = await externalTask;

        var combinedResult = new[] { localResult, externalResult }.Combine();
        
        if (combinedResult.IsSuccess)
        {
            _logger.LogDebug(
                "Hybrid dispatch completed successfully for event {EventType}",
                eventType);
        }
        else
        {
            _logger.LogWarning(
                "Hybrid dispatch completed with failures for event {EventType}: {Error}",
                eventType, combinedResult.ToString());
        }

        return combinedResult;
    }

    /// <summary>
    ///     Executes event processing directly without storing in outbox.
    ///     Used when replaying events from outbox.
    /// </summary>
    /// <typeparam name="TEvent">The type of event to execute.</typeparam>
    /// <param name="event">The event to execute.</param>
    /// <param name="handlers">The local event handlers for this event type.</param>
    /// <param name="distributionMode">The distribution mode determining how the event should be processed.</param>
    /// <param name="traits">Optional message traits for external dispatch configuration.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result indicating success or failure of the operation.</returns>
    public async ValueTask<Result> ExecuteDirectAsync<TEvent>(
        TEvent @event,
        IEventHandler<TEvent>[] handlers,
        DistributionMode distributionMode,
        MessageTraits? traits,
        CancellationToken cancellationToken)
        where TEvent : class, IEvent
    {
        var result = distributionMode switch
        {
            DistributionMode.LocalOnly => await ExecuteLocalAsync(@event, handlers, cancellationToken),
            DistributionMode.ExternalOnly => await ExecuteExternalAsync(@event, traits, cancellationToken),
            DistributionMode.Hybrid => await ExecuteHybridAsync(@event, handlers, traits, cancellationToken),
            _ => Result.Failure($"Unknown distribution mode: {distributionMode}")
        };

        return result;
    }

    /// <summary>
    ///     Processes pending events from the outbox.
    ///     Retrieves pending events and dispatches them using registered dispatcher delegates.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A combined result of all processed events.</returns>
    public async ValueTask<Result> ProcessPendingAsync(CancellationToken cancellationToken)
    {
        _logger.LogDebug("Processing pending events from outbox");
        
        var pendingEvents = await _outboxStorage.GetPendingEventsAsync(cancellationToken);
        var events = _outboxOptions.BatchSize.HasValue
            ? pendingEvents.Take(_outboxOptions.BatchSize.Value).ToList()
            : pendingEvents.ToList();

        if (events.Count == 0)
        {
            _logger.LogDebug("No pending events found in outbox");
            return Result.Success();
        }

        _logger.LogInformation(
            "Processing {EventCount} pending events from outbox (batch size: {BatchSize})",
            events.Count, _outboxOptions.BatchSize);

        var results = new List<Result>();

        foreach (var @event in events)
        {
            var result = await DispatchEventAsync(@event, cancellationToken);
            results.Add(result);
        }

        var combinedResult = results.Combine();
        
        if (combinedResult.IsSuccess)
        {
            _logger.LogInformation(
                "Successfully processed {EventCount} pending events from outbox",
                events.Count);
        }
        else
        {
            _logger.LogWarning(
                "Completed processing {EventCount} pending events from outbox with failures: {Error}",
                events.Count, combinedResult.ToString());
        }

        return combinedResult;
    }

    private async ValueTask<Result> DispatchEventAsync(
        IEvent @event,
        CancellationToken cancellationToken)
    {
        var eventType = @event.GetType().Name;
        
        try
        {
            var distributionMode = await _outboxStorage.GetDistributionModeAsync(@event, cancellationToken);
            
            _logger.LogDebug(
                "Dispatching event {EventType} from outbox with distribution mode {DistributionMode}",
                eventType, distributionMode);

            // Use the registered dispatcher delegate to maintain type information
            // This delegate is registered at startup via source generation or explicit registration
            var dispatcher = _options.Dispatchers.GetValueOrDefault(@event.GetType());
            if (dispatcher == null)
            {
                _logger.LogError(
                    "No dispatcher registered for event type {EventType}, cannot process from outbox",
                    eventType);
                return Result.Failure($"No dispatcher registered for event type {eventType}");
            }

            // The dispatcher delegate calls EventDispatcher.DispatchFromOutboxAsync<TEvent>
            // with the correct generic type, avoiding reflection
            var result = await dispatcher(@event, _eventDispatcher, distributionMode, cancellationToken);

            if (result.IsSuccess)
            {
                _logger.LogDebug(
                    "Event {EventType} dispatched successfully from outbox, marking as processed",
                    eventType);
                await _outboxStorage.MarkAsProcessedAsync(@event, cancellationToken);
                _metrics.RecordOutboxEventProcessed(eventType, true);
            }
            else
            {
                _logger.LogWarning(
                    "Event {EventType} dispatch from outbox failed: {Error}",
                    eventType, result.ToString());
                await HandleDispatchFailureAsync(@event, result.ToString(), cancellationToken);
                _metrics.RecordOutboxEventProcessed(eventType, false);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Exception occurred while dispatching event {EventType} from outbox",
                eventType);
            await HandleDispatchFailureAsync(@event, ex.Message, cancellationToken);
            return Result.Failure(ex.Message);
        }
    }

    /// <summary>
    ///     Handles dispatch failures by calculating retry delays and moving events to dead-letter when appropriate.
    /// </summary>
    /// <param name="event">The event that failed to dispatch.</param>
    /// <param name="reason">The reason for the failure.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    private async ValueTask HandleDispatchFailureAsync(
        IEvent @event,
        string reason,
        CancellationToken cancellationToken)
    {
        var eventType = @event.GetType().Name;
        var attemptCount = await _outboxStorage.GetAttemptCountAsync(@event, cancellationToken) ?? 0;
        var nextAttemptNumber = attemptCount + 1;
        var shouldDeadLetter = nextAttemptNumber >= _outboxOptions.MaxRetryAttempts;

        DateTimeOffset? nextAttemptAt = null;
        TimeSpan? calculatedDelay = null;
        
        if (!shouldDeadLetter && _outboxOptions.InitialRetryDelay > TimeSpan.Zero)
        {
            // Calculate exponential backoff: delay * (backoffFactor ^ attemptCount)
            var factorPower = Math.Pow(_outboxOptions.BackoffFactor, attemptCount);
            calculatedDelay = TimeSpan.FromMilliseconds(_outboxOptions.InitialRetryDelay.TotalMilliseconds * factorPower);
            nextAttemptAt = DateTimeOffset.UtcNow + calculatedDelay.Value;
            
            _logger.LogWarning(
                "Event {EventType} dispatch failed (attempt {AttemptNumber}/{MaxAttempts}), scheduling retry with exponential backoff. " +
                "Backoff calculation: {InitialDelay}ms * ({BackoffFactor} ^ {AttemptCount}) = {CalculatedDelay}ms. " +
                "Next retry at: {NextRetryTime}. Reason: {FailureReason}",
                eventType,
                nextAttemptNumber,
                _outboxOptions.MaxRetryAttempts,
                _outboxOptions.InitialRetryDelay.TotalMilliseconds,
                _outboxOptions.BackoffFactor,
                attemptCount,
                calculatedDelay.Value.TotalMilliseconds,
                nextAttemptAt.Value,
                reason);
        }
        else if (shouldDeadLetter)
        {
            _logger.LogError(
                "Event {EventType} exceeded maximum retry attempts ({MaxAttempts}), moving to dead-letter queue. " +
                "Total attempts: {TotalAttempts}. Final failure reason: {FailureReason}",
                eventType,
                _outboxOptions.MaxRetryAttempts,
                nextAttemptNumber,
                reason);
        }
        else
        {
            _logger.LogWarning(
                "Event {EventType} dispatch failed (attempt {AttemptNumber}/{MaxAttempts}), no retry delay configured. Reason: {FailureReason}",
                eventType,
                nextAttemptNumber,
                _outboxOptions.MaxRetryAttempts,
                reason);
        }

        await _outboxStorage.MarkAsFailedAsync(@event, reason, shouldDeadLetter, nextAttemptAt, cancellationToken);
        
        if (shouldDeadLetter)
        {
            _logger.LogError(
                "Event {EventType} successfully moved to dead-letter queue after {TotalAttempts} failed attempts",
                eventType,
                nextAttemptNumber);
            _metrics.RecordOutboxDeadLettered(eventType);
        }
        else
        {
            _logger.LogInformation(
                "Event {EventType} marked as failed in outbox, retry scheduled for {NextRetryTime}",
                eventType,
                nextAttemptAt);
        }
    }
}
