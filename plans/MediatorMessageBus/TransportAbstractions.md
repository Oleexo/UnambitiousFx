# Transport Abstractions Plan

## Objectives

- Provide a broker-neutral interface that encapsulates publish, subscribe, acknowledgement, and dead-letter functionality while keeping the noop transport as the default.
- Enable a single transport registration with scoped configuration for the initial release, with multiple transports treated as an advanced scenario for later phases.
- Support custom transport adapters that can be contributed externally with minimal friction when teams are ready to extend beyond the defaults.

## Key Interfaces

### IMessageTransport

```csharp
public interface IMessageTransport
{
    ValueTask PublishAsync(MessageEnvelope envelope, CancellationToken cancellationToken);
    ValueTask SubscribeAsync(SubscriptionDescriptor descriptor, CancellationToken cancellationToken);
    ValueTask UnsubscribeAsync(SubscriptionDescriptor descriptor, CancellationToken cancellationToken);
}
```

### ITransportMessageProcessor

```csharp
public interface ITransportMessageProcessor
{
    ValueTask ProcessAsync(TransportMessage message, CancellationToken cancellationToken);
}
```

- Separates message fetching from mediator invocation to allow custom batching, streaming, or transactional processing.

## Support Components

- **TransportOptions**: Holds configuration such as connection strings, queue/topic names, credential strategies, batching limits, with defaults aligned to the simplest supported broker.
- **TransportRegistration**: Connects a message type and policy to a specific transport instance, defaulting to a single transport per application.
- **TransportLifetime Management**: Factory pattern ensuring connections are re-used appropriately and disposed gracefully.

## Implementation Steps

1. **Define Contracts**
   - Finalize interfaces for publish/subscribe/unsubscribe, message processor, and transport options.
   - Provide default implementations of descriptors (subscription filters, concurrency limits).
2. **Noop Transport**
   - Implement in-memory transport used when distributed messaging is disabled.
   - Ensures API parity without external dependencies.
3. **Transport Registry**
   - Extend mediator configuration to register transports by name and associate message types with transports.
   - Focus on a single transport within the same application for the first release, documenting how multiple transports will evolve without exposing that complexity prematurely.
4. **Adapter Authoring Guide**
   - Document required behaviors (idempotence, error signalling, cancellation handling).
   - Provide base classes or helper utilities for common patterns (e.g., message serialization, logging wrappers).
5. **Lifecycle Hooks**
   - Ensure adapters can access application lifecycle events (start, stop) to manage broker connections.
   - Provide health check integration points.

## Testing Strategy

- Contract tests using a harness that validates adapter conformity by simulating publish and receive flows.
- Mock transport for unit testing mediator pipeline without external dependencies.
- Integration test suites for each official adapter (executed conditionally when broker is available).

## Deliverables Checklist

- [ ] Published interface definitions and supporting descriptors.
- [ ] Noop transport registered by default.
- [ ] Configuration API for registering transports and binding message types.
- [ ] Adapter development guide in documentation.
- [ ] Contract test harness packaged for reuse by third-party adapters.
