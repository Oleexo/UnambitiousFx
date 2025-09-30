---
title: xUnit Assertions
parent: Core
nav_order: 4
---

# xUnit Assertions

Assertion helpers for UnambitiousFx functional primitives (`Result`, `Option`, `Either`) when writing tests with xUnit.net v3. These helpers provide both classic assertion styles and fluent/predicate-based chaining for expressive, low‑ceremony tests.

## Installation
Add a project reference (recommended when working inside this mono‑repo):

```xml
<ProjectReference Include="..\..\..\src\Core.XUnit\Core.XUnit.csproj" />
```

Or (when published) add the NuGet package:

```xml
<PackageReference Include="UnambitiousFx.Core.XUnit" Version="x.y.z" />
```

## Namespace
Add once (a `global using` is convenient):

```csharp
using UnambitiousFx.Core.XUnit;
```

## Classic Assertion Helpers
### Result assertions
```csharp
var r0 = Result.Success();
r0.ShouldBeSuccess();

var r1 = Result.Success(42);
r1.ShouldBeSuccess(out var value); // value == 42

Result.Failure<int>(new Exception("boom"))
      .ShouldBeFailure(out var err)
      .ShouldBeFailureWithMessage("boom");

Result.Success(1, "a", true)
      .ShouldBeSuccess((i, s, b) => {
          Assert.Equal(1, i);
          Assert.Equal("a", s);
          Assert.True(b);
      });
```

### Option assertions
```csharp
Option<int>.Some(42).ShouldBeSome(out var v);
Option<int>.None().ShouldBeNone();
```

### Either assertions
```csharp
Either<int,string> e1 = 10; // implicit left
e1.ShouldBeLeft(out var left);

Either<int,string> e2 = "ok"; // implicit right
e2.ShouldBeRight(r => Assert.Equal("ok", r));
```

### Async (Task / ValueTask)
```csharp
await Task.FromResult(Result.Success(9))
          .ShouldBeSuccess(out var nine);
await new ValueTask<Result<int>>(Result.Failure<int>(new Exception("x")))
          .ShouldBeFailure(out var ex);
```

## Fluent Chaining Assertions
Fluent helpers return lightweight wrapper structs enabling method chaining, mapping, filtering (Where) and additional assertions.

### Result fluent examples
```csharp
Result.Success(42)
      .EnsureSuccess()      // returns SuccessAssertion<int>
      .And(v => Assert.Equal(42, v))
      .Map(v => v + 1)
      .And(v => Assert.Equal(43, v));

Result.Failure<int>(new Exception("boom"))
      .EnsureFailure()
      .And(e => Assert.Equal("boom", e.Message))
      .AndMessage("boom");

// Multi-arity => tuple wrapped
var (tuple) = Result.Success(1, "a")
                     .EnsureSuccess(); // tuple == (1, "a")
```

### Option fluent examples
```csharp
Option.Some(5)
      .EnsureSome()
      .Map(x => x * 2)
      .And(x => Assert.Equal(10, x));

Option<int>.None()
      .EnsureNone()
      .And(() => Assert.True(true));
```

### Either fluent examples
```csharp
Either<int,string>.FromLeft(7)
    .EnsureLeft()
    .Map(l => l + 3)
    .And(l => Assert.Equal(10, l));

Either<int,string>.FromRight("hi")
    .EnsureRight()
    .Map(r => r + "!")
    .And(r => Assert.Equal("hi!", r));
```

### Async fluent examples
```csharp
await Task.FromResult(Result.Success(10))
          .EnsureSuccess()
          .And(v => Assert.Equal(10, v));

await Task.FromResult(Option.Some("x"))
          .EnsureSome()
          .And(s => Assert.Equal("x", s));

await new ValueTask<Either<int,string>>(Either<int,string>.FromRight("R"))
          .EnsureRight()
          .And(r => Assert.Equal("R", r));
```

## Predicate / Property-based Assertions
Predicate helpers let you express invariant checks without manually extracting values. They exist for sync and async (`Task` / `ValueTask`) and cover all Result arities (1–8), Option states, and Either sides.

### Result predicate examples
```csharp
// Arity 1
Result.Success(42).ShouldBeSuccessWhere(v => v > 40);
Result.Failure<int>(new Exception("boom"))
      .ShouldBeFailureWhere(ex => ex.Message == "boom");

// Multi-arity (tuple)
Result.Success(1,2,3)
      .ShouldBeSuccessWhere(t => t.Item1 + t.Item2 + t.Item3 == 6);
Result.Failure<int,int>(new InvalidOperationException())
      .ShouldBeFailureWhere(ex => ex is InvalidOperationException);

// Async
await Task.FromResult(Result.Success(9))
          .ShouldBeSuccessWhereAsync(v => v == 9);
await new ValueTask<Result<int>>(Result.Failure<int>(new Exception("x")))
          .ShouldBeFailureWhereAsync(ex => ex.Message == "x");

// Custom message
Result.Success(5).ShouldBeSuccessWhere(v => v == 5, "Expected computed value to be 5");
```

### Option predicate examples
```csharp
Option.Some(10).ShouldBeSomeWhere(v => v % 5 == 0);
Option<int>.None().ShouldBeNoneWhere(() => true); // add extra condition for None branch

await Task.FromResult(Option<int>.Some(7))
          .ShouldBeSomeWhereAsync(v => v > 3);
await Task.FromResult(Option<int>.None())
          .ShouldBeNoneWhereAsync(() => true);
```

### Either predicate examples
```csharp
Either<int,string>.FromLeft(2).ShouldBeLeftWhere(l => l < 5);
Either<int,string>.FromRight("abc").ShouldBeRightWhere(r => r.Length == 3);

await Task.FromResult(Either<int,string>.FromLeft(9))
          .ShouldBeLeftWhereAsync(l => l == 9);
await Task.FromResult(Either<int,string>.FromRight("ok"))
          .ShouldBeRightWhereAsync(r => r == "ok");
```

### Fluent Where chaining
```csharp
Result.Success(50)
      .EnsureSuccess()
      .Where(v => v == 50)
      .Map(v => v + 1)
      .Where(v => v == 51)
      .And(v => Assert.Equal(51, v));

Result.Failure<int>(new Exception("boom"))
      .EnsureFailure()
      .Where(e => e.Message == "boom")
      .AndMessage("boom");

Option.Some(5)
      .EnsureSome()
      .Where(v => v > 3)
      .Map(v => v * 2)
      .Where(v => v == 10);

Either<int,string>.FromLeft(7)
      .EnsureLeft()
      .WhereLeft(l => l == 7);

Either<int,string>.FromRight("hi")
      .EnsureRight()
      .WhereRight(r => r.StartsWith("h"));
```

## Guidance & Tips
- Prefer predicate helpers for concise intent: they short‑circuit with clear failure messages.
- Use the optional `because` parameter when context matters in larger test flows.
- Result multi‑arity predicates treat the success payload as a tuple: pattern matching & deconstruction work naturally.
- Async variants await then delegate to sync implementations—no extra allocations besides the state machine.
- Result predicate methods are source‑generated (see generated files, adjust `MaxArity` in the generator if you need > 8 values).

## Failure Messages
All helpers emit concise failure reasons (e.g. `"Expected success result but was failure."`). Messages try to balance clarity and brevity for high test signal.

## Design Notes
- Wrapper types (`SuccessAssertion<T>`, `FailureAssertion`, etc.) are `readonly struct` to stay allocation‑light.
- `Map` / `Select` support quick projection without leaving the fluent chain.
- Async helpers simply await and forward to sync variants to keep call sites clean.
- Predicate helpers are generated to avoid manual duplication across 8 arities.

## See Also
- [Result](result.markdown)
- [Option](option.markdown)
- [Either](either.markdown)

## License
MIT

