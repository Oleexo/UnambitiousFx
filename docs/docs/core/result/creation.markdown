---
title: Creation & Basics
parent: Result
nav_order: 1
---

# Result Creation & Basics
{: .no_toc }

## Table of contents
{: .no_toc .text-delta }

1. TOC
{:toc}

---

## Creating Results

### Success Results

Creating successful results is straightforward - wrap your value(s) with `Result.Success()`:

```csharp
// Non-generic Result (success without a value)
Result ok = Result.Success();

// Result<T> - success with a single value
Result<int> age = Result.Success(42);
Result<string> name = Result.Success("Alice");

// Type inference works naturally
var score = Result.Success(95.5); // Result<double>

// Explicit type when needed
var result = Result.Success<User>(new User("Bob"));

// Multiple values (tuple results)
Result<string, int> nameAndAge = Result.Success("Alice", 30);
Result<int, int, int> coordinates = Result.Success(10, 20, 30);
```

#### Why Non-Generic Result?

Use `Result` (without type parameter) for operations that:
- Indicate success/failure but don't return a value
- Perform side effects (save to database, send email, etc.)
- Validate conditions

```csharp
Result SaveUser(User user)
{
    try
    {
        _database.Save(user);
        return Result.Success();
    }
    catch (Exception ex)
    {
        return Result.Failure(ex);
    }
}

Result ValidateAge(int age)
{
    return age >= 18 
        ? Result.Success() 
        : Result.Failure("Must be 18 or older");
}
```

### Failure Results

Create failures from exceptions or error messages:

```csharp
// From exception
Result failure1 = Result.Failure(new InvalidOperationException("Database unavailable"));
Result<int> failure2 = Result.Failure<int>(new ArgumentException("Invalid input"));

// From error message (creates an exception internally)
Result failure3 = Result.Failure("Operation failed");
Result<string> failure4 = Result.Failure<string>("User not found");

// From domain errors (recommended approach)
Result<User> failure5 = Result.Failure<User>(
    new NotFoundError("User with ID 123 not found")
);

Result<decimal> failure6 = Result.Failure<decimal>(
    new ValidationError("Amount must be positive")
        .WithMetadata("field", "amount")
        .WithMetadata("value", -100)
);
```

### Factory Methods for Common Patterns

#### From Try/Catch

Wrap potentially throwing code in Result:

```csharp
// Synchronous
Result<int> ParseNumber(string input)
{
    return Result.Try(() => int.Parse(input));
}

// Async
async Task<Result<User>> LoadUserAsync(string id)
{
    return await Result.TryAsync(async () => 
    {
        var user = await _database.GetUserAsync(id);
        if (user == null)
            throw new NotFoundException($"User {id} not found");
        return user;
    });
}
```

**How Try Works:**
- Executes the function
- Returns `Result.Success(value)` if no exception
- Returns `Result.Failure(exception)` if exception thrown

#### From Task

Convert existing Task-returning methods:

```csharp
// If the task returns a value
Task<User> GetUserFromApiAsync(string id);

Result<User> result = await Result.FromTask(GetUserFromApiAsync("123"));

// The task's exception (if any) becomes a failure
```

#### Conditional Results

Create results based on conditions:

```csharp
Result<User> GetAdmin(User user)
{
    // Using ternary
    return user.IsAdmin 
        ? Result.Success(user)
        : Result.Failure<User>(new UnauthorizedError("User is not an admin"));
}

// With helper (if available in your version)
Result<int> GetPositive(int value)
{
    return value > 0
        ? Result.Success(value)
        : Result.Failure<int>(new ValidationError("Value must be positive"));
}
```

---

## Inspecting Results

### Status Properties

Every Result has two boolean properties:

```csharp
Result<int> result = GetValue();

if (result.IsSuccess)
{
    // Result succeeded - safe to access value
}

if (result.IsFaulted)
{
    // Result failed - has error information
}

// They are always opposites
Debug.Assert(result.IsSuccess != result.IsFaulted);
```

### The Ok Pattern

The `Ok()` method provides safe extraction with pattern matching:

```csharp
// Result<T> - extracts both value and error
Result<int> result = ParseNumber("42");

if (result.Ok(out var value, out var error))
{
    Console.WriteLine($"Success: {value}");
    // value is int
}
else
{
    Console.WriteLine($"Failure: {error.Message}");
    // error is Exception
}

// Non-generic Result - only extracts error
Result operation = SaveToDatabase();

if (operation.Ok(out var error))
{
    Console.WriteLine("Operation succeeded");
}
else
{
    Console.WriteLine($"Operation failed: {error.Message}");
}
```

### Pattern Matching with Match

The `Match` method executes different actions based on success/failure:

```csharp
// Action-based (side effects)
result.Match(
    success: value => Console.WriteLine($"Got value: {value}"),
    failure: error => Console.WriteLine($"Got error: {error.Message}")
);

// Value-returning (transforming)
string message = result.Match(
    success: value => $"Success: {value}",
    failure: error => $"Error: {error.Message}"
);

// Non-generic Result
operation.Match(
    success: () => Console.WriteLine("Operation completed"),
    failure: error => Console.WriteLine($"Operation failed: {error.Message}")
);
```

### Conditional Actions

Execute code only on success or failure:

```csharp
result.IfSuccess(() => 
{
    Console.WriteLine("Success!");
    _metrics.IncrementSuccess();
});

result.IfFailure(error => 
{
    Console.WriteLine($"Failed: {error.Message}");
    _logger.LogError(error);
});

// With Result<T>, access the value
result.IfSuccess(value => 
{
    Console.WriteLine($"Value is {value}");
});
```

---

## Working with Errors

### Checking Error Types

```csharp
// Check if result has a specific error type
if (result.HasError<ValidationError>())
{
    // Handle validation failure
}

// Check for exception type
if (result.HasException<ArgumentNullException>())
{
    // Handle null argument
}
```

### Multiple Values (Tuples)

Results can carry multiple success values:

```csharp
// Create
Result<string, int, bool> parseResult = ParseUserData("Alice,30,true");

// Extract with Ok
if (parseResult.Ok(out var name, out var age, out var active, out var error))
{
    Console.WriteLine($"{name} is {age} years old, active: {active}");
}
else
{
    Console.WriteLine($"Parse failed: {error.Message}");
}

// Transform (Map works with tuples)
Result<string, int, bool> data = Result.Success("Bob", 25, false);

var formatted = data.Map((name, age, active) => 
    $"{name} ({age}) - {(active ? "Active" : "Inactive")}"
);
// formatted is Result<string>
```

---

## Real-World Examples

### Example 1: User Registration

```csharp
using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Reasons;

public class UserService
{
    Result<User> RegisterUser(string email, string password)
    {
        // Validate email
        if (string.IsNullOrWhiteSpace(email))
            return Result.Failure<User>(
                new ValidationError("Email is required")
                    .WithMetadata("field", "email")
            );
        
        // Validate password
        if (password.Length < 8)
            return Result.Failure<User>(
                new ValidationError("Password must be at least 8 characters")
                    .WithMetadata("field", "password")
                    .WithMetadata("minLength", 8)
            );
        
        // Check if user exists
        var existing = _repository.FindByEmail(email);
        if (existing != null)
            return Result.Failure<User>(
                new ConflictError($"User with email {email} already exists")
                    .WithMetadata("email", email)
            );
        
        // Create user (wrapped in try)
        return Result.Try(() => 
        {
            var user = new User(email, HashPassword(password));
            _repository.Save(user);
            return user;
        });
    }
}

// Usage
var result = service.RegisterUser("alice@example.com", "secret123");

result.Match(
    success: user => Console.WriteLine($"Registered: {user.Email}"),
    failure: error => Console.WriteLine($"Registration failed: {error.Message}")
);
```

### Example 2: Configuration Loading

```csharp
public class ConfigLoader
{
    Result<AppConfig> LoadConfig(string path)
    {
        // Check file exists
        if (!File.Exists(path))
            return Result.Failure<AppConfig>(
                new NotFoundError($"Config file not found: {path}")
                    .WithMetadata("path", path)
            );
        
        // Read and parse (exception-safe)
        return Result.Try(() => 
        {
            var json = File.ReadAllText(path);
            var config = JsonSerializer.Deserialize<AppConfig>(json);
            
            if (config == null)
                throw new InvalidOperationException("Config deserialized to null");
            
            return config;
        });
    }
}

// Usage
var configResult = loader.LoadConfig("appsettings.json");

var config = configResult.ValueOr(AppConfig.Default);
// Uses loaded config if successful, otherwise uses default
```

### Example 3: Database Operation

```csharp
public class OrderRepository
{
    Result SaveOrder(Order order)
    {
        // Non-generic Result - we just want success/failure
        return Result.Try(() => 
        {
            _dbContext.Orders.Add(order);
            _dbContext.SaveChanges();
        });
    }
    
    Result<Order> GetOrder(int orderId)
    {
        var order = _dbContext.Orders.Find(orderId);
        
        return order != null
            ? Result.Success(order)
            : Result.Failure<Order>(
                new NotFoundError($"Order {orderId} not found")
                    .WithMetadata("orderId", orderId)
              );
    }
}

// Usage
var saveResult = repository.SaveOrder(newOrder);

if (saveResult.IsSuccess)
{
    Console.WriteLine("Order saved successfully");
}
else
{
    saveResult.IfFailure(error => _logger.LogError(error));
}
```

---

## Best Practices

### ✅ Do

**Use domain-specific error types:**
```csharp
// Good - clear, structured error
return Result.Failure<User>(
    new NotFoundError($"User {id} not found")
        .WithMetadata("userId", id)
);

// Avoid - generic exception
return Result.Failure<User>(new Exception("User not found"));
```

**Validate early:**
```csharp
Result<User> CreateUser(string name, int age)
{
    // Validate at entry
    if (string.IsNullOrEmpty(name))
        return Result.Failure<User>(new ValidationError("Name required"));
    
    if (age < 0)
        return Result.Failure<User>(new ValidationError("Age cannot be negative"));
    
    // Main logic
    return Result.Success(new User(name, age));
}
```

**Add metadata for context:**
```csharp
return Result.Failure<Order>(
    new ValidationError("Invalid order total")
        .WithMetadata("orderId", order.Id)
        .WithMetadata("total", order.Total)
        .WithMetadata("expectedMin", 0)
);
```

### ❌ Don't

**Don't throw exceptions for expected failures:**
```csharp
// Bad - throwing for control flow
Result<User> FindUser(string id)
{
    var user = _db.Find(id);
    if (user == null)
        throw new NotFoundException(); // Don't do this!
    return Result.Success(user);
}

// Good - return failure Result
Result<User> FindUser(string id)
{
    var user = _db.Find(id);
    return user != null
        ? Result.Success(user)
        : Result.Failure<User>(new NotFoundError($"User {id} not found"));
}
```

**Don't ignore Result values:**
```csharp
// Bad - ignoring the Result
SaveUser(user); // What if it failed?

// Good - handle the Result
var result = SaveUser(user);
if (result.IsFaulted)
{
    _logger.LogError(result.Match(
        success: () => "",
        failure: e => e.Message
    ));
}
```

**Don't use Result for truly exceptional cases:**
```csharp
// Bad - out of memory is exceptional, not expected
Result<byte[]> AllocateBuffer(int size)
{
    return Result.Try(() => new byte[size]); // OutOfMemoryException should crash
}

// Good - use Result for expected failures
Result<byte[]> ReadFile(string path)
{
    return Result.Try(() => File.ReadAllBytes(path)); // File might not exist
}
```

---

## Next Steps

Now that you understand Result creation and basic inspection, explore:

- **[Transformations](transformations.html)** - Map, Bind, Flatten, SelectMany for chaining operations
- **[Error Handling](error-handling.html)** - Deep dive into error model, domain errors, and recovery
- **[Validation](validation.html)** - Ensure and side-effects with Tap

Or return to the **[Result Overview](index.html)** to see all available features.
