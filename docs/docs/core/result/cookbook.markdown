---
title: Cookbook
parent: Result
nav_order: 9
---

# Result Cookbook
{: .no_toc }

Short, copy‑paste friendly patterns that combine multiple operators. This initial section focuses on chaining `MapErrors` with `TapBoth` (and related error / success side‑effect patterns).

## Pattern Index
Quick jump links to patterns on this page:

- [1. Logging & Error Normalization Pipeline](#1-logging-and-error-normalization-pipeline)
- [2. Success Event + Failure Metric](#2-success-event--failure-metric)
- [3. Consolidating Validation Failures](#3-consolidating-validation-failures)
- [4. Tuple Success + Aggregated Failure Shape](#4-tuple-success--aggregated-failure-shape)
- [5. Standardized Error Envelope](#5-standardized-error-envelope)
- [When to Use MapError vs MapErrors](#when-to-use-maperror-vs-maperrors)
- [Gotchas & Tips](#gotchas--tips)
- [Future Cookbook Additions (Planned)](#future-cookbook-additions-planned)

## Table of contents
{: .no_toc .text-delta }
1. TOC
{:toc}

---

## 1. Logging and Error Normalization Pipeline

Goal: Execute a multi-step business pipeline, log on either path, then normalize all failure causes into a single aggregated exception while preserving original reasons & metadata.

```csharp
Result<User> GetUser(string id);
Result<IReadOnlyList<Order>> GetOrders(Guid userId);
Result<Report> BuildReport(User user, IReadOnlyList<Order> orders);

Result<Report> GetUserReport(string id, ILogger log)
{
    return GetUser(id)
        .Bind(user => GetOrders(user.Id)
            .Bind(orders => BuildReport(user, orders)))
        // Observe BOTH outcomes (non-intrusive):
        .TapBoth(
            onSuccess: report => log.LogInformation("Report built for {UserId} with {Count} orders", id, report.OrderCount),
            onFailure:  ex => log.LogWarning(ex, "Failed building report for {UserId}", id)
        )
        // Collapse all collected exceptions (primary + additional reasons) into a single AggregateException:
        .MapErrors(errors => new AggregateException(
            $"User report pipeline failed (userId={id})", errors));
}
```

Key points:
- `TapBoth` never alters the result (pure side‑effect hook).
- `MapErrors` runs only on failure; it replaces the *primary* exception while leaving existing reasons & metadata intact.
- Original exceptions stay accessible inside the new `AggregateException.InnerExceptions`.

---

## 2. Success Event + Failure Metric

Emit a domain event when successful; increment a metric on failure. Keep the original error surface (no remapping).

```csharp
Result<Invoice> GenerateInvoice(Guid customerId);

Result<Invoice> GenerateAndEmit(Guid customerId, IEventBus bus, IMetrics metrics)
{
    return GenerateInvoice(customerId)
        .TapBoth(
            onSuccess: invoice => bus.Publish(new InvoiceGenerated(invoice.Id, customerId)),
            onFailure: _ => metrics.Increment("invoice_generation_failures")
        ); // unchanged Result
}
```

Tip: If you only need the failure side you could use `TapError`; only success side use `Tap`.

---

## 3. Consolidating Validation Failures

Combine multiple validation failure reasons (potentially coming from earlier chained validations) into a single `ValidationError` while preserving original reason list for diagnostics.

```csharp
Result<UserInput> ValidateRequired(UserInput input);
Result<UserInput> ValidateFormats(UserInput input);
Result<UserInput> ValidateBusinessRules(UserInput input);

Result<UserInput> ValidateAll(UserInput input)
{
    return Result.Success(input)
        .Bind(_ => ValidateRequired(input))
        .Bind(_ => ValidateFormats(input))
        .Bind(_ => ValidateBusinessRules(input))
        .MapErrors(errs => new ValidationException(
            message: "Input failed composite validation",
            failures: errs.Select(e => e.Message).ToList()
        ));
}

public sealed class ValidationException : Exception
{
    public IReadOnlyList<string> Failures { get; }
    public ValidationException(string message, IReadOnlyList<string> failures) : base(message)
        => Failures = failures;
}
```

Why `MapErrors` here? Earlier steps may each have produced their own specific exceptions / reasons. `MapErrors` lets you surface a *single* domain-centric exception while still keeping the original granular causes in the reasons collection.

---

## 4. Tuple Success + Aggregated Failure Shape

Demonstrates `Result<T1,T2>` success path with consolidated failure error.

```csharp
Result<User> GetUser(string id);
Result<Account> GetAccount(string id);

Result<User, Account> LoadUserAndAccount(string id, ILogger log)
{
    return Result.Success() // seed
        .Bind(_ => GetUser(id)
            .Bind(user => GetAccount(id)
                .Map(acct => (user, acct))
            )
        )
        .TapBoth(
            onSuccess: tuple => log.LogInformation("Loaded user+account {Id}", id),
            onFailure: ex => log.LogError(ex, "Failed loading user/account {Id}", id)
        )
        .MapErrors(errs => new AggregateException(
            $"Failed loading user+account (id={id})", errs));
}
```

Multi-arity success values pass each tuple element individually to `TapBoth` (arity-aware overloads are source-generated). In failures, `MapErrors` aggregates all captured errors and rewrites only the primary exception.

---

## 5. Standardized Error Envelope

Wrap arbitrary failures in a standardized domain error while preserving the original stack & reasons.

```csharp
sealed record ServiceFailureError(string Operation, Exception Cause) : Exception(
    $"Operation '{Operation}' failed: {Cause.Message}", Cause);

Result<T> Shield<T>(string opName, Func<Result<T>> action)
{
    return Result.Try(action) // captures unexpected exceptions as ExceptionalError reasons
        .MapErrors(errors => new ServiceFailureError(opName, errors.First()));
}

// Usage
var result = Shield("RecalculatePricing", () => RecalculatePricing(cartId))
    .TapBoth(
        onSuccess: _ => _logger.Info("Pricing recalculated"),
        onFailure: ex => _logger.Warn(ex, "Pricing failure")
    );
```

This pattern gives consumers a stable outer error type while preserving internal detail for diagnostics.

---

## When to Use MapError vs MapErrors

| Scenario | Use |
|----------|-----|
| Transform only the primary exception | `MapError` |
| Consolidate multiple exceptions / reasons into one | `MapErrors` |
| Need both: normalize each then aggregate | Chain `MapError` before `MapErrors` |

---

## Gotchas & Tips

- `TapBoth` returns the original Result; avoid assigning its return unless continuing the chain.
- `MapErrors` replaces the primary exception but leaves the reasons collection count unchanged (except for that swapped primary exceptional reason).
- Avoid expensive aggregation (like building large strings) inside `MapErrors` if the result is frequently successful — it only runs on failure, which helps.
- If you need to attach metadata during normalization, prefer `WithMetadata` on the failure *after* `MapErrors` so the metadata belongs to the final primary error context.

---

## Future Cookbook Additions (Planned)

- HTTP controller mapping patterns (`TapBoth` + `Match`)
- Circuit-breaker style retry with ResultPolicy (pending implementation)
- Validation accumulation vs short-circuit comparison
- Async streaming (IAsyncEnumerable<Result<T>>) aggregation

Have a useful pattern? Open a PR and add it here.
