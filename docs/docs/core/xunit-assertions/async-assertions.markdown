---
title: Async Assertions
parent: xUnit Assertions
nav_order: 5
---

# Async Assertions
{: .no_toc }

## Table of contents
{: .no_toc .text-delta }

1. TOC
{:toc}

---

## Overview

Async assertions provide first-class support for testing `Task` and `ValueTask` wrapped functional types. All assertion styles (classic, fluent, and predicate) have async variants that seamlessly integrate with async/await patterns.

### Why Use Async Assertions?

- **Async/await support**: Natural integration with async test methods
- **Task and ValueTask**: Support for both Task and ValueTask types
- **All styles supported**: Classic, fluent, and predicate assertions
- **Zero overhead**: Async variants simply await and delegate to sync implementations
- **Type-safe**: Full type inference and compile-time checks

---

## Result Async Assertions

### Classic Async Assertions

#### Task<Result<T>>

```csharp
[Fact]
public async Task GetUser_ReturnsSuccess()
{
    // Arrange
    var service = new UserService();

    // Act
    Task<Result<User>> result = service.GetUserAsync(userId: 1);

    // Assert
    await result.ShouldBeSuccess(out var user);
    Assert.Equal("John", user.Name);
}

[Fact]
public async Task InvalidOperation_ReturnsFailure()
{
    var result = service.ProcessAsync(invalidInput);
    
    await result.ShouldBeFailure(out var exception);
    Assert.IsType<InvalidOperationException>(exception);
}
```

#### ValueTask<Result<T>>

```csharp
[Fact]
public async Task GetCachedValue_ReturnsSuccess()
{
    ValueTask<Result<int>> result = cache.GetAsync("key");
    
    await result.ShouldBeSuccess(out var value);
    Assert.Equal(42, value);
}

[Fact]
public async Task MissingKey_ReturnsFailure()
{
    ValueTask<Result<string>> result = cache.GetAsync("nonexistent");
    
    await result
        .ShouldBeFailure(out var ex)
        .ShouldBeFailureWithMessage("Key not found");
}
```

#### Multi-Arity Results

```csharp
[Fact]
public async Task CreateUser_ReturnsUserAndToken()
{
    var result = service.CreateUserAsync("John", "john@example.com");
    
    await result.ShouldBeSuccess((user, token) => {
        Assert.Equal("John", user.Name);
        Assert.NotEmpty(token);
    });
}
```

### Fluent Async Assertions

#### Task<Result<T>>

```csharp
[Fact]
public async Task ProcessData_WithValidInput_ReturnsExpectedValue()
{
    await service.ProcessAsync(input)
        .EnsureSuccess()
        .And(v => Assert.Equal(42, v));
}

[Fact]
public async Task FluentChain_WithTransformations()
{
    await Task.FromResult(Result.Success(10))
        .EnsureSuccess()
        .Map(v => v * 2)
        .Where(v => v == 20)
        .And(v => Assert.Equal(20, v));
}
```

#### ValueTask<Result<T>>

```csharp
[Fact]
public async Task CachedComputation_ReturnsTransformedValue()
{
    await new ValueTask<Result<int>>(Result.Success(10))
        .EnsureSuccess()
        .Map(v => v + 5)
        .And(v => Assert.Equal(15, v));
}
```

#### Failure Assertions

```csharp
[Fact]
public async Task InvalidInput_ReturnsFailureWithMessage()
{
    await service.ValidateAsync(invalidInput)
        .EnsureFailure()
        .And(e => Assert.IsType<ValidationException>(e))
        .AndMessage("Invalid input");
}
```

### Predicate Async Assertions

#### ShouldBeSuccessWhereAsync

```csharp
[Fact]
public async Task Compute_ReturnsValueInRange()
{
    await service.ComputeAsync()
        .ShouldBeSuccessWhereAsync(v => v > 0 && v < 100);
}

[Fact]
public async Task GetUser_ReturnsValidUser()
{
    await repository.GetUserAsync(1)
        .ShouldBeSuccessWhereAsync(
            user => user.Age >= 18,
            because: "User must be an adult"
        );
}
```

#### ShouldBeFailureWhereAsync

```csharp
[Fact]
public async Task InvalidOperation_FailsWithCorrectException()
{
    await service.ProcessAsync(invalid)
        .ShouldBeFailureWhereAsync(ex => ex is ValidationException);
}

[Fact]
public async Task Timeout_FailsWithTimeoutException()
{
    await service.SlowOperationAsync()
        .ShouldBeFailureWhereAsync(
            ex => ex is TimeoutException,
            because: "Operation should timeout after 5 seconds"
        );
}
```

---

## Option Async Assertions

### Classic Async Assertions

#### Task<Option<T>>

```csharp
[Fact]
public async Task FindUser_WhenExists_ReturnsSome()
{
    Task<Option<User>> result = repository.FindAsync("john");
    
    await result.ShouldBeSome(out var user);
    Assert.Equal("john", user.Username);
}

[Fact]
public async Task FindUser_WhenMissing_ReturnsNone()
{
    Task<Option<User>> result = repository.FindAsync("nonexistent");
    
    await result.ShouldBeNone();
}
```

#### ValueTask<Option<T>>

```csharp
[Fact]
public async Task GetCached_WhenPresent_ReturnsSome()
{
    ValueTask<Option<string>> result = cache.GetAsync("key");
    
    await result.ShouldBeSome(out var value);
    Assert.Equal("cached-value", value);
}
```

### Fluent Async Assertions

#### Task<Option<T>>

```csharp
[Fact]
public async Task FindAndTransform_ReturnsExpectedValue()
{
    await repository.FindAsync("john")
        .EnsureSome()
        .Map(u => u.Name)
        .Where(name => name.Length > 0)
        .And(name => Assert.Equal("John", name));
}
```

#### ValueTask<Option<T>>

```csharp
[Fact]
public async Task CachedValue_TransformsCorrectly()
{
    await new ValueTask<Option<int>>(Option.Some(10))
        .EnsureSome()
        .Map(v => v * 2)
        .And(v => Assert.Equal(20, v));
}

[Fact]
public async Task MissingValue_ReturnsNone()
{
    await cache.GetAsync("missing")
        .EnsureNone()
        .And(() => Assert.True(true));
}
```

### Predicate Async Assertions

#### ShouldBeSomeWhereAsync

```csharp
[Fact]
public async Task FindUser_ReturnsValidUser()
{
    await repository.FindAsync("john")
        .ShouldBeSomeWhereAsync(u => u.Age >= 18);
}

[Fact]
public async Task GetConfig_ReturnsValidValue()
{
    await config.GetAsync("timeout")
        .ShouldBeSomeWhereAsync(
            timeout => timeout > 0 && timeout < 3600,
            because: "Timeout should be between 1 and 3600 seconds"
        );
}
```

#### ShouldBeNoneWhereAsync

```csharp
[Fact]
public async Task MissingKey_ReturnsNone()
{
    await cache.GetAsync("nonexistent")
        .ShouldBeNoneWhereAsync(() => true);
}
```

---

## Either Async Assertions

### Classic Async Assertions

#### Task<Either<TLeft, TRight>>

```csharp
[Fact]
public async Task Search_WithMatch_ReturnsRight()
{
    Task<Either<List<string>, User>> result = service.SearchAsync("john");
    
    await result.ShouldBeRight(out var user);
    Assert.Equal("john", user.Username);
}

[Fact]
public async Task Search_WithoutMatch_ReturnsLeftWithSuggestions()
{
    Task<Either<List<string>, User>> result = service.SearchAsync("johndoe");
    
    await result.ShouldBeLeft(out var suggestions);
    Assert.NotEmpty(suggestions);
}
```

#### ValueTask<Either<TLeft, TRight>>

```csharp
[Fact]
public async Task ProcessPayment_Success_ReturnsRight()
{
    ValueTask<Either<Error, string>> result = 
        processor.ProcessAsync(validCard);
    
    await result.ShouldBeRight(out var transactionId);
    Assert.NotEmpty(transactionId);
}
```

### Fluent Async Assertions

#### Task<Either<TLeft, TRight>>

```csharp
[Fact]
public async Task LeftBranch_TransformsCorrectly()
{
    await Task.FromResult(Either<int, string>.FromLeft(7))
        .EnsureLeft()
        .Map(l => l + 3)
        .Where Left(l => l == 10)
        .And(l => Assert.Equal(10, l));
}

[Fact]
public async Task RightBranch_TransformsCorrectly()
{
    await Task.FromResult(Either<int, string>.FromRight("hi"))
        .EnsureRight()
        .Map(r => r + "!")
        .WhereRight(r => r == "hi!")
        .And(r => Assert.Equal("hi!", r));
}
```

#### ValueTask<Either<TLeft, TRight>>

```csharp
[Fact]
public async Task AsyncEitherChain_WorksCorrectly()
{
    await new ValueTask<Either<string, int>>(
            Either<string, int>.FromRight(42)
        )
        .EnsureRight()
        .Map(v => v * 2)
        .And(v => Assert.Equal(84, v));
}
```

### Predicate Async Assertions

#### ShouldBeLeftWhereAsync / ShouldBeRightWhereAsync

```csharp
[Fact]
public async Task ProcessPayment_WithError_ReturnsLeftWithCorrectCode()
{
    await processor.ProcessAsync(invalidCard)
        .ShouldBeLeftWhereAsync(err => err.Code == "INVALID_CARD");
}

[Fact]
public async Task ProcessPayment_Success_ReturnsRightWithValidId()
{
    await processor.ProcessAsync(validCard)
        .ShouldBeRightWhereAsync(
            txId => txId.Length == 32,
            because: "Transaction ID should be 32 characters"
        );
}

[Fact]
public async Task Search_WithSuggestions_ReturnsLeftWithMatches()
{
    await service.SearchAsync("johndoe")
        .ShouldBeLeftWhereAsync(suggestions => 
            suggestions.Count > 0 && 
            suggestions.Any(s => s.Contains("john"))
        );
}
```

---

## Usage Examples

### Testing Async Service Methods

```csharp
public class UserService
{
    public async Task<Result<User>> GetUserAsync(int id)
    {
        var user = await _repository.FindByIdAsync(id);
        return user != null 
            ? Result.Success(user)
            : Result.Failure<User>(new NotFoundException($"User {id} not found"));
    }
}

[Fact]
public async Task GetUser_WithValidId_ReturnsSuccess()
{
    // Arrange
    var service = new UserService();

    // Act & Assert
    await service.GetUserAsync(1)
        .ShouldBeSuccessWhereAsync(user => user.Id == 1);
}

[Fact]
public async Task GetUser_WithInvalidId_ReturnsFailure()
{
    var service = new UserService();

    await service.GetUserAsync(999)
        .ShouldBeFailureWhereAsync(ex => ex is NotFoundException);
}
```

### Testing Async Pipelines

```csharp
[Fact]
public async Task ProcessUserData_CompleteFlow_ReturnsExpected()
{
    await service.GetUserAsync(1)
        .EnsureSuccess()
        .Map(user => user.Email)
        .Where(email => email.Contains("@"))
        .Map(email => email.ToLower())
        .And(email => Assert.Matches(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", email));
}
```

### Testing Async Validation

```csharp
[Fact]
public async Task ValidateAsync_WithValidData_Passes()
{
    await validator.ValidateAsync(validInput)
        .ShouldBeSuccessWhereAsync(result => 
            result.Errors.Count == 0 &&
            result.IsValid
        );
}

[Fact]
public async Task ValidateAsync_WithInvalidData_FailsWithDetails()
{
    await validator.ValidateAsync(invalidInput)
        .ShouldBeFailureWhereAsync(ex => 
            ex is ValidationException &&
            ex.Message.Contains("required"),
            because: "Missing required fields should fail validation"
        );
}
```

### Testing Cached Operations

```csharp
[Fact]
public async Task GetFromCache_WhenPresent_ReturnsSomeQuickly()
{
    var stopwatch = Stopwatch.StartNew();
    
    await cache.GetAsync("hot-key")
        .ShouldBeSomeWhereAsync(value => value != null);
    
    stopwatch.Stop();
    Assert.True(stopwatch.ElapsedMilliseconds < 100, "Cache hit should be fast");
}

[Fact]
public async Task GetFromCache_WhenMissing_ReturnsNone()
{
    await cache.GetAsync("nonexistent")
        .ShouldBeNoneWhereAsync(() => !cache.Contains("nonexistent"));
}
```

### Testing Either with Async Operations

```csharp
[Fact]
public async Task SearchAsync_WithExactMatch_ReturnsRight()
{
    await searchService.SearchAsync("exact-term")
        .ShouldBeRightWhereAsync(result => 
            result.IsExactMatch && 
            result.Score == 1.0
        );
}

[Fact]
public async Task SearchAsync_WithPartialMatch_ReturnsLeftWithAlternatives()
{
    await searchService.SearchAsync("partial")
        .ShouldBeLeftWhereAsync(alternatives => 
            alternatives.Count > 0 &&
            alternatives.All(alt => alt.Contains("partial")),
            because: "Should suggest similar terms"
        );
}
```

---

## Design Notes

### Implementation

Async assertions simply await the Task/ValueTask and delegate to sync implementations:

```csharp
// Example implementation
public static async Task ShouldBeSuccess<T>(
    this Task<Result<T>> task, 
    out T value)
{
    var result = await task;
    result.ShouldBeSuccess(out value);
}
```

This approach:
- Minimizes code duplication
- Ensures consistent behavior
- Avoids extra allocations beyond the async state machine

### Method Signatures

All sync assertion methods have async equivalents:

```csharp
// Classic
Task ShouldBeSuccess<T>(this Task<Result<T>> task, out T value);
Task ShouldBeSuccess<T>(this ValueTask<Result<T>> task, out T value);

// Fluent
Task<SuccessAssertion<T>> EnsureSuccess<T>(this Task<Result<T>> task);
Task<SuccessAssertion<T>> EnsureSuccess<T>(this ValueTask<Result<T>> task);

// Predicate
Task ShouldBeSuccessWhereAsync<T>(
    this Task<Result<T>> task, 
    Func<T, bool> predicate, 
    string? because = null);
```

---

## Best Practices

1. **Use async/await consistently** - Don't mix blocking and async code
2. **Prefer ValueTask for hot paths** - Use ValueTask for frequently called methods
3. **Chain fluent assertions** - Build readable async test flows
4. **Add timeouts** - Use xUnit's timeout attribute for long-running tests
5. **Test cancellation** - Verify proper CancellationToken handling
6. **Use predicates for async invariants** - Express conditions concisely

---

## Common Patterns

### Async Result Validation

```csharp
await service.ProcessAsync(input)
    .ShouldBeSuccessWhereAsync(result => 
        result.IsValid && result.Data != null
    );
```

### Async Option Checks

```csharp
await repository.FindAsync(id)
    .ShouldBeSomeWhereAsync(entity => entity.Id == id);
```

### Async Either Branches

```csharp
await processor.ProcessAsync(data)
    .ShouldBeRightWhereAsync(output => output.Status == "completed");
```

### Async Fluent Chains

```csharp
await service.GetAsync(id)
    .EnsureSuccess()
    .Map(entity => entity.Name)
    .Where(name => name.Length > 0)
    .And(name => Assert.NotEmpty(name));
```

---

## See Also

- **[Result Assertions](result-assertions.html)** - Sync Result assertions
- **[Option Assertions](option-assertions.html)** - Sync Option assertions
- **[Either Assertions](either-assertions.html)** - Sync Either assertions
- **[Predicate Assertions](predicate-assertions.html)** - Sync predicate assertions
