---
layout: default
title: Register Mediator
parent: Basics
nav_order: 5
---

# Registering Mediator into Dependency Injection

UnambitiousFx.Mediator is designed to work seamlessly with the .NET dependency injection system. This page explains how to register the mediator and its components in your application.

## Basic Registration

The simplest way to register the mediator is to call the `AddMediator` extension method on your `IServiceCollection`:

```csharp
using Microsoft.Extensions.DependencyInjection;
using UnambitiousFx.Mediator;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddApplicationServices(this IServiceCollection services) {
        services.AddMediator(config => {
            // Configuration goes here
        });
        
        return services;
    }
}
```

This registers the core mediator services:
- `ISender` - For sending requests
- `IPublisher` - For publishing events
- `IContextFactory` - For creating contexts
- Other internal services needed by the mediator

## Registering Handlers Manually

You can register handlers manually using the configuration object:

```csharp
services.AddMediator(config => {
    // Register request handlers
    config.RegisterRequestHandler<GetTodoByIdQueryHandler, GetTodoByIdQuery, Todo>();
    config.RegisterRequestHandler<CreateTodoCommandHandler, CreateTodoCommand, Guid>();
    config.RegisterRequestHandler<UpdateTodoCommandHandler, UpdateTodoCommand>();
    config.RegisterRequestHandler<DeleteTodoCommandHandler, DeleteTodoCommand>();
    
    // Register event handlers
    config.RegisterEventHandler<TodoCreatedHandler, TodoCreated>();
    config.RegisterEventHandler<TodoUpdatedHandler, TodoUpdated>();
    config.RegisterEventHandler<TodoDeletedHandler, TodoDeleted>();
});
```

## Registering Pipeline Behaviors

Pipeline behaviors are registered using the configuration object:

```csharp
services.AddMediator(config => {
    // Register request pipeline behaviors
    config.RegisterRequestPipelineBehavior<LoggingBehavior>();
    config.RegisterRequestPipelineBehavior<ValidationBehavior>();
    config.RegisterRequestPipelineBehavior<CachingBehavior>();
    
    // Register event pipeline behaviors
    config.RegisterEventPipelineBehavior<EventLoggingBehavior>();
    config.RegisterEventPipelineBehavior<EventValidationBehavior>();
});
```

Remember that the order of registration matters. Behaviors are executed in the order they are registered, with the first behavior being the outermost in the pipeline.

## Using Register Groups

For better organization, you can create register groups that encapsulate related registrations:

```csharp
public sealed class TodoRegisterGroup : IRegisterGroup {
    public void Register(IDependencyInjectionBuilder builder) {
        // Register request handlers
        builder.RegisterRequestHandler<GetTodoByIdQueryHandler, GetTodoByIdQuery, Todo>();
        builder.RegisterRequestHandler<CreateTodoCommandHandler, CreateTodoCommand, Guid>();
        builder.RegisterRequestHandler<UpdateTodoCommandHandler, UpdateTodoCommand>();
        builder.RegisterRequestHandler<DeleteTodoCommandHandler, DeleteTodoCommand>();
        
        // Register event handlers
        builder.RegisterEventHandler<TodoCreatedHandler, TodoCreated>();
        builder.RegisterEventHandler<TodoUpdatedHandler, TodoUpdated>();
        builder.RegisterEventHandler<TodoDeletedHandler, TodoDeleted>();
    }
}

public sealed class PipelineBehaviorRegisterGroup : IRegisterGroup {
    public void Register(IDependencyInjectionBuilder builder) {
        // Register request pipeline behaviors
        builder.RegisterRequestPipelineBehavior<LoggingBehavior>();
        builder.RegisterRequestPipelineBehavior<ValidationBehavior>();
        
        // Register event pipeline behaviors
        builder.RegisterEventPipelineBehavior<EventLoggingBehavior>();
        builder.RegisterEventPipelineBehavior<EventValidationBehavior>();
    }
}
```

Then register these groups:

```csharp
services.AddMediator(config => {
    config.RegisterGroup<TodoRegisterGroup>();
    config.RegisterGroup<PipelineBehaviorRegisterGroup>();
});
```

## Scanning Assemblies

UnambitiousFx.Mediator can automatically scan assemblies for handlers and register them:

```csharp
services.AddMediator(config => {
    // Scan the assembly containing the TodoQuery class
    config.ScanAssembly(typeof(TodoQuery).Assembly);
    
    // Or scan multiple assemblies
    config.ScanAssemblies(typeof(TodoQuery).Assembly, typeof(ProductQuery).Assembly);
});
```

This will find and register all:
- Request handlers (classes implementing `IRequestHandler<TRequest, TResponse>` or `IRequestHandler<TRequest>`)
- Event handlers (classes implementing `IEventHandler<TEvent>`)
- Register groups (classes implementing `IRegisterGroup`)

## Customizing Service Lifetime

By default, all handlers and behaviors are registered with a scoped lifetime. You can customize this:

```csharp
services.AddMediator(config => {
    // Register with a specific lifetime
    config.RegisterRequestHandler<GetTodoByIdQueryHandler, GetTodoByIdQuery, Todo>(ServiceLifetime.Transient);
    config.RegisterEventHandler<TodoCreatedHandler, TodoCreated>(ServiceLifetime.Singleton);
    config.RegisterRequestPipelineBehavior<LoggingBehavior>(ServiceLifetime.Singleton);
});
```

## Complete Registration Example

Here's a complete example showing how to register the mediator in a typical ASP.NET Core application:

```csharp
using Microsoft.Extensions.DependencyInjection;
using UnambitiousFx.Mediator;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddApplicationServices(this IServiceCollection services) {
        // Register other services
        services.AddScoped<ITodoRepository, TodoRepository>();
        services.AddSingleton<ICurrentUserService, CurrentUserService>();
        services.AddMemoryCache();
        
        // Register mediator
        services.AddMediator(config => {
            // Register pipeline behaviors
            config.RegisterRequestPipelineBehavior<LoggingBehavior>();
            config.RegisterRequestPipelineBehavior<ValidationBehavior>();
            config.RegisterEventPipelineBehavior<EventLoggingBehavior>();
            
            // Scan assemblies for handlers and register groups
            config.ScanAssembly(typeof(TodoQuery).Assembly);
            
            // Manually register specific handlers if needed
            config.RegisterRequestHandler<SpecialQueryHandler, SpecialQuery, SpecialResult>();
        });
        
        return services;
    }
}
```

And in your `Program.cs` or `Startup.cs`:

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddApplicationServices();

var app = builder.Build();

// Configure the HTTP request pipeline
// ...

app.Run();
```

## Using the Mediator

Once registered, you can inject and use the mediator in your classes:

```csharp
public class TodoController : ControllerBase {
    private readonly ISender _sender;
    private readonly IPublisher _publisher;
    private readonly IContextFactory _contextFactory;

    public TodoController(
        ISender sender,
        IPublisher publisher,
        IContextFactory contextFactory) {
        _sender = sender;
        _publisher = publisher;
        _contextFactory = contextFactory;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTodo(Guid id) {
        var result = await _sender.SendAsync<GetTodoByIdQuery, Todo>(
            new GetTodoByIdQuery { Id = id });

        return result.Match(
            todo => Ok(todo),
            error => NotFound(error.Message));
    }

    [HttpPost]
    public async Task<IActionResult> CreateTodo(CreateTodoRequest request) {
        var result = await _sender.SendAsync<CreateTodoCommand, Guid>(
            new CreateTodoCommand { Name = request.Name });

        return result.Match(
            id => CreatedAtAction(nameof(GetTodo), new { id }, null),
            error => BadRequest(error.Message));
    }

    [HttpPost("publish-event")]
    public async Task<IActionResult> PublishEvent(PublishEventRequest request) {
        var context = _contextFactory.Create();
        
        await _publisher.PublishAsync(
            context,
            new CustomEvent { Message = request.Message });

        return Ok();
    }
}
```

## Best Practices

1. **Organize by feature**: Group related handlers and register them together, either using register groups or by organizing them in feature-specific assemblies.
2. **Register behaviors first**: Register pipeline behaviors before handlers to ensure they are applied to all handlers.
3. **Consider service lifetimes**: Use appropriate service lifetimes for your handlers and behaviors. Most handlers should be scoped, but stateless behaviors can be singletons.
4. **Use assembly scanning**: For larger applications, use assembly scanning to automatically register handlers rather than registering them manually.
5. **Separate configuration**: Keep mediator registration in a separate extension method to keep your startup code clean and organized.