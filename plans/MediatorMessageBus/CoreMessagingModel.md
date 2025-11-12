# Core Messaging Model Plan

## Goals

- Establish a unified message envelope that supports both in-memory and broker-based communication scenarios without adding ceremony to existing handlers.
- Provide consistent headers for correlation, causation, tenancy, user identity, and versioning with sensible defaults that populate automatically.
- Simplify message taxonomy to Command, Query, and Event while leaving room for future notification or stream abstractions.

## Deliverables

1. **Envelope Specification**
   - Define required fields: message id, correlation id, causation id, tenant id, timestamp, payload type, payload serialization metadata.
   - Document optional headers: auth claims snapshot, routing hints, priority, delivery mode.
   - Offer extension mechanism for custom headers while protecting reserved keys and keeping optional data out of the happy path.
2. **Envelope Builder**
   - Utility to construct envelopes from mediator requests using context metadata.
   - Supports overrides via pipeline behaviors for advanced scenarios (e.g., overriding tenant id).
3. **Message Traits Registry**
   - Mechanism to annotate message types with attributes or configuration (local, hybrid, external-only) while defaulting all messages to local-only mode.
   - Extracts additional metadata such as default topic, partition key, fail-fast behavior only when developers supply overrides.
   - Supports sensitivity metadata (e.g., attribute-based tagging of fields requiring encryption or role-based access) as an optional add-on.
4. **Serialization Contract**
   - Standard interface for serializing envelopes into transport payloads and vice versa.
   - Default JSON implementation provided out of the box; hooks for custom serializer injection documented for advanced scenarios.
5. **Schema Documentation**
   - Publish schema and versioning guidelines to `docs/` ensuring cross-service compatibility.

## Tasks

- [ ] Write envelope struct/class and supporting header collection abstractions.
- [ ] Implement extension methods on mediator context to populate correlation identifiers when absent.
- [ ] Create attributes (`[DistributedEvent]`, `[DistributedCommand]`, etc.) or alternative fluent configuration for message traits, ensuring attribute usage is optional sugar over configuration APIs.
- [ ] Design serialization abstractions with versioning support (ex: content-type header, schema version).
- [ ] Produce unit tests covering envelope construction for commands, queries, and events.
- [ ] Document guidelines for message authors (naming, immutability, serialization safety).

## Acceptance Criteria

- Envelope can round-trip between in-memory dispatch and serialized payload without losing metadata.
- Message traits can be defined via attribute or configuration and inspected during dispatch.
- Serialization abstraction allows swapping implementation without touching handlers.
- Documentation clearly states how to add new headers and how versioning is handled.
