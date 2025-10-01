---
title: Roadmap
parent: Result
nav_order: 8
---

# Result Roadmap

This page summarizes the current and planned capabilities for the `Result` abstraction. The authoritative, most granular source is the repository root file `ResultFeatures.md`. This page is updated periodically and focuses on high‑signal planning items.

## Legend
✅ Implemented  ·  🔄 In Progress  ·  ⭐ High Priority (next 1–2 milestones)  ·  📋 Planned  ·  🤔 Considering

## Phase Overview
1. Foundation (✅ complete)
2. Core Expansion & Error Model (✅ complete)
3. Inspection & Value Access (🔄 ToString enrichment in progress)
4. Async Parity & Composition Refinement (⭐ upcoming: TapErrorAsync, MapErrorAsync, MapErrors, TapBoth)
5. Collections & Aggregation Enhancements (📋 error grouping, merge helpers)
6. Interop & Language Integration (📋 factory helpers, implicit success lift)
7. Resilience & Policies (📋 retry, timeout)
8. Performance & Memory (📋 struct variant, shared success instances)
9. Domain & Tooling Extensions (📋 analyzers, source generation)
10. Documentation, Testing & Benchmarks (📋 debugger display, cookbook, benchmarks)

## Recently Completed Highlights
- Structured reason model (`IReason`, `IError`, `ISuccess`) with result-level and reason-level metadata
- Domain error records: NotFound, Validation, Conflict, Unauthorized, Exceptional, SuccessReason
- Enrichment helpers: `WithSuccess`, `WithError`, `WithErrors`, `WithMetadata` (dictionary / single / params tuple)
- Multi-arity results with generated introspection helpers (`ToNullable`, `TryGet`, deconstruction)
- Core functional operators (Bind, Map, Ensure, Try, Tap, TapError) + async counterparts

## Active / Next (Short Horizon)

| Item                              | Status | Notes                                           |
| --------------------------------- | ------ | ----------------------------------------------- |
| Value-fold `Match<TOut>`          | ⭐      | Complete symmetry with action-based `Match`     |
| ToString enrichment               | 🔄      | Add first error code + compact metadata preview |
| Error wrapping helper             | 📋      | Map Exception → domain error record             |
| Prepend/Append error transformers | 📋      | Message shaping utilities                       |
| EnsureNotNull / EnsureNotEmpty    | 📋      | Convenience validation errors (ValidationError) |

## Async Parity (Upcoming Batch)

| Item                | Status | Goal                                                           |
| ------------------- | ------ | -------------------------------------------------------------- |
| TapErrorAsync       | ⭐      | Observe async side-effects on failure                          |
| MapErrorAsync       | ⭐      | Normalize async error transformation                           |
| MapErrors           | ⭐      | Batch transform aggregated errors (future multi-error support) |
| TapBoth / TapEither | ⭐      | Symmetric observation for success/failure branches             |
| BindTryAsync        | 📋      | Async factory with exception capture                           |

## Aggregation & Analysis (Planned)

| Feature                | Status | Purpose                                                       |
| ---------------------- | ------ | ------------------------------------------------------------- |
| Errors() / AllErrors() | ⭐      | Enumerate error reasons directly                              |
| Merge                  | ⭐      | Combine maintaining success reasons & union of error metadata |
| GroupByErrorCode       | ⭐      | Quick diagnostics / reporting                                 |
| SummarizeErrors        | 📋      | Human-readable aggregated summary                             |

## Interop & Language Integration

| Feature                               | Status | Purpose                                  |
| ------------------------------------- | ------ | ---------------------------------------- |
| FromNullable / FromCondition          | ⭐      | Lift conventional patterns into Result   |
| FromValidation                        | 📋      | Bridge validation libraries              |
| Implicit success lift (T → Result<T>) | ⭐      | Reduce ceremony in pipelines             |
| ASP.NET Core mappers                  | 📋      | Map Result → standardized HTTP responses |
| ToOption / FromOption                 | 📋      | Interoperate with Option type            |

## Resilience & Performance (Longer Horizon)

| Feature                  | Status | Purpose                                         |
| ------------------------ | ------ | ----------------------------------------------- |
| Retry / Timeout policies | ⭐      | Declarative execution wrappers returning Result |
| Struct Result<T> variant | 📋      | Allocation reduction for hot paths              |
| Shared success instances | 📋      | Reuse zero-alloc sentinel successes             |
| Lazy exception creation  | 📋      | Defer cost until inspected                      |

## Tooling & DX

| Feature                        | Status | Purpose                            |
| ------------------------------ | ------ | ---------------------------------- |
| Analyzer: ignored Result       | ⭐      | Warn on discarded results          |
| Analyzer: chain simplification | ⭐      | Suggest Bind/Ensure improvements   |
| Domain error source generator  | 📋      | Scaffold boilerplate error records |
| Debugger Display               | 📋      | Rich summary in debugger views     |
| Cookbook & Migration Guides    | 📋      | Adoption & onboarding              |

## Design Tenets
- Composability first, side-effects explicit
- Rich, structured errors (codes + metadata) without forcing heavy frameworks
- Parity between sync and async to avoid ergonomic drift
- Source generation for advanced introspection & multi-arity to keep runtime lean
- Opt-in performance modes (struct variant) after API surface stabilizes

## Open Questions (Tracking)
1. Should implicit conversions be always-on or opt-in via a using directive? (Ergonomics vs surprise conversions)
2. Error propagation policy for metadata collisions (currently: last write wins). Need documented policy & maybe strategy enum.
3. Formatting contract for ToString / DebuggerDisplay (deterministic key ordering? truncation rules?)
4. Scope of multi-error aggregation vs single primary error (how deep do we go before complexity hurts usability?)

## Contributing
Issues / PRs welcome. When proposing a new operator, outline:
- Category (Transformation / Inspection / Composition / Interop)
- Sync/Async symmetry plan
- Overload shape and naming rationale
- Interaction with existing operators (e.g., precedence with Ensure / MapError)

## Full Detail
For exhaustive status (including exploratory backlog) see the repository root: `ResultFeatures.md`.

---
_Last updated: {{ site.time | date: '%Y-%m-%d' }}_

