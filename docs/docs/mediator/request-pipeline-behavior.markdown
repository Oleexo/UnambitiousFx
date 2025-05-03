---
layout: default
title: Request Pipeline Behavior
parent: Mediator
nav_order: 3
---

# Creating a RequestPipelineBehavior

Pipeline behaviors in UnambitiousFx.Mediator allow you to add cross-cutting concerns to your request processing pipeline. This is useful for implementing features like logging, validation, caching, and error handling without cluttering your request handlers.

## Understanding the Request Pipeline

When you send a request through the mediator, it passes through a pipeline of behaviors before reaching the request handler. Each behavior in the pipeline can:

1. Execute code before the request is handled
2. Execute code after the request is handled
3. Modify the request before it reaches the handler
4. Modify the response after it leaves the handler
5. Short-circuit the pipeline and return a response without calling the handler

This provides a powerful way to implement cross-cutting concerns in a clean, reusable manner.

## Implementing a RequestPipelineBehavior

To create a request pipeline behavior, implement the `IRequestPipelineBehavior` interface:

```csharp
using UnambitiousFx.Core;
using UnambitiousFx.Mediator.Abstractions;

public sealed class LoggingBehavior : IRequestPipelineBehavior {
    private readonly ILogger<LoggingBehavior> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior> logger) {
        _logger = logger;
    }

    // For requests without a response
    public async ValueTask<Result> HandleAsync<TRequest>(
        IContext context,
        TRequest request,
        RequestHandlerDelegate next,
        CancellationToken cancellationToken = default)
        where TRequest : IRequest {
        
        var requestName = typeof(TRequest).Name;
        _logger.LogInformation("Handling request: {RequestName}", requestName);
        
        var result = await next();
        
        if (!result.Ok(out var error)) {
            _logger.LogWarning("Request {RequestName} failed: {ErrorMessage}", requestName, error.Message);
        } else {
            _logger.LogInformation("Request {RequestName} completed successfully", requestName);
        }
        
        return result;
    }

    // For requests with a response
    public async ValueTask<Result<TResponse>> HandleAsync<TRequest, TResponse>(
        IContext context,
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken = default)
        where TRequest : IRequest<TResponse>
        where TResponse : notnull {
        
        var requestName = typeof(TRequest).Name;
        _logger.LogInformation("Handling request: {RequestName}", requestName);
        
        var result = await next();
        
        if (!result.Ok(out _, out var error)) {
            _logger.LogWarning("Request {RequestName} failed: {ErrorMessage}", requestName, error.Message);
        } else {
            _logger.LogInformation("Request {RequestName} completed successfully", requestName);
        }
        
        return result;
    }
}
```

## Common Pipeline Behavior Scenarios

### Validation Behavior

A validation behavior can check if a request is valid before it reaches the handler:

```csharp
using FluentValidation;
using UnambitiousFx.Core;
using UnambitiousFx.Mediator.Abstractions;

public sealed class ValidationBehavior : IRequestPipelineBehavior {
    private readonly IServiceProvider _serviceProvider;

    public ValidationBehavior(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
    }

    public async ValueTask<Result> HandleAsync<TRequest>(
        IContext context,
        TRequest request,
        RequestHandlerDelegate next,
        CancellationToken cancellationToken = default)
        where TRequest : IRequest {
        
        // Try to resolve a validator for this request type
        var validator = _serviceProvider.GetService<IValidator<TRequest>>();
        
        if (validator != null) {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            
            if (!validationResult.IsValid) {
                // Return a failure result if validation fails
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return Result.Failure($"Validation failed: {errors}");
            }
        }
        
        // Continue with the pipeline if validation passes or no validator exists
        return await next();
    }

    public async ValueTask<Result<TResponse>> HandleAsync<TRequest, TResponse>(
        IContext context,
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken = default)
        where TRequest : IRequest<TResponse>
        where TResponse : notnull {
        
        // Try to resolve a validator for this request type
        var validator = _serviceProvider.GetService<IValidator<TRequest>>();
        
        if (validator != null) {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            
            if (!validationResult.IsValid) {
                // Return a failure result if validation fails
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return Result<TResponse>.Failure($"Validation failed: {errors}");
            }
        }
        
        // Continue with the pipeline if validation passes or no validator exists
        return await next();
    }
}
```

### Caching Behavior

A caching behavior can cache responses to avoid redundant processing:

```csharp
using Microsoft.Extensions.Caching.Memory;
using UnambitiousFx.Core;
using UnambitiousFx.Mediator.Abstractions;

public sealed class CachingBehavior : IRequestPipelineBehavior {
    private readonly IMemoryCache _cache;

    public CachingBehavior(IMemoryCache cache) {
        _cache = cache;
    }

    public ValueTask<Result> HandleAsync<TRequest>(
        IContext context,
        TRequest request,
        RequestHandlerDelegate next,
        CancellationToken cancellationToken = default)
        where TRequest : IRequest {
        
        // We typically don't cache commands (requests without responses)
        return next();
    }

    public async ValueTask<Result<TResponse>> HandleAsync<TRequest, TResponse>(
        IContext context,
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken = default)
        where TRequest : IRequest<TResponse>
        where TResponse : notnull {
        
        // Only cache if the request implements ICacheableRequest
        if (request is ICacheableRequest cacheableRequest) {
            var cacheKey = cacheableRequest.CacheKey;
            
            // Try to get from cache
            if (_cache.TryGetValue(cacheKey, out Result<TResponse> cachedResult)) {
                return cachedResult;
            }
            
            // Execute the request
            var result = await next();
            
            // Cache the result if successful
            if (result.Ok(out _, out _)) {
                _cache.Set(cacheKey, result, TimeSpan.FromMinutes(cacheableRequest.CacheTimeInMinutes));
            }
            
            return result;
        }
        
        // If not cacheable, just execute the request
        return await next();
    }
}

// Interface to mark requests as cacheable
public interface ICacheableRequest {
    string CacheKey { get; }
    int CacheTimeInMinutes { get; }
}
```

## Registering Pipeline Behaviors

To register a pipeline behavior, add it to the service collection when configuring the mediator:

```csharp
using Microsoft.Extensions.DependencyInjection;
using UnambitiousFx.Mediator;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddApplicationServices(this IServiceCollection services) {
        services.AddMediator(config => {
            // Register pipeline behaviors in the order they should execute
            config.RegisterRequestPipelineBehavior<LoggingBehavior>();
            config.RegisterRequestPipelineBehavior<ValidationBehavior>();
            config.RegisterRequestPipelineBehavior<CachingBehavior>();
            
            // Register other mediator components
            // ...
        });
        
        return services;
    }
}
```

## Pipeline Behavior Execution Order

Pipeline behaviors are executed in the order they are registered. The first behavior registered will be the outermost in the pipeline, and the last behavior registered will be the innermost (closest to the handler).

For example, with the registration above, the execution flow would be:

1. LoggingBehavior (before)
2. ValidationBehavior (before)
3. CachingBehavior (before)
4. Request Handler
5. CachingBehavior (after)
6. ValidationBehavior (after)
7. LoggingBehavior (after)

## Best Practices

1. **Keep behaviors focused**: Each behavior should handle a single cross-cutting concern.
2. **Consider performance**: Be mindful of the performance impact of your behaviors, especially for high-throughput applications.
3. **Order matters**: Register behaviors in the order they should execute, with the most critical ones first.
4. **Error handling**: Behaviors should handle exceptions gracefully and return appropriate Result objects.
5. **Avoid state**: Pipeline behaviors should be stateless to avoid unexpected behavior in concurrent scenarios.