---
title: Result Assertions
parent: xUnit Assertions
nav_order: 1
---

# Result Assertions
{: .no_toc }

## Table of contents
{: .no_toc .text-delta }

1. TOC
{:toc}

---

## Overview

Result assertions provide specialized testing helpers for the `Result` and `Result<T>` types. They support both single-value and multi-arity results (tuples) with three assertion styles: classic, fluent, and predicate-based.

---

## Classic Assertions

Classic assertions follow traditional xUnit patterns, extracting values using `out` parameters.

### Success Assertions

#### Non-Generic Result

Test a `Result` (non-generic) for success:

```csharp
var result = Result.Success();
result.ShouldBeSuccess();
```

#### Single-Value Result

Test a `Result<T>` and extract the value:

```csharp
var result = Result.Success(42);
result.ShouldBeSuccess(out var value);
Assert.Equal(42, value);
```

#### Multi-Arity Result

Test results with multiple values using action callbacks:

```csharp
// Result with 2 values
Result.Success(1, "hello")
    .ShouldBeSuccess((i, s) => {
        Assert.Equal(1, i);
        Assert.Equal("hello", s);
    });

// Result with 3 values
Result.Success(1, "a", true)
    .ShouldBeSuccess((i, s, b) => {
        Assert.Equal(1, i);
        Assert.Equal("a", s);
        Assert.True(b);
    });

// Up to 8 values supported
Result.Success(1, 2, 3, 4, 5, 6, 7, 8)
    .ShouldBeSuccess((v1, v2, v3, v4, v5, v6, v7, v8) => {
        Assert.Equal(1, v1);
        Assert.Equal(8, v8);
    });
```

### Failure Assertions

#### Basic Failure Check

Test that a Result is a failure:

```csharp
Result.Failure<int>(new Exception("boom"))
    .ShouldBeFailure();
```

#### Extract Exception

Extract the exception for further assertions:

```csharp
Result.Failure<int>(new Exception("boom"))
    .ShouldBeFailure(out var exception);
Assert.Equal("boom", exception.Message);
```

#### Message Validation

Test the failure message directly:

```csharp
Result.Failure<int>(new Exception("boom"))
    .ShouldBeFailureWithMessage("boom");
```

#### Combined Extraction and Validation

```csharp
Result.Failure<int>(new InvalidOperationException("Invalid state"))
    .ShouldBeFailure(out var ex);

Assert.IsType<InvalidOperationException>(ex);
Assert.Equal("Invalid state", ex.Message);
```

---

## Fluent Assertions

Fluent assertions return wrapper types that enable method chaining, mapping, and filtering.

### Success Fluent Chain

#### Basic Success Assertion

```csharp
Result.Success(42)
    .EnsureSuccess()  // Returns SuccessAssertion<int>
    .And(v => Assert.Equal(42, v));
```

#### Chaining with Map

Transform the value within the assertion chain:

```csharp
Result.Success(10)
    .EnsureSuccess()
    .Map(v => v * 2)
    .And(v => Assert.Equal(20, v))
    .Map(v => v.ToString())
    .And(s => Assert.Equal("20", s));
```

#### Using Where for Filtering

Add predicate checks within the chain:

```csharp
Result.Success(50)
    .EnsureSuccess()
    .Where(v => v > 40)
    .Map(v => v + 10)
    .Where(v => v == 60)
    .And(v => Assert.Equal(60, v));
```

#### Multi-Arity Success Assertion

Multi-arity results wrap the tuple:

```csharp
var (tuple) = Result.Success(1, "a")
    .EnsureSuccess(); // tuple is (int, string)

Assert.Equal(1, tuple.Item1);
Assert.Equal("a", tuple.Item2);

// Or chain directly
Result.Success(5, 10)
    .EnsureSuccess()
    .And(t => Assert.Equal(15, t.Item1 + t.Item2));
```

### Failure Fluent Chain

#### Basic Failure Assertion

```csharp
Result.Failure<int>(new Exception("boom"))
    .EnsureFailure()  // Returns FailureAssertion
    .And(e => Assert.Equal("boom", e.Message));
```

#### Message Validation

```csharp
Result.Failure<int>(new Exception("boom"))
    .EnsureFailure()
    .AndMessage("boom");
```

#### Chaining Multiple Checks

```csharp
Result.Failure<int>(new InvalidOperationException("Invalid"))
    .EnsureFailure()
    .And(e => Assert.IsType<InvalidOperationException>(e))
    .AndMessage("Invalid");
```

#### Where Predicate on Exception

```csharp
Result.Failure<int>(new Exception("boom"))
    .EnsureFailure()
    .Where(e => e.Message == "boom")
    .And(e => Assert.Contains("boom", e.Message));
```

---

## Usage Examples

### Testing Service Methods

```csharp
[Fact]
public void ValidateUser_WithValidData_ReturnsSuccess()
{
    // Arrange
    var user = new User { Name = "John", Age = 30 };
    var validator = new UserValidator();

    // Act
    var result = validator.Validate(user);

    // Assert - Classic style
    result.ShouldBeSuccess(out var validatedUser);
    Assert.Equal("John", validatedUser.Name);
}

[Fact]
public void ValidateUser_WithInvalidData_ReturnsFailure()
{
    // Arrange
    var user = new User { Name = "", Age = -5 };
    var validator = new UserValidator();

    // Act
    var result = validator.Validate(user);

    // Assert - Fluent style
    result
        .EnsureFailure()
        .And(e => Assert.IsType<ValidationException>(e))
        .AndMessage("Invalid user data");
}
```

### Testing Complex Workflows

```csharp
[Fact]
public void ProcessOrder_WithValidOrder_ReturnsOrderId()
{
    // Arrange
    var order = CreateValidOrder();
    var processor = new OrderProcessor();

    // Act
    var result = processor.Process(order);

    // Assert - Fluent with transformations
    result
        .EnsureSuccess()
        .Where(orderId => orderId > 0)
        .Map(orderId => orderId.ToString())
        .And(id => Assert.Matches(@"^\d+$", id));
}
```

### Testing Multi-Arity Results

```csharp
[Fact]
public void CreateUser_WithValidData_ReturnsUserAndToken()
{
    // Arrange
    var service = new UserService();

    // Act
    var result = service.CreateUser("John", "john@example.com");

    // Assert - Multi-arity with callback
    result.ShouldBeSuccess((user, token) => {
        Assert.Equal("John", user.Name);
        Assert.Equal("john@example.com", user.Email);
        Assert.NotEmpty(token);
    });
}

[Fact]
public void GetUserProfile_ReturnsMultipleValues()
{
    // Arrange
    var service = new ProfileService();

    // Act
    var result = service.GetProfile(userId: 1);

    // Assert - Fluent with tuple
    result
        .EnsureSuccess()
        .And(t => {
            var (user, settings, preferences) = t;
            Assert.NotNull(user);
            Assert.NotNull(settings);
            Assert.NotNull(preferences);
        });
}
```

### Testing Error Handling

```csharp
[Fact]
public void DivideNumbers_WithZeroDivisor_ReturnsFailure()
{
    // Arrange
    var calculator = new Calculator();

    // Act
    var result = calculator.Divide(10, 0);

    // Assert - Classic with message check
    result
        .ShouldBeFailure(out var ex)
        .ShouldBeFailureWithMessage("Division by zero");

    Assert.IsType<DivideByZeroException>(ex);
}

[Fact]
public void ParseJson_WithInvalidJson_ReturnsFailureWithDetails()
{
    // Arrange
    var parser = new JsonParser();

    // Act
    var result = parser.Parse("{ invalid json }");

    // Assert - Fluent with predicate
    result
        .EnsureFailure()
        .Where(e => e is JsonException)
        .And(e => Assert.Contains("invalid", e.Message.ToLower()));
}
```

---

## Design Notes

### Wrapper Types

Fluent assertions use lightweight wrapper structs:

- **`SuccessAssertion<T>`**: Wraps a success value of type `T`
- **`FailureAssertion`**: Wraps an exception

These are `readonly struct` types to minimize allocations.

### Multi-Arity Support

Multi-arity results (Result<T1, T2, ...>) are treated as tuples:

```csharp
// The value is a tuple
Result<int, string> result = Result.Success(1, "a");
var (num, str) = result.EnsureSuccess(); // Deconstruct the tuple
```

### Method Signatures

```csharp
// Classic assertions
public static void ShouldBeSuccess(this Result result);
public static void ShouldBeSuccess<T>(this Result<T> result, out T value);
public static void ShouldBeSuccess<T1, T2>(this Result<T1, T2> result, Action<T1, T2> assert);
public static void ShouldBeFailure<T>(this Result<T> result);
public static void ShouldBeFailure<T>(this Result<T> result, out Exception exception);
public static void ShouldBeFailureWithMessage<T>(this Result<T> result, string expectedMessage);

// Fluent assertions
public static SuccessAssertion<T> EnsureSuccess<T>(this Result<T> result);
public static FailureAssertion EnsureFailure<T>(this Result<T> result);
```

---

## Best Practices

1. **Choose the right style**:
   - Use classic for simple value extraction
   - Use fluent for complex test flows
   - Use predicates (see [Predicate Assertions](predicate-assertions.html)) for property validation

2. **Chain fluent assertions** for readable test flows
3. **Use Where** to add inline predicate checks
4. **Use Map** to transform values within the test
5. **Extract and validate** exceptions in failure cases

---

## See Also

- **[Predicate Assertions](predicate-assertions.html)** - Property-based Result testing
- **[Async Assertions](async-assertions.html)** - Testing async Result operations
- **[Result Documentation](../result/index.html)** - Core Result type reference
