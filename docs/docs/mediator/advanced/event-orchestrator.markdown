---
layout: default
title: Event Orchestrator
parent: Advanced
nav_order: 2
---

# Event Orchestrator

Event orchestrators control how event handlers are executed when an event is published. They receive the resolved handlers for a given `TEvent` and decide how to run them (sequentially, concurrently, fail‑fast, bounded concurrency, etc.).

UnambitiousFx.Mediator ships with:
- `SequentialEventOrchestrator` (default): runs handlers one after another.
- `ConcurrentEventOrchestrator`: runs all handlers in parallel and aggregates results.

Both orchestrators are AOT‑friendly and allocation‑conscious; they do not use runtime reflection.

## How it plugs into publishing

When you call `IPublisher.PublishAsync(@event)`, the mediator gathers the handlers and behaviors, runs event pipeline behaviors, and then delegates the actual execution of handlers to the configured `IEventOrchestrator`.

Interface (simplified):

```csharp
public interface IEventOrchestrator
{
    ValueTask<Result> RunAsync<TEvent>(
        IEnumerable<IEventHandler<TEvent>> handlers,
        TEvent @event,
        CancellationToken cancellationToken = default)
        where TEvent : class, IEvent;
}
```

## Default: sequential orchestrator

This is the out‑of‑the‑box default. Handlers execute one after the other; the final result is the combination of all handler results.

```csharp
using UnambitiousFx.Mediator.Orchestrators;

services.AddMediator(cfg =>
{
    // Default is SequentialEventOrchestrator — you do not need to set it explicitly.
    // cfg.SetEventOrchestrator<SequentialEventOrchestrator>();
});
```

Behavior:
- If all handlers succeed, you get `Result.Success()`.
- Errors from handlers are aggregated via `Combine()`.

## Switch to concurrent orchestrator

Run all handlers in parallel; useful when handlers are independent and you want lower latency.

```csharp
using UnambitiousFx.Mediator.Orchestrators;

services.AddMediator(cfg =>
{
    cfg.SetEventOrchestrator<ConcurrentEventOrchestrator>();
});
```

Notes:
- Handlers must be thread‑safe with respect to the event data or external resources they touch.
- The combined result aggregates errors from all handlers.

## Example: publishing an event

```csharp
public sealed record InvoicePaid(Guid InvoiceId) : IEvent;

public sealed class SendReceiptEmailHandler : IEventHandler<InvoicePaid>
{
    public async ValueTask<Result> HandleAsync(InvoicePaid @event, CancellationToken ct = default)
    {
        // send email…
        await Task.Delay(10, ct);
        return Result.Success();
    }
}

public sealed class UpdateAnalyticsHandler : IEventHandler<InvoicePaid>
{
    public ValueTask<Result> HandleAsync(InvoicePaid @event, CancellationToken ct = default)
    {
        // update analytics…
        return ValueTask.FromResult(Result.Success());
    }
}
```

DI registration (handlers omitted for brevity):

```csharp
services.AddMediator(cfg =>
{
    cfg.RegisterEventHandler<SendReceiptEmailHandler, InvoicePaid>();
    cfg.RegisterEventHandler<UpdateAnalyticsHandler, InvoicePaid>();

    // Choose orchestrator strategy
    cfg.SetEventOrchestrator<SequentialEventOrchestrator>();
});
```

Publishing:

```csharp
public sealed class BillingService(IPublisher publisher)
{
    public ValueTask<Result> MarkAsPaidAsync(Guid invoiceId, CancellationToken ct)
        => publisher.PublishAsync(new InvoicePaid(invoiceId), ct);
}
```

## Custom orchestrator examples

You can implement your own `IEventOrchestrator` to fit bespoke requirements. Keep implementations AOT‑friendly (public constructor, no reflection on open generics) and mindful of exceptions (return `Result` instead of throwing for domain/control flow).

### 1) Fail‑fast sequential orchestrator

Stops on the first failure and returns that error immediately.

```csharp
using UnambitiousFx.Core.Results;
using UnambitiousFx.Mediator.Abstractions;
using UnambitiousFx.Mediator.Orchestrators;

public sealed class FailFastSequentialEventOrchestrator : IEventOrchestrator
{
    public async ValueTask<Result> RunAsync<TEvent>(
        IEnumerable<IEventHandler<TEvent>> handlers,
        TEvent @event,
        CancellationToken cancellationToken = default)
        where TEvent : class, IEvent
    {
        foreach (var handler in handlers)
        {
            var result = await handler.HandleAsync(@event, cancellationToken);
            if (result.IsFaulted)
                return result; // stop at first failure
        }
        return Result.Success();
    }
}
```

Register it:

```csharp
services.AddMediator(cfg =>
{
    cfg.SetEventOrchestrator<FailFastSequentialEventOrchestrator>();
});
```

### 2) Bounded‑concurrency orchestrator

Runs handlers with a maximum degree of parallelism. Useful when you have many handlers or I/O limits.

```csharp
using System.Threading;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions.Collections;
using UnambitiousFx.Mediator.Abstractions;
using UnambitiousFx.Mediator.Orchestrators;

public sealed class BoundedConcurrencyEventOrchestrator : IEventOrchestrator
{
    private readonly int _maxDegreeOfParallelism;
    public BoundedConcurrencyEventOrchestrator(int maxDegreeOfParallelism = 4)
        => _maxDegreeOfParallelism = Math.Max(1, maxDegreeOfParallelism);

    public async ValueTask<Result> RunAsync<TEvent>(
        IEnumerable<IEventHandler<TEvent>> handlers,
        TEvent @event,
        CancellationToken cancellationToken = default)
        where TEvent : class, IEvent
    {
        using var semaphore = new SemaphoreSlim(_maxDegreeOfParallelism);
        var tasks = handlers.Select(async handler =>
        {
            await semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                return await handler.HandleAsync(@event, cancellationToken);
            }
            finally
            {
                semaphore.Release();
            }
        }).ToList();

        var results = await Task.WhenAll(tasks);
        return results.Combine();
    }
}
```

Register it:

```csharp
services.AddMediator(cfg =>
{
    cfg.SetEventOrchestrator<BoundedConcurrencyEventOrchestrator>();
});
```

## Guidance and trade‑offs

- Sequential: deterministic order, simpler resource usage; longest latency if handlers are slow.
- Concurrent: best latency if handlers are independent; ensure downstream systems can handle parallelism.
- Fail‑fast: returns quickly on first error; remaining handlers do not run.
- Bounded concurrency: balance throughput vs. pressure on external systems.

Keep orchestrators small, with clear behavior and minimal allocations.

## See also
- Publish an event (basics): ../basics/publish-event.html
- Event pipeline behavior (wrap event handling): ../behaviors/event-pipeline-behavior.html
- Register mediator (DI): ../basics/register-mediator.html
- Advanced (overview): ./index.html
- Glossary: ../glossary.html
