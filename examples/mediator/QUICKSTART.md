# Quick Start Guide - Mediator with Transports

This guide will get you up and running with the Mediator transport examples in 5 minutes.

## Option 1: In-Memory Transport (Simplest)

The examples are already configured to use in-memory transport, which simulates external messaging within the same process.

### Run the application

```bash
cd examples/mediator/WebApi
dotnet run
```

### Test the endpoints

Open `WebApi/WebApi.http` in VS Code (with REST Client extension) or your favorite HTTP client.

Try these requests:

```bash
# 1. Create an order (local events)
POST http://localhost:5000/orders
Content-Type: application/json

{
  "customerName": "John Doe",
  "totalAmount": 299.99
}

# 2. Ship the order (external event through transport)
POST http://localhost:5000/orders/{orderId}/ship

# 3. Process payment (external event through transport)
POST http://localhost:5000/payments
Content-Type: application/json

{
  "orderId": "{orderId}",
  "amount": 299.99,
  "paymentMethod": "CreditCard"
}
```

### What to observe

Check the console logs. You'll see:
- **Local events** handled immediately in-process
- **External events** going through the transport layer and being consumed

## Option 2: RabbitMQ Transport (Production-like)

For a more realistic distributed messaging experience with RabbitMQ.

### 1. Start RabbitMQ

```bash
cd examples/mediator
docker-compose up -d
```

Or manually:
```bash
docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
```

### 2. Verify RabbitMQ is running

Open http://localhost:15672 in your browser
- Username: `guest`
- Password: `guest`

### 3. Update WebApi configuration

Replace the `EnableDistributedMessaging` section in `Program.cs` with the code from `Program.RabbitMQ.cs.example`.

Or simply:

```csharp
// Change this line:
messaging.UseTransport<InMemoryTransport>();

// To this:
messaging.AddRabbitMq(opts =>
{
    opts.HostName = "localhost";
    opts.Port = 5672;
    opts.UserName = "guest";
    opts.Password = "guest";
    opts.ExchangeName = "mediator-events";
});
```

### 4. Run the application

```bash
cd examples/mediator/WebApi
dotnet run
```

### 5. Test and observe

1. Send requests using `WebApi/WebApi.http`
2. Watch the console logs
3. Open RabbitMQ Management UI (http://localhost:15672)
4. Navigate to **Exchanges** → `mediator-events` to see messages
5. Navigate to **Queues** to see message queues

## Understanding the Flow

### Local Event Flow
```
Command → Handler → Publisher → Local Event Handler
                                 (in-process, immediate)
```

### External Event Flow
```
Command → Handler → Publisher → Transport Layer (RabbitMQ/InMemory)
                                      ↓
                                 Subscriber
                                      ↓
                                Event Handler
```

## Example Scenarios

All examples are available in `WebApi/WebApi.http` and `WebApiAot/WebApiAot.http`.

### Scenario 1: Order Processing

```bash
# Step 1: Create order (triggers local events)
POST /orders
{
  "customerName": "Jane Smith",
  "totalAmount": 599.99
}
# Response: { "orderId": "..." }

# Step 2: Process payment (external event)
POST /payments
{
  "orderId": "...",
  "amount": 599.99,
  "paymentMethod": "CreditCard"
}

# Step 3: Update inventory (external event)
POST /inventory/{productId}/update
{
  "quantityChange": -1
}

# Step 4: Ship order (external event)
POST /orders/{orderId}/ship
```

### Scenario 2: Inventory Management

```bash
# Decrease inventory (external event)
POST /inventory/{productId}/update
{
  "quantityChange": -10
}

# Increase inventory (external event)
POST /inventory/{productId}/update
{
  "quantityChange": 50
}
```

## Events Overview

| Event | Type | Description |
|-------|------|-------------|
| `OrderCreated` | Local | Order creation notification |
| `NotificationRequested` | Local | Email/SMS notification |
| `OrderShipped` | External | Order shipping notification |
| `PaymentProcessed` | External | Payment confirmation |
| `InventoryUpdated` | External | Inventory level changes |

## Troubleshooting

### RabbitMQ connection issues

```bash
# Check if RabbitMQ is running
docker ps | grep rabbitmq

# Check RabbitMQ logs
docker logs mediator-rabbitmq

# Restart RabbitMQ
docker-compose restart rabbitmq
```

### No events being processed

1. Check that handlers are registered in `Program.cs`
2. Verify event configuration (`.AsExternal()` vs `.AsLocal()`)
3. Check that you've subscribed to external events (`.Subscribe<EventType>()`)
4. Look for errors in console logs

### Events not appearing in RabbitMQ

1. Verify the event is configured as `.AsExternal()`
2. Check RabbitMQ connection settings
3. Ensure the exchange exists in RabbitMQ Management UI
4. Check application logs for transport errors

## Next Steps

1. ✅ Run the examples with in-memory transport
2. ✅ Try the HTTP examples in `WebApi/WebApi.http`
3. ✅ Try RabbitMQ transport
4. 📖 Read the full documentation in `README-TRANSPORTS.md`
5. 🔧 Create your own events and handlers
6. 🚀 Deploy multiple instances to see distributed consumption

## Clean Up

```bash
# Stop RabbitMQ
docker-compose down

# Remove volumes (optional)
docker-compose down -v
```

## Additional Resources

- [Full Transport Documentation](README-TRANSPORTS.md)
- [HTTP Examples](WebApi/WebApi.http)
- [RabbitMQ Configuration Example](WebApi/Program.RabbitMQ.cs.example)
