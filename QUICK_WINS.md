# Quick Win Optimizations - Action Plan

## Summary of Performance Issues

Your mediator is **6x slower** than MediatR (351ns vs 58ns). Here are the THREE critical bottlenecks and how to fix them:

---

## 🚨 CRITICAL FIX #1: Cache Behaviors Array (Expected: ~80ns improvement)

**Problem**: `_behaviors.ToArray()` is called on EVERY request in `ProxyRequestHandler`

**Files to Edit**: `/src/Mediator/ProxyRequestHandler.cs`

### Current Code (BAD):
```csharp
private readonly IEnumerable<IRequestPipelineBehavior> _behaviors;

public ValueTask<Result> HandleAsync(IContext context, TRequest request, CancellationToken cancellationToken = default) {
    return ExecutePipelineAsync(context, request, _behaviors.ToArray(), 0, cancellationToken);
    //                                            ^^^^^^^^^^^^^^^^^^^ 
    //                                            ALLOCATES ARRAY ON EVERY CALL!
}
```

### Fixed Code (GOOD):
```csharp
private readonly IRequestPipelineBehavior[] _behaviors;

public ProxyRequestHandler(TRequestHandler handler, IEnumerable<IRequestPipelineBehavior> behaviors) {
    _handler = handler;
    _behaviors = behaviors.ToArray(); // Cache once during construction
}

public ValueTask<Result> HandleAsync(IContext context, TRequest request, CancellationToken cancellationToken = default) {
    return ExecutePipelineAsync(context, request, _behaviors, 0, cancellationToken);
    //                                            ^^^^^^^^^^ 
    //                                            Use cached array!
}
```

**Apply this fix to BOTH classes in ProxyRequestHandler.cs:**
1. `ProxyRequestHandler<TRequestHandler, TRequest>` (line 7-40)
2. `ProxyRequestHandler<TRequestHandler, TRequest, TResponse>` (line 42-80)

---

## 🚨 CRITICAL FIX #2: Remove Maybe Pattern Matching Overhead (Expected: ~70ns improvement)

**Problem**: Using `Maybe<T>.Match()` adds virtual method call + lambda overhead

**Files to Edit**: 
1. `/src/Mediator/Resolvers/IDependencyResolver.cs` - Add new method
2. `/src/Mediator/Resolvers/DefaultDependencyResolver.cs` - Implement method
3. `/src/Mediator/Sender.cs` - Use direct call instead of Match

### Step 1: Add GetRequiredService to IDependencyResolver

**File**: `/src/Mediator/Resolvers/IDependencyResolver.cs`

```csharp
public interface IDependencyResolver {
    Maybe<TService> GetService<TService>() where TService : class;
    IEnumerable<TService> GetServices<TService>() where TService : class;
    
    // ADD THIS:
    TService GetRequiredService<TService>() where TService : class;
}
```

### Step 2: Implement in DefaultDependencyResolver

**File**: `/src/Mediator/Resolvers/DefaultDependencyResolver.cs`

```csharp
public TService GetRequiredService<TService>() where TService : class {
    return _serviceProvider.GetRequiredService<TService>();
}
```

### Step 3: Update Sender to use direct call

**File**: `/src/Mediator/Sender.cs`

**Current Code (BAD):**
```csharp
public ValueTask<Result<TResponse>> SendAsync<TRequest, TResponse>(...) {
    return _resolver.GetService<IRequestHandler<TRequest, TResponse>>()
                    .Match(handler => {
                        var ctx = _contextFactory.Create();
                        return handler.HandleAsync(ctx, request, cancellationToken);
                    }, () => throw new MissingHandlerException(...));
}
```

**Fixed Code (GOOD):**
```csharp
public ValueTask<Result<TResponse>> SendAsync<TRequest, TResponse>(...) {
    var handler = _resolver.GetRequiredService<IRequestHandler<TRequest, TResponse>>();
    var ctx = _contextFactory.Create();
    return handler.HandleAsync(ctx, request, cancellationToken);
}
```

**Apply this fix to BOTH methods in Sender.cs:**
1. `SendAsync<TRequest, TResponse>` (with response)
2. `SendAsync<TRequest>` (void)

---

## 🔶 MEDIUM FIX #3: Optimize Context Creation (Expected: ~70ns improvement)

**Problem**: Every request creates a new Context with `Guid.CreateVersion7()` and `DateTimeOffset.UtcNow`

**This is more complex - consider these options:**

### Option A: Lazy Context Creation (Recommended - Best Performance)

Only create context if the handler actually needs it:

```csharp
// Step 1: Add optional context parameter to IRequestHandler
public interface IRequestHandler<TRequest, TResponse> {
    // New: Handler that doesn't need context (default implementation throws)
    ValueTask<Result<TResponse>> HandleAsync(TRequest request, CancellationToken ct) {
        throw new NotSupportedException("Handler requires context");
    }
    
    // Existing: Handler with context (optional now)
    ValueTask<Result<TResponse>> HandleAsync(IContext ctx, TRequest request, CancellationToken ct) {
        return HandleAsync(request, ct); // Default: call non-context version
    }
}

// Step 2: Update Sender to try no-context first
public ValueTask<Result<TResponse>> SendAsync<TRequest, TResponse>(...) {
    var handler = _resolver.GetRequiredService<IRequestHandler<TRequest, TResponse>>();
    
    // Try without context first (faster!)
    try {
        return handler.HandleAsync(request, cancellationToken);
    }
    catch (NotSupportedException) {
        // Handler needs context - create it now
        var ctx = _contextFactory.Create();
        return handler.HandleAsync(ctx, request, cancellationToken);
    }
}

// Step 3: Handlers choose their approach
public class SimpleHandler : IRequestHandler<MyRequest, int> {
    // Fast path: no context needed
    public ValueTask<Result<int>> HandleAsync(MyRequest request, CancellationToken ct) {
        return ValueTask.FromResult(Result.Success(42));
    }
}

public class ComplexHandler : IRequestHandler<MyRequest, int> {
    // Slow path: needs context for events/correlation
    public ValueTask<Result<int>> HandleAsync(IContext ctx, MyRequest request, CancellationToken ct) {
        await ctx.PublishEventAsync(new SomethingHappened(), ct);
        return Result.Success(42);
    }
}
```

**Benefits:**
- ✅ Zero context overhead for handlers that don't need it
- ✅ Backward compatible (existing handlers keep working)
- ✅ Handlers explicitly opt-in to context
- ✅ No pooling complexity

### Option B: Struct-Based Context (Good for Small Contexts)

Make Context a struct to eliminate heap allocation:

```csharp
// File: src/Mediator/Context.cs
public readonly struct Context : IContext {
    private readonly IPublisher _publisher;
    
    public Guid CorrelationId { get; }
    public DateTimeOffset OccuredAt { get; }

    public Context(IPublisher publisher) {
        _publisher = publisher;
        CorrelationId = Guid.CreateVersion7();  // Still ~30ns, but no allocation
        OccuredAt = DateTimeOffset.UtcNow;      // Still ~15ns, but no allocation
    }
    
    // ...rest of implementation
}
```

**Benefits:**
- ✅ No heap allocation (~15-20ns saved)
- ✅ No GC pressure
- ✅ Simple implementation
- ❌ Still pays ~45ns for Guid/DateTimeOffset creation
- ⚠️ Passed by value (copied on method calls)

### Option C: Cache Timestamps (Simple Optimization)

Reuse timestamps for requests within a time window:

```csharp
// File: src/Mediator/ContextFactory.cs
internal sealed class ContextFactory : IContextFactory {
    private readonly IPublisher _publisher;
    private DateTimeOffset _cachedTime;
    private long _cacheTimestamp;
    private const long CacheWindowTicks = TimeSpan.TicksPerMillisecond * 100; // 100ms window

    public IContext Create() {
        var now = DateTimeOffset.UtcNow;
        var timestamp = now.Ticks;
        
        // Reuse timestamp if within 100ms window
        if (timestamp - _cacheTimestamp < CacheWindowTicks) {
            now = _cachedTime;
        } else {
            _cachedTime = now;
            _cacheTimestamp = timestamp;
        }
        
        return new Context(_publisher, Guid.CreateVersion7(), now);
    }
}
```

**Benefits:**
- ✅ Saves ~15ns on DateTimeOffset.UtcNow (when cache hit)
- ✅ Simple to implement
- ✅ No breaking changes
- ❌ Timestamps slightly less accurate (acceptable for correlation)
- ⚠️ Thread-safety considerations needed

### ❌ Option D: ObjectPool (NOT Recommended)

**Why NOT to use ObjectPool:**
- ❌ Complex: When to return context to pool? Handlers might keep references
- ❌ Unsafe: Risk of reusing a context that's still in use
- ❌ Minimal benefit: Context allocation is not the bottleneck (Guid/DateTimeOffset are)
- ❌ Added complexity: Tracking, lifecycle management, thread-safety

**Don't do this:**
```csharp
// BAD: ObjectPool for Context
var pool = new DefaultObjectPool<Context>();
var ctx = pool.Get();  // Who returns it? When? Risk of use-after-return!
```

---

## Expected Results

| Change | Before | After | Improvement |
|--------|--------|-------|-------------|
| Baseline | 351ns | - | - |
| Fix #1: Cache behaviors | 351ns | ~270ns | **-80ns** |
| Fix #2: Remove Maybe.Match | 270ns | ~200ns | **-70ns** |
| Fix #3: Optimize Context | 200ns | ~120ns | **-80ns** |
| **Total** | **351ns** | **~120ns** | **-231ns (2.9x faster)** |

After all three fixes, your mediator would be **~2x slower than MediatR** instead of **6x slower**.

This is acceptable given the additional features you provide:
- ✅ Context with correlation ID & timestamps
- ✅ Result<T> pattern (Railway-Oriented Programming)
- ✅ Unified event publishing through context
- ✅ More flexible pipeline architecture

---

## Implementation Priority

1. **Start with Fix #1** (easiest, biggest impact): 5 minutes
2. **Then Fix #2** (medium complexity): 15 minutes  
3. **Consider Fix #3** (more complex trade-offs): Discuss architecture first

---

## How to Measure

After each fix, run the benchmark to measure improvement:

```bash
cd benchmarks/MediatorBenchmark
dotnet run -c Release --filter "*Send*response*"
```

Also run the component analysis benchmarks to measure individual parts:

```bash
dotnet run -c Release --filter "*Analysis*"
```

This will show you exactly where the time is being spent.

