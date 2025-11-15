# Mediator Transport Examples

This example demonstrates how to use the UnambitiousFx Mediator with transport layers to handle both **local** and **external** events.

## Overview

The example shows:
- **Local Events**: Handled within the same process (no transport layer)
- **External Events**: Published to and consumed from a transport layer (RabbitMQ, in-memory, etc.)
- How the same process can both publish and consume external events
- Various usage patterns for distributed messaging

## Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                        WebApi / WebApiAot                    │
│                                                               │
│  ┌──────────────┐         ┌──────────────┐                  │
│  │   Commands   │────────▶│   Handlers   │                  │
│  └──────────────┘         └──────┬───────┘                  │
│                                   │                           │
│                                   ▼                           │
│                          ┌────────────────┐                  │
│                          │   Publisher    │                  │
│                          └────────┬───────┘                  │
│                                   │                           │
│                    ┌──────────────┴──────────────┐           │
│                    │                              │           │
│                    ▼                              ▼           │
│           ┌────────────────┐           ┌─────────────────┐   │
│           │  Local Events  │           │ External Events │   │
│           │                │           │                 │   │
│           │ • OrderCreated │           │ • OrderShipped  │   │
│           │ • Notification │           │ • Payment       │   │
│           └────────┬───────┘           │ • Inventory     │   │
│                    │                   └────────┬────────┘   │
│                    │                            │             │
│                    ▼                            ▼             │
│           ┌────────────────┐           ┌─────────────────┐   │
│           │ Local Handlers │           │   Transport     │   │
│           │  (in-process)  │           │  (RabbitMQ/     │   │
│           └────────────────┘           │   InMemory)     │   │
│                                        └────────┬────────┘   │
│                                                 │             │
│                                                 ▼             │
│                                        ┌─────────────────┐   │
│                                        │ Event Handlers  │   │
│                                        │  (subscribed)   │   │
│                                        └─────────────────┘   │
└─────────────────────────────────────────────────────────────┘
```

## Events

### Local Events (In-Process Only)

These events are handled synchronously within the same process and are NOT sent through the transport layer:

1. **OrderCreated** - Triggered when a new order is created
2. **NotificationRequested** - Triggered to send notifications (email, SMS, etc.)

### External Events (Transport Layer)

These events are published to the transport layer and can be consumed by other services or the same service:

1. **OrderShipped** - Published when an order is shipped
2. **PaymentProcessed** - Published when a payment is processed
3. **InventoryUpdated** - Published when inventory levels change

## Configuration

### WebApi Configuration

```csharp
builder.Services.AddMediator(cfg =>
{
    // Register handlers...

    cfg.EnableDistributedMessaging(messaging =>
    {
        // Configure external events
        messaging.ConfigureEvent<OrderShipped>(opts => opts
            .AsExternal()
            .WithTopic("orders.shipped"));

        messaging.ConfigureEvent<PaymentProcessed>(opts => opts
            .AsExternal()
            .WithTopic("payments.processed"));

        messaging.ConfigureEvent<InventoryUpdated>(opts => opts
            .AsExternal()
            .WithTopic("inventory.updated"));

        // Configure local events
        messaging.ConfigureEvent<OrderCreated>(opts => opts.AsLocal());
        messaging.ConfigureEvent<NotificationRequested>(opts => opts.AsLocal());

        // Use transport (InMemory for demo, RabbitMQ for production)
        messaging.UseTransport<InMemoryTransport>();
        // For RabbitMQ: messaging.AddRabbitMq(opts => { ... });

        // Subscribe to external events
        messaging.Subscribe<OrderShipped>();
        messaging.Subscribe<PaymentProcessed>();
        messaging.Subscribe<InventoryUpdated>();
    });
});
```

## Usage Examples

### 1. Create Order (Local Events)

```bash
POST /orders
{
  "customerName": "John Doe",
  "totalAmount": 299.99
}
```

**What happens:**
- Creates an order
- Publishes `OrderCreated` event (LOCAL - handled in-process)
- Publishes `NotificationRequested` event (LOCAL - sends email notification)

### 2. Ship Order (External Event)

```bash
POST /orders/{orderId}/ship
```

**What happens:**
- Publishes `OrderShipped` event to transport layer
- Event is sent through RabbitMQ/InMemory transport
- Same process (or other services) can consume this event
- Handler logs the shipping information

### 3. Process Payment (External Event)

```bash
POST /payments
{
  "orderId": "...",
  "amount": 299.99,
  "paymentMethod": "CreditCard"
}
```

**What happens:**
- Publishes `PaymentProcessed` event to transport layer
- Event flows through the transport
- Handler processes the payment confirmation

### 4. Update Inventory (External Event)

```bash
POST /inventory/{productId}/update
{
  "quantityChange": -5
}
```

**What happens:**
- Publishes `InventoryUpdated` event to transport layer
- Event is distributed to all subscribers
- Inventory management systems can react to changes

## Transport Options

### In-Memory Transport (Demo)

Used in this example for simplicity. Simulates external transport within the same process:

```csharp
messaging.UseTransport<InMemoryTransport>();
```

### RabbitMQ Transport (Production)

For production use with actual message broker:

```csharp
messaging.AddRabbitMq(opts =>
{
    opts.HostName = "localhost";
    opts.Port = 5672;
    opts.UserName = "guest";
    opts.Password = "guest";
    opts.ExchangeName = "mediator-events";
});
```

## Running the Examples

### WebApi (Standard)

```bash
cd examples/mediator/WebApi
dotnet run
```

### WebApiAot (Native AOT)

```bash
cd examples/mediator/WebApiAot
dotnet run
```

### Test with HTTP file

Use the `WebApi/WebApi.http` file with your favorite HTTP client (VS Code REST Client, Rider, etc.)

## Key Concepts

### Local vs External Events

- **Local Events**: Fast, synchronous, in-process only. Use for internal business logic.
- **External Events**: Asynchronous, distributed, can cross service boundaries. Use for integration.

### Event Configuration

```csharp
// External event
messaging.ConfigureEvent<OrderShipped>(opts => opts
    .AsExternal()
    .WithTopic("orders.shipped")
    .WithRetryPolicy(3, TimeSpan.FromSeconds(5)));

// Local event
messaging.ConfigureEvent<OrderCreated>(opts => opts.AsLocal());
```

### Subscribing to Events

```csharp
// Subscribe to consume events from transport
messaging.Subscribe<OrderShipped>();
messaging.Subscribe<PaymentProcessed>();
```

### Publishing Events

```csharp
// In your handler
await _publisher.PublishAsync(new OrderShipped
{
    OrderId = orderId,
    TrackingNumber = trackingNumber,
    ShippedAt = DateTime.UtcNow
}, cancellationToken);
```

## Benefits

1. **Decoupling**: Services don't need to know about each other
2. **Scalability**: External events can be processed by multiple consumers
3. **Reliability**: Transport layer provides message persistence and retry
4. **Flexibility**: Mix local and external events based on requirements
5. **Testability**: Use InMemory transport for testing, RabbitMQ for production

## Project Structure

```
Application/
├── Domain/
│   └── Events/
│       ├── OrderCreated.cs          (Local)
│       ├── OrderShipped.cs          (External)
│       ├── PaymentProcessed.cs      (External)
│       ├── InventoryUpdated.cs      (External)
│       └── NotificationRequested.cs (Local)
└── Application/
    ├── Orders/
    │   ├── CreateOrderCommand.cs
    │   ├── CreateOrderCommandHandler.cs
    │   ├── ShipOrderCommand.cs
    │   ├── ShipOrderCommandHandler.cs
    │   ├── OrderCreatedHandler.cs
    │   └── OrderShippedHandler.cs
    ├── Payments/
    │   ├── ProcessPaymentCommand.cs
    │   ├── ProcessPaymentCommandHandler.cs
    │   └── PaymentProcessedHandler.cs
    ├── Inventory/
    │   ├── UpdateInventoryCommand.cs
    │   ├── UpdateInventoryCommandHandler.cs
    │   └── InventoryUpdatedHandler.cs
    └── Notifications/
        └── NotificationRequestedHandler.cs

WebApi/
├── Program.cs                       (Configuration)
├── Models/                          (Request models)
└── WebApi.http                      (HTTP examples)

WebApiAot/
├── Program.cs                       (AOT-compatible configuration)
└── Models/                          (Request models)
```

## Next Steps

1. Try the examples with the HTTP file
2. Check the logs to see local vs external event handling
3. Experiment with RabbitMQ transport for real distributed messaging
4. Create your own events and handlers
5. Deploy multiple instances to see distributed event consumption
