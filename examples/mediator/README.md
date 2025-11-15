# Mediator Examples

This directory contains comprehensive examples demonstrating the UnambitiousFx Mediator library.

## Examples Overview

### 1. ConsoleApp - Performance Profiling
Located in `ConsoleApp/`

Demonstrates:
- Command handling (with and without responses)
- Query patterns
- Event publishing
- Streaming queries
- Pipeline behaviors
- Performance profiling scenarios

**Use this for**: Understanding basic mediator patterns and performance characteristics

### 2. WebApi & WebApiAot - Transport Examples
Located in `WebApi/` and `WebApiAot/`

Demonstrates:
- **Local Events** - In-process event handling
- **External Events** - Distributed messaging through transports
- **Fan-Out Pattern** - Multiple handlers for one event
- **In-Memory Transport** - Development and testing
- **RabbitMQ Transport** - Production messaging
- **Native AOT** compatibility (WebApiAot)

**Use this for**: Learning distributed messaging patterns and transport configuration

### 3. Application - Shared Domain
Located in `Application/`

Contains shared code used by WebApi and WebApiAot:
- Domain events
- Command/Query handlers
- Business logic

## Quick Start

### Option 1: Basic Mediator (ConsoleApp)

```bash
cd ConsoleApp
dotnet run
```

### Option 2: Transport Examples (WebApi)

```bash
cd WebApi
dotnet run
```

Then open `WebApi/WebApi.http` to test the endpoints.

### Option 3: With RabbitMQ

```bash
# Start RabbitMQ
docker-compose up -d

# Run the application
cd WebApi
dotnet run
```

## Documentation

- **[QUICKSTART.md](QUICKSTART.md)** - Get started in 5 minutes
- **[README-TRANSPORTS.md](README-TRANSPORTS.md)** - Full transport documentation
- **[TRANSPORT-PATTERNS.md](TRANSPORT-PATTERNS.md)** - Messaging patterns explained
- **[ConsoleApp/README.md](ConsoleApp/README.md)** - Profiling guide

## Project Structure

```
examples/mediator/
├── ConsoleApp/                    # Performance profiling examples
│   ├── Commands/
│   ├── Queries/
│   ├── Events/
│   ├── Handlers/
│   └── Pipelines/
│
├── Application/                   # Shared domain code
│   ├── Domain/
│   │   ├── Events/               # Domain events (local & external)
│   │   ├── Entities/
│   │   └── Repositories/
│   └── Application/
│       ├── Orders/               # Order-related handlers
│       ├── Payments/             # Payment-related handlers
│       ├── Inventory/            # Inventory-related handlers
│       ├── Notifications/        # Notification handlers
│       └── Todos/                # Todo handlers
│
├── WebApi/                        # Standard ASP.NET Core example
│   ├── Models/                   # Request/Response models
│   ├── Program.cs                # Configuration
│   ├── WebApi.http               # HTTP test file (includes transport examples)
│   └── Program.RabbitMQ.cs.example
│
├── WebApiAot/                     # Native AOT example
│   ├── Models/
│   └── Program.cs
│
├── docker-compose.yml             # RabbitMQ setup
├── QUICKSTART.md                  # Quick start guide
├── README-TRANSPORTS.md           # Transport documentation
└── TRANSPORT-PATTERNS.md          # Pattern explanations
```

## Features Demonstrated

### Core Mediator Features
- ✅ Commands (with and without responses)
- ✅ Queries
- ✅ Events
- ✅ Streaming queries
- ✅ Pipeline behaviors
- ✅ Request validation
- ✅ Error handling

### Transport Features
- ✅ Local events (in-process)
- ✅ External events (distributed)
- ✅ In-Memory transport
- ✅ RabbitMQ transport
- ✅ Event configuration
- ✅ Subscription management
- ✅ Retry policies
- ✅ Concurrency control
- ✅ Fan-out pattern
- ✅ Publish-Subscribe pattern

### Advanced Features
- ✅ Native AOT compatibility
- ✅ Source generators
- ✅ Dependency injection
- ✅ Logging integration
- ✅ Performance profiling

## Example Scenarios

### Scenario 1: Simple CRUD with Events (ConsoleApp)
```csharp
// Send a command
var command = new CreateOrderCommand { ... };
var result = await sender.SendAsync<CreateOrderCommand, Guid>(command);

// Publish an event
var @event = new OrderProcessedEvent { ... };
await publisher.PublishAsync(@event);
```

### Scenario 2: Local Events (WebApi)
```csharp
// Create order - triggers local events
POST /orders
{
  "customerName": "John Doe",
  "totalAmount": 299.99
}

// Events handled in-process:
// - OrderCreated
// - NotificationRequested
```

### Scenario 3: External Events (WebApi)
```csharp
// Ship order - triggers external event
POST /orders/{orderId}/ship

// Event flows through transport:
// OrderShipped → RabbitMQ → Subscribers
```

### Scenario 4: Fan-Out Pattern (WebApi)
```csharp
// Process payment - multiple handlers
POST /payments
{
  "orderId": "...",
  "amount": 299.99,
  "paymentMethod": "CreditCard"
}

// PaymentProcessed event handled by:
// - PaymentProcessedHandler (primary)
// - PaymentAnalyticsHandler (analytics)
```

## Configuration Examples

### Basic Mediator Setup
```csharp
services.AddMediator(cfg =>
{
    cfg.RegisterRequestHandler<CreateOrderCommandHandler, CreateOrderCommand, Guid>();
    cfg.RegisterEventHandler<OrderCreatedHandler, OrderCreated>();
});
```

### With In-Memory Transport
```csharp
services.AddMediator(cfg =>
{
    cfg.EnableDistributedMessaging(messaging =>
    {
        messaging.ConfigureEvent<OrderShipped>(opts => opts
            .AsExternal()
            .WithTopic("orders.shipped"));
        
        messaging.UseTransport<InMemoryTransport>();
        messaging.Subscribe<OrderShipped>();
    });
});
```

### With RabbitMQ Transport
```csharp
services.AddMediator(cfg =>
{
    cfg.EnableDistributedMessaging(messaging =>
    {
        messaging.ConfigureEvent<OrderShipped>(opts => opts
            .AsExternal()
            .WithTopic("orders.shipped"));
        
        messaging.AddRabbitMq(opts =>
        {
            opts.HostName = "localhost";
            opts.Port = 5672;
            opts.ExchangeName = "mediator-events";
        });
        
        messaging.Subscribe<OrderShipped>();
    });
});
```

## Testing the Examples

### Using HTTP Files
1. Open `WebApi/WebApi.http` in VS Code (with REST Client extension)
2. Run the requests to see events in action
3. Check console logs to see event flow

### Using curl
```bash
# Create order
curl -X POST http://localhost:5000/orders \
  -H "Content-Type: application/json" \
  -d '{"customerName":"John Doe","totalAmount":299.99}'

# Ship order
curl -X POST http://localhost:5000/orders/{orderId}/ship
```

### Using RabbitMQ Management UI
1. Start RabbitMQ: `docker-compose up -d`
2. Open http://localhost:15672 (guest/guest)
3. Navigate to Exchanges → `mediator-events`
4. See messages flowing through the system

## Performance Benchmarks

From ConsoleApp profiling:

| Operation | Throughput | Latency |
|-----------|-----------|---------|
| Simple Commands | 100,000+ ops/sec | < 0.1ms |
| Commands with Response | 100,000+ ops/sec | < 0.1ms |
| Queries | 100,000+ ops/sec | < 0.1ms |
| Local Events | 100,000+ ops/sec | < 0.1ms |
| External Events (InMemory) | 10,000+ ops/sec | 1-5ms |
| External Events (RabbitMQ) | 1,000-10,000 ops/sec | 5-50ms |

## Requirements

- .NET 9.0 or later
- Docker (for RabbitMQ examples)
- VS Code with REST Client extension (optional, for HTTP files)

## Next Steps

1. ✅ Start with [QUICKSTART.md](QUICKSTART.md)
2. 🧪 Try the HTTP examples in `WebApi/WebApi.http`
3. 📖 Read [README-TRANSPORTS.md](README-TRANSPORTS.md) for detailed transport documentation
4. 🎯 Study [TRANSPORT-PATTERNS.md](TRANSPORT-PATTERNS.md) for messaging patterns
5. � Explore uthe code in `Application/`, `WebApi/`, and `ConsoleApp/`
6. 🚀 Build your own events and handlers

## Support

For issues, questions, or contributions, please visit the main repository.

## License

See the main repository for license information.
