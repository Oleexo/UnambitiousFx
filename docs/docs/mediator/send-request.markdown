---
layout: default
title: Send a Request
parent: Mediator
nav_order: 1
---

# Send a Request Using Mediator

Sending requests is one of the core functionalities of the mediator pattern. UnambitiousFx.Mediator provides a simple and intuitive API for sending requests and receiving responses.

## Request Types

UnambitiousFx.Mediator supports two types of requests:

1. **Requests with a response** - Implement `IRequest<TResponse>` where `TResponse` is the type of the response
2. **Requests without a response** - Implement `IRequest` (typically used for commands)

## Creating a Request

To create a request, define a class or record that implements either `IRequest<TResponse>` or `IRequest`:

```csharp
using UnambitiousFx.Mediator.Abstractions;

// Request with a response (typically a query)
public sealed record GetTodoByIdQuery : IRequest<Todo> {
    public required Guid Id { get; init; }
}

// Request without a response (typically a command)
public sealed record DeleteTodoCommand : IRequest {
    public required Guid Id { get; init; }
}
```

## Creating a Request Handler

For each request, you need to create a corresponding handler that implements either `IRequestHandler<TRequest, TResponse>` or `IRequestHandler<TRequest>`:

```csharp
using UnambitiousFx.Core;
using UnambitiousFx.Mediator;
using UnambitiousFx.Mediator.Abstractions;

// Handler for a request with a response
[RequestHandler<GetTodoByIdQuery, Todo>] // Optional: Used by Mediator.Generator
public sealed class GetTodoByIdQueryHandler : IRequestHandler<GetTodoByIdQuery, Todo> {
    private readonly ITodoRepository _todoRepository;

    public GetTodoByIdQueryHandler(ITodoRepository todoRepository) {
        _todoRepository = todoRepository;
    }

    public async ValueTask<Result<Todo>> HandleAsync(
        IContext context,
        GetTodoByIdQuery request,
        CancellationToken cancellationToken = default) {
        
        var todo = await _todoRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (todo == null) {
            return Result<Todo>.Failure("Todo not found");
        }
        
        return Result<Todo>.Success(todo);
    }
}

// Handler for a request without a response
[RequestHandler<DeleteTodoCommand>] // Optional: Used by Mediator.Generator
public sealed class DeleteTodoCommandHandler : IRequestHandler<DeleteTodoCommand> {
    private readonly ITodoRepository _todoRepository;

    public DeleteTodoCommandHandler(ITodoRepository todoRepository) {
        _todoRepository = todoRepository;
    }

    public async ValueTask<Result> HandleAsync(
        IContext context,
        DeleteTodoCommand request,
        CancellationToken cancellationToken = default) {
        
        var deleted = await _todoRepository.DeleteAsync(request.Id, cancellationToken);
        
        if (!deleted) {
            return Result.Failure("Todo not found");
        }
        
        return Result.Success();
    }
}
```

## Sending a Request

To send a request, inject `ISender` into your class and call one of the `SendAsync` methods:

```csharp
using UnambitiousFx.Mediator.Abstractions;

public class TodoController {
    private readonly ISender _sender;

    public TodoController(ISender sender) {
        _sender = sender;
    }

    public async Task<IActionResult> GetTodo(Guid id) {
        var result = await _sender.SendAsync<GetTodoByIdQuery, Todo>(
            new GetTodoByIdQuery { Id = id });

        return result.Match(
            todo => Ok(todo),
            error => NotFound(error.Message));
    }

    public async Task<IActionResult> DeleteTodo(Guid id) {
        var result = await _sender.SendAsync(
            new DeleteTodoCommand { Id = id });

        return result.Match(
            () => NoContent(),
            error => NotFound(error.Message));
    }
}
```

## Working with Results

UnambitiousFx.Mediator uses the `Result<T>` and `Result` types from UnambitiousFx.Core to handle success and failure cases. These types provide a functional approach to error handling:

```csharp
// Handling a Result<T>
var result = await _sender.SendAsync<GetTodoByIdQuery, Todo>(query);
result.Match(
    todo => {
        // Handle success case with the todo
        Console.WriteLine($"Found todo: {todo.Name}");
    },
    error => {
        // Handle error case
        Console.WriteLine($"Error: {error.Message}");
    });

// Handling a Result
var result = await _sender.SendAsync(command);
result.Match(
    () => {
        // Handle success case
        Console.WriteLine("Command executed successfully");
    },
    error => {
        // Handle error case
        Console.WriteLine($"Error: {error.Message}");
    });
```

## Best Practices

1. **Keep requests focused**: Each request should represent a single operation or query.
2. **Use records for requests**: Records provide immutability and value-based equality, which are desirable for requests.
3. **Return meaningful errors**: Use the `Result` type to provide clear error messages when operations fail.
4. **Consider using CQRS**: Separate commands (which modify state) from queries (which retrieve data) for better separation of concerns.