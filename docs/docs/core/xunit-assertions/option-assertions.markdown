---
title: Option Assertions
parent: xUnit Assertions
nav_order: 2
---

# Option Assertions
{: .no_toc }

## Table of contents
{: .no_toc .text-delta }

1. TOC
{:toc}

---

## Overview

Option assertions provide specialized testing helpers for the `Option<T>` type. They support testing both `Some` (value present) and `None` (value absent) cases with classic and fluent assertion styles.

---

## Classic Assertions

Classic assertions follow traditional xUnit patterns, extracting values using `out` parameters.

### Some Assertions

Test an Option that contains a value:

```csharp
var option = Option.Some(42);
option.ShouldBeSome(out var value);
Assert.Equal(42, value);
```

#### Basic Some Check

```csharp
Option<string> option = Option.Some("hello");
option.ShouldBeSome();
```

#### Extract and Validate Value

```csharp
Option<int> option = Option.Some(100);
option.ShouldBeSome(out var value);
Assert.True(value > 50);
```

### None Assertions

Test an Option that is empty:

```csharp
var option = Option<int>.None();
option.ShouldBeNone();
```

#### Verify None State

```csharp
Option<string> result = FindUser("nonexistent");
result.ShouldBeNone();
```

---

## Fluent Assertions

Fluent assertions return wrapper types that enable method chaining, mapping, and filtering.

### Some Fluent Chain

#### Basic Some Assertion

```csharp
Option.Some(42)
    .EnsureSome()  // Returns SomeAssertion<int>
    .And(v => Assert.Equal(42, v));
```

#### Chaining with Map

Transform the value within the assertion chain:

```csharp
Option.Some(10)
    .EnsureSome()
    .Map(v => v * 2)
    .And(v => Assert.Equal(20, v))
    .Map(v => v.ToString())
    .And(s => Assert.Equal("20", s));
```

#### Using Where for Filtering

Add predicate checks within the chain:

```csharp
Option.Some(50)
    .EnsureSome()
    .Where(v => v > 40)
    .Map(v => v + 10)
    .Where(v => v == 60)
    .And(v => Assert.Equal(60, v));
```

#### Complex Chaining

```csharp
Option.Some("hello")
    .EnsureSome()
    .Where(s => s.Length > 0)
    .Map(s => s.ToUpper())
    .Where(s => s == "HELLO")
    .Map(s => s.Length)
    .And(len => Assert.Equal(5, len));
```

### None Fluent Chain

#### Basic None Assertion

```csharp
Option<int>.None()
    .EnsureNone()  // Returns NoneAssertion<int>
    .And(() => Assert.True(true));
```

#### Verifying Conditions for None

```csharp
Option<string>.None()
    .EnsureNone()
    .And(() => {
        // Additional assertions about the None state
        Assert.True(true);
    });
```

---

## Usage Examples

### Testing Optional Returns

```csharp
[Fact]
public void FindUser_WithExistingId_ReturnsSome()
{
    // Arrange
    var repository = new UserRepository();

    // Act
    var result = repository.FindById(userId: 1);

    // Assert - Classic style
    result.ShouldBeSome(out var user);
    Assert.Equal("John", user.Name);
}

[Fact]
public void FindUser_WithNonExistentId_ReturnsNone()
{
    // Arrange
    var repository = new UserRepository();

    // Act
    var result = repository.FindById(userId: 999);

    // Assert
    result.ShouldBeNone();
}
```

### Testing Fluent Workflows

```csharp
[Fact]
public void GetUserAge_WithValidUser_ReturnsAge()
{
    // Arrange
    var service = new UserService();

    // Act
    var result = service.GetAge(userId: 1);

    // Assert - Fluent style
    result
        .EnsureSome()
        .Where(age => age > 0)
        .And(age => Assert.InRange(age, 18, 100));
}

[Fact]
public void GetMiddleName_WithUserWithoutMiddleName_ReturnsNone()
{
    // Arrange
    var user = new User { FirstName = "John", LastName = "Doe" };

    // Act
    var result = user.GetMiddleName();

    // Assert - Fluent style
    result
        .EnsureNone()
        .And(() => Assert.NotNull(user.FirstName));
}
```

### Testing Transformations

```csharp
[Fact]
public void ParseInt_WithValidString_ReturnsSomeInt()
{
    // Arrange
    var parser = new Parser();

    // Act
    var result = parser.ParseInt("42");

    // Assert - Fluent with transformation
    result
        .EnsureSome()
        .Map(n => n * 2)
        .Where(n => n == 84)
        .And(n => Assert.Equal(84, n));
}

[Fact]
public void GetConfigValue_WithMissingKey_ReturnsNone()
{
    // Arrange
    var config = new Configuration();

    // Act
    var result = config.Get("nonexistent-key");

    // Assert
    result
        .EnsureNone()
        .And(() => Assert.True(true));
}
```

### Testing Optional Properties

```csharp
public class UserProfile
{
    public string Name { get; set; }
    public Option<string> MiddleName { get; set; }
    public Option<DateTime> DateOfBirth { get; set; }
}

[Fact]
public void CreateProfile_WithMiddleName_StoresMiddleName()
{
    // Arrange & Act
    var profile = new UserProfile
    {
        Name = "John",
        MiddleName = Option.Some("Q"),
        DateOfBirth = Option<DateTime>.None()
    };

    // Assert
    profile.MiddleName.ShouldBeSome(out var middleName);
    Assert.Equal("Q", middleName);

    profile.DateOfBirth.ShouldBeNone();
}

[Fact]
public void GetFormattedName_WithMiddleName_IncludesMiddleName()
{
    // Arrange
    var profile = new UserProfile
    {
        Name = "John",
        MiddleName = Option.Some("Q")
    };

    // Act
    var formatted = profile.MiddleName
        .Map(m => $"{profile.Name} {m}.")
        .ValueOr(profile.Name);

    // Assert
    profile.MiddleName
        .EnsureSome()
        .And(m => Assert.Equal("Q", m));
    Assert.Equal("John Q.", formatted);
}
```

### Testing Chained Operations

```csharp
[Fact]
public void FindAndFormatUser_WithExistingUser_ReturnsFormattedName()
{
    // Arrange
    var service = new UserService();

    // Act
    Option<string> result = service.FindUser("john")
        .Map(user => user.Name)
        .Map(name => name.ToUpper());

    // Assert - Fluent chain
    result
        .EnsureSome()
        .Where(name => name.Length > 0)
        .And(name => Assert.Equal("JOHN", name));
}

[Fact]
public void FindAndProcessUser_WithNonExistentUser_ReturnsNone()
{
    // Arrange
    var service = new UserService();

    // Act
    Option<ProcessedUser> result = service.FindUser("nonexistent")
        .Map(user => service.ProcessUser(user));

    // Assert
    result.ShouldBeNone();
}
```

---

## Design Notes

### Wrapper Types

Fluent assertions use lightweight wrapper structs:

- **`SomeAssertion<T>`**: Wraps an Option value of type `T`
- **`NoneAssertion<T>`**: Represents a None state with type context

These are `readonly struct` types to minimize allocations.

### Method Signatures

```csharp
// Classic assertions
public static void ShouldBeSome<T>(this Option<T> option);
public static void ShouldBeSome<T>(this Option<T> option, out T value);
public static void ShouldBeNone<T>(this Option<T> option);

// Fluent assertions
public static SomeAssertion<T> EnsureSome<T>(this Option<T> option);
public static NoneAssertion<T> EnsureNone<T>(this Option<T> option);

// Wrapper methods
public SomeAssertion<TResult> Map<TResult>(Func<T, TResult> mapper);
public SomeAssertion<T> Where(Func<T, bool> predicate);
public void And(Action<T> assert);

public void And(Action assert); // For NoneAssertion
```

---

## Pattern Matching in Tests

Option assertions work well with pattern matching:

```csharp
[Fact]
public void ProcessOption_WithSomeValue_ProcessesValue()
{
    // Arrange
    Option<int> option = Option.Some(42);

    // Act & Assert
    option.Match(
        some: value => {
            Assert.Equal(42, value);
            return true;
        },
        none: () => {
            Assert.True(false, "Expected Some, got None");
            return false;
        }
    );

    // Alternatively with assertions
    option.ShouldBeSome(out var v);
    Assert.Equal(42, v);
}
```

---

## Best Practices

1. **Use ShouldBeSome/ShouldBeNone** for simple value extraction
2. **Chain fluent assertions** for complex test flows
3. **Use Where** to add inline predicate checks
4. **Use Map** to transform values within the test
5. **Prefer predicates** (see [Predicate Assertions](predicate-assertions.html)) for property validation
6. **Test None cases** explicitly to ensure proper handling of absent values

---

## Common Patterns

### Testing Optional Configuration

```csharp
[Fact]
public void GetSetting_WhenPresent_ReturnsSome()
{
    var config = new Config();
    config.Get("timeout")
        .EnsureSome()
        .Where(timeout => timeout > 0)
        .And(timeout => Assert.InRange(timeout, 1, 3600));
}
```

### Testing Optional Lookups

```csharp
[Fact]
public void LookupCache_WhenCached_ReturnsSome()
{
    var cache = new Cache();
    cache.Get("key")
        .EnsureSome()
        .Map(value => value.ToString())
        .And(str => Assert.NotEmpty(str));
}
```

### Testing Optional Parsing

```csharp
[Fact]
public void TryParse_WhenValid_ReturnsSome()
{
    Parser.TryParse("123")
        .EnsureSome()
        .Where(n => n > 0)
        .And(n => Assert.Equal(123, n));
}
```

---

## See Also

- **[Predicate Assertions](predicate-assertions.html)** - Property-based Option testing
- **[Async Assertions](async-assertions.html)** - Testing async Option operations
- **[Option Documentation](../option/index.html)** - Core Option type reference
