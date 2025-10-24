---
title: Either
parent: Core
nav_order: 4
has_children: true
---

# Either

> NOTE: `Either<TLeft, TRight>` now inherits from the generic discriminated union base `OneOf<TLeft, TRight>`. Use `Either` for application code because it gives semantic meaning (`Left` / `Right`) and provides higher-level helpers like `Bind`. Use `OneOf` only when defining new semantic two-branch unions.

## What is an Either?

The `Either<TLeft, TRight>` type represents a value that can be one of two possible types. It's often used to represent a computation that can result in different types of values, with the convention that `Right` represents the "correct" or primary case, and `Left` represents an alternative or error case.

### Why use Either?

- **Dual outcomes**: Represents operations that can produce two different types of results
- **Type safety**: Both possible outcomes are explicitly typed
- **Composability**: Eithers can be easily combined and chained together
- **More flexibility than Result**: Unlike Result, the Left case can contain any type, not just errors

### When to use Either vs Result vs Option

- Use **Result** when an operation can succeed with a value or fail with an error
- Use **Option** when a value may or may not be present
- Use **Either** when an operation can produce two different types of values, and neither is necessarily an error

### Real-world example

Consider a search function that can either return a user or suggest alternative usernames:

```csharp
// Traditional approach with special return values
public class SearchResult {
    public User? FoundUser { get; set; }
    public List<string>? AlternativeSuggestions { get; set; }
    public bool IsUserFound => FoundUser != null;
}

public SearchResult FindUser(string username) {
    var user = _userRepository.FindByUsername(username);

    if (user != null) {
        return new SearchResult { FoundUser = user };
    } else {
        var suggestions = _suggestionEngine.GetSimilarUsernames(username);
        return new SearchResult { AlternativeSuggestions = suggestions };
    }
}

// Using the result
var result = FindUser("johndoe");
if (result.IsUserFound) {
    DisplayUserProfile(result.FoundUser);
} else {
    DisplaySuggestions(result.AlternativeSuggestions);
}

// Either-based approach
public Either<List<string>, User> FindUser(string username) {
    var user = _userRepository.FindByUsername(username);

    if (user != null) {
        return Either<List<string>, User>.FromRight(user);
    } else {
        var suggestions = _suggestionEngine.GetSimilarUsernames(username);
        return Either<List<string>, User>.FromLeft(suggestions);
    }
}

// Using the result with pattern matching
FindUser("johndoe").Match(
    leftAction: suggestions => DisplaySuggestions(suggestions),
    rightAction: user => DisplayUserProfile(user)
);
```

The Either approach makes it clear that the function returns exactly one of two possible types and provides elegant ways to handle both cases.

## Creating Eithers

```csharp
// Create a Left instance
var leftEither = Either<string, int>.FromLeft("Error message");

// Create a Right instance
var rightEither = Either<string, int>.FromRight(42);

// Using implicit conversion
Either<string, int> implicitLeft = "Error message";
Either<string, int> implicitRight = 42;
```

## Checking Either Status

```csharp
// Check if either is Left
if (either.IsLeft) {
    // Handle Left case
}

// Check if either is Right
if (either.IsRight) {
    // Handle Right case
}

// Extract Left value
if (either.Left(out var leftValue, out var _)) {
    // Left case
    Console.WriteLine($"Left: {leftValue}");
}

// Extract Right value
if (either.Right(out var _, out var rightValue)) {
    // Right case
    Console.WriteLine($"Right: {rightValue}");
}
```

## Pattern Matching

```csharp
// Match with actions
either.Match(
    leftAction: left => Console.WriteLine($"Left: {left}"),
    rightAction: right => Console.WriteLine($"Right: {right}")
);

// Match with functions
string message = either.Match(
    leftFunc: left => $"Left: {left}",
    rightFunc: right => $"Right: {right}"
);
```

## Functional Composition with Bind

```csharp
// Transform an Either using Bind
Either<string, int> originalEither = Either<string, int>.FromRight(42);

Either<string, string> transformedEither = originalEither.Bind(
    leftFunc: left => Either<string, string>.FromLeft(left),
    rightFunc: right => Either<string, string>.FromRight(right.ToString())
);
```

## Chaining Operations (Happy Path)

```csharp
// Example of chaining operations with Either
Either<Error, User> GetUser(string userId) {
    // Implementation that returns Either<Error, User>
}

Either<Error, Order> GetLatestOrder(User user) {
    // Implementation that returns Either<Error, Order>
}

Either<Error, ShippingInfo> GetShippingInfo(Order order) {
    // Implementation that returns Either<Error, ShippingInfo>
}

// Chain operations, handling Left cases automatically
Either<Error, ShippingInfo> GetUserShippingInfo(string userId) {
    return GetUser(userId)
        .Bind(
            leftFunc: error => Either<Error, ShippingInfo>.FromLeft(error),
            rightFunc: user => GetLatestOrder(user)
                .Bind(
                    leftFunc: error => Either<Error, ShippingInfo>.FromLeft(error),
                    rightFunc: order => GetShippingInfo(order)
                )
        );
}
```

## Understanding Either as a Monoid

A monoid in functional programming is a type with:
1. An **associative binary operation** (combining two values produces another value of the same type)
2. An **identity element** (a neutral value that doesn't change other values when combined with them)

The `Either<TLeft, TRight>` type forms a monoid where:
- The binary operation is the `Bind` method, which allows you to chain operations
- The identity element is a `Right` value (the "correct" or primary case)

This monoid structure enables elegant handling of branching logic throughout a chain of operations.

### Visualizing the Either chain

When you chain operations with `Bind`, you're creating a pipeline that:

1. Propagates any `Left` value through the entire chain if encountered
2. Continues processing with `Right` values only if each step returns a `Right`

```
GetUser("user123") → Right(user)
    ↓
GetLatestOrder(user) → Right(order)
    ↓
GetShippingInfo(order) → Right(shippingInfo)
```

If any step had returned a `Left` value (e.g., an error), the chain would short-circuit and return that `Left` value immediately, without executing the remaining operations.

### Comparison with the other monoids

The three monoids in this library follow similar patterns but serve different purposes:

| Type          | Left/None case  | Right/Some case    | Typical use case         |
| ------------- | --------------- | ------------------ | ------------------------ |
| `Result<T>`   | Error           | Success with value | Operations that can fail |
| `Option<T>`   | None (no value) | Some (has value)   | Optional values          |
| `Either<L,R>` | Any left type   | Any right type     | Dual outcomes            |

All three types allow for elegant chaining of operations with early termination when necessary, which is the essence of their monoid behavior.
