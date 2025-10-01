---
title: Predicate Assertions
parent: xUnit Assertions
nav_order: 4
---

# Predicate Assertions
{: .no_toc }

## Table of contents
{: .no_toc .text-delta }

1. TOC
{:toc}

---

## Overview

Predicate assertions provide property-based testing for `Result`, `Option`, and `Either` types using lambda predicates. They offer a concise way to express invariant checks without manual value extraction, with clear failure messages and support for custom error descriptions.

### Why Use Predicate Assertions?

- **Concise**: Express conditions directly without extracting values
- **Clear intent**: Predicates make test expectations explicit
- **Automatic short-circuiting**: Fail fast with meaningful messages
- **Custom messages**: Optional `because` parameter for context
- **Type-safe**: Leverage the type system for correct assertions

---

## Result Predicate Assertions

### Single-Value Result

#### ShouldBeSuccessWhere

Test that a Result is successful and the value satisfies a predicate:

```csharp
Result.Success(42).ShouldBeSuccessWhere(v => v > 40);
```

#### With Custom Message

```csharp
Result.Success(5).ShouldBeSuccessWhere(
    v => v == 5, 
    because: "Expected computed value to be 5"
);
```

#### Complex Predicates

```csharp
Result.Success("hello")
    .ShouldBeSuccessWhere(s => s.Length > 0 && s.StartsWith("h"));

Result.Success(100)
    .ShouldBeSuccessWhere(v => v % 10 == 0);
```

#### ShouldBeFailureWhere

Test that a Result is a failure and the exception satisfies a predicate:

```csharp
Result.Failure<int>(new Exception("boom"))
    .ShouldBeFailureWhere(ex => ex.Message == "boom");
```

#### With Type Checking

```csharp
Result.Failure<int>(new InvalidOperationException())
    .ShouldBeFailureWhere(ex => ex is InvalidOperationException);

Result.Failure<string>(new ArgumentException("Invalid arg"))
    .ShouldBeFailureWhere(ex => 
        ex is ArgumentException && ex.Message.Contains("Invalid")
    );
```

### Multi-Arity Result

For multi-arity results, the predicate receives a tuple:

```csharp
// Result<int, int, int>
Result.Success(1, 2, 3)
    .ShouldBeSuccessWhere(t => t.Item1 + t.Item2 + t.Item3 == 6);

// Result<string, int>
Result.Success("hello", 5)
    .ShouldBeSuccessWhere(t => t.Item1.Length == t.Item2);

// Result with 4 values
Result.Success(1, 2, 3, 4)
    .ShouldBeSuccessWhere(t => 
        t.Item1 < t.Item2 && 
        t.Item3 < t.Item4
    );
```

#### Tuple Deconstruction

```csharp
Result.Success(10, 20)
    .ShouldBeSuccessWhere(t => {
        var (a, b) = t;
        return a + b == 30;
    });

Result.Success("user", 42, true)
    .ShouldBeSuccessWhere(t => {
        var (name, age, active) = t;
        return name.Length > 0 && age > 0 && active;
    });
```

#### Failure with Multi-Arity

```csharp
Result.Failure<int, int>(new InvalidOperationException())
    .ShouldBeFailureWhere(ex => ex is InvalidOperationException);
```

### Source-Generated Support

Predicate methods are source-generated for arities 1-8:

```csharp
// All of these are generated
Result.Success(1).ShouldBeSuccessWhere(v => v > 0);
Result.Success(1, 2).ShouldBeSuccessWhere(t => t.Item1 + t.Item2 > 0);
Result.Success(1, 2, 3).ShouldBeSuccessWhere(t => t.Item1 + t.Item2 + t.Item3 > 0);
// ... up to Result.Success(v1, v2, v3, v4, v5, v6, v7, v8)
```

---

## Option Predicate Assertions

### ShouldBeSomeWhere

Test that an Option has a value and it satisfies a predicate:

```csharp
Option.Some(10).ShouldBeSomeWhere(v => v % 5 == 0);
```

#### With Custom Message

```csharp
Option.Some(42).ShouldBeSomeWhere(
    v => v == 42,
    because: "Expected the answer to everything"
);
```

#### Complex Predicates

```csharp
Option.Some("hello")
    .ShouldBeSomeWhere(s => s.Length > 3 && s.Contains("ll"));

Option.Some(DateTime.Now)
    .ShouldBeSomeWhere(dt => dt.Year == 2025);

Option.Some(new User { Age = 25 })
    .ShouldBeSomeWhere(u => u.Age >= 18);
```

### ShouldBeNoneWhere

Test that an Option is None and an additional condition holds:

```csharp
Option<int>.None().ShouldBeNoneWhere(() => true);
```

#### Use Cases

```csharp
// Verify None with context check
var cache = new Cache();
Option<string> result = cache.Get("nonexistent");

result.ShouldBeNoneWhere(() => !cache.Contains("nonexistent"));
```

#### With Custom Message

```csharp
Option<User>.None().ShouldBeNoneWhere(
    () => true,
    because: "User should not exist in test database"
);
```

---

## Either Predicate Assertions

### ShouldBeLeftWhere

Test that an Either is Left and the value satisfies a predicate:

```csharp
Either<int, string>.FromLeft(2).ShouldBeLeftWhere(l => l < 5);
```

#### With Custom Message

```csharp
Either<string, User>.FromLeft("Not found")
    .ShouldBeLeftWhere(
        msg => msg.Contains("Not"),
        because: "Expected user lookup to fail"
    );
```

#### Complex Predicates

```csharp
Either<List<string>, User>.FromLeft(new List<string> { "a", "b" })
    .ShouldBeLeftWhere(list => list.Count == 2);

Either<Error, Result>.FromLeft(new Error { Code = 404 })
    .ShouldBeLeftWhere(err => err.Code >= 400 && err.Code < 500);
```

### ShouldBeRightWhere

Test that an Either is Right and the value satisfies a predicate:

```csharp
Either<int, string>.FromRight("abc").ShouldBeRightWhere(r => r.Length == 3);
```

#### With Custom Message

```csharp
Either<Error, User>.FromRight(new User { Name = "John" })
    .ShouldBeRightWhere(
        u => u.Name == "John",
        because: "Expected successful user creation"
    );
```

#### Complex Predicates

```csharp
Either<string, int>.FromRight(100)
    .ShouldBeRightWhere(n => n > 0 && n % 10 == 0);

Either<Error, SearchResult>.FromRight(new SearchResult { Count = 5 })
    .ShouldBeRightWhere(result => result.Count > 0 && result.Count < 10);
```

---

## Usage Examples

### Testing Computed Values

```csharp
[Fact]
public void Calculate_WithValidInput_ReturnsExpectedResult()
{
    // Arrange
    var calculator = new Calculator();

    // Act
    var result = calculator.Divide(100, 5);

    // Assert - Predicate style
    result.ShouldBeSuccessWhere(v => v == 20);
}

[Fact]
public void Compute_ReturnsValueInRange()
{
    var result = service.Compute();
    result.ShouldBeSuccessWhere(
        v => v >= 1 && v <= 100,
        because: "Computed value should be in valid range"
    );
}
```

### Testing Validation Logic

```csharp
[Fact]
public void ValidateAge_WithValidAge_Passes()
{
    var result = validator.ValidateAge(25);
    result.ShouldBeSuccessWhere(age => age >= 18 && age <= 120);
}

[Fact]
public void ValidateEmail_WithInvalidEmail_Fails()
{
    var result = validator.ValidateEmail("invalid");
    result.ShouldBeFailureWhere(ex => 
        ex is ValidationException && 
        ex.Message.Contains("email")
    );
}
```

### Testing Multi-Value Results

```csharp
[Fact]
public void CreateUser_ReturnsValidUserAndToken()
{
    var result = service.CreateUser("John", "john@example.com");
    
    result.ShouldBeSuccessWhere(t => {
        var (user, token) = t;
        return user.Name == "John" && 
               user.Email == "john@example.com" &&
               !string.IsNullOrEmpty(token);
    });
}

[Fact]
public void GetCoordinates_ReturnsValidLatLong()
{
    var result = geoService.GetCoordinates("New York");
    
    result.ShouldBeSuccessWhere(t => {
        var (lat, lon) = t;
        return lat >= -90 && lat <= 90 &&
               lon >= -180 && lon <= 180;
    });
}
```

### Testing Optional Values

```csharp
[Fact]
public void FindUser_WhenExists_ReturnsSomeWithValidData()
{
    var result = repository.FindUser("john");
    result.ShouldBeSomeWhere(u => 
        u.Username == "john" && 
        u.Email != null
    );
}

[Fact]
public void GetConfig_WhenMissing_ReturnsNone()
{
    var result = config.Get("nonexistent");
    result.ShouldBeNoneWhere(() => !config.Exists("nonexistent"));
}
```

### Testing Either Branches

```csharp
[Fact]
public void ProcessPayment_WithValidCard_ReturnsRightWithTransactionId()
{
    var result = processor.ProcessPayment(validCard);
    result.ShouldBeRightWhere(txId => 
        txId.Length == 32 && 
        txId.All(char.IsLetterOrDigit)
    );
}

[Fact]
public void ProcessPayment_WithInvalidCard_ReturnsLeftWithError()
{
    var result = processor.ProcessPayment(invalidCard);
    result.ShouldBeLeftWhere(err => 
        err.Code == "INVALID_CARD" && 
        err.Message != null
    );
}
```

### Testing Business Rules

```csharp
[Fact]
public void CalculateDiscount_AppliesCorrectPercentage()
{
    var result = service.CalculateDiscount(totalAmount: 1000, membershipLevel: "Gold");
    
    result.ShouldBeSuccessWhere(
        discount => discount >= 100 && discount <= 200,
        because: "Gold members should get 10-20% discount"
    );
}

[Fact]
public void ValidateOrder_ChecksAllInvariants()
{
    var result = validator.ValidateOrder(order);
    
    result.ShouldBeSuccessWhere(validOrder =>
        validOrder.Items.Count > 0 &&
        validOrder.Total > 0 &&
        validOrder.CustomerId > 0,
        because: "Valid orders must have items, positive total, and customer"
    );
}
```

---

## Combining with Fluent Assertions

Predicate assertions can be combined with fluent assertions:

```csharp
[Fact]
public void ComplexValidation_UsesMultipleStyles()
{
    var result = service.ProcessData(input);
    
    // Start with predicate
    result.ShouldBeSuccessWhere(v => v > 0);
    
    // Continue with fluent
    result
        .EnsureSuccess()
        .Where(v => v % 2 == 0)
        .Map(v => v * 2)
        .And(v => Assert.Equal(expected, v));
}
```

---

## Design Notes

### Method Signatures

```csharp
// Result predicates
public static void ShouldBeSuccessWhere<T>(
    this Result<T> result, 
    Func<T, bool> predicate, 
    string? because = null);

public static void ShouldBeFailureWhere<T>(
    this Result<T> result, 
    Func<Exception, bool> predicate, 
    string? because = null);

// Option predicates
public static void ShouldBeSomeWhere<T>(
    this Option<T> option, 
    Func<T, bool> predicate, 
    string? because = null);

public static void ShouldBeNoneWhere<T>(
    this Option<T> option, 
    Func<bool> predicate, 
    string? because = null);

// Either predicates
public static void ShouldBeLeftWhere<TLeft, TRight>(
    this Either<TLeft, TRight> either, 
    Func<TLeft, bool> predicate, 
    string? because = null);

public static void ShouldBeRightWhere<TLeft, TRight>(
    this Either<TLeft, TRight> either, 
    Func<TRight, bool> predicate, 
    string? because = null);
```

### Failure Messages

Predicate assertions emit clear failure messages:

```csharp
// Example failure output:
// "Expected success result where predicate holds, but was failure."
// "Expected success result where predicate holds, but predicate returned false. Expected computed value to be 5"
```

### Source Generation

Result predicate methods for arities 2-8 are source-generated to avoid manual duplication. The generator can be adjusted if you need support for more than 8 values.

---

## Best Practices

1. **Use predicates for property validation** - Express invariants directly
2. **Add custom messages** - Use the `because` parameter for context
3. **Keep predicates simple** - Complex logic can be hard to debug
4. **Combine styles** - Use predicates with fluent assertions when needed
5. **Test edge cases** - Verify boundary conditions with predicates
6. **Use meaningful names** - Make test intent clear

---

## Common Patterns

### Range Validation

```csharp
result.ShouldBeSuccessWhere(v => v >= min && v <= max);
```

### Property Checks

```csharp
result.ShouldBeSuccessWhere(user => 
    user.Name != null && 
    user.Email.Contains("@")
);
```

### Type Validation

```csharp
result.ShouldBeFailureWhere(ex => ex is ValidationException);
```

### Collection Checks

```csharp
result.ShouldBeSuccessWhere(list => 
    list.Count > 0 && 
    list.All(x => x > 0)
);
```

---

## See Also

- **[Result Assertions](result-assertions.html)** - Classic and fluent Result assertions
- **[Option Assertions](option-assertions.html)** - Classic and fluent Option assertions
- **[Either Assertions](either-assertions.html)** - Classic and fluent Either assertions
- **[Async Assertions](async-assertions.html)** - Async predicate variants
