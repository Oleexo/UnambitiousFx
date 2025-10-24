---
title: Extensions Reference
parent: Result
nav_order: 8
---

# Result Extensions Reference
{: .no_toc }

Comprehensive reference for all Result extension methods.

## Table of contents
{: .no_toc .text-delta }

1. TOC
{:toc}

---

## Overview

This page provides a complete reference for all Result extension methods organized by category. Each extension includes:
- **Description**: What the extension does
- **Signature**: Method signature with type parameters
- **Examples**: Practical usage examples
- **Related**: Links to similar extensions

---

## Quick Reference

Jump to any extension category or specific method:

### Transformations
[Map](#map) · [Bind](#bind) · [Flatten](#flatten) · [Zip](#zip) · [Try](#try)

### Validation
[Ensure](#ensure) · [EnsureNotNull](#ensurenotnull) · [EnsureNotEmpty](#ensurenotempty)

### Side Effects
[Tap](#tap) · [TapError](#taperror) · [TapBoth](#tapboth)

### Error Handling
[MapError](#maperror) · [PrependError](#prependerror) · [AppendError](#appenderror) · [WithContext](#withcontext) · [HasError](#haserror) · [HasException](#hasexception) · [FindError](#finderror) · [TryPickError](#trypickerror) · [MatchError](#matcherror) · [FilterError](#filtererror) · [Recover](#recover)

### Collections
[Traverse](#traverse) · [Sequence](#sequence) · [Combine](#combine) · [Apply](#apply)

### Value Access
[ValueOr](#valueor) · [ValueOrThrow](#valueorthrow) · [TryGet](#tryget) · [ToNullable](#tonullable) · [Match](#match)

### Async Operations
[MapAsync](#mapasync) · [BindAsync](#bindasync) · [EnsureAsync](#ensureasync) · [TapAsync](#tapasync) · [TapErrorAsync](#taperrorasync) · [TraverseAsync](#traverseasync) · [RecoverAsync](#recoverasync)

### Advanced
[Ok](#ok) · [Deconstruct](#deconstruct)

---

## Transformations

Extensions that transform Result values.

### Map

**Description**: Transforms the success value of a Result without changing success/failure status.

**Signature**:
```csharp
Result<TOut> Map<T, TOut>(this Result<T> result, Func<T, TOut> mapper)
```

**Example**:
```csharp
Result<int> age = Result.Success(25);
Result<string> message = age.Map(a => $"Age: {a}");
// Success: "Age: 25"

Result<int> failed = Result.Failure<int>("Error");
Result<string> stillFailed = failed.Map(a => $"Age: {a}");
// Failure: "Error" - Map doesn't execute
```

**Related**: [Bind](#bind), [MapAsync](#mapasync)

---

### Bind

**Description**: Chains operations that return Results. Automatically flattens nested Results and short-circuits on failure.

**Signature**:
```csharp
Result<TOut> Bind<T, TOut>(this Result<T> result, Func<T, Result<TOut>> binder)
```

**Example**:
```csharp
Result<int> ParseInt(string s) => 
    int.TryParse(s, out var v) ? Result.Success(v) : Result.Failure<int>("Not a number");

Result<int> Divide(int a, int b) =>
    b != 0 ? Result.Success(a / b) : Result.Failure<int>("Division by zero");

Result<int> result = ParseInt("10")
    .Bind(num => Divide(100, num));
// Success: 10

Result<int> failed = ParseInt("abc")
    .Bind(num => Divide(100, num));
// Failure: "Not a number" - Bind short-circuits
```

**Related**: [Map](#map), [BindAsync](#bindasync), [Flatten](#flatten)

---

### Flatten

**Description**: Collapses nested `Result<Result<T>>` into `Result<T>`.

**Signature**:
```csharp
Result<T> Flatten<T>(this Result<Result<T>> result)
```

**Example**:
```csharp
Result<Result<int>> nested = Result.Success(Result.Success(42));
Result<int> flattened = nested.Flatten();
// Success: 42

// Useful when Map returns a Result
Result<string> userId = Result.Success("123");
Result<User> user = userId
    .Map(id => GetUser(id))  // Returns Result<Result<User>>
    .Flatten();              // Flattens to Result<User>

// Better: use Bind directly
Result<User> user2 = userId.Bind(id => GetUser(id));
```

**Related**: [Bind](#bind), [Map](#map)

---

### Zip

**Description**: Combines multiple independent Results into a tuple or projected value.

**Signature**:
```csharp
// Tuple result
Result<T1, T2> Zip<T1, T2>(this Result<T1> r1, Result<T2> r2)

// Projected result
Result<TOut> Zip<T1, T2, TOut>(
    this Result<T1> r1, 
    Result<T2> r2, 
    Func<T1, T2, TOut> projector)
```

**Example**:
```csharp
Result<string> name = Result.Success("Alice");
Result<int> age = Result.Success(30);

// Zip into tuple
Result<string, int> nameAge = name.Zip(age);
// Success: ("Alice", 30)

// Zip with projection
Result<User> user = name.Zip(age, (n, a) => new User(n, a));
// Success: User(Name="Alice", Age=30)

// Failure if either fails
Result<string> failed = Result.Failure<string>("No name");
Result<int> stillFailed = failed.Zip(age);
// Failure: "No name"
```

**Related**: [Apply](#apply), [Combine](#combine)

---

### Try

**Description**: Wraps potentially throwing code in a Result. Catches exceptions and converts them to failure Results.

**Signature**:
```csharp
// Synchronous
Result<T> Try<T>(Func<T> action)
Result Try(Action action)

// Asynchronous
Task<Result<T>> TryAsync<T>(Func<Task<T>> asyncAction)
Task<Result> TryAsync(Func<Task> asyncAction)
```

**Example**:
```csharp
// Wrap synchronous operations
Result<int> ParseNumber(string input)
{
    return Result.Try(() => int.Parse(input));
}

ParseNumber("42");    // Success: 42
ParseNumber("abc");   // Failure: ExceptionalError with FormatException

// Wrap complex operations
Result<Config> LoadConfig(string path)
{
    return Result.Try(() => 
    {
        var json = File.ReadAllText(path);
        var config = JsonSerializer.Deserialize<Config>(json);
        
        if (config == null)
            throw new InvalidOperationException("Config is null");
        
        return config;
    });
}

// Non-generic for void operations
Result SaveData(Data data)
{
    return Result.Try(() => 
    {
        ValidateData(data);
        _database.Save(data);
        _cache.Invalidate();
    });
}
```

**Async Examples**:
```csharp
// Async operations
async Task<Result<User>> GetUserAsync(string id)
{
    return await Result.TryAsync(async () =>
    {
        var user = await _api.GetUserAsync(id);
        
        if (user == null)
            throw new NotFoundException($"User {id} not found");
        
        return user;
    });
}

// Async void operations
async Task<Result> SendEmailAsync(string to, string subject, string body)
{
    return await Result.TryAsync(async () =>
    {
        await _emailService.SendAsync(to, subject, body);
        await _auditLog.LogEmailSentAsync(to);
    });
}
```

**Practical Examples**:
```csharp
// File operations
Result<string> ReadFile(string path)
{
    return Result.Try(() => File.ReadAllText(path));
    // FileNotFoundException, IOException, etc. become failures
}

// JSON deserialization
Result<T> Deserialize<T>(string json)
{
    return Result.Try(() => JsonSerializer.Deserialize<T>(json));
    // JsonException becomes failure
}

// Database operations
async Task<Result<Order>> CreateOrderAsync(OrderData data)
{
    return await Result.TryAsync(async () =>
    {
        using var transaction = await _db.BeginTransactionAsync();
        
        var order = new Order(data);
        await _db.Orders.AddAsync(order);
        await _db.SaveChangesAsync();
        await transaction.CommitAsync();
        
        return order;
    });
    // Any database exception becomes ExceptionalError
}

// API calls with retry logic
async Task<Result<Data>> FetchDataWithRetry(string url)
{
    return await Result.TryAsync(async () =>
    {
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Data>();
    })
    .RecoverAsync(async error => 
    {
        if (error is HttpRequestException)
        {
            await Task.Delay(1000);
            return await FetchDataWithRetry(url);
        }
        return Result.Failure<Data>(error);
    });
}
```

**When to Use Try**:
- Wrapping third-party code that throws exceptions
- File I/O operations
- Network calls and API requests
- Parsing operations (JSON, XML, etc.)
- Database operations
- Any operation where exceptions are expected failure modes

**Best Practices**:
```csharp
// Good - expected exceptions become Results
Result<int> result = Result.Try(() => int.Parse(input));

// Good - specific exception handling
Result<Config> config = Result.Try(() => LoadConfig(path))
    .MapError(error => 
    {
        if (error is FileNotFoundException)
            return new NotFoundError("Config", path);
        
        return new ExceptionalError(error);
    });

// Avoid - Try for validation logic (use Ensure instead)
// Bad
Result<User> ValidateUser(User user)
{
    return Result.Try(() => 
    {
        if (user.Age < 18)
            throw new ValidationException("Must be 18+");
        return user;
    });
}

// Good
Result<User> ValidateUser(User user)
{
    return Result.Success(user)
        .Ensure(u => u.Age >= 18, 
                _ => new ValidationError(new[] { "Must be 18+" }));
}
```

**Related**: [Map](#map), [Bind](#bind), [MapError](#maperror)

---

## Validation

Extensions for validating Result values.

### Ensure

**Description**: Validates a success value against a predicate. Converts to failure if predicate fails.

**Signature**:
```csharp
Result<T> Ensure<T>(
    this Result<T> result,
    Func<T, bool> predicate,
    Func<T, IError> errorFactory)
```

**Example**:
```csharp
Result<int> ValidatePositive(int value)
{
    return Result.Success(value)
        .Ensure(
            x => x > 0,
            x => new ValidationError(new[] { $"Value {x} must be positive" })
        );
}

ValidatePositive(42);   // Success: 42
ValidatePositive(-5);   // Failure: ValidationError

// Chain multiple validations
Result<User> ValidateUser(User user)
{
    return Result.Success(user)
        .Ensure(u => !string.IsNullOrEmpty(u.Name), 
                _ => new ValidationError(new[] { "Name required" }))
        .Ensure(u => u.Age >= 18, 
                _ => new ValidationError(new[] { "Must be 18+" }))
        .Ensure(u => u.Email.Contains("@"), 
                _ => new ValidationError(new[] { "Invalid email" }));
}
```

**Related**: [EnsureNotNull](#ensurenotnull), [EnsureNotEmpty](#ensurenotempty), [EnsureAsync](#ensureasync)

---

### EnsureNotNull

**Description**: Validates that a projected property is not null.

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
Result<Order> ValidateOrder(Order order)
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
        );
}

// With null customer
var order = new Order { Customer = null };
var result = ValidateOrder(order);
// Failure: ValidationError("Customer: Customer is required")
```

**Related**: [Ensure](#ensure), [EnsureNotEmpty](#ensurenotempty)

---

### EnsureNotEmpty

**Description**: Validates that a string or collection is not empty.

**Signature**:
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

**Example**:
```csharp
Result<string> ValidateName(string name)
{
    return Result.Success(name)
        .EnsureNotEmpty("Name cannot be empty", field: "name");
}

ValidateName("Alice");  // Success: "Alice"
ValidateName("");       // Failure: ValidationError("name: Name cannot be empty")

Result<List<string>> ValidateItems(List<string> items)
{
    return Result.Success(items)
        .EnsureNotEmpty<List<string>, string>(
            "Must have at least one item",
            field: "items"
        );
}

ValidateItems(new List<string> { "item1" });  // Success
ValidateItems(new List<string>());            // Failure
```

**Related**: [Ensure](#ensure), [EnsureNotNull](#ensurenotnull)

---

## Side Effects

Extensions for performing side effects without changing Result values.

### Tap

**Description**: Executes a side effect when Result is successful. Value passes through unchanged.

**Signature**:
```csharp
Result<T> Tap<T>(this Result<T> result, Action<T> action)
```

**Example**:
```csharp
Result<User> result = GetUser(userId)
    .Tap(user => _logger.LogInfo($"Found user: {user.Name}"))
    .Tap(user => _metrics.IncrementUserLoads())
    .Tap(user => _cache.Set(userId, user));

// Value passes through: result still contains User
// Tap only runs if Result is success

// Common use cases
return ProcessOrder(order)
    .Tap(o => _logger.LogInfo($"Order {o.Id} processed"))
    .Tap(o => SendConfirmationEmail(o.Email))
    .Tap(o => _eventBus.Publish(new OrderCreated(o.Id)));
```

**Related**: [TapError](#taperror), [TapBoth](#tapboth), [TapAsync](#tapasync)

---

### TapError

**Description**: Executes a side effect when Result is a failure. Error passes through unchanged.

**Signature**:
```csharp
Result<T> TapError<T>(this Result<T> result, Action<IError> action)
```

**Example**:
```csharp
Result<User> result = GetUser(userId)
    .TapError(error => _logger.LogError($"Failed: {error.Message}"))
    .TapError(error => _metrics.IncrementFailures())
    .TapError(error => _alerting.SendAlert(error));

// Error passes through unchanged
// TapError only runs if Result is failure

// Error logging and monitoring
return ProcessPayment(payment)
    .TapError(error => _logger.LogError("Payment failed", error.Metadata))
    .TapError(error => _monitoring.ReportError(error.Code))
    .TapError(error => 
    {
        if (error is ExceptionalError ee)
            _sentry.CaptureException(ee.Exception);
    });
```

**Related**: [Tap](#tap), [TapBoth](#tapboth), [TapErrorAsync](#taperrorasync)

---

### TapBoth

**Description**: Executes side effects for both success and failure cases.

**Signature**:
```csharp
Result<T> TapBoth<T>(
    this Result<T> result,
    Action<T> onSuccess,
    Action<Exception> onFailure)
```

**Example**:
```csharp
Result<Order> result = ProcessOrder(order)
    .TapBoth(
        onSuccess: o => _logger.LogInfo($"Order {o.Id} processed successfully"),
        onFailure: error => _logger.LogError($"Order processing failed: {error.Message}")
    )
    .TapBoth(
        onSuccess: o => _metrics.RecordSuccess(),
        onFailure: error => _metrics.RecordFailure(error.Message)
    );

// Useful for consistent logging/metrics
return SaveUser(user)
    .TapBoth(
        onSuccess: u => _audit.Log($"User {u.Id} saved"),
        onFailure: ex => _audit.Log($"Save failed: {ex.Message}")
    );
```

**Related**: [Tap](#tap), [TapError](#taperror), [Match](#match)

---

## Error Handling

Extensions for inspecting, transforming, and recovering from errors.

### MapError

**Description**: Transforms the error of a failed Result. Success values pass through unchanged.

**Signature**:
```csharp
Result<T> MapError<T>(this Result<T> result, Func<Exception, Exception> mapper)
```

**Example**:
```csharp
Result<User> AddContext(Result<User> result, string userId)
{
    return result.MapError(error => 
        new Exception($"Failed to load user {userId}: {error.Message}", error)
    );
}

// Add structured error information
return GetUser(userId)
    .MapError(error => new ExceptionalError(
        error,
        $"User retrieval failed",
        new Dictionary<string, object?> 
        { 
            ["userId"] = userId,
            ["timestamp"] = DateTime.UtcNow 
        }
    ));
```

**Related**: [PrependError](#prependerror), [AppendError](#appenderror), [MapErrorAsync](#maperrorasync)

---

### PrependError

**Description**: Adds a prefix to the error message.

**Signature**:
```csharp
Result<T> PrependError<T>(this Result<T> result, string prefix)
```

**Example**:
```csharp
Result<Order> GetOrder(string userId, int orderId)
{
    return LoadOrder(orderId)
        .PrependError($"Failed to load order for user {userId}: ");
}

// Original error: "Order not found"
// After prepend: "Failed to load order for user ABC: Order not found"
```

**Related**: [AppendError](#appenderror), [MapError](#maperror), [WithContext](#withcontext)

---

### AppendError

**Description**: Adds a suffix to the error message.

**Signature**:
```csharp
Result<T> AppendError<T>(this Result<T> result, string suffix)
```

**Example**:
```csharp
Result<Data> LoadData(string id)
{
    return FetchData(id)
        .AppendError($" (Attempted at {DateTime.UtcNow})");
}

// Original error: "Data not found"
// After append: "Data not found (Attempted at 2025-10-22 10:30:00)"
```

**Related**: [PrependError](#prependerror), [MapError](#maperror)

---

### WithContext

**Description**: Alias for `PrependError`. Adds contextual information to errors.

**Signature**:
```csharp
Result<T> WithContext<T>(this Result<T> result, string context)
```

**Example**:
```csharp
Result<User> GetUserWithContext(string userId)
{
    return GetUser(userId)
        .WithContext($"GetUserWithContext(userId: {userId})");
}

// Original error: "User not found"
// With context: "GetUserWithContext(userId: 123): User not found"
```

**Related**: [PrependError](#prependerror), [AppendError](#appenderror)

---

### HasError

**Description**: Checks if a Result contains a specific error type.

**Signature**:
```csharp
bool HasError<TError>(this Result result) where TError : IError
```

**Example**:
```csharp
Result<User> result = GetUser(userId);

if (result.HasError<NotFoundError>())
{
    return NotFound("User not found");
}

if (result.HasError<ValidationError>())
{
    return BadRequest("Validation failed");
}

if (result.HasError<UnauthorizedError>())
{
    return Unauthorized();
}
```

**Related**: [HasException](#hasexception), [FindError](#finderror), [MatchError](#matcherror)

---

### HasException

**Description**: Checks if a Result contains an exception of a specific type.

**Signature**:
```csharp
bool HasException<TException>(this Result result) where TException : Exception
```

**Example**:
```csharp
Result<Config> config = LoadConfig(path);

if (config.HasException<FileNotFoundException>())
{
    Console.WriteLine("Config file not found, using defaults");
    return UseDefaultConfig();
}

if (config.HasException<JsonException>())
{
    Console.WriteLine("Config file is malformed");
    return Result.Failure<Config>("Invalid configuration format");
}
```

**Related**: [HasError](#haserror), [FindError](#finderror)

---

### FindError

**Description**: Finds the first error matching a predicate.

**Signature**:
```csharp
IError? FindError(this Result result, Func<IError, bool> predicate)
```

**Example**:
```csharp
Result<User> result = GetUser(userId);

// Find specific error
IError? validationError = result.FindError(e => e.Code == "VALIDATION");

if (validationError is ValidationError ve)
{
    foreach (var failure in ve.Failures)
    {
        Console.WriteLine($"Validation error: {failure}");
    }
}

// Find by metadata
IError? fieldError = result.FindError(e => 
    e.Metadata.ContainsKey("field") && 
    e.Metadata["field"]?.ToString() == "email"
);
```

**Related**: [TryPickError](#trypickerror), [HasError](#haserror), [MatchError](#matcherror)

---

### TryPickError

**Description**: Attempts to find an error matching a predicate, using an out parameter.

**Signature**:
```csharp
bool TryPickError(
    this Result result, 
    Func<IError, bool> predicate, 
    out IError? error)
```

**Example**:
```csharp
Result<User> result = GetUser(userId);

if (result.TryPickError(e => e is ValidationError, out var error))
{
    var validationError = (ValidationError)error!;
    return BadRequest(new 
    { 
        errors = validationError.Failures 
    });
}

// Pattern matching style
if (result.TryPickError(e => e.Code == "NOT_FOUND", out var notFound))
{
    _logger.LogWarning($"Resource not found: {notFound.Message}");
    return NotFound();
}
```

**Related**: [FindError](#finderror), [HasError](#haserror)

---

### MatchError

**Description**: Pattern matches on a specific error type and transforms the Result.

**Signature**:
```csharp
TOut MatchError<TError, TOut>(
    this Result result,
    Func<TError, TOut> onMatch,
    Func<TOut> onElse) where TError : class, IError
```

**Example**:
```csharp
IActionResult HandleResult<T>(Result<T> result)
{
    return result.MatchError<NotFoundError, IActionResult>(
        onMatch: notFound => NotFound(notFound.Message),
        onElse: () => result.MatchError<ValidationError, IActionResult>(
            onMatch: validation => BadRequest(validation.Failures),
            onElse: () => result.Match(
                success: value => Ok(value),
                failure: error => StatusCode(500, error.Message)
            )
        )
    );
}

// Simpler error-specific handling
string message = result.MatchError<ValidationError, string>(
    onMatch: ve => $"Validation failed: {string.Join(", ", ve.Failures)}",
    onElse: () => result.Match(
        success: _ => "Success",
        failure: e => $"Error: {e.Message}"
    )
);
```

**Related**: [Match](#match), [FindError](#finderror), [HasError](#haserror)

---

### FilterError

**Description**: Filters errors based on a predicate. Removes errors that don't match, potentially converting failure to success.

**Signature**:
```csharp
Result FilterError(this Result result, Func<IError, bool> predicate)
```

**Example**:
```csharp
// Keep only critical errors
Result result = Operation()
    .FilterError(e => e.Code == "CRITICAL" || e is ExceptionalError);

// Remove temporary/retriable errors
Result<Data> data = LoadData()
    .FilterError(e => e.Code != "TIMEOUT" && e.Code != "RETRY");

// If all errors are filtered out, Result becomes success
Result operation = PerformOperation()
    .FilterError(e => e is ValidationError);
// If only warnings (non-ValidationError) existed, this becomes Success
```

**Related**: [FindError](#finderror), [HasError](#haserror)

---

### Recover

**Description**: Converts a failed Result into a success by providing a fallback value. The fallback can be computed from the error or provided directly. This method always returns a success Result.

**Signatures**:
```csharp
// With error-dependent fallback
Result<T> Recover<T>(this Result<T> result, Func<Exception, T> recovery)

// With constant fallback
Result<T> Recover<T>(this Result<T> result, T fallback)

// Multiple value Results (2-8 values supported)
Result<T1, T2> Recover<T1, T2>(
    this Result<T1, T2> result, 
    Func<Exception, (T1, T2)> recovery)

Result<T1, T2> Recover<T1, T2>(
    this Result<T1, T2> result, 
    T1 fallback1, 
    T2 fallback2)
```

**Examples**:

**Basic Recovery**:
```csharp
// Simple constant fallback
Result<int> result = GetValue()
    .Recover(0);  // If failed, becomes Success(0)

// Computed fallback
Result<int> withDefault = GetValue()
    .Recover(error => {
        _logger.LogWarning($"Using default: {error.Message}");
        return 0;
    });
```

**Conditional Recovery**:
```csharp
// Recover only specific errors
Result<User> user = GetUser(userId)
    .Recover(error => 
    {
        if (error is NotFoundException)
            return User.Guest;
        
        // Re-throw unexpected errors
        throw new InvalidOperationException("Cannot recover", error);
    });

// Pattern-based recovery
Result<Config> config = LoadConfig(path)
    .Recover(error => error switch
    {
        FileNotFoundException => Config.Default,
        JsonException => Config.Empty,
        _ => throw error
    });
```

**Graceful Degradation**:
```csharp
// Provide degraded functionality on failure
Result<UserProfile> profile = LoadFullProfile(userId)
    .Recover(error => 
    {
        _metrics.RecordProfileLoadFailure();
        return UserProfile.Minimal(userId);
    });

// Cache with fallback to default
Result<Settings> settings = LoadFromCache("settings")
    .Recover(_ => Settings.Default);

// API with default response
Result<WeatherData> weather = FetchWeather(city)
    .Recover(error => 
    {
        _logger.LogWarning($"Weather API failed: {error.Message}");
        return WeatherData.Unavailable;
    });
```

**Multiple Values**:
```csharp
// Recover Result with multiple values
Result<string, int> nameAndAge = GetUserInfo(userId)
    .Recover(error => 
    {
        _logger.LogError($"Failed to get user: {error.Message}");
        return ("Unknown", 0);
    });

// Constant fallback for multiple values
Result<string, int> withDefaults = GetUserInfo(userId)
    .Recover("Guest", 18);

// Three values
Result<string, string, int> fullInfo = GetCompleteUserInfo(userId)
    .Recover(
        fallback1: "Unknown",
        fallback2: "guest@example.com", 
        fallback3: 0
    );
```

**Real-World Patterns**:
```csharp
// Feature toggle with fallback
Result<bool> isFeatureEnabled = GetFeatureFlag("new-ui")
    .Recover(false);  // Default to disabled

// Quota with fallback to limits
Result<int> userQuota = LoadUserQuota(userId)
    .Recover(error => 
    {
        if (error is NotFoundException)
            return 100;  // Default quota
        if (error is TimeoutException)
            return 50;   // Conservative quota on timeout
        throw error;
    });

// Configuration chain with recovery
Result<int> timeout = LoadFromEnvironment("TIMEOUT")
    .Bind(ParseInt)
    .Recover(error => 
    {
        _logger.LogInfo($"Using default timeout: {error.Message}");
        return 30;
    });

// Data pipeline with fallback
Result<ReportData> report = LoadFromPrimarySource()
    .Tap(data => _cache.Set("report", data))
    .Recover(error => 
    {
        _alerting.NotifyPrimarySourceFailure(error);
        return ReportData.EmptyWithError(error.Message);
    });
```

**When to Use Recover**:
- Providing default values when operations fail
- Implementing graceful degradation
- Converting recoverable errors to success states
- Feature flags with safe defaults
- Fallback configurations

**When NOT to Use Recover**:
- When you need to try alternative operations (use `Recover` instead)
- When you want to preserve failure state (use `ValueOr` instead)
- When error information needs to be propagated

**Difference from ValueOr**:
```csharp
// Recover returns a Result (always success)
Result<int> recovered = GetValue()
    .Recover(0);  // Result<int> - Success(0)

// ValueOr returns the unwrapped value
int value = GetValue()
    .ValueOr(0);  // int - 0
```

**Related**: [Recover](#recover), [ValueOr](#valueor), [RecoverAsync](#recoverasync)

---

## Collections

Extensions for working with collections of Results.

### Traverse

**Description**: Applies a Result-returning function to each item in a collection and collects results. Short-circuits on first failure.

**Signature**:
```csharp
Result<List<TOut>> Traverse<TIn, TOut>(
    this IEnumerable<TIn> source,
    Func<TIn, Result<TOut>> selector)
```

**Example**:
```csharp
List<string> userIds = new() { "1", "2", "3" };

// Load all users
Result<List<User>> users = userIds.Traverse(id => GetUser(id));
// Success if all users found, Failure on first missing user

// Validate multiple values
List<string> emails = new() { "alice@ex.com", "bob@ex.com" };
Result<List<Email>> validEmails = emails.Traverse(email => 
    ValidateEmail(email)
);

// Parse collection
Result<List<int>> numbers = new[] { "1", "2", "3" }.Traverse(s =>
    int.TryParse(s, out var n)
        ? Result.Success(n)
        : Result.Failure<int>($"Invalid: {s}")
);
```

**Related**: [TraverseAsync](#traverseasync), [Sequence](#sequence), [Combine](#combine)

---

### Sequence

**Description**: Converts `IEnumerable<Result<T>>` into `Result<List<T>>`. Collects all successes or returns first failure.

**Signature**:
```csharp
Result<List<T>> Sequence<T>(this IEnumerable<Result<T>> source)
```

**Example**:
```csharp
List<Result<int>> results = new()
{
    Result.Success(1),
    Result.Success(2),
    Result.Success(3)
};

Result<List<int>> combined = results.Sequence();
// Success: [1, 2, 3]

// With failure
List<Result<int>> mixedResults = new()
{
    Result.Success(1),
    Result.Failure<int>("Error"),
    Result.Success(3)
};

Result<List<int>> failed = mixedResults.Sequence();
// Failure: "Error"

// Common pattern
Result<List<User>> users = userIds
    .Map(id => GetUser(id))  // IEnumerable<Result<User>>
    .ToList()
    .Sequence();  // Result<List<User>>
```

**Related**: [Traverse](#traverse), [Combine](#combine)

---

### Combine

**Description**: Combines multiple Results into a single Result with a list. Collects all errors if any fail.

**Signature**:
```csharp
Result<List<T>> Combine<T>(params Result<T>[] results)
```

**Example**:
```csharp
Result<int> r1 = Result.Success(1);
Result<int> r2 = Result.Success(2);
Result<int> r3 = Result.Success(3);

Result<List<int>> combined = Result.Combine(r1, r2, r3);
// Success: [1, 2, 3]

// With failures - collects ALL errors
Result<int> f1 = Result.Success(1);
Result<int> f2 = Result.Failure<int>("Error 1");
Result<int> f3 = Result.Failure<int>("Error 2");

Result<List<int>> failed = Result.Combine(f1, f2, f3);
// Failure with both "Error 1" and "Error 2"
```

**Related**: [Traverse](#traverse), [Sequence](#sequence), [Zip](#zip)

---

### Apply

**Description**: Combines Results using a function (applicative functor pattern). Collects all errors.

**Signature**:
```csharp
Result<TOut> Apply<T1, T2, TOut>(
    this Result<T1> r1,
    Result<T2> r2,
    Func<T1, T2, TOut> projector)
```

**Example**:
```csharp
// Combine two Results
Result<int> width = GetWidth();
Result<int> height = GetHeight();

Result<int> area = width.Apply(height, (w, h) => w * h);
// Success if both succeed

// Validate form fields independently
Result<string> validName = ValidateName(form.Name);
Result<string> validEmail = ValidateEmail(form.Email);
Result<int> validAge = ValidateAge(form.Age);

// Combine all validations
Result<User> user = validName.Apply(
    validEmail, 
    validAge,
    (name, email, age) => new User(name, email, age)
);
// Collects ALL validation errors if any fail
```

**Related**: [Zip](#zip), [Combine](#combine)

---

## Value Access

Extensions for extracting values from Results.

### ValueOr

**Description**: Extracts the value if successful, otherwise returns a fallback.

**Signature**:
```csharp
T ValueOr<T>(this Result<T> result, T defaultValue)
T ValueOr<T>(this Result<T> result, Func<T> defaultFactory)
```

**Example**:
```csharp
Result<int> result = GetValue();

// Simple fallback
int value = result.ValueOr(0);

// Lazy fallback (computed only if needed)
int value2 = result.ValueOr(() => ExpensiveDefault());

// Configuration with defaults
var timeout = LoadConfig("timeout")
    .Map(c => c.TimeoutSeconds)
    .ValueOr(30);

// Chained fallbacks
var data = LoadFromCache(key)
    .ValueOr(() => LoadFromDatabase(key)
        .ValueOr(DefaultData));
```

**Related**: [ValueOrThrow](#valueorthrow), [ToNullable](#tonullable), [Match](#match)

---

### ValueOrThrow

**Description**: Extracts the value if successful, otherwise throws an exception.

**Signature**:
```csharp
T ValueOrThrow<T>(this Result<T> result)
T ValueOrThrow<T>(this Result<T> result, Func<Exception, Exception> exceptionFactory)
```

**Example**:
```csharp
// Throw the Result's exception
User user = GetUser(userId).ValueOrThrow();

// Custom exception
User user2 = GetUser(userId).ValueOrThrow(error =>
    new ApplicationException($"Failed to get user: {error.Message}")
);

// In ASP.NET Core controllers
public IActionResult GetUser(string id)
{
    try
    {
        var user = _service.GetUser(id).ValueOrThrow();
        return Ok(user);
    }
    catch (Exception ex)
    {
        return StatusCode(500, ex.Message);
    }
}

// In tests
[Fact]
public void Should_Return_User()
{
    var user = service.GetUser("123").ValueOrThrow();
    Assert.Equal("Alice", user.Name);
}
```

**Related**: [ValueOr](#valueor), [TryGet](#tryget)

---

### TryGet

**Description**: Safely extracts value and error using out parameters.

**Signature**:
```csharp
bool TryGet<T>(this Result<T> result, out T value, out Exception? error)
```

**Example**:
```csharp
Result<User> result = GetUser(userId);

if (result.TryGet(out var user, out var error))
{
    Console.WriteLine($"User: {user.Name}");
}
else
{
    Console.WriteLine($"Error: {error.Message}");
}

// With pattern matching
if (GetUser(userId).TryGet(out var user, out _))
{
    ProcessUser(user);
}
else
{
    _logger.LogError("User not found");
}
```

**Related**: [Ok](#ok), [ValueOr](#valueor), [Deconstruct](#deconstruct)

---

### ToNullable

**Description**: Converts Result to a nullable value.

**Signature**:
```csharp
T? ToNullable<T>(this Result<T> result)
```

**Example**:
```csharp
Result<int> result = GetValue();

int? nullable = result.ToNullable();
// Success: nullable has value
// Failure: nullable is null

// With reference types
User? user = GetUser(id).ToNullable();

// Optional chaining
string? city = GetUser(userId)
    .ToNullable()
    ?.Address
    ?.City;

// Interop with nullable-aware code
string? GetUserName(string userId) =>
    GetUser(userId)
        .Map(u => u.Name)
        .ToNullable();
```

**Related**: [ValueOr](#valueor), [TryGet](#tryget)

---

### Match

**Description**: Transforms Result to any type by providing success and failure handlers.

**Signature**:
```csharp
TOut Match<T, TOut>(
    this Result<T> result,
    Func<T, TOut> success,
    Func<Exception, TOut> failure)
```

**Example**:
```csharp
// Transform to string
string message = result.Match(
    success: user => $"Hello, {user.Name}!",
    failure: error => $"Error: {error.Message}"
);

// Transform to HTTP response
IActionResult response = result.Match(
    success: user => Ok(user),
    failure: error => error switch
    {
        NotFoundError => NotFound(error.Message),
        UnauthorizedError => Unauthorized(error.Message),
        ValidationError => BadRequest(error.Message),
        _ => StatusCode(500, error.Message)
    }
);

// Compute value
int score = GetScore(userId).Match(
    success: s => s,
    failure: _ => 0
);
```

**Related**: [MatchError](#matcherror), [ValueOr](#valueor), [TryGet](#tryget)

---

## Async Operations

Extensions for working with asynchronous Results.

### MapAsync

**Description**: Transforms Result values using async functions.

**Signature**:
```csharp
Task<Result<TOut>> MapAsync<T, TOut>(
    this Task<Result<T>> resultTask,
    Func<T, Task<TOut>> asyncMapper)
```

**Example**:
```csharp
Task<Result<UserDto>> GetUserDtoAsync(string userId)
{
    return GetUserAsync(userId)
        .MapAsync(async user => 
        {
            var settings = await LoadSettingsAsync(user.Id);
            return new UserDto(user, settings);
        });
}

// Chain multiple async maps
Task<Result<string>> ProcessAsync(string input)
{
    return Result.Success(input)
        .MapAsync(async s => await NormalizeAsync(s))
        .MapAsync(async s => await EnrichAsync(s))
        .MapAsync(async s => await FormatAsync(s));
}
```

**Related**: [Map](#map), [BindAsync](#bindasync)

---

### BindAsync

**Description**: Chains async operations that return Results.

**Signature**:
```csharp
Task<Result<TOut>> BindAsync<T, TOut>(
    this Task<Result<T>> resultTask,
    Func<T, Task<Result<TOut>>> asyncBinder)
```

**Example**:
```csharp
async Task<Result<Order>> PlaceOrderAsync(string userId, List<int> productIds)
{
    return await GetUserAsync(userId)
        .BindAsync(async user => await ValidateUserAsync(user))
        .BindAsync(async user => await GetProductsAsync(productIds))
        .BindAsync(async products => await CreateOrderAsync(products))
        .BindAsync(async order => await ProcessPaymentAsync(order));
}
```

**Related**: [Bind](#bind), [MapAsync](#mapasync)

---

### EnsureAsync

**Description**: Validates with async predicates.

**Signature**:
```csharp
Task<Result<T>> EnsureAsync<T>(
    this Task<Result<T>> resultTask,
    Func<T, Task<bool>> asyncPredicate,
    Func<T, IError> errorFactory)
```

**Example**:
```csharp
async Task<Result<User>> ValidateUniqueEmailAsync(User user)
{
    return await Result.Success(user)
        .EnsureAsync(
            async u => !await _db.EmailExistsAsync(u.Email),
            u => new ConflictError($"Email {u.Email} already exists")
        );
}
```

**Related**: [Ensure](#ensure), [BindAsync](#bindasync)

---

### TapAsync

**Description**: Executes async side effects.

**Signature**:
```csharp
Task<Result<T>> TapAsync<T>(
    this Task<Result<T>> resultTask,
    Func<T, Task> asyncAction)
```

**Example**:
```csharp
Task<Result<Order>> CreateOrderAsync(Order order)
{
    return Result.Success(order)
        .BindAsync(async o => await SaveOrderAsync(o))
        .TapAsync(async o => await SendConfirmationEmailAsync(o))
        .TapAsync(async o => await UpdateInventoryAsync(o))
        .TapAsync(async o => await _cache.InvalidateAsync($"orders:{o.UserId}"));
}
```

**Related**: [Tap](#tap), [TapErrorAsync](#taperrorasync)

---

### TapErrorAsync

**Description**: Executes async side effects on failure.

**Signature**:
```csharp
Task<Result<T>> TapErrorAsync<T>(
    this Task<Result<T>> resultTask,
    Func<IError, Task> asyncAction)
```

**Example**:
```csharp
Task<Result<Data>> LoadDataAsync(string id)
{
    return FetchDataAsync(id)
        .TapErrorAsync(async error => 
            await _errorLog.SaveAsync(new ErrorEntry
            {
                Message = error.Message,
                Code = error.Code,
                Timestamp = DateTime.UtcNow
            })
        );
}
```

**Related**: [TapError](#taperror), [TapAsync](#tapasync)

---

### TraverseAsync

**Description**: Processes collections with async operations.

**Signature**:
```csharp
Task<Result<List<TOut>>> TraverseAsync<TIn, TOut>(
    this IEnumerable<TIn> source,
    Func<TIn, Task<Result<TOut>>> asyncSelector)
```

**Example**:
```csharp
List<string> userIds = new() { "1", "2", "3" };

Task<Result<List<User>>> users = userIds.TraverseAsync(async id =>
    await GetUserAsync(id)
);
// Processes all items concurrently
```

**Related**: [Traverse](#traverse), [BindAsync](#bindasync)

---

### RecoverAsync

**Description**: Provides async alternative when Result fails.

**Signature**:
```csharp
Task<Result<T>> RecoverAsync<T>(
    this Task<Result<T>> resultTask,
    Func<Exception, Task<Result<T>>> asyncRecovery)
```

**Example**:
```csharp
Task<Result<Data>> LoadDataAsync(string id)
{
    return LoadFromCacheAsync(id)
        .RecoverAsync(async error => await LoadFromDatabaseAsync(id))
        .RecoverAsync(async error => await LoadFromBackupAsync(id));
}
```

**Related**: [Recover](#recover), [BindAsync](#bindasync)

---

## Advanced Operations

### Ok

**Description**: Extracts value and error using out parameters. Returns true if successful.

**Signature**:
```csharp
bool Ok<T>(this Result<T> result, out T value, out Exception? error)
```

**Example**:
```csharp
if (result.Ok(out var value, out var error))
{
    // Success path
    Console.WriteLine($"Value: {value}");
}
else
{
    // Failure path
    Console.WriteLine($"Error: {error.Message}");
}
```

**Related**: [TryGet](#tryget), [Match](#match)

---

### Deconstruct

**Description**: Deconstructs Result into tuple for pattern matching.

**Signature**:
```csharp
void Deconstruct<T>(this Result<T> result, out bool isSuccess, out T value, out Exception? error)
```

**Example**:
```csharp
var (ok, user, error) = GetUser(userId);

if (ok)
{
    ProcessUser(user);
}
else
{
    LogError(error);
}

// With pattern matching
return GetUser(userId) switch
{
    var (true, user, _) => Ok(user),
    var (false, _, error) => HandleError(error)
};
```

**Related**: [Ok](#ok), [TryGet](#tryget), [Match](#match)

---

## Summary

This reference covers all major Result extension methods. For detailed usage guides, see:

- **[Transformations](transformations.html)** - Detailed Map, Bind, Flatten guides
- **[Validation](validation.html)** - Ensure patterns and validation strategies
- **[Error Handling](error-handling.html)** - Error model and recovery patterns
- **[Async Operations](async.html)** - Async/await integration
- **[Collections](collections.html)** - Batch processing patterns
- **[Value Access](value-access.html)** - Safe value extraction patterns

Return to **[Result Overview](index.html)** for conceptual introduction.
