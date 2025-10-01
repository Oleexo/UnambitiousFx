---
title: xUnit Assertions
parent: Core
nav_order: 4
has_children: true
---

# xUnit Assertions Overview
{: .no_toc }

## Table of contents
{: .no_toc .text-delta }

1. TOC
{:toc}

---

## What are xUnit Assertions?

The `UnambitiousFx.Core.XUnit` library provides assertion helpers for testing `Result`, `Option`, and `Either` types with xUnit.net v3. These helpers offer both classic assertion styles and fluent/predicate-based chaining for expressive, low-ceremony tests.

### Why use specialized assertions?

- **Type-safe testing**: Assert on functional types without manual unwrapping
- **Fluent syntax**: Chain assertions for readable test flows
- **Predicate-based validation**: Express invariants concisely
- **Async support**: First-class support for Task and ValueTask
- **Clear failure messages**: Concise, actionable test failure reports

---

## Installation

### Project Reference
Add a project reference (recommended when working inside the mono-repo):

```xml
<ProjectReference Include="..\..\..\src\Core.XUnit\Core.XUnit.csproj" />
```

### NuGet Package
Or add the NuGet package (when published):

```xml
<PackageReference Include="UnambitiousFx.Core.XUnit" Version="x.y.z" />
```

---

## Quick Start

Add the namespace (a `global using` is convenient):

```csharp
using UnambitiousFx.Core.XUnit;
```

### Classic Style
```csharp
var result = Result.Success(42);
result.ShouldBeSuccess(out var value);
Assert.Equal(42, value);
```

### Fluent Style
```csharp
Result.Success(42)
    .EnsureSuccess()
    .And(v => Assert.Equal(42, v))
    .Map(v => v + 1)
    .And(v => Assert.Equal(43, v));
```

### Predicate Style
```csharp
Result.Success(42).ShouldBeSuccessWhere(v => v > 40);
Option.Some(10).ShouldBeSomeWhere(v => v % 5 == 0);
Either<int,string>.FromLeft(2).ShouldBeLeftWhere(l => l < 5);
```

---

## Assertion Styles

The library provides three complementary assertion styles:

### 1. Classic Assertions
Traditional assertion methods that extract values using `out` parameters:
- Simple and straightforward
- Familiar to xUnit users
- Good for basic value extraction

**Example:**
```csharp
Result.Success(42).ShouldBeSuccess(out var value);
Assert.Equal(42, value);
```

[Learn more about classic assertions →](result-assertions.html#classic-assertions)

### 2. Fluent Assertions
Chainable assertions that return wrapper types for composable testing:
- Enables method chaining
- Supports mapping and filtering
- Keeps test flow readable

**Example:**
```csharp
Result.Success(42)
    .EnsureSuccess()
    .Map(v => v * 2)
    .And(v => Assert.Equal(84, v));
```

[Learn more about fluent assertions →](result-assertions.html#fluent-assertions)

### 3. Predicate Assertions
Property-based assertions using lambda predicates:
- Concise invariant checks
- No manual value extraction
- Clear failure messages

**Example:**
```csharp
Result.Success(42).ShouldBeSuccessWhere(v => v > 40);
```

[Learn more about predicate assertions →](predicate-assertions.html)

---

## Documentation Navigation

Explore xUnit Assertions features in depth:

1. **[Result Assertions](result-assertions.html)** - Testing Result types with classic and fluent assertions
2. **[Option Assertions](option-assertions.html)** - Testing Option types for Some/None cases
3. **[Either Assertions](either-assertions.html)** - Testing Either types for Left/Right branches
4. **[Predicate Assertions](predicate-assertions.html)** - Property-based testing with predicates
5. **[Async Assertions](async-assertions.html)** - Testing Task and ValueTask with assertions

---

## Key Features

### Multi-Arity Result Support
Test Result types with multiple values (Result<T1, T2, ...>):

```csharp
Result.Success(1, "a", true)
    .ShouldBeSuccess((i, s, b) => {
        Assert.Equal(1, i);
        Assert.Equal("a", s);
        Assert.True(b);
    });
```

### Async/Await Support
All assertions have async variants for Task and ValueTask:

```csharp
await Task.FromResult(Result.Success(42))
    .ShouldBeSuccess(out var value);

await GetUserAsync()
    .EnsureSuccess()
    .And(user => Assert.Equal("John", user.Name));
```

### Allocation-Light Design
Wrapper types are `readonly struct` to minimize allocations:

```csharp
// SuccessAssertion<T> is a struct wrapper
var assertion = Result.Success(42).EnsureSuccess();
assertion.Map(v => v * 2).And(v => Assert.Equal(84, v));
```

### Source-Generated Predicates
Result predicate methods are source-generated for all arities (1-8):

```csharp
// Generated for Result<T1>, Result<T1,T2>, ..., Result<T1,...,T8>
Result.Success(1, 2, 3)
    .ShouldBeSuccessWhere(t => t.Item1 + t.Item2 + t.Item3 == 6);
```

---

## Design Philosophy

The xUnit assertion helpers follow these principles:

1. **Type Safety**: Leverage the type system to prevent testing mistakes
2. **Expressiveness**: Make test intent clear and readable
3. **Performance**: Minimize allocations with struct wrappers
4. **Consistency**: Uniform API across Result, Option, and Either
5. **Composability**: Support chaining and transformation within tests

---

## Failure Messages

All helpers emit concise failure messages for quick diagnosis:

```csharp
// Example failure output:
// "Expected success result but was failure with message: Invalid input"
Result.Failure<int>(new Exception("Invalid input"))
    .ShouldBeSuccess();
```

Messages are designed to balance clarity and brevity for high test signal.

---

## Best Practices

### 1. Choose the Right Style
- Use **classic** for simple value extraction
- Use **fluent** for complex test flows with transformations
- Use **predicate** for concise property validation

### 2. Add Context with Messages
Use the optional `because` parameter for clarity:

```csharp
Result.Success(5)
    .ShouldBeSuccessWhere(v => v == 5, "Expected computed value to be 5");
```

### 3. Leverage Async Variants
Use async assertions for async operations:

```csharp
await GetUserAsync()
    .ShouldBeSuccessWhereAsync(user => user.Age >= 18);
```

### 4. Chain Fluent Assertions
Build readable test flows:

```csharp
Result.Success(50)
    .EnsureSuccess()
    .Where(v => v == 50)
    .Map(v => v + 1)
    .Where(v => v == 51)
    .And(v => Assert.Equal(51, v));
```

---

## See Also

- **[Result](../result/index.html)** - Core Result type documentation
- **[Option](../option/index.html)** - Core Option type documentation
- **[Either](../either/index.html)** - Core Either type documentation

---

## License

MIT
