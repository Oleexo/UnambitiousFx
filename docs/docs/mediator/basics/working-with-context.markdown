---
layout: default
title: Working with Context
parent: Basics
nav_order: 4
---

# Working with Context (Basics)

UnambitiousFx.Mediator provides a lightweight ambient context you use to:
- Publish events consistently during a request/operation
- Carry correlation and metadata across handlers/behaviors
- Batch and commit events when using outbox publishing

This page covers `IContext`, `IContextAccessor`, and `IContextFactory`, when to use each, and common pitfalls.

## What is a Context?

A context (`IContext`) represents the current logical operation. It holds:
- `CorrelationId` — a stable id to correlate logs and events
- `OccuredAt` — timestamp for the operation
- `Metadata` — key/value bag for behaviors, logging enrichment, or domain hints
- Event publishing methods: `PublishEventAsync(...)` and `CommitEventsAsync()`

Source: `UnambitiousFx.Mediator.Abstractions.IContext`

```csharp
public interface IContext {
    string CorrelationId { get; }
    DateTimeOffset OccuredAt { get; }
    IReadOnlyDictionary<string, object> Metadata { get; }

    void SetMetadata(string key, object value);
    bool RemoveMetadata(string key);
    bool TryGetMetadata<T>(string key, out T? value);
    T? GetMetadata<T>(string key);

    ValueTask<Result> PublishEventAsync<TEvent>(TEvent @event, CancellationToken ct = default)
        where TEvent : class, IEvent;

    ValueTask<Result> PublishEventAsync<TEvent>(TEvent @event, PublishMode mode, CancellationToken ct = default)
        where TEvent : class, IEvent;

    ValueTask<Result> CommitEventsAsync(CancellationToken ct = default);
}
```

## How contexts are registered

When you call `services.AddMediator(...)`, the DI container registers:
- `IContext` (scoped) — resolved from the current `IContextAccessor.Context`
- `IContextAccessor` (scoped) — a single slot holding the active context for the scope
- `IContextFactory` (scoped) — creates new `IContext` instances

This means all components that ask for `IContext` within the same scope share the same underlying context instance. You can replace the active context by setting `IContextAccessor.Context` (see background jobs example).

## Publishing events from handlers (recommended)

Inside request handlers and pipeline behaviors, inject `IContext` and publish events through it. This keeps correlation and metadata consistent and lets you switch publishing mode globally without changing calling code.

Example: publish an event from a command handler

```csharp
using UnambitiousFx.Core.Results;
using UnambitiousFx.Mediator;
using UnambitiousFx.Mediator.Abstractions;

[RequestHandler<CreateTodoCommand, Guid>]
public sealed class CreateTodoCommandHandler : IRequestHandler<CreateTodoCommand, Guid>
{
    private readonly ITodoRepository _todoRepository;
    private readonly IContext _context;

    public CreateTodoCommandHandler(ITodoRepository todoRepository, IContext context)
    {
        _todoRepository = todoRepository;
        _context = context;
    }

    public async ValueTask<Result<Guid>> HandleAsync(
        CreateTodoCommand request,
        CancellationToken cancellationToken = default)
    {
        var todo = new Todo { Id = Guid.NewGuid(), Name = request.Name };
        await _todoRepository.CreateAsync(todo, cancellationToken);

        // Publish domain event via the current context
        await _context.PublishEventAsync(new TodoCreated { Todo = todo }, cancellationToken);

        return Result.Success(todo.Id);
    }
}
```

Notes:
- Prefer `IContext` over injecting `IPublisher` directly in handlers. You keep correlation/metadata and can batch/commit if using an outbox.
- If using `PublishMode.Outbox` (globally or per call), remember to call `CommitEventsAsync()` at the appropriate boundary if you need explicit commit semantics.

## Publishing outside the request flow

For code running outside request handling (timers, background services, hosted jobs, integration listeners), create a fresh context with `IContextFactory`. Optionally set it as the current context so components that depend on `IContext` keep working.

Example: create a context for a background job

```csharp
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UnambitiousFx.Mediator.Abstractions;

public sealed class RebuildSearchIndexJob : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    public RebuildSearchIndexJob(IServiceScopeFactory scopeFactory) => _scopeFactory = scopeFactory;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // Run job on a new scope
            using var scope = _scopeFactory.CreateScope();
            var contextFactory = scope.ServiceProvider.GetRequiredService<IContextFactory>();
            var contextAccessor = scope.ServiceProvider.GetRequiredService<IContextAccessor>();

            // Create and activate a fresh context for this run
            var context = contextFactory.Create();
            contextAccessor.Context = context; // make it available to components that inject IContext

            // Publish an event (mode uses global default unless overridden)
            await context.PublishEventAsync(new SearchIndexRebuildRequested(), stoppingToken);

            // If you configured outbox publishing, flush pending events now
            await context.CommitEventsAsync(stoppingToken);

            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }
}
```

Tips:
- You can set explicit correlation: `context.SetMetadata("JobName", "RebuildSearchIndex")` or provide your own `CorrelationId` by creating an extension around `IContextFactory` if you need custom ids.
- If you only need to publish directly and don’t need ambient metadata, you can inject and use `IPublisher` instead of creating/activating a context. However, using a context keeps behavior consistent with handlers.

## Choosing a publish mode

`PublishMode` values:
- `Now` — dispatch handlers immediately
- `Outbox` — enqueue for later processing in an outbox storage
- `Default` — use mediator’s default (configured via `IMediatorConfig.SetDefaultPublishingMode(...)` or `PublisherOptions`)

You can pick per call: `await _context.PublishEventAsync(@event, PublishMode.Outbox, ct);`

With outbox mode, make sure to call `CommitEventsAsync()` at your boundary (end of request, after a unit of work, or at the end of a job iteration) to process pending events.

## Lifetimes and scoping

- `IContext`, `IContextAccessor`, and `IContextFactory` are registered as scoped services.
- A single active `IContext` instance (held by `IContextAccessor.Context`) is used per scope by default.
- Creating a new context with `IContextFactory.Create()` does not automatically replace the active context — set `IContextAccessor.Context` if you want other components to observe it.
- Use `IServiceScopeFactory.CreateScope()` to isolate contexts per background operation or per message handling loop.

## Common pitfalls

- Do not cache `IContext` or `IContextAccessor` in static fields or singletons. Always resolve them in a scope.
- Avoid sharing one `IContext` instance across concurrent operations; create one context per logical flow/scope.
- Only mutate metadata keys you own. Behaviors may rely on certain metadata (e.g., CQRS boundary enforcement).
- Don’t forget to call `CommitEventsAsync()` when using outbox mode and you expect dispatch to happen at the boundary.
- Prefer `IContext` inside handlers; use `IContextFactory` (and optionally `IContextAccessor`) outside handlers.

## See also
- Publish an event: ./publish-event.html
- Register mediator (DI): ./register-mediator.html
- Behaviors (cross‑cutting concerns): ../behaviors/index.html
- Glossary: ../glossary.html
