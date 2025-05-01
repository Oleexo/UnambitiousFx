---
title: Option
parent: Core
nav_order: 2
---

# Option

The `Option<T>` type represents an optional value that may or may not be present. It's a safer alternative to null references, making the possibility of missing values explicit in your code.

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