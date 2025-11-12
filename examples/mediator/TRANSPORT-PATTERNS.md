# Mediator Transport Patterns

This document explains the various messaging patterns demonstrated in the transport examples.

## Pattern Overview

### 1. Local Event Pattern (In-Process)

**Use Case**: Fast, synchronous operations within the same service

```csharp
// Configuration
messaging.ConfigureEvent<OrderCreated>(opts => opts.AsLocal());

// Publishing
await _publisher.PublishAsync(new OrderCreated
{
    OrderId = orderId,
    CustomerName = customerName,
    TotalAmount = totalAmount,
    CreatedAt = DateTime.UtcNow
}, cancellationToken);

// Handling
[EventHandler<OrderCreated>]
public class OrderCreatedHandler : IEventHandler<OrderCreated>
{
    public ValueTask<Result> HandleAsync(OrderCreated @event, CancellationToken ct)
    {
        // Handle immediately in-process
        return ValueTask.FromResult(Result.Success());
    }
}
```

**Characteristics**:
- ✅ Fast (no network overhead)
- ✅ Synchronous execution
- ✅ Transactional with the command
- ❌ Not distributed
- ❌ Single process only

**Examples in this project**:
- `OrderCreated` - Logs order creation
- `NotificationRequested` - Sends immediate notifications

---

### 2. External Event Pattern (Distributed)

**Use Case**: Cross-service communication, event-driven architecture

```csharp
// Configuration
messaging.ConfigureEvent<OrderShipped>(opts => opts
    .AsExternal()
    .WithTopic("orders.shipped"));

// Publishing
await _publisher.PublishAsync(new OrderShipped
{
    OrderId = orderId,
    TrackingNumber = trackingNumber,
    ShippedAt = DateTime.UtcNow
}, cancellationToken);

// Subscribing
messaging.Subscribe<OrderShipped>();

// Handling
[EventHandler<OrderShipped>]
public class OrderShippedHandler : IEventHandler<OrderShipped>
{
    public ValueTask<Result> HandleAsync(OrderShipped @event, CancellationToken ct)
    {
        // Handle event from transport
        return ValueTask.FromResult(Result.Success());
    }
}
```

**Characteristics**:
- ✅ Distributed across services
- ✅ Asynchronous execution
- ✅ Reliable delivery (with transport)
- ✅ Scalable (multiple consumers)
- ❌ Network latency
- ❌ Eventual consistency

**Examples in this project**:
- `OrderShipped` - Notifies shipping systems
- `PaymentProcessed` - Integrates with payment services
- `InventoryUpdated` - Syncs inventory across services

---

### 3. Fan-Out Pattern (Multiple Handlers)

**Use Case**: One event triggers multiple independent actions

```csharp
// Configuration (same as external event)
messaging.ConfigureEvent<PaymentProcessed>(opts => opts
    .AsExternal()
    .WithTopic("payments.processed"));

// Multiple handlers for the same event
[EventHandler<PaymentProcessed>]
public class PaymentProcessedHandler : IEventHandler<PaymentProcessed>
{
    public ValueTask<Result> HandleAsync(PaymentProcessed @event, CancellationToken ct)
    {
        // Primary handler - update order status
        return ValueTask.FromResult(Result.Success());
    }
}

[EventHandler<PaymentProcessed>]
public class PaymentAnalyticsHandler : IEventHandler<PaymentProcessed>
{
    public ValueTask<Result> HandleAsync(PaymentProcessed @event, CancellationToken ct)
    {
        // Secondary handler - record analytics
        return ValueTask.FromResult(Result.Success());
    }
}
```

**Characteristics**:
- ✅ Decoupled handlers
- ✅ Independent failure handling
- ✅ Easy to add new handlers
- ⚠️ All handlers execute (no short-circuit)

**Examples in this project**:
- `PaymentProcessed` → `PaymentProcessedHandler` + `PaymentAnalyticsHandler`
- `OrderShipped` → `OrderShippedHandler` + `OrderShippedNotificationHandler`

---

### 4. Publish-Subscribe Pattern

**Use Case**: Broadcasting events to multiple services

```csharp
// Service A - Publisher
messaging.ConfigureEvent<InventoryUpdated>(opts => opts
    .AsExternal()
    .WithTopic("inventory.updated"));

await _publisher.PublishAsync(new InventoryUpdated { ... });

// Service B - Subscriber
messaging.Subscribe<InventoryUpdated>();

// Service C - Subscriber
messaging.Subscribe<InventoryUpdated>();

// Both services receive the event independently
```

**Characteristics**:
- ✅ Loose coupling between services
- ✅ Multiple independent consumers
- ✅ Easy to add new subscribers
- ✅ Scalable

**Examples in this project**:
- `InventoryUpdated` - Can be consumed by warehouse, analytics, and reporting services

---

### 5. Request-Response with Events Pattern

**Use Case**: Command triggers events that other services react to

```csharp
// Step 1: Command creates order
public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
{
    public async ValueTask<Result<Guid>> HandleAsync(CreateOrderCommand request, CancellationToken ct)
    {
        var orderId = Guid.NewGuid();
        
        // Publish local event
        await _publisher.PublishAsync(new OrderCreated { OrderId = orderId }, ct);
        
        return Result.Success(orderId);
    }
}

// Step 2: Ship order command publishes external event
public class ShipOrderCommandHandler : IRequestHandler<ShipOrderCommand>
{
    public async ValueTask<Result> HandleAsync(ShipOrderCommand request, CancellationToken ct)
    {
        // Publish external event
        await _publisher.PublishAsync(new OrderShipped { OrderId = request.OrderId }, ct);
        
        return Result.Success();
    }
}

// Step 3: External systems react to OrderShipped
[EventHandler<OrderShipped>]
public class OrderShippedHandler : IEventHandler<OrderShipped>
{
    public ValueTask<Result> HandleAsync(OrderShipped @event, CancellationToken ct)
    {
        // Update tracking systems, notify customer, etc.
        return ValueTask.FromResult(Result.Success());
    }
}
```

**Flow**:
```
HTTP Request → Command → Handler → Event → Transport → Subscribers
```

**Examples in this project**:
- Create Order → OrderCreated (local) + NotificationRequested (local)
- Ship Order → OrderShipped (external) → Multiple handlers
- Process Payment → PaymentProcessed (external) → Multiple handlers

---

## Transport Configuration Patterns

### In-Memory Transport (Development/Testing)

```csharp
messaging.UseTransport<InMemoryTransport>();
```

**Use for**:
- Local development
- Integration tests
- Demos
- Single-process applications

### RabbitMQ Transport (Production)

```csharp
messaging.AddRabbitMq(opts =>
{
    opts.HostName = "localhost";
    opts.Port = 5672;
    opts.UserName = "guest";
    opts.Password = "guest";
    opts.ExchangeName = "mediator-events";
    opts.ExchangeType = "topic";
    opts.DurableExchange = true;
    opts.DurableQueues = true;
});
```

**Use for**:
- Production environments
- Distributed systems
- Microservices
- High availability scenarios

---

## Event Configuration Options

### Basic External Event

```csharp
messaging.ConfigureEvent<OrderShipped>(opts => opts
    .AsExternal()
    .WithTopic("orders.shipped"));
```

### With Retry Policy

```csharp
messaging.ConfigureEvent<PaymentProcessed>(opts => opts
    .AsExternal()
    .WithTopic("payments.processed")
    .WithRetryPolicy(maxAttempts: 3, retryDelay: TimeSpan.FromSeconds(5)));
```

### With Custom Serialization

```csharp
messaging.ConfigureEvent<InventoryUpdated>(opts => opts
    .AsExternal()
    .WithTopic("inventory.updated")
    .WithSerializer<CustomJsonSerializer>());
```

---

## Subscription Patterns

### Basic Subscription

```csharp
messaging.Subscribe<OrderShipped>();
```

### With Concurrency Control

```csharp
messaging.Subscribe<PaymentProcessed>(opts => opts
    .WithMaxConcurrency(10));
```

### With Retry Policy

```csharp
messaging.Subscribe<InventoryUpdated>(opts => opts
    .WithMaxConcurrency(5)
    .WithRetryPolicy(maxAttempts: 3, retryDelay: TimeSpan.FromSeconds(5)));
```

---

## Best Practices

### 1. Choose the Right Event Type

| Scenario | Event Type | Reason |
|----------|-----------|--------|
| Update database in same transaction | Local | Transactional consistency |
| Send email notification | Local | Fast, no need for distribution |
| Notify other services | External | Cross-service communication |
| Trigger workflows in other systems | External | Distributed processing |
| Analytics/Audit logging | External | Can be processed asynchronously |

### 2. Event Naming Conventions

```csharp
// ✅ Good - Past tense, describes what happened
OrderShipped
PaymentProcessed
InventoryUpdated

// ❌ Bad - Present tense or commands
ShipOrder
ProcessPayment
UpdateInventory
```

### 3. Event Data

```csharp
// ✅ Good - Contains all necessary data
public record OrderShipped
{
    public required Guid OrderId { get; init; }
    public required string TrackingNumber { get; init; }
    public required DateTime ShippedAt { get; init; }
}

// ❌ Bad - Requires additional queries
public record OrderShipped
{
    public required Guid OrderId { get; init; }
    // Missing tracking number and timestamp
}
```

### 4. Error Handling

```csharp
[EventHandler<PaymentProcessed>]
public class PaymentProcessedHandler : IEventHandler<PaymentProcessed>
{
    public async ValueTask<Result> HandleAsync(PaymentProcessed @event, CancellationToken ct)
    {
        try
        {
            // Process event
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to process payment event");
            // Return failure to trigger retry
            return Result.Failure(new Error(ex.Message));
        }
    }
}
```

### 5. Idempotency

```csharp
[EventHandler<OrderShipped>]
public class OrderShippedHandler : IEventHandler<OrderShipped>
{
    public async ValueTask<Result> HandleAsync(OrderShipped @event, CancellationToken ct)
    {
        // Check if already processed
        if (await _repository.IsProcessedAsync(@event.OrderId))
        {
            _logger.LogInformation("Event already processed, skipping");
            return Result.Success();
        }

        // Process event
        await ProcessShippingAsync(@event);

        // Mark as processed
        await _repository.MarkProcessedAsync(@event.OrderId);

        return Result.Success();
    }
}
```

---

## Testing Patterns

### Unit Testing with In-Memory Transport

```csharp
[Fact]
public async Task ShipOrder_PublishesOrderShippedEvent()
{
    // Arrange
    var services = new ServiceCollection()
        .AddMediator(cfg =>
        {
            cfg.RegisterRequestHandler<ShipOrderCommandHandler, ShipOrderCommand>();
            cfg.RegisterEventHandler<OrderShippedHandler, OrderShipped>();
            cfg.EnableDistributedMessaging(m => m.UseTransport<InMemoryTransport>());
        })
        .BuildServiceProvider();

    var sender = services.GetRequiredService<ISender>();

    // Act
    var result = await sender.SendAsync(new ShipOrderCommand { OrderId = Guid.NewGuid() });

    // Assert
    Assert.True(result.IsSuccess);
}
```

### Integration Testing with RabbitMQ

```csharp
[Fact]
public async Task PaymentProcessed_IsConsumedFromRabbitMQ()
{
    // Arrange - Start RabbitMQ container
    await using var container = new RabbitMqBuilder().Build();
    await container.StartAsync();

    // Configure services with RabbitMQ
    var services = new ServiceCollection()
        .AddMediator(cfg =>
        {
            cfg.EnableDistributedMessaging(m =>
            {
                m.AddRabbitMq(opts => opts.HostName = container.Hostname);
                m.Subscribe<PaymentProcessed>();
            });
        })
        .BuildServiceProvider();

    // Act & Assert
    // ...
}
```

---

## Performance Considerations

### Local Events
- **Latency**: < 1ms
- **Throughput**: 100,000+ events/sec
- **Use when**: Speed is critical, no distribution needed

### External Events (In-Memory)
- **Latency**: 1-5ms
- **Throughput**: 10,000+ events/sec
- **Use when**: Testing, development, single process

### External Events (RabbitMQ)
- **Latency**: 5-50ms
- **Throughput**: 1,000-10,000 events/sec
- **Use when**: Production, distributed systems

---

## Summary

This example demonstrates:

✅ **Local Events** - Fast, in-process event handling  
✅ **External Events** - Distributed messaging through transports  
✅ **Fan-Out Pattern** - Multiple handlers for one event  
✅ **Publish-Subscribe** - Broadcasting to multiple services  
✅ **In-Memory Transport** - Development and testing  
✅ **RabbitMQ Transport** - Production-ready messaging  
✅ **Retry Policies** - Resilient event processing  
✅ **Concurrency Control** - Scalable event consumption  

For more information, see:
- [Quick Start Guide](QUICKSTART.md)
- [Full Documentation](README-TRANSPORTS.md)
- [HTTP Examples](WebApi/WebApi.http)
