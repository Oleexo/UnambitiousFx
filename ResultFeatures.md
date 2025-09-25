# Result Class Features Roadmap

A comprehensive checklist of features to implement for a robust, functional-style Result class in C#.

## ✅ Implementation Status Legend
- ✅ **Implemented** - Feature is complete and tested
- 🔄 **In Progress** - Currently being worked on
- ⭐ **High Priority** - Should be implemented next
- 📋 **Planned** - On the roadmap
- 🤔 **Considering** - Might be useful, needs evaluation

---

## 🎯 Core Ergonomics

### ✅ Basic Structure
Already implemented: `Result`, `Result<T>`, `Result<T1,T2>`, etc. with Success/Failure factories.

### 📋 Map/Select
Transform success values without changing the Result shape.

---

## 📈 Implementation Roadmap

### ✅ Completed Features
- [x] Basic Result Structure (Success/Failure factories, generic arities)

### 🔥 Phase 1: Core Ergonomics
- [x] Map/Select - Transform success values without changing Result shape
- [x] MapError - Transform error/exception values
- [ ] SelectMany (LINQ) - Enable query expression syntax
- [x] Tap - Side effects on success, return original Result
- [x] TapError - Side effects on failure, return original Result
- [x] Ensure - Validate success values with predicates

#### Examples
- Map/Select
  ```csharp
  var r = Result.Success(42);
  var mapped = r.Map(x => x.ToString()); // Success("42")

  // Multi-value (when supported)
  var r2 = Result.Success(2, 3);
  var sum = r2.Map((a, b) => a + b); // Success(5)
  ```

- MapError
  ```csharp
  var r = Result.Failure<int>(new ArgumentException("bad input"));
  var mappedErr = r.MapError(ex => new InvalidOperationException($"wrapped: {ex.Message}", ex));
  ```

- SelectMany (LINQ)
  ```csharp
  var result =
      from a in Result.Success(10)
      from b in Result.Success(5)
      select a / b; // Success(2)
  ```

- Tap
  ```csharp
  var r = Result.Success("john")
      .Tap(name => Console.WriteLine($"Hello {name}"))
      .Map(name => name.ToUpperInvariant()); // "JOHN"
  ```

- TapError
  ```csharp
  var r = Result.Failure<string>(new Exception("oops"))
      .TapError(ex => Console.Error.WriteLine(ex.Message));
  ```

- Ensure
  ```csharp
  var r = Result.Success(42)
      .Ensure(x => x > 0, x => new ArgumentOutOfRangeException(nameof(x), "Must be positive"))
      .Ensure(x => x < 100, x => new ArgumentOutOfRangeException(nameof(x), "Must be < 100"));
  ```

### ⚡ Phase 2: Async & Interoperability
- [x] MapAsync - Async transformation of success values
- [x] BindAsync - Async monadic bind operations
- [ ] TapAsync - Async side effects with passthrough
- [ ] EnsureAsync - Async validation with predicates
- [ ] FromTask - Convert Task<T> to Task<Result<T>> (wrap exceptions)
- [ ] ToTask - Convert Result<T> to Task<Result<T>>
- [ ] FromTry - Wrap try/catch blocks into Result
- [ ] FromTryAsync - Wrap async try/catch blocks

#### Examples
- MapAsync
  ```csharp
  var r = Result.Success(21);
  var doubled = await r.MapAsync(async x => {
      await Task.Delay(10);
      return x * 2;
  }); // Success(42)
  ```

- BindAsync
  ```csharp
  async Task<Result<int>> GetScoreAsync(string id) => Result.Success(100);
  async Task<Result<string>> GetBadgeAsync(int score) => Result.Success(score >= 100 ? "gold" : "silver");

  var badge = await Result.Success("user-123")
      .BindAsync(id => GetScoreAsync(id))
      .BindAsync(score => GetBadgeAsync(score));
  ```

- TapAsync
  ```csharp
  var saved = await Result.Success("payload")
      .TapAsync(async s => await logger.LogAsync($"Got: {s}"));
  ```

- EnsureAsync
  ```csharp
  var ok = await Result.Success("abc@example.com")
      .EnsureAsync(async email => await EmailExistsAsync(email),
                   email => new InvalidOperationException($"Email {email} not found"));
  ```

- FromTask
  ```csharp
  Task<string> raw = FetchAsync(); // may throw
  Task<Result<string>> safe = Result.FromTask(raw); // wraps exceptions as Failure
  ```

- ToTask
  ```csharp
  Result<int> r = Result.Success(42);
  Task<Result<int>> t = r.ToTask(); // convenience for async pipelines
  ```

- FromTry
  ```csharp
  var parsed = Result.FromTry(() => int.Parse("42")); // Success(42)
  var failed = Result.FromTry(() => int.Parse("oops")); // Failure(FormatException)
  ```

- FromTryAsync
  ```csharp
  var data = await Result.FromTryAsync(async () => await GetDataAsync()); // wraps thrown exceptions
  ```

### 🔗 Phase 3: Composition & Collections
- [ ] Traverse/Sequence - IEnumerable<Result<T>> -> Result<IEnumerable<T>>
- [ ] TraverseAsync - Async traversal with Task/ValueTask
- [ ] Apply/Zip - Apply Result<Func<...>> to Result<...> or zip multiple Results
- [ ] Combine/Aggregate - Combine many Results; optionally accumulate errors
- [ ] Partition - Split a collection of Results into successes and failures
- [ ] Recover - Provide fallback value on failure
- [ ] RecoverWith - Provide alternate Result on failure
- [ ] RecoverAsync - Async error recovery

#### Examples
- Traverse/Sequence
  ```csharp
  var ids = new[] { "a", "b", "c" };
  var results = ids.Select(id => Result.Success(id.ToUpperInvariant()));
  var sequenced = results.Sequence(); // Result<IEnumerable<string>> -> Success(["A","B","C"])
  ```

- TraverseAsync
  ```csharp
  var ids = new[] { "a", "b", "c" };
  var traversed = await ids.TraverseAsync(async id => await FetchUserResultAsync(id));
  ```

- Apply/Zip
  ```csharp
  var add = Result.Success<Func<int,int,int>>((x,y) => x + y);
  var x = Result.Success(10);
  var y = Result.Success(32);

  var sum = add.Apply(x).Apply(y); // Success(42)
  var sum2 = x.Zip(y, (a,b) => a + b); // Success(42)
  ```

- Combine/Aggregate
  ```csharp
  var r1 = ValidateName("john");
  var r2 = ValidateEmail("john@example.com");
  var r3 = ValidateAge(42);

  var combined = Result.Combine(r1, r2, r3)
      .Map((name, email, age) => new User(name, email, age));
  ```

- Partition
  ```csharp
  var list = new[] { "1", "x", "3" }.Select(s => Result.FromTry(() => int.Parse(s))).ToList();
  var (oks, errs) = list.Partition(); // oks: [1,3], errs: [FormatException for "x"]
  ```

- Recover / RecoverWith
  ```csharp
  var r = Result.Failure<int>(new Exception("network"))
      .Recover(_ => 0) // fallback value
      .RecoverWith(_ => Result.Success(1)); // or fallback Result
  ```

- RecoverAsync
  ```csharp
  var r = await Result.Failure<int>(new Exception("db"))
      .RecoverAsync(async _ => await GetCachedValueAsync());
  ```

### 🎯 Phase 4: Language Integration
- [ ] Deconstruct - Deconstruct Result into (bool ok, T value, Exception? error)
- [ ] Equality/GetHashCode - Value semantics for success; reasonable failure semantics
- [ ] ToString - Meaningful representation (Success(value)/Failure(error))
- [ ] ToNullable - Convert Result<T> to T?
- [ ] TryGet - Try extracting value with bool return
- [ ] ThrowIfFailure - Throw original error when needed
- [ ] Factory Helpers - FromNullable, FromCondition, FromValidation

#### Examples
- Deconstruct
  ```csharp
  var r = Result.Success(42);
  var (ok, value, error) = r;
  if (ok) Console.WriteLine(value);
  ```

- Equality/GetHashCode
  ```csharp
  var r1 = Result.Success(42);
  var r2 = Result.Success(42);
  var eq = r1.Equals(r2); // true
  var set = new HashSet<Result<int>> { r1, r2 }; // single element if equal/hash same
  ```

- ToString
  ```csharp
  Console.WriteLine(Result.Success(42)); // e.g., "Success(42)"
  Console.WriteLine(Result.Failure<int>(new Exception("boom"))); // "Failure(Exception: boom)"
  ```

- ToNullable
  ```csharp
  int? n = Result.Success(42).ToNullable(); // 42
  int? m = Result.Failure<int>(new Exception()).ToNullable(); // null
  ```

- TryGet
  ```csharp
  if (Result.Success(42).TryGet(out var v)) { /* v == 42 */ }
  ```

- ThrowIfFailure
  ```csharp
  var user = GetUser("id").ThrowIfFailure(); // throws if failure, else returns value
  ```

- Factory Helpers
  ```csharp
  var fromNullable = Result.FromNullable<string>(maybeNull, () => new ArgumentNullException());
  var fromCondition = Result.FromCondition(x > 0, x, () => new ArgumentOutOfRangeException());
  var fromValidation = Result.FromValidation(input, Parser.TryParse, () => new FormatException());
  ```

### 🧪 Phase 5: Testing & QoL
- [ ] Test Assertion Extensions - ShouldBeSuccess/ShouldBeFailure helpers
- [ ] Debugger Display - Improved debugging visualization
- [ ] XML Documentation - Public API docs
- [ ] Benchmark Suite - Performance tests and baselines
- [ ] Usage Examples - In-repo examples and snippets
- [ ] Error Aggregation Utilities - Collect and present multiple errors

#### Examples
- Test Assertion Extensions
  ```csharp
  Result<int> r = Result.Success(42);
  r.ShouldBeSuccess(v => v == 42);

  Result<int> e = Result.Failure<int>(new Exception("boom"));
  e.ShouldBeFailure(ex => ex.Message.Contains("boom"));
  ```

- Debugger Display
  ```csharp
  // With [DebuggerDisplay("{DebuggerDisplay,nq}")]
  var r = Result.Success(7); // Visualize as "Success(7)" in debugger
  ```

- XML Documentation
  ```csharp
  /// <summary> Maps the success value to another value. </summary>
  /// <remarks> Does not change the Result shape. </remarks>
  public Result<TOut> Map<TOut>(Func<T, TOut> mapper) { ... }
  ```

- Benchmark Suite
  ```csharp
  // Using BenchmarkDotNet
  [Benchmark] public Result<int> Map_Chain() => Result.Success(1).Map(x => x + 1).Map(x => x + 1);
  ```

- Usage Examples
  ```csharp
  var r =
      from user in GetUser("id")
      from order in GetLatestOrder(user)
      select (user, order);
  ```

- Error Aggregation Utilities
  ```csharp
  var results = new[] { r1, r2, r3 };
  var errors = results.Errors(); // collect errors for reporting
  ```

### 🚀 Phase 6: Advanced Features
- [ ] Generic Error Types - Result<T, TError> (not limited to Exception)
- [ ] Rich Error Information - Codes, metadata, context/breadcrumbs
- [ ] Context Attachment - Append contextual data to failures
- [ ] Struct-Based Variants - Low-allocation, high-performance Results
- [ ] Lazy Exception Creation - Defer expensive error object creation
- [ ] Result Policies - Retry/timeout wrappers returning Result

#### Examples
- Generic Error Types
  ```csharp
  public enum ValidationError { Required, TooShort, Invalid }

  Result<string, ValidationError> ValidateName(string s) =>
      string.IsNullOrWhiteSpace(s)
          ? Result.Failure<string, ValidationError>(ValidationError.Required)
          : Result.Success<string, ValidationError>(s.Trim());
  ```

- Rich Error Information
  ```csharp
  var err = new ResultError(code: "USER_NOT_FOUND", message: "User not found", metadata: new() { ["id"] = "abc" });
  var r = Result.Failure<User>(err);
  ```

- Context Attachment
  ```csharp
  var r = GetUser("abc")
      .Attach("operation", "GetUser")
      .Attach("userId", "abc");
  ```

- Struct-Based Variants
  ```csharp
  // For hot paths:
  readonly struct ResultStruct<T> { /* value | error | isSuccess */ }
  ```

- Lazy Exception Creation
  ```csharp
  var r = Result.Failure<int>(() => new Exception("expensive message")); // created only if accessed
  ```

- Result Policies
  ```csharp
  var r = await ResultPolicy.Retry(3, TimeSpan.FromMilliseconds(50))
      .ExecuteAsync(() => Result.FromTryAsync(() => MightFailAsync()));
  ```

---
