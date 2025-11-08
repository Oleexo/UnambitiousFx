---
layout: default
title: Mediator configuration reference (Advanced)
parent: Advanced
nav_order: 1
---

# Mediator configuration reference (Advanced)

This page documents the full `IMediatorConfig` surface and how it maps to DI registrations and runtime behavior. It also clarifies behavior ordering, lifetimes, and streaming‑specific registrations.

All APIs are AOT‑friendly: no runtime scanning or reflection. Registrations are compiled or explicit.

## Quick gist

```csharp
using Microsoft.Extensions.DependencyInjection;
using UnambitiousFx.Mediator;

services.AddMediator(cfg =>
{
    // Lifetime (applies to all registrations made through cfg and register groups)
    cfg.SetLifetime(ServiceLifetime.Scoped);

    // Request handlers (both arities)
    cfg.RegisterRequestHandler<CreateInvoiceHandler, CreateInvoiceCommand>();
    cfg.RegisterRequestHandler<GetInvoiceHandler, GetInvoiceQuery, Invoice>();

    // Request pipeline behaviors (untyped/typed/conditional)
    cfg.RegisterRequestPipelineBehavior<TimingBehavior>();
    cfg.RegisterRequestPipelineBehavior<AuditBehavior, CreateInvoiceCommand>();
    cfg.RegisterConditionalRequestPipelineBehavior<DebugBehavior>(o => o is CreateInvoiceCommand);

    // Event handlers and behaviors
    cfg.RegisterEventHandler<InvoicePaidEmailHandler, InvoicePaid>();
    cfg.RegisterEventPipelineBehavior<EventLoggingBehavior>();

    // Orchestrator strategy for events
    cfg.SetEventOrchestrator<UnambitiousFx.Mediator.Orchestrators.SequentialEventOrchestrator>();

    // Generated registrations (handlers, including streaming) via register group
    MediatorRegistrations.Register(cfg); // from Mediator.Generator (recommended)

    // Outbox & publishing defaults (optional)
    cfg.SetEventOutboxStorage<InMemoryEventOutboxStorage>();
    cfg.SetDefaultPublishingMode(PublishMode.Now);
    cfg.ConfigureOutbox(o => { o.MaxBatchSize = 100; });

    // CQRS guard and validators (optional)
    cfg.EnableCqrsBoundaryEnforcement();
    cfg.AddValidator<CreateInvoiceCommandValidator, CreateInvoiceCommand>();
});
```

## SetLifetime(ServiceLifetime)

- Default: `Scoped`.
- Applies to all registrations done via `IMediatorConfig` and via `AddRegisterGroup(IRegisterGroup)`.
- When to change:
  - `Singleton`: stateless behaviors or orchestrators that do not hold per‑request state; be mindful of injected dependencies also being singletons.
  - `Transient`: rarely needed; use for behaviors that must be recreated per call (usually not necessary — prefer `Scoped`).

```csharp
cfg.SetLifetime(ServiceLifetime.Singleton); // make mediator components singletons
```

## Handler registrations (requests and events)

### Requests without response

```csharp
cfg.RegisterRequestHandler<CreateInvoiceHandler, CreateInvoiceCommand>();
```
- `CreateInvoiceHandler : IRequestHandler<CreateInvoiceCommand>`
- Registers the concrete handler and a proxy handler that wraps it with request behaviors in registration order.

### Requests with response

```csharp
cfg.RegisterRequestHandler<GetInvoiceHandler, GetInvoiceQuery, Invoice>();
```
- `GetInvoiceHandler : IRequestHandler<GetInvoiceQuery, Invoice>`
- Response type `TResponse` must be non‑nullable.

### Event handlers

```csharp
cfg.RegisterEventHandler<SendReceiptEmailHandler, InvoicePaid>();
```
- `SendReceiptEmailHandler : IEventHandler<InvoicePaid>`
- Works with the configured event orchestrator and event pipeline behaviors.

## Request pipeline behaviors

Behaviors wrap request handlers. Ordering is the DI registration order: the first registered behavior is the outermost wrapper; the last runs nearest to the handler.

### Untyped (covers all requests)

```csharp
cfg.RegisterRequestPipelineBehavior<TimingBehavior>();
```
- `TimingBehavior : IRequestPipelineBehavior`
- Must implement both `HandleAsync<TRequest>(...)` and `HandleAsync<TRequest, TResponse>(...)` members if you need to intercept both arities.

### Typed (specific request type)

```csharp
cfg.RegisterRequestPipelineBehavior<MyAuditBehavior, CreateInvoiceCommand>();
cfg.RegisterRequestPipelineBehavior<MyMetricsBehavior, GetInvoiceQuery, Invoice>();
```
- Implements `IRequestPipelineBehavior<TRequest>` or `IRequestPipelineBehavior<TRequest, TResponse>`.

### Conditional

```csharp
// Untyped with a predicate on the runtime request object
a => cfg.RegisterConditionalRequestPipelineBehavior<MyDebugBehavior>(o => o is CreateInvoiceCommand);

// Typed with a predicate on the request instance
cfg.RegisterConditionalRequestPipelineBehavior<MyAuditBehavior, CreateInvoiceCommand>(r => r.RequiresAudit);

// Typed with response
a => cfg.RegisterConditionalRequestPipelineBehavior<MyMetricsBehavior, GetInvoiceQuery, Invoice>(q => q.IncludePerf);
```

Notes:
- Conditions are evaluated per request dispatch; if false, the behavior is skipped.
- Behaviors are resolved via DI; lifetime comes from `SetLifetime`.

## Stream handlers and stream behaviors

### Stream request handlers

`IMediatorConfig` does not expose a direct `RegisterStreamRequestHandler` method. Streaming handlers are typically registered via generated register groups (recommended), which call into the builder API under the hood.

- Preferred: Generator output

```csharp
// Generated by UnambitiousFx.Mediator.Generator in your project
MediatorRegistrations.Register(cfg); // includes stream handler registrations
```

- Manual: implement a custom `IRegisterGroup` that calls the builder

```csharp
using UnambitiousFx.Mediator.Abstractions;

public sealed class MyStreamingRegistrations : IRegisterGroup
{
    public void Register(IDependencyInjectionBuilder builder)
    {
        builder.RegisterStreamRequestHandler<SearchInvoicesQueryHandler, SearchInvoicesQuery, Invoice>();
    }
}

// then
cfg.AddRegisterGroup(new MyStreamingRegistrations());
```

### Stream pipeline behaviors

- Implement `IStreamRequestPipelineBehavior` and register in DI. Behaviors are applied in DI registration order around the streaming handler.

```csharp
using UnambitiousFx.Mediator.Abstractions;

public sealed class StreamTimingBehavior : IStreamRequestPipelineBehavior
{
    public async IAsyncEnumerable<Result<TItem>> HandleAsync<TRequest, TItem>(
        TRequest request,
        StreamRequestHandlerDelegate<TItem> next,
        [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct = default)
        where TRequest : IStreamRequest<TItem>
        where TItem : notnull
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();
        await foreach (var item in next())
            yield return item;
        sw.Stop();
        // log if needed
    }
}

// Register in DI (uses the lifetime from SetLifetime only for components registered via cfg; 
// stream behaviors are regular DI registrations)
services.AddScoped<IStreamRequestPipelineBehavior, StreamTimingBehavior>();
```

Notes:
- Currently the untyped `IStreamRequestPipelineBehavior` is the supported way to plug stream behaviors. They are discovered via `IEnumerable<IStreamRequestPipelineBehavior>`.
- Ordering: same as request behaviors — first registered is outermost.

## Event orchestrator

Choose how multiple handlers of the same event are executed. Default is sequential.

```csharp
// Default (sequential) is used if not set explicitly
cfg.SetEventOrchestrator<UnambitiousFx.Mediator.Orchestrators.SequentialEventOrchestrator>();

// Or switch to concurrent
cfg.SetEventOrchestrator<UnambitiousFx.Mediator.Orchestrators.ConcurrentEventOrchestrator>();
```

See the dedicated page for details: ./event-orchestrator.html

## Register groups (AddRegisterGroup)

Use register groups to bring in a bundle of registrations (often generated):

```csharp
public static class MediatorRegistrations
{
    public static void Register(IMediatorConfig cfg)
    {
        // Generated code typically calls cfg.AddRegisterGroup(new GeneratedGroup());
    }
}

cfg.AddRegisterGroup(new MyFeatureRegistrations());
```

- `IRegisterGroup.Register(IDependencyInjectionBuilder builder)` receives a builder to register request, event, and stream handlers.
- Changing `cfg.SetLifetime(...)` affects the lifetime used when the group is applied.

## Behavior order and DI lifetimes

- Ordering
  - Behaviors (request, event, stream) execute in the order they are registered.
  - “First in” is the outermost wrapper; the last registered runs closest to the handler.
- Lifetimes
  - Default is `Scoped`. Use `SetLifetime` to switch lifetimes for all mediator registrations performed through `cfg` and register groups.
  - Event orchestrator and outbox storage are registered as scoped services by default.
  - Stream behaviors are regular DI services — register them with the desired lifetimes directly on `IServiceCollection`.
- Guidance
  - Prefer `Scoped` for most behaviors and handlers.
  - `Singleton` behaviors must be stateless and depend only on singletons.
  - Avoid `Transient` unless you have a strong reason.

## Other configuration knobs (brief)

- `RegisterEventPipelineBehavior<T>()` — add cross‑cutting concerns around publishing.
- `SetEventOutboxStorage<T>()` — plug a custom outbox storage implementation.
- `SetDefaultPublishingMode(PublishMode)` — e.g., `PublishMode.Now` vs `PublishMode.Outbox`.
- `ConfigureOutbox(Action<OutboxOptions>)` — retry/dead‑letter/batching.
- `EnableCqrsBoundaryEnforcement(bool enable = true)` — enables a guard behavior that prevents sending commands from command handlers, etc.
- `AddValidator<TValidator, TRequest>()` — add request validators; pairs well with the built‑in `RequestValidationBehavior`.

## End‑to‑end example

```csharp
using Microsoft.Extensions.DependencyInjection;
using UnambitiousFx.Mediator;
using UnambitiousFx.Mediator.Abstractions;

var services = new ServiceCollection();

services.AddMediator(cfg =>
{
    cfg.SetLifetime(ServiceLifetime.Scoped);

    // Handlers
    cfg.RegisterRequestHandler<CreateInvoiceHandler, CreateInvoiceCommand>();
    cfg.RegisterRequestHandler<GetInvoiceHandler, GetInvoiceQuery, Invoice>();
    cfg.RegisterEventHandler<InvoicePaidEmailHandler, InvoicePaid>();

    // Behaviors (order matters)
    cfg.RegisterRequestPipelineBehavior<RequestLoggingBehavior>();
    cfg.RegisterRequestPipelineBehavior<RequestMetricsBehavior, GetInvoiceQuery, Invoice>();
    cfg.RegisterConditionalRequestPipelineBehavior<RequestDebugBehavior>(o => o is CreateInvoiceCommand);
    cfg.RegisterEventPipelineBehavior<EventLoggingBehavior>();

    // Orchestrator
    cfg.SetEventOrchestrator<UnambitiousFx.Mediator.Orchestrators.SequentialEventOrchestrator>();

    // Generated registrations (includes streaming handlers)
    MediatorRegistrations.Register(cfg);

    // Outbox & defaults
    cfg.SetDefaultPublishingMode(PublishMode.Now);
});

// Stream behavior registration (DI)
services.AddScoped<IStreamRequestPipelineBehavior, StreamTimingBehavior>();
```

## See also
- Behaviors (overview): ../behaviors/index.html
- Event orchestrator: ./event-orchestrator.html
- Mediator generator: ./mediator-generator.html
- Basics — Streaming requests: ../basics/streaming-requests.html
- Glossary: ../glossary.html
