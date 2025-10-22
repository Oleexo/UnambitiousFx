---
title: Validation & Side Effects
parent: Result
nav_order: 4
---

# Result Validation & Side Effects
{: .no_toc }

## Table of contents
{: .no_toc .text-delta }

1. TOC
{:toc}

---

## Overview

Validation and side effects are essential for building robust applications with Result. This page covers:

- **Ensure**: Validate success values with predicates
- **Tap**: Execute side effects on success
- **TapError**: Execute side effects on failure  
- **TapBoth**: Execute side effects regardless of outcome

These operations maintain the Result chain while adding validation logic or side effects like logging, metrics, and notifications.

---

## Ensure - Predicate Validation

`Ensure` validates a success value against a predicate and converts to failure if the predicate fails.

### Basic Ensure

```csharp
Result<int> ValidatePositive(int value)
{
    return Result.Success(value)
        .Ensure(
            x => x > 0,
            x => new ValidationError(new[] { $"Value {x} must be positive" })
        );
}

// Success case
ValidatePositive(42);   // Success: 42

// Failure case
ValidatePositive(-5);   // Failure: ValidationError("Value -5 must be positive")
```

### Ensure Signature

```csharp
Result<T> Ensure<T>(
    Func<T, bool> predicate,           // Test the value
    Func<T, IError> errorFactory       // Create error from value if test fails
)

// Alternative with simple error
Result<T> Ensure<T>(
    Func<T, bool> predicate,
    IError error                        // Fixed error if test fails
)
```

### Multiple Ensures

Chain multiple validations:

```csharp
Result<User> ValidateUser(User user)
{
    return Result.Success(user)
        .Ensure(
            u => !string.IsNullOrEmpty(u.Name),
            _ => new ValidationError(new[] { "Name is required" })
        )
        .Ensure(
            u => !string.IsNullOrEmpty(u.Email),
            _ => new ValidationError(new[] { "Email is required" })
        )
        .Ensure(
            u => u.Email.Contains("@"),
            u => new ValidationError(
                new[] { "Email must be valid" },
                new Dictionary<string, object?> { ["email"] = u.Email }
            )
        )
        .Ensure(
            u => u.Age >= 18,
            u => new ValidationError(
                new[] { "Must be 18 or older" },
                new Dictionary<string, object?> { ["age"] = u.Age }
            )
        );
}

// Stops at first failed validation
```

### Ensure with Tuples

Works with multi-value Results:

```csharp
Result<string, int> ValidateNameAge(string name, int age)
{
    return Result.Success(name, age)
        .Ensure(
            (n, a) => !string.IsNullOrEmpty(n),
            (n, a) => new ValidationError(new[] { "Name required" })
        )
        .Ensure(
            (n, a) => a >= 18,
            (n, a) => new ValidationError(
                new[] { $"{n} must be 18 or older" },
                new Dictionary<string, object?> { ["age"] = a }
            )
        );
}
```

### Practical Ensure Examples

**Example 1: Range Validation**

```csharp
Result<int> ValidateQuantity(int quantity)
{
    return Result.Success(quantity)
        .Ensure(
            q => q >= 1,
            q => new ValidationError(
                new[] { "Quantity must be at least 1" },
                new Dictionary<string, object?> 
                { 
                    ["field"] = "quantity",
                    ["value"] = q,
                    ["min"] = 1
                }
            )
        )
        .Ensure(
            q => q <= 100,
            q => new ValidationError(
                new[] { "Quantity cannot exceed 100" },
                new Dictionary<string, object?> 
                { 
                    ["field"] = "quantity",
                    ["value"] = q,
                    ["max"] = 100
                }
            )
        );
}
```

**Example 2: Business Rule Validation**

```csharp
Result<Order> ValidateOrder(Order order)
{
    return Result.Success(order)
        .Ensure(
            o => o.Items.Any(),
            _ => new ValidationError(new[] { "Order must have at least one item" })
        )
        .Ensure(
            o => o.Total > 0,
            _ => new ValidationError(new[] { "Order total must be positive" })
        )
        .Ensure(
            o => o.Total == o.Items.Sum(i => i.Price),
            o => new ValidationError(
                new[] { "Order total doesn't match items" },
                new Dictionary<string, object?>
                {
                    ["calculatedTotal"] = o.Items.Sum(i => i.Price),
                    ["orderTotal"] = o.Total
                }
            )
        );
}
```

**Example 3: Authorization Check**

```csharp
Result<Document> GetDocument(User user, int documentId)
{
    return LoadDocument(documentId)
        .Ensure(
            doc => doc.OwnerId == user.Id || user.IsAdmin,
            doc => new UnauthorizedError(
                $"User {user.Id} cannot access document {doc.Id}",
                new Dictionary<string, object?>
                {
                    ["userId"] = user.Id,
                    ["documentId"] = doc.Id,
                    ["ownerId"] = doc.OwnerId
                }
            )
        );
}
```

### EnsureAsync - Async Validation

Validate with async predicates:

```csharp
Task<Result<User>> ValidateUniqueEmail(User user)
{
    return Result.Success(user)
        .EnsureAsync(
            async u => !await _database.EmailExistsAsync(u.Email),
            u => new ConflictError(
                $"Email {u.Email} is already registered",
                new Dictionary<string, object?> { ["email"] = u.Email }
            )
        );
}
```

### EnsureNotNull - Validate Non-Null Properties

`EnsureNotNull` validates that a projected property or value is not null. It's useful for ensuring required related objects exist.

**Signature**:
```csharp
Result<T> EnsureNotNull<T, TInner>(
    this Result<T> result,
    Func<T, TInner?> selector,
    string message,
    string? field = null) where TInner : class
```

**Example**:
```csharp
public Result<Order> ValidateOrder(Order order)
{
    return Result.Success(order)
        .EnsureNotNull(
            o => o.Customer,
            "Customer is required",
            field: "Customer"
        )
        .EnsureNotNull(
            o => o.ShippingAddress,
            "Shipping address is required",
            field: "ShippingAddress"
        )
        .EnsureNotNull(
            o => o.BillingAddress,
            "Billing address is required",
            field: "BillingAddress"
        );
}

// Usage
var order = new Order 
{ 
    Customer = null,  // Invalid!
    ShippingAddress = new Address()
};

var result = ValidateOrder(order);
// Failure: ValidationError("Customer: Customer is required")
```

**Practical Examples**:
```csharp
// Validate navigation properties
Result<User> ValidateUserProfile(User user)
{
    return Result.Success(user)
        .EnsureNotNull(u => u.Profile, "User must have a profile")
        .EnsureNotNull(u => u.Profile?.Avatar, "Profile must have an avatar")
        .EnsureNotNull(u => u.Settings, "User must have settings");
}

// Validate required relationships
Result<Post> ValidatePost(Post post)
{
    return Result.Success(post)
        .EnsureNotNull(p => p.Author, "Post must have an author", field: "Author")
        .EnsureNotNull(p => p.Category, "Post must have a category", field: "Category");
}
```

---

### EnsureNotEmpty - Validate Non-Empty Values

`EnsureNotEmpty` validates that strings or collections are not empty. Provides specific validation for common empty-check scenarios.

**Signatures**:
```csharp
// For strings
Result<string> EnsureNotEmpty(
    this Result<string> result,
    string message = "Value must not be empty.",
    string? field = null)

// For collections
Result<TCollection> EnsureNotEmpty<TCollection, TItem>(
    this Result<TCollection> result,
    string message = "Collection must not be empty.",
    string? field = null) where TCollection : IEnumerable<TItem>
```

**String Example**:
```csharp
Result<string> ValidateName(string name)
{
    return Result.Success(name)
        .EnsureNotEmpty("Name cannot be empty", field: "name");
}

ValidateName("Alice");  // Success: "Alice"
ValidateName("");       // Failure: ValidationError("name: Name cannot be empty")
ValidateName(null);     // Failure: ValidationError("name: Name cannot be empty")
```

**Collection Example**:
```csharp
Result<List<string>> ValidateItems(List<string> items)
{
    return Result.Success(items)
        .EnsureNotEmpty<List<string>, string>(
            "Order must contain at least one item",
            field: "items"
        );
}

ValidateItems(new List<string> { "item1" });  // Success
ValidateItems(new List<string>());            // Failure
```

**Practical Examples**:
```csharp
// Form validation
public Result<ContactForm> ValidateContactForm(ContactForm form)
{
    return Result.Success(form.Name)
        .EnsureNotEmpty("Name is required", field: "name")
        .Bind(_ => Result.Success(form.Email)
            .EnsureNotEmpty("Email is required", field: "email"))
        .Bind(_ => Result.Success(form.Message)
            .EnsureNotEmpty("Message is required", field: "message"))
        .Map(_ => form);
}

// Collection validation
public Result<Order> ValidateOrderItems(Order order)
{
    return Result.Success(order.Items)
        .EnsureNotEmpty<List<OrderItem>, OrderItem>(
            "Order must have at least one item",
            field: "items"
        )
        .Map(_ => order);
}

// Combined with other validations
Result<SearchQuery> ValidateSearchQuery(SearchQuery query)
{
    return Result.Success(query.Keywords)
        .EnsureNotEmpty("Search keywords cannot be empty", field: "keywords")
        .Ensure(
            k => k.Length >= 3,
            _ => new ValidationError(new[] { "Keywords must be at least 3 characters" })
        )
        .Map(_ => query);
}
```

---

## Tap - Side Effects on Success

`Tap` executes a side effect when Result is successful, without modifying the Result value.

### Basic Tap

```csharp
Result<User> result = GetUser(userId)
    .Tap(user => Console.WriteLine($"Found user: {user.Name}"))
    .Tap(user => _logger.LogInfo($"User {user.Id} accessed"))
    .Tap(user => _metrics.IncrementUserLoads());

// Value passes through unchanged
// Tap only runs if Result is success
```

### Tap Signature

```csharp
Result<T> Tap<T>(Action<T> action)

// action receives the success value
// action return value is ignored
// Result value is unchanged
```

### Common Tap Use Cases

**Logging:**
```csharp
return GetOrder(orderId)
    .Tap(order => _logger.LogInfo(
        "Order retrieved",
        new { orderId = order.Id, total = order.Total }
    ));
```

**Metrics:**
```csharp
return ProcessPayment(payment)
    .Tap(_ => _metrics.IncrementPaymentsProcessed())
    .Tap(result => _metrics.RecordPaymentAmount(result.Amount));
```

**Caching:**
```csharp
return LoadFromDatabase(key)
    .Tap(value => _cache.Set(key, value, TimeSpan.FromMinutes(10)));
```

**Notifications:**
```csharp
return CreateUser(userData)
    .Tap(user => _emailService.SendWelcomeEmail(user.Email))
    .Tap(user => _eventBus.Publish(new UserCreatedEvent(user.Id)));
```

**Debugging:**
```csharp
return CalculateTotal(items)
    .Tap(total => Debug.WriteLine($"Total calculated: {total}"))
    .Tap(total => 
    {
        if (total > 1000)
            Debug.WriteLine("Large order detected");
    });
```

### Tap with Tuples

```csharp
Result<string, int> nameAge = GetNameAndAge(userId)
    .Tap((name, age) => Console.WriteLine($"{name} is {age} years old"));
```

### TapAsync - Async Side Effects

Execute async operations as side effects:

```csharp
Task<Result<Order>> PlaceOrder(Order order)
{
    return ValidateOrder(order)
        .Bind(o => SaveOrder(o))
        .TapAsync(async o => await SendConfirmationEmailAsync(o))
        .TapAsync(async o => await UpdateInventoryAsync(o))
        .TapAsync(async o => await _analytics.TrackOrderAsync(o));
}
```

---

## TapError - Side Effects on Failure

`TapError` executes side effects when Result is a failure:

### Basic TapError

```csharp
Result<User> result = GetUser(userId)
    .TapError(error => _logger.LogError($"Failed to get user: {error.Message}"))
    .TapError(error => _metrics.IncrementUserLoadFailures())
    .TapError(error => _alerting.SendAlert("User load failed", error));

// Error passes through unchanged
// TapError only runs if Result is failure
```

### TapError Signature

```csharp
Result<T> TapError<T>(Action<IError> action)

// action receives the error
// action return value is ignored
// Result (including error) is unchanged
```

### Common TapError Use Cases

**Error Logging:**
```csharp
return ProcessPayment(payment)
    .TapError(error => _logger.LogError(
        "Payment processing failed",
        new 
        { 
            error = error.Message,
            code = error.Code,
            metadata = error.Metadata
        }
    ));
```

**Metrics & Monitoring:**
```csharp
return GetData(id)
    .TapError(error => _metrics.IncrementErrorCount(error.Code))
    .TapError(error => 
    {
        if (error is ExceptionalError)
            _monitoring.ReportException(error.Exception);
    });
```

**Alerting:**
```csharp
return CriticalOperation()
    .TapError(error => 
    {
        if (error.Code == "CRITICAL")
            _alerting.SendUrgentAlert(error.Message);
    });
```

**Audit Trail:**
```csharp
return DeleteUser(userId)
    .TapError(error => _audit.LogFailedDeletion(userId, error));
```

### TapErrorAsync - Async Error Side Effects

```csharp
Task<Result<Data>> LoadData(string id)
{
    return FetchDataAsync(id)
        .TapErrorAsync(async error => 
            await _errorRepository.SaveErrorAsync(new ErrorLog
            {
                Message = error.Message,
                Code = error.Code,
                Timestamp = DateTime.UtcNow
            })
        );
}
```

---

## TapBoth - Side Effects for Success and Failure

`TapBoth` executes different side effects based on whether the Result is successful or failed. It combines `Tap` and `TapError` into a single operation.

### Basic TapBoth

```csharp
Result<Order> result = ProcessOrder(order)
    .TapBoth(
        onSuccess: o => _logger.LogInfo($"Order {o.Id} processed"),
        onFailure: error => _logger.LogError($"Processing failed: {error.Message}")
    );

// Value or error passes through unchanged
// Appropriate action executes based on success/failure
```

### TapBoth Signature

```csharp
Result<T> TapBoth<T>(
    this Result<T> result,
    Action<T> onSuccess,
    Action<Exception> onFailure)
```

### Common TapBoth Use Cases

**Logging Both Outcomes:**
```csharp
return GetUser(userId)
    .TapBoth(
        onSuccess: user => _logger.LogInfo($"User {user.Id} loaded successfully"),
        onFailure: error => _logger.LogError($"Failed to load user: {error.Message}")
    );
```

**Metrics Collection:**
```csharp
return ProcessPayment(payment)
    .TapBoth(
        onSuccess: result => 
        {
            _metrics.RecordPaymentSuccess();
            _metrics.RecordPaymentAmount(result.Amount);
        },
        onFailure: error => 
        {
            _metrics.RecordPaymentFailure();
            _metrics.RecordErrorCode(error.Message);
        }
    );
```

**Audit Trail:**
```csharp
return UpdateUserProfile(userId, profile)
    .TapBoth(
        onSuccess: updated => _audit.Log($"Profile updated for user {userId}"),
        onFailure: error => _audit.Log($"Profile update failed for user {userId}: {error.Message}")
    );
```

**Notification:**
```csharp
return CreateOrder(orderData)
    .TapBoth(
        onSuccess: order => _notifications.SendSuccess($"Order {order.Id} created"),
        onFailure: error => _notifications.SendFailure($"Order creation failed: {error.Message}")
    );
```

### Chaining TapBoth

```csharp
Result<User> result = RegisterUser(userData)
    .TapBoth(
        onSuccess: user => _logger.LogInfo($"User {user.Id} registered"),
        onFailure: error => _logger.LogError($"Registration failed: {error.Message}")
    )
    .TapBoth(
        onSuccess: user => _metrics.IncrementRegistrations(),
        onFailure: error => _metrics.IncrementRegistrationFailures()
    )
    .TapBoth(
        onSuccess: user => _cache.Set(user.Id, user),
        onFailure: _ => { /* No cache on failure */ }
    );
```

### Practical TapBoth Examples

**Example 1: Complete Order Processing with Tracking**

```csharp
public async Task<Result<Order>> ProcessOrderAsync(OrderRequest request)
{
    var sw = Stopwatch.StartNew();
    
    return await ValidateOrder(request)
        .TapBoth(
            onSuccess: _ => _logger.LogDebug("Order validation passed"),
            onFailure: error => _logger.LogWarning($"Order validation failed: {error.Message}")
        )
        .BindAsync(async _ => await ReserveInventoryAsync(request.Items))
        .TapBoth(
            onSuccess: _ => _logger.LogDebug("Inventory reserved"),
            onFailure: error => 
            {
                _logger.LogError($"Inventory reservation failed: {error.Message}");
                _alerting.NotifyInventoryIssue(request.Items);
            }
        )
        .BindAsync(async _ => await ProcessPaymentAsync(request.Payment))
        .TapBoth(
            onSuccess: payment => 
            {
                _logger.LogInfo($"Payment processed: {payment.Id}");
                _metrics.RecordPaymentSuccess(payment.Amount);
            },
            onFailure: error => 
            {
                _logger.LogError($"Payment failed: {error.Message}");
                _metrics.RecordPaymentFailure();
                _alerting.NotifyPaymentFailure(request);
            }
        )
        .BindAsync(async payment => await CreateOrderAsync(request, payment))
        .TapBoth(
            onSuccess: order => 
            {
                sw.Stop();
                _logger.LogInfo($"Order {order.Id} created in {sw.ElapsedMilliseconds}ms");
                _metrics.RecordOrderProcessingTime(sw.ElapsedMilliseconds);
            },
            onFailure: error => 
            {
                sw.Stop();
                _logger.LogError($"Order creation failed after {sw.ElapsedMilliseconds}ms");
                _metrics.RecordOrderFailure();
            }
        );
}
```

**Example 2: User Authentication with Comprehensive Logging**

```csharp
public Result<AuthToken> Authenticate(LoginRequest request)
{
    return ValidateCredentials(request)
        .TapBoth(
            onSuccess: _ => _audit.Log($"Credentials validated for {request.Username}"),
            onFailure: _ => _audit.Log($"Invalid credentials for {request.Username}")
        )
        .Bind(user => CheckAccountStatus(user))
        .TapBoth(
            onSuccess: user => _audit.Log($"Account {user.Id} is active"),
            onFailure: error => 
            {
                _audit.Log($"Account check failed for {request.Username}");
                if (error.Message.Contains("locked"))
                    _security.NotifyAccountLocked(request.Username);
            }
        )
        .Bind(user => GenerateToken(user))
        .TapBoth(
            onSuccess: token => 
            {
                _logger.LogInfo($"User {request.Username} authenticated successfully");
                _metrics.RecordSuccessfulLogin();
                _cache.Set($"token:{token.Value}", token, token.ExpiresAt);
            },
            onFailure: error =>
            {
                _logger.LogWarning($"Authentication failed for {request.Username}");
                _metrics.RecordFailedLogin();
                _security.RecordFailedAttempt(request.Username);
            }
        );
}
```

**Example 3: Data Synchronization with Status Updates**

```csharp
public async Task<Result> SyncDataAsync(string sourceId, string targetId)
{
    var progress = new SyncProgress();
    
    return await LoadSourceDataAsync(sourceId)
        .TapBoth(
            onSuccess: data => 
            {
                progress.SourceLoaded = true;
                _events.Publish(new SyncProgress { Stage = "Source loaded" });
            },
            onFailure: error => 
            {
                progress.Failed = true;
                _events.Publish(new SyncProgress { Stage = "Source load failed", Error = error.Message });
            }
        )
        .BindAsync(async data => await ValidateDataAsync(data))
        .TapBoth(
            onSuccess: _ => 
            {
                progress.Validated = true;
                _events.Publish(new SyncProgress { Stage = "Data validated" });
            },
            onFailure: error => _events.Publish(new SyncProgress { Stage = "Validation failed", Error = error.Message })
        )
        .BindAsync(async data => await TransformDataAsync(data))
        .TapBoth(
            onSuccess: transformed => 
            {
                progress.Transformed = true;
                _events.Publish(new SyncProgress { Stage = "Data transformed", RecordCount = transformed.Count });
            },
            onFailure: error => _events.Publish(new SyncProgress { Stage = "Transform failed", Error = error.Message })
        )
        .BindAsync(async data => await SaveToTargetAsync(targetId, data))
        .TapBoth(
            onSuccess: _ => 
            {
                progress.Complete = true;
                _logger.LogInfo($"Sync completed: {sourceId} -> {targetId}");
                _events.Publish(new SyncProgress { Stage = "Complete" });
            },
            onFailure: error => 
            {
                _logger.LogError($"Sync failed: {sourceId} -> {targetId}: {error.Message}");
                _events.Publish(new SyncProgress { Stage = "Failed", Error = error.Message });
            }
        );
}
```

### TapBoth vs Tap + TapError

```csharp
// Using TapBoth - single call
result.TapBoth(
    onSuccess: value => LogSuccess(value),
    onFailure: error => LogError(error)
);

// Equivalent using Tap + TapError
result
    .Tap(value => LogSuccess(value))
    .TapError(error => LogError(error));

// TapBoth is more concise when you need both
// Tap + TapError is better when you only need one or want to chain differently
```

---

## Combining Tap and TapError

Handle both success and failure cases:

```csharp
Result<Order> ProcessOrder(Order order)
{
    return ValidateOrder(order)
        .Tap(o => _logger.LogInfo($"Order {o.Id} validated"))
        .TapError(error => _logger.LogError($"Validation failed: {error.Message}"))
        .Bind(o => SaveOrder(o))
        .Tap(o => _logger.LogInfo($"Order {o.Id} saved"))
        .TapError(error => _logger.LogError($"Save failed: {error.Message}"))
        .Tap(o => _eventBus.Publish(new OrderCreatedEvent(o.Id)))
        .TapError(error => _metrics.IncrementOrderFailures());
}
```

---

## Practical Examples

### Example 1: Complete User Registration

```csharp
public async Task<Result<User>> RegisterUserAsync(UserRegistration registration)
{
    return await ValidateRegistration(registration)
        .Tap(_ => _logger.LogInfo("Registration validated"))
        .TapError(error => _logger.LogWarning($"Validation failed: {error.Message}"))
        
        .EnsureAsync(
            async reg => !await _db.EmailExistsAsync(reg.Email),
            reg => new ConflictError($"Email {reg.Email} already exists")
        )
        .Tap(_ => _logger.LogInfo("Email uniqueness confirmed"))
        
        .BindAsync(async reg => await CreateUserAsync(reg))
        .Tap(user => _logger.LogInfo($"User {user.Id} created"))
        .TapError(error => _logger.LogError($"User creation failed: {error.Message}"))
        
        .TapAsync(async user => await _emailService.SendWelcomeEmailAsync(user.Email))
        .Tap(user => _metrics.IncrementUserRegistrations())
        
        .TapAsync(async user => await _eventBus.PublishAsync(
            new UserRegisteredEvent(user.Id, user.Email)
        ));
}
```

### Example 2: Order Processing Pipeline

```csharp
public class OrderProcessor
{
    public async Task<Result<OrderConfirmation>> ProcessOrderAsync(OrderRequest request)
    {
        var sw = Stopwatch.StartNew();
        
        return await ValidateRequest(request)
            .Tap(_ => _logger.LogDebug("Request validated"))
            .TapError(LogValidationError)
            
            .Bind(req => CheckInventory(req.Items))
            .Tap(available => _logger.LogDebug($"Inventory check: {available} items available"))
            .TapError(error => _metrics.RecordInventoryFailure())
            
            .Ensure(
                available => available,
                _ => new ValidationError(new[] { "Insufficient inventory" })
            )
            
            .BindAsync(async _ => await ProcessPaymentAsync(request.Payment))
            .Tap(payment => _logger.LogInfo($"Payment processed: {payment.Id}"))
            .TapError(LogPaymentError)
            .TapError(_ => _alerting.NotifyPaymentFailure(request))
            
            .BindAsync(async payment => await CreateOrderAsync(request, payment))
            .Tap(order => 
            {
                _logger.LogInfo($"Order created: {order.Id}");
                _metrics.RecordOrderValue(order.Total);
            })
            
            .TapAsync(async order => await UpdateInventoryAsync(order.Items))
            .TapAsync(async order => await SendConfirmationAsync(order))
            
            .Map(order => new OrderConfirmation
            {
                OrderId = order.Id,
                Total = order.Total,
                EstimatedDelivery = DateTime.Now.AddDays(3),
                ProcessingTime = sw.ElapsedMilliseconds
            })
            .Tap(confirmation => _logger.LogInfo(
                $"Order {confirmation.OrderId} processed in {confirmation.ProcessingTime}ms"
            ));
    }
    
    void LogValidationError(IError error) => 
        _logger.LogWarning($"Validation failed: {error.Message}", error.Metadata);
    
    void LogPaymentError(IError error) =>
        _logger.LogError($"Payment failed: {error.Message}", error.Metadata);
}
```

### Example 3: Debugging Pipeline

```csharp
Result<ProcessedData> ProcessData(RawData raw)
{
    return Result.Success(raw)
        .Tap(data => Debug.WriteLine($"Input: {JsonSerializer.Serialize(data)}"))
        
        .Map(data => Parse(data))
        .Tap(parsed => Debug.WriteLine($"Parsed: {parsed}"))
        
        .Bind(parsed => Validate(parsed))
        .Tap(validated => Debug.WriteLine($"Validated: {validated}"))
        .TapError(error => Debug.WriteLine($"Validation error: {error.Message}"))
        
        .Map(validated => Transform(validated))
        .Tap(transformed => Debug.WriteLine($"Transformed: {transformed}"))
        
        .Map(transformed => Enrich(transformed))
        .Tap(enriched => Debug.WriteLine($"Final result: {JsonSerializer.Serialize(enriched)}"));
}
```

---

## Best Practices

### ✅ Do

**Use Tap for observability:**
```csharp
return ProcessOrder(order)
    .Tap(o => _logger.LogInfo($"Order {o.Id} processed"))
    .Tap(o => _metrics.RecordOrderProcessed(o.Total))
    .Tap(o => _tracing.RecordSuccess());
```

**Use TapError for error tracking:**
```csharp
return CriticalOperation()
    .TapError(error => _logger.LogError(error.Message, error.Metadata))
    .TapError(error => _metrics.IncrementFailures(error.Code));
```

**Chain multiple Ensures for clear validation:**
```csharp
return Result.Success(user)
    .Ensure(u => u.Age >= 18, _ => new ValidationError(new[] { "Must be 18+" }))
    .Ensure(u => u.Email.Contains("@"), _ => new ValidationError(new[] { "Invalid email" }))
    .Ensure(u => u.Name.Length > 0, _ => new ValidationError(new[] { "Name required" }));
```

**Keep side effects side-effect-only:**
```csharp
// Good - Tap doesn't modify data
.Tap(user => _cache.Set(user.Id, user))

// Bad - modifying state that affects business logic
.Tap(user => user.LoginCount++)  // Don't mutate in Tap!
```

### ❌ Don't

**Don't throw exceptions in Tap:**
```csharp
// Bad - exception breaks the chain
.Tap(x => 
{
    if (x < 0) throw new Exception("Negative!");
})

// Good - use Ensure for validation
.Ensure(x => x >= 0, _ => new ValidationError(new[] { "Must be non-negative" }))
```

**Don't use Tap for transformations:**
```csharp
// Bad - Tap doesn't transform
string name = "";
result.Tap(user => name = user.Name);  // Awkward!

// Good - use Map
var name = result.Map(user => user.Name);
```

**Don't ignore Tap failures silently:**
```csharp
// Bad - swallowing exceptions
.Tap(x => 
{
    try { _cache.Set(key, x); }
    catch { /* silent fail */ }
})

// Good - log failures in side effects
.Tap(x => 
{
    try { _cache.Set(key, x); }
    catch (Exception ex) 
    { 
        _logger.LogWarning($"Cache set failed: {ex.Message}");
    }
})
```

---

## Next Steps

- **[Async Operations](async.html)** - TapAsync, EnsureAsync, async transformations
- **[Collections](collections.html)** - Validate multiple items
- **[Value Access](value-access.html)** - Extract values safely

Return to **[Result Overview](index.html)**
