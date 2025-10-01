---
title: Error Handling
parent: Result
nav_order: 3
---

# Result Error Handling
{: .no_toc }

## Table of contents
{: .no_toc .text-delta }

1. TOC
{:toc}

---

## Error Model Overview

UnambitiousFx provides a rich, structured error system that goes beyond simple exceptions. The error model includes:

- **IReason**: Base interface for all success/failure reasons
- **IError**: Interface for error reasons with code, message, and optional exception
- **ISuccess**: Interface for success reasons (enriched successes)
- **Domain Errors**: Pre-built error types for common scenarios
- **Metadata**: Attach contextual information to errors and results

### Why Structured Errors?

```csharp
// Traditional approach - limited information
catch (Exception ex)
{
    // All you have is: ex.Message, ex.StackTrace
    // Hard to distinguish error types
    // No structured context
}

// Result approach - rich, structured information
Result<User> result = FindUser(id);
if (result.IsFaulted)
{
    var error = result.Error;
    // - Stable error code (e.g., "NOT_FOUND", "VALIDATION")
    // - Human-readable message
    // - Structured metadata (resource type, field names, etc.)
    // - Original exception (if applicable)
    // - Type-safe error checking
}
```

---

## IReason Interfaces

### IReason - Base Interface

All reasons (successes and errors) implement `IReason`:

```csharp
public interface IReason
{
    string Message { get; }
    IReadOnlyDictionary<string, object?> Metadata { get; }
}
```

### IError - Error Reasons

Errors extend IReason with error-specific information:

```csharp
public interface IError : IReason
{
    string Code { get; }           // Machine-readable error code
    Exception? Exception { get; }  // Underlying exception (if any)
}
```

### ISuccess - Success Reasons

Success reasons provide additional context for successful operations:

```csharp
public interface ISuccess : IReason
{
    // Inherits Message and Metadata from IReason
}

// Example: Enriched success
var result = Result.Success(user)
    .WithSuccess(new SuccessReason(
        "User loaded from cache",
        new Dictionary<string, object?> { ["cacheHit"] = true }
    ));
```

---

## Domain Errors

UnambitiousFx includes pre-built domain error types for common scenarios. All domain errors include automatic metadata.

### NotFoundError

For resources that don't exist:

```csharp
public sealed record NotFoundError(
    string Resource,     // Type of resource (e.g., "User", "Order")
    string Identifier,   // ID or key that wasn't found
    IReadOnlyDictionary<string, object?>? Extra = null
) : ErrorBase;

// Usage
Result<User> FindUser(string userId)
{
    var user = _db.Find(userId);
    return user != null
        ? Result.Success(user)
        : Result.Failure<User>(new NotFoundError("User", userId));
}

// Error contains:
// - Code: "NOT_FOUND"
// - Message: "Resource 'User' with id '123' was not found."
// - Metadata: { "resource": "User", "identifier": "123" }

// With extra metadata
return Result.Failure<Order>(
    new NotFoundError("Order", orderId, new Dictionary<string, object?>
    {
        ["userId"] = userId,
        ["attemptedAt"] = DateTime.UtcNow
    })
);
```

### ValidationError

For validation failures:

```csharp
public sealed record ValidationError(
    IReadOnlyList<string> Failures,  // List of validation errors
    IReadOnlyDictionary<string, object?>? Extra = null
) : ErrorBase;

// Single failure
Result<User> ValidateAge(int age)
{
    return age >= 18
        ? Result.Success(age)
        : Result.Failure<int>(new ValidationError(
            new[] { "Age must be at least 18" }
        ));
}

// Multiple failures
Result<User> ValidateUser(UserInput input)
{
    var errors = new List<string>();
    
    if (string.IsNullOrEmpty(input.Name))
        errors.Add("Name is required");
    
    if (string.IsNullOrEmpty(input.Email))
        errors.Add("Email is required");
    else if (!input.Email.Contains("@"))
        errors.Add("Email must be valid");
    
    if (input.Age < 18)
        errors.Add("Must be 18 or older");
    
    return errors.Count == 0
        ? Result.Success(new User(input))
        : Result.Failure<User>(new ValidationError(errors));
}

// Error contains:
// - Code: "VALIDATION"
// - Message: "Name is required; Email is required; Must be 18 or older"
// - Metadata: { "failures": ["Name is required", ...] }

// With field-specific metadata
return Result.Failure<User>(new ValidationError(
    new[] { "Invalid email format" },
    new Dictionary<string, object?> 
    { 
        ["field"] = "email",
        ["value"] = input.Email 
    }
));
```

### ConflictError

For resource conflicts (duplicate keys, concurrent modifications):

```csharp
public sealed record ConflictError(
    string Message,
    IReadOnlyDictionary<string, object?>? Extra = null
) : ErrorBase;

// Usage
Result<User> RegisterUser(string email)
{
    if (_db.EmailExists(email))
        return Result.Failure<User>(new ConflictError(
            $"User with email '{email}' already exists",
            new Dictionary<string, object?> { ["email"] = email }
        ));
    
    // ... create user
}

// Error contains:
// - Code: "CONFLICT"
// - Message: Custom message
// - Metadata: Extra metadata you provide
```

### UnauthorizedError

For authentication/authorization failures:

```csharp
public sealed record UnauthorizedError(
    string? Reason = null,
    IReadOnlyDictionary<string, object?>? Extra = null
) : ErrorBase;

// Usage - simple
Result<Resource> GetResource(User user)
{
    if (!user.IsAuthenticated)
        return Result.Failure<Resource>(new UnauthorizedError());
    
    // ... get resource
}

// With reason
Result<Order> GetOrder(User user, int orderId)
{
    var order = _db.GetOrder(orderId);
    
    if (order.UserId != user.Id)
        return Result.Failure<Order>(new UnauthorizedError(
            "You can only access your own orders",
            new Dictionary<string, object?> 
            { 
                ["userId"] = user.Id,
                ["orderId"] = orderId 
            }
        ));
    
    return Result.Success(order);
}

// Error contains:
// - Code: "UNAUTHORIZED"
// - Message: Reason or "Unauthorized."
// - Metadata: Extra metadata
```

### ExceptionalError

Wraps exceptions that weren't expected:

```csharp
public sealed record ExceptionalError(
    Exception Exception,
    string? MessageOverride = null,
    IReadOnlyDictionary<string, object?>? Extra = null
) : ErrorBase;

// Usage - automatic with Try
Result<Config> LoadConfig(string path)
{
    return Result.Try(() => 
    {
        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<Config>(json);
    });
    // Any exception becomes ExceptionalError automatically
}

// Manual creation
Result<User> ProcessUser(User user)
{
    try
    {
        // ... processing
        return Result.Success(user);
    }
    catch (Exception ex)
    {
        return Result.Failure<User>(new ExceptionalError(
            ex,
            "Failed to process user",  // Custom message
            new Dictionary<string, object?> { ["userId"] = user.Id }
        ));
    }
}

// Error contains:
// - Code: "EXCEPTION"
// - Message: Exception message or override
// - Exception: Original exception preserved
// - Metadata: { "exceptionType": "System.IO.FileNotFoundException", ...extra }
```

---

## Metadata System

Metadata enriches Results and Reasons with contextual information that's structured and queryable.

### Result-Level Metadata

Attach metadata to the Result itself:

```csharp
Result<User> result = Result.Success(user)
    .WithMetadata("source", "cache")
    .WithMetadata("timestamp", DateTime.UtcNow);

// Multiple metadata at once (params tuples)
result = Result.Success(user)
    .WithMetadata(
        ("source", "database"),
        ("queryTime", 42),
        ("cached", false)
    );

// Access metadata
if (result.Metadata.TryGetValue("source", out var source))
{
    Console.WriteLine($"Loaded from: {source}");
}
```

### Reason-Level Metadata

Errors and success reasons have their own metadata:

```csharp
var error = new ValidationError(
    new[] { "Invalid email" },
    new Dictionary<string, object?>
    {
        ["field"] = "email",
        ["value"] = "not-an-email",
        ["attemptNumber"] = 3
    }
);

// Metadata is part of the error
// Access through error.Metadata
```

### WithError/WithSuccess Helpers

Add errors or successes with metadata:

```csharp
// Add error with metadata
Result<User> result = Result.Failure<User>(
    new ValidationError(new[] { "Invalid input" })
)
.WithError(new NotFoundError("Database", "connection"));

// Add success reason with metadata
Result<User> cached = Result.Success(user)
    .WithSuccess(new SuccessReason(
        "Loaded from cache",
        new Dictionary<string, object?> { ["cacheKey"] = "user:123" }
    ));

// WithMetadata copies to reasons (optional)
Result<User> enriched = Result.Success(user)
    .WithMetadata(("operation", "create"), copyToReasons: true)
    .WithSuccess(new SuccessReason("User created", new()));
// Success reason now includes "operation" metadata
```

### Practical Metadata Example

```csharp
public class UserService
{
    Result<User> GetUser(string userId)
    {
        var sw = Stopwatch.StartNew();
        
        // Try cache first
        var cached = _cache.Get<User>(userId);
        if (cached != null)
        {
            return Result.Success(cached)
                .WithMetadata("source", "cache")
                .WithMetadata("duration", sw.ElapsedMilliseconds);
        }
        
        // Load from database
        var user = _db.Find(userId);
        sw.Stop();
        
        if (user == null)
        {
            return Result.Failure<User>(
                new NotFoundError("User", userId, new Dictionary<string, object?>
                {
                    ["checkedCache"] = true,
                    ["duration"] = sw.ElapsedMilliseconds
                })
            );
        }
        
        _cache.Set(userId, user);
        
        return Result.Success(user)
            .WithMetadata("source", "database")
            .WithMetadata("duration", sw.ElapsedMilliseconds)
            .WithMetadata("cached", true);
    }
}

// Usage - metadata available for logging, metrics, debugging
var result = service.GetUser("123");
result.Match(
    success: user => 
    {
        var source = result.Metadata["source"];
        var duration = result.Metadata["duration"];
        _metrics.RecordUserLoad(source, duration);
    },
    failure: error =>
    {
        _logger.LogError("Failed to load user", error.Metadata);
    }
);
```

---

## Transforming Errors

### MapError - Transform Error Information

`MapError` transforms the error while preserving success:

```csharp
Result<User> result = FindUser(userId)
    .MapError(error => new ExceptionalError(
        new Exception($"User lookup failed: {error.Message}"),
        extra: new Dictionary<string, object?> 
        { 
            ["originalCode"] = error.Code,
            ["userId"] = userId 
        }
    ));

// Success → Success (unchanged)
// Failure → Transformed failure
```

### Prepend/Append Error Messages

Add context to error messages:

```csharp
Result<Order> GetOrder(string userId, int orderId)
{
    return FindUser(userId)
        .Bind(user => LoadOrder(orderId))
        .PrependError($"Failed to get order {orderId} for user {userId}: ")
        .AppendError($" (Attempted at {DateTime.UtcNow})");
}

// Original error: "Order not found"
// After prepend: "Failed to get order 123 for user ABC: Order not found"
// After append: "Failed to get order 123 for user ABC: Order not found (Attempted at 2025-09-30...)"
```

### Practical MapError Example

```csharp
public class OrderService
{
    // Add consistent error context across service boundary
    Result<Order> GetOrder(int orderId)
    {
        return _repository.GetOrder(orderId)
            .MapError(error => error switch
            {
                NotFoundError nf => new NotFoundError(
                    "Order",
                    orderId.ToString(),
                    new Dictionary<string, object?>
                    {
                        ["service"] = "OrderService",
                        ["operation"] = "GetOrder",
                        ["timestamp"] = DateTime.UtcNow
                    }
                ),
                ValidationError ve => new ValidationError(
                    ve.Failures.Prepend("Order validation failed:").ToList(),
                    ve.Extra
                ),
                _ => error
            });
    }
}
```

---

## Recovery

Recovery methods handle failures and provide fallbacks or alternatives.

### Recover - Provide Fallback Value

```csharp
// Simple fallback
Result<int> result = GetValue()
    .Recover(error => 0);  // If failed, use 0

// Conditional recovery
Result<User> user = FindUser(userId)
    .Recover(error => error is NotFoundError 
        ? User.Guest 
        : throw new InvalidOperationException("Cannot recover")
    );
```

### RecoverWith - Provide Alternative Result

```csharp
// Try alternative source
Result<Config> config = LoadFromFile(path)
    .RecoverWith(error => LoadFromDatabase());

// Multiple fallbacks
Result<User> user = LoadFromCache(userId)
    .RecoverWith(_ => LoadFromDatabase(userId))
    .RecoverWith(_ => LoadFromBackupDatabase(userId))
    .Recover(_ => User.Guest);  // Final fallback to value
```

### RecoverAsync - Async Recovery

```csharp
Task<Result<Data>> data = LoadLocalAsync()
    .RecoverAsync(error => LoadFromApiAsync())
    .RecoverAsync(error => LoadFromBackupApiAsync());
```

### Practical Recovery Example

```csharp
public class CacheService<T>
{
    public async Task<Result<T>> GetAsync(string key)
    {
        return await GetFromRedis(key)
            .RecoverAsync(error => 
            {
                _logger.LogWarning($"Redis failed: {error.Message}, trying memory cache");
                return GetFromMemoryCache(key);
            })
            .RecoverAsync(error =>
            {
                _logger.LogWarning($"Memory cache failed: {error.Message}, loading from source");
                return LoadFromSourceAsync(key);
            })
            .TapAsync(value => 
            {
                // Repopulate caches on successful load
                await PopulateCachesAsync(key, value);
            });
    }
    
    Result<T> GetFromRedis(string key) { /* ... */ }
    Result<T> GetFromMemoryCache(string key) { /* ... */ }
    Task<Result<T>> LoadFromSourceAsync(string key) { /* ... */ }
    Task PopulateCachesAsync(string key, T value) { /* ... */ }
}
```

---

## Error Inspection

### HasError<TError>

Check for specific error types:

```csharp
Result<User> result = FindUser(userId);

if (result.HasError<NotFoundError>())
{
    // Handle not found specifically
    return NotFound();
}

if (result.HasError<ValidationError>())
{
    // Handle validation errors
    return BadRequest(result.Error);
}

if (result.HasError<UnauthorizedError>())
{
    // Handle authorization
    return Unauthorized();
}
```

### HasException<TException>

Check for wrapped exceptions:

```csharp
Result<Config> config = LoadConfig(path);

if (config.HasException<FileNotFoundException>())
{
    Console.WriteLine("Config file not found, using defaults");
}

if (config.HasException<JsonException>())
{
    Console.WriteLine("Config file is malformed");
}
```

---

## Best Practices

### ✅ Do

**Use domain-specific errors:**
```csharp
// Good - clear intent, structured data
return Result.Failure<User>(new NotFoundError("User", userId));

// Avoid - generic, less structured
return Result.Failure<User>(new Exception("User not found"));
```

**Add metadata for debugging:**
```csharp
return Result.Failure<Order>(
    new ValidationError(
        new[] { "Invalid quantity" },
        new Dictionary<string, object?>
        {
            ["field"] = "quantity",
            ["value"] = order.Quantity,
            ["min"] = 1,
            ["max"] = 100
        }
    )
);
```

**Use MapError to add context across boundaries:**
```csharp
// Service layer adds operation context
return _repository.GetUser(id)
    .MapError(error => error
        .WithMetadata("service", "UserService")
        .WithMetadata("operation", "GetUser")
    );
```

**Recover gracefully:**
```csharp
// Try alternatives before failing
return LoadFromCache(key)
    .RecoverWith(_ => LoadFromDatabase(key))
    .Recover(_ => defaultValue);
```

### ❌ Don't

**Don't lose error information:**
```csharp
// Bad - discards original error
.MapError(_ => new ValidationError(new[] { "Failed" }))

// Good - preserve original error
.MapError(error => new ValidationError(
    new[] { $"Validation failed: {error.Message}" },
    new Dictionary<string, object?> { ["originalError"] = error }
))
```

**Don't recover from all errors blindly:**
```csharp
// Bad - hides real problems
.Recover(_ => defaultValue)

// Good - recover only expected errors
.Recover(error => error is NotFoundError ? defaultValue : throw error.Exception)
```

---

## Next Steps

- **[Validation & Side Effects](validation.html)** - Ensure, Tap, TapError
- **[Async Operations](async.html)** - MapErrorAsync, RecoverAsync
- **[Value Access](value-access.html)** - Safe value extraction

Return to **[Result Overview](index.html)**
