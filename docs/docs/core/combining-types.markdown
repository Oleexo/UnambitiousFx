---
title: Combining Types
parent: Core
nav_order: 4
---

# Combining Types

The Core library types (Result, Option, and Either) can be combined to create powerful abstractions for handling complex scenarios.

## Example: Result with Option

```csharp
// Example: Function that returns a Result containing an Option
Result<Option<User>> FindUserById(string userId) {
    try {
        var user = repository.GetUser(userId);
        return user != null
            ? Result<Option<User>>.Success(Option<User>.Some(user))
            : Result<Option<User>>.Success(Option<User>.None());
    }
    catch (Exception ex) {
        return Result<Option<User>>.Failure(new Error(ex.Message));
    }
}

// Usage
var result = FindUserById("123");
result.Match(
    success: option => option.Match(
        some: user => Console.WriteLine($"Found user: {user.Name}"),
        none: () => Console.WriteLine("User not found")
    ),
    failure: error => Console.WriteLine($"Error: {error.Message}")
);
```

This example demonstrates how to use a Result type that contains an Option type. This combination allows you to express three different states:
1. Success with a value (user found)
2. Success with no value (user not found, but no error occurred)
3. Failure (an error occurred while trying to find the user)

By using these functional programming types together, you can write more expressive and robust code that clearly communicates possible outcomes and handles edge cases in a consistent way.