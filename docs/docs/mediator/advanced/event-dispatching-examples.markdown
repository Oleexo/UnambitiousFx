---
layout: default
title: Event Dispatching Examples
parent: Advanced
nav_order: 6
---

# Event Dispatching Examples

This page provides practical examples and recipes for common event dispatching scenarios using the unified event dispatching system.

## Table of Contents
{: .no_toc .text-delta }

1. TOC
{:toc}

---

## Basic Scenarios

### Local-Only Domain Events

For events that should only be processed within your application:

```csharp
using UnambitiousFx.Mediator.Abstractions;
using UnambitiousFx.Mediator.Transports.Abstractions;

// Define the event
[DistributedEvent(DistributionMode = DistributionMode.LocalOnly)]
public sealed record UserProfileUpdated : IEvent
{
    public required Guid UserId { get; init; }
    public required string Email { get; init; }
    public required string DisplayName { get; init; }
}

// Create handlers
public class UpdateSearchIndexHandler : IEventHandler<UserProfileUpdated>
{
    private readonly ISearchIndexService _searchIndex;
    
    public async ValueTask<Result> HandleAsync(
        UserProfileUpdated @event,
        CancellationToken cancellationToken)
    {
        await _searchIndex.UpdateUserAsync(@event.UserId, @event.DisplayName, cancellationToken);
        return Result.Success();
    }
}

public class InvalidateCacheHandler : IEventHandler<UserProfileUpdated>
{
    private readonly ICacheService _cache;
    
    public async ValueTask<Result> HandleAsync(
        UserProfileUpdated @event,
        CancellationToken cancellationToken)
    {
        await _cache.RemoveAsync($"user:{@event.UserId}", cancellationToken);
        return Result.Success();
    }
}

// Publish the event
await _publisher.PublishAsync(new UserProfileUpdated
{
    UserId = userId,
    Email = email,
    DisplayName = displayName
});
```

### External-Only Integration Events

For events consumed only by other services:

```csharp
// Define the event with schema versioning
[DistributedEvent(DistributionMode = DistributionMode.ExternalOnly)]
[MessageSchema(Name = "order.shipped", Version = "1.0")]
public sealed record OrderShippedEvent : IEvent
{
    public required Guid OrderId { get; init; }
    public required Guid CustomerId { get; init; }
    public required string TrackingNumber { get; init; }
    public required string Carrier { get; init; }
    public required DateTimeOffset ShippedAt { get; init; }
}

// Publish the event - no local handlers will execute
await _publisher.PublishAsync(new OrderShippedEvent
{
    OrderId = order.Id,
    CustomerId = order.CustomerId,
    TrackingNumber = trackingNumber,
    Carrier = "FedEx",
    ShippedAt = DateTimeOffset.UtcNow
});
```

### Hybrid Events (Local + External)

For events that need both local processing and external distribution:

```csharp
// Define the event
[DistributedEvent(DistributionMode = DistributionMode.Hybrid)]
[MessageSchema(Name = "order.created", Version = "1.0")]
public sealed record OrderCreatedEvent : IEvent
{
    public required Guid OrderId { get; init; }
    public required Guid CustomerId { get; init; }
    public required decimal TotalAmount { get; init; }
    public required List<OrderItem> Items { get; init; }
}

// Local handler - updates internal systems
public class UpdateInventoryHandler : IEventHandler<OrderCreatedEvent>
{
    private readonly IInventoryService _inventory;
    
    public async ValueTask<Result> HandleAsync(
        OrderCreatedEvent @event,
        CancellationToken cancellationToken)
    {
        foreach (var item in @event.Items)
        {
            await _inventory.ReserveStockAsync(item.ProductId, item.Quantity, cancellationToken);
        }
        return Result.Success();
    }
}

// External consumers (other services) will receive the event via transport
// Example: Shipping service, Analytics service, Notification service
```

## Advanced Routing

### Tenant-Based Routing

Route events differently based on tenant context:

```csharp
public class TenantBasedRoutingFilter : IEventRoutingFilter
{
    private readonly IContextAccessor _contextAccessor;
    private readonly ITenantConfigurationService _tenantConfig;
    
    public TenantBasedRoutingFilter(
        IContextAccessor contextAccessor,
        ITenantConfigurationService tenantConfig)
    {
        _contextAccessor = contextAccessor;
        _tenantConfig = tenantConfig;
    }
    
    public int Order => 100;

    public DistributionMode? GetDistributionMode<TEvent>(TEvent @event) 
        where TEvent : class, IEvent
    {
        var context = _contextAccessor.Context;
        var tenantId = context?.GetMetadata<string>("TenantId");
        
        if (string.IsNullOrEmpty(tenantId))
            return null;
        
        var config = _tenantConfig.GetConfiguration(tenantId);
        
        // Premium tenants get hybrid distribution
        if (config.Tier == TenantTier.Premium)
            return DistributionMode.Hybrid;
        
        // Standard tenants get local-only
        if (config.Tier == TenantTier.Standard)
            return DistributionMode.LocalOnly;
        
        // Trial tenants - no external events
        if (config.Tier == TenantTier.Trial)
            return DistributionMode.LocalOnly;
        
        return null;
    }
}

// Register the filter
services.AddMediator(cfg =>
{
    cfg.Services.AddScoped<IEventRoutingFilter, TenantBasedRoutingFilter>();
});
```

### Feature Flag-Based Routing

Gradually roll out distributed events using feature flags:

```csharp
public class FeatureFlagRoutingFilter : IEventRoutingFilter
{
    private readonly IFeatureManager _featureManager;
    
    public FeatureFlagRoutingFilter(IFeatureManager featureManager)
    {
        _featureManager = featureManager;
    }
    
    public int Order => 50; // Execute before tenant filter

    public async DistributionMode? GetDistributionMode<TEvent>(TEvent @event) 
        where TEvent : class, IEvent
    {
        // Check if distributed events are enabled globally
        var isEnabled = await _featureManager.IsEnabledAsync("distributed-events");
        if (!isEnabled)
            return DistributionMode.LocalOnly;
        
        // Check event-specific feature flags
        var eventType = typeof(TEvent).Name;
        var eventFlagEnabled = await _featureManager.IsEnabledAsync($"distributed-events:{eventType}");
        
        if (!eventFlagEnabled)
            return DistributionMode.LocalOnly;
        
        // Defer to next filter or default
        return null;
    }
}
```

### Environment-Based Routing

Route events differently in development vs. production:

```csharp
public class EnvironmentRoutingFilter : IEventRoutingFilter
{
    private readonly IHostEnvironment _environment;
    
    public EnvironmentRoutingFilter(IHostEnvironment environment)
    {
        _environment = environment;
    }
    
    public int Order => 10; // Execute first

    public DistributionMode? GetDistributionMode<TEvent>(TEvent @event) 
        where TEvent : class, IEvent
    {
        // In development, keep all events local unless explicitly marked
        if (_environment.IsDevelopment())
        {
            // Check if event has explicit external distribution
            var attr = typeof(TEvent).GetCustomAttribute<DistributedEventAttribute>();
            if (attr?.DistributionMode == DistributionMode.ExternalOnly)
                return DistributionMode.ExternalOnly;
            
            // Default to local-only in development
            return DistributionMode.LocalOnly;
        }
        
        // In production, defer to other filters and defaults
        return null;
    }
}
```

### Event Type-Based Routing

Route specific event types differently:

```csharp
public class EventTypeRoutingFilter : IEventRoutingFilter
{
    public int Order => 200;

    public DistributionMode? GetDistributionMode<TEvent>(TEvent @event) 
        where TEvent : class, IEvent
    {
        // Route all audit events externally
        if (@event is IAuditEvent)
            return DistributionMode.ExternalOnly;
        
        // Route all notification events as hybrid
        if (@event is INotificationEvent)
            return DistributionMode.Hybrid;
        
        // Route all analytics events externally
        if (@event is IAnalyticsEvent)
            return DistributionMode.ExternalOnly;
        
        return null;
    }
}

// Define marker interfaces
public interface IAuditEvent : IEvent { }
public interface INotificationEvent : IEvent { }
public interface IAnalyticsEvent : IEvent { }

// Use marker interfaces on events
public sealed record UserLoginEvent : IAuditEvent
{
    public required Guid UserId { get; init; }
    public required string IpAddress { get; init; }
    public required DateTimeOffset LoginAt { get; init; }
}
```

## Configuration Patterns

### High-Throughput Configuration

Optimize for maximum throughput with deferred processing:

```csharp
services.AddMediator(cfg =>
{
    cfg.ConfigureEventDispatcher(options =>
    {
        options.DefaultDistributionMode = DistributionMode.Hybrid;
        options.DispatchStrategy = DispatchStrategy.Deferred;
    });
    
    cfg.ConfigureOutbox(options =>
    {
        options.MaxRetryAttempts = 3;
        options.InitialRetryDelay = TimeSpan.FromSeconds(2);
        options.BackoffFactor = 2.0;
        options.BatchSize = 500; // Process in large batches
    });
});

// Add background service to process outbox
services.AddHostedService<OutboxProcessorBackgroundService>();
```

### Low-Latency Configuration

Optimize for minimum latency with immediate processing:

```csharp
services.AddMediator(cfg =>
{
    cfg.ConfigureEventDispatcher(options =>
    {
        options.DefaultDistributionMode = DistributionMode.LocalOnly;
        options.DispatchStrategy = DispatchStrategy.Immediate;
    });
    
    cfg.ConfigureOutbox(options =>
    {
        options.MaxRetryAttempts = 5;
        options.InitialRetryDelay = TimeSpan.FromMilliseconds(100);
        options.BackoffFactor = 1.5;
    });
});
```

### Batched Processing Configuration

Optimize for efficiency with batched processing:

```csharp
services.AddMediator(cfg =>
{
    cfg.ConfigureEventDispatcher(options =>
    {
        options.DefaultDistributionMode = DistributionMode.ExternalOnly;
        options.DispatchStrategy = DispatchStrategy.Batched;
        options.BatchSize = 100;
        options.BatchFlushInterval = TimeSpan.FromSeconds(5);
    });
    
    cfg.ConfigureOutbox(options =>
    {
        options.MaxRetryAttempts = 5;
        options.InitialRetryDelay = TimeSpan.FromSeconds(5);
        options.BackoffFactor = 2.0;
        options.BatchSize = 100;
    });
});
```

## Error Handling Patterns

### Idempotent Event Handlers

Implement idempotent handlers for at-least-once delivery:

```csharp
public class SendOrderConfirmationHandler : IEventHandler<OrderCreatedEvent>
{
    private readonly IEmailService _emailService;
    private readonly INotificationRepository _repository;
    private readonly ILogger<SendOrderConfirmationHandler> _logger;
    
    public async ValueTask<Result> HandleAsync(
        OrderCreatedEvent @event,
        CancellationToken cancellationToken)
    {
        try
        {
            // Check if notification already sent (idempotency key)
            var notificationId = $"order-confirmation:{@event.OrderId}";
            var exists = await _repository.ExistsAsync(notificationId, cancellationToken);
            
            if (exists)
            {
                _logger.LogInformation(
                    "Order confirmation already sent for order {OrderId}", 
                    @event.OrderId);
                return Result.Success();
            }
            
            // Send email
            await _emailService.SendOrderConfirmationAsync(
                @event.CustomerId,
                @event.OrderId,
                cancellationToken);
            
            // Record that notification was sent
            await _repository.RecordAsync(
                notificationId,
                DateTimeOffset.UtcNow,
                cancellationToken);
            
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, 
                "Failed to send order confirmation for order {OrderId}", 
                @event.OrderId);
            return Result.Failure(ex.Message);
        }
    }
}
```

### Compensating Actions

Handle failures with compensating actions:

```csharp
public class ReserveInventoryHandler : IEventHandler<OrderCreatedEvent>
{
    private readonly IInventoryService _inventory;
    private readonly IPublisher _publisher;
    private readonly ILogger<ReserveInventoryHandler> _logger;
    
    public async ValueTask<Result> HandleAsync(
        OrderCreatedEvent @event,
        CancellationToken cancellationToken)
    {
        try
        {
            // Attempt to reserve inventory
            var reservationResult = await _inventory.ReserveAsync(
                @event.OrderId,
                @event.Items,
                cancellationToken);
            
            if (reservationResult.IsFailure)
            {
                // Publish compensation event
                await _publisher.PublishAsync(new InventoryReservationFailed
                {
                    OrderId = @event.OrderId,
                    Reason = reservationResult.ToString()
                }, cancellationToken);
                
                return reservationResult;
            }
            
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, 
                "Failed to reserve inventory for order {OrderId}", 
                @event.OrderId);
            
            // Publish compensation event
            await _publisher.PublishAsync(new InventoryReservationFailed
            {
                OrderId = @event.OrderId,
                Reason = ex.Message
            }, cancellationToken);
            
            return Result.Failure(ex.Message);
        }
    }
}
```

### Circuit Breaker Pattern

Implement circuit breaker for external dependencies:

```csharp
public class ExternalApiEventHandler : IEventHandler<DataSyncRequiredEvent>
{
    private readonly IExternalApiClient _apiClient;
    private readonly ICircuitBreakerPolicy _circuitBreaker;
    private readonly ILogger<ExternalApiEventHandler> _logger;
    
    public async ValueTask<Result> HandleAsync(
        DataSyncRequiredEvent @event,
        CancellationToken cancellationToken)
    {
        try
        {
            // Execute with circuit breaker
            var result = await _circuitBreaker.ExecuteAsync(async () =>
            {
                return await _apiClient.SyncDataAsync(@event.Data, cancellationToken);
            });
            
            return result;
        }
        catch (CircuitBreakerOpenException)
        {
            _logger.LogWarning(
                "Circuit breaker open, skipping external API call for event {EventId}",
                @event.Id);
            
            // Return success to avoid retry - circuit breaker will handle recovery
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to sync data to external API");
            return Result.Failure(ex.Message);
        }
    }
}
```

## Testing Patterns

### Testing Event Routing

Test routing filter logic:

```csharp
public class TenantBasedRoutingFilterTests
{
    [Fact]
    public void PremiumTenant_ReturnsHybridMode()
    {
        // Arrange
        var contextAccessor = new MockContextAccessor();
        contextAccessor.SetTenantId("premium-tenant");
        
        var tenantConfig = new MockTenantConfigurationService();
        tenantConfig.SetTier("premium-tenant", TenantTier.Premium);
        
        var filter = new TenantBasedRoutingFilter(contextAccessor, tenantConfig);
        var @event = new OrderCreatedEvent { /* ... */ };
        
        // Act
        var mode = filter.GetDistributionMode(@event);
        
        // Assert
        Assert.Equal(DistributionMode.Hybrid, mode);
    }
    
    [Fact]
    public void StandardTenant_ReturnsLocalOnly()
    {
        // Arrange
        var contextAccessor = new MockContextAccessor();
        contextAccessor.SetTenantId("standard-tenant");
        
        var tenantConfig = new MockTenantConfigurationService();
        tenantConfig.SetTier("standard-tenant", TenantTier.Standard);
        
        var filter = new TenantBasedRoutingFilter(contextAccessor, tenantConfig);
        var @event = new OrderCreatedEvent { /* ... */ };
        
        // Act
        var mode = filter.GetDistributionMode(@event);
        
        // Assert
        Assert.Equal(DistributionMode.LocalOnly, mode);
    }
}
```

### Testing Event Handlers

Test event handlers in isolation:

```csharp
public class UpdateInventoryHandlerTests
{
    [Fact]
    public async Task HandleAsync_ReservesInventory()
    {
        // Arrange
        var inventory = new MockInventoryService();
        var handler = new UpdateInventoryHandler(inventory);
        
        var @event = new OrderCreatedEvent
        {
            OrderId = Guid.NewGuid(),
            Items = new List<OrderItem>
            {
                new() { ProductId = Guid.NewGuid(), Quantity = 2 }
            }
        };
        
        // Act
        var result = await handler.HandleAsync(@event, CancellationToken.None);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(1, inventory.ReservationCount);
    }
}
```

## See Also

- [Unified Event Dispatching](./unified-event-dispatching.html) - Complete guide
- [Publish an Event](../../basics/publish-event.html) - Basic event publishing
- [Event Pipeline Behavior](../../behaviors/event-pipeline-behavior.html) - Custom behaviors
- [Migration Guide](../MIGRATION-GUIDE-v2.html) - Migrating to unified dispatching
