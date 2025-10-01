---
title: Either Assertions
parent: xUnit Assertions
nav_order: 3
---

# Either Assertions
{: .no_toc }

## Table of contents
{: .no_toc .text-delta }

1. TOC
{:toc}

---

## Overview

Either assertions provide specialized testing helpers for the `Either<TLeft, TRight>` type. They support testing both `Left` and `Right` branches with classic and fluent assertion styles.

---

## Classic Assertions

Classic assertions follow traditional xUnit patterns, extracting values using `out` parameters.

### Left Assertions

Test an Either that contains a Left value:

```csharp
Either<int, string> either = Either<int, string>.FromLeft(42);
either.ShouldBeLeft(out var leftValue);
Assert.Equal(42, leftValue);
```

#### Basic Left Check

```csharp
Either<string, int> either = Either<string, int>.FromLeft("error");
either.ShouldBeLeft();
```

#### Extract and Validate Left Value

```csharp
Either<int, string> either = Either<int, string>.FromLeft(100);
either.ShouldBeLeft(out var value);
Assert.True(value > 50);
```

#### Using Action Callback

```csharp
Either<string, User> either = Either<string, User>.FromLeft("Not found");
either.ShouldBeLeft(left => {
    Assert.Equal("Not found", left);
    Assert.Contains("Not", left);
});
```

### Right Assertions

Test an Either that contains a Right value:

```csharp
Either<int, string> either = Either<int, string>.FromRight("success");
either.ShouldBeRight(out var rightValue);
Assert.Equal("success", rightValue);
```

#### Basic Right Check

```csharp
Either<string, int> either = Either<string, int>.FromRight(42);
either.ShouldBeRight();
```

#### Extract and Validate Right Value

```csharp
Either<Error, User> either = Either<Error, User>.FromRight(new User { Name = "John" });
either.ShouldBeRight(out var user);
Assert.Equal("John", user.Name);
```

#### Using Action Callback

```csharp
Either<Error, int> either = Either<Error, int>.FromRight(42);
either.ShouldBeRight(right => {
    Assert.Equal(42, right);
    Assert.True(right > 0);
});
```

---

## Fluent Assertions

Fluent assertions return wrapper types that enable method chaining, mapping, and filtering.

### Left Fluent Chain

#### Basic Left Assertion

```csharp
Either<int, string>.FromLeft(42)
    .EnsureLeft()  // Returns LeftAssertion<int, string>
    .And(v => Assert.Equal(42, v));
```

#### Chaining with Map

Transform the left value within the assertion chain:

```csharp
Either<int, string>.FromLeft(10)
    .EnsureLeft()
    .Map(v => v * 2)
    .And(v => Assert.Equal(20, v))
    .Map(v => v.ToString())
    .And(s => Assert.Equal("20", s));
```

#### Using WhereLeft for Filtering

Add predicate checks within the chain:

```csharp
Either<int, string>.FromLeft(50)
    .EnsureLeft()
    .WhereLeft(v => v > 40)
    .Map(v => v + 10)
    .WhereLeft(v => v == 60)
    .And(v => Assert.Equal(60, v));
```

#### Complex Left Chaining

```csharp
Either<string, int>.FromLeft("error")
    .EnsureLeft()
    .WhereLeft(s => s.Length > 0)
    .Map(s => s.ToUpper())
    .WhereLeft(s => s == "ERROR")
    .Map(s => s.Length)
    .And(len => Assert.Equal(5, len));
```

### Right Fluent Chain

#### Basic Right Assertion

```csharp
Either<int, string>.FromRight("success")
    .EnsureRight()  // Returns RightAssertion<int, string>
    .And(v => Assert.Equal("success", v));
```

#### Chaining with Map

Transform the right value within the assertion chain:

```csharp
Either<Error, int>.FromRight(10)
    .EnsureRight()
    .Map(v => v * 2)
    .And(v => Assert.Equal(20, v))
    .Map(v => v.ToString())
    .And(s => Assert.Equal("20", s));
```

#### Using WhereRight for Filtering

Add predicate checks within the chain:

```csharp
Either<Error, int>.FromRight(50)
    .EnsureRight()
    .WhereRight(v => v > 40)
    .Map(v => v + 10)
    .WhereRight(v => v == 60)
    .And(v => Assert.Equal(60, v));
```

#### Complex Right Chaining

```csharp
Either<int, string>.FromRight("hello")
    .EnsureRight()
    .WhereRight(s => s.Length > 0)
    .Map(s => s.ToUpper())
    .WhereRight(s => s == "HELLO")
    .Map(s => s.Length)
    .And(len => Assert.Equal(5, len));
```

---

## Usage Examples

### Testing Dual-Outcome Operations

```csharp
[Fact]
public void FindUser_WithExistingUsername_ReturnsRight()
{
    // Arrange
    var service = new UserService();

    // Act - Returns Either<List<string>, User>
    var result = service.FindUser("john");

    // Assert - Classic style
    result.ShouldBeRight(out var user);
    Assert.Equal("john", user.Username);
}

[Fact]
public void FindUser_WithNonExistentUsername_ReturnsLeftWithSuggestions()
{
    // Arrange
    var service = new UserService();

    // Act - Returns Either<List<string>, User>
    var result = service.FindUser("johndoe123");

    // Assert - Classic style
    result.ShouldBeLeft(out var suggestions);
    Assert.NotEmpty(suggestions);
}
```

### Testing Error or Success Scenarios

```csharp
[Fact]
public void ProcessPayment_WithValidCard_ReturnsRightWithTransactionId()
{
    // Arrange
    var processor = new PaymentProcessor();
    var card = CreateValidCard();

    // Act - Returns Either<PaymentError, string>
    var result = processor.ProcessPayment(card, amount: 100);

    // Assert - Fluent style
    result
        .EnsureRight()
        .WhereRight(txId => !string.IsNullOrEmpty(txId))
        .Map(txId => txId.Length)
        .And(len => Assert.True(len > 0));
}

[Fact]
public void ProcessPayment_WithInvalidCard_ReturnsLeftWithError()
{
    // Arrange
    var processor = new PaymentProcessor();
    var card = CreateInvalidCard();

    // Act - Returns Either<PaymentError, string>
    var result = processor.ProcessPayment(card, amount: 100);

    // Assert - Fluent style
    result
        .EnsureLeft()
        .WhereLeft(err => err.Code == "INVALID_CARD")
        .And(err => Assert.Equal("Card validation failed", err.Message));
}
```

### Testing Alternative Paths

```csharp
public class SearchService
{
    // Returns either a direct match (Right) or alternative suggestions (Left)
    public Either<List<string>, SearchResult> Search(string query)
    {
        var exactMatch = FindExactMatch(query);
        if (exactMatch != null)
            return Either<List<string>, SearchResult>.FromRight(exactMatch);

        var suggestions = FindSimilar(query);
        return Either<List<string>, SearchResult>.FromLeft(suggestions);
    }
}

[Fact]
public void Search_WithExactMatch_ReturnsRight()
{
    // Arrange
    var service = new SearchService();

    // Act
    var result = service.Search("apple");

    // Assert
    result
        .EnsureRight()
        .And(match => {
            Assert.Equal("apple", match.Term);
            Assert.True(match.IsExactMatch);
        });
}

[Fact]
public void Search_WithPartialMatch_ReturnsLeftWithSuggestions()
{
    // Arrange
    var service = new SearchService();

    // Act
    var result = service.Search("aple");

    // Assert
    result
        .EnsureLeft()
        .WhereLeft(suggestions => suggestions.Count > 0)
        .And(suggestions => Assert.Contains("apple", suggestions));
}
```

### Testing Validation Results

```csharp
public class Validator
{
    // Returns Either<ValidationErrors, ValidatedData>
    public Either<List<string>, User> ValidateUser(UserInput input)
    {
        var errors = new List<string>();
        
        if (string.IsNullOrEmpty(input.Name))
            errors.Add("Name is required");
        if (input.Age < 18)
            errors.Add("Age must be 18 or older");

        if (errors.Any())
            return Either<List<string>, User>.FromLeft(errors);

        return Either<List<string>, User>.FromRight(
            new User { Name = input.Name, Age = input.Age }
        );
    }
}

[Fact]
public void ValidateUser_WithValidInput_ReturnsRight()
{
    // Arrange
    var validator = new Validator();
    var input = new UserInput { Name = "John", Age = 25 };

    // Act
    var result = validator.ValidateUser(input);

    // Assert
    result
        .EnsureRight()
        .And(user => {
            Assert.Equal("John", user.Name);
            Assert.Equal(25, user.Age);
        });
}

[Fact]
public void ValidateUser_WithInvalidInput_ReturnsLeftWithErrors()
{
    // Arrange
    var validator = new Validator();
    var input = new UserInput { Name = "", Age = 15 };

    // Act
    var result = validator.ValidateUser(input);

    // Assert
    result
        .EnsureLeft()
        .WhereLeft(errors => errors.Count == 2)
        .And(errors => {
            Assert.Contains("Name is required", errors);
            Assert.Contains("Age must be 18 or older", errors);
        });
}
```

### Testing Transformation Chains

```csharp
[Fact]
public void ProcessAndTransform_WithRightValue_TransformsSuccessfully()
{
    // Arrange
    Either<string, int> input = Either<string, int>.FromRight(42);

    // Act
    var result = input.Bind(
        leftFunc: left => Either<string, string>.FromLeft(left),
        rightFunc: right => Either<string, string>.FromRight(right.ToString())
    );

    // Assert
    result
        .EnsureRight()
        .WhereRight(s => s == "42")
        .Map(s => s.Length)
        .And(len => Assert.Equal(2, len));
}

[Fact]
public void ProcessAndTransform_WithLeftValue_PreservesLeft()
{
    // Arrange
    Either<string, int> input = Either<string, int>.FromLeft("error");

    // Act
    var result = input.Bind(
        leftFunc: left => Either<string, string>.FromLeft(left.ToUpper()),
        rightFunc: right => Either<string, string>.FromRight(right.ToString())
    );

    // Assert
    result
        .EnsureLeft()
        .WhereLeft(s => s == "ERROR")
        .And(s => Assert.Equal(5, s.Length));
}
```

---

## Design Notes

### Wrapper Types

Fluent assertions use lightweight wrapper structs:

- **`LeftAssertion<TLeft, TRight>`**: Wraps a Left value of type `TLeft`
- **`RightAssertion<TLeft, TRight>`**: Wraps a Right value of type `TRight`

These are `readonly struct` types to minimize allocations.

### Method Signatures

```csharp
// Classic assertions
public static void ShouldBeLeft<TLeft, TRight>(this Either<TLeft, TRight> either);
public static void ShouldBeLeft<TLeft, TRight>(this Either<TLeft, TRight> either, out TLeft left);
public static void ShouldBeLeft<TLeft, TRight>(this Either<TLeft, TRight> either, Action<TLeft> assert);
public static void ShouldBeRight<TLeft, TRight>(this Either<TLeft, TRight> either);
public static void ShouldBeRight<TLeft, TRight>(this Either<TLeft, TRight> either, out TRight right);
public static void ShouldBeRight<TLeft, TRight>(this Either<TLeft, TRight> either, Action<TRight> assert);

// Fluent assertions
public static LeftAssertion<TLeft, TRight> EnsureLeft<TLeft, TRight>(this Either<TLeft, TRight> either);
public static RightAssertion<TLeft, TRight> EnsureRight<TLeft, TRight>(this Either<TLeft, TRight> either);

// Wrapper methods
public LeftAssertion<TResult, TRight> Map<TResult>(Func<TLeft, TResult> mapper);
public LeftAssertion<TLeft, TRight> WhereLeft(Func<TLeft, bool> predicate);
public void And(Action<TLeft> assert);

public RightAssertion<TLeft, TResult> Map<TResult>(Func<TRight, TResult> mapper);
public RightAssertion<TLeft, TRight> WhereRight(Func<TRight, bool> predicate);
public void And(Action<TRight> assert);
```

---

## Best Practices

1. **Use ShouldBeLeft/ShouldBeRight** for simple value extraction
2. **Chain fluent assertions** for complex test flows
3. **Use WhereLeft/WhereRight** to add inline predicate checks
4. **Use Map** to transform values within the test
5. **Test both branches** explicitly to ensure proper handling
6. **Prefer predicates** (see [Predicate Assertions](predicate-assertions.html)) for property validation
7. **Use Either for dual outcomes** where neither is inherently an error

---

## Common Patterns

### Testing Search with Alternatives

```csharp
[Fact]
public void Search_WithTypo_ProvidesAlternatives()
{
    var result = service.Search("aple");
    result
        .EnsureLeft()
        .WhereLeft(alt => alt.Any())
        .And(alt => Assert.Contains("apple", alt));
}
```

### Testing Validation with Errors

```csharp
[Fact]
public void Validate_WithErrors_ReturnsLeftWithDetails()
{
    var result = validator.Validate(input);
    result
        .EnsureLeft()
        .WhereLeft(errors => errors.Count > 0)
        .Map(errors => errors.First())
        .And(first => Assert.NotEmpty(first));
}
```

### Testing Success Path

```csharp
[Fact]
public void Process_WithValidInput_ReturnsRightWithResult()
{
    var result = processor.Process(validInput);
    result
        .EnsureRight()
        .WhereRight(r => r.IsValid)
        .And(r => Assert.NotNull(r.Data));
}
```

---

## See Also

- **[Predicate Assertions](predicate-assertions.html)** - Property-based Either testing
- **[Async Assertions](async-assertions.html)** - Testing async Either operations
- **[Either Documentation](../either/index.html)** - Core Either type reference
