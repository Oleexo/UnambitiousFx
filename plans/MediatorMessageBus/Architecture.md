# Architecture Overview

This architecture merges the existing mediator and message bus capabilities into a single library without forcing additional complexity on teams that stay in-process. The default behaviour mirrors today's mediator: everything runs locally unless an application explicitly opts into distributed messaging.

## Guiding Simplicity Principles

- **Local-First Defaults**: Pure mediator scenarios require no new configuration or handler changes; distributed features layer on top only when requested.
- **Progressive Disclosure**: Each optional component exposes a minimal entry point with sensible defaults, keeping advanced policies, transports, and tooling behind deliberate configuration.
- **Familiar Mental Model**: Concepts reuse mediator terminology wherever possible so developers reason about envelopes, handlers, and pipelines with the same vocabulary across in-process and distributed flows.

## High-Level Components

1. **Message Envelope**
   - Canonical structure containing payload, headers, trace metadata, and transport hints.
   - Serves as the serialization contract when crossing process boundaries.
2. **Dispatcher Pipeline**
   - Extends current mediator pipeline with stages that evaluate distribution policies and invoke transports.
   - Evolves pipeline semantics to support distributed behaviour while reusing contextual metadata concepts and preserving local-only execution semantics by default.
3. **Transport Abstraction Layer**
   - `IMessageTransport` interface hides broker-specific mechanics for publish, subscribe, acknowledge, and dead-letter operations.
   - Starts with a single transport registration per application; additional transports remain an advanced opt-in for later phases.
4. **Subscription Registry**
   - Central catalogue of message types and their routing policies (local-only, hybrid, external-only).
   - Drives outbound routing decisions and inbound subscription creation.
5. **Receiver Host**
   - Background hosted service responsible for translating transport messages back into mediator invocations.
   - Handles concurrency, flow control, and lifecycle management.
6. **Reliability Services**
   - Outbox and optional inbox layers that guarantee delivery semantics and deduplication.
   - Policy-driven retries, dead-letter routing, and poison queue handling provided as opt-in extensions so the simple path stays lightweight.
7. **Observability Stack**
   - Logging, metrics, and tracing providers instrument each stage with consistent correlation identifiers.
   - Ships with structured logging enabled by default; richer metrics and tracing destinations activate only when configured.

## Interaction Flow

1. A handler sends or publishes a message through the mediator.
2. The dispatcher wraps the payload in a message envelope, enriching headers with correlation and tenant identifiers from the current context.
3. Distribution policies are evaluated:
   - Local-only messages execute existing handler dispatch.
   - Hybrid messages dispatch locally and enqueue to the configured transport.
   - External-only messages bypass local dispatch and rely solely on transport delivery.
4. When external dispatch is required, the message envelope is handed to the registered transport adapter, which handles serialization and broker communication.
5. Incoming messages are processed by the receiver host:
   - Messages are deserialized into envelopes, restored to mediator requests, and routed to appropriate handlers.
   - Acknowledgement or retry actions are determined by the outcome of handler execution and configured reliability policies.
6. Observability hooks record each outbound and inbound interaction, enabling cross-service tracing and diagnostics.

## Deployment Model

- **In-Process Only**: Applications continue to run purely in-memory with the noop transport.
- **Hybrid Application**: Mediator-enabled service publishes to an external broker while still executing local handlers.
- **Listener Service**: Dedicated worker host consumes from transport and delegates to mediator handlers, allowing vertical scaling for high-throughput scenarios.

## Extension Points

- **Transport Providers**: Custom implementations can target any broker by conforming to the transport interface and registering with the configuration API. Authoring guidance stresses keeping a single transport registration simple before layering in multi-transport scenarios.
- **Serialization Strategies**: JSON remains the default and only required serializer for the initial release; hooks for MessagePack, Protobuf, or custom formats are reserved for advanced adopters.
- **Policy Providers**: Reliability, retry, and deduplication policies can be supplied via configuration or code as optional enhancers per message type.
- **Pipeline Behaviors**: Existing mediator behaviors may require updates; new behaviors can inspect message envelopes for richer context without mandating changes for local-only applications.

## Migration Strategy

- Preserve the current mediator experience as the on-ramp; existing applications can upgrade the package with no configuration changes.
- Provide guidance and tooling to help teams refactor toward the new configuration and pipeline model when they choose to enable distributed messaging.
- Ensure observability and diagnostics remain available during migration, even as APIs change, with default logging requiring no additional setup.

## Runtime Targets

- Design core libraries to remain compatible with .NET NativeAOT and trimming requirements.
- Avoid reflection-heavy patterns or provide source generation where necessary to satisfy trimming analyzers.
- Acknowledge that third-party broker SDKs may limit NativeAOT support; wrap them behind abstraction boundaries and document any constraints.
