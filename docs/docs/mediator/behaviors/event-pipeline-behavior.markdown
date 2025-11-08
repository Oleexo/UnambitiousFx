---
layout: default
title: Event Pipeline Behavior
parent: Behaviors
nav_order: 4
---

# Creating an EventPipelineBehavior

Event pipeline behaviors in UnambitiousFx.Mediator allow you to add cross-cutting concerns to your event processing pipeline. This is similar to request pipeline behaviors but specifically for events.

## Understanding the Event Pipeline

When you publish an event through the mediator, it passes through a pipeline of behaviors before reaching the event handlers. Each behavior in the pipeline can:

1. Execute code before the event is handled
2. Execute code after the event is handled
3. Modify the event before it reaches the handlers
4. Short-circuit the pipeline and prevent handlers from being called

This provides a powerful way to implement cross-cutting concerns for event processing, such as logging, validation, and error handling.

## Implementing an EventPipelineBehavior

To create an event pipeline behavior, implement the `IEventPipelineBehavior` interface:

```csharp
using UnambitiousFx.Core;
using UnambitiousFx.Mediator.Abstractions;
using Microsoft.Extensions.Logging;

public sealed class EventLoggingBehavior : IEventPipelineBehavior {
    private readonly ILogger<EventLoggingBehavior> _logger;

    public EventLoggingBehavior(ILogger<EventLoggingBehavior> logger) {
        _logger = logger;
    }

    public async ValueTask<Result> HandleAsync<TEvent>(
        IContext context,
        TEvent @event,
        EventHandlerDelegate next,
        CancellationToken cancellationToken = default)
        where TEvent : IEvent {
        
        var eventName = typeof(TEvent).Name;
        _logger.LogInformation("Handling event: {EventName}", eventName);
        
        var result = await next();
        
        if (!result.Ok(out var error)) {
            _logger.LogWarning("Event {EventName} handling failed: {ErrorMessage}", eventName, error.Message);
        } else {
            _logger.LogInformation("Event {EventName} handled successfully", eventName);
        }
        
        return result;
    }
}
```

## Common Event Pipeline Behavior Scenarios

### Event Validation Behavior

A validation behavior can check if an event is valid before it reaches the handlers:

```csharp
using FluentValidation;
using UnambitiousFx.Core;
using UnambitiousFx.Mediator.Abstractions;

public sealed class EventValidationBehavior : IEventPipelineBehavior {
    private readonly IServiceProvider _serviceProvider;

    public EventValidationBehavior(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
    }

    public async ValueTask<Result> HandleAsync<TEvent>(
        IContext context,
        TEvent @event,
        EventHandlerDelegate next,
        CancellationToken cancellationToken = default)
        where TEvent : IEvent {
        
        // Try to resolve a validator for this event type
        var validator = _serviceProvider.GetService<IValidator<TEvent>>();
        
        if (validator != null) {
            var validationResult = await validator.ValidateAsync(@event, cancellationToken);
            
            if (!validationResult.IsValid) {
                // Return a failure result if validation fails
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return Result.Failure($"Event validation failed: {errors}");
            }
        }
        
        // Continue with the pipeline if validation passes or no validator exists
        return await next();
    }
}
```

### Event Enrichment Behavior

An enrichment behavior can add additional information to events before they are handled:

```csharp
using UnambitiousFx.Core;
using UnambitiousFx.Mediator.Abstractions;

public sealed class EventEnrichmentBehavior : IEventPipelineBehavior {
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTime _dateTime;

    public EventEnrichmentBehavior(
        ICurrentUserService currentUserService,
        IDateTime dateTime) {
        _currentUserService = currentUserService;
        _dateTime = dateTime;
    }

    public ValueTask<Result> HandleAsync<TEvent>(
        IContext context,
        TEvent @event,
        EventHandlerDelegate next,
        CancellationToken cancellationToken = default)
        where TEvent : IEvent {
        
        // Enrich the event with additional information if it implements IEnrichableEvent
        if (@event is IEnrichableEvent enrichableEvent) {
            enrichableEvent.UserId = _currentUserService.UserId;
            enrichableEvent.Timestamp = _dateTime.Now;
        }
        
        // Continue with the pipeline
        return next();
    }
}

// Interface to mark events that can be enriched
public interface IEnrichableEvent : IEvent {
    string UserId { get; set; }
    DateTime Timestamp { get; set; }
}
```

### Event Persistence Behavior

A persistence behavior can save events to an event store for event sourcing or auditing:

```csharp
using UnambitiousFx.Core;
using UnambitiousFx.Mediator.Abstractions;

public sealed class EventPersistenceBehavior : IEventPipelineBehavior {
    private readonly IEventStore _eventStore;

    public EventPersistenceBehavior(IEventStore eventStore) {
        _eventStore = eventStore;
    }

    public async ValueTask<Result> HandleAsync<TEvent>(
        IContext context,
        TEvent @event,
        EventHandlerDelegate next,
        CancellationToken cancellationToken = default)
        where TEvent : IEvent {
        
        // Save the event to the event store
        await _eventStore.SaveEventAsync(@event, cancellationToken);
        
        // Continue with the pipeline
        return await next();
    }
}

// Simple interface for an event store
public interface IEventStore {
    Task SaveEventAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : IEvent;
}
```

## Registering Event Pipeline Behaviors

To register an event pipeline behavior, add it to the service collection when configuring the mediator:

```csharp
using Microsoft.Extensions.DependencyInjection;
using UnambitiousFx.Mediator;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddApplicationServices(this IServiceCollection services) {
        services.AddMediator(config => {
            // Register event pipeline behaviors in the order they should execute
            config.RegisterEventPipelineBehavior<EventLoggingBehavior>();
            config.RegisterEventPipelineBehavior<EventValidationBehavior>();
            config.RegisterEventPipelineBehavior<EventEnrichmentBehavior>();
            config.RegisterEventPipelineBehavior<EventPersistenceBehavior>();
            
            // Register other mediator components
            // ...
        });
        
        return services;
    }
}
```

## Pipeline Behavior Execution Order

Event pipeline behaviors are executed in the order they are registered. The first behavior registered will be the outermost in the pipeline, and the last behavior registered will be the innermost (closest to the handlers).

For example, with the registration above, the execution flow would be:

1. EventLoggingBehavior (before)
2. EventValidationBehavior (before)
3. EventEnrichmentBehavior (before)
4. EventPersistenceBehavior (before)
5. Event Handlers
6. EventPersistenceBehavior (after)
7. EventEnrichmentBehavior (after)
8. EventValidationBehavior (after)
9. EventLoggingBehavior (after)

## Combining Request and Event Behaviors

In some cases, you might want to apply the same cross-cutting concerns to both requests and events. You can create a class that implements both `IRequestPipelineBehavior` and `IEventPipelineBehavior`:

```csharp
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using UnambitiousFx.Core;
using UnambitiousFx.Mediator.Abstractions;

public sealed class LoggingBehavior : IRequestPipelineBehavior, IEventPipelineBehavior {
    private readonly ILogger<LoggingBehavior> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior> logger) {
        _logger = logger;
    }

    // Request handling (without response)
    public async ValueTask<Result> HandleAsync<TRequest>(
        IContext context,
        TRequest request,
        RequestHandlerDelegate next,
        CancellationToken cancellationToken = default)
        where TRequest : IRequest {
        
        var stopwatch = Stopwatch.StartNew();
        var requestName = typeof(TRequest).Name;
        
        _logger.LogInformation("Handling request: {RequestName}", requestName);
        
        var result = await next();
        
        stopwatch.Stop();
        
        if (!result.Ok(out var error)) {
            _logger.LogWarning("Request {RequestName} failed in {ElapsedMilliseconds}ms: {ErrorMessage}", 
                requestName, stopwatch.ElapsedMilliseconds, error.Message);
        } else {
            _logger.LogInformation("Request {RequestName} completed in {ElapsedMilliseconds}ms", 
                requestName, stopwatch.ElapsedMilliseconds);
        }
        
        return result;
    }

    // Request handling (with response)
    public async ValueTask<Result<TResponse>> HandleAsync<TRequest, TResponse>(
        IContext context,
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken = default)
        where TRequest : IRequest<TResponse>
        where TResponse : notnull {
        
        var stopwatch = Stopwatch.StartNew();
        var requestName = typeof(TRequest).Name;
        
        _logger.LogInformation("Handling request: {RequestName}", requestName);
        
        var result = await next();
        
        stopwatch.Stop();
        
        if (!result.Ok(out _, out var error)) {
            _logger.LogWarning("Request {RequestName} failed in {ElapsedMilliseconds}ms: {ErrorMessage}", 
                requestName, stopwatch.ElapsedMilliseconds, error.Message);
        } else {
            _logger.LogInformation("Request {RequestName} completed in {ElapsedMilliseconds}ms", 
                requestName, stopwatch.ElapsedMilliseconds);
        }
        
        return result;
    }

    // Event handling
    public async ValueTask<Result> HandleAsync<TEvent>(
        IContext context,
        TEvent @event,
        EventHandlerDelegate next,
        CancellationToken cancellationToken = default)
        where TEvent : IEvent {
        
        var stopwatch = Stopwatch.StartNew();
        var eventName = typeof(TEvent).Name;
        
        _logger.LogInformation("Handling event: {EventName}", eventName);
        
        var result = await next();
        
        stopwatch.Stop();
        
        if (!result.Ok(out var error)) {
            _logger.LogWarning("Event {EventName} failed in {ElapsedMilliseconds}ms: {ErrorMessage}", 
                eventName, stopwatch.ElapsedMilliseconds, error.Message);
        } else {
            _logger.LogInformation("Event {EventName} completed in {ElapsedMilliseconds}ms", 
                eventName, stopwatch.ElapsedMilliseconds);
        }
        
        return result;
    }
}
```

Then register it as both a request and event pipeline behavior:

```csharp
services.AddMediator(config => {
    var loggingBehavior = new LoggingBehavior(loggerFactory.CreateLogger<LoggingBehavior>());
    
    config.RegisterRequestPipelineBehavior<LoggingBehavior>();
    config.RegisterEventPipelineBehavior<LoggingBehavior>();
    
    // Register other mediator components
    // ...
});
```

## Best Practices

1. **Keep behaviors focused**: Each behavior should handle a single cross-cutting concern.
2. **Consider performance**: Be mindful of the performance impact of your behaviors, especially for high-throughput applications.
3. **Order matters**: Register behaviors in the order they should execute, with the most critical ones first.
4. **Error handling**: Behaviors should handle exceptions gracefully and return appropriate Result objects.
5. **Avoid state**: Pipeline behaviors should be stateless to avoid unexpected behavior in concurrent scenarios.
6. **Reuse code**: Consider implementing both IRequestPipelineBehavior and IEventPipelineBehavior in the same class for behaviors that apply to both requests and events.