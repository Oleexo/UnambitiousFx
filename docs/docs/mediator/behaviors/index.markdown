---
layout: default
title: Behaviors
parent: Mediator
nav_order: 40
has_children: true
---

# Behaviors

Pipeline behaviors let you add cross‑cutting concerns (logging, validation, caching, retries, metrics, etc.) around request and event handling. They wrap your handlers and run in registration order, keeping handler code focused on business logic.

- Request pipeline behaviors implement `IRequestPipelineBehavior` (or typed variants) and run when you call `ISender.SendAsync(...)`.
- Event pipeline behaviors implement `IEventPipelineBehavior` and run when you `Publish` events.

Behavior order: the first registered behavior is the outermost wrapper; the last one runs closest to the handler.

AOT-friendly: behaviors are regular DI services with public constructors; avoid reflection-heavy code in runtime paths.

## Basic usage (requests)

Implement `IRequestPipelineBehavior` to wrap request handling. You can intercept both non‑response and response requests.

```csharp
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Mediator.Abstractions;

public sealed class TimingBehavior : IRequestPipelineBehavior
{
    private readonly ILogger<TimingBehavior> _logger;
    public TimingBehavior(ILogger<TimingBehavior> logger) => _logger = logger;

    // For requests without a response
    public async ValueTask<Result> HandleAsync<TRequest>(
        TRequest request,
        RequestHandlerDelegate next,
        CancellationToken cancellationToken = default)
        where TRequest : IRequest
    {
        var sw = Stopwatch.StartNew();
        var result = await next();
        sw.Stop();
        _logger.LogInformation("Handled {Request} in {ElapsedMs} ms (Ok={Ok})",
            typeof(TRequest).Name, sw.ElapsedMilliseconds, result.IsSuccess);
        return result;
    }

    // For requests with a response
    public async ValueTask<Result<TResponse>> HandleAsync<TRequest, TResponse>(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken = default)
        where TRequest : IRequest<TResponse>
        where TResponse : notnull
    {
        var sw = Stopwatch.StartNew();
        var result = await next();
        sw.Stop();
        _logger.LogInformation("Handled {Request} in {ElapsedMs} ms (Ok={Ok})",
            typeof(TRequest).Name, sw.ElapsedMilliseconds, result.IsSuccess);
        return result;
    }
}
```

Register it:

```csharp
services.AddMediator(cfg =>
{
    cfg.RegisterRequestPipelineBehavior<TimingBehavior>();
});
```

## Basic usage (events)

Implement `IEventPipelineBehavior` to wrap event handling.

```csharp
using System.Linq;
using Microsoft.Extensions.Logging;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Mediator.Abstractions;

public sealed class EventLoggingBehavior : IEventPipelineBehavior
{
    private readonly ILogger<EventLoggingBehavior> _logger;
    public EventLoggingBehavior(ILogger<EventLoggingBehavior> logger) => _logger = logger;

    public async ValueTask<Result> HandleAsync<TEvent>(
        TEvent @event,
        EventHandlerDelegate next,
        CancellationToken cancellationToken = default)
        where TEvent : IEvent
    {
        _logger.LogInformation("Publishing {Event}", typeof(TEvent).Name);
        var result = await next();
        if (!result.IsSuccess)
        {
            _logger.LogWarning("Event {Event} failed: {Errors}", typeof(TEvent).Name, string.Join(", ", result.Errors.Select(e => e.Message)));
        }
        return result;
    }
}
```

Register it:

```csharp
services.AddMediator(cfg =>
{
    cfg.RegisterEventPipelineBehavior<EventLoggingBehavior>();
});
```

## Typed and conditional behaviors

You can scope behaviors to specific requests and/or add them conditionally.

- Typed (specific request without response):

```csharp
cfg.RegisterRequestPipelineBehavior<MyAuditBehavior, CreateInvoiceCommand>();
```

- Typed (specific request with response):

```csharp
cfg.RegisterRequestPipelineBehavior<MyMetricsBehavior, GetInvoiceQuery, Invoice>();
```

- Conditional (untyped, based on the current request object):

```csharp
cfg.RegisterConditionalRequestPipelineBehavior<MyDebugBehavior>(o => o is CreateInvoiceCommand);
```

- Conditional typed (predicate on request instance):

```csharp
cfg.RegisterConditionalRequestPipelineBehavior<MyAuditBehavior, CreateInvoiceCommand>(r => r.RequiresAudit);
```

## Built-in validation

A ready-to-use `RequestValidationBehavior<TRequest, TResponse>` runs all `IRequestValidator<TRequest>` implementations and short‑circuits on validation failure. See the dedicated page for details and examples.

## Learn more
- Request pipeline behavior: ./request-pipeline-behavior.html
- Event pipeline behavior: ./event-pipeline-behavior.html
- Request validation: ./validator.html
- Basics — Register mediator: ../basics/register-mediator.html
- Basics — Send a request: ../basics/send-request.html
- Basics — Publish an event: ../basics/publish-event.html
- Glossary: ../glossary.html