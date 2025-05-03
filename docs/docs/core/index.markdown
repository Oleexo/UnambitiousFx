---
title: Core
nav_order: 2
has_children: true
---

# Core

The Core library of UnambitiousFx provides fundamental functional programming types that help you write more robust and expressive code. These types follow functional programming principles to handle common scenarios like operations that might fail, optional values, and disjoint unions.

## Overview

The library includes three main types:

- **[Result](result.markdown)** - Represents the outcome of an operation that can either succeed with a value or fail with an error
- **[Option](option.markdown)** - Represents an optional value that may or may not be present
- **[Either](either.markdown)** - Represents a value that can be one of two possible types

These types help you write code that is more explicit about possible outcomes, reducing the need for exceptions and null checks, and making your code more predictable and easier to reason about.

## Key Benefits

- **Explicit Error Handling**: Make potential failures part of your function signatures
- **No Null References**: Avoid null reference exceptions by using Option to represent missing values
- **Composable Operations**: Chain operations together in a clean, readable way
- **Happy Path Programming**: Focus on the successful path while handling errors gracefully
- **Type Safety**: Leverage the type system to ensure correct handling of all possible states

## Getting Started

Click on each type to learn more about its usage and features:

- [Result](result.markdown) - For operations that can succeed or fail
- [Option](option.markdown) - For values that may or may not be present
- [Either](either.markdown) - For values that can be one of two possible types

## Example

Here's a quick example of how these types can be used together:

```csharp
// Define functions that return different types
Result<User> GetUser(string userId) => /* implementation */;
Option<Address> GetPrimaryAddress(User user) => /* implementation */;
Either<Error, ShippingLabel> CreateShippingLabel(Address address) => /* implementation */;

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
