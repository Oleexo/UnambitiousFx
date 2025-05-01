---
title: Either
parent: Core
nav_order: 3
---

# Either

The `Either<TLeft, TRight>` type represents a value that can be one of two possible types. It's often used to represent a computation that can result in different types of values, with the convention that `Right` represents the "correct" or primary case, and `Left` represents an alternative or error case.

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