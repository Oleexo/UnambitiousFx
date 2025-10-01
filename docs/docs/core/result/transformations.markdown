---
title: Transformations
parent: Result
nav_order: 2
---

# Result Transformations
{: .no_toc }

## Table of contents
{: .no_toc .text-delta }

1. TOC
{:toc}

---

## Overview

Transformations are the heart of functional composition with Result. They let you:
- Transform success values without unwrapping
- Chain operations that might fail
- Automatically short-circuit on failure
- Build complex pipelines from simple functions

All transformation methods:
- ✅ **Preserve failures** - If the input is failure, output is failure (no transformation)
- ✅ **Transform successes** - Only successful results are transformed
- ✅ **Maintain type safety** - Compiler enforces correct types throughout the chain

---

## Map - Transform Success Values

`Map` transforms the value inside a successful Result. If the Result is a failure, Map does nothing.

### Basic Map

```csharp
Result<int> age = Result.Success(25);

Result<string> description = age.Map(a => $"Age is {a}");
// Success: "Age is 25"

Result<int> failed = Result.Failure<int>("Invalid input");
Result<string> stillFailed = failed.Map(a => $"Age is {a}");
// Still failure: "Invalid input" - Map didn't run
```

### Map Signature

```csharp
// Transform T to TOut
Result<TOut> Map<TOut>(Func<T, TOut> mapper)

// The mapper function:
// - Receives the success value
// - Returns a new value
// - Should NOT return Result (use Bind for that)
```

### Practical Map Examples

**Example 1: Data transformation pipeline**

```csharp
Result<User> GetUser(string id);

Result<UserDto> GetUserDto(string id)
{
    return GetUser(id)
        .Map(user => new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        });
}
```

**Example 2: Multiple Maps**

```csharp
Result<string> input = Result.Success("  HELLO  ");

var processed = input
    .Map(s => s.Trim())           // "HELLO"
    .Map(s => s.ToLower())        // "hello"
    .Map(s => s + " world");      // "hello world"

// processed is Result<string>
```

**Example 3: Map with tuples**

```csharp
Result<string, int> nameAge = Result.Success("Alice", 30);

// Map receives tuple parameters
var description = nameAge.Map((name, age) => 
    $"{name} is {age} years old"
);
// Result<string>
```

### When to Use Map

✅ **Use Map when:**
- Transforming a value (string → int, User → UserDto)
- The transformation always succeeds
- You're not calling another function that returns Result

❌ **Don't use Map when:**
- The transformation might fail (use Bind instead)
- You're calling a function that returns Result<T> (use Bind instead)

---

## Bind - Chain Operations That Can Fail

`Bind` (also called FlatMap or Then) chains operations where each step might fail. It's essential for composing multiple Result-returning functions.

### Basic Bind

```csharp
Result<int> ParseInt(string s) => 
    int.TryParse(s, out var v) ? Result.Success(v) : Result.Failure<int>("Not a number");

Result<int> Divide(int numerator, int denominator) =>
    denominator != 0 
        ? Result.Success(numerator / denominator)
        : Result.Failure<int>("Division by zero");

// Chain operations
Result<int> Calculate(string input)
{
    return ParseInt(input)
        .Bind(num => Divide(100, num));
}

// Success case
Calculate("4");    // Success: 25
Calculate("0");    // Failure: "Division by zero"
Calculate("abc");  // Failure: "Not a number"
```

### Bind Signature

```csharp
// Transform T to Result<TOut>
Result<TOut> Bind<TOut>(Func<T, Result<TOut>> binder)

// The binder function:
// - Receives the success value
// - Returns a Result (which might be success or failure)
// - Bind automatically flattens Result<Result<TOut>> to Result<TOut>
```

### Map vs Bind Comparison

```csharp
Result<User> user = GetUser("123");

// Map: transformation always succeeds
Result<string> name = user.Map(u => u.Name);
//                              ^ returns string

// Bind: transformation can fail
Result<Address> address = user.Bind(u => GetAddress(u.AddressId));
//                                   ^ returns Result<Address>
```

### Practical Bind Examples

**Example 1: Multi-step validation**

```csharp
Result<User> ValidateAndCreateUser(string email, string password)
{
    return ValidateEmail(email)
        .Bind(_ => ValidatePassword(password))
        .Bind(_ => CheckEmailNotInUse(email))
        .Bind(_ => Result.Try(() => new User(email, password)));
}

Result ValidateEmail(string email)
{
    return email.Contains("@") 
        ? Result.Success()
        : Result.Failure("Invalid email");
}

Result ValidatePassword(string password)
{
    return password.Length >= 8
        ? Result.Success()
        : Result.Failure("Password too short");
}

Result CheckEmailNotInUse(string email)
{
    return _db.EmailExists(email)
        ? Result.Failure("Email already registered")
        : Result.Success();
}
```

**Example 2: Database query chain**

```csharp
Result<OrderSummary> GetOrderSummary(string userId, int orderId)
{
    return GetUser(userId)
        .Bind(user => GetOrder(orderId))
        .Bind(order => ValidateUserOwnsOrder(user, order))
        .Bind(order => CalculateSummary(order));
}

// Each step can fail independently
// Failure at any step short-circuits the chain
```

**Example 3: Parsing and validation**

```csharp
Result<DateTime> ParseAndValidateDate(string input)
{
    return ParseDate(input)
        .Bind(date => ValidateNotInPast(date))
        .Bind(date => ValidateWithinRange(date));
}

Result<DateTime> ParseDate(string s) =>
    DateTime.TryParse(s, out var d) 
        ? Result.Success(d) 
        : Result.Failure<DateTime>("Invalid date format");

Result<DateTime> ValidateNotInPast(DateTime date) =>
    date >= DateTime.Now
        ? Result.Success(date)
        : Result.Failure<DateTime>("Date cannot be in the past");

Result<DateTime> ValidateWithinRange(DateTime date) =>
    date <= DateTime.Now.AddYears(1)
        ? Result.Success(date)
        : Result.Failure<DateTime>("Date too far in the future");
```

---

## SelectMany - LINQ Query Syntax

`SelectMany` enables LINQ query comprehension syntax with Result. It's syntactic sugar over `Bind` and `Map`.

### Basic SelectMany

```csharp
// Traditional Bind syntax
Result<string> traditional = 
    GetUser("123")
        .Bind(user => GetAddress(user.AddressId)
            .Map(address => $"{user.Name} lives at {address.Street}"));

// LINQ query syntax (using SelectMany)
Result<string> query =
    from user in GetUser("123")
    from address in GetAddress(user.AddressId)
    select $"{user.Name} lives at {address.Street}";

// Both are equivalent!
```

### Complex LINQ Queries

```csharp
Result<OrderReport> GenerateReport(string userId, int orderId)
{
    return 
        from user in GetUser(userId)
        from order in GetOrder(orderId)
        from items in GetOrderItems(orderId)
        where order.UserId == user.Id  // Additional conditions
        select new OrderReport
        {
            UserName = user.Name,
            OrderDate = order.Date,
            ItemCount = items.Count(),
            Total = items.Sum(i => i.Price)
        };
}
```

### When to Use SelectMany

✅ **Use LINQ syntax when:**
- Combining multiple Results
- Code reads more naturally with `from...select`
- You want to use LINQ features (where, let, etc.)

✅ **Use Bind syntax when:**
- Single chaining operation
- Functional pipeline style preferred
- Want explicit transformation steps

---

## Flatten - Collapse Nested Results

`Flatten` transforms `Result<Result<T>>` into `Result<T>`, removing one layer of nesting.

### Basic Flatten

```csharp
// Sometimes you end up with nested Results
Result<Result<int>> nested = Result.Success(Result.Success(42));

Result<int> flattened = nested.Flatten();
// Success: 42

// If outer is failure
Result<Result<int>> outerFail = Result.Failure<Result<int>>("Outer failed");
Result<int> stillFailed = outerFail.Flatten();
// Failure: "Outer failed"

// If outer success, inner failure
Result<Result<int>> innerFail = Result.Success(
    Result.Failure<int>("Inner failed")
);
Result<int> alsofailed = innerFail.Flatten();
// Failure: "Inner failed"
```

### When Does Nesting Occur?

Nesting happens when using `Map` with a function that returns `Result`:

```csharp
Result<string> userId = Result.Success("123");

// Wrong: Using Map with Result-returning function
Result<Result<User>> nested = userId.Map(id => GetUser(id));
//                                          ^ returns Result<User>

// Fix 1: Use Bind instead of Map
Result<User> correct = userId.Bind(id => GetUser(id));

// Fix 2: Use Map then Flatten
Result<User> alsoCorrect = userId
    .Map(id => GetUser(id))
    .Flatten();
```

### Flatten with Async

Flatten also works with `Task<Result<T>>`:

```csharp
// Task<Result<Result<T>>> → Task<Result<T>>
Task<Result<int>> flattened = nestedTask.Flatten();

// Common with async operations
Task<Result<User>> GetUserAsync(string id);

Task<Result<Result<User>>> nested = 
    Task.FromResult(Result.Success("123"))
        .MapAsync(id => GetUserAsync(id));  // Returns Task<Result<Result<User>>>

Task<Result<User>> correct = nested.Flatten();
```

### Practical Flatten Example

```csharp
public class CacheService
{
    Result<Result<User>> GetFromCache(string key)
    {
        var cached = _cache.Get(key);
        if (cached == null)
            return Result.Failure<Result<User>>("Not in cache");
        
        // cached value is itself a Result<User>
        return Result.Success(JsonSerializer.Deserialize<Result<User>>(cached));
    }
    
    Result<User> GetUser(string userId)
    {
        return GetFromCache(userId)
            .Flatten()  // Result<Result<User>> → Result<User>
            .RecoverWith(error => LoadFromDatabase(userId));
    }
}
```

---

## Combining Transformations

### Real-World Pipeline Example

```csharp
public class OrderService
{
    public async Task<Result<OrderConfirmation>> PlaceOrderAsync(
        string userId, 
        List<string> productIds)
    {
        return await ValidateUser(userId)
            .Bind(user => ValidateProducts(productIds)
                .Map(products => (user, products)))
            .Bind(data => CalculateTotal(data.products)
                .Map(total => (data.user, data.products, total)))
            .Bind(data => CheckInventory(data.products)
                .Map(_ => data))
            .BindAsync(data => ProcessPaymentAsync(data.user, data.total)
                .Map(payment => (data.user, data.products, payment)))
            .BindAsync(data => SaveOrderAsync(data.user, data.products, data.payment))
            .MapAsync(order => new OrderConfirmation
            {
                OrderId = order.Id,
                Total = order.Total,
                EstimatedDelivery = DateTime.Now.AddDays(3)
            })
            .TapAsync(confirmation => 
                SendConfirmationEmailAsync(confirmation));
    }
    
    Result<User> ValidateUser(string userId) { /* ... */ }
    Result<List<Product>> ValidateProducts(List<string> ids) { /* ... */ }
    Result<decimal> CalculateTotal(List<Product> products) { /* ... */ }
    Result CheckInventory(List<Product> products) { /* ... */ }
    Task<Result<Payment>> ProcessPaymentAsync(User user, decimal amount) { /* ... */ }
    Task<Result<Order>> SaveOrderAsync(User user, List<Product> products, Payment payment) { /* ... */ }
    Task SendConfirmationEmailAsync(OrderConfirmation confirmation) { /* ... */ }
}
```

### Transformation Decision Tree

```
Need to transform Result value?
│
├─ Transformation always succeeds?
│  └─ Use Map
│     Result<User>.Map(u => u.Name) → Result<string>
│
├─ Transformation returns Result?
│  └─ Use Bind
│     Result<User>.Bind(u => GetAddress(u.Id)) → Result<Address>
│
├─ Multiple Results to combine?
│  └─ Use LINQ (SelectMany) or nested Bind
│     from user in GetUser(id)
│     from order in GetOrder(orderId)
│     select new { user, order }
│
└─ Have nested Result<Result<T>>?
   └─ Use Flatten
      Result<Result<User>>.Flatten() → Result<User>
```

---

## Best Practices

### ✅ Do

**Chain operations for readability:**
```csharp
return GetUser(userId)
    .Bind(user => ValidateUser(user))
    .Bind(user => GetUserSettings(user.Id))
    .Map(settings => settings.ToDto());
```

**Use appropriate method for the job:**
```csharp
// Map for pure transformations
.Map(x => x * 2)

// Bind for operations that can fail
.Bind(x => Divide(100, x))

// LINQ for multiple dependencies
from a in GetA()
from b in GetB(a)
select Combine(a, b)
```

**Keep functions small and focused:**
```csharp
// Good - each function does one thing
Result<Email> ValidateEmail(string email) { /* ... */ }
Result<User> CreateUser(Email email) { /* ... */ }
Result SendWelcomeEmail(User user) { /* ... */ }

// Chain them
ValidateEmail(input)
    .Bind(email => CreateUser(email))
    .Bind(user => SendWelcomeEmail(user));
```

### ❌ Don't

**Don't use Map when you need Bind:**
```csharp
// Bad - creates nested Result
Result<Result<User>> nested = userId.Map(id => GetUser(id));

// Good - use Bind
Result<User> flat = userId.Bind(id => GetUser(id));
```

**Don't throw exceptions in Map:**
```csharp
// Bad - exception breaks the Result chain
.Map(x => {
    if (x == 0) throw new Exception("Zero!");
    return 100 / x;
})

// Good - use Bind with Result
.Bind(x => x != 0 
    ? Result.Success(100 / x)
    : Result.Failure<int>("Cannot divide by zero"))
```

**Don't forget to handle failures:**
```csharp
// Bad - ignoring potential failure
var value = GetUser("123").Map(u => u.Name);
// What if GetUser failed?

// Good - handle both cases
var name = GetUser("123")
    .Map(u => u.Name)
    .ValueOr("Unknown");
```

---

## Performance Considerations

### Transformation Cost

- **Map**: No overhead - just a function call
- **Bind**: Minimal overhead - checks status then calls function
- **SelectMany**: Same as Bind - LINQ syntax compiles to Bind/Map
- **Flatten**: Very cheap - just unwraps one layer

### Avoid Unnecessary Allocations

```csharp
// Good - reuse functions
Func<int, int> double = x => x * 2;
result.Map(double);

// Less optimal - creates new lambda each time
for (int i = 0; i < 1000; i++)
{
    result.Map(x => x * 2);  // New lambda allocation
}
```

---

## Next Steps

Master transformations? Continue learning:

- **[Error Handling](error-handling.html)** - MapError, Recovery, domain errors
- **[Validation](validation.html)** - Ensure predicates and Tap for side-effects
- **[Async Operations](async.html)** - MapAsync, BindAsync, and Task integration
- **[Collections](collections.html)** - Working with multiple Results

Or return to **[Result Overview](index.html)** to see all features.
