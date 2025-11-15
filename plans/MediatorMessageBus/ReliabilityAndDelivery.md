# Reliability and Delivery Guarantees Plan

## Objectives

- Provide configurable delivery guarantees (best-effort by default, with opt-in at-least-once or exactly-once via outbox) for outbound and inbound flows.
- Offer retry, backoff, and dead-letter strategies that match common broker capabilities without complicating teams that do not enable them.
- Surface deduplication primitives to support idempotent handlers and saga coordination as an optional layer.

## Core Capabilities

1. **Outbox Enhancements**
   - Extend existing outbox storage to persist message envelopes and transport metadata when an application opts into deferred or reliable dispatch.
   - Support per-transport dispatch workers and back-pressure mechanisms.
   - Expose monitoring metrics (pending count, dispatch latency, failure rate).
2. **Inbox / Deduplication**
   - Optional inbox storage keyed by message id and source to detect duplicates.
   - Configurable retention and eviction policies.
3. **Retry Policies**
   - Immediate retries with configurable count, disabled by default to keep the baseline behaviour predictable.
   - Exponential backoff and jitter strategies.
   - Circuit breaker integration to protect transports during sustained outages.
4. **Poison Message Handling**
   - Dead-letter routing with contextual metadata and reason codes.
   - Configurable max delivery attempts, with hook to invoke custom failure handlers.
5. **Transactional Boundaries**
   - Guidance for coordinating mediator operations and outbox persistence within application transactions when the outbox is enabled.
   - Support for distributed transactions where available, but default to transactional outbox patterns.

## Implementation Tasks

- [ ] Augment outbox schema to store serialized envelope, target transport, and policy flags.
- [ ] Implement outbox dispatcher service that respects per-transport throttling.
- [ ] Introduce retry policy abstractions and default implementations (immediate, exponential) with defaults disabled until explicitly configured.
- [ ] Build deduplication store with pluggable backends (memory for tests, persistent for production).
- [ ] Wire policy evaluation into publishing pipeline and inbound hosting components.
- [ ] Add configuration surface for setting policies per message type or transport.

## Testing Strategy

- Unit tests for retry logic, ensuring time calculations and max attempt thresholds behave as expected.
- Integration tests simulating transient transport failures with eventual success.
- Tests verifying outbox dispatch order, idempotent handling, and poison routing.
- Stress tests to evaluate performance impact of deduplication and outbox processing.

## Documentation Requirements

- Explain supported delivery modes and how to choose between them.
- Provide cookbook examples for common requirements (e.g., exactly-once with SQL outbox, at-least-once with SQS).
- Clarify operational procedures for monitoring outbox/inbox tables and handling dead-letter queues.
