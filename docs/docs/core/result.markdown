---
title: Result
parent: Core
nav_order: 1
---

# Result

Comprehensive guide to the `Result` type and all its features: creation, transformation, validation, side‑effects, error handling, async composition, multi-value (tuple) results, collection helpers, and a full end‑to‑end example.

## What is a Result?

The `Result` family encodes success or failure without throwing exceptions for expected control-flow.

- `Result` (non generic): success without a value OR failure with an `Exception`
- `Result<T>`: success with a value of type `T` OR failure with an `Exception`
- `Result<T1, T2, ...>`: success with multiple values (stored as a tuple) OR failure

## Quick Start
```csharp
Result<int> ParseInt(string raw) => int.TryParse(raw, out var v)
    ? Result.Success(v)
    : Result.Failure<int>(new FormatException("Not an int"));

var length = ParseInt("42")
    .Map(x => x * 2)          // 84
    .Ensure(x => x > 10, x => new Exception($"Too small: {x}"))
    .Try(x => 100 / x)        // protect divide by zero (would map to Failure if <exception>)
    .Tap(x => Console.WriteLine($"Final value: {x}"))
    .Match(
        success: v => v,
        failure: e => -1);
```

---
## Creating Results
```csharp
// Success (no value)
var ok = Result.Success();

// Success with value
a = Result.Success(123); // inferred as Result<int>
var b = Result.Success<string>("hello");

// Failure from exception
var fail1 = Result.Failure(new InvalidOperationException("Boom"));      // Result
var fail2 = Result.Failure<int>(new InvalidOperationException("Boom")); // Result<int>

// Failure from message (creates an Exception internally)
var fail3 = Result.Failure("Something went wrong");          // Result
var fail4 = Result.Failure<int>("Not found");                 // Result<int>
```

Tip: Prefer domain specific exceptions (or a custom exception base) to keep error semantics clear.

---
## Inspecting Results
### Status Flags
```csharp
if (result.IsSuccess) { /* ... */ }
if (result.IsFaulted) { /* ... */ }
```

### Safe Extraction with Ok
```csharp
if (result.Ok(out var value, out var error)) {
    Console.WriteLine($"Value = {value}");
} else {
    Console.WriteLine($"Error = {error.Message}");
}

// Non generic Result
if (nonGeneric.Ok(out var err)) { /* success */ } else { /* failure */ }
```

### Pattern Matching
```csharp
result.Match(
    success: v => Console.WriteLine($"Success: {v}"),
    failure: e => Console.WriteLine($"Error: {e.Message}")
);

string message = result.Match(
    success: v => $"OK:{v}",
    failure: e => $"ERR:{e.Message}"
);
```

### Conditional Side Branches
```csharp
result.IfSuccess(() => Console.WriteLine("It worked"));
result.IfFailure(e => Console.WriteLine($"Failed: {e.Message}"));
```

---
## Transforming Values
### Bind (FlatMap / Then)
Use when the lambda itself returns a `Result`.
```csharp
Result<User>    GetUser(UserId id) { /* ... */ }
Result<Account> GetAccount(User user) { /* ... */ }
Result<Invoice> GetLatestInvoice(Account account) { /* ... */ }

var invoiceResult = GetUser(id)
    .Bind(GetAccount)
    .Bind(GetLatestInvoice);
```
Short‑circuits on the first failure.

### Map
Use when you transform the success value to a plain value (not a Result).
```csharp
Result<User> userResult = GetUser(id);
var nameResult = userResult.Map(u => u.DisplayName);
```
Behind the scenes `Map` = `Bind(value => Result.Success(map(value)))`.

### Try
Wraps a value transformation that might throw, converting thrown exceptions into a Failure.
```csharp
var safe = userResult
    .Try(u => ComputeExpensive(u)) // if ComputeExpensive throws -> Failure
    .Try(v => DangerousParsing(v.RawText));
```
Use `Try` instead of manual try/catch inside pipelines.

---
## Validating with Ensure
`Ensure` keeps the same success value if predicate passes, otherwise converts to a failure via the provided error factory.
```csharp
Result<User> userResult = GetUser(id)
    .Ensure(u => u.IsActive, u => new InvalidOperationException($"Inactive user: {u.Id}"))
    .Ensure(u => u.EmailVerified, u => new InvalidOperationException("Email not verified"));
```
Chain multiple ensures for declarative validation.

Async variant: `EnsureAsync` takes async predicate and async error factory.

---
## Side Effects (Success Path)
### Tap
Perform a side effect ONLY when successful; returns the original unchanged result.
```csharp
var r = userResult
    .Tap(u => metrics.Increment("user.loaded"))
    .Tap(u => logger.LogInformation("Loaded user {Id}", u.Id));
```
Async variant: `TapAsync`.

### TapError
Perform a side effect ONLY when faulted.
```csharp
var final = userResult
    .TapError(ex => logger.LogError(ex, "Failed to load user"))
    .TapError(ex => metrics.Increment("user.load.error"));
```
Async variant: `TapErrorAsync`.

---
## Working With Errors
### MapError
Transform the error (wrap / specialize) while preserving failure status.
```csharp
var specialized = userResult
    .MapError(ex => ex is TimeoutException
        ? new ExternalServiceException("User service timeout", ex)
        : ex);
```

### Use Domain Exception Types
```csharp
class ValidationException : Exception { public ValidationException(string m) : base(m) {} }

var validated = inputResult
    .Ensure(x => IsValid(x), x => new ValidationException("Bad input"))
    .MapError(ex => ex is ValidationException ? ex : new ValidationException("Wrapped: " + ex.Message));
```

---
## Async Composition
All major operators have `*Async` counterparts supporting `Task` or `ValueTask` sources:
- `BindAsync`
- `MapAsync`
- `TryAsync`
- `TapAsync` / `TapErrorAsync`
- `EnsureAsync`
- `MapErrorAsync`

You can mix sync and async seamlessly:
```csharp
Task<Result<User>> GetUserAsync(UserId id) { /* ... */ }
Task<Result<Account>> GetAccountAsync(User user) { /* ... */ }

var invoiceResult = await GetUserAsync(id)
    .BindAsync(GetAccountAsync)              // account
    .EnsureAsync(a => Task.FromResult(a.Enabled), a => Task.FromResult<Exception>(new Exception("Disabled")))
    .MapAsync(a => a.PrimaryInvoiceId)       // Guid
    .TryAsync(id => FetchInvoiceAsync(id))   // may throw internally
    .TapAsync(inv => audit.LogAccess(inv.Id));
```

---
## Multiple Values (Tuple Results)
The library generates `Result<T1, T2, ...>` up to a configured arity. Helps compose parallel dependencies stepwise and keep structure.
```csharp
Result<User>      userR = GetUser(id);
Result<Account>   accR  = GetAccount(userId);
Result<Settings>  setR  = GetSettings(userId);

// Combine sequentially into a multi-value result
var composed = userR
    .Bind(u => accR.Map(a => (u, a)))               // Result<User, Account>
    .Bind((u, a) => setR.Map(s => (u, a, s)));      // Result<User, Account, Settings>

var proj = composed.Map((u, a, s) => new DashboardVm(u, a, s));
```
You still access with `.Ok(out (User u, Account a, Settings s), out var error)` or use further operators.

---
## Collections
### Sequence
Turn `IEnumerable<Result<T>>` into `Result<List<T>>` (fails fast on first error).
```csharp
var collected = listOfResults.Sequence();
```

### Traverse
Map each element to a Result and collect successes (fails fast).
```csharp
Result<List<User>> loaded = userIds.Traverse(id => GetUser(id));
```

### Partition
Collect successes and failures separately.
```csharp
var (oks, errors) = listOfResults.Partition();
```

### Combine (no values)
Aggregate a set of `Result` (non generic). If any fail returns an `AggregateException`.
```csharp
var validations = new [] { rule1(), rule2(), rule3() };
var combined = validations.Combine();
```

---
## Comparing Operators

| Operator | Input Func Returns      | On Failure          | Purpose                          |
|----------|-------------------------|---------------------|----------------------------------|
| Bind     | Result                  | short‑circuits      | Chain dependent computations     |
| Map      | Plain value             | short‑circuits      | Transform value shape            |
| Try      | Plain value (may throw) | converts thrown     | Safely wrap throwing code        |
| Ensure   | bool predicate          | converts to failure | Validate intermediate invariants |
| Tap      | void (side effect)      | skipped on failure  | Observe success without altering |
| TapError | void (side effect)      | runs only failure   | Observe failure without altering |
| MapError | Exception               | stays failure       | Normalize / wrap errors          |

---
## Monoid Perspective
`Result<T>` forms a monoid where:
- Identity: `Result.Success(value)`
- Associative operation: `Bind`
Chaining is associative: `(r.Bind(f)).Bind(g)` == `r.Bind(v => f(v).Bind(g))`.

---
## Full End‑to‑End Example
Scenario: Process an order -> validate input -> load user & items -> ensure inventory -> charge payment (async) -> generate invoice -> email customer. Demonstrates most features.
```csharp
Task<Result<Order>> LoadOrderAsync(OrderId id);
Task<Result<User>>  LoadUserAsync(UserId id);
Task<Result<Item>>  LoadItemAsync(ItemId id);
Task<Result<Receipt>> ChargeAsync(Order order);
Task<Result<Invoice>> GenerateInvoiceAsync(Order order, Receipt receipt);
Task<Result<Unit>> EmailAsync(Invoice invoice, User user);

Result CombineValidations(Order o) => new [] {
        Result.Success().Ensure(_ => o.Total > 0, _ => new Exception("Empty order")),
        Result.Success().Ensure(_ => o.Items.Count <= 50, _ => new Exception("Too many items"))
    }.Combine();

Result<List<Item>> LoadAllItems(IEnumerable<ItemId> ids) => ids
    .Traverse(id => LoadItemAsync(id).Result) // (demo simplification – prefer full async pipeline)
    .Ensure(list => list.Count > 0, _ => new Exception("No items resolved"));

Task<Result<ProcessedOrder>> ProcessAsync(OrderId orderId, UserId userId) =>
    LoadOrderAsync(orderId)
        // Validate order invariants early
        .EnsureAsync(o => Task.FromResult(o.Status == OrderStatus.Pending), o => Task.FromResult<Exception>(new Exception("Order not pending")))
        .TapAsync(o => audit.Log("Order loaded", o.Id))
        .BindAsync(async o => CombineValidations(o).Ok(out var err) ? Result.Success(o) : Result.Failure<Order>(err))
        // Load user in parallel-ish (sequential here for clarity)
        .BindAsync(async o => (await LoadUserAsync(userId))
            .Map(user => (o, user)))
        // Load all items and attach
        .BindAsync(async tuple => {
            var (order, user) = tuple;
            var itemsResult = await Task.Run(() => LoadAllItems(order.Items.Select(i => i.Id)));
            return itemsResult.Map(items => (order, user, items));
        })
        // Inventory check
        .EnsureAsync(t => Task.FromResult(t.items.All(i => i.InStock)), t => Task.FromResult<Exception>(new Exception("Out of stock")))
        // Charge payment (Try to catch unexpected exceptions inside ChargeAsync path)
        .BindAsync(t => ChargeAsync(t.order)
            .TryAsync(r => Task.FromResult(r)) // illustrate TryAsync even if redundant
            .MapAsync(receipt => (t.order, t.user, t.items, receipt)))
        // Generate invoice + email
        .BindAsync(t => GenerateInvoiceAsync(t.order, t.receipt)
            .MapAsync(invoice => (t.order, t.user, t.items, t.receipt, invoice)))
        .TapErrorAsync(ex => Task.Run(() => metrics.Increment("order.process.error")))
        .TapAsync(t => logger.LogInformation("Invoice {InvoiceId} generated", t.invoice.Id))
        .BindAsync(t => EmailAsync(t.invoice, t.user)
            .MapAsync(_ => new ProcessedOrder(t.order.Id, t.invoice.Id)))
        // Normalize certain errors
        .MapErrorAsync(ex => Task.FromResult<Exception>(ex is TimeoutException ? new OrderProcessingTimeout(ex) : ex))
        // Final observation
        .TapErrorAsync(ex => Task.Run(() => logger.LogError(ex, "Order processing failed")));
```
Takeaways:
- Progressive enrichment of the result tuple.
- Validation with `Ensure/EnsureAsync`.
- Error observation with `TapErrorAsync`.
- Exception‑to‑failure translation via `TryAsync`.
- Error normalization with `MapErrorAsync`.
- Final side‑effect logging without disturbing the pipeline.

---
## When to Throw vs Return Failure

| Use Result Failure                                                | Use Exception                               |
|-------------------------------------------------------------------|---------------------------------------------|
| Expected business rule violation                                  | Truly exceptional / unrecoverable           |
| Validation errors                                                 | Programming bugs (null, index out of range) |
| External dependency transient issue you want to bubble gracefully | Catastrophic resource failures              |

---
## Practical Tips
- Prefer small focused functions returning `Result<T>` and compose with `Bind`.
- Use `Ensure` for invariant gates instead of manual `if` returning failures.
- Wrap legacy or potentially throwing code with `Try` / `TryAsync`.
- Keep side effects isolated with `Tap` / `TapError` so core logic remains pure.
- Normalize heterogeneous exceptions early with `MapError` so callers handle a consistent set.
- For batch operations use `Traverse` (map+sequence) or `Partition` when you want partial successes.

---
## Glossary
- Bind: flatten result-producing functions
- Map: transform contained value
- Try: protect against thrown exceptions
- Ensure: guard + preserve value
- Tap/TapError: side-effect only
- MapError: transform exception
- Sequence/Traverse: convert between collections of results and result of collections
- Combine: aggregate void results

---
## Further Ideas
You can layer domain specific helpers:
```csharp
static Result<T> NotNull<T>(T? value, string name) where T : class =>
    value is null ? Result.Failure<T>(new ArgumentNullException(name)) : Result.Success(value);
```
Then integrate into pipelines instead of throwing.

---
Happy composing!
