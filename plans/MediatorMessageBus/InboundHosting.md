# Inbound Hosting Plan

## Objective

Create a hosting layer that subscribes to external transports, receives messages, and invokes mediator handlers with appropriate reliability guarantees.

## Key Responsibilities

1. **Subscription Management**
   - Register subscriptions based on the message trait registry and transport configuration.
   - Support dynamic updates (hot reload or configuration refresh) where feasible.
2. **Message Pump**
   - Continuously receive messages with configurable concurrency, batching, and prefetch settings.
   - Integrate cancellation tokens to support graceful shutdown.
3. **Deserialization & Envelope Restoration**
   - Convert transport payloads back into message envelopes, validate schema/version compatibility, and reconstruct mediator requests.
4. **Handler Invocation**
   - Execute mediator pipeline using reconstructed message, capturing results and exceptions.
5. **Acknowledgement & Retry**
   - Acknowledge messages only after successful processing.
   - Apply retry policies including immediate retries, exponential backoff, and dead-letter routing.
6. **Health & Monitoring**
   - Emit health check status for subscription readiness and error rates.
   - Provide runtime diagnostics (queue depth, in-flight messages, failure counts).

## Hosting Model Options

- **In-Process Hosted Service**: Utilize `IHostedService` or `.NET` background service to run message pumps within application host.
- **External Worker Template**: Provide optional standalone worker project for scenarios requiring independent scaling.
- **Hybrid Approach**: Allow multiple hosted services per transport instance to enable sharding or partition-level scaling.

## Implementation Steps

- [ ] Define `SubscriptionDescriptor` capturing transport name, message type, filter/topic, concurrency limit, retry policy.
- [ ] Implement base `MessagePump` class handling lifecycle, concurrency, and error propagation.
- [ ] Develop transport-specific receivers that adapt broker callbacks/polling to the base pump.
- [ ] Integrate mediator invocation pipeline, ensuring context metadata (correlation, tenant) is restored.
- [ ] Provide hooks for custom middleware (e.g., metrics, logging) around handler execution.
- [ ] Expose management APIs or integration with ASP.NET health checks for monitoring.

## Testing Approach

- Unit tests for subscription registration logic and cancellation behaviour.
- Integration tests using mock transport to simulate message inflow, retries, and dead-letter conditions.
- Stress tests to validate concurrency controls and benchmark throughput.

## Documentation Requirements

- Step-by-step guide for enabling inbound hosting within an existing service.
- Instructions for deploying a dedicated worker host, including containerization guidance.
- Troubleshooting section for connection errors, schema mismatches, and poison messages.
