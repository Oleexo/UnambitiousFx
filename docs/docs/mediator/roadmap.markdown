---
title: Roadmap
parent: Mediator
nav_order: 7
---

# Mediator Roadmap

This page summarizes the current and planned capabilities for the `Mediator` library. This roadmap focuses on high-signal planning items and provides visibility into the evolution of the mediator pattern implementation.

## Legend
✅ Implemented  ·  🔄 In Progress  ·  ⭐ High Priority (next 1–2 milestones)  ·  📋 Planned  ·  🤔 Considering

## Phase Overview
1. Foundation (✅ complete)
2. Core Request/Event Handling (✅ complete)
3. Pipeline Behaviors & Orchestration (✅ complete)
4. Source Generation & DI Integration (✅ complete)
5. Advanced Event Patterns (🔄 outbox refinement, distributed events)
6. Performance & Scalability (⭐ streaming, batching, caching)
7. Observability & Diagnostics (📋 tracing, metrics, debugging)
8. Resilience & Error Recovery (📋 retry policies, circuit breakers)
9. Advanced Composition (📋 saga patterns, distributed transactions)
10. Tooling & Developer Experience (📋 analyzers, testing utilities)

## Recently Completed Highlights
- Request/response pattern with `ISender` returning `Result<TResponse>` or `Result`
- Event publishing with `IPublisher` supporting multiple handlers per event
- Pipeline behaviors for both requests (`IRequestPipelineBehavior`) and events (`IEventPipelineBehavior`)
- Event orchestration: Sequential and Concurrent execution strategies
- Outbox pattern support with `IEventOutboxStorage` and `PublishMode` enum
- Source generation with `MediatorGenerator` for automatic handler registration
- Context propagation with `IContext` and `IContextFactory`
- Integration with Result type for functional error handling

## Active / Next (Short Horizon)

| Item                                   | Status | Notes                                            |
| -------------------------------------- | ------ | ------------------------------------------------ |
| Outbox pattern improvements            | ⭐      | Retry logic, dead-letter queue, batch processing |
| Request pipeline behaviors enhancement | ⭐      | Generic typed behaviors, conditional execution   |
| Context enrichment                     | 📋      | Metadata helpers, correlation ID propagation     |
| Handler result aggregation strategies  | 📋      | Custom combiners for multi-handler event results |
| Notification vs Command clarification  | 📋      | Explicit abstractions for one-way notifications  |

## Event Publishing & Orchestration

| Item                            | Status | Goal                                                  |
| ------------------------------- | ------ | ----------------------------------------------------- |
| Sequential orchestrator         | ✅      | Execute handlers in order                             |
| Concurrent orchestrator         | ✅      | Execute handlers in parallel                          |
| Custom orchestration strategies | ⭐      | Priority-based, conditional, throttled execution      |
| Event filtering                 | 📋      | Handler selection based on runtime criteria           |
| Event versioning                | 📋      | Support multiple versions of same event type          |
| Distributed event bus           | 📋      | Integration with message brokers (RabbitMQ, Azure SB) |
| Event replay                    | 🤔      | Reprocess events from outbox/event store              |

## Request Handling Enhancements

| Feature                      | Status | Purpose                                                |
| ---------------------------- | ------ | ------------------------------------------------------ |
| Request/response with Result | ✅      | Functional error handling                              |
| Request without response     | ✅      | Command pattern support                                |
| Streaming requests           | ⭐      | IAsyncEnumerable support for large datasets            |
| Request validation behavior  | ⭐      | Built-in validation pipeline behavior                  |
| Request caching behavior     | 📋      | Automatic response caching with invalidation           |
| Request deduplication        | 📋      | Prevent duplicate request processing                   |
| Request timeout behavior     | 📋      | Automatic timeout enforcement                          |
| Multi-handler requests       | 🤔      | Support multiple handlers for single request (scatter) |

## Pipeline Behaviors

| Feature                         | Status | Purpose                                            |
| ------------------------------- | ------ | -------------------------------------------------- |
| Request pipeline behaviors      | ✅      | Cross-cutting concerns for requests                |
| Event pipeline behaviors        | ✅      | Cross-cutting concerns for events                  |
| Simple logging behavior         | ✅      | Built-in logging example                           |
| Typed behaviors                 | ⭐      | Behaviors specific to request/event types          |
| Conditional behavior execution  | ⭐      | Execute behaviors based on context or metadata     |
| Behavior ordering               | 📋      | Explicit control over behavior execution order     |
| Behavior composition            | 📋      | Combine multiple behaviors into reusable pipelines |
| Performance monitoring behavior | 📋      | Track execution time and resource usage            |
| Exception handling behavior     | 📋      | Centralized exception mapping to Result errors     |

## Outbox Pattern & Reliability

| Feature                    | Status | Purpose                                        |
| -------------------------- | ------ | ---------------------------------------------- |
| In-memory outbox storage   | ✅      | Development and testing                        |
| Publish modes (Now/Outbox) | ✅      | Control event delivery timing                  |
| CommitAsync for batch send | ✅      | Process all pending events                     |
| Persistent outbox storage  | ⭐      | EF Core, Dapper implementations                |
| Outbox retry policies      | ⭐      | Exponential backoff, max attempts              |
| Dead-letter queue          | ⭐      | Failed event handling                          |
| Outbox batch processing    | 📋      | Process multiple events efficiently            |
| Outbox cleanup             | 📋      | Remove processed events after retention period |
| Outbox monitoring          | 📋      | Track pending, failed, processed event counts  |
| Idempotency support        | 📋      | Prevent duplicate event processing             |

## Source Generation & DI

| Feature                             | Status | Purpose                                        |
| ----------------------------------- | ------ | ---------------------------------------------- |
| RequestHandler attribute            | ✅      | Mark classes for auto-registration             |
| EventHandler attribute              | ✅      | Mark classes for auto-registration             |
| IRegisterGroup interface generation | ✅      | Type-safe DI registration                      |
| Handler discovery                   | ✅      | Automatic handler detection                    |
| Scoped vs singleton detection       | 📋      | Infer or specify handler lifetimes             |
| Multiple assemblies support         | 📋      | Scan handlers across multiple projects         |
| Conditional registration            | 🤔      | Register handlers based on build configuration |

## Performance & Scalability

| Feature                    | Status | Purpose                                        |
| -------------------------- | ------ | ---------------------------------------------- |
| ValueTask usage            | ✅      | Reduce allocations for synchronous completions |
| Handler result caching     | 📋      | Cache expensive request results                |
| Request batching           | 📋      | Group similar requests for efficiency          |
| Streaming support          | ⭐      | Handle large datasets with IAsyncEnumerable    |
| Memory pooling             | 📋      | Reuse buffers for event/request processing     |
| Parallel event dispatching | ✅      | Concurrent orchestrator                        |
| Rate limiting behavior     | 📋      | Throttle request/event processing              |
| Backpressure handling      | 🤔      | Manage event queue overflow                    |

## Observability & Diagnostics

| Feature                         | Status | Purpose                                     |
| ------------------------------- | ------ | ------------------------------------------- |
| Context correlation ID          | ✅      | Track requests across boundaries            |
| OpenTelemetry integration       | ⭐      | Distributed tracing and metrics             |
| Request/event logging behavior  | ✅      | Simple logging example included             |
| Performance metrics             | 📋      | Track handler execution time, success rates |
| Handler execution visualization | 📋      | Debug complex pipelines and behaviors       |
| Diagnostic source integration   | 📋      | .NET diagnostic infrastructure support      |
| Health checks                   | 📋      | Monitor mediator and handler health         |
| Debugger display attributes     | 📋      | Rich debugging experience                   |

## Resilience & Error Recovery

| Feature                     | Status | Purpose                                           |
| --------------------------- | ------ | ------------------------------------------------- |
| Result-based error handling | ✅      | Functional error propagation                      |
| Retry behavior              | ⭐      | Automatic retry with configurable policies        |
| Circuit breaker behavior    | ⭐      | Prevent cascading failures                        |
| Fallback behavior           | 📋      | Provide default responses on failure              |
| Timeout behavior            | 📋      | Enforce maximum execution time                    |
| Bulkhead isolation          | 📋      | Isolate handler failures                          |
| Compensating actions        | 🤔      | Saga pattern support for distributed transactions |

## Advanced Patterns

| Feature                  | Status | Purpose                                    |
| ------------------------ | ------ | ------------------------------------------ |
| Request transformation   | 📋      | Map one request type to another            |
| Request aggregation      | 📋      | Combine multiple requests into one         |
| Event sourcing support   | 🤔      | Store events as source of truth            |
| CQRS helpers             | 📋      | Separate command and query infrastructures |
| Saga orchestration       | 🤔      | Long-running business processes            |
| Request/event versioning | 📋      | Handle multiple versions of messages       |
| Content-based routing    | 🤔      | Route to handlers based on message content |

## Testing & Developer Experience

| Feature                       | Status | Purpose                                      |
| ----------------------------- | ------ | -------------------------------------------- |
| In-memory testing utilities   | 📋      | Test handlers without infrastructure         |
| Mock mediator                 | 📋      | Test components using mediator               |
| Behavior testing helpers      | 📋      | Test pipeline behaviors in isolation         |
| Request/event builders        | 📋      | Fluent API for test data creation            |
| Analyzer: missing handler     | ⭐      | Warn when no handler registered              |
| Analyzer: multiple handlers   | 📋      | Warn about unexpected multiple registrations |
| Analyzer: async void handlers | ⭐      | Prevent common async mistakes                |
| Code snippets                 | 📋      | VS/Rider snippets for common patterns        |
| Migration guides              | 📋      | From MediatR and other libraries             |

## Integration & Interop

| Feature                      | Status | Purpose                                  |
| ---------------------------- | ------ | ---------------------------------------- |
| ASP.NET Core integration     | ⭐      | Endpoint filters, minimal APIs           |
| Native AOT compatibility     | ✅      | Optimized for ahead-of-time compilation  |
| Dependency injection support | ✅      | Microsoft.Extensions.DependencyInjection |
| Other DI containers          | 📋      | Autofac, Simple Injector support         |
| gRPC integration             | 📋      | Use mediator in gRPC services            |
| SignalR integration          | 📋      | Publish events to connected clients      |
| Azure Functions support      | 📋      | Use mediator in serverless functions     |
| Message broker adapters      | 📋      | Publish events to external systems       |

## Design Tenets
- Result-based error handling for explicit success/failure
- Native AOT compatibility for optimal performance
- Source generation for zero-reflection handler registration
- Pipeline behaviors for cross-cutting concerns
- Flexible event orchestration strategies
- Outbox pattern for reliable event delivery
- Context propagation for correlation and tracing
- Composability and extensibility at every level

## Open Questions (Tracking)
1. Should we support multiple handlers per request (scatter-gather pattern) or enforce single handler?
2. What's the best approach for handler lifetime management in source generation vs manual registration?
3. Should pipeline behaviors have explicit ordering or rely on registration order?
4. How deep should we integrate with observability frameworks (OpenTelemetry, Application Insights)?
5. Should we provide built-in validation behavior or leave it to users/separate packages?
6. Outbox pattern: should we support multiple storage backends out-of-the-box or provide interfaces only?
7. How to handle versioning of events/requests in a breaking-change scenario?

## Contributing
Issues / PRs welcome. When proposing a new feature, outline:
- Category (Request Handling / Event Publishing / Pipeline / Orchestration / Integration)
- Use case and benefits
- Breaking changes or compatibility concerns
- Performance implications
- Integration with existing features
- Testing strategy

## Comparison with MediatR
UnambitiousFx.Mediator differentiates itself through:
- **Result-based APIs**: First-class Result<T> support for functional error handling
- **Native AOT**: Optimized for ahead-of-time compilation with source generation
- **Event orchestration**: Built-in concurrent and sequential strategies
- **Outbox pattern**: First-class support for reliable event delivery
- **Modern C#**: Leverages latest C# features (ValueTask, readonly structs, etc.)

Migration guide from MediatR: Coming soon 📋

---
_Last updated: {{ site.time | date: '%Y-%m-%d' }}_
