---
title: Option
parent: Core
nav_order: 2
has_children: true
---

# Option Overview

## What is an Option?

The `Option<T>` type represents an optional value that may or may not be present. It's a safer alternative to null references, making the possibility of missing values explicit in your code.

### Why use Option instead of null?

- **Null safety**: Eliminates null reference exceptions
- **Explicit optionality**: Makes it clear in your method signatures when a value might be missing
- **Composability**: Options can be easily combined and chained together
- **Forced handling**: Encourages handling the "no value" case explicitly

### Real-world example

Consider a user profile system where some fields might be optional:

```csharp
// Traditional approach with nulls
public class UserProfile {
    public string Name { get; set; }  // Required
    public string? MiddleName { get; set; }  // Optional
    public DateTime? DateOfBirth { get; set; }  // Optional

    public string GetFormattedName() {
        // Need null checks
        if (MiddleName != null) {
            return $"{Name} {MiddleName}";
        }
        return Name;
    }

    public int? GetAge() {
        // Need null checks
        if (DateOfBirth == null) {
            return null;
        }
        return DateTime.Now.Year - DateOfBirth.Value.Year;
    }
}

// Option-based approach
public class UserProfile {
    public string Name { get; set; }  // Required
    public Option<string> MiddleName { get; set; }  // Explicitly optional
    public Option<DateTime> DateOfBirth { get; set; }  // Explicitly optional

    public string GetFormattedName() {
        // Pattern matching makes intent clear
        return MiddleName.Match(
            some: middle => $"{Name} {middle}",
            none: () => Name
        );
    }

    public Option<int> GetAge() {
        // Composable transformations
        return DateOfBirth.Match(
            some: dob => DateTime.Now.Year - dob.Year,
            none: () => Option<int>.None()
        );
    }
}
```

The Option approach makes the optional nature of values explicit and provides elegant ways to handle missing values.

## Creating Options

```csharp
// Create an option with a value (Some)
var someOption = Option<string>.Some("Hello, world!");

// Create an empty option (None)
var noneOption = Option<string>.None();
```

## Checking Option Status

```csharp
// Check if option has a value
if (option.IsSome) {
    // Handle case when value is present
}

// Check if option is empty
if (option.IsNone) {
    // Handle case when value is not present
}

// Extract value in one operation
if (option.Some(out var value)) {
    // Value is present
    Console.WriteLine($"Value: {value}");
} else {
    // Value is not present
    Console.WriteLine("No value");
}
```

## Pattern Matching

```csharp
// Match with actions
option.Match(
    some: value => Console.WriteLine($"Value: {value}"),
    none: () => Console.WriteLine("No value")
);

// Match with functions
string message = option.Match(
    some: value => $"Value: {value}",
    none: () => "No value"
);
```

## Conditional Execution

```csharp
// Execute action only when value is present
option.IfSome(value => {
    Console.WriteLine($"Processing value: {value}");
});

// Execute action only when value is not present
option.IfNone(() => {
    Console.WriteLine("Handling missing value");
});
```

## Chaining Operations (Happy Path)

```csharp
// Example of chaining operations with optional values
Option<User> FindUser(string username) {
    // Implementation that returns Option<User>
}

Option<Address> GetPrimaryAddress(User user) {
    // Implementation that returns Option<Address>
}

Option<string> FormatAddress(Address address) {
    // Implementation that returns Option<string>
}

// Chain operations, handling None cases automatically
Option<string> GetFormattedUserAddress(string username) {
    return FindUser(username)
        .Match(
            some: user => GetPrimaryAddress(user)
                .Match(
                    some: address => FormatAddress(address),
                    none: () => Option<string>.None()
                ),
            none: () => Option<string>.None()
        );
}
```

## Understanding Option as a Monoid

A monoid in functional programming is a type with:
1. An **associative binary operation** (combining two values produces another value of the same type)
2. An **identity element** (a neutral value that doesn't change other values when combined with them)

The `Option<T>` type forms a monoid where:
- The binary operation is the `Match` method when used for chaining operations
- The identity element is `Some` (a present value)

This monoid structure enables elegant handling of potentially missing values throughout a chain of operations.

### Visualizing the Option chain

When you chain operations with `Match`, you're creating a pipeline that:

1. Propagates `None` through the entire chain if any operation returns `None`
2. Continues processing only if each step returns `Some` with a value

```
FindUser("johndoe") → Some(user)
    ↓
GetPrimaryAddress(user) → Some(address)
    ↓
FormatAddress(address) → Some("123 Main St, City")
```

If any step had returned `None`, the chain would short-circuit and return `None` immediately, without executing the remaining operations.

### Simplified chaining with Bind

Many functional libraries also provide a `Bind` method for Option, which simplifies the chaining pattern:

```csharp
Option<string> GetFormattedUserAddress(string username) {
    return FindUser(username)
        .Bind(user => GetPrimaryAddress(user))
        .Bind(address => FormatAddress(address));
}
```

This makes the monoid nature of Option even more apparent, as it closely resembles how we chain operations with Result.
