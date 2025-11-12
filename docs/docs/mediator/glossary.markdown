---
layout: default
title: Glossary
parent: Mediator
nav_order: 98
---

# Glossary

A compact reference for the core concepts and types used in UnambitiousFx.Mediator. Terms link to the relevant pages for deeper explanations and examples.

- Mediator
  - The library component that routes messages (requests/events) to their handlers through a single abstraction. It helps decouple senders from receivers and enables cross‑cutting behaviors via pipelines. See: Registering Mediator (DI) and Send/Publish guides.

- Request
  - A message that represents an operation to perform.
  - Two forms:
    - `IRequest<TResponse>`: request expecting a response of type `TResponse`.
    - `IRequest`: request without a response (command‑style).
  - See: Send a request.

- Request Handler
  - A class implementing `IRequestHandler<TRequest, TResponse>` or `IRequestHandler<TRequest>` that contains the business logic for a request. Handlers return `Result<TResponse>` or `Result`.
  - Handlers are registered with DI and wrapped with proxy/pipelines by the mediator.
  - See: Send a request, Registering Mediator.

- Result / Result<T>
  - Outcome types from UnambitiousFx.Core.Results used throughout the mediator.
  - `Result<T>` models success with a value of `T` or failure with errors; `Result` is the non‑generic counterpart.

- ISender
  - The service used to send requests and receive `Result`/`Result<T>`. Obtain via DI and call `SendAsync(...)`.
  - See: Send a request.

- IEvent
  - A message that represents something that happened. Events are published and can have multiple handlers.
  - See: Publish an event.

- Event Handler
  - A class implementing `IEventHandler<TEvent>` that reacts to a published event and returns a `Result`.
  - Multiple event handlers may process the same event.
  - See: Publish an event.

- IPublisher
  - The service responsible for publishing events. Obtain via DI; most app code publishes via a context (see `IContext`).
  - See: Publish an event, Unified event dispatching.

- Distribution Mode
  - Determines how events are processed: LocalOnly (local handlers only), ExternalOnly (external transport only), or Hybrid (both local and external).
  - Configured via `[DistributedEvent]` attribute, routing filters, or default configuration.
  - See: Unified event dispatching.

- Dispatch Strategy
  - Controls when and how events are processed: Immediate (synchronous), Deferred (background processing), or Batched (accumulated processing).
  - Configured in `EventDispatcherOptions`.
  - See: Unified event dispatching.

- Event Routing Filter
  - Custom logic implementing `IEventRoutingFilter` that determines distribution mode for events based on runtime conditions (tenant, feature flags, etc.).
  - Filters are evaluated in order before falling back to message traits or defaults.
  - See: Unified event dispatching.

- Outbox Pattern
  - Reliability pattern that stores events in a transient store before dispatching to ensure at-least-once delivery with automatic retry and dead-letter handling.
  - Integrated into the unified event dispatching system.
  - See: Unified event dispatching.

- IContext / IContextFactory / IContextAccessor
  - Ambient context used by the mediator to carry ephemeral information and to publish events during request processing.
  - Typical usage: inject `IContext` (or access it via `IContextAccessor`) inside handlers or services; create a new context via `IContextFactory` when publishing outside request handling.
  - See: Publish an event.

- Pipeline Behavior (requests)
  - Cross‑cutting components that wrap request handling. Implement the untyped `IRequestPipelineBehavior` or typed variants and are executed in registration order.
  - Examples: logging, validation, caching.
  - See: Request pipeline behaviors, Request validation.

- RequestValidationBehavior
  - A built‑in request pipeline behavior that runs all `IRequestValidator<TRequest>` implementations and aggregates their results. If validation fails, the request is short‑circuited with a failure `Result`.
  - See: Request validation.

- IRequestValidator<TRequest>
  - Defines validation for a specific request. Returns `Result` indicating success or failure of validation.
  - See: Request validation.

- Pipeline Behavior (events)
  - Cross‑cutting components that wrap event handling; implement `IEventPipelineBehavior` and run in registration order when events are published.
  - See: Event pipeline behaviors.

- Stream Request / Stream Handler
  - `IStreamRequest<TItem>` represents a request that produces a stream (sequence) of items of type `TItem`.
  - `IStreamRequestHandler<TRequest, TItem>` processes a stream request and yields items; can be wrapped by `IStreamRequestPipelineBehavior`.
  - See: Registering Mediator (streaming registration APIs).

- IMediatorConfig / Registration
  - The configuration interface used inside `services.AddMediator(config => { ... })` to register request handlers, event handlers, and pipeline behaviors (including conditional/typed forms) and register groups.
  - See: Registering Mediator.

- Register Group (`IRegisterGroup`)
  - A class that groups multiple mediator registrations (handlers/behaviors) into a single reusable unit.
  - See: Registering Mediator (Register Groups).

- Mediator.Generator
  - A Roslyn source generator that can help with registrations and diagnostics (e.g., via attributes such as `[RequestHandler<...>]`).
  - The generator targets `netstandard2.0` for broad compatibility with consumers on `net9.0`.
  - See: Mediator generator.

- AOT / Trimming
  - The mediator is designed to be AOT‑friendly. Avoid reflection‑heavy patterns on runtime paths; registrations use DI and proxies that are compatible with trimming.
  - See repository guidelines for AOT best practices.

- Behavior Order
  - Behaviors are executed in the order they are registered. The first registered behavior is the outermost wrapper around the handler.

- DI Lifetime
  - Handlers and behaviors are registered with a default scoped lifetime unless configured otherwise by the mediator’s internal registration helpers.

## See also
- Send a request: ./basics/send-request.html
- Publish an event: ./basics/publish-event.html
- Request pipeline behaviors: ./behaviors/request-pipeline-behavior.html
- Event pipeline behaviors: ./behaviors/event-pipeline-behavior.html
- Request validation: ./behaviors/validator.html
- Register mediator into DI (all options): ./basics/register-mediator.html
- Source generator to simplify registrations: ./advanced/mediator-generator.html
