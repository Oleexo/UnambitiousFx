---
title: Collections & Aggregation
parent: Result
nav_order: 6
---

# Result Collections & Aggregation
{: .no_toc }

## Table of contents
{: .no_toc .text-delta }

1. TOC
{:toc}

---

## Overview

Working with collections of Results is common when processing multiple items. UnambitiousFx provides powerful operators for:

- **Traverse/Sequence** - Transform collections to Results
- **Combine/Aggregate** - Merge multiple Results
- **Apply/Zip** - Combine independent Results
- **Partition** - Split successes from failures

---

## Traverse - Transform Collection to Result

`Traverse` applies a Result-returning function to each item and collects results:

```csharp
List<string> userIds = new() { "1", "2", "3" };

// Transform each ID to User, collect all Results
Result<List<User>> users = userIds.Traverse(id => GetUser(id));

// If all succeed: Success with List<User>
// If any fails: Failure with first error
```

### Traverse Signature

```csharp
Result<List<TOut>> Traverse<TIn, TOut>(
    this IEnumerable<TIn> source,
    Func<TIn, Result<TOut>> selector
)
```

### Practical Traverse

```csharp
// Validate multiple emails
List<string> emails = new() { "alice@ex.com", "bob@ex.com", "invalid" };

Result<List<Email>> validEmails = emails.Traverse(email =>
    ValidateEmail(email)  // Returns Result<Email>
);

// Load multiple orders
Result<List<Order>> orders = orderIds.Traverse(id => LoadOrder(id));

// Parse multiple integers
Result<List<int>> numbers = strings.Traverse(s =>
    int.TryParse(s, out var n)
        ? Result.Success(n)
        : Result.Failure<int>($"Invalid number: {s}")
);
```

---

## TraverseAsync - Async Collection Processing

Process collections with async operations:

```csharp
List<string> userIds = new() { "1", "2", "3" };

Task<Result<List<User>>> users = userIds.TraverseAsync(async id =>
    await GetUserAsync(id)
);

// Processes all items concurrently by default
```

### Sequential vs Parallel

```csharp
// Parallel (default) - faster
Task<Result<List<Data>>> parallel = ids.TraverseAsync(
    async id => await FetchDataAsync(id)
);

// For sequential processing, use SelectMany with Task.WhenAll manually
// or process one at a time
```

---

## Sequence - Flip Collection and Result

`Sequence` transforms `IEnumerable<Result<T>>` into `Result<List<T>>`:

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
```

### When to Use Sequence

```csharp
// You have: List<Result<T>>
// You want: Result<List<T>>

// Common scenario: After mapping
List<string> inputs = new() { "1", "2", "3" };

Result<List<int>> numbers = inputs
    .Select(s => ParseInt(s))  // Returns IEnumerable<Result<int>>
    .ToList()
    .Sequence();  // Converts to Result<List<int>>

// Or use Traverse directly
Result<List<int>> numbers2 = inputs.Traverse(s => ParseInt(s));
```

---

## Combine - Merge Multiple Results

`Combine` merges Results, collecting all errors if any fail:

```csharp
Result<int> r1 = Result.Success(10);
Result<int> r2 = Result.Success(20);
Result<int> r3 = Result.Success(30);

Result<List<int>> combined = Result.Combine(r1, r2, r3);
// Success: [10, 20, 30]

// With failures
Result<int> f1 = Result.Success(10);
Result<int> f2 = Result.Failure<int>("Error 1");
Result<int> f3 = Result.Failure<int>("Error 2");

Result<List<int>> failed = Result.Combine(f1, f2, f3);
// Failure with both "Error 1" and "Error 2"
```

### Combine vs Traverse

```csharp
// Traverse: Transform collection items
Result<List<User>> users = ids.Traverse(id => GetUser(id));

// Combine: Merge existing Results
Result<User> user = GetUser("1");
Result<Settings> settings = GetSettings("1");
Result<List<object>> combined = Result.Combine(user, settings);
```

---

## Apply / Zip - Combine Independent Results

Apply combines Results using a function (applicative functor pattern):

### Apply with Two Results

```csharp
Result<int> width = GetWidth();
Result<int> height = GetHeight();

Result<int> area = width.Apply(height, (w, h) => w * h);
// Both must succeed to calculate area
```

### Zip (Alternative Syntax)

```csharp
Result<string> name = GetName();
Result<int> age = GetAge();

Result<User> user = name.Zip(age, (n, a) => new User(n, a));
// Combines both into User if both succeed
```

### Practical Apply Examples

```csharp
// Validate form fields independently
Result<string> validName = ValidateName(form.Name);
Result<string> validEmail = ValidateEmail(form.Email);
Result<int> validAge = ValidateAge(form.Age);

// Combine all validations
Result<UserRegistration> registration = validName
    .Apply(validEmail, validAge, (name, email, age) =>
        new UserRegistration(name, email, age)
    );
// Only succeeds if ALL validations pass
// Collects ALL validation errors if any fail
```

---

## Aggregate - Custom Aggregation

Aggregate multiple Results with a custom accumulator:

```csharp
List<Result<int>> results = new()
{
    Result.Success(10),
    Result.Success(20),
    Result.Success(30)
};

// Sum all values
Result<int> sum = results.Aggregate(
    Result.Success(0),  // Initial value
    (acc, curr) => acc.Apply(curr, (a, c) => a + c)
);
// Success: 60

// Find maximum
Result<int> max = results.Aggregate(
    Result.Success(int.MinValue),
    (acc, curr) => acc.Apply(curr, Math.Max)
);
```

---

## Partition - Split Successes and Failures

Separate successful and failed Results:

```csharp
List<Result<int>> results = new()
{
    Result.Success(1),
    Result.Failure<int>("Error 1"),
    Result.Success(2),
    Result.Failure<int>("Error 2"),
    Result.Success(3)
};

var (successes, failures) = results.Partition();
// successes: [1, 2, 3]
// failures: [Error 1, Error 2]
```

### Practical Partition

```csharp
// Process users, some may fail
List<Result<User>> results = userIds
    .Select(id => GetUser(id))
    .ToList();

var (users, errors) = results.Partition();

// Log successful users
foreach (var user in users)
{
    _logger.LogInfo($"Loaded user: {user.Name}");
}

// Handle errors
foreach (var error in errors)
{
    _logger.LogError($"Failed to load user: {error.Message}");
}

// Continue with successful users
return ProcessUsers(users);
```

---

## Practical Examples

### Example 1: Batch User Loading

```csharp
public class UserService
{
    public async Task<Result<List<UserDto>>> GetUsersAsync(List<string> userIds)
    {
        // Load all users (parallel)
        var results = await userIds.TraverseAsync(async id =>
            await GetUserAsync(id)
        );
        
        // Transform to DTOs if all succeeded
        return results.Map(users =>
            users.Select(u => new UserDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email
            }).ToList()
        );
    }
    
    // Alternative: Continue with partial results
    public async Task<(List<UserDto> Users, List<IError> Errors)> 
        GetUsersWithErrorsAsync(List<string> userIds)
    {
        var results = await userIds.TraverseAsync(async id =>
            await GetUserAsync(id)
        );
        
        // Use Partition to handle partial success
        var resultsList = userIds
            .Select(id => GetUserAsync(id).Result)  // For demo - use proper async
            .ToList();
            
        var (users, errors) = resultsList.Partition();
        
        var dtos = users.Select(u => new UserDto
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email
        }).ToList();
        
        return (dtos, errors);
    }
}
```

### Example 2: Form Validation

```csharp
public Result<Registration> ValidateRegistrationForm(RegistrationForm form)
{
    // Validate each field independently
    var nameResult = ValidateName(form.Name);
    var emailResult = ValidateEmail(form.Email);
    var passwordResult = ValidatePassword(form.Password);
    var ageResult = ValidateAge(form.Age);
    
    // Combine all validations
    return nameResult.Apply(
        emailResult,
        passwordResult,
        ageResult,
        (name, email, password, age) => new Registration
        {
            Name = name,
            Email = email,
            Password = password,
            Age = age
        }
    );
    // Collects ALL validation errors if any field fails
}

Result<string> ValidateName(string name) =>
    !string.IsNullOrEmpty(name)
        ? Result.Success(name)
        : Result.Failure<string>(new ValidationError(new[] { "Name required" }));

Result<string> ValidateEmail(string email) =>
    email.Contains("@")
        ? Result.Success(email)
        : Result.Failure<string>(new ValidationError(new[] { "Invalid email" }));

Result<string> ValidatePassword(string password) =>
    password.Length >= 8
        ? Result.Success(password)
        : Result.Failure<string>(new ValidationError(new[] { "Password must be 8+ chars" }));

Result<int> ValidateAge(int age) =>
    age >= 18
        ? Result.Success(age)
        : Result.Failure<int>(new ValidationError(new[] { "Must be 18+" }));
```

### Example 3: Batch Processing with Partial Failure

```csharp
public class OrderProcessor
{
    public async Task<BatchResult> ProcessOrdersAsync(List<Order> orders)
    {
        var results = await orders.TraverseAsync(async order =>
            await ProcessOrderAsync(order)
        );
        
        // Get results as list for partitioning
        var resultsList = orders
            .Select(o => ProcessOrderAsync(o).Result)
            .ToList();
        
        var (processed, failures) = resultsList.Partition();
        
        // Log results
        _logger.LogInfo($"Processed {processed.Count} orders successfully");
        _logger.LogError($"Failed to process {failures.Count} orders");
        
        // Send notifications for processed orders
        await Task.WhenAll(processed.Select(p =>
            _notificationService.SendConfirmationAsync(p)
        ));
        
        return new BatchResult
        {
            SuccessCount = processed.Count,
            FailureCount = failures.Count,
            ProcessedOrders = processed,
            Errors = failures
        };
    }
}
```

---

## Best Practices

### ✅ Do

**Use Traverse for transforming collections:**
```csharp
// Good - single operation
Result<List<User>> users = ids.Traverse(id => GetUser(id));

// Avoid - manual loop
List<User> users = new();
foreach (var id in ids)
{
    var result = GetUser(id);
    if (result.IsFaulted) return Result.Failure<List<User>>(result.Error);
    users.Add(result.Value);
}
```

**Use Apply for independent validations:**
```csharp
// Collects all errors
return name.Apply(email, age, (n, e, a) => new User(n, e, a));
```

**Use Partition for partial success scenarios:**
```csharp
var (successes, failures) = results.Partition();
ProcessSuccesses(successes);
LogFailures(failures);
```

### ❌ Don't

**Don't use Traverse for dependent operations:**
```csharp
// Bad - second operation depends on first
ids.Traverse(id => GetUser(id).Bind(user => GetOrders(user.Id)))

// Good - use proper chaining
GetUser(id).Bind(user => GetOrders(user.Id))
```

---

## Next Steps

- **[Value Access](value-access.html)** - Extract values from Results
- **[Async Operations](async.html)** - TraverseAsync details

Return to **[Result Overview](index.html)**
