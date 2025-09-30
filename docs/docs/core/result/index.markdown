---
title: Result
parent: Core
nav_order: 1
has_children: true
---

# Result Overview
{: .no_toc }

## Table of contents
{: .no_toc .text-delta }

1. TOC
{:toc}

---

## What is Result?

The `Result` type is a functional programming pattern that represents the outcome of an operation that can either **succeed** or **fail**. Instead of throwing exceptions for expected failures, `Result` makes success and failure explicit parts of your type system.

Think of `Result` as a container that holds either:
- **Success**: A value (or confirmation of success for operations without return values)
- **Failure**: An error with rich contextual information

### Result Variants

UnambitiousFx provides three main Result variants:

| Type                  | Description                           | Example                       |
| --------------------- | ------------------------------------- | ----------------------------- |
| `Result`              | Success/failure without a value       | Database delete operation     |
| `Result<T>`           | Success with single value of type `T` | Parsing a string to integer   |
| `Result<T1, T2, ...>` | Success with multiple values (tuple)  | Parsing name and age together |

---

## Why Use Result?

### Pros ✅

**1. Explicit Error Handling**
- Failures are part of the type signature, making APIs self-documenting
- No hidden exceptions to discover at runtime
- Compiler enforces checking for both success and failure cases

**2. Composable Operations**
- Chain multiple operations that might fail
- Short-circuit on first failure automatically
- Clean, readable pipeline-style code

**3. Rich Error Context**
- Structured error information (codes, messages, metadata)
- Multiple errors can be accumulated
- Preserve error chains without losing information

**4. Better Performance**
- Avoid exception overhead for expected failures
- No stack unwinding for control flow
- More predictable performance characteristics

**5. Type Safety**
- Leverage compiler to ensure proper error handling
- Prevent forgetting to check for errors
- Refactoring safety when changing return types

### Cons ⚠️

**1. Learning Curve**
- Requires understanding functional programming concepts (Map, Bind, etc.)
- Team members need to learn new patterns

**2. Verbose for Simple Cases**
- Overkill for operations that truly never fail
- More code compared to simple try-catch blocks

**3. Integration Challenges**
- May need adapters when working with traditional exception-based APIs
- Not all .NET libraries follow this pattern

**4. Stack Traces**
- Wrapped exceptions don't throw, so immediate stack traces require inspection
- Debugging might need adjusting your mental model

---

## Quick Example

Here's a practical example showing Result in action:

```csharp
using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Reasons;

// Define operations that can fail
Result<User> FindUser(string userId)
{
    if (string.IsNullOrEmpty(userId))
        return Result.Failure<User>(new ValidationError("User ID cannot be empty"));
    
    var user = _database.FindById(userId);
    return user != null 
        ? Result.Success(user)
        : Result.Failure<User>(new NotFoundError($"User {userId} not found"));
}

Result<decimal> GetAccountBalance(User user)
{
    if (!user.IsActive)
        return Result.Failure<decimal>(new UnauthorizedError("User account is inactive"));
    
    return Result.Success(user.AccountBalance);
}

Result<string> FormatCurrency(decimal amount)
{
    try
    {
        return Result.Success($"${amount:N2}");
    }
    catch (Exception ex)
    {
        return Result.Failure<string>(new ExceptionalError(ex));
    }
}

// Compose operations - automatically short-circuits on first failure
Result<string> GetFormattedBalance(string userId)
{
    return FindUser(userId)
        .Bind(user => GetAccountBalance(user))        // Only runs if FindUser succeeds
        .Bind(balance => FormatCurrency(balance))     // Only runs if GetAccountBalance succeeds
        .Tap(formatted => _logger.LogInfo($"Balance: {formatted}"))
        .MapError(error => error.WithMetadata("userId", userId)); // Add context to any error
}

// Use the result
var result = GetFormattedBalance("user-123");

result.Match(
    success: balance => Console.WriteLine($"Balance: {balance}"),
    failure: error => Console.WriteLine($"Error: {error.Message}")
);

// Or extract value with fallback
var display = result.ValueOr("Balance unavailable");
```

### What This Example Shows

- **No exceptions thrown** for expected failures (not found, validation, etc.)
- **Automatic short-circuiting**: If any step fails, subsequent steps are skipped
- **Clean composition**: Each operation builds on the previous one
- **Rich error context**: Different error types (NotFoundError, ValidationError, etc.)
- **Metadata attachment**: Add contextual information to errors
- **Multiple ways to consume**: Match, ValueOr, or other access patterns

---

## Documentation Navigation

Explore Result features in depth:

1. **[Result Creation & Basics](creation.html)** - Start here to learn Result fundamentals
2. **[Transformations](transformations.html)** - Map, Bind, SelectMany, Flatten
3. **[Error Handling](error-handling.html)** - Error model, domain errors, recovery
4. **[Validation & Side Effects](validation.html)** - Ensure, Tap, TapError
5. **[Async Operations](async.html)** - Working with Task and ValueTask
6. **[Collections & Aggregation](collections.html)** - Batch operations, Traverse, Combine
7. **[Value Access & Interop](value-access.html)** - Extracting values safely
8. **[Roadmap](roadmap.html)** - Full feature status and timeline

---

## Getting Started

Ready to use Result in your project?

1. **Install the package**:
   ```bash
   dotnet add package UnambitiousFx.Core
   ```

2. **Add using statement**:
   ```csharp
   using UnambitiousFx.Core.Results;
   using UnambitiousFx.Core.Results.Reasons;
   ```

3. **Start with [Result Creation](creation.html)** to learn the basics

4. **Explore [patterns and examples](#quick-example)** in each feature page

---

## Contributing

Found an issue or have a feature request? Visit our [GitHub repository](https://github.com/oleexo/UnambitiousFx) to contribute or report issues.

The Result roadmap is actively maintained - see [ResultFeatures.md](https://github.com/oleexo/UnambitiousFx/blob/main/ResultFeatures.md) for detailed status and planning.
