---
layout: default
title: Mediator Generator
parent: Mediator
nav_order: 6
---

# Using Mediator.Generator for Dependency Injection

UnambitiousFx.Mediator.Generator is a source generator that simplifies the registration of mediator components in your application. It automatically generates the necessary code to register handlers, reducing boilerplate and ensuring that all handlers are properly registered.

## Why Use a Source Generator?

Manual registration of mediator components can become tedious and error-prone as your application grows. The Mediator.Generator offers several advantages:

1. **Reduced boilerplate**: No need to manually register each handler
2. **Compile-time safety**: Registration errors are caught at compile time
3. **Performance**: No runtime reflection for assembly scanning
4. **Native AOT compatibility**: Works seamlessly with Native AOT compilation

## Getting Started

### 1. Add the NuGet Package

First, add the UnambitiousFx.Mediator.Generator package to your project:

```xml
<ItemGroup>
  <PackageReference Include="UnambitiousFx.Mediator.Generator" Version="1.0.0" />
</ItemGroup>
```

### 2. Mark Your Handlers with Attributes

Decorate your request and event handlers with the appropriate attributes:

```csharp
using UnambitiousFx.Mediator;
using UnambitiousFx.Mediator.Abstractions;

// For request handlers with a response
[RequestHandler<GetTodoByIdQuery, Todo>]
public sealed class GetTodoByIdQueryHandler : IRequestHandler<GetTodoByIdQuery, Todo> {
    // Implementation...
}

// For request handlers without a response
[RequestHandler<DeleteTodoCommand>]
public sealed class DeleteTodoCommandHandler : IRequestHandler<DeleteTodoCommand> {
    // Implementation...
}

// For event handlers
[EventHandler<TodoCreated>]
public sealed class TodoCreatedHandler : IEventHandler<TodoCreated> {
    // Implementation...
}
```

### 3. Register the Generated Code

The generator creates a class called `MediatorRegistrations` in your project's namespace. Use it in your service registration:

```csharp
using Microsoft.Extensions.DependencyInjection;
using UnambitiousFx.Mediator;
using YourNamespace; // Namespace where MediatorRegistrations is generated

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddApplicationServices(this IServiceCollection services) {
        services.AddMediator(config => {
            // Register pipeline behaviors
            config.RegisterRequestPipelineBehavior<LoggingBehavior>();
            config.RegisterEventPipelineBehavior<EventLoggingBehavior>();
            
            // Register generated handlers
            MediatorRegistrations.Register(config);
        });
        
        return services;
    }
}
```

## How It Works

The source generator analyzes your code at compile time and:

1. Finds all classes decorated with `[RequestHandler]` or `[EventHandler]` attributes
2. Generates a static class with a `Register` method
3. Adds registration code for each handler it finds

The generated code looks something like this:

```csharp
using UnambitiousFx.Mediator.Abstractions;

namespace YourNamespace {
    public static class MediatorRegistrations {
        public static void Register(IDependencyInjectionBuilder builder) {
            // Request handlers
            builder.RegisterRequestHandler<GetTodoByIdQueryHandler, GetTodoByIdQuery, Todo>();
            builder.RegisterRequestHandler<CreateTodoCommandHandler, CreateTodoCommand, Guid>();
            builder.RegisterRequestHandler<UpdateTodoCommandHandler, UpdateTodoCommand>();
            builder.RegisterRequestHandler<DeleteTodoCommandHandler, DeleteTodoCommand>();
            
            // Event handlers
            builder.RegisterEventHandler<TodoCreatedHandler, TodoCreated>();
            builder.RegisterEventHandler<TodoUpdatedHandler, TodoUpdated>();
            builder.RegisterEventHandler<TodoDeletedHandler, TodoDeleted>();
        }
    }
}
```

## Customizing Registration

### Specifying Service Lifetime

You can specify the service lifetime for a handler by setting the `Lifetime` property on the attribute:

```csharp
using UnambitiousFx.Mediator;
using UnambitiousFx.Mediator.Abstractions;
using Microsoft.Extensions.DependencyInjection;

[RequestHandler<CachedQuery, CachedResult>(Lifetime = ServiceLifetime.Singleton)]
public sealed class CachedQueryHandler : IRequestHandler<CachedQuery, CachedResult> {
    // This handler will be registered as a singleton
}
```

### Excluding Handlers from Generation

If you want to exclude a handler from automatic registration, you can use the `[ExcludeFromGeneration]` attribute:

```csharp
using UnambitiousFx.Mediator;
using UnambitiousFx.Mediator.Abstractions;

[ExcludeFromGeneration]
[RequestHandler<SpecialQuery, SpecialResult>]
public sealed class SpecialQueryHandler : IRequestHandler<SpecialQuery, SpecialResult> {
    // This handler won't be included in the generated registrations
}
```

You might want to do this if you need to register the handler manually with special configuration.

## Using with Multiple Projects

If your application is split across multiple projects, the generator will create a `MediatorRegistrations` class in each project. You can register them all:

```csharp
services.AddMediator(config => {
    // Register handlers from different projects
    YourNamespace.Core.MediatorRegistrations.Register(config);
    YourNamespace.Infrastructure.MediatorRegistrations.Register(config);
    YourNamespace.Web.MediatorRegistrations.Register(config);
});
```

## Native AOT Compatibility

UnambitiousFx.Mediator.Generator is fully compatible with Native AOT compilation. Since it uses source generation instead of runtime reflection, it works seamlessly in AOT scenarios.

When using Native AOT:

1. Assembly scanning is not available (due to reflection limitations)
2. All types must be known at compile time
3. Dynamic type resolution is restricted

The source generator addresses these limitations by generating all the necessary registration code at compile time.

## Example: Complete Setup with Native AOT

Here's a complete example of setting up a Native AOT compatible application with UnambitiousFx.Mediator.Generator:

```csharp
// Program.cs
using Microsoft.Extensions.DependencyInjection;
using UnambitiousFx.Mediator;
using YourNamespace;

var builder = WebApplication.CreateSlimBuilder(args);

// Register services
builder.Services.AddMediator(config => {
    // Register behaviors
    config.RegisterRequestPipelineBehavior<LoggingBehavior>();
    
    // Register generated handlers
    MediatorRegistrations.Register(config);
});

var app = builder.Build();

// Configure endpoints
app.MapGet("/todos/{id}", async (Guid id, ISender sender) => {
    var result = await sender.SendAsync<GetTodoByIdQuery, Todo>(
        new GetTodoByIdQuery { Id = id });
        
    return result.Match(
        todo => Results.Ok(todo),
        error => Results.NotFound(error.Message));
});

app.Run();
```

## Best Practices

1. **Use attributes consistently**: Apply the appropriate attributes to all your handlers to ensure they're properly registered.
2. **Check generated code**: Occasionally inspect the generated code to ensure it's registering handlers as expected.
3. **Combine with manual registration when needed**: For complex scenarios, you can combine generated registration with manual registration.
4. **Keep handlers in the same assembly as their requests/events**: This makes it easier to maintain and understand the code.
5. **Use explicit service lifetimes for special cases**: Most handlers should use the default scoped lifetime, but specify different lifetimes when needed.

## Troubleshooting

### Handler Not Being Registered

If a handler isn't being registered:

1. Ensure it has the correct attribute (`[RequestHandler]` or `[EventHandler]`)
2. Check that it's not marked with `[ExcludeFromGeneration]`
3. Verify that the handler implements the correct interface
4. Make sure the generator package is properly referenced

### Viewing Generated Code

To view the generated code:

1. Build your project
2. Look in the `obj/Debug/net8.0/generated` directory (adjust for your target framework)
3. Find the file named `MediatorRegistrations.g.cs`

This can be helpful for debugging registration issues.