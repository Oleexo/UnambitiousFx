using System.Collections.Concurrent;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Mediator.Abstractions;
using UnambitiousFx.Mediator.Orchestrators;
using UnambitiousFx.Mediator.Resolvers;
using UnambitiousFx.Mediator.Transports.Abstractions;
using UnambitiousFx.Mediator.Transports.Observability;

namespace UnambitiousFx.Mediator;

/// <summary>
///     Core component responsible for all event dispatching logic.
///     Coordinates distribution mode determination, pipeline execution, and outbox integration.
/// </summary>
/// <remarks>
///     <para>
///         The EventDispatcher is the central orchestrator for event processing in the unified event dispatching system.
///         It handles:
///         <list type="bullet">
///             <item>Distribution mode determination through routing filters, message traits, and default configuration</item>
///             <item>Pipeline behavior execution for cross-cutting concerns</item>
///             <item>Integration with the outbox pattern for reliable event delivery</item>
///             <item>OpenTelemetry tracing and metrics collection</item>
///             <item>NativeAOT-compatible event replay from outbox</item>
///         </list>
///     </para>
///     <para>
///         <b>Distribution Mode Resolution Order:</b>
///         <list type="number">
///             <item>Routing filters (evaluated in order of <see cref="IEventRoutingFilter.Order"/>)</item>
///             <item>Message traits from <see cref="IMessageTraitsRegistry"/></item>
///             <item>Default distribution mode from <see cref="EventDispatcherOptions.DefaultDistributionMode"/></item>
///         </list>
///     </para>
///     <para>
///         <b>NativeAOT Compatibility:</b>
///         The EventDispatcher uses pre-registered dispatcher delegates to avoid reflection when replaying events
///         from the outbox. These delegates are registered at startup via source generation or explicit registration.
///     </para>
/// </remarks>
internal sealed class EventDispatcher : IEventDispatcher
{
    private readonly IDependencyResolver _dependencyResolver;
    private readonly IEventOrchestrator _eventOrchestrator;
    private readonly IMessageTraitsRegistry _traitsRegistry;
    private readonly IEventRoutingFilter[] _routingFilters;
    private readonly OutboxManager _outboxManager;
    private readonly EventDispatcherOptions _options;
    private readonly ILogger<EventDispatcher> _logger;
    private readonly IReadOnlyDictionary<Type, DispatchEventDelegate> _dispatchers;
    private readonly MediatorMetrics _metrics;
    
    // Cache for routing decisions per event type
    private readonly ConcurrentDictionary<Type, DistributionMode> _routingCache = new();

    public EventDispatcher(
        IDependencyResolver dependencyResolver,
        IEventOrchestrator eventOrchestrator,
        IMessageTraitsRegistry traitsRegistry,
        IEnumerable<IEventRoutingFilter> routingFilters,
        OutboxManager outboxManager,
        IOptions<EventDispatcherOptions> options,
        ILogger<EventDispatcher> logger,
        MediatorMetrics metrics)
    {
        _dependencyResolver = dependencyResolver;
        _eventOrchestrator = eventOrchestrator;
        _traitsRegistry = traitsRegistry;
        _routingFilters = routingFilters.OrderBy(f => f.Order).ToArray();
        _outboxManager = outboxManager;
        _options = options.Value;
        _logger = logger;
        _dispatchers = _options.Dispatchers;
        _metrics = metrics;
    }

    /// <summary>
    ///     Dispatches an event using the non-generic interface.
    ///     Uses pre-registered dispatcher delegates for NativeAOT compatibility.
    /// </summary>
    /// <param name="event">The event to dispatch.</param>
    /// <param name="cancellationToken">A cancellation token to observe.</param>
    /// <returns>A ValueTask containing the result of the dispatch operation.</returns>
    /// <remarks>
    ///     This method is used for non-generic event dispatch scenarios and relies on
    ///     pre-registered dispatcher delegates to maintain type information without reflection.
    /// </remarks>
    public ValueTask<Result> DispatchAsync(IEvent @event,
        CancellationToken cancellationToken)
    {
        var eventType = @event.GetType();

        if (_dispatchers.TryGetValue(eventType, out var dispatcher))
        {
            // Use LocalOnly as default distribution mode for backward compatibility
            // This will be replaced with proper distribution mode determination in task 6
            return dispatcher(@event, this, DistributionMode.LocalOnly, cancellationToken);
        }

        return ValueTask.FromResult(Result.Failure($"No dispatcher registered for event type {eventType.Name}"));
    }

    /// <summary>
    ///     Dispatches an event through the unified event dispatching pipeline.
    /// </summary>
    /// <typeparam name="TEvent">The type of event to dispatch.</typeparam>
    /// <param name="event">The event to dispatch.</param>
    /// <param name="cancellationToken">A cancellation token to observe.</param>
    /// <returns>A ValueTask containing the result of the dispatch operation.</returns>
    /// <remarks>
    ///     <para>
    ///         This method orchestrates the complete event dispatch flow:
    ///         <list type="number">
    ///             <item>Determines distribution mode (routing filters → message traits → default)</item>
    ///             <item>Resolves event handlers and pipeline behaviors</item>
    ///             <item>Executes pipeline behaviors for cross-cutting concerns</item>
    ///             <item>Routes to OutboxManager for storage and dispatch based on strategy</item>
    ///         </list>
    ///     </para>
    ///     <para>
    ///         The event is always stored in the outbox first for transactional guarantees,
    ///         then dispatched according to the configured <see cref="DispatchStrategy"/>.
    ///     </para>
    /// </remarks>
    public ValueTask<Result> DispatchAsync<TEvent>(TEvent @event,
        CancellationToken cancellationToken)
        where TEvent : class, IEvent
    {
        // 1. Determine distribution mode
        var distributionMode = DetermineDistributionMode(@event);
        
        // 2. Get handlers and behaviors
        var handlers = _dependencyResolver.GetServices<IEventHandler<TEvent>>();
        var behaviors = _dependencyResolver.GetServices<IEventPipelineBehavior>();

        return ExecutePipelineAsync(
            @event, 
            handlers.ToArray(), 
            behaviors.ToArray(), 
            distributionMode,
            skipOutbox: false,
            0, 
            cancellationToken);
    }

    /// <summary>
    ///     Dispatches an event from the outbox, skipping outbox storage.
    ///     This method is NativeAOT-friendly as it maintains generic type information.
    /// </summary>
    public ValueTask<Result> DispatchFromOutboxAsync<TEvent>(
        TEvent @event,
        DistributionMode distributionMode,
        CancellationToken cancellationToken)
        where TEvent : class, IEvent
    {
        var handlers = _dependencyResolver.GetServices<IEventHandler<TEvent>>();
        var behaviors = _dependencyResolver.GetServices<IEventPipelineBehavior>();
        
        return ExecutePipelineAsync(
            @event, 
            handlers.ToArray(), 
            behaviors.ToArray(), 
            distributionMode,
            skipOutbox: true,
            0, 
            cancellationToken);
    }

    /// <summary>
    ///     Determines the distribution mode for an event by evaluating routing filters,
    ///     message traits, and default configuration in that order.
    /// </summary>
    private DistributionMode DetermineDistributionMode<TEvent>(TEvent @event)
        where TEvent : class, IEvent
    {
        var eventType = typeof(TEvent);
        
        // Check cache first for deterministic routing
        if (_routingCache.TryGetValue(eventType, out var cachedMode))
        {
            _logger.LogDebug("Using cached distribution mode {DistributionMode} for event type {EventType}", 
                cachedMode, eventType.Name);
            return cachedMode;
        }
        
        _logger.LogDebug("Determining distribution mode for event type {EventType}", eventType.Name);
        
        // 1. Evaluate routing filters in order
        if (_routingFilters.Length > 0)
        {
            _logger.LogDebug("Evaluating {FilterCount} routing filters for event type {EventType}", 
                _routingFilters.Length, eventType.Name);
            
            foreach (var filter in _routingFilters)
            {
                var filterType = filter.GetType().Name;
                _logger.LogTrace("Evaluating routing filter {FilterType} (Order: {Order}) for event type {EventType}",
                    filterType, filter.Order, eventType.Name);
                
                var mode = filter.GetDistributionMode(@event);
                if (mode.HasValue)
                {
                    _logger.LogInformation(
                        "Routing filter {FilterType} determined distribution mode {DistributionMode} for event type {EventType}",
                        filterType, mode.Value, eventType.Name);
                    
                    // Cache the decision (filters should be deterministic for caching to be safe)
                    _routingCache.TryAdd(eventType, mode.Value);
                    return mode.Value;
                }
                
                _logger.LogTrace("Routing filter {FilterType} returned no distribution mode for event type {EventType}",
                    filterType, eventType.Name);
            }
            
            _logger.LogDebug("No routing filter determined distribution mode for event type {EventType}, falling back to message traits",
                eventType.Name);
        }
        
        // 2. Check message traits
        var traits = _traitsRegistry.GetTraits<TEvent>();
        if (traits != null)
        {
            _logger.LogInformation(
                "Using message traits distribution mode {DistributionMode} for event type {EventType}",
                traits.DistributionMode, eventType.Name);
            
            // Cache the decision
            _routingCache.TryAdd(eventType, traits.DistributionMode);
            return traits.DistributionMode;
        }
        
        // 3. Use default from options
        _logger.LogInformation(
            "Using default distribution mode {DistributionMode} for event type {EventType}",
            _options.DefaultDistributionMode, eventType.Name);
        
        // Cache the decision
        _routingCache.TryAdd(eventType, _options.DefaultDistributionMode);
        return _options.DefaultDistributionMode;
    }

    private ValueTask<Result> ExecutePipelineAsync<TEvent>(
        TEvent @event,
        IEventHandler<TEvent>[] handlers,
        IEventPipelineBehavior[] behaviors,
        DistributionMode distributionMode,
        bool skipOutbox,
        int index,
        CancellationToken cancellationToken)
        where TEvent : class, IEvent
    {
        if (index >= behaviors.Length)
            return DispatchByModeAsync(@event, handlers, distributionMode, skipOutbox, cancellationToken);

        return behaviors[index].HandleAsync(@event, Next, cancellationToken);

        ValueTask<Result> Next()
        {
            return ExecutePipelineAsync(@event, handlers, behaviors, distributionMode, skipOutbox, index + 1, cancellationToken);
        }
    }

    private async ValueTask<Result> DispatchByModeAsync<TEvent>(
        TEvent @event,
        IEventHandler<TEvent>[] handlers,
        DistributionMode distributionMode,
        bool skipOutbox,
        CancellationToken cancellationToken)
        where TEvent : class, IEvent
    {
        var eventType = typeof(TEvent).Name;
        var distributionModeStr = distributionMode.ToString();
        var startTime = Stopwatch.GetTimestamp();
        
        using var activity = MediatorActivitySource.Source.StartActivity(
            "mediator.event.dispatch",
            ActivityKind.Producer);
        
        activity?.SetTag("messaging.distribution_mode", distributionModeStr);
        activity?.SetTag("messaging.event_type", eventType);
        activity?.SetTag("messaging.from_outbox", skipOutbox);

        try
        {
            var traits = _traitsRegistry.GetTraits<TEvent>();
            
            Result result;
            if (skipOutbox)
            {
                // Event is being replayed from outbox, execute directly
                result = await _outboxManager.ExecuteDirectAsync(
                    @event, 
                    handlers, 
                    distributionMode, 
                    traits, 
                    cancellationToken);
            }
            else
            {
                // Normal flow: store in outbox then dispatch
                result = await _outboxManager.StoreAndDispatchAsync(
                    @event, 
                    handlers, 
                    distributionMode, 
                    traits, 
                    cancellationToken);
            }
            
            // Record metrics
            var elapsedMs = Stopwatch.GetElapsedTime(startTime).TotalMilliseconds;
            _metrics.RecordDispatchLatency(elapsedMs, eventType, distributionModeStr);
            _metrics.RecordEventDispatched(eventType, distributionModeStr, result.IsSuccess);
            
            return result;
        }
        catch (Exception ex)
        {
            // Record failure metrics
            var elapsedMs = Stopwatch.GetElapsedTime(startTime).TotalMilliseconds;
            _metrics.RecordDispatchLatency(elapsedMs, eventType, distributionModeStr);
            _metrics.RecordEventDispatched(eventType, distributionModeStr, false);
            
            activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
            _logger.LogError(ex, "Error dispatching event {EventType} with distribution mode {DistributionMode}",
                eventType, distributionMode);
            throw;
        }
    }
}
