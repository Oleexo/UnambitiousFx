---
title: Async Operations
parent: Result
nav_order: 5
---

# Result Async Operations
{: .no_toc }

## Table of contents
{: .no_toc .text-delta }

1. TOC
{:toc}

---

## Overview

Result provides full async support for building asynchronous pipelines. All major operations have async variants that work seamlessly with `Task<Result<T>>` and `ValueTask<Result<T>>`.

**Key Features:**
- Async transformations (MapAsync, BindAsync)
- Async validation (EnsureAsync)
- Async side effects (TapAsync, TapErrorAsync)
- Task integration (FromTask, ToTask)
- ValueTask support for high-performance scenarios
- Exception wrapping (FromTryAsync)

---

## MapAsync - Async Transformations

Transform Result values using async functions:

```csharp
Task<Result<UserDto>> GetUserDtoAsync(string userId)
{
    return GetUserAsync(userId)
        .MapAsync(async user => 
        {
            var settings = await LoadSettingsAsync(user.Id);
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                PreferredLanguage = settings.Language
            };
        });
}

// Signature
Task<Result<TOut>> MapAsync<T, TOut>(
    this Task<Result<T>> resultTask,
    Func<T, Task<TOut>> asyncMapper
)
```

### Multiple Async Maps

```csharp
Task<Result<string>> ProcessAsync(string input)
{
    return Result.Success(input)
        .MapAsync(async s => await NormalizeAsync(s))
        .MapAsync(async s => await EnrichAsync(s))
        .MapAsync(async s => await FormatAsync(s));
}
```

---

## BindAsync - Async Chaining

Chain async operations that return `Result`:

```csharp
async Task<Result<Order>> PlaceOrderAsync(string userId, List<int> productIds)
{
    return await GetUserAsync(userId)
        .BindAsync(async user => await ValidateUserAsync(user))
        .BindAsync(async user => await GetProductsAsync(productIds))
        .BindAsync(async products => await CreateOrderAsync(user, products))
        .BindAsync(async order => await ProcessPaymentAsync(order));
}

// Signature
Task<Result<TOut>> BindAsync<T, TOut>(
    this Task<Result<T>> resultTask,
    Func<T, Task<Result<TOut>>> asyncBinder
)
```

### Mixing Sync and Async

```csharp
Task<Result<ProcessedData>> ProcessAsync(RawData data)
{
    return Result.Success(data)
        .Map(d => Validate(d))              // Sync operation
        .BindAsync(async d => await FetchAdditionalDataAsync(d))  // Async
        .Map(d => Transform(d))              // Sync again
        .BindAsync(async d => await SaveAsync(d));  // Async
}
```

---

## EnsureAsync - Async Validation

Validate with async predicates:

```csharp
async Task<Result<User>> ValidateUniqueEmailAsync(User user)
{
    return await Result.Success(user)
        .EnsureAsync(
            async u => !await _db.EmailExistsAsync(u.Email),
            u => new ConflictError($"Email {u.Email} already exists")
        )
        .EnsureAsync(
            async u => await _spamChecker.IsValidEmailAsync(u.Email),
            u => new ValidationError(new[] { "Email failed spam check" })
        );
}

// Signature
Task<Result<T>> EnsureAsync<T>(
    this Task<Result<T>> resultTask,
    Func<T, Task<bool>> asyncPredicate,
    Func<T, IError> errorFactory
)
```

---

## TapAsync - Async Side Effects

Execute async side effects:

```csharp
Task<Result<Order>> CreateOrderAsync(Order order)
{
    return Result.Success(order)
        .BindAsync(async o => await SaveOrderAsync(o))
        .TapAsync(async o => await SendConfirmationEmailAsync(o.UserEmail))
        .TapAsync(async o => await UpdateInventoryAsync(o.Items))
        .TapAsync(async o => await _analytics.TrackOrderAsync(o))
        .TapAsync(async o => await _cache.InvalidateAsync($"orders:{o.UserId}"));
}

// Signature
Task<Result<T>> TapAsync<T>(
    this Task<Result<T>> resultTask,
    Func<T, Task> asyncAction
)
```

### TapErrorAsync

Async side effects on failure:

```csharp
Task<Result<Data>> FetchDataAsync(string id)
{
    return LoadDataAsync(id)
        .TapErrorAsync(async error => 
            await _errorLog.SaveAsync(new ErrorEntry
            {
                Message = error.Message,
                Code = error.Code,
                Timestamp = DateTime.UtcNow
            })
        )
        .TapErrorAsync(async error =>
            await _monitoring.ReportFailureAsync(error));
}
```

---

## Exception Handling

### FromTryAsync - Wrap Async Operations

Automatically catch exceptions and convert to Result:

```csharp
async Task<Result<Config>> LoadConfigAsync(string path)
{
    return await Result.TryAsync(async () =>
    {
        var json = await File.ReadAllTextAsync(path);
        var config = JsonSerializer.Deserialize<Config>(json);
        
        if (config == null)
            throw new InvalidOperationException("Config is null");
        
        return config;
    });
}

// Any exception becomes ExceptionalError
```

### FromTask - Convert Task to Result

Convert existing Task-returning methods:

```csharp
// External API that throws exceptions
Task<User> GetUserFromApiAsync(string id);

// Wrap in Result
Result<User> result = await Result.FromTask(GetUserFromApiAsync("123"));

// Exceptions become failures automatically
```

---

## ValueTask Support

For high-performance scenarios, use ValueTask:

```csharp
ValueTask<Result<User>> GetUserAsync(string id)
{
    // Check memory cache (synchronous)
    if (_memoryCache.TryGetValue(id, out User cached))
        return new ValueTask<Result<User>>(Result.Success(cached));
    
    // Load from database (asynchronous)
    return LoadFromDatabaseAsync(id);
}

// All async operations support ValueTask
ValueTask<Result<string>> processed = GetUserAsync(id)
    .MapAsync(async user => await ProcessAsync(user))
    .BindAsync(async processed => await SaveAsync(processed));
```

---

## Flatten with Async

Remove nesting from async Results:

```csharp
// Task<Result<Result<T>>> → Task<Result<T>>
Task<Result<User>> flattened = nestedResultTask.Flatten();

// Common scenario
Task<Result<User>> GetUserAsync(string id)
{
    return LoadUserIdAsync()
        .MapAsync(async userId => await FetchUserAsync(userId))  // Returns Task<Result<Result<User>>>
        .Flatten();  // Flatten to Task<Result<User>>
}
```

---

## Practical Examples

### Example 1: User Registration Flow

```csharp
public class UserService
{
    public async Task<Result<User>> RegisterAsync(RegistrationData data)
    {
        return await Result.Success(data)
            // Validate email format (sync)
            .Ensure(
                d => IsValidEmail(d.Email),
                d => new ValidationError(new[] { "Invalid email format" })
            )
            
            // Check email uniqueness (async)
            .EnsureAsync(
                async d => !await _db.EmailExistsAsync(d.Email),
                d => new ConflictError($"Email {d.Email} already registered")
            )
            
            // Validate password strength (sync)
            .Ensure(
                d => d.Password.Length >= 8,
                d => new ValidationError(new[] { "Password must be 8+ characters" })
            )
            
            // Hash password (async)
            .MapAsync(async d => new User
            {
                Email = d.Email,
                PasswordHash = await _hasher.HashAsync(d.Password),
                CreatedAt = DateTime.UtcNow
            })
            
            // Save to database (async)
            .BindAsync(async user => await SaveUserAsync(user))
            
            // Side effects (async)
            .TapAsync(async user => await _email.SendWelcomeAsync(user.Email))
            .TapAsync(async user => await _events.PublishAsync(new UserRegistered(user.Id)))
            .Tap(user => _logger.LogInfo($"User {user.Id} registered"));
    }
}
```

### Example 2: Order Processing

```csharp
public async Task<Result<OrderConfirmation>> ProcessOrderAsync(OrderRequest request)
{
    return await ValidateRequest(request)
        // Check inventory (async database call)
        .BindAsync(async req => await CheckInventoryAsync(req.Items))
        
        // Calculate totals (sync)
        .Map(items => CalculateTotals(items))
        
        // Process payment (async external API)
        .BindAsync(async totals => await ProcessPaymentAsync(request.Payment, totals))
        .TapError(async error => await _payment.RefundAsync(request.Payment))
        
        // Create order (async database)
        .BindAsync(async payment => await CreateOrderAsync(request, payment))
        
        // Update inventory (async)
        .TapAsync(async order => await ReduceInventoryAsync(order.Items))
        
        // Send notifications (async, fire and forget style)
        .TapAsync(async order => 
        {
            await Task.WhenAll(
                _email.SendConfirmationAsync(order.UserEmail),
                _sms.SendTrackingInfoAsync(order.Phone),
                _webhook.NotifyAsync(order.Id)
            );
        })
        
        // Build response (sync)
        .Map(order => new OrderConfirmation
        {
            OrderId = order.Id,
            Total = order.Total,
            EstimatedDelivery = DateTime.Now.AddDays(3)
        });
}
```

### Example 3: Data Pipeline

```csharp
public async Task<Result<ProcessedData>> ProcessDataAsync(string dataId)
{
    return await Result.TryAsync(async () => 
        {
            // Load raw data
            var raw = await _storage.LoadAsync(dataId);
            if (raw == null)
                throw new NotFoundException($"Data {dataId} not found");
            return raw;
        })
        // Parse (sync)
        .Map(raw => Parse(raw))
        
        // Enrich with external data (async)
        .BindAsync(async parsed => await EnrichDataAsync(parsed))
        
        // Transform (sync)
        .Map(enriched => Transform(enriched))
        
        // Validate transformed data (async business rules)
        .EnsureAsync(
            async transformed => await ValidateBusinessRulesAsync(transformed),
            t => new ValidationError(new[] { "Business rule validation failed" })
        )
        
        // Save processed data (async)
        .TapAsync(async processed => await _storage.SaveAsync(dataId, processed))
        
        // Update cache (async)
        .TapAsync(async processed => await _cache.SetAsync(dataId, processed, TimeSpan.FromHours(1)))
        
        // Log metrics (async)
        .TapAsync(async processed => await _metrics.RecordProcessingAsync(processed))
        .TapErrorAsync(async error => await _metrics.RecordFailureAsync(dataId, error));
}
```

---

## Best Practices

### ✅ Do

**Use async all the way:**
```csharp
// Good - fully async
Task<Result<User>> GetUserAsync(string id)
{
    return LoadAsync(id)
        .MapAsync(async u => await EnrichAsync(u))
        .TapAsync(async u => await CacheAsync(u));
}

// Avoid - mixing sync/async awkwardly
Task<Result<User>> GetUserAsync(string id)
{
    var result = LoadAsync(id).Result;  // Blocking!
    return Task.FromResult(result);
}
```

**Use ValueTask for hot paths:**
```csharp
// Cache-heavy operations
ValueTask<Result<T>> GetAsync(string key)
{
    if (_cache.TryGet(key, out T value))
        return new ValueTask<Result<T>>(Result.Success(value));
    
    return LoadAndCacheAsync(key);
}
```

**Handle cancellation explicitly when needed:**
```csharp
async Task<Result<Data>> LoadAsync(string id, CancellationToken ct)
{
    return await Result.TryAsync(async () =>
    {
        ct.ThrowIfCancellationRequested();
        var data = await _http.GetAsync(url, ct);
        return ProcessData(data);
    });
}
```

### ❌ Don't

**Don't block on async Result:**
```csharp
// Bad - deadlock risk
var result = GetUserAsync(id).Result;

// Good - await it
var result = await GetUserAsync(id);
```

**Don't forget error handling in async side effects:**
```csharp
// Bad - exceptions lost
.TapAsync(async x => await _cache.SetAsync(key, x))

// Good - handle exceptions
.TapAsync(async x =>
{
    try
    {
        await _cache.SetAsync(key, x);
    }
    catch (Exception ex)
    {
        _logger.LogWarning($"Cache failed: {ex.Message}");
    }
})
```

---

## Performance Considerations

- **ValueTask** is more efficient for cached/synchronous results
- **Task** is simpler and sufficient for most scenarios
- Avoid excessive Task allocations in hot paths
- Use ConfigureAwait(false) in library code when appropriate

---

## Next Steps

- **[Collections](collections.html)** - Async batch operations
- **[Value Access](value-access.html)** - Extract values from async Results
- **[Error Handling](error-handling.html)** - RecoverAsync

Return to **[Result Overview](index.html)**
