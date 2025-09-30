---
title: Value Access & Interop
parent: Result
nav_order: 7
---

# Result Value Access & Interop
{: .no_toc }

## Table of contents
{: .no_toc .text-delta }

1. TOC
{:toc}

---

## Overview

Once you've built a Result pipeline, you need to extract values safely. This page covers all the ways to access Result values and integrate with non-Result code.

---

## ValueOr - Safe Value with Fallback

`ValueOr` provides a value if successful, otherwise returns a fallback:

```csharp
Result<int> result = GetValue();

// Simple fallback
int value = result.ValueOr(0);

// Success: returns the value
// Failure: returns 0

// Lazy fallback (computed only if needed)
int value2 = result.ValueOr(() => ExpensiveDefault());

// Result<T> with tuples
Result<string, int> nameAge = GetNameAge();
(string name, int age) = nameAge.ValueOr(("Unknown", 0));
```

### ValueOr Signature

```csharp
T ValueOr<T>(T defaultValue)
T ValueOr<T>(Func<T> defaultFactory)

// For Result<T1, T2, ...>
(T1, T2, ...) ValueOr<T1, T2, ...>((T1, T2, ...) defaultValue)
```

### Practical ValueOr

```csharp
// Configuration with defaults
var timeout = LoadConfig("timeout")
    .Map(c => c.TimeoutSeconds)
    .ValueOr(30);

// User profile with fallback
var displayName = GetUser(userId)
    .Map(u => u.DisplayName)
    .ValueOr("Guest");

// Computed fallback
var data = LoadFromCache(key)
    .ValueOr(() => LoadFromDatabase(key).ValueOr(DefaultData));
```

---

## ValueOrThrow - Throw on Failure

Extract value or throw an exception:

```csharp
Result<User> result = GetUser(userId);

// Throw result's exception if failed
User user = result.ValueOrThrow();

// Custom exception factory
User user2 = result.ValueOrThrow(error =>
    new ApplicationException($"Failed to get user: {error.Message}")
);
```

### When to Use ValueOrThrow

```csharp
// At application boundaries where exceptions are acceptable
public IActionResult GetUser(string id)
{
    try
    {
        var user = _service.GetUser(id)
            .ValueOrThrow(error => new HttpException(404, error.Message));
        
        return Ok(user);
    }
    catch (HttpException ex)
    {
        return StatusCode(ex.StatusCode, ex.Message);
    }
}

// In test assertions
[Fact]
public void Should_Return_Valid_User()
{
    var user = service.GetUser("123").ValueOrThrow();
    Assert.Equal("Alice", user.Name);
}
```

---

## TryGet - Pattern Matching Extraction

Safe extraction with out parameters:

```csharp
Result<User> result = GetUser(userId);

// Result<T>
if (result.TryGet(out var user, out var error))
{
    Console.WriteLine($"User: {user.Name}");
}
else
{
    Console.WriteLine($"Error: {error.Message}");
}

// Result<T1, T2, ...> - multiple out parameters
Result<string, int> nameAge = GetNameAge();

if (nameAge.TryGet(out var name, out var age, out var error))
{
    Console.WriteLine($"{name} is {age}");
}
else
{
    Console.WriteLine($"Error: {error.Message}");
}
```

### TryGet vs Ok

```csharp
// Ok - boolean return, extracts value and error
if (result.Ok(out var value, out var error))
{
    // success
}

// TryGet - boolean return, alias for Ok (source generated)
if (result.TryGet(out var value, out var error))
{
    // success
}

// Both are equivalent - TryGet follows .NET TryXxx convention
```

---

## ToNullable - Convert to Nullable

Convert Result to nullable value:

```csharp
Result<int> result = GetValue();

int? nullable = result.ToNullable();
// Success: nullable has value
// Failure: nullable is null

// With reference types
Result<User> userResult = GetUser(id);
User? user = userResult.ToNullable();

// With tuples
Result<string, int> nameAge = GetNameAge();
(string, int)? tuple = nameAge.ToNullable();

if (tuple.HasValue)
{
    Console.WriteLine($"{tuple.Value.Item1} is {tuple.Value.Item2}");
}
```

### When to Use ToNullable

```csharp
// Interop with nullable-aware code
string? GetUserName(string userId) =>
    GetUser(userId)
        .Map(u => u.Name)
        .ToNullable();

// Optional chaining
var city = GetUser(userId)
    .ToNullable()
    ?.Address
    ?.City;
```

---

## Deconstruct - Tuple Deconstruction

Deconstruct Results directly:

```csharp
Result<User> result = GetUser(userId);

// Deconstruct into ok, value, error
var (ok, user, error) = result;

if (ok)
{
    Console.WriteLine($"User: {user.Name}");
}
else
{
    Console.WriteLine($"Error: {error.Message}");
}

// Non-generic Result
Result operation = SaveData();
var (succeeded, err) = operation;

// With Result<T1, T2, ...>
Result<string, int> nameAge = GetNameAge();
var (success, name, age, err) = nameAge;
```

### Practical Deconstruct

```csharp
// Clean control flow
var (ok, user, error) = GetUser(userId);
if (!ok)
{
    _logger.LogError(error.Message);
    return NotFound();
}

ProcessUser(user);

// With pattern matching
return GetUser(userId) switch
{
    var (true, user, _) => Ok(user),
    var (false, _, error) => StatusCode(500, error.Message)
};
```

---

## Match - Value-Returning Fold

Transform Result to any type:

```csharp
Result<User> result = GetUser(userId);

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
        _ => StatusCode(500, error.Message)
    }
);

// Compute value
int score = GetScore(userId).Match(
    success: s => s,
    failure: _ => 0
);
```

---

## Interop Examples

### Example 1: ASP.NET Core Controllers

```csharp
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _service;
    
    [HttpGet("{id}")]
    public IActionResult GetUser(string id)
    {
        return _service.GetUser(id).Match(
            success: user => Ok(new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            }),
            failure: error => error switch
            {
                NotFoundError => NotFound(error.Message),
                UnauthorizedError => Unauthorized(error.Message),
                ValidationError => BadRequest(error.Message),
                _ => StatusCode(500, "Internal server error")
            }
        );
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        var result = await _service.CreateUserAsync(request);
        
        return result.Match(
            success: user => CreatedAtAction(
                nameof(GetUser),
                new { id = user.Id },
                user
            ),
            failure: error => BadRequest(new
            {
                error = error.Message,
                code = error.Code,
                metadata = error.Metadata
            })
        );
    }
    
    [HttpDelete("{id}")]
    public IActionResult DeleteUser(string id)
    {
        var result = _service.DeleteUser(id);
        
        // Non-generic Result
        return result.Match(
            success: () => NoContent(),
            failure: error => error switch
            {
                NotFoundError => NotFound(error.Message),
                _ => StatusCode(500, error.Message)
            }
        );
    }
}
```

### Example 2: Working with Nullable APIs

```csharp
public class UserRepository
{
    // Convert nullable database result to Result
    public Result<User> GetUser(string id)
    {
        User? user = _dbContext.Users.Find(id);
        
        return user != null
            ? Result.Success(user)
            : Result.Failure<User>(new NotFoundError("User", id));
    }
    
    // Convert Result to nullable for optional operations
    public User? TryGetUser(string id)
    {
        return GetUser(id).ToNullable();
    }
}

// Usage
var user = repository.TryGetUser("123");
Console.WriteLine(user?.Name ?? "Not found");
```

### Example 3: Configuration with Fallbacks

```csharp
public class ConfigService
{
    public AppConfig LoadConfig()
    {
        // Try multiple sources with fallbacks
        var config = LoadFromFile("appsettings.json")
            .RecoverWith(_ => LoadFromEnvironment())
            .RecoverWith(_ => LoadFromDefaults())
            .ValueOrThrow(error =>
                new ConfigurationException($"Failed to load config: {error.Message}")
            );
        
        return config;
    }
    
    public T GetSetting<T>(string key, T defaultValue)
    {
        return GetSettingResult<T>(key).ValueOr(defaultValue);
    }
    
    public T GetRequiredSetting<T>(string key)
    {
        return GetSettingResult<T>(key)
            .ValueOrThrow(error =>
                new ConfigurationException($"Required setting '{key}' not found")
            );
    }
    
    private Result<T> GetSettingResult<T>(string key)
    {
        // Implementation
        throw new NotImplementedException();
    }
}
```

### Example 4: LINQ Integration

```csharp
public class DataService
{
    public Result<List<UserDto>> GetActiveUsers()
    {
        return GetAllUsers()
            .Map(users => users
                .Where(u => u.IsActive)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email
                })
                .ToList()
            );
    }
    
    public Result<UserStats> CalculateStats()
    {
        return GetAllUsers().Map(users => new UserStats
        {
            TotalUsers = users.Count,
            ActiveUsers = users.Count(u => u.IsActive),
            AverageAge = users.Average(u => u.Age)
        });
    }
}
```

---

## Best Practices

### ✅ Do

**Use Match for branching logic:**
```csharp
return result.Match(
    success: value => ProcessSuccess(value),
    failure: error => HandleError(error)
);
```

**Use ValueOr for defaults:**
```csharp
var setting = GetConfig("theme").ValueOr("light");
```

**Use TryGet for pattern matching:**
```csharp
if (result.TryGet(out var value, out var error))
{
    ProcessValue(value);
}
else
{
    LogError(error);
}
```

**Use Deconstruct for clean code:**
```csharp
var (ok, user, error) = GetUser(id);
if (!ok) return HandleError(error);
ProcessUser(user);
```

### ❌ Don't

**Don't use ValueOrThrow unnecessarily:**
```csharp
// Bad - losing error information
try
{
    var value = result.ValueOrThrow();
}
catch
{
    // What was the error?
}

// Good - handle Result properly
result.Match(
    success: value => Process(value),
    failure: error => LogError(error)
);
```

**Don't ignore errors:**
```csharp
// Bad - error information lost
var value = result.ToNullable();
if (value == null) { /* Why null? */ }

// Good - use TryGet to access error
if (!result.TryGet(out var value, out var error))
{
    _logger.LogError(error.Message);
}
```

---

## Next Steps

- **[Error Handling](error-handling.html)** - Working with errors
- **[Async Operations](async.html)** - Async value access

Return to **[Result Overview](index.html)**
