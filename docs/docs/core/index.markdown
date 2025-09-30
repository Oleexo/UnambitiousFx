---
title: Core
nav_order: 2
has_children: true
---

# Core

The Core library of UnambitiousFx provides fundamental functional programming types that help you write more robust and expressive code. These types follow functional programming principles to handle common scenarios like operations that might fail, optional values, and disjoint unions.

## Overview

The library includes three main types:

- **[Result](result/index.html)** - Represents the outcome of an operation that can either succeed with a value or fail with an error (comprehensive guide with examples)
- **[Option](option/index.html)** - Represents an optional value that may or may not be present
- **[Either](either/index.html)** - Represents a value that can be one of two possible types
- **[xUnit Assertions](xunit-assertions/index.html)** - Assertion helpers for testing `Result`, `Option`, and `Either` with xUnit

These types help you write code that is more explicit about possible outcomes, reducing the need for exceptions and null checks, and making your code more predictable and easier to reason about.

## Key Benefits

- **Explicit Error Handling**: Make potential failures part of your function signatures
- **No Null References**: Avoid null reference exceptions by using Option to represent missing values
- **Composable Operations**: Chain operations together in a clean, readable way
- **Happy Path Programming**: Focus on the successful path while handling errors gracefully
- **Type Safety**: Leverage the type system to ensure correct handling of all possible states

## Getting Started

Click on each type to learn more about its usage and features:

- **[Result](../result/index.html)** - Complete guide to Result with pros/cons, examples, and feature roadmap
  - [Creation & Basics](../result/creation.html) - Learn how to create and inspect Results
  - [Transformations](../result/transformations.html) - Map, Bind, SelectMany, Flatten
  - [Error Handling](../result/error-handling.html) - Domain errors, metadata, recovery
  - [Validation & Side Effects](../result/validation.html) - Ensure, Tap, TapError
  - [Async Operations](../result/async.html) - Working with Task and ValueTask
  - [Collections & Aggregation](../result/collections.html) - Traverse, Sequence, Combine
  - [Value Access & Interop](../result/value-access.html) - Safe value extraction
- **[Option](option.html)** - For values that may or may not be present
- **[Either](either.html)** - For values that can be one of two possible types
- **[xUnit Assertions](xunit-assertions.html)** - For expressive testing helpers
- **[Result Roadmap](result-roadmap.html)** - For current & planned capabilities

## Example

Here's a quick example showing how Result works:

```csharp
using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Reasons;

// Define operations that can fail
Result<User> GetUser(string userId) =>
    _repository.Find(userId) is User user
        ? Result.Success(user)
        : Result.Failure<User>(new NotFoundError("User", userId));

Result<decimal> GetAccountBalance(User user) =>
    user.IsActive
        ? Result.Success(user.AccountBalance)
        : Result.Failure<decimal>(new UnauthorizedError("User account is inactive"));

// Compose operations - automatically short-circuits on first failure
Result<string> GetFormattedBalance(string userId)
{
    return GetUser(userId)
        .Bind(user => GetAccountBalance(user))
        .Map(balance => $"${balance:N2}")
        .Tap(formatted => _logger.LogInfo($"Balance: {formatted}"))
        .MapError(error => error.WithMetadata("userId", userId));
}

// Use the result
var result = GetFormattedBalance("user-123");
var display = result.ValueOr("Balance unavailable");
```

For more examples and detailed documentation, see the **[Result Overview](result-overview.html)**.

// Compose them together
Result<Either<Error, ShippingLabel>> ProcessOrder(string userId) {
    return GetUser(userId)
        .Match(
            success: user => GetPrimaryAddress(user)
                .Match(
                    some: address => Result<Either<Error, ShippingLabel>>.Success(CreateShippingLabel(address)),
                    none: () => Result<Either<Error, ShippingLabel>>.Success(
                        Either<Error, ShippingLabel>.FromLeft(new Error("No address found")))
                ),
            failure: error => Result<Either<Error, ShippingLabel>>.Failure(error)
        );
}
```

This example demonstrates how these types can be combined to handle complex workflows with multiple potential failure points in a clean, type-safe way.
