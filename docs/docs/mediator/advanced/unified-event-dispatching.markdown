---
layout: default
title: Unified Event Dispatching
parent: Advanced
nav_order: 5
---

# Unified Event Dispatching

The unified event dispatching system consolidates local event handling, distributed event publishing, and outbox pattern integration into a cohesive architecture. This provides a single, coherent approach to event dispatching that handles both local and external consumption while maintaining reliability through integrated outbox support.

## Overview

The unified event dispatching system provides:

- **Single Responsibility**: EventDispatcher owns all event routing and dispatch logic
- **Integrated Reliability**: Outbox pattern is built into the dispatch flow
- **Flexible Routing**: Support for custom routing filters for dynamic distribution decisions
- **Multiple Distribution Modes**: LocalOnly, ExternalOnly, and Hybrid event processing
- **Configurable Strategies**: Immediate, Deferred, and Batched dispatch strategies
- **Comprehensive Observability**: Built-in OpenTelemetry tracing and metrics

## Distribution Modes

Distribution modes determine how events are processed:

### LocalOnly

Events are processed only by local handlers within the same application process. This is the default mode and is suitable for domain events that don't need to leave the application boundary.

```csharp
using UnambitiousFx.Mediator.Abstractions;
using UnambitiousFx.Mediator.Transports.Abstractions;

// Explicitly mark an event as local-only
[DistributedEvent(DistributionMode = DistributionMode.LocalOnly)]
public sealed record OrderValidated : IEvent
{
    public required Guid OrderId { get; init; }
    public required decimal TotalAmount { get; init; }
}
```

**Use Cases:**
- Domain events within a bounded context
- Internal state changes
- Triggering side effects within the same service

### ExternalOnly

Events are published only to external message transports (e.g., RabbitMQ, AWS SNS/SQS) for consumption by other services. Local handlers are not executed.

```csharp
// Mark an event for external distribution only
[DistributedEvent(DistributionMode = DistributionMode.ExternalOnly)]
public sealed record OrderShipped : IEvent
{
    public required Guid OrderId { get; init; }
    public required string TrackingNumber { get; init; }
    public required DateTimeOffset ShippedAt { get; init; }
}
```

**Use Cases:**
- Integration events for microservices communication
- Events consumed only by external systems
- Cross-bounded context notifications

### Hybrid

Events are processed by both local handlers and published to external transports. Both operations execute in parallel for optimal performance.

```csharp
// Mark an event for both local and external distribution
[DistributedEvent(DistributionMode = DistributionMode.Hybrid)]
public sealed record OrderCreated : IEvent
{
    public required Guid OrderId { get; init; }
    public required Guid CustomerId { get; init; }
    public required decimal TotalAmount { get; init; }
}
```

**Use Cases:**
- Events that trigger both local and external workflows
- Domain events that also serve as integration events
- Events requiring immediate local processing and eventual external delivery

## Dispatch Strategies

Dispatch strategies control when and how events are processed after being stored in the outbox.

### Immediate

Events are processed synchronously immediately after being stored in the outbox. This provides the lowest latency but may impact request processing time.

```csharp
services.AddMediator(cfg =>
{
    cfg.ConfigureEventDispatcher(options =>
    {
        options.DispatchStrategy = DispatchStrategy.Immediate;
    });
});
```

**Characteristics:**
- Lowest latency
- Synchronous processing
- Failures are immediately visible
- May increase request processing time

**Best For:**
- Real-time event processing requirements
- Events with immediate side effects
- Development and testing environments

### Deferred

Events are stored in the outbox and returned immediately. Background processing handles event dispatch asynchronously.

```csharp
services.AddMediator(cfg =>
{
    cfg.ConfigureEventDispatcher(options =>
    {
        options.DispatchStrategy = DispatchStrategy.Deferred;
    });
});
```

**Characteristics:**
- Fast request completion
- Asynchronous processing
- Requires background worker
- Better throughput for high-volume scenarios

**Best For:**
- High-throughput applications
- Non-critical event processing
- Production environments with background workers

### Batched

Events are accumulated and processed in configurable batches, optimizing for throughput over latency.

```csharp
services.AddMediator(cfg =>
{
    cfg.ConfigureEventDispatcher(options =>
    {
        options.DispatchStrategy = DispatchStrategy.Batched;
        options.BatchSize = 100;
        options.BatchFlushInterval = TimeSpan.FromSeconds(5);
    });
});
```

**Characteristics:**
- Highest throughput
- Configurable batch size and flush interval
- Increased latency (up to flush interval)
- Efficient for bulk operations

**Best For:**
- High-volume event processing
- Analytics and reporting events
- Scenarios where latency is acceptable

## Custom Event Routing Filters

Routing filters provide dynamic, runtime-based routing decisions for events. They are evaluated before message traits and allow you to implement complex routing logic based on context, tenant, feature flags, or any other runtime condition.

### Creating a Routing Filter

Implement the `IEventRoutingFilter` interface:

```csharp
using UnambitiousFx.Mediator.Abstractions;
using UnambitiousFx.Mediator.Transports.Abstractions;

public class TenantBasedRoutingFilter : IEventRoutingFilter
{
    private readonly IContextAccessor _contextAccessor;
    
    public TenantBasedRoutingFilter(IContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }
    
    public int Order => 100;

    public DistributionMode? GetDistributionMode<TEvent>(TEvent @event) 
        where TEvent : class, IEvent
    {
        var context = _contextAccessor.Context;
        var tenantId = context?.GetMetadata<string>("TenantId");
        
        // Route events for premium tenants to both local and external
        if (tenantId == "premium-tenant")
            return DistributionMode.Hybrid;
        
        // Route events for standard tenants locally only
        if (tenantId == "standard-tenant")
            return DistributionMode.LocalOnly;
        
        // Defer to next filter or default configuration
        return null;
    }
}
```

### Registering Routing Filters

Register filters in dependency injection. Multiple filters can be registered and are executed in order based on their `Order` property:

```csharp
services.AddMediator(cfg =>
{
    // Register routing filters
    cfg.Services.AddScoped<IEventRoutingFilter, TenantBasedRoutingFilter>();
    cfg.Services.AddScoped<IEventRoutingFilter, FeatureFlagRoutingFilter>();
});
```

### Filter Evaluation Order

1. Routing filters are evaluated in ascending order by their `Order` property
2. The first filter that returns a non-null `DistributionMode` determines the routing
3. If no filter returns a mode, the system falls back to message traits
4. If no message traits are configured, the default distribution mode is used

### Example: Feature Flag Based Routing

```csharp
public class FeatureFlagRoutingFilter : IEventRoutingFilter
{
    private readonly IFeatureFlagService _featureFlagService;
    
    public FeatureFlagRoutingFilter(IFeatureFlagService featureFlagService)
    {
        _featureFlagService = featureFlagService;
    }
    
    public int Order => 50; // Executes before TenantBasedRoutingFilter

    public DistributionMode? GetDistributionMode<TEvent>(TEvent @event) 
        where TEvent : class, IEvent
    {
        // Check if distributed events feature is enabled
        if (!_featureFlagService.IsEnabled("distributed-events"))
            return DistributionMode.LocalOnly;
        
        // Defer to next filter
        return null;
    }
}
```

## Configuration

### Basic Configuration

Configure the event dispatcher with default settings:

```csharp
using Microsoft.Extensions.DependencyInjection;
using UnambitiousFx.Mediator;
using UnambitiousFx.Mediator.Transports.Abstractions;

var services = new ServiceCollection();

services.AddMediator(cfg =>
{
    cfg.ConfigureEventDispatcher(options =>
    {
        // Set default distribution mode
        options.DefaultDistributionMode = DistributionMode.LocalOnly;
        
        // Set dispatch strategy
        options.DispatchStrategy = DispatchStrategy.Immediate;
    });
});
```

### Outbox Configuration

Configure outbox behavior including retry policies:

```csharp
services.AddMediator(cfg =>
{
    cfg.ConfigureOutbox(options =>
    {
        // Maximum retry attempts before dead-lettering
        options.MaxRetryAttempts = 5;
        
        // Initial retry delay
        options.InitialRetryDelay = TimeSpan.FromSeconds(5);
        
        // Exponential backoff factor
        options.BackoffFactor = 2.0;
        
        // Batch size for processing pending events
        options.BatchSize = 100;
    });
});
```

### Advanced Configuration

Combine all configuration options:

```csharp
services.AddMediator(cfg =>
{
    // Event dispatcher configuration
    cfg.ConfigureEventDispatcher(options =>
    {
        options.DefaultDistributionMode = DistributionMode.LocalOnly;
        options.DispatchStrategy = DispatchStrategy.Deferred;
        options.BatchSize = 100;
        options.BatchFlushInterval = TimeSpan.FromSeconds(5);
    });
    
    // Outbox configuration
    cfg.ConfigureOutbox(options =>
    {
        options.MaxRetryAttempts = 5;
        options.InitialRetryDelay = TimeSpan.FromSeconds(5);
        options.BackoffFactor = 2.0;
        options.BatchSize = 100;
    });
    
    // Register routing filters
    cfg.Services.AddScoped<IEventRoutingFilter, TenantBasedRoutingFilter>();
    cfg.Services.AddScoped<IEventRoutingFilter, FeatureFlagRoutingFilter>();
});
```

## Publishing Events

### Using IPublisher

The `IPublisher` interface provides a simple API for publishing events:

```csharp
public class OrderService
{
    private readonly IPublisher _publisher;
    
    public OrderService(IPublisher publisher)
    {
        _publisher = publisher;
    }
    
    public async Task CreateOrderAsync(CreateOrderRequest request)
    {
        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerId = request.CustomerId,
            TotalAmount = request.TotalAmount
        };
        
        // Save order to database
        await SaveOrderAsync(order);
        
        // Publish event - routing is determined automatically
        await _publisher.PublishAsync(new OrderCreated
        {
            OrderId = order.Id,
            CustomerId = order.CustomerId,
            TotalAmount = order.TotalAmount
        });
    }
}
```

### Using IContext

When working within request handlers, use `IContext` to publish events:

```csharp
[RequestHandler<CreateOrderCommand, Guid>]
public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly IOrderRepository _repository;
    private readonly IContext _context;
    
    public CreateOrderCommandHandler(IOrderRepository repository, IContext context)
    {
        _repository = repository;
        _context = context;
    }
    
    public async ValueTask<Result<Guid>> HandleAsync(
        CreateOrderCommand command,
        CancellationToken cancellationToken)
    {
        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerId = command.CustomerId,
            TotalAmount = command.TotalAmount
        };
        
        await _repository.SaveAsync(order, cancellationToken);
        
        // Publish event through context
        await _context.PublishEventAsync(new OrderCreated
        {
            OrderId = order.Id,
            CustomerId = order.CustomerId,
            TotalAmount = order.TotalAmount
        }, cancellationToken);
        
        return Result.Success(order.Id);
    }
}
```

### Publish Modes (Backward Compatibility)

The legacy `PublishMode` API is still supported for backward compatibility:

```csharp
// Publish immediately
await _publisher.PublishAsync(@event, PublishMode.Now, cancellationToken);

// Store in outbox for deferred processing
await _publisher.PublishAsync(@event, PublishMode.Outbox, cancellationToken);

// Process pending outbox events
await _publisher.CommitAsync(cancellationToken);
```

## Error Handling and Reliability

### Automatic Retry

Failed events are automatically retried with exponential backoff:

```
Attempt 1: Immediate
Attempt 2: After 5 seconds
Attempt 3: After 10 seconds (5 * 2^1)
Attempt 4: After 20 seconds (5 * 2^2)
Attempt 5: After 40 seconds (5 * 2^3)
```

### Dead Letter Queue

After exceeding maximum retry attempts, events are moved to dead letter status for manual intervention:

```csharp
// Retrieve dead-lettered events for inspection
var deadLetterEvents = await outboxStorage.GetDeadLetterEventsAsync(cancellationToken);

foreach (var @event in deadLetterEvents)
{
    // Inspect and decide whether to reprocess
    Console.WriteLine($"Dead letter event: {@event.GetType().Name}");
}
```

### Fail-Fast Mode

For critical events that should fail immediately rather than retry:

```csharp
[DistributedEvent(
    DistributionMode = DistributionMode.Hybrid,
    FailFast = true)]
public sealed record CriticalOrderEvent : IEvent
{
    public required Guid OrderId { get; init; }
}
```

When `FailFast` is enabled:
- Exceptions are propagated immediately
- No retry attempts are made
- The calling code must handle failures

## Observability

### OpenTelemetry Tracing

The event dispatcher automatically creates OpenTelemetry activities for all event operations:

```csharp
// Activities are created with the following tags:
// - messaging.distribution_mode: LocalOnly, ExternalOnly, or Hybrid
// - messaging.event_type: The event type name
// - messaging.from_outbox: Whether the event is being replayed from outbox
// - messaging.handler_count: Number of local handlers
// - messaging.transport_name: External transport name (if applicable)
```

### Structured Logging

Enable structured logging to track event dispatch operations:

```csharp
services.AddLogging(builder =>
{
    builder.AddConsole();
    builder.SetMinimumLevel(LogLevel.Information);
});
```

Log events include:
- Event dispatch started
- Distribution mode determined
- Routing filter evaluations
- Handler execution results
- Transport dispatch results
- Retry attempts and backoff calculations
- Dead letter transitions

### Metrics

The event dispatcher exposes metrics for monitoring:

- **Event Dispatch Count**: Total events dispatched by distribution mode
- **Event Dispatch Duration**: P50, P95, P99 latencies
- **Event Failure Rate**: Percentage of failed dispatches
- **Outbox Queue Depth**: Number of pending events in outbox
- **Outbox Processing Throughput**: Events processed per second
- **Dead Letter Count**: Number of dead-lettered events

## Best Practices

### 1. Choose the Right Distribution Mode

- Use **LocalOnly** for domain events within a bounded context
- Use **ExternalOnly** for integration events consumed by other services
- Use **Hybrid** when events need both local and external processing

### 2. Configure Appropriate Dispatch Strategy

- Use **Immediate** for real-time requirements and development
- Use **Deferred** for production environments with background workers
- Use **Batched** for high-volume, latency-tolerant scenarios

### 3. Implement Idempotent Event Handlers

Since the outbox pattern provides at-least-once delivery, handlers should be idempotent:

```csharp
public class OrderCreatedHandler : IEventHandler<OrderCreated>
{
    private readonly IEmailService _emailService;
    private readonly INotificationRepository _repository;
    
    public async ValueTask<Result> HandleAsync(
        OrderCreated @event,
        CancellationToken cancellationToken)
    {
        // Check if notification already sent (idempotency)
        var exists = await _repository.ExistsAsync(@event.OrderId, cancellationToken);
        if (exists)
            return Result.Success();
        
        // Send notification
        await _emailService.SendOrderConfirmationAsync(@event.OrderId, cancellationToken);
        
        // Record that notification was sent
        await _repository.RecordAsync(@event.OrderId, cancellationToken);
        
        return Result.Success();
    }
}
```

### 4. Use Routing Filters for Cross-Cutting Concerns

Implement routing filters for concerns that affect multiple event types:

- Tenant-based routing
- Feature flag-based routing
- Environment-based routing (dev vs. prod)
- Load shedding and circuit breaking

### 5. Monitor Outbox Health

Regularly monitor outbox metrics:

- Queue depth should remain low
- Dead letter count should be investigated
- Retry rates indicate transport health

### 6. Handle Partial Failures in Hybrid Mode

In Hybrid mode, local and external dispatch can fail independently:

```csharp
public class OrderCreatedHandler : IEventHandler<OrderCreated>
{
    public async ValueTask<Result> HandleAsync(
        OrderCreated @event,
        CancellationToken cancellationToken)
    {
        try
        {
            // Local processing
            await ProcessLocallyAsync(@event, cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            // Log but don't fail - external dispatch may still succeed
            _logger.LogError(ex, "Local processing failed for order {OrderId}", @event.OrderId);
            return Result.Failure(ex.Message);
        }
    }
}
```

## Migration from DistributedEventDispatchBehavior

If you're migrating from the legacy `DistributedEventDispatchBehavior`, see the [Migration Guide](../MIGRATION-GUIDE-v2.html) for detailed instructions.

Key changes:
- Remove `DistributedEventDispatchBehavior` registrations
- Configure distribution modes using `[DistributedEvent]` attribute or routing filters
- Update event handler implementations if needed
- Test thoroughly, especially for Hybrid mode events

## See Also

- [Publish an Event](../../basics/publish-event.html) - Basic event publishing
- [Event Pipeline Behavior](../../behaviors/event-pipeline-behavior.html) - Custom event behaviors
- [Event Orchestrator](./event-orchestrator.html) - Handler execution strategies
- [Migration Guide](../MIGRATION-GUIDE-v2.html) - Migrating to unified event dispatching
