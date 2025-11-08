---
title: Roadmap
parent: Mediator
nav_order: 99
---

# Mediator Roadmap

This page summarizes the current and planned capabilities for the `Mediator` library. This roadmap focuses on
high-signal planning items and provides visibility into the evolution of the mediator pattern implementation.

## Legend

✅ Implemented · 🔄 In Progress · ⭐ High Priority (next 1–2 milestones)  · 📋 Planned · 🤔 Considering

## Event Publishing & Orchestration

| Item                            | Status | Goal                                                  |
|---------------------------------|--------|-------------------------------------------------------|
| Sequential orchestrator         | ✅      | Execute handlers in order                             |
| Concurrent orchestrator         | ✅      | Execute handlers in parallel                          |
| Custom orchestration strategies | 🤔     | Priority-based, conditional, throttled execution      |
| Event filtering                 | 🤔     | Handler selection based on runtime criteria           |
| Event versioning                | 🤔     | Support multiple versions of same event type          |
| Distributed event bus           | 📋     | Integration with message brokers (RabbitMQ, Azure SB) |
| Event replay                    | 🤔     | Reprocess events from outbox/event store              |

## Request Handling Enhancements

| Feature                      | Status | Purpose                                                                                |
|------------------------------|--------|----------------------------------------------------------------------------------------|
| Request/response with Result | ✅      | Functional error handling                                                              |
| Request without response     | ✅      | Command pattern support                                                                |
| Streaming requests           | ✅      | IAsyncEnumerable support for large datasets                                            |
| Request validation behavior  | ✅      | Built-in validation pipeline behavior                                                  |
| CQRS boundary enforcement    | ✅      | Prevent queries in command, commands in query, commands in command or queries in query |
| Request caching behavior     | 📋     | Automatic response caching with invalidation                                           |
| Request deduplication        | 📋     | Prevent duplicate request processing                                                   |
| Request timeout behavior     | 📋     | Automatic timeout enforcement                                                          |
| Multi-handler requests       | 🤔     | Support multiple handlers for single request (scatter)                                 |

## Pipeline Behaviors

| Feature                        | Status | Purpose                                        |
|--------------------------------|--------|------------------------------------------------|
| Request pipeline behaviors     | ✅      | Cross-cutting concerns for requests            |
| Event pipeline behaviors       | ✅      | Cross-cutting concerns for events              |
| Simple logging behavior        | ✅      | Built-in logging example                       |
| Typed behaviors                | ✅      | Behaviors specific to request/event types      |
| Conditional behavior execution | ✅      | Execute behaviors based on context or metadata |
| Behavior ordering              | 🤔     | Explicit control over behavior execution order |
| Exception handling behavior    | 🤔     | Centralized exception mapping to Result errors |

## Outbox Pattern & Reliability

| Feature                    | Status | Purpose                                        |
|----------------------------|--------|------------------------------------------------|
| In-memory outbox storage   | ✅      | Development and testing                        |
| Publish modes (Now/Outbox) | ✅      | Control event delivery timing                  |
| CommitAsync for batch send | ✅      | Process all pending events                     |
| Persistent outbox storage  | ⭐      | EF Core, Dapper implementations                |
| Outbox retry policies      | ✅      | Exponential backoff, max attempts (in-memory)  |
| Dead-letter queue          | ✅      | Failed event handling (in-memory)              |
| Outbox batch processing    | ✅      | Process multiple events efficiently            |
| Outbox cleanup             | 🤔     | Remove processed events after retention period |
| Outbox monitoring          | 🤔     | Track pending, failed, processed event counts  |
| Idempotency support        | 🤔     | Prevent duplicate event processing             |

## Source Generation & DI

| Feature                             | Status | Purpose                                        |
|-------------------------------------|--------|------------------------------------------------|
| RequestHandler attribute            | ✅      | Mark classes for auto-registration             |
| EventHandler attribute              | ✅      | Mark classes for auto-registration             |
| IRegisterGroup interface generation | ✅      | Type-safe DI registration                      |
| Handler discovery                   | ✅      | Automatic handler detection                    |
| Scoped vs singleton detection       | 🤔     | Infer or specify handler lifetimes             |
| Multiple assemblies support         | 🤔     | Scan handlers across multiple projects         |
| Conditional registration            | 🤔     | Register handlers based on build configuration |

## Performance & Scalability

| Feature                    | Status | Purpose                                        |
|----------------------------|--------|------------------------------------------------|
| ValueTask usage            | ✅      | Reduce allocations for synchronous completions |
| Handler result caching     | 🤔     | Cache expensive request results                |
| Request batching           | 🤔     | Group similar requests for efficiency          |
| Streaming support          | ✅      | Handle large datasets with IAsyncEnumerable    |
| Memory pooling             | 🤔     | Reuse buffers for event/request processing     |
| Parallel event dispatching | ✅      | Concurrent orchestrator                        |
| Rate limiting behavior     | 🤔     | Throttle request/event processing              |
| Backpressure handling      | 🤔     | Manage event queue overflow                    |

## Observability & Diagnostics

| Feature                         | Status | Purpose                                     |
|---------------------------------|--------|---------------------------------------------|
| Context correlation ID          | ✅      | Track requests across boundaries            |
| OpenTelemetry integration       | ⭐      | Distributed tracing and metrics             |
| Request/event logging behavior  | ✅      | Simple logging example included             |
| Performance metrics             | 🤔     | Track handler execution time, success rates |
| Handler execution visualization | 🤔     | Debug complex pipelines and behaviors       |
| Diagnostic source integration   | 🤔     | .NET diagnostic infrastructure support      |
| Health checks                   | 🤔     | Monitor mediator and handler health         |
| Debugger display attributes     | 🤔     | Rich debugging experience                   |

## Integration & Interop

| Feature                      | Status | Purpose                                  |
|------------------------------|--------|------------------------------------------|
| ASP.NET Core integration     | ⭐      | Endpoint filters, minimal APIs           |
| Native AOT compatibility     | ✅      | Optimized for ahead-of-time compilation  |
| Dependency injection support | ✅      | Microsoft.Extensions.DependencyInjection |
| Other DI containers          | 🤔     | Autofac, Simple Injector support         |
| gRPC integration             | 🤔     | Use mediator in gRPC services            |
| SignalR integration          | 🤔     | Publish events to connected clients      |
| Azure Functions support      | 🤔     | Use mediator in serverless functions     |
| Message broker adapters      | 🤔     | Publish events to external systems       |

## Design Tenets

- Result-based error handling for explicit success/failure
- Native AOT compatibility for optimal performance
- Source generation for zero-reflection handler registration
- Pipeline behaviors for cross-cutting concerns
- Flexible event orchestration strategies
- Outbox pattern for reliable event delivery
- Context propagation for correlation and tracing
- Composability and extensibility at every level

---
_Last updated: {{ site.time | date: '%Y-%m-%d' }}_
