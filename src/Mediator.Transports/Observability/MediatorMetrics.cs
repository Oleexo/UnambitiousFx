using System.Diagnostics.Metrics;
using UnambitiousFx.Mediator.Abstractions;
using UnambitiousFx.Mediator.Transports.Outbox;

namespace UnambitiousFx.Mediator.Transports.Observability;

/// <summary>
///     Provides metrics for monitoring mediator transport operations using OpenTelemetry.
/// </summary>
public sealed class MediatorMetrics
{
    private readonly Counter<long> _messagesConsumed;
    private readonly Counter<long> _messagesDeadLettered;
    private readonly Counter<long> _messagesPublished;
    private readonly Counter<long> _messagesRetried;
    private readonly Histogram<double> _consumeLatency;
    private readonly Histogram<double> _publishLatency;
    private readonly Meter _meter;
    private readonly IDistributedOutboxStorage? _outboxStorage;
    
    // Event dispatch metrics
    private readonly Counter<long> _eventsDispatched;
    private readonly Counter<long> _dispatchFailures;
    private readonly Histogram<double> _dispatchLatency;
    
    // Outbox metrics
    private readonly Counter<long> _outboxEventsProcessed;
    private readonly Counter<long> _outboxDeadLettered;
    private readonly IEventOutboxStorage? _eventOutboxStorage;

    /// <summary>
    ///     Initializes a new instance of the <see cref="MediatorMetrics" /> class.
    /// </summary>
    /// <param name="meterFactory">The meter factory for creating meters.</param>
    /// <param name="outboxStorage">Optional distributed outbox storage for backlog metrics.</param>
    /// <param name="eventOutboxStorage">Optional event outbox storage for queue depth metrics.</param>
    public MediatorMetrics(
        IMeterFactory meterFactory, 
        IDistributedOutboxStorage? outboxStorage = null,
        IEventOutboxStorage? eventOutboxStorage = null)
    {
        _outboxStorage = outboxStorage;
        _eventOutboxStorage = eventOutboxStorage;
        _meter = meterFactory.Create("Unambitious.Mediator", "1.0.0");

        // Counters
        _messagesPublished = _meter.CreateCounter<long>(
            "mediator.messages.published",
            unit: "{message}",
            description: "Number of messages published to transports");

        _messagesConsumed = _meter.CreateCounter<long>(
            "mediator.messages.consumed",
            unit: "{message}",
            description: "Number of messages consumed from transports");

        _messagesRetried = _meter.CreateCounter<long>(
            "mediator.messages.retried",
            unit: "{message}",
            description: "Number of message processing retries");

        _messagesDeadLettered = _meter.CreateCounter<long>(
            "mediator.messages.dead_lettered",
            unit: "{message}",
            description: "Number of messages sent to dead-letter queue");

        // Histograms
        _publishLatency = _meter.CreateHistogram<double>(
            "mediator.publish.duration",
            unit: "ms",
            description: "Duration of message publish operations in milliseconds");

        _consumeLatency = _meter.CreateHistogram<double>(
            "mediator.consume.duration",
            unit: "ms",
            description: "Duration of message consume operations in milliseconds");

        // Event dispatch metrics
        _eventsDispatched = _meter.CreateCounter<long>(
            "mediator.events.dispatched",
            unit: "{event}",
            description: "Number of events dispatched by distribution mode");

        _dispatchFailures = _meter.CreateCounter<long>(
            "mediator.events.dispatch_failures",
            unit: "{event}",
            description: "Number of event dispatch failures");

        _dispatchLatency = _meter.CreateHistogram<double>(
            "mediator.events.dispatch.duration",
            unit: "ms",
            description: "Duration of event dispatch operations in milliseconds");

        // Outbox metrics
        _outboxEventsProcessed = _meter.CreateCounter<long>(
            "mediator.outbox.events.processed",
            unit: "{event}",
            description: "Number of events processed from the outbox");

        _outboxDeadLettered = _meter.CreateCounter<long>(
            "mediator.outbox.events.dead_lettered",
            unit: "{event}",
            description: "Number of events moved to dead-letter queue");

        // Observable Gauge for distributed outbox backlog
        if (_outboxStorage != null)
            _meter.CreateObservableGauge(
                "mediator.outbox.backlog",
                ObserveOutboxBacklog,
                unit: "{message}",
                description: "Number of pending messages in the distributed outbox");

        // Observable Gauge for event outbox queue depth
        if (_eventOutboxStorage != null)
            _meter.CreateObservableGauge(
                "mediator.outbox.queue_depth",
                ObserveEventOutboxQueueDepth,
                unit: "{event}",
                description: "Number of pending events in the event outbox");
    }

    /// <summary>
    ///     Records a message published to a transport.
    /// </summary>
    /// <param name="messageType">The type of message published.</param>
    /// <param name="transportName">The name of the transport.</param>
    public void RecordPublished(string messageType, string transportName)
    {
        _messagesPublished.Add(1, new KeyValuePair<string, object?>("message.type", messageType),
            new KeyValuePair<string, object?>("transport.name", transportName));
    }

    /// <summary>
    ///     Records a message consumed from a transport.
    /// </summary>
    /// <param name="messageType">The type of message consumed.</param>
    /// <param name="transportName">The name of the transport.</param>
    public void RecordConsumed(string messageType, string transportName)
    {
        _messagesConsumed.Add(1, new KeyValuePair<string, object?>("message.type", messageType),
            new KeyValuePair<string, object?>("transport.name", transportName));
    }

    /// <summary>
    ///     Records a message retry attempt.
    /// </summary>
    /// <param name="messageType">The type of message being retried.</param>
    public void RecordRetry(string messageType)
    {
        _messagesRetried.Add(1, new KeyValuePair<string, object?>("message.type", messageType));
    }

    /// <summary>
    ///     Records a message sent to dead-letter queue.
    /// </summary>
    /// <param name="messageType">The type of message dead-lettered.</param>
    public void RecordDeadLettered(string messageType)
    {
        _messagesDeadLettered.Add(1, new KeyValuePair<string, object?>("message.type", messageType));
    }

    /// <summary>
    ///     Records the latency of a publish operation.
    /// </summary>
    /// <param name="durationMs">The duration in milliseconds.</param>
    /// <param name="messageType">The type of message published.</param>
    /// <param name="transportName">The name of the transport.</param>
    public void RecordPublishLatency(double durationMs, string messageType, string transportName)
    {
        _publishLatency.Record(durationMs, new KeyValuePair<string, object?>("message.type", messageType),
            new KeyValuePair<string, object?>("transport.name", transportName));
    }

    /// <summary>
    ///     Records the latency of a consume operation.
    /// </summary>
    /// <param name="durationMs">The duration in milliseconds.</param>
    /// <param name="messageType">The type of message consumed.</param>
    /// <param name="transportName">The name of the transport.</param>
    public void RecordConsumeLatency(double durationMs, string messageType, string transportName)
    {
        _consumeLatency.Record(durationMs, new KeyValuePair<string, object?>("message.type", messageType),
            new KeyValuePair<string, object?>("transport.name", transportName));
    }

    /// <summary>
    ///     Records an event dispatch operation.
    /// </summary>
    /// <param name="eventType">The type of event dispatched.</param>
    /// <param name="distributionMode">The distribution mode used for dispatch.</param>
    /// <param name="success">Whether the dispatch was successful.</param>
    public void RecordEventDispatched(string eventType, string distributionMode, bool success)
    {
        _eventsDispatched.Add(1,
            new KeyValuePair<string, object?>("event.type", eventType),
            new KeyValuePair<string, object?>("distribution.mode", distributionMode),
            new KeyValuePair<string, object?>("success", success));

        if (!success)
        {
            _dispatchFailures.Add(1,
                new KeyValuePair<string, object?>("event.type", eventType),
                new KeyValuePair<string, object?>("distribution.mode", distributionMode));
        }
    }

    /// <summary>
    ///     Records the latency of an event dispatch operation.
    /// </summary>
    /// <param name="durationMs">The duration in milliseconds.</param>
    /// <param name="eventType">The type of event dispatched.</param>
    /// <param name="distributionMode">The distribution mode used for dispatch.</param>
    public void RecordDispatchLatency(double durationMs, string eventType, string distributionMode)
    {
        _dispatchLatency.Record(durationMs,
            new KeyValuePair<string, object?>("event.type", eventType),
            new KeyValuePair<string, object?>("distribution.mode", distributionMode));
    }

    /// <summary>
    ///     Records an event processed from the outbox.
    /// </summary>
    /// <param name="eventType">The type of event processed.</param>
    /// <param name="success">Whether the processing was successful.</param>
    public void RecordOutboxEventProcessed(string eventType, bool success)
    {
        _outboxEventsProcessed.Add(1,
            new KeyValuePair<string, object?>("event.type", eventType),
            new KeyValuePair<string, object?>("success", success));
    }

    /// <summary>
    ///     Records an event moved to the dead-letter queue.
    /// </summary>
    /// <param name="eventType">The type of event dead-lettered.</param>
    public void RecordOutboxDeadLettered(string eventType)
    {
        _outboxDeadLettered.Add(1,
            new KeyValuePair<string, object?>("event.type", eventType));
    }

    private long ObserveOutboxBacklog()
    {
        if (_outboxStorage == null) return 0;

        try
        {
            // Get pending entries count
            // Note: This is a synchronous call in an observable callback
            // GetPendingAsync should be fast for counting purposes
            var pendingEntries = _outboxStorage.GetPendingAsync(int.MaxValue, CancellationToken.None)
                .GetAwaiter()
                .GetResult();

            return pendingEntries.Count;
        }
        catch
        {
            // Return 0 if unable to get backlog size
            return 0;
        }
    }

    private long ObserveEventOutboxQueueDepth()
    {
        if (_eventOutboxStorage == null) return 0;

        try
        {
            // Get pending events count
            // Note: This is a synchronous call in an observable callback
            // GetPendingEventsAsync should be fast for counting purposes
            var pendingEvents = _eventOutboxStorage.GetPendingEventsAsync(CancellationToken.None)
                .GetAwaiter()
                .GetResult();

            return pendingEvents.Count();
        }
        catch
        {
            // Return 0 if unable to get queue depth
            return 0;
        }
    }
}
