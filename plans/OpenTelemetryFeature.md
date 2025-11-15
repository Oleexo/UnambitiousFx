# OpenTelemetry Integration Plan for UnambitiousFx.Mediator

## Executive Summary

This plan outlines the implementation of OpenTelemetry (OTel) distributed tracing and metrics for the UnambitiousFx.Mediator library. The integration will provide deep observability into request/response flows, event publishing, pipeline behaviors, and outbox processing while maintaining the library's core principles: Native AOT compatibility, zero-reflection design, and minimal performance overhead.

---

## Architecture Overview

### Current Architecture Analysis

The mediator library has several key execution paths that need instrumentation:

1. **Request Processing Pipeline**
    - `ISender.SendAsync<TRequest, TResponse>()` → Request handlers
    - `ProxyRequestHandler` → Pipeline behaviors → Actual handler
    - Support for regular requests, void requests, and streaming requests

2. **Event Publishing Pipeline**
    - `IPublisher.PublishAsync()` → Event dispatcher → Orchestrator → Handlers
    - `EventDispatcher.DispatchAsync()` → Pipeline behaviors → Event handlers
    - Sequential and concurrent orchestration strategies

3. **Outbox Pattern**
    - `IPublisher.PublishAsync(mode: PublishMode.Outbox)` → Outbox storage
    - `IPublisher.CommitAsync()` → Batch processing with retry logic
    - Dead-letter queue handling

4. **Context Propagation**
    - Existing `IContext` with `CorrelationId` and `Metadata`
    - Integration with OpenTelemetry context propagation

---

## Implementation Strategy

### Phase 1: Core Infrastructure (Foundation)

#### 1.1 New Package: `Mediator.OpenTelemetry`

**Purpose**: Separate package for OTel integration to keep core library lightweight and avoid forcing dependencies on consumers.

**Package Structure**:

```
src/Mediator.OpenTelemetry/
├── Mediator.OpenTelemetry.csproj
├── ActivitySources.cs                          # Central ActivitySource definitions
├── MediatorInstrumentation.cs                  # Main instrumentation class
├── MediatorInstrumentationOptions.cs           # Configuration options
├── DependencyInjectionExtensions.cs            # .AddMediatorOpenTelemetry()
├── Diagnostics/
│   ├── MediatorActivitySource.cs               # ActivitySource wrapper
│   ├── MediatorMetrics.cs                      # Metrics definitions
│   ├── ActivityEnricher.cs                     # Enrich activities with context
│   └── SemanticConventions.cs                  # OTel semantic conventions
├── Behaviors/
│   ├── TracingRequestPipelineBehavior.cs       # Request tracing behavior
│   ├── TracingEventPipelineBehavior.cs         # Event tracing behavior
│   └── TracingStreamRequestPipelineBehavior.cs # Streaming request tracing
└── Internals/
    ├── ActivityHelper.cs                       # Helper methods for Activity
    └── TagNames.cs                             # Standard tag names
```

**Dependencies**:

```xml
<PackageReference Include="OpenTelemetry" Version="1.10.0" />
<PackageReference Include="OpenTelemetry.Api" Version="1.10.0" />
<PackageReference Include="System.Diagnostics.DiagnosticSource" Version="9.0.0" />
```

---

#### 1.2 Core Components

##### **ActivitySources.cs**

```csharp
namespace UnambitiousFx.Mediator.OpenTelemetry;

public static class ActivitySources
{
    public const string MediatorActivitySourceName = "UnambitiousFx.Mediator";
    public const string Version = "1.0.0"; // Should match assembly version
    
    private static ActivitySource? _mediatorActivitySource;
    
    public static ActivitySource Mediator => 
        _mediatorActivitySource ??= new ActivitySource(MediatorActivitySourceName, Version);
}
```

##### **SemanticConventions.cs**

```csharp
// Define OTel semantic conventions for mediator operations
public static class SemanticConventions
{
    // Activity names
    public const string RequestActivity = "mediator.request";
    public const string EventActivity = "mediator.event";
    public const string StreamRequestActivity = "mediator.stream_request";
    public const string OutboxCommitActivity = "mediator.outbox.commit";
    
    // Attribute keys (following OTel semantic conventions)
    public const string RequestType = "mediator.request.type";
    public const string ResponseType = "mediator.response.type";
    public const string EventType = "mediator.event.type";
    public const string HandlerType = "mediator.handler.type";
    public const string BehaviorType = "mediator.behavior.type";
    public const string CorrelationId = "mediator.correlation_id";
    public const string PublishMode = "mediator.publish.mode";
    public const string OrchestrationMode = "mediator.orchestration.mode";
    public const string IsSuccess = "mediator.result.is_success";
    public const string ErrorMessage = "mediator.result.error";
    public const string OutboxEventCount = "mediator.outbox.event_count";
    public const string OutboxRetryAttempt = "mediator.outbox.retry_attempt";
    public const string OutboxDeadLetter = "mediator.outbox.dead_letter";
}
```

##### **MediatorInstrumentationOptions.cs**

```csharp
public sealed class MediatorInstrumentationOptions
{
    /// <summary>
    /// Enable distributed tracing for requests
    /// </summary>
    public bool EnableRequestTracing { get; set; } = true;
    
    /// <summary>
    /// Enable distributed tracing for events
    /// </summary>
    public bool EnableEventTracing { get; set; } = true;
    
    /// <summary>
    /// Enable distributed tracing for outbox operations
    /// </summary>
    public bool EnableOutboxTracing { get; set; } = true;
    
    /// <summary>
    /// Enable metrics collection
    /// </summary>
    public bool EnableMetrics { get; set; } = true;
    
    /// <summary>
    /// Record request/response payloads as activity tags (be careful with PII)
    /// </summary>
    public bool RecordPayloads { get; set; } = false;
    
    /// <summary>
    /// Record exception details in activities
    /// </summary>
    public bool RecordExceptions { get; set; } = true;
    
    /// <summary>
    /// Custom enricher for activities
    /// </summary>
    public Action<Activity, object>? Enrich { get; set; }
    
    /// <summary>
    /// Filter which requests/events to trace (for performance)
    /// </summary>
    public Func<Type, bool>? Filter { get; set; }
}
```

---

### Phase 2: Tracing Behaviors (Core Instrumentation)

#### 2.1 Request Tracing Behavior

**Purpose**: Wrap request execution in OpenTelemetry activities/spans

**Key Implementation Points**:

- Create a new Activity for each request
- Propagate parent Activity context from ambient context or IContext
- Enrich with request type, handler type, correlation ID
- Record success/failure status
- Handle exceptions and set error status

**File**: `Behaviors/TracingRequestPipelineBehavior.cs`

```csharp
public sealed class TracingRequestPipelineBehavior : IRequestPipelineBehavior
{
    private readonly IContext _context;
    private readonly MediatorInstrumentationOptions _options;
    
    // Implement both HandleAsync overloads
    
    public ValueTask<Result<TResponse>> HandleAsync<TRequest, TResponse>(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
        where TResponse : notnull
        where TRequest : IRequest<TResponse>
    {
        if (!_options.EnableRequestTracing) 
            return next();
            
        using var activity = ActivitySources.Mediator.StartActivity(
            SemanticConventions.RequestActivity,
            ActivityKind.Internal);
            
        if (activity is null)
            return next();
            
        EnrichActivity(activity, request);
        
        var result = await next();
        
        RecordResult(activity, result);
        
        return result;
    }
    
    private void EnrichActivity<TRequest>(Activity activity, TRequest request)
    {
        activity.SetTag(SemanticConventions.RequestType, typeof(TRequest).Name);
        activity.SetTag(SemanticConventions.CorrelationId, _context.CorrelationId);
        
        // Apply custom enrichment
        _options.Enrich?.Invoke(activity, request);
        
        // Optionally record payload
        if (_options.RecordPayloads)
        {
            activity.SetTag("mediator.request.payload", 
                JsonSerializer.Serialize(request));
        }
    }
}
```

#### 2.2 Event Tracing Behavior

**Purpose**: Wrap event execution in OpenTelemetry activities

**File**: `Behaviors/TracingEventPipelineBehavior.cs`

Similar structure to request tracing, but:

- Use `SemanticConventions.EventActivity` name
- Add publish mode tags
- Add orchestration strategy tags
- Link to parent request activity if available

#### 2.3 Stream Request Tracing Behavior

**Purpose**: Wrap streaming request execution with support for per-item tracing

**File**: `Behaviors/TracingStreamRequestPipelineBehavior.cs`

**Challenges**:

- Streams are IAsyncEnumerable - need to decide granularity
- Options: Single span for entire stream vs. span per item vs. events per item

**Recommended Approach**:

- Create parent activity for the stream request
- Emit events for each item yielded (not full spans to avoid overhead)
- Track total item count, errors, duration

---

### Phase 3: Metrics Collection

#### 3.1 Metrics Definitions

**File**: `Diagnostics/MediatorMetrics.cs`

```csharp
public sealed class MediatorMetrics : IDisposable
{
    private readonly Meter _meter;
    
    // Counters
    private readonly Counter<long> _requestCounter;
    private readonly Counter<long> _eventCounter;
    private readonly Counter<long> _requestErrorCounter;
    private readonly Counter<long> _eventErrorCounter;
    
    // Histograms
    private readonly Histogram<double> _requestDuration;
    private readonly Histogram<double> _eventDuration;
    private readonly Histogram<double> _outboxProcessingDuration;
    
    // Gauges (using ObservableGauge)
    private readonly ObservableGauge<int> _outboxPendingEvents;
    private readonly ObservableGauge<int> _outboxDeadLetterEvents;
    
    public MediatorMetrics(IMeterFactory meterFactory, IEventOutboxStorage outboxStorage)
    {
        _meter = meterFactory.Create("UnambitiousFx.Mediator");
        
        _requestCounter = _meter.CreateCounter<long>(
            "mediator.requests",
            description: "Total number of requests processed");
            
        _requestDuration = _meter.CreateHistogram<double>(
            "mediator.request.duration",
            unit: "ms",
            description: "Request processing duration");
            
        // ... initialize other metrics
        
        _outboxPendingEvents = _meter.CreateObservableGauge(
            "mediator.outbox.pending",
            () => outboxStorage.GetPendingCountAsync().Result,
            description: "Number of pending events in outbox");
    }
}
```

**Metrics to Track**:

| Metric Name                      | Type      | Description                  | Dimensions                     |
|----------------------------------|-----------|------------------------------|--------------------------------|
| `mediator.requests`              | Counter   | Total requests               | request_type, is_success       |
| `mediator.request.duration`      | Histogram | Request duration in ms       | request_type, handler_type     |
| `mediator.events`                | Counter   | Total events published       | event_type, publish_mode       |
| `mediator.event.duration`        | Histogram | Event processing duration    | event_type, orchestration_mode |
| `mediator.outbox.pending`        | Gauge     | Pending events in outbox     | -                              |
| `mediator.outbox.processed`      | Counter   | Events processed from outbox | is_success                     |
| `mediator.outbox.dead_letter`    | Counter   | Events moved to dead-letter  | event_type                     |
| `mediator.outbox.retry_attempts` | Histogram | Retry attempts distribution  | event_type                     |
| `mediator.behaviors.duration`    | Histogram | Behavior execution time      | behavior_type                  |

---

### Phase 4: Outbox Instrumentation

#### 4.1 Outbox Tracing Wrapper

**Purpose**: Instrument outbox operations without modifying core code

**Approach**: Create a decorator for `IEventOutboxStorage`

**File**: `Internals/TracedEventOutboxStorage.cs`

```csharp
internal sealed class TracedEventOutboxStorage : IEventOutboxStorage
{
    private readonly IEventOutboxStorage _inner;
    private readonly MediatorMetrics _metrics;
    
    public async ValueTask<Result> AddAsync(IEvent @event, CancellationToken ct)
    {
        using var activity = ActivitySources.Mediator.StartActivity(
            "mediator.outbox.add",
            ActivityKind.Internal);
            
        activity?.SetTag(SemanticConventions.EventType, @event.GetType().Name);
        
        var result = await _inner.AddAsync(@event, ct);
        
        if (result.IsSuccess)
            _metrics.RecordOutboxAdd(@event.GetType().Name);
            
        return result;
    }
    
    // Similar for other methods: GetPendingEventsAsync, MarkAsProcessedAsync, etc.
}
```

#### 4.2 CommitAsync Tracing

**Challenge**: The `Publisher.CommitAsync()` method needs tracing but can't be easily wrapped

**Solution**: Create an `IPublisher` decorator

**File**: `Internals/TracedPublisher.cs`

```csharp
internal sealed class TracedPublisher : IPublisher
{
    private readonly IPublisher _inner;
    
    public async ValueTask<Result> CommitAsync(CancellationToken ct)
    {
        using var activity = ActivitySources.Mediator.StartActivity(
            SemanticConventions.OutboxCommitActivity,
            ActivityKind.Internal);
            
        var stopwatch = ValueStopwatch.StartNew();
        var result = await _inner.CommitAsync(ct);
        var elapsed = stopwatch.GetElapsedTime();
        
        activity?.SetTag(SemanticConventions.IsSuccess, result.IsSuccess);
        _metrics.RecordOutboxCommit(elapsed.TotalMilliseconds, result.IsSuccess);
        
        return result;
    }
}
```

---

### Phase 5: Context Integration

#### 5.1 Activity Context Propagation

**Goal**: Integrate OpenTelemetry's Activity propagation with IContext

**Implementation**:

1. **Enhance Context Creation**
    - When a new `IContext` is created, capture `Activity.Current?.Id` and `Activity.Current?.TraceId`
    - Store in metadata

2. **Context Factory Enhancement**
    - Optionally add extension method to create context from Activity

**File**: `Extensions/ContextExtensions.cs`

```csharp
public static class ContextExtensions
{
    public static void EnrichFromActivity(this IContext context)
    {
        var activity = Activity.Current;
        if (activity is null) return;
        
        context.SetMetadata("Activity.TraceId", activity.TraceId.ToString());
        context.SetMetadata("Activity.SpanId", activity.SpanId.ToString());
        context.SetMetadata("Activity.ParentId", activity.ParentId);
    }
    
    public static void PropagateToActivity(this IContext context, Activity activity)
    {
        activity.SetTag(SemanticConventions.CorrelationId, context.CorrelationId);
        
        foreach (var (key, value) in context.Metadata)
        {
            activity.SetTag($"mediator.context.{key}", value?.ToString());
        }
    }
}
```

---

### Phase 6: Registration & Configuration

#### 6.1 Dependency Injection Extensions

**File**: `DependencyInjectionExtensions.cs`

```csharp
public static class OpenTelemetryMediatorExtensions
{
    /// <summary>
    /// Adds OpenTelemetry instrumentation for UnambitiousFx.Mediator
    /// </summary>
    public static IDependencyInjectionBuilder AddMediatorOpenTelemetry(
        this IDependencyInjectionBuilder builder,
        Action<MediatorInstrumentationOptions>? configure = null)
    {
        var options = new MediatorInstrumentationOptions();
        configure?.Invoke(options);
        
        builder.Services.AddSingleton(options);
        
        if (options.EnableRequestTracing)
        {
            builder.Services.AddSingleton<IRequestPipelineBehavior, 
                TracingRequestPipelineBehavior>();
        }
        
        if (options.EnableEventTracing)
        {
            builder.Services.AddSingleton<IEventPipelineBehavior, 
                TracingEventPipelineBehavior>();
        }
        
        if (options.EnableMetrics)
        {
            builder.Services.AddSingleton<MediatorMetrics>();
        }
        
        // Wrap outbox storage with tracing decorator
        if (options.EnableOutboxTracing)
        {
            builder.Services.Decorate<IEventOutboxStorage, TracedEventOutboxStorage>();
            builder.Services.Decorate<IPublisher, TracedPublisher>();
        }
        
        return builder;
    }
    
    /// <summary>
    /// Adds MediatorInstrumentation to OpenTelemetry TracerProvider
    /// </summary>
    public static TracerProviderBuilder AddMediatorInstrumentation(
        this TracerProviderBuilder builder)
    {
        return builder.AddSource(ActivitySources.MediatorActivitySourceName);
    }
    
    /// <summary>
    /// Adds Mediator metrics to OpenTelemetry MeterProvider
    /// </summary>
    public static MeterProviderBuilder AddMediatorInstrumentation(
        this MeterProviderBuilder builder)
    {
        return builder.AddMeter("UnambitiousFx.Mediator");
    }
}
```

---

### Phase 7: Examples & Documentation

#### 7.1 Example Application

**Create**: `examples/mediator/WebApiWithTelemetry/`

**Purpose**: Show complete setup with:

- Jaeger/Zipkin export
- Prometheus metrics export
- Console exporter for debugging
- Integration with ASP.NET Core distributed tracing

**Key Files**:

```
WebApiWithTelemetry/
├── Program.cs                      # Setup OTel with mediator
├── Controllers/
│   └── OrdersController.cs         # Trigger requests/events
├── Commands/
│   └── CreateOrderCommand.cs
├── Events/
│   └── OrderCreatedEvent.cs
├── Handlers/
│   ├── CreateOrderHandler.cs
│   └── OrderCreatedEventHandler.cs
└── docker-compose.yml              # Jaeger + Prometheus
```

**Program.cs Setup**:

```csharp
builder.Services
    .AddMediator(cfg => cfg
        .RegisterHandlersFromAssembly(typeof(Program).Assembly))
    .AddMediatorOpenTelemetry(opts =>
    {
        opts.EnableRequestTracing = true;
        opts.EnableEventTracing = true;
        opts.EnableMetrics = true;
        opts.RecordPayloads = false; // Be careful with PII
    });

builder.Services
    .AddOpenTelemetry()
    .WithTracing(tracing => tracing
        .AddAspNetCoreInstrumentation()
        .AddMediatorInstrumentation()  // ← Our extension
        .AddJaegerExporter())
    .WithMetrics(metrics => metrics
        .AddAspNetCoreInstrumentation()
        .AddMediatorInstrumentation()  // ← Our extension
        .AddPrometheusExporter());
```

#### 7.2 Documentation Pages

**Create**: `docs/docs/mediator/opentelemetry.markdown`

**Sections**:

1. Overview & Benefits
2. Installation
3. Quick Start
4. Configuration Options
5. Trace Examples (with Jaeger screenshots)
6. Metrics Reference
7. Custom Enrichment
8. Performance Considerations
9. Troubleshooting

---

## Technical Considerations

### 1. Native AOT Compatibility

**Challenge**: OpenTelemetry libraries use reflection in some places

**Solution**:

- Use only OTel APIs that are AOT-compatible
- Avoid `Activity.SetTag<T>(string, T)` generic overloads if problematic
- Test with `PublishAot=true` in example project
- Document any limitations

**Verification**:

```bash
dotnet publish -c Release /p:PublishAot=true
```

### 2. Performance Impact

**Concerns**:

- Activity creation overhead
- Tag serialization cost
- Metric recording overhead

**Mitigations**:

- Activities are only created if a listener is registered (ActivitySource is efficient)
- Provide `Filter` option to skip tracing for specific types
- Use `ActivitySource.HasListeners()` check before enrichment
- Keep tags minimal by default
- Lazy evaluation for expensive tag values

**Benchmarking**:

- Add benchmarks to `MediatorBenchmark` project
- Compare overhead with/without OTel enabled
- Target: <5% overhead when tracing is active

### 3. Sampling

**Strategy**: Delegate to OpenTelemetry's built-in sampling

- Head-based sampling (configured at TracerProvider level)
- Users can configure `ParentBased`, `AlwaysOn`, `AlwaysOff`, `TraceIdRatioBased`

### 4. Error Handling

**Principle**: Never throw from instrumentation code

**Implementation**:

- Wrap all OTel calls in try-catch
- Log errors to `ILogger` if available
- Gracefully degrade (continue without tracing)

```csharp
try
{
    activity?.SetTag(key, value);
}
catch (Exception ex)
{
    // Log but don't propagate
    _logger?.LogWarning(ex, "Failed to set activity tag");
}
```

---

## Testing Strategy

### Unit Tests

**Project**: `test/Mediator.OpenTelemetry.Tests/`

**Test Categories**:

1. **Behavior Tests**
    - Verify activities are created with correct names
    - Verify tags are set correctly
    - Verify success/failure status is recorded

2. **Metrics Tests**
    - Verify counters increment
    - Verify histograms record values
    - Verify gauges return correct values

3. **Context Propagation Tests**
    - Verify parent-child relationships
    - Verify baggage propagation
    - Verify trace context across async boundaries

4. **Configuration Tests**
    - Verify enable/disable flags work
    - Verify filter function works
    - Verify custom enrichment works

**Test Infrastructure**:

```csharp
// Use InMemoryExporter for testing
services.AddOpenTelemetry()
    .WithTracing(b => b
        .AddInMemoryExporter(exportedActivities));

// Assert on exported activities
Assert.Single(exportedActivities);
Assert.Equal("mediator.request", exportedActivities[0].DisplayName);
```

### Integration Tests

**Project**: `test/Mediator.OpenTelemetry.IntegrationTests/`

**Tests**:

- Full request/event flow with OTel
- Outbox pattern with retries
- ASP.NET Core integration
- Multiple handlers and behaviors

### Performance Tests

**Project**: `benchmarks/MediatorBenchmark/`

**New Benchmarks**:

```csharp
[Benchmark(Baseline = true)]
public Task SendRequest_NoTracing() { }

[Benchmark]
public Task SendRequest_WithTracing_NoListener() { }

[Benchmark]
public Task SendRequest_WithTracing_ActiveListener() { }
```

---

## Migration Path for Existing Users

### Non-Breaking Changes

All changes are **additive and opt-in**:

- New package: `Mediator.OpenTelemetry`
- Existing `Mediator` package unchanged
- No breaking changes to `IContext`, `ISender`, `IPublisher`

### Adoption Steps

1. **Add Package**
   ```bash
   dotnet add package UnambitiousFx.Mediator.OpenTelemetry
   ```

2. **Register Instrumentation**
   ```csharp
   builder.Services
       .AddMediator(...)
       .AddMediatorOpenTelemetry();
   ```

3. **Configure OpenTelemetry**
   ```csharp
   builder.Services
       .AddOpenTelemetry()
       .WithTracing(t => t.AddMediatorInstrumentation())
       .WithMetrics(m => m.AddMediatorInstrumentation());
   ```

---

## Deliverables

### Code Deliverables

1. ✅ `src/Mediator.OpenTelemetry/` - New package with all instrumentation code
2. ✅ `test/Mediator.OpenTelemetry.Tests/` - Comprehensive unit tests
3. ✅ `test/Mediator.OpenTelemetry.IntegrationTests/` - Integration tests
4. ✅ `examples/mediator/WebApiWithTelemetry/` - Working example with Jaeger
5. ✅ `benchmarks/MediatorBenchmark/` - Performance benchmarks with OTel

### Documentation Deliverables

1. ✅ `docs/docs/mediator/opentelemetry.markdown` - Main documentation
2. ✅ `README.md` update in `Mediator.OpenTelemetry` package
3. ✅ Update main `README.md` with OTel mention
4. ✅ Update roadmap to mark as ✅ Implemented

---

## Success Criteria

### Functional Requirements

- ✅ Distributed tracing works for requests, events, and streaming
- ✅ Metrics are collected and exportable
- ✅ Outbox operations are instrumented
- ✅ Context propagates correctly across async boundaries
- ✅ Compatible with ASP.NET Core distributed tracing

### Non-Functional Requirements

- ✅ Native AOT compatible
- ✅ Performance overhead <5% when active
- ✅ Zero overhead when disabled (ActivitySource.HasListeners() check)
- ✅ No breaking changes to existing APIs
- ✅ Comprehensive documentation with examples

---

## Future Enhancements (Post-MVP)

### V1.1 - Enhanced Metrics

- Per-handler success rate metrics
- P50/P95/P99 latency percentiles
- Concurrent event processing metrics
- Dead-letter queue metrics with reasons

### V1.2 - Advanced Tracing

- Automatic error sampling (always sample on errors)
- Span events for pipeline behavior execution
- Span links between related events
- Custom sampling strategies (e.g., sample slow requests)

### V1.3 - Logs Integration

- Correlate `ILogger` logs with traces using trace_id/span_id
- Structured logging with OTel semantic conventions
- Log-based sampling (sample traces that have ERROR logs)

### V1.4 - Cloud-Native Features

- W3C Baggage propagation for cross-service correlation
- Integration with cloud provider APM (AWS X-Ray, Azure App Insights, GCP Cloud Trace)
- Auto-instrumentation for message brokers (when distributed event bus is added)

---

## Timeline Estimate

| Phase                           | Effort         | Dependencies |
|---------------------------------|----------------|--------------|
| Phase 1: Core Infrastructure    | 2-3 days       | None         |
| Phase 2: Tracing Behaviors      | 3-4 days       | Phase 1      |
| Phase 3: Metrics Collection     | 2-3 days       | Phase 1      |
| Phase 4: Outbox Instrumentation | 2 days         | Phase 2, 3   |
| Phase 5: Context Integration    | 1-2 days       | Phase 2      |
| Phase 6: Registration & Config  | 1 day          | All phases   |
| Phase 7: Examples & Docs        | 3-4 days       | All phases   |
| Testing & Refinement            | 3-4 days       | All phases   |
| **Total**                       | **17-25 days** | -            |

---

## Open Questions

1. **Payload Recording**: Should we serialize request/response objects by default, or only with opt-in?
    - **Recommendation**: Opt-in only, with clear warnings about PII/secrets

2. **Stream Item Tracing**: Should each item in a stream get a span, or just events?
    - **Recommendation**: Events per item by default, spans opt-in for debugging

3. **Behavior Tracing**: Should each behavior get a separate span?
    - **Recommendation**: No by default (too noisy), but add opt-in via metadata tag

4. **Activity Naming**: Use operation-based names (e.g., `CreateOrder`) or generic (`mediator.request`)?
    - **Recommendation**: Generic by default, add operation name as tag, allow customization

5. **Metrics Cardinality**: How to handle high-cardinality dimensions (e.g., specific request types)?
    - **Recommendation**: Document best practices, provide aggregation options

---

## References

- [OpenTelemetry Semantic Conventions](https://opentelemetry.io/docs/specs/semconv/)
- [.NET Distributed Tracing](https://learn.microsoft.com/en-us/dotnet/core/diagnostics/distributed-tracing)
- [OpenTelemetry .NET SDK](https://github.com/open-telemetry/opentelemetry-dotnet)
- [Activity API Best Practices](https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.activity)

---

_This plan provides a comprehensive, production-ready approach to OpenTelemetry integration while maintaining the library's core principles and performance characteristics._
