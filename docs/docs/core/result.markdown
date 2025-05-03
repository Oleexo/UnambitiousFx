---
title: Result
parent: Core
nav_order: 1
---

# Result

## What is a Result?

The `Result<T>` type represents the outcome of an operation that can either succeed with a value of type `T` or fail
with an error. It's a safer alternative to throwing exceptions for expected failure cases.

The `Result` type (without generic parameter) represents operations that can either succeed without returning a value or
fail with an error.

Both types provide a robust way to handle errors in a functional programming style.

### Why use Result instead of exceptions?

- **Explicit error handling**: Forces you to consider error cases in your code
- **Type safety**: Errors are part of your method signature, not hidden side effects
- **Composability**: Results can be easily combined and chained together
- **Testability**: Makes testing error scenarios much easier

### Real-world example

Consider a user registration process where multiple validations need to occur:

```csharp
// Traditional approach with exceptions
public void RegisterUser(string email, string password) {
    if (!IsValidEmail(email))
        throw new ValidationException("Invalid email format");

    if (UserExists(email))
        throw new DuplicateUserException("User already exists");

    if (!IsStrongPassword(password))
        throw new ValidationException("Password too weak");

    // Create user...
}

// Result-based approach
public Result<User> RegisterUser(string email, string password) {
    if (!IsValidEmail(email))
        return Result<User>.Failure(new Error("Invalid email format"));

    if (UserExists(email))
        return Result<User>.Failure(new Error("User already exists"));

    if (!IsStrongPassword(password))
        return Result<User>.Failure(new Error("Password too weak"));

    var user = new User(email, password);
    return Result<User>.Success(user);
}
```

The Result approach makes error handling explicit and allows for more elegant composition of operations.

## Creating Results

```csharp
// Create a success result
var successResult = Result<int>.Success(42);

// Create a result without value
var sucessResultWithoutValue = Result.Success();

// Create a failure result
var failureResult = Result<int>.Failure(new Error("Something went wrong"));
```

The `Result` type ensures you cannot accidentally access the value or error property directly.
The API provides multiple methods to safely access the value and check the validity of the operation.

## Checking Result Status


```csharp
// Check if result is successful
if (result.IsSuccess) {
    // Handle success case
}

// Check if result is a failure
if (result.IsFaulted) {
    // Handle failure case
}

// Extract value and error in one operation
if (result.Ok(out var value, out var error)) {
    // Success case: value contains the result, error is null
    Console.WriteLine($"Success: {value}");
} else {
    // Failure case: error contains the error, value is default
    Console.WriteLine($"Error: {error.Message}");
}
```

## Pattern Matching

```csharp
// Match with actions
result.Match(
    success: value => Console.WriteLine($"Success: {value}"),
    failure: error => Console.WriteLine($"Error: {error.Message}")
);

// Match with functions
string message = result.Match(
    success: value => $"Success: {value}",
    failure: error => $"Error: {error.Message}"
);
```

## Conditional Execution

```csharp
// Execute action only on success
result.IfSuccess(value => {
    Console.WriteLine($"Processing value: {value}");
});

// Execute action only on failure
result.IfFailure(error => {
    Console.WriteLine($"Handling error: {error.Message}");
});
```

## Chaining Operations (Happy Path)

```csharp
Result<string> GetShippingInfo((string order, int user) tuple) {
     return $"Hello {tuple.user} from {tuple.order}";
}
Result<(string, int)> GetLatestOrder(int user) {
     return user == 42
                ? ("fx", 42)
                : ("fx", 24);
}
Result<int> GetUser(string userId) {
    return userId == "toto"
               ? 42
               : 24;
}


var result = GetUser("toto")
                    .Bind(user => GetLatestOrder(user))
                    .Bind(tuple => GetShippingInfo(tuple));

if (result.Ok(out var value, out _)) {
    Console.WriteLine(value);
}

// print: Hello 42 from fx
```

## Understanding Result as a Monoid

A monoid in functional programming is a type with:
1. An **associative binary operation** (combining two values produces another value of the same type)
2. An **identity element** (a neutral value that doesn't change other values when combined with them)

The `Result<T>` type forms a monoid where:
- The binary operation is the `Bind` method, which allows you to chain operations
- The identity element is a successful result

This monoid structure is what enables the elegant chaining of operations shown above. Each operation in the chain can be thought of as a transformation that preserves the structure of the Result type.

### Visualizing the Bind operation

When you chain operations with `Bind`, you're essentially creating a pipeline where:

1. If any operation fails, the failure is propagated through the entire chain
2. If all operations succeed, the final result contains the value from the last operation

```
GetUser("toto") → Success(42)
    ↓
GetLatestOrder(42) → Success(("fx", 42))
    ↓
GetShippingInfo(("fx", 42)) → Success("Hello 42 from fx")
```

If any step had failed, the chain would short-circuit and return that failure immediately, without executing the remaining operations.
