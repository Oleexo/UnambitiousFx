---
layout: default
title: Publish an Event
parent: Basics
nav_order: 2
---

# Publish an Event Using Mediator

Events are a powerful way to decouple components in your application. UnambitiousFx.Mediator provides a simple and efficient way to publish and handle events.

## Event Types

In UnambitiousFx.Mediator, events are represented by classes or records that implement the `IEvent` interface:

```csharp
using UnambitiousFx.Mediator.Abstractions;

public sealed record TodoCreated : IEvent {
    public required Todo Todo { get; init; }
}

public sealed record TodoUpdated : IEvent {
    public required Todo Todo { get; init; }
}

public sealed record TodoDeleted : IEvent {
    public required Guid TodoId { get; init; }
}
```

## Creating Event Handlers

For each event, you can create one or more handlers that implement the `IEventHandler<TEvent>` interface:

```csharp
using UnambitiousFx.Core;
using UnambitiousFx.Mediator.Abstractions;
using Microsoft.Extensions.Logging;

public sealed class TodoCreatedHandler : IEventHandler<TodoCreated> {
    private readonly ILogger<TodoCreatedHandler> _logger;

    public TodoCreatedHandler(ILogger<TodoCreatedHandler> logger) {
        _logger = logger;
    }

    public ValueTask<Result> HandleAsync(
        IContext context,
        TodoCreated @event,
        CancellationToken cancellationToken = default) {
        
        _logger.LogInformation("Todo created: {TodoId}", @event.Todo.Id);
        
        // Perform additional actions when a todo is created
        
        return ValueTask.FromResult(Result.Success());
    }
}
```

You can have multiple handlers for the same event, and they will all be executed when the event is published:

```csharp
public sealed class TodoCreatedNotificationHandler : IEventHandler<TodoCreated> {
    private readonly INotificationService _notificationService;

    public TodoCreatedNotificationHandler(INotificationService notificationService) {
        _notificationService = notificationService;
    }

    public async ValueTask<Result> HandleAsync(
        IContext context,
        TodoCreated @event,
        CancellationToken cancellationToken = default) {
        
        await _notificationService.SendNotificationAsync(
            "New Todo Created", 
            $"A new todo '{@event.Todo.Name}' was created.",
            cancellationToken);
        
        return Result.Success();
    }
}
```

## Publishing Events

There are two ways to publish events in UnambitiousFx.Mediator:

### 1. Publishing Events from Request Handlers

The most common way to publish events is from within a request handler, using the `IContext` provided to the handler:

```csharp
using UnambitiousFx.Core;
using UnambitiousFx.Mediator;
using UnambitiousFx.Mediator.Abstractions;

[RequestHandler<CreateTodoCommand, Guid>]
public sealed class CreateTodoCommandHandler : IRequestHandler<CreateTodoCommand, Guid> {
    private readonly ITodoRepository _todoRepository;

    public CreateTodoCommandHandler(ITodoRepository todoRepository) {
        _todoRepository = todoRepository;
    }

    public async ValueTask<Result<Guid>> HandleAsync(
        IContext context,
        CreateTodoCommand request,
        CancellationToken cancellationToken = default) {
        
        var todo = new Todo {
            Id = Guid.NewGuid(),
            Name = request.Name
        };

        await _todoRepository.CreateAsync(todo, cancellationToken);

        // Publish an event to notify other components
        await context.PublishAsync(new TodoCreated {
            Todo = todo
        }, cancellationToken);

        return Result<Guid>.Success(todo.Id);
    }
}
```

### 2. Publishing Events Directly

You can also publish events directly by injecting `IPublisher` into your class:

```csharp
using UnambitiousFx.Mediator.Abstractions;

public class TodoService {
    private readonly IPublisher _publisher;
    private readonly IContextFactory _contextFactory;

    public TodoService(
        IPublisher publisher,
        IContextFactory contextFactory) {
        _publisher = publisher;
        _contextFactory = contextFactory;
    }

    public async Task UpdateTodoAsync(Todo todo, CancellationToken cancellationToken = default) {
        // Update the todo in the database
        
        // Create a context for publishing the event
        var context = _contextFactory.Create();
        
        // Publish an event to notify other components
        await _publisher.PublishAsync(
            context,
            new TodoUpdated { Todo = todo },
            cancellationToken);
    }
}
```

## Event Handling Best Practices

1. **Keep events focused**: Each event should represent a single occurrence or state change.
2. **Use immutable events**: Define events as records to ensure they are immutable.
3. **Include all relevant data**: Events should contain all the data needed by handlers, to avoid additional database queries.
4. **Handle failures gracefully**: Use the `Result` type to handle and report errors in event handlers.
5. **Consider event ordering**: Be aware that event handlers are executed in a non-deterministic order.
6. **Keep handlers lightweight**: Event handlers should be quick to execute. For long-running operations, consider using a background service or queue.

## Event-Driven Architecture

UnambitiousFx.Mediator's event system can be used as a foundation for event-driven architecture:

- **Domain Events**: Represent something that happened in your domain model
- **Integration Events**: Used for communication between different bounded contexts or microservices
- **Event Sourcing**: Store and replay events to reconstruct the state of your application

By leveraging events effectively, you can build more maintainable and scalable applications with clear separation of concerns.