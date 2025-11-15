---
title: OneOf
parent: Core
nav_order: 3
has_children: false
---

# OneOf Overview

## What is OneOf?

`OneOf<TFirst, TSecond>` is a minimal discriminated union base abstraction that represents a value that can be **exactly one** of two possible types. It provides the foundational shape for richer, semantically named two-track types like `Either<TLeft, TRight>`.

Higher-arity variants (`OneOf<...>` up to 8 cases) follow the same model for exclusive choice.

### Extended Arities

Available abstract unions:

- `OneOf<T1, T2, T3>`
- `OneOf<T1, T2, T3, T4>`
- `OneOf<T1, T2, T3, T4, T5>`
- `OneOf<T1, T2, T3, T4, T5, T6>`
- `OneOf<T1, T2, T3, T4, T5, T6, T7>`
- `OneOf<T1, T2, T3, T4, T5, T6, T7, T8>`

Each exposes:

- Boolean discriminator properties (`IsFirst`, `IsSecond`, ..., `IsEighth`)
- `Match` with delegates per case returning a value
- `Match` with actions per case (side-effects only)
- Single-value extraction helpers per case (`First(out TFirst? value)`, `Second(out TSecond? value)`, etc.) returning `true` only if that case is active

### Example (Arity 3)

```csharp
public abstract class OneOf<T1, T2, T3> {
    bool IsFirst { get; }
    bool IsSecond { get; }
    bool IsThird { get; }
    TOut Match<TOut>(Func<T1,TOut> f1, Func<T2,TOut> f2, Func<T3,TOut> f3);
    void Match(Action<T1> a1, Action<T2> a2, Action<T3> a3);
    bool First(out T1? value);
    bool Second(out T2? value);
    bool Third(out T3? value);
}
```

### Why this simplified extraction API?

- Cleaner: You request only the branch you care about
- Clear nullability: Value is non-null only when the method returns `true`
- Scales better: No explosion of unused `out` parameters as arity increases

### Relationship with Either

`Either<TLeft, TRight>` inherits from `OneOf<TLeft, TRight>` and adds:

- Semantic naming (`Left` / `Right`)
- `Bind` for chained transformations
- Factory helpers (`FromLeft`, `FromRight`)

### Design Notes

- No generic factory methods for higher arities—create semantic wrappers
- No base-level `Bind`: transformation semantics differ by domain
- Arity-specific abstract classes only; supply concrete implementations in your domain layer

### Next Steps

- Use `Either` for dual semantic flows (error/success, alternative/primary)
- Use higher-arity `OneOf` for multi-branch domain modeling (e.g., parsing states)
- Combine with `Result` or `Option` for richer functional pipelines
