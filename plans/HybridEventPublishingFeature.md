# Plan: Hybrid Event Publishing - Local and External Event Distribution

## Feature Name

**"Hybrid Event Publishing"** (or **"Distributed Event Publishing"**)

## Overview

This feature enables a modular monolith to automatically publish events to both:

1. **Local handlers** (in-process event handlers within the same application)
2. **External listeners** (microservices or other processes via a message broker)

The key innovation is **automatic routing**: the mediator determines which events have external listeners and publishes them to a message bus without requiring manual intervention in every handler.

## Business Use Case

### Scenario: Order Processing in Modular Monolith + Microservices

```
┌─────────────────────────────────────────────────┐
│         Modular Monolith Process                │
│                                                  │
│  ┌──────────────────────────────────┐           │
│  │ Module MA: Orders                │           │
│  │                                   │           │
│  │  CreateOrderCommand              │           │
│  │       ↓                          │           │
│  │  CreateOrderHandler               │           │
│  │       ↓                          │           │
│  │  Publishes: OrderCreatedEvent    │           │
│  └──────────────────────────────────┘           │
│                   ↓                              │
│         Mediator (Hybrid Publisher)              │
│                   ↓                              │
│          ┌────────┴────────┐                    │
│          ↓                 ↓                     │
│  ┌─────────────┐   ┌──────────────────┐        │
│  │ Local       │   │ Message Bus       │────────┼──→ External Service S1
│  │ Handlers    │   │ Publisher         │        │    (Notification Service)
│  │             │   │ (via IEventBroker)│────────┼──→ External Service S2
│  │ Module MB:  │   └──────────────────┘        │    (Analytics Service)
│  │ Inventory   │                                │
│  │ Handler     │                                │
│  └─────────────┘                                │
└─────────────────────────────────────────────────┘
```

### Flow

1. **Command C1** (`CreateOrderCommand`) is sent to Module MA
2. **Handler** processes command and publishes `OrderCreatedEvent`
3. **Mediator** automatically:
    - Dispatches to **local handlers** (Module MB's inventory handler)
    - Publishes to **message bus** (for external services S1 & S2)
4. **External services** receive the event via the message broker

## Current State Analysis

### Existing Event Infrastructure

✅ **Already Has:**

- `IPublisher` - publishes events
- `IEventDispatcher` - dispatches to local handlers
- `IEventHandler<TEvent>` - local event handlers
- `IEventPipelineBehavior` - event pipeline behaviors
- `PublishMode` - (Now, Outbox, Default)
- `IEventOutboxStorage` - outbox pattern support

❌ **Missing:**

- External event broker abstraction
- Metadata to mark events as having external listeners
- Automatic routing logic
- Configuration API to declare external listeners

## Solution Architecture

### Core Concept: Event Distribution Strategy

Each event can have:

- **0+ Local Handlers** (in-process)
- **0 or 1 External Broker** (out-of-process)

The mediator automatically:

1. Always dispatches to local handlers
2. **Additionally** publishes to external broker if configured

### Key Design Principles

1. **Non-Breaking**: Existing events work exactly as before
2. **Opt-In**: Events are local-only unless explicitly marked as external
3. **Separation of Concerns**: Message broker details hidden behind abstraction
4. **Testable**: Can mock broker for unit tests
5. **Observable**: OpenTelemetry/logging integration points

## Detailed Design

### 1. Event Broker Abstraction

#### 1.1 Core Interface

**File**: `src/Mediator.Abstractions/IEventBroker.cs`

```csharp
namespace UnambitiousFx.Mediator.Abstractions;

/// <summary>
/// Abstraction for publishing events to external message brokers.
/// Implementations handle the specifics of the underlying broker (RabbitMQ, Azure Service Bus, Kafka, etc.)
/// </summary>
public interface IEventBroker
{
    /// <summary>
    /// Publishes an event to the external message broker.
    /// </summary>
    /// <typeparam name="TEvent">The type of event to publish.</typeparam>
    /// <param name="event">The event instance to publish.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Result indicating success or failure.</returns>
    ValueTask<Result> PublishAsync<TEvent>(
        TEvent @event,
        CancellationToken cancellationToken = default)
        where TEvent : class, IEvent;
}
```

#### 1.2 Null Implementation (Default)

**File**: `src/Mediator/NullEventBroker.cs`

```csharp
namespace UnambitiousFx.Mediator;

/// <summary>
/// No-op implementation of IEventBroker used when no external broker is configured.
/// </summary>
internal sealed class NullEventBroker : IEventBroker
{
    public ValueTask<Result> PublishAsync<TEvent>(
        TEvent @event,
        CancellationToken cancellationToken = default)
        where TEvent : class, IEvent
    {
        // No external publishing - this is intentional
        return ValueTask.FromResult(Result.Success());
    }
}
```

### 2. Event Metadata for External Publishing

#### 2.1 Marker Attribute

**File**: `src/Mediator.Abstractions/ExternalEventAttribute.cs`

```csharp
namespace UnambitiousFx.Mediator.Abstractions;

/// <summary>
/// Marks an event as having external listeners (e.g., microservices, external modules).
/// Events with this attribute will be automatically published to the configured message broker
/// in addition to being dispatched to local handlers.
/// </summary>
/// <remarks>
/// This attribute is used to enable hybrid event publishing where events are handled both
/// locally (in-process) and externally (via message broker).
/// </remarks>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class ExternalEventAttribute : Attribute
{
    /// <summary>
    /// Gets the optional topic or exchange name for routing the event.
    /// If not specified, the event type name will be used.
    /// </summary>
    public string? Topic { get; init; }

    /// <summary>
    /// Gets or sets whether external publishing should fail fast.
    /// If true, failure to publish externally will fail the entire operation.
    /// If false (default), external publishing failures are logged but don't fail the operation.
    /// </summary>
    public bool FailFast { get; init; } = false;
}
```

#### 2.2 Alternative: Interface-based Approach

**File**: `src/Mediator.Abstractions/IExternalEvent.cs`

```csharp
namespace UnambitiousFx.Mediator.Abstractions;

/// <summary>
/// Marker interface for events that should be published to external systems.
/// Implementing this interface indicates that the event has external listeners
/// and should be published to the configured message broker.
/// </summary>
public interface IExternalEvent : IEvent
{
    /// <summary>
    /// Gets the optional topic or routing key for the event.
    /// Return null to use the default event type name.
    /// </summary>
    string? GetTopic() => null;
}
```

**Design Decision**: Recommend **Attribute-based** approach for flexibility:

- Can be added without changing event signatures
- Doesn't force interface implementation
- Can still combine with interface if needed
- Better for backward compatibility

### 3. External Event Publishing Pipeline Behavior

#### 3.1 Hybrid Publishing Behavior

**File**: `src/Mediator/Pipelines/ExternalEventPublishingBehavior.cs`

```csharp
namespace UnambitiousFx.Mediator.Pipelines;

/// <summary>
/// Pipeline behavior that automatically publishes events to external message broker
/// when they are marked with <see cref="ExternalEventAttribute"/> or implement <see cref="IExternalEvent"/>.
/// </summary>
/// <remarks>
/// This behavior:
/// 1. Checks if the event should be published externally
/// 2. Dispatches to local handlers (via next())
/// 3. Publishes to external broker if configured
/// 4. Handles failures based on FailFast configuration
/// </remarks>
public sealed class ExternalEventPublishingBehavior : IEventPipelineBehavior
{
    private readonly IEventBroker _eventBroker;
    private readonly ILogger<ExternalEventPublishingBehavior>? _logger;
    private readonly ExternalEventPublishingOptions _options;

    public ExternalEventPublishingBehavior(
        IEventBroker eventBroker,
        IOptions<ExternalEventPublishingOptions> options,
        ILogger<ExternalEventPublishingBehavior>? logger = null)
    {
        _eventBroker = eventBroker;
        _logger = logger;
        _options = options.Value;
    }

    public async ValueTask<Result> HandleAsync<TEvent>(
        TEvent @event,
        EventHandlerDelegate next,
        CancellationToken cancellationToken = default)
        where TEvent : IEvent
    {
        // First, execute local handlers
        var localResult = await next();

        // Check if event should be published externally
        var shouldPublishExternally = ShouldPublishExternally<TEvent>(@event, out var failFast);

        if (!shouldPublishExternally)
        {
            return localResult; // Local-only event
        }

        // Publish to external broker
        try
        {
            var externalResult = await _eventBroker.PublishAsync(@event, cancellationToken);

            if (externalResult.IsFailure)
            {
                _logger?.LogWarning(
                    "Failed to publish event {EventType} to external broker: {Error}",
                    typeof(TEvent).Name,
                    externalResult.Error);

                // Decide if we should fail the entire operation
                if (failFast)
                {
                    return localResult.IsSuccess
                        ? externalResult // External failure becomes the result
                        : Result.Failure($"{localResult.Error}; External publish also failed: {externalResult.Error}");
                }
            }
            else
            {
                _logger?.LogDebug(
                    "Successfully published event {EventType} to external broker",
                    typeof(TEvent).Name);
            }
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex,
                "Exception while publishing event {EventType} to external broker",
                typeof(TEvent).Name);

            if (failFast)
            {
                return Result.Failure($"External publish failed: {ex.Message}");
            }
        }

        // Return local result (external failures are logged but don't fail unless FailFast)
        return localResult;
    }

    private bool ShouldPublishExternally<TEvent>(TEvent @event, out bool failFast)
        where TEvent : IEvent
    {
        failFast = false;

        // Strategy 1: Check if event implements IExternalEvent
        if (@event is IExternalEvent)
        {
            return true;
        }

        // Strategy 2: Check for ExternalEventAttribute
        var attribute = typeof(TEvent).GetCustomAttribute<ExternalEventAttribute>();
        if (attribute != null)
        {
            failFast = attribute.FailFast;
            return true;
        }

        // Strategy 3: Check configuration-based registration
        if (_options.IsRegisteredAsExternal(typeof(TEvent)))
        {
            failFast = _options.GetFailFast(typeof(TEvent));
            return true;
        }

        return false;
    }
}
```

#### 3.2 Options

**File**: `src/Mediator/ExternalEventPublishingOptions.cs`

```csharp
namespace UnambitiousFx.Mediator;

/// <summary>
/// Configuration options for external event publishing.
/// </summary>
public sealed class ExternalEventPublishingOptions
{
    private readonly Dictionary<Type, ExternalEventConfig> _externalEvents = new();

    /// <summary>
    /// Registers an event type as having external listeners.
    /// </summary>
    public void RegisterExternalEvent<TEvent>(
        string? topic = null,
        bool failFast = false)
        where TEvent : class, IEvent
    {
        _externalEvents[typeof(TEvent)] = new ExternalEventConfig
        {
            Topic = topic,
            FailFast = failFast
        };
    }

    internal bool IsRegisteredAsExternal(Type eventType)
        => _externalEvents.ContainsKey(eventType);

    internal bool GetFailFast(Type eventType)
        => _externalEvents.TryGetValue(eventType, out var config) && config.FailFast;

    internal string? GetTopic(Type eventType)
        => _externalEvents.TryGetValue(eventType, out var config) ? config.Topic : null;

    private sealed class ExternalEventConfig
    {
        public string? Topic { get; init; }
        public bool FailFast { get; init; }
    }
}
```

### 4. Configuration API

#### 4.1 Mediator Configuration Extensions

**File**: `src/Mediator/IMediatorConfig.cs` (additions)

```csharp
public interface IMediatorConfig
{
    // ...existing methods...

    /// <summary>
    /// Enables hybrid event publishing, allowing events to be published to both
    /// local handlers and external message brokers.
    /// </summary>
    /// <param name="configureOptions">Optional action to configure external event publishing options.</param>
    /// <returns>The current configuration instance for fluent chaining.</returns>
    IMediatorConfig EnableHybridEventPublishing(
        Action<ExternalEventPublishingOptions>? configureOptions = null);

    /// <summary>
    /// Registers a custom event broker implementation.
    /// </summary>
    /// <typeparam name="TEventBroker">The event broker implementation type.</typeparam>
    /// <param name="lifetime">The service lifetime (default: Scoped).</param>
    /// <returns>The current configuration instance for fluent chaining.</returns>
    IMediatorConfig RegisterEventBroker<TEventBroker>(
        ServiceLifetime lifetime = ServiceLifetime.Scoped)
        where TEventBroker : class, IEventBroker;

    /// <summary>
    /// Registers a custom event broker using a factory function.
    /// </summary>
    /// <param name="factory">Factory function to create the event broker.</param>
    /// <param name="lifetime">The service lifetime (default: Scoped).</param>
    /// <returns>The current configuration instance for fluent chaining.</returns>
    IMediatorConfig RegisterEventBroker(
        Func<IServiceProvider, IEventBroker> factory,
        ServiceLifetime lifetime = ServiceLifetime.Scoped);
}
```

#### 4.2 Implementation

**File**: `src/Mediator/MediatorConfig.cs` (additions)

```csharp
public IMediatorConfig EnableHybridEventPublishing(
    Action<ExternalEventPublishingOptions>? configureOptions = null)
{
    // Register options
    if (configureOptions != null)
    {
        _services.Configure(configureOptions);
    }
    else
    {
        _services.Configure<ExternalEventPublishingOptions>(_ => { });
    }

    // Register the pipeline behavior
    RegisterEventPipelineBehavior<ExternalEventPublishingBehavior>();

    // Register default null broker if none registered
    if (!_services.Any(sd => sd.ServiceType == typeof(IEventBroker)))
    {
        _services.TryAddScoped<IEventBroker, NullEventBroker>();
    }

    return this;
}

public IMediatorConfig RegisterEventBroker<TEventBroker>(
    ServiceLifetime lifetime = ServiceLifetime.Scoped)
    where TEventBroker : class, IEventBroker
{
    // Remove any existing registrations
    var existing = _services.FirstOrDefault(sd => sd.ServiceType == typeof(IEventBroker));
    if (existing != null)
    {
        _services.Remove(existing);
    }

    _services.Add(new ServiceDescriptor(
        typeof(IEventBroker),
        typeof(TEventBroker),
        lifetime));

    return this;
}

public IMediatorConfig RegisterEventBroker(
    Func<IServiceProvider, IEventBroker> factory,
    ServiceLifetime lifetime = ServiceLifetime.Scoped)
{
    // Remove any existing registrations
    var existing = _services.FirstOrDefault(sd => sd.ServiceType == typeof(IEventBroker));
    if (existing != null)
    {
        _services.Remove(existing);
    }

    _services.Add(new ServiceDescriptor(
        typeof(IEventBroker),
        factory,
        lifetime));

    return this;
}
```

### 5. Usage Examples

#### 5.1 Simple Attribute-Based

```csharp
// Event definition
[ExternalEvent(Topic = "orders.created", FailFast = false)]
public sealed class OrderCreatedEvent : IEvent
{
    public Guid OrderId { get; init; }
    public decimal TotalAmount { get; init; }
    public DateTime CreatedAt { get; init; }
}

// Configuration
services.AddMediator(cfg =>
{
    cfg.RegisterRequestHandler<CreateOrderHandler, CreateOrderCommand>();
    cfg.RegisterEventHandler<LocalInventoryHandler, OrderCreatedEvent>();
    
    // Enable hybrid publishing
    cfg.EnableHybridEventPublishing();
    
    // Register your broker implementation
    cfg.RegisterEventBroker<RabbitMqEventBroker>();
});
```

#### 5.2 Interface-Based

```csharp
// Event definition
public sealed class PaymentProcessedEvent : IExternalEvent
{
    public Guid PaymentId { get; init; }
    public decimal Amount { get; init; }

    public string? GetTopic() => "payments.processed";
}

// Same configuration as above
```

#### 5.3 Configuration-Based (Runtime)

```csharp
services.AddMediator(cfg =>
{
    cfg.EnableHybridEventPublishing(options =>
    {
        // Register events as external at configuration time
        options.RegisterExternalEvent<OrderCreatedEvent>(
            topic: "orders.created",
            failFast: false);
        
        options.RegisterExternalEvent<PaymentProcessedEvent>(
            topic: "payments.processed",
            failFast: true); // Payment events must publish externally
    });

    cfg.RegisterEventBroker<RabbitMqEventBroker>();
});
```

#### 5.4 Custom Broker Implementation Example

```csharp
// Example RabbitMQ implementation
public sealed class RabbitMqEventBroker : IEventBroker
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly ILogger<RabbitMqEventBroker> _logger;

    public RabbitMqEventBroker(
        IConnection connection,
        ILogger<RabbitMqEventBroker> logger)
    {
        _connection = connection;
        _channel = _connection.CreateModel();
        _logger = logger;
    }

    public async ValueTask<Result> PublishAsync<TEvent>(
        TEvent @event,
        CancellationToken cancellationToken = default)
        where TEvent : class, IEvent
    {
        try
        {
            var eventType = typeof(TEvent);
            var topic = GetTopic(@event);
            var body = JsonSerializer.SerializeToUtf8Bytes(@event);

            var properties = _channel.CreateBasicProperties();
            properties.ContentType = "application/json";
            properties.Type = eventType.Name;
            properties.Persistent = true;

            _channel.BasicPublish(
                exchange: "events",
                routingKey: topic,
                basicProperties: properties,
                body: body);

            _logger.LogInformation(
                "Published event {EventType} to topic {Topic}",
                eventType.Name,
                topic);

            return await ValueTask.FromResult(Result.Success());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish event to RabbitMQ");
            return Result.Failure($"RabbitMQ publish failed: {ex.Message}");
        }
    }

    private string GetTopic<TEvent>(TEvent @event)
        where TEvent : class, IEvent
    {
        // Check IExternalEvent first
        if (@event is IExternalEvent externalEvent)
        {
            var customTopic = externalEvent.GetTopic();
            if (!string.IsNullOrWhiteSpace(customTopic))
                return customTopic;
        }

        // Check attribute
        var attribute = typeof(TEvent).GetCustomAttribute<ExternalEventAttribute>();
        if (attribute?.Topic != null)
            return attribute.Topic;

        // Default: use event type name
        return typeof(TEvent).Name;
    }
}
```

## Implementation Plan

### Phase 1: Core Infrastructure (Week 1)

#### 1.1 Create Core Abstractions

- [ ] Create `IEventBroker` interface
- [ ] Create `NullEventBroker` implementation
- [ ] Create `ExternalEventAttribute`
- [ ] Create `IExternalEvent` interface
- [ ] Create `ExternalEventPublishingOptions`

#### 1.2 Pipeline Behavior

- [ ] Create `ExternalEventPublishingBehavior`
- [ ] Add logging support
- [ ] Add error handling (FailFast logic)

#### 1.3 Configuration API

- [ ] Add methods to `IMediatorConfig`
- [ ] Implement in `MediatorConfig`
- [ ] Add service registration logic

### Phase 2: Testing (Week 1-2)

#### 2.1 Unit Tests

**File**: `test/Mediator.Tests/HybridEventPublishing/HybridEventPublishingTests.cs`

Test scenarios:

- [ ] Local-only event (no external attribute) → only local handlers called
- [ ] External event (with attribute) → both local and external called
- [ ] External event with FailFast=true → failure in broker fails operation
- [ ] External event with FailFast=false → failure in broker logged but continues
- [ ] IExternalEvent interface approach → external publishing works
- [ ] Configuration-based registration → works without attribute
- [ ] Custom topic routing → uses specified topic
- [ ] No broker registered → uses NullEventBroker (no-op)

#### 2.2 Integration Tests

**File**: `test/Mediator.Tests/HybridEventPublishing/IntegrationTests.cs`

- [ ] Full end-to-end with mock broker
- [ ] Multiple events, mixed local/external
- [ ] Outbox mode + external publishing
- [ ] Exception handling scenarios

### Phase 3: Example Implementations (Week 2)

#### 3.1 Create Example Project

**File**: `examples/ConsoleApp/HybridEventPublishing/`

Structure:

```
HybridEventPublishing/
  ├── Events/
  │   ├── OrderCreatedEvent.cs (external)
  │   ├── OrderValidatedEvent.cs (local only)
  │   └── PaymentProcessedEvent.cs (external)
  ├── Handlers/
  │   ├── LocalInventoryHandler.cs
  │   └── LocalAuditHandler.cs
  ├── Brokers/
  │   ├── InMemoryEventBroker.cs (for demo)
  │   └── RabbitMqEventBroker.cs (real implementation)
  ├── Program.cs
  └── README.md
```

#### 3.2 Demo Scenarios

- [ ] Order creation flow (command → event → local + external)
- [ ] Payment processing (with FailFast=true)
- [ ] Mixed event publishing (some local, some external)

### Phase 4: Documentation (Week 2-3)

#### 4.1 Feature Documentation

**File**: `docs/docs/mediator/advanced/hybrid-event-publishing.markdown`

Sections:

- Overview
- Use Cases (modular monolith + microservices)
- Configuration
- Event Marking Strategies (attribute vs interface vs config)
- Custom Broker Implementation
- Error Handling
- Best Practices
- OpenTelemetry Integration

#### 4.2 Broker Implementation Guides

**Files**:

- `docs/docs/mediator/brokers/rabbitmq-broker.markdown`
- `docs/docs/mediator/brokers/azure-service-bus-broker.markdown`
- `docs/docs/mediator/brokers/kafka-broker.markdown`
- `docs/docs/mediator/brokers/custom-broker-guide.markdown`

#### 4.3 Migration Guide

**File**: `docs/docs/mediator/migration/hybrid-events-migration.markdown`

- How to migrate existing events
- When to use hybrid vs local-only
- Performance considerations

### Phase 5: Advanced Features (Optional - Week 3-4)

#### 5.1 OpenTelemetry Integration

- [ ] Add activity tags for external publishing
- [ ] Trace external broker calls
- [ ] Metrics for publish success/failure rates

#### 5.2 Outbox Integration

- [ ] Support external publishing from outbox
- [ ] Ensure transactional consistency

#### 5.3 Retry Policies

- [ ] Add retry logic for external publishing
- [ ] Configurable backoff strategies

#### 5.4 Dead Letter Queue Support

- [ ] Track failed external publishes
- [ ] DLQ for external broker failures

## Configuration Patterns

### Pattern 1: Attribute-Based (Recommended for Most Cases)

**When to use**: Clear, declarative, event definition contains its own metadata

```csharp
[ExternalEvent(Topic = "orders.created")]
public sealed class OrderCreatedEvent : IEvent { }
```

**Pros**:

- Self-documenting
- No additional configuration needed
- Easy to see which events are external

**Cons**:

- Requires recompilation to change
- Can't be overridden at runtime

### Pattern 2: Interface-Based (For Dynamic Topics)

**When to use**: Topic/routing needs runtime information

```csharp
public sealed class OrderCreatedEvent : IExternalEvent
{
    public string Region { get; init; }
    
    public string? GetTopic() => $"orders.created.{Region.ToLower()}";
}
```

**Pros**:

- Dynamic topic calculation
- Runtime flexibility

**Cons**:

- Adds interface to event
- More complex

### Pattern 3: Configuration-Based (For Third-Party Events)

**When to use**: Can't modify event source code

```csharp
cfg.EnableHybridEventPublishing(options =>
{
    options.RegisterExternalEvent<ThirdPartyEvent>(topic: "external.events");
});
```

**Pros**:

- Works with events you don't own
- Runtime reconfiguration

**Cons**:

- Less discoverable
- Configuration overhead

## Broker Implementation Examples

### Minimal Broker Interface Implementations

#### RabbitMQ

```bash
# NuGet package
dotnet add package RabbitMQ.Client
```

#### Azure Service Bus

```bash
# NuGet package
dotnet add package Azure.Messaging.ServiceBus
```

#### Apache Kafka

```bash
# NuGet package
dotnet add package Confluent.Kafka
```

#### AWS SNS/SQS

```bash
# NuGet package
dotnet add package AWSSDK.SimpleNotificationService
```

## Testing Strategy

### Unit Test Mock Broker

```csharp
public sealed class MockEventBroker : IEventBroker
{
    public List<IEvent> PublishedEvents { get; } = new();
    public bool ShouldFail { get; set; }

    public ValueTask<Result> PublishAsync<TEvent>(
        TEvent @event,
        CancellationToken cancellationToken = default)
        where TEvent : class, IEvent
    {
        if (ShouldFail)
            return ValueTask.FromResult(Result.Failure("Mock failure"));

        PublishedEvents.Add(@event);
        return ValueTask.FromResult(Result.Success());
    }
}
```

## Performance Considerations

### Async Publishing Strategies

1. **Synchronous** (default): Wait for external publish to complete
2. **Fire-and-Forget**: Don't wait (use Task.Run or background channel)
3. **Batched**: Collect events and publish in batches
4. **Outbox + Background Worker**: Most reliable but adds latency

### Recommendation

Start with **synchronous** for reliability, optimize later if needed.

## Migration Path for Existing Users

### Before: Local-Only Events

```csharp
services.AddMediator(cfg =>
{
    cfg.RegisterEventHandler<OrderCreatedHandler, OrderCreatedEvent>();
});
```

**Behavior**: Event only handled locally

### After: Opt-In Hybrid Publishing

```csharp
// Step 1: Mark event as external
[ExternalEvent]
public sealed class OrderCreatedEvent : IEvent { }

// Step 2: Enable feature and register broker
services.AddMediator(cfg =>
{
    cfg.RegisterEventHandler<OrderCreatedHandler, OrderCreatedEvent>();
    cfg.EnableHybridEventPublishing(); // Enable feature
    cfg.RegisterEventBroker<RabbitMqEventBroker>(); // Register broker
});
```

**Behavior**: Event handled locally AND published to broker

### Key Points

- ✅ **Zero Breaking Changes**: Existing events without `[ExternalEvent]` work exactly as before
- ✅ **Opt-In**: Feature disabled until `EnableHybridEventPublishing()` is called
- ✅ **Gradual Migration**: Add `[ExternalEvent]` to events one at a time
- ✅ **Safe Default**: Without broker registration, uses `NullEventBroker` (no-op)

## Alternative Approaches Considered

### ❌ Approach 1: Separate IExternalEvent Handler Registration

**Rejected**: Too much ceremony, requires duplicate handler infrastructure

### ❌ Approach 2: Always Publish All Events Externally

**Rejected**: Performance overhead, no way to keep events internal

### ❌ Approach 3: Manual Broker Injection in Handlers

**Rejected**: Violates DRY, error-prone, couples handlers to infrastructure

### ✅ Approach 4: Pipeline Behavior with Metadata (Selected)

**Chosen**: Clean separation, automatic, flexible, testable

## Security Considerations

### Event Serialization

- Events published externally must be serializable
- Consider PII/sensitive data exposure
- Recommend: Use DTOs for external events, map from domain events

### Authentication

- Broker implementations should handle auth
- Don't include credentials in event payloads
- Use managed identities/service principals where possible

### Topic Naming

- Use consistent naming conventions
- Consider namespacing (e.g., `company.domain.eventname`)
- Document topic structure

## Observability

### Logging Events

- Log when event published externally
- Log failures with context
- Use structured logging

### Metrics to Track

- Events published externally (count)
- External publish failures (count)
- External publish latency (duration)
- Events by topic (distribution)

### Tracing

- Create spans for external publish operations
- Link to parent request span
- Include event metadata in tags

## Future Enhancements

### Phase 2 Features

- [ ] Event schema registry integration
- [ ] Automatic dead-letter queue handling
- [ ] Event versioning support
- [ ] CloudEvents format support
- [ ] Multi-broker support (publish to multiple brokers)
- [ ] Circuit breaker for broker failures
- [ ] Event replay capabilities

## Checklist

### Core Implementation

- [ ] `IEventBroker` interface
- [ ] `NullEventBroker` implementation
- [ ] `ExternalEventAttribute`
- [ ] `IExternalEvent` interface
- [ ] `ExternalEventPublishingOptions`
- [ ] `ExternalEventPublishingBehavior`
- [ ] Configuration API methods

### Testing

- [ ] Unit tests for behavior
- [ ] Unit tests for metadata detection
- [ ] Integration tests
- [ ] Mock broker for tests
- [ ] FailFast scenarios
- [ ] Error handling tests

### Documentation

- [ ] Feature overview
- [ ] Configuration guide
- [ ] Broker implementation guide (RabbitMQ)
- [ ] Broker implementation guide (Azure Service Bus)
- [ ] Broker implementation guide (Kafka)
- [ ] Migration guide
- [ ] Best practices
- [ ] Troubleshooting guide

### Examples

- [ ] Console app example
- [ ] RabbitMQ broker implementation
- [ ] In-memory broker (for testing)
- [ ] Order processing scenario
- [ ] README with usage

### Advanced

- [ ] OpenTelemetry integration
- [ ] Outbox integration
- [ ] Performance benchmarks
- [ ] Load testing results

## Conclusion

The **Hybrid Event Publishing** feature enables seamless integration between modular monoliths and distributed microservices by automatically routing events to both local handlers and external message brokers. The design prioritizes:

1. **Simplicity**: Single attribute to mark events as external
2. **Flexibility**: Multiple strategies (attribute, interface, configuration)
3. **Reliability**: FailFast control, error handling, outbox support
4. **Testability**: Mock broker, isolated testing
5. **Observability**: Logging, metrics, tracing built-in
6. **Non-Breaking**: Fully backward compatible, opt-in design

This feature bridges the gap between in-process event handling and distributed event-driven architectures, making it easy to evolve from monolith to microservices incrementally.

