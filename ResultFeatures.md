# Result Roadmap (Reworked)

A consolidated, prioritized roadmap for building a robust functional Result abstraction, merging existing implementation, parity gaps vs FluentResults, and future ambitions.

## Legend
- ✅ Implemented
- 🔄 In Progress
- ⭐ High Priority (next 1–2 milestones)
- 📋 Planned (accepted, not yet scheduled)
- 🤔 Considering (needs validation / may be dropped)

---

## Phase Overview (Strategic Order)
1. (Done) Foundation
2. Core Expansion & Error Model
3. Inspection & Value Access
4. Async Parity & Composition Refinement
5. Collections & Aggregation Enhancements
6. Interop & Language Integration
7. Resilience & Policies
8. Performance & Memory
9. Domain & Tooling Extensions
10. Documentation, Testing & Benchmarks

Rough delivery batches group features that reinforce each other and minimize churn.

---

## Phase 0 (Complete) – Foundation
Core building blocks already in place.
- ✅ Basic Result Types (`Result`, `Result<T>`, multi-arity variants) with Success/Failure factories
- ✅ Map / Select (sync)
- ✅ MapError (single error transform)
- ✅ Tap (success side‑effects)
- ✅ TapError (failure side‑effects)
- ✅ Ensure (sync predicate validation)
- ✅ Async: MapAsync, BindAsync, TapAsync, EnsureAsync
- ✅ Exception Wrapping: FromTry, FromTryAsync, FromTask
- ✅ Task Integration: ToTask
- ✅ Composition: Apply / Zip
- ✅ Collections: Traverse / Sequence / TraverseAsync
- ✅ Aggregation: Combine / Aggregate
- ✅ Partition (split successes vs failures)
- ✅ Recovery: Recover / RecoverWith / RecoverAsync

---

## Phase 1 – Core Expansion & Error Model (Highest Impact Foundation Upgrade)
Goal: Unlock richer ergonomics + structured errors without breaking later phases.
- ✅ Bind / Then (monadic chaining — value to Result)
- ✅ SelectMany (LINQ comprehension support – sync + Task + ValueTask variants)
- ✅ Match (action-based variant implemented; value-return fold variant 📋)
- ✅ Flatten (Result<Result<T>> → Result<T> – sync + Task + ValueTask)
- ✅ IReason / IError / ISuccess abstractions (interfaces + storage scaffold in place)
- ✅ Metadata attachment (result-level dictionary + WithMetadata overloads + params tuple + WithSuccess / WithError / WithErrors helpers with optional metadata copy)
- ✅ Standard Error Base (Code, Message, Metadata) – `ErrorBase`
- ✅ Specialized Domain Errors (NotFound, Validation, Conflict, Unauthorized, ExceptionalError + SuccessReason)
- ✅ ValueOr(default) / ValueOr(Func<T>) (all arities + async)
- ✅ ValueOrThrow() / ValueOrThrow(factory) (all arities + async)
- ✅ Error wrapping helper (wrap Exception → domain error)
- ✅ Prepend/Append error transformers (message shaping)

Rationale: Improves developer ergonomics and sets the contract for inspection, formatting, and interop.

> Note: `Match` currently supports the side-effect (Action) pattern. A value-returning functional fold `(onSuccess, onFailure) => TOut` (including generic success-value access) is still pending (📋).

---

## Phase 2 – Inspection & Value Access
Visibility + safe extraction.
- ✅ HasError<TError>() (exception fallback; reason-based matching will expand once Error base lands)
- ✅ HasException<TException>()
- ✅ TryGet(out T) (all arities via source generation)
- ✅ ToNullable() (all arities; returns nullable value/tuple)
- ✅ Deconstruct (ok, value, error) (non-generic & all arities)
- 🔄 ToString overhaul (basic form with reasons count implemented; codes/metadata excerpt pending)
- 📋 FindError(predicate) / TryPickError
- 📋 EnsureNotNull / EnsureNotEmpty helpers
- 📋 MatchError / FilterError utilities
- 🤔 FlattenExceptions() (collect all underlying exceptions)

---

## Phase 3 – Async Parity & Composition Refinement
Complete ergonomic symmetry between sync & async; extend error transforms.
- ⭐ TapErrorAsync
- ⭐ MapErrorAsync
- ⭐ MapErrors (plural transform on aggregated errors)
- ⭐ TapBoth / TapEither (side-effect branching)
- 📋 FirstFailureOrSuccess helper
- 📋 BindTryAsync (async value factory with exception capture)
- 📋 MapError chain policies (short‑circuit or accumulate)
- 🤔 Cancellation-aware variants (MapAsyncWithCancellation, etc.)

---

## Phase 4 – Collections & Aggregation Enhancements
Batch operations + analytical utilities.
- ⭐ Errors() / AllErrors() enumeration helpers
- ⭐ Merge (preserve success reasons + combine errors)
- ⭐ GroupByErrorCode / SummarizeErrors
- 📋 Result matrix utilities (e.g., Lift2/Lift3 convenience)
- 📋 CombineUpToN (efficient fixed-arity combinators) 
- 📋 AccumulateWith(strategy) (configurable aggregation semantics)
- 🤔 Streaming traversal (IAsyncEnumerable<Result<T>> → progressive aggregation)

---

## Phase 5 – Interop & Language Integration
Bridge with standard patterns & ecosystem.
- ⭐ Factory Helpers: FromNullable / FromCondition / FromValidation
- ⭐ Implicit success lift (T → Result<T>)
- 📋 Implicit failure lift (Error → Result / Result<T>)
- 📋 ASP.NET Core mappers (Result → IActionResult)
- 📋 ToOption / FromOption
- 📋 ToValidation / FromValidation (e.g., FluentValidation adapter)
- 📋 Domain-specific adapters (ProblemDetails integration)
- 🤔 Polymorphic serialization guidelines (System.Text.Json converters)

---

## Phase 6 – Resilience & Policies
Declarative execution policies returning Results.
- ⭐ ResultPolicy: Retry, Timeout (core)
- 📋 FallbackPolicy (alternate Result supplier)
- 📋 CircuitBreakerPolicy (stateful; optional separate package)
- 📋 Bulkhead / RateLimit wrappers
- 🤔 Policy composition DSL (Retry.ThenTimeout())

---

## Phase 7 – Performance & Memory
Optimize hot paths and allocations.
- ⭐ Struct-based `Result<T>` variant (opt-in) 
- ⭐ Shared static Success instance(s) (no metadata) when T is unit-like
- 📋 Lazy exception creation (factory invoked only on access)
- 📋 Pooled error object patterns (for high-frequency codes)
- 📋 Avoid boxing for constraints (generic inlining strategies)
- 🤔 Source-generated fast-path for common Bind/Map chains

---

## Phase 8 – Domain & Tooling Extensions
Developer productivity & large-scale adoption.
- ⭐ Analyzer: warn on ignored Result (unused expression)
- ⭐ Analyzer: suggest Bind/Ensure chain simplification
- 📋 Source generator: domain error boilerplate (code, message, template)
- 📋 Analyzer: inconsistent error code usage detection
- 📋 Template pack / project snippet (dotnet new result-api)
- 🤔 VS / Rider plugin for quick action (Wrap in Result.Try)

---

## Phase 9 – Documentation, Testing & Benchmarks
Confidence, clarity, credibility.
- ⭐ Test Assertion Extensions (ShouldBeSuccess, ShouldHaveError<T>, etc.)
- ⭐ Debugger Display (rich reason summary)
- ⭐ XML Docs (public surface)
- ⭐ Usage Cookbook (patterns: validation pipelines, HTTP mapping, async composition)
- 📋 Benchmark Suite (Baseline vs FluentResults vs OneOf)
- 📋 Performance scoreboard in README
- 📋 Migration Guide (from FluentResults / OneOf / Either libs)
- 🤔 Design Rationale Whitepaper

---

## Backlog (Unscheduled / Needs Validation)
These items are intentionally deferred; revisit after core maturity.
- 🤔 Message formatting policies (indentation, truncation)
- 🤔 Error transformation pipelines (middleware-like abstraction)
- 🤔 Result graph tracing (breadcrumb chain for distributed ops)
- 🤔 Observability hooks (structured logging exporter)
- 🤔 Telemetry: automatic Activity enrichment on failure
- 🤔 Semantic versioning guidance doc for Result evolution

---

## Cross-Cutting Concerns & Order Justification
- Error Model first (Phase 1) to avoid retrofitting metadata & codes later.
- Inspection (Phase 2) depends on stable error abstractions.
- Async parity (Phase 3) prevents fragmented APIs and user confusion.
- Aggregation (Phase 4) becomes more valuable once errors are richer.
- Interop (Phase 5) waits for stable core semantics to avoid breaking integrations.
- Policies (Phase 6) leverage earlier Try/Async primitives.
- Performance (Phase 7) only after API surface stabilizes.
- Tooling & Docs follow to lock in adoption.

---

## Milestone Suggestion (Example Slicing)
Milestone 1 (Phase 1 subset – DONE): Bind, Match (action), SelectMany, Flatten, Error base + Domain Errors + Metadata system & helpers, ValueOr/ValueOrThrow
Milestone 2 (Phase 1 completion + Phase 2 core): Value-fold Match<TOut>, Error wrapping helper, Prepend/Append transformers, ToString enrichment (codes + metadata preview), HasError refinement (reason type matching), EnsureNotNull/EnsureNotEmpty
Milestone 3 (Async Parity): TapErrorAsync, MapErrorAsync, MapErrors, TapBoth/TapEither, BindTryAsync
Milestone 4 (Interop & Aggregation): Factory helpers (FromNullable/Condition/Validation), Errors()/Merge(), GroupByErrorCode, ASP.NET Core mappers
Milestone 5 (Resilience + Perf foundation): Retry/Timeout policies, Struct variant prototype, DebuggerDisplay, Assertions

---

## Updated Global Feature Index
(Alphabetical quick reference with status — see phases for grouping.)
- Apply / Zip ✅
- Bind / Then ✅ (Implemented – monadic chaining core)
- BindAsync ✅
- BindTryAsync 📋 (P3)
- Combine / Aggregate ✅
- Context Attachment ✅ (Metadata + helpers + selective copy flag)
- Deconstruct ✅
- Ensure ✅
- EnsureAsync ✅
- EnsureNotNull / EnsureNotEmpty 📋 (P2)
- Error Base (Code/Message/Metadata) ✅
- Error Wrapping Helper ✅
- Errors() / AllErrors() ⭐ (P4)
- ExceptionalError ✅
- Flatten ✅ (P1 – includes Task/ValueTask variants)
- FlattenExceptions() 🤔 (P4)
- FromCondition 📋 (P5)
- FromNullable 📋 (P5)
- FromTask ✅
- FromTry / FromTryAsync ✅
- FromValidation 📋 (P5)
- GroupByErrorCode ⭐ (P4)
- HasError<TError>() ✅
- HasException<TException>() ✅
- IReason / IError / ISuccess ✅
- Implicit Conversions (T → Result<T>) ⭐ (P5)
- Implicit Error lift 📋 (P5)
- Lazy Exception Creation 📋 (P7)
- Match ✅ (Action-based; value-fold enhancement 📋)
- Map ✅
- MapAsync ✅
- MapError ✅
- MapErrorAsync ⭐ (P3)
- MapErrors ⭐ (P3)
- Merge ⭐ (P4)
- Metadata Attachment ✅ (Result-level + reason-level + tuple overload + helper APIs)
- Partition ✅
- Policies: Retry / Timeout ⭐ (P6)
- Policies: CircuitBreaker 📋 (P6)
- Prepend/Append Error Messages ✅
- Recover / RecoverWith / RecoverAsync ✅
- ResultPolicy Abstraction ⭐ (P6)
- SelectMany (LINQ) ✅ (P1 – includes async variants)
- Sequence / Traverse ✅
- Tap ✅
- TapAsync ✅
- TapBoth / TapEither ⭐ (P3)
- TapError ✅
- TapErrorAsync ⭐ (P3)
- ToNullable ✅
- ToOption / FromOption 📋 (P5)
- ToString Overhaul 🔄 (basic reasons count; pending codes + metadata preview)
- ToTask ✅
- TryGet ✅
- ValueOr / ValueOr(Func) ✅
- ValueOrThrow ✅

---

## Open Design Questions (Track in Issues)
1. Should metadata live on Result root, each reason, or both? (Current: both implemented; helpers support optional propagation.)
2. Are specialized errors first-class types or factory helpers? (Current: concrete types implemented.)
3. Struct variant: generic or separate namespace to avoid accidental copying costs?
4. Implicit conversions opt-in (via using static) or always enabled?
5. Policy execution: integrate with Polly or standalone minimal layer?

---

## Immediate Next Steps (Actionable)
1. Implement value-fold Match<TOut> (sync + Task/ValueTask for generic results) exposing success values.
2. (Done) Implement Error wrapping helper (Exception → ExceptionalError) + Prepend/Append error message transformers.
3. Enhance ToString: include first error code (if any), total error count, compact metadata preview (first 2 keys) for both Result & generic variants.
4. Add EnsureNotNull / EnsureNotEmpty utilities (lift null/empty to ValidationError) and associated tests.
5. Draft design note for metadata propagation policy (document copyMetadata flag semantics).

### ToString Enhancement Plan (Upcoming)
Format example (success with tuple & metadata sample):
`Success<int,int>(1,2) reasons=0 meta=env:prod,trace:abc123`
Failure example with codes:
`Failure<int>(ValidationError: Required field) code=VALIDATION reasons=1 meta=userId:42`

---

*Note:* Full error reason richness (aggregation formatting, nested propagation policy) deferred until additional formatting helpers land.
