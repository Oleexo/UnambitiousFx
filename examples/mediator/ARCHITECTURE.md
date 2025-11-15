# Architecture Overview

This document provides visual representations of the mediator transport architecture.

## System Architecture

```
┌─────────────────────────────────────────────────────────────────────┐
│                          WebApi / WebApiAot                          │
│                                                                       │
│  ┌──────────────┐                                                    │
│  │   HTTP API   │                                                    │
│  └──────┬───────┘                                                    │
│         │                                                             │
│         ▼                                                             │
│  ┌──────────────┐         ┌─────────────────┐                       │
│  │   Commands   │────────▶│  Command        │                       │
│  │   Queries    │         │  Handlers       │                       │
│  └──────────────┘         └────────┬────────┘                       │
│                                     │                                 │
│                                     ▼                                 │
│                            ┌─────────────────┐                       │
│                            │   IPublisher    │                       │
│                            └────────┬────────┘                       │
│                                     │                                 │
│                    ┌────────────────┴────────────────┐               │
│                    │                                  │               │
│                    ▼                                  ▼               │
│         ┌──────────────────────┐         ┌──────────────────────┐   │
│         │    Local Events      │         │   External Events    │   │
│         │                      │         │                      │   │
│         │  • OrderCreated      │         │  • OrderShipped      │   │
│         │  • Notification      │         │  • PaymentProcessed  │   │
│         │                      │         │  • InventoryUpdated  │   │
│         └──────────┬───────────┘         └──────────┬───────────┘   │
│                    │                                 │               │
│                    │                                 │               │
│                    ▼                                 ▼               │
│         ┌──────────────────────┐         ┌──────────────────────┐   │
│         │   Event Handlers     │         │  Transport Layer     │   │
│         │   (Synchronous)      │         │  (RabbitMQ/InMemory) │   │
│         │                      │         │                      │   │
│         │  Execute immediately │         │  Publish to broker   │   │
│         │  in same transaction │         │  with persistence    │   │
│         └──────────────────────┘         └──────────┬───────────┘   │
│                                                      │               │
│                                                      │               │
│                                                      ▼               │
│                                          ┌──────────────────────┐   │
│                                          │   Event Handlers     │   │
│                                          │   (Asynchronous)     │   │
│                                          │                      │   │
│                                          │  Consume from broker │   │
│                                          │  with retry logic    │   │
│                                          └──────────────────────┘   │
└─────────────────────────────────────────────────────────────────────┘
```

## Event Flow Comparison

### Local Event Flow

```
┌─────────┐     ┌─────────┐     ┌───────────┐     ┌─────────┐
│ HTTP    │────▶│ Command │────▶│ Publisher │────▶│ Handler │
│ Request │     │ Handler │     │           │     │         │
└─────────┘     └─────────┘     └───────────┘     └─────────┘
                                                         │
                                                         ▼
                                                   ┌─────────┐
                                                   │ Response│
                                                   └─────────┘

Characteristics:
• Synchronous execution
• Same transaction
• Fast (< 1ms)
• In-process only
```

### External Event Flow

```
┌─────────┐     ┌─────────┐     ┌───────────┐     ┌──────────┐
│ HTTP    │────▶│ Command │────▶│ Publisher │────▶│ Transport│
│ Request │     │ Handler │     │           │     │  Layer   │
└─────────┘     └─────────┘     └───────────┘     └────┬─────┘
                                                        │
                                                        │ Async
                                                        ▼
                                                   ┌─────────┐
                                                   │ Message │
                                                   │ Broker  │
                                                   │(RabbitMQ)│
                                                   └────┬────┘
                                                        │
                                                        │ Subscribe
                                                        ▼
                                                   ┌─────────┐
                                                   │ Handler │
                                                   │ (Same or│
                                                   │ Different│
                                                   │ Process)│
                                                   └─────────┘

Characteristics:
• Asynchronous execution
• Separate transaction
• Slower (5-50ms)
• Distributed
• Reliable delivery
```

## Component Interaction

### Publisher Side

```
┌──────────────────────────────────────────────────────────┐
│                    Command Handler                        │
│                                                           │
│  public async ValueTask<Result> HandleAsync(...)         │
│  {                                                        │
│      // Business logic                                    │
│      var orderId = CreateOrder(...);                     │
│                                                           │
│      // Publish event                                     │
│      await _publisher.PublishAsync(new OrderShipped      │
│      {                                                    │
│          OrderId = orderId,                              │
│          TrackingNumber = trackingNumber,                │
│          ShippedAt = DateTime.UtcNow                     │
│      });                                                  │
│                                                           │
│      return Result.Success();                            │
│  }                                                        │
└──────────────────┬───────────────────────────────────────┘
                   │
                   ▼
┌──────────────────────────────────────────────────────────┐
│                      Publisher                            │
│                                                           │
│  • Checks event configuration (Local vs External)        │
│  • For Local: Dispatches to handlers immediately         │
│  • For External: Sends to transport layer                │
└──────────────────┬───────────────────────────────────────┘
                   │
                   ▼
┌──────────────────────────────────────────────────────────┐
│                   Transport Layer                         │
│                                                           │
│  • Serializes event to JSON                              │
│  • Adds metadata (correlation ID, timestamp, etc.)       │
│  • Publishes to message broker                           │
│  • Returns immediately (fire-and-forget)                 │
└──────────────────────────────────────────────────────────┘
```

### Subscriber Side

```
┌──────────────────────────────────────────────────────────┐
│                   Message Broker                          │
│                                                           │
│  • Receives message from publisher                       │
│  • Stores in queue                                       │
│  • Delivers to subscribers                               │
└──────────────────┬───────────────────────────────────────┘
                   │
                   ▼
┌──────────────────────────────────────────────────────────┐
│                   Transport Layer                         │
│                                                           │
│  • Receives message from broker                          │
│  • Deserializes JSON to event object                     │
│  • Extracts metadata                                     │
│  • Invokes event dispatcher                              │
└──────────────────┬───────────────────────────────────────┘
                   │
                   ▼
┌──────────────────────────────────────────────────────────┐
│                   Event Dispatcher                        │
│                                                           │
│  • Finds all registered handlers for event type          │
│  • Executes handlers (sequentially or parallel)          │
│  • Handles errors and retries                            │
│  • Acknowledges message to broker                        │
└──────────────────┬───────────────────────────────────────┘
                   │
                   ▼
┌──────────────────────────────────────────────────────────┐
│                   Event Handlers                          │
│                                                           │
│  [EventHandler<OrderShipped>]                            │
│  public class OrderShippedHandler                        │
│  {                                                        │
│      public async ValueTask<Result> HandleAsync(...)     │
│      {                                                    │
│          // Process the event                            │
│          _logger.LogInformation("Order shipped...");     │
│          return Result.Success();                        │
│      }                                                    │
│  }                                                        │
└──────────────────────────────────────────────────────────┘
```

## Fan-Out Pattern

```
                    ┌──────────────────┐
                    │  PaymentProcessed│
                    │      Event       │
                    └────────┬─────────┘
                             │
                             │ Published to Transport
                             ▼
                    ┌──────────────────┐
                    │  Message Broker  │
                    │    (RabbitMQ)    │
                    └────────┬─────────┘
                             │
                ┌────────────┴────────────┐
                │                         │
                ▼                         ▼
    ┌──────────────────────┐  ┌──────────────────────┐
    │ PaymentProcessed     │  │ PaymentAnalytics     │
    │ Handler              │  │ Handler              │
    │                      │  │                      │
    │ • Update order       │  │ • Record metrics     │
    │ • Send confirmation  │  │ • Update dashboard   │
    └──────────────────────┘  └──────────────────────┘

Both handlers execute independently:
• Separate transactions
• Independent failure handling
• Can scale separately
```

## Configuration Flow

```
┌──────────────────────────────────────────────────────────┐
│                    Startup (Program.cs)                   │
└──────────────────┬───────────────────────────────────────┘
                   │
                   ▼
┌──────────────────────────────────────────────────────────┐
│              AddMediator Configuration                    │
│                                                           │
│  services.AddMediator(cfg =>                             │
│  {                                                        │
│      // Register handlers                                │
│      cfg.RegisterRequestHandler<...>();                  │
│      cfg.RegisterEventHandler<...>();                    │
│                                                           │
│      // Enable distributed messaging                     │
│      cfg.EnableDistributedMessaging(messaging =>         │
│      {                                                    │
│          // Configure events                             │
│          messaging.ConfigureEvent<OrderShipped>(...)     │
│                                                           │
│          // Setup transport                              │
│          messaging.AddRabbitMq(...);                     │
│                                                           │
│          // Subscribe to events                          │
│          messaging.Subscribe<OrderShipped>();            │
│      });                                                  │
│  });                                                      │
└──────────────────┬───────────────────────────────────────┘
                   │
                   ▼
┌──────────────────────────────────────────────────────────┐
│              Service Registration                         │
│                                                           │
│  • IMediator                                             │
│  • ISender                                               │
│  • IPublisher                                            │
│  • IMessageTransport                                     │
│  • ITransportDispatcher                                  │
│  • Event Handlers                                        │
│  • Request Handlers                                      │
└──────────────────┬───────────────────────────────────────┘
                   │
                   ▼
┌──────────────────────────────────────────────────────────┐
│              Application Startup                          │
│                                                           │
│  • Connect to message broker                             │
│  • Subscribe to configured events                        │
│  • Start consuming messages                              │
│  • Ready to handle requests                              │
└──────────────────────────────────────────────────────────┘
```

## Message Envelope Structure

```
┌──────────────────────────────────────────────────────────┐
│                    MessageEnvelope                        │
├──────────────────────────────────────────────────────────┤
│  MessageId:       "550e8400-e29b-41d4-a716-446655440000" │
│  CorrelationId:   "660e8400-e29b-41d4-a716-446655440000" │
│  CausationId:     "770e8400-e29b-41d4-a716-446655440000" │
│  Timestamp:       "2024-11-13T10:30:00Z"                 │
│  PayloadType:     "OrderShipped"                         │
│  TenantId:        null                                   │
│                                                           │
│  Headers:                                                │
│    - topic:       "orders.shipped"                       │
│    - traceparent: "00-..."                               │
│    - tracestate:  "..."                                  │
│                                                           │
│  Payload:                                                │
│  {                                                        │
│    "orderId": "880e8400-e29b-41d4-a716-446655440000",   │
│    "trackingNumber": "TRACK-ABC123",                     │
│    "shippedAt": "2024-11-13T10:30:00Z"                  │
│  }                                                        │
└──────────────────────────────────────────────────────────┘
```

## Retry Flow

```
┌──────────────────┐
│  Message Arrives │
└────────┬─────────┘
         │
         ▼
┌──────────────────┐
│  Try Processing  │
└────────┬─────────┘
         │
         ├─────────────────┐
         │                 │
         ▼                 ▼
    ┌─────────┐      ┌──────────┐
    │ Success │      │  Failure │
    └────┬────┘      └─────┬────┘
         │                 │
         │                 ▼
         │           ┌──────────────┐
         │           │ Retry Count  │
         │           │ < Max?       │
         │           └──────┬───────┘
         │                  │
         │         ┌────────┴────────┐
         │         │                 │
         │         ▼                 ▼
         │    ┌─────────┐      ┌──────────┐
         │    │  Retry  │      │ Dead     │
         │    │  (Delay)│      │ Letter   │
         │    └────┬────┘      └──────────┘
         │         │
         │         └──────────┐
         │                    │
         ▼                    ▼
    ┌─────────────────────────────┐
    │      Acknowledge Message     │
    └─────────────────────────────┘
```

## Deployment Scenarios

### Single Instance (Development)

```
┌─────────────────────────────────┐
│         WebApi Instance          │
│                                  │
│  ┌──────────┐   ┌────────────┐ │
│  │Publisher │──▶│ InMemory   │ │
│  └──────────┘   │ Transport  │ │
│                 └──────┬─────┘ │
│                        │        │
│                        ▼        │
│                 ┌────────────┐ │
│                 │ Subscriber │ │
│                 └────────────┘ │
└─────────────────────────────────┘
```

### Multiple Instances (Production)

```
┌──────────────────┐         ┌──────────────────┐
│  WebApi Instance │         │  WebApi Instance │
│        #1        │         │        #2        │
│                  │         │                  │
│  ┌──────────┐   │         │   ┌──────────┐  │
│  │Publisher │───┼─────────┼──▶│Publisher │  │
│  └──────────┘   │         │   └──────────┘  │
└────────┬─────────┘         └────────┬────────┘
         │                            │
         └──────────┬─────────────────┘
                    ▼
         ┌──────────────────┐
         │     RabbitMQ     │
         │   Message Broker │
         └──────────┬───────┘
                    │
         ┌──────────┴─────────────────┐
         │                            │
         ▼                            ▼
┌──────────────────┐         ┌──────────────────┐
│  WebApi Instance │         │  WebApi Instance │
│        #1        │         │        #2        │
│                  │         │                  │
│  ┌────────────┐ │         │  ┌────────────┐ │
│  │ Subscriber │ │         │  │ Subscriber │ │
│  └────────────┘ │         │  └────────────┘ │
└──────────────────┘         └──────────────────┘

Load balanced event consumption:
• Each instance can publish
• Each instance can subscribe
• RabbitMQ distributes messages
• Automatic failover
```

## Summary

This architecture provides:

✅ **Flexibility** - Mix local and external events  
✅ **Scalability** - Horizontal scaling with message broker  
✅ **Reliability** - Message persistence and retry logic  
✅ **Decoupling** - Services don't need to know about each other  
✅ **Testability** - Use InMemory for tests, RabbitMQ for production  
✅ **Performance** - Local events for speed, external for distribution  

For implementation details, see:
- [README-TRANSPORTS.md](README-TRANSPORTS.md)
- [TRANSPORT-PATTERNS.md](TRANSPORT-PATTERNS.md)
- [QUICKSTART.md](QUICKSTART.md)
