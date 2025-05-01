---
title: Result
parent: Core
nav_order: 1
---

# Result

The `Result<T>` type represents the outcome of an operation that can either succeed with a value of type `T` or fail with an error. It's a safer alternative to throwing exceptions for expected failure cases.

## Creating Results

```csharp
// Create a success result
var successResult = Result<int>.Success(42);

// Create a failure result
var failureResult = Result<int>.Failure(new Error("Something went wrong"));
```

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
// Example of chaining operations that might fail
Result<User> GetUser(string userId) {
    // Implementation that returns Result<User>
}

Result<Order> GetLatestOrder(User user) {
    // Implementation that returns Result<Order>
}

Result<ShippingInfo> GetShippingInfo(Order order) {
    // Implementation that returns Result<ShippingInfo>
}

// Chain operations, handling errors automatically
Result<ShippingInfo> GetUserShippingInfo(string userId) {
    return GetUser(userId)
        .Match(
            success: user => GetLatestOrder(user)
                .Match(
                    success: order => GetShippingInfo(order),
                    failure: error => Result<ShippingInfo>.Failure(error)
                ),
            failure: error => Result<ShippingInfo>.Failure(error)
        );
}
```