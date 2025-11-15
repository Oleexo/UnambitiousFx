# Migration Guide: Unified Event Dispatching (v2.0)

This guide helps you migrate from the previous event dispatching architecture to the new unified event dispatching system introduced in v2.0.

## Table of Contents

1. [Overview](#overview)
2. [Breaking Changes](#breaking-changes)
3. [Migration Steps](#migration-steps)
4. [Code Examples](#code-examples)
5. [FAQ](#faq)

## Overview

Version 2.0 introduces a unified event dispatching architecture that consolidates local event handling, distributed event publishing, and outbox pattern integration into a cohesive system. The key changes are:

- **Removed**: `DistributedEventDispatchBehavior` class
- **Enhanced**: `EventDispatcher` now handles all distribution mode routing
- **Added**: `IEventRoutingFilter` for custom routing logic
- **Added**: `OutboxManager` for unified outbox and dispatch strategy management
- **Added**: NativeAOT-friendly event dispatching from outbox

### Why This Change?

The previous architecture split event dispatching concerns across multiple components (`EventDispatcher`, `Publisher`, `DistributedEventDispatchBehavior`), creating complexity and unclear boundaries. The unified design provides:

- **Single Responsibility**: EventDispatcher owns all event routing and dispatch logic
- **Integrated Reliability**: Outbox pattern is built into the dispatch flow
- **Flexible Routing**: Support for custom routing filters
- **Better Performance**: Optimized for common cases (local-only events)
- **NativeAOT Support**: No reflection for event dispatch

## Breaking Changes

### 1. Removed `DistributedEventDispatchBehavior`

**What Changed**: The `DistributedEventDispatchBehavior` class has been removed from the `Mediator.Transports.Pipeline` namespace.

**Why**: Its functionality has been integrated directly into `EventDispatcher` and `OutboxManager`, eliminating the need for a separate pipeline behavior.

**Impact**: If you manually registered this behavior in your DI configuration, you'll need to remove that registration.

### 2. Automatic Distribution Mode Routing

**What Changed**: Distribution mode routing is now handled automatically by `EventDispatcher` without requiring a pipeline behavior.

**Why**: This simplifies the architecture and makes event routing more transparent and configurable.

**Impact**: You no longer need to configure or think about the distributed event dispatch behavior—it just works.

### 3. New Routing Priority

**What Changed**: Distribution mode is now determined in this order:
1. `IEventRoutingFilter` implementations (evaluated first)
2. `MessageTraits` configuration (fallback)
3. `EventDispatcherOptions.DefaultDistributionMode` (default)

**Why**: This provides more flexibility and control over event routing decisions.

**Impact**: If you were relying on specific routing behavior, verify it still works as expected.

## Migration Steps

### Step 1: Remove Manual Behavior Registration

If you manually registered `DistributedEventDispatchBehavior` in your DI configuration, remove it:

**Before (v1.x)**:
```csharp
services.AddMediator(cfg =>
{
    // Remove this line if present
    cfg.AddEventPipelineBehavior<DistributedEventDispatchBehavior>();
});
```

**After (v2.0)**:
```csharp
services.AddMediator(cfg =>
{
    // No need to register DistributedEventDispatchBehavior
    // Distribution mode routing is automatic
});
```

### Step 2: Configure Default Distribution Mode (Optional)

If you want to change the default distribution mode for all events:

```csharp
services.AddMediator(cfg =>
{
    cfg.ConfigureEventDispatcher(options =>
    {
        // Default is LocalOnly
        options.DefaultDistributionMode = DistributionMode.LocalOnly;
        
        // Or use Hybrid for both local and external
        // options.DefaultDistributionMode = DistributionMode.Hybrid;
    });
});
```

### Step 3: Configure Event-Specific Distribution Modes

Continue using `MessageTraits` to configure distribution modes for specific events:

```csharp
services.AddMediator(cfg =>
{
    cfg.EnableDistributedMessaging(messaging =>
    {
        // External-only events
        messaging.ConfigureEvent<OrderShipped>(opts => opts
            .AsExternal()
            .WithTopic("orders.shipped"));

        // Local-only events
        messaging.ConfigureEvent<OrderCreated>(opts => opts
            .AsLocal());

        // Hybrid events (both local and external)
        messaging.ConfigureEvent<PaymentProcessed>(opts => opts
            .AsHybrid()
            .WithTopic("payments.processed"));
    });
});
```

### Step 4: Implement Custom Routing Filters (Optional)

If you need dynamic routing logic based on runtime conditions, implement `IEventRoutingFilter`:

```csharp
public class TenantBasedRoutingFilter : IEventRoutingFilter
{
    private readonly IContextAccessor _contextAccessor;
    
    public int Order => 100; // Lower values execute first
    
    public TenantBasedRoutingFilter(IContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public DistributionMode? GetDistributionMode<TEvent>(TEvent @event) 
        where TEvent : class, IEvent
    {
        var context = _contextAccessor.Context;
        var tenantId = context?.GetMetadata<string>("TenantId");
        
        // Route events for premium tenants to both local and external
        if (tenantId == "premium-tenant")
            return DistributionMode.Hybrid;
        
        // Defer to next filter or default configuration
        return null;
    }
}
```

Register your routing filter:

```csharp
services.AddMediator(cfg =>
{
    cfg.RegisterEventRoutingFilter<TenantBasedRoutingFilter>();
});
```

### Step 5: Configure Dispatch Strategy (Optional)

Control when and how events are dispatched:

```csharp
services.AddMediator(cfg =>
{
    cfg.ConfigureEventDispatcher(options =>
    {
        // Immediate: Dispatch right after storing in outbox (default)
        options.DispatchStrategy = DispatchStrategy.Immediate;
        
        // Deferred: Store in outbox and process in background
        // options.DispatchStrategy = DispatchStrategy.Deferred;
        
        // Batched: Accumulate and dispatch in batches
        // options.DispatchStrategy = DispatchStrategy.Batched;
        // options.BatchSize = 100;
        // options.BatchFlushInterval = TimeSpan.FromSeconds(5);
    });
});
```

### Step 6: Verify Your Application

1. **Build your application** to ensure there are no compilation errors
2. **Run your tests** to verify event handling still works correctly
3. **Check logs** to confirm events are being routed as expected
4. **Monitor metrics** to ensure performance is acceptable

## Code Examples

### Example 1: Simple Local Event

**Before and After** (No changes required):

```csharp
// Event definition
public sealed record OrderCreated : IEvent
{
    public required Guid OrderId { get; init; }
    public required string CustomerName { get; init; }
}

// Event handler
public sealed class OrderCreatedHandler : IEventHandler<OrderCreated>
{
    private readonly ILogger<OrderCreatedHandler> _logger;
    
    public OrderCreatedHandler(ILogger<OrderCreatedHandler> logger)
    {
        _logger = logger;
    }

    public ValueTask<Result> HandleAsync(
        OrderCreated @event,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Order {OrderId} created for {CustomerName}",
            @event.OrderId,
            @event.CustomerName);
        
        return ValueTask.FromResult(Result.Success());
    }
}

// Publishing (no changes)
await _publisher.PublishAsync(new OrderCreated
{
    OrderId = orderId,
    CustomerName = customerName
}, cancellationToken);
```

### Example 2: External Event with Transport

**Before (v1.x)**:
```csharp
services.AddMediator(cfg =>
{
    cfg.EnableDistributedMessaging(messaging =>
    {
        messaging.ConfigureEvent<OrderShipped>(opts => opts
            .AsExternal()
            .WithTopic("orders.shipped"));
        
        messaging.UseTransport<RabbitMqTransport>();
        messaging.Subscribe<OrderShipped>();
    });
    
    // Had to manually register the behavior
    cfg.AddEventPipelineBehavior<DistributedEventDispatchBehavior>();
});
```

**After (v2.0)**:
```csharp
services.AddMediator(cfg =>
{
    cfg.EnableDistributedMessaging(messaging =>
    {
        messaging.ConfigureEvent<OrderShipped>(opts => opts
            .AsExternal()
            .WithTopic("orders.shipped"));
        
        messaging.UseTransport<RabbitMqTransport>();
        messaging.Subscribe<OrderShipped>();
    });
    
    // No need to register DistributedEventDispatchBehavior
    // Distribution mode routing is automatic!
});
```

### Example 3: Hybrid Event (Local + External)

**Before (v1.x)**:
```csharp
services.AddMediator(cfg =>
{
    cfg.EnableDistributedMessaging(messaging =>
    {
        // Had to configure as external and handle local separately
        messaging.ConfigureEvent<PaymentProcessed>(opts => opts
            .AsExternal()
            .WithTopic("payments.processed"));
    });
});
```

**After (v2.0)**:
```csharp
services.AddMediator(cfg =>
{
    cfg.EnableDistributedMessaging(messaging =>
    {
        // Now explicitly support hybrid mode
        messaging.ConfigureEvent<PaymentProcessed>(opts => opts
            .AsHybrid() // Both local handlers AND external transport
            .WithTopic("payments.processed"));
    });
});
```

### Example 4: Dynamic Routing with Filters

**New in v2.0**:

```csharp
// Define a routing filter
public class EnvironmentBasedRoutingFilter : IEventRoutingFilter
{
    private readonly IConfiguration _configuration;
    
    public int Order => 50;
    
    public EnvironmentBasedRoutingFilter(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public DistributionMode? GetDistributionMode<TEvent>(TEvent @event) 
        where TEvent : class, IEvent
    {
        var environment = _configuration["Environment"];
        
        // In development, keep all events local
        if (environment == "Development")
            return DistributionMode.LocalOnly;
        
        // In production, use configured distribution mode
        return null; // Defer to MessageTraits or default
    }
}

// Register the filter
services.AddMediator(cfg =>
{
    cfg.RegisterEventRoutingFilter<EnvironmentBasedRoutingFilter>();
});
```

### Example 5: Outbox with Deferred Dispatch

**New in v2.0**:

```csharp
services.AddMediator(cfg =>
{
    cfg.ConfigureEventDispatcher(options =>
    {
        // Store events in outbox but don't dispatch immediately
        options.DispatchStrategy = DispatchStrategy.Deferred;
    });
    
    cfg.ConfigureOutbox(options =>
    {
        options.MaxRetryAttempts = 5;
        options.InitialRetryDelay = TimeSpan.FromSeconds(2);
        options.BackoffFactor = 2.0;
    });
});

// Events are stored in outbox immediately
await _publisher.PublishAsync(new OrderShipped
{
    OrderId = orderId,
    TrackingNumber = trackingNumber
}, cancellationToken);

// Process pending events in background or on-demand
await _publisher.CommitAsync(cancellationToken);
```

### Example 6: Batched Dispatch for High Throughput

**New in v2.0**:

```csharp
services.AddMediator(cfg =>
{
    cfg.ConfigureEventDispatcher(options =>
    {
        // Accumulate events and dispatch in batches
        options.DispatchStrategy = DispatchStrategy.Batched;
        options.BatchSize = 100;
        options.BatchFlushInterval = TimeSpan.FromSeconds(5);
    });
});

// Events are accumulated in memory
for (int i = 0; i < 1000; i++)
{
    await _publisher.PublishAsync(new InventoryUpdated
    {
        ProductId = productId,
        QuantityChange = -1
    }, cancellationToken);
}

// Batches are automatically flushed based on size or interval
```

## FAQ

### Q: Do I need to change my event definitions?

**A**: No, event definitions remain unchanged. Continue implementing `IEvent` as before.

### Q: Do I need to change my event handlers?

**A**: No, event handlers remain unchanged. Continue implementing `IEventHandler<TEvent>` as before.

### Q: Will my existing events still work?

**A**: Yes, the Publisher API is backward compatible. Existing code will continue to work without changes.

### Q: What happens to events already in the outbox?

**A**: Events in the outbox will be processed normally. The new system is compatible with the existing outbox storage format.

### Q: How do I know which distribution mode an event is using?

**A**: Check the logs or use OpenTelemetry tracing. The distribution mode is tagged on all dispatch activities.

### Q: Can I still use PublishMode.Now and PublishMode.Outbox?

**A**: Yes, these modes are still supported and work as before:
- `PublishMode.Now`: Immediate dispatch (uses `DispatchStrategy.Immediate`)
- `PublishMode.Outbox`: Deferred dispatch (uses `DispatchStrategy.Deferred`)

### Q: What if I don't want to use the outbox for certain events?

**A**: Configure the event as `LocalOnly` and use `DispatchStrategy.Immediate`. Local-only events skip outbox storage for performance.

### Q: How do routing filters interact with MessageTraits?

**A**: Routing filters are evaluated first. If a filter returns a distribution mode, that mode is used. Otherwise, the system falls back to MessageTraits configuration, then the default distribution mode.

### Q: Can I have multiple routing filters?

**A**: Yes, register multiple filters and control their execution order using the `Order` property. Lower values execute first.

### Q: What about NativeAOT compatibility?

**A**: The new system is fully NativeAOT compatible. Event dispatchers are registered at compile-time via source generation, avoiding reflection.

### Q: How do I troubleshoot routing issues?

**A**: Enable debug logging for the `UnambitiousFx.Mediator` namespace:

```json
{
  "Logging": {
    "LogLevel": {
      "UnambitiousFx.Mediator": "Debug"
    }
  }
}
```

You'll see logs showing:
- Distribution mode determination
- Routing filter evaluations
- Dispatch strategy application
- Outbox operations

### Q: What's the performance impact?

**A**: For local-only events, performance is similar or better than v1.x. For external events, the integrated outbox provides better reliability with minimal overhead.

### Q: Can I opt out of the new system?

**A**: No, the new unified architecture is the only supported approach in v2.0. However, the migration is straightforward and the API is backward compatible.

## Need Help?

If you encounter issues during migration:

1. **Check the logs**: Enable debug logging to see what's happening
2. **Review the design document**: See `.kiro/specs/unified-event-dispatching/design.md`
3. **Check the examples**: See `examples/mediator/` for working code
4. **Open an issue**: Report bugs or ask questions on GitHub

## Summary

The unified event dispatching system simplifies the architecture while providing more flexibility and better performance. Most applications can migrate by simply removing the manual `DistributedEventDispatchBehavior` registration—everything else continues to work as before.

Key takeaways:
- ✅ Remove manual `DistributedEventDispatchBehavior` registration
- ✅ Event definitions and handlers remain unchanged
- ✅ Publisher API is backward compatible
- ✅ Use routing filters for dynamic routing logic
- ✅ Configure dispatch strategies for different reliability/performance trade-offs
- ✅ Enjoy better observability with integrated tracing and metrics
