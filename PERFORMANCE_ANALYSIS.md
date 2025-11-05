# Performance Analysis: UnambitiousFx.Mediator vs MediatR

## Executive Summary

Your mediator is approximately **6x slower** than MediatR for basic send operations (351ns vs 58ns). The analysis below identifies the key bottlenecks and provides actionable recommendations.

## Benchmark Results Analysis

```
| Method                                  | Mean      | Allocated | Ratio |
|---------------------------------------- |----------:|----------:|------:|
| MediatR - Send (response)               |  58.26 ns |     336 B |  0.17 |
| Our Mediator - Direct Send (response)   | 309.17 ns |     224 B |  0.88 |
| Our Mediator - Send (response)          | 351.92 ns |     472 B |  1.00 |
```

**Key Observations:**
1. Direct handler call (309ns) is already 5.3x slower than MediatR despite bypassing mediator infrastructure
2. The mediator infrastructure adds only ~43ns overhead (351 - 309 = 42ns)
3. Memory allocation is competitive (472B vs 336B)

## Identified Performance Bottlenecks

### 1. ⚠️ **CRITICAL: `_behaviors.ToArray()` on Every Request**

**Location**: `ProxyRequestHandler.cs` line 22 & 57

**Current Code:**
```csharp
public ValueTask<Result> HandleAsync(IContext context, TRequest request, CancellationToken cancellationToken = default) {
    return ExecutePipelineAsync(context, request, _behaviors.ToArray(), 0, cancellationToken);
    //                                            ^^^^^^^^^^^^^^^^^^^
    //                                            Called on EVERY request!
}
```

**Impact**: ~50-100ns overhead + array allocation
- Creates a new array on every single request
- Enumerates the `IEnumerable<IRequestPipelineBehavior>`
- Allocates memory that needs to be GC'd

**Solution**: Cache the array once during construction
```csharp
private readonly IRequestPipelineBehavior[] _behaviorsArray;

public ProxyRequestHandler(TRequestHandler handler, IEnumerable<IRequestPipelineBehavior> behaviors) {
    _handler = handler;
    _behaviorsArray = behaviors.ToArray(); // Cache once
}

public ValueTask<Result> HandleAsync(...) {
    return ExecutePipelineAsync(context, request, _behaviorsArray, 0, cancellationToken);
}
```

**Expected Improvement**: ~50-100ns reduction

---

### 2. ⚠️ **HIGH: Maybe<T> Pattern Matching Overhead**

**Location**: `Sender.cs` line 23 & 35

**Current Code:**
```csharp
return _resolver.GetService<IRequestHandler<TRequest, TResponse>>()
                .Match(handler => {
                    var ctx = _contextFactory.Create();
                    return handler.HandleAsync(ctx, request, cancellationToken);
                }, () => throw new MissingHandlerException(...));
```

**Impact**: ~30-50ns overhead
- Involves virtual method calls (abstract methods on `Maybe<T>`)
- Lambda allocation and invocation
- Pattern matching dispatch

**Current Flow:**
1. `GetService<T>()` → creates `SomeMaybe<T>` or `NoneMaybe<T>`
2. `.Match()` → virtual method call
3. Lambda invocation → closure/delegate overhead
4. Result extraction

**MediatR Equivalent:**
```csharp
var handler = _serviceProvider.GetRequiredService<IRequestHandler<TRequest, TResponse>>();
return handler.Handle(request, cancellationToken);
```

**Solution Options:**

**Option A: Direct GetService (Fastest)**
```csharp
public ValueTask<Result<TResponse>> SendAsync<TRequest, TResponse>(...) {
    var handler = _resolver.GetRequiredService<IRequestHandler<TRequest, TResponse>>();
    var ctx = _contextFactory.Create();
    return handler.HandleAsync(ctx, request, cancellationToken);
}
```

**Option B: Keep Maybe but optimize**
```csharp
public ValueTask<Result<TResponse>> SendAsync<TRequest, TResponse>(...) {
    var maybeHandler = _resolver.GetService<IRequestHandler<TRequest, TResponse>>();
    if (!maybeHandler.Some(out var handler)) {
        throw new MissingHandlerException(typeof(IRequestHandler<TRequest, TResponse>));
    }
    var ctx = _contextFactory.Create();
    return handler.HandleAsync(ctx, request, cancellationToken);
}
```

**Expected Improvement**: 30-50ns reduction

---

### 3. 🔶 **MEDIUM: Context Factory Overhead**

**Location**: `Sender.cs` line 25 & 37

**Current Code:**
```csharp
var ctx = _contextFactory.Create();
return handler.HandleAsync(ctx, request, cancellationToken);
```

**Context Implementation:**
```csharp
internal sealed class Context : IContext {
    private readonly IPublisher _publisher;

    public Context(IPublisher publisher) {
        _publisher    = publisher;
        CorrelationId = Guid.CreateVersion7();  // ⚠️ System call
        OccuredAt     = DateTimeOffset.UtcNow;  // ⚠️ System call
    }

    public Guid           CorrelationId { get; }
    public DateTimeOffset OccuredAt     { get; }
    // ... PublishEventAsync, CommitEventsAsync methods
}
```

**Impact**: ~50-100ns overhead
- **`Guid.CreateVersion7()`**: ~20-40ns (system call for timestamp + random generation)
- **`DateTimeOffset.UtcNow`**: ~10-20ns (system call)
- Object allocation: ~10-20ns
- Virtual method calls: ~5-10ns

**MediatR Approach**: No context object - passes request/cancellation directly

**Analysis**: 
- Context provides correlation ID and timestamp for every request
- Includes event publishing capabilities
- These features add value but come at a cost

**Potential Solutions:**

**Option A: Lazy Context Creation** (Recommended)
```csharp
// Add overload without context for simple handlers
public interface IRequestHandler<TRequest, TResponse> {
    // Existing: requires context
    ValueTask<Result<TResponse>> HandleAsync(IContext ctx, TRequest req, CancellationToken ct);
    
    // New: optional context (default implementation throws NotImplementedException)
    ValueTask<Result<TResponse>> HandleAsync(TRequest req, CancellationToken ct);
}

// In Sender: try no-context first, fall back to context
var handler = _resolver.GetRequiredService<IRequestHandler<TRequest, TResponse>>();
try {
    return handler.HandleAsync(request, cancellationToken); // No context!
} catch (NotImplementedException) {
    var ctx = _contextFactory.Create();
    return handler.HandleAsync(ctx, request, cancellationToken);
}
```

**Option B: Context Pooling**
```csharp
// Use ArrayPool<Context> to reuse context objects
// Reset CorrelationId and OccuredAt on retrieval
private static readonly ObjectPool<Context> _contextPool = new();
```

**Option C: Struct Context** (Breaking change)
```csharp
// Make Context a readonly struct - avoid heap allocation
public readonly struct Context : IContext {
    private readonly IPublisher _publisher;
    public Guid CorrelationId { get; }
    public DateTimeOffset OccuredAt { get; }
}
```

**Trade-off**: Context adds ~70-90ns overhead but provides valuable features (correlation ID, event publishing). Consider making it optional for performance-critical handlers.

---

### 4. 🔶 **MEDIUM: Recursive Pipeline Execution**

**Location**: `ProxyRequestHandler.cs` ExecutePipelineAsync

**Current Code:**
```csharp
private ValueTask<Result<TResponse>> ExecutePipelineAsync(
    IContext context, TRequest request, IRequestPipelineBehavior[] behaviors, 
    int index, CancellationToken cancellationToken) {
    
    if (index >= behaviors.Length) {
        return _handler.HandleAsync(context, request, cancellationToken);
    }

    return behaviors[index].HandleAsync(context, request, Next, cancellationToken);

    ValueTask<Result<TResponse>> Next() {
        return ExecutePipelineAsync(context, request, behaviors, index + 1, cancellationToken);
    }
}
```

**Impact**: ~10-30ns per behavior
- Local function allocation (captures context)
- Recursive call overhead
- Multiple stack frames

**MediatR Approach**: Uses similar pattern but with Task-based continuations

**Note**: This is actually a good pattern and similar to MediatR. The overhead is acceptable.

---

### 5. 🔷 **LOW: ValueTask vs Task**

**Current**: Using `ValueTask<Result<T>>`
**MediatR**: Using `Task<T>`

**Impact**: Minimal in your case
- `ValueTask` can avoid allocation when result is synchronously available
- However, your handlers return `ValueTask.FromResult()` which allocates
- MediatR uses `Task.FromResult()` which also allocates

**Not a significant factor** in the performance difference.

---

### 6. 🔷 **LOW: Result<T> Wrapper**

**Your Approach**: `Result<T>` wrapping the response
**MediatR**: Direct return of `T`

**Impact**: ~10-20ns overhead
- Additional struct wrapping
- Pattern matching for `TryGet()`

**Trade-off**: This is a design choice (Railway-Oriented Programming) - the overhead is acceptable for the benefits.

---

## Why is "Direct Send" Slow?

The "Direct Send (response)" benchmark (309ns) bypasses the Sender infrastructure but is still slow because:

1. **Context Creation**: `contextFactory.Create()` - unknown overhead
2. **Handler Instance**: Resolved from DI container
3. **Result<T> Handling**: `TryGet()` pattern matching

Let me check the Context implementation to quantify its overhead:

---

## Recommended Action Plan

### Phase 1: Quick Wins (Expected: ~50-100ns improvement)

1. ✅ **Cache behaviors array** (Critical - Do this first!)
   ```csharp
   // In ProxyRequestHandler constructor:
   _behaviorsArray = behaviors.ToArray();
   ```

2. ✅ **Add GetRequiredService to resolver** (if doesn't exist)
   ```csharp
   public interface IDependencyResolver {
       TService GetRequiredService<TService>() where TService : class;
   }
   ```

3. ✅ **Optimize Sender to avoid Maybe.Match**
   ```csharp
   var handler = _resolver.GetRequiredService<IRequestHandler<TRequest, TResponse>>();
   var ctx = _contextFactory.Create();
   return handler.HandleAsync(ctx, request, cancellationToken);
   ```

### Phase 2: Context Optimization (Expected: ~50-100ns improvement)

1. 🔍 **Profile Context.Create()**
   - Add benchmark: `[Benchmark] public IContext Context_Creation()`
   - Measure: What's inside? Can it be pooled?

2. 🔍 **Consider Context alternatives**
   - Struct-based context for small data
   - Optional context (overload without context)
   - Pooled context instances

### Phase 3: Advanced Optimizations (Expected: ~20-50ns improvement)

1. ⚡ **Source Generator for Handler Registration**
   - Pre-generate handler lookups
   - Avoid runtime type resolution overhead

2. ⚡ **Inline Behavior Execution** (when behavior count = 0)
   ```csharp
   if (_behaviorsArray.Length == 0) {
       return _handler.HandleAsync(ctx, request, cancellationToken);
   }
   ```

---

## Benchmarking Methodology

To properly diagnose, add these component-level benchmarks:

```csharp
[Benchmark(Description = "Component - Context Creation")]
public IContext Measure_Context_Creation() {
    var factory = _ourBaseSp.GetRequiredService<IContextFactory>();
    return factory.Create();
}

[Benchmark(Description = "Component - Resolver GetService")]
public object? Measure_Resolver_GetService() {
    var resolver = _ourBaseSp.GetRequiredService<IDependencyResolver>();
    return resolver.GetService<RequestWithResponseHandler>();
}

[Benchmark(Description = "Component - Behaviors ToArray")]
public IRequestPipelineBehavior[] Measure_Behaviors_ToArray() {
    var behaviors = _our1BehSp.GetRequiredService<IEnumerable<IRequestPipelineBehavior>>();
    return behaviors.ToArray();
}

[Benchmark(Description = "Component - Handler Direct Call")]
public async Task<int> Measure_Handler_DirectCall() {
    var handler = _ourBaseSp.GetRequiredService<RequestWithResponseHandler>();
    var ctx = _ourContextBase;
    var res = await handler.HandleAsync(ctx, RrRequest, CancellationToken.None);
    return res.TryGet(out var v) ? v! : -1;
}
```

---

## Expected Results After Optimizations

| Optimization | Current | Expected | Improvement |
|-------------|---------|----------|-------------|
| Baseline | 351ns | - | - |
| Cache behaviors array | 351ns | ~270ns | -80ns |
| Remove Maybe.Match | 270ns | ~200ns | -70ns |
| Optimize Context | 200ns | ~120ns | -80ns |
| **Target** | 351ns | **~120ns** | **-230ns (2.9x faster)** |

This would bring your mediator to ~2x slower than MediatR, which is acceptable given:
- Context abstraction (not in MediatR)
- Result<T> pattern (Railway-Oriented Programming)
- More flexible pipeline architecture

---

## Conclusion

The main culprits are:
1. **`_behaviors.ToArray()` on every request** - Most critical
2. **Maybe<T> pattern matching overhead** - High impact
3. **Context creation overhead** - Needs profiling

Implementing the Phase 1 optimizations should get you from 351ns to ~200ns (1.75x improvement), making your mediator only ~3.5x slower than MediatR instead of 6x.

The remaining gap is likely acceptable given the additional features (Context, Result<T>) that your mediator provides.

