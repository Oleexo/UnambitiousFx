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

## Structured Reasons & Metadata (New)
The error model now supports attaching structured reasons (both successes and errors) to any `Result` via the `IReason` abstraction:

- `IReason` : `{ string Message; IReadOnlyDictionary<string, object?> Metadata }`
- `IError`  : extends `IReason` adding `{ string Code; Exception? Exception }`
- `ISuccess`: marker for non-error enrichment reasons

Each `Result` maintains:
- `Reasons` (list of `IReason`) preserving attachment order
- `Metadata` (case-insensitive bag) at the result level for quick inspection / logging

### Domain Error Types
Out of the box, several domain-oriented immutable error record types exist (all inherit `ErrorBase`):
- `NotFoundError(resource, identifier, extra?)` (Code: `NOT_FOUND`)
- `ValidationError(failures: IReadOnlyList<string>, extra?)` (Code: `VALIDATION`)
- `ConflictError(message, extra?)` (Code: `CONFLICT`)
- `UnauthorizedError(reason?, extra?)` (Code: `UNAUTHORIZED`)
- `ExceptionalError(exception, messageOverride?, extra?)` (Code: `EXCEPTION`)
- `SuccessReason(message, metadata)` (non-error enrichment)

### Attaching Reasons
Use fluent helpers to attach reasons and metadata *without* breaking existing chaining:
```csharp
var r = Result.Success(42)
    .WithSuccess("cache hit", new Dictionary<string, object?> { ["cache"] = true })
    .WithMetadata("traceId", traceId)
    .WithError("DERIVED", "Derived warning", metadata: new Dictionary<string, object?> { ["severity"] = "warn" }, copyMetadata: false);

// Reasons count
Console.WriteLine(r.Reasons.Count); // 2 (1 success, 1 error)
```

### Metadata Propagation
`WithSuccess` / `WithError` (and multiple variants) accept a `copyMetadata` flag (default `true`). When `true`, the reason's metadata keys are merged into the parent `Result.Metadata` (last write wins). When `false`, metadata stays local to the reason.
```csharp
var r = Result.Success()
    .WithSuccess("cache", new Dictionary<string, object?> { ["layer"] = 2 })    // copied
    .WithError(new NotFoundError("User", "42", new Dictionary<string, object?> { ["shard"] = 3 }), copyMetadata: false);

bool hasLayer = r.Metadata.ContainsKey("layer");      // true
bool hasShard = r.Metadata.ContainsKey("shard");      // false (not copied)
```

### Inline Errors
For quick ad‑hoc failures you can attach an inline error to a *successful* result (for enrichment / multi-reason scenarios):
```csharp
var enriched = Result.Success()
    .WithError("AUDIT", "Soft audit marker", metadata: new Dictionary<string, object?> { ["scope"] = "billing" });
```
This does **not** convert the success into a failure—reasons are orthogonal metadata; the failure state is determined by the core success/failure instance.

### Creating Failure from Domain Error
```csharp
Result<User> user = Result.Failure<User>(new NotFoundError("User", userId));
// FailureResult carries the original exception (if any), plus the error reason & metadata copied to Result.Metadata
```

### Inspecting Reasons
```csharp
foreach (var reason in result.Reasons) {
    switch (reason) {
        case IError err:
            logger.LogError("[{Code}] {Message}", err.Code, err.Message);
            break;
        case ISuccess s:
            logger.LogInformation("Success detail: {Msg}", s.Message);
            break;
    }
}

// Quick error filtering
var validationErrors = result.Reasons.OfType<ValidationError>().ToList();
```

### Multiple Reasons & Filtering Examples
Attach a mix of success and error reasons, selectively propagating metadata:
```csharp
var r = Result.Success(100)
    .WithSuccess("cache hit", new Dictionary<string, object?> { ["layer"] = 2 })          // metadata copied (layer)
    .WithError("WARN_QUOTA", "Approaching quota", metadata: new Dictionary<string, object?> { ["pct"] = 0.82 }, copyMetadata: false)
    .WithError(new ValidationError(new [] { "email required" }))                           // metadata (failures list) copied
    .WithSuccess("fallback used", new Dictionary<string, object?> { ["provider"] = "secondary" }, copyMetadata: false);

// List all errors (IError)
var errors = r.Reasons.OfType<IError>().ToList();
// List only success enrichments
var successes = r.Reasons.OfType<ISuccess>().ToList();
// Pick specific domain error type
var validation = r.Reasons.OfType<ValidationError>().FirstOrDefault();

// Enumerate codes for logging/metrics
foreach (var e in errors) metrics.Increment($"result.error.{e.Code.ToLowerInvariant()}");

// Result-level metadata contains only keys from reasons where copyMetadata == true (layer + failures)
bool hasLayer   = r.Metadata.ContainsKey("layer");        // true
bool hasPct     = r.Metadata.ContainsKey("pct");          // false (copyMetadata:false)
bool hasFailure = r.Metadata.ContainsKey("failures");     // true (from ValidationError)

// Quick summarization example
string summary = string.Join(", ", errors.Select(e => $"{e.Code}:{e.Message}"));
```
Guidelines:
- Use `copyMetadata: false` for verbose / large metadata you do not want in the aggregated result bag.
- Filter by concrete error types (`ValidationError`) for domain-specific handling.
- Keep success enrichments (ISuccess) lightweight—treat them as trace breadcrumbs.

### Future Enhancements
Planned (see roadmap) additions:
- Value-fold `Match<TOut>` for all arities
- Error discovery helpers (`FindError`, `Errors()`, grouping by code)
- ToString enrichment (first error code + compact metadata excerpt)

## Reference: Helper API Summary
| Helper | Purpose |
|--------|---------|
| `WithSuccess(message, metadata?, copyMetadata?)` | Attach success reason + optional metadata |
| `WithSuccess(ISuccess, copyMetadata?)` | Attach existing success reason instance |
| `WithError(IError, copyMetadata?)` | Attach an error reason |
| `WithError(code, message, exception?, metadata?, copyMetadata?)` | Inline simple error definition |
| `WithErrors(IEnumerable<IError>, copyMetadata?)` | Attach multiple error reasons |
| `WithMetadata(key, value)` / `WithMetadata(dict)` / `WithMetadata(params (string,object?)[])` | Enrich result-level metadata bag |

## Roadmap
A continuously updated roadmap for Result (and related functional types) now lives in its own page: [Result Roadmap](result-roadmap.markdown). The canonical, most detailed version is also stored at the repository root (`ResultFeatures.md`).

> The roadmap tracks completed items (✅), in-progress (🔄), high-priority next steps (⭐), planned (📋), and exploratory (🤔). It helps consumers anticipate API stability and upcoming capabilities like async error transforms, aggregation utilities, interop, and performance variants.

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

## Troubleshooting
| Scenario | Symptom | Explanation | Fix |
|----------|---------|-------------|-----|
| Using `WithError` on a success expecting it to become failure | `IsSuccess == true` even after adding error reason | The success/failure state is determined by the core Result instance, not the presence of error reasons | Use `Result.Failure(...)` or propagate a failure earlier with `Bind/Ensure` |
| Metadata key overwritten unexpectedly | Final `result.Metadata["k"]` not what earlier reason added | Metadata bag is last-write-wins (case-insensitive) | Use distinct keys or namespace them (`validation:k`) |
| Large reason metadata bloating logs | Log exporter shows many repeated keys | Reason metadata copied when `copyMetadata: true` | Pass `copyMetadata: false` then inspect reason.Metadata directly |
| Confusing `MapError` vs `WithError` | Extra reason added but underlying exception unchanged (or vice versa) | `MapError` transforms the primary exception; `WithError` just attaches a supplemental reason | Use `MapError` to replace/wrap exception, `WithError` for auxiliary context |
| Lost original stack trace after wrapping | New exception hides original stack | Custom wrapping created new Exception without inner | Preserve inner via `new DomainEx("msg", ex)` or only attach reason using `WithError` |
| Validation logic throwing instead of failing | Unexpected exceptions in pipeline | Used `throw` inside predicate instead of returning failure | Use `Ensure` with error factory; reserve `throw` for exceptional conditions |
| Multi-arity `ToNullable` surprise (single arity returns default on failure) | Single-value `ToNullable<T>()` returns `default(T)` instead of `null` | Design keeps return type as `T` (no allocation); multi-arity returns nullable tuple | Use explicit `result.Ok(out var v, out _)` when you need to distinguish default vs failure |

### Quick Decision Flow
1. Want to enrich without failing? -> `WithSuccess` / `WithError(code, message)` (stays success)
2. Want to fail if predicate fails? -> `Ensure`
3. Want to convert thrown code to failure? -> `Try` / `TryAsync`
4. Need to rewrite underlying exception? -> `MapError`
5. Need structured error info? -> Use domain error record (e.g., `ValidationError`)

### Example: Converting a success-with-warnings into a failure
If business logic later decides warnings should block processing:
```csharp
Result<int> r = GetAmount();
if (r.Reasons.OfType<IError>().Any(e => e.Code == "WARN_QUOTA")) {
    r = Result.Failure<int>(new Exception("Quota threshold exceeded"))
        .WithErrors(r.Reasons.OfType<IError>()); // re-attach prior warnings as reasons
}
```

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
