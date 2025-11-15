---
layout: default
title: Mediator
nav_order: 3
permalink: /mediator/
has_children: true
---

# Mediator

UnambitiousFx.Mediator is a lightweight, AOT-friendly mediator for .NET. It helps you decouple application logic by sending requests to handlers through a single abstraction, and it supports events and pipelines for cross‑cutting concerns.

## Quick start

This is the minimum you need: a request, its handler, and registration in DI.

### 1) Define a request
```csharp
using UnambitiousFx.Mediator.Abstractions;

public sealed record GetTodoByIdQuery(Guid Id) : IRequest<Todo>;
```

### 2) Implement the handler
```csharp
using UnambitiousFx.Core.Results;
using UnambitiousFx.Mediator.Abstractions;

public sealed class GetTodoByIdQueryHandler : IRequestHandler<GetTodoByIdQuery, Todo>
{
    private readonly ITodoRepository _repo;

    public GetTodoByIdQueryHandler(ITodoRepository repo) => _repo = repo;

    public async ValueTask<Result<Todo>> HandleAsync(
        GetTodoByIdQuery request,
        CancellationToken cancellationToken = default)
    {
        var todo = await _repo.GetByIdAsync(request.Id, cancellationToken);
        return todo is not null
            ? Result.Success(todo)
            : Result.Failure<Todo>("Todo not found");
    }
}
```

### 3) Register mediator and the handler in DI
```csharp
using Microsoft.Extensions.DependencyInjection;
using UnambitiousFx.Mediator;

var services = new ServiceCollection();

services.AddMediator(config =>
{
    // Register the request handler
    config.RegisterRequestHandler<GetTodoByIdQueryHandler, GetTodoByIdQuery, Todo>();
});
```

At this point you can inject `ISender` anywhere and call `SendAsync(new GetTodoByIdQuery(id))` to get a `Result<Todo>`.

## Further reading
- Basics — Send a request: ./basics/send-request.html
- Basics — Publish an event: ./basics/publish-event.html
- Behaviors — Request pipeline behaviors: ./behaviors/request-pipeline-behavior.html
- Behaviors — Event pipeline behaviors: ./behaviors/event-pipeline-behavior.html
- Behaviors — Request validation: ./behaviors/validator.html
- Basics — Register mediator into DI (all options): ./basics/register-mediator.html
- Advanced — Source generator to simplify registrations: ./advanced/mediator-generator.html
- Roadmap: ./roadmap.html
