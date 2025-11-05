# ObjectPool Pattern - When and Why to Use It

## What is ObjectPool?

`ObjectPool<T>` is a creational design pattern that **reuses objects** instead of creating new ones. It's provided by Microsoft in the `Microsoft.Extensions.ObjectPool` NuGet package.

## The Core Concept

```
Traditional Approach:                Object Pool Approach:
┌────────────────┐                  ┌────────────────────┐
│ Request 1      │                  │   Object Pool      │
│ new Object()   │─→ GC             │  ┌───┐ ┌───┐ ┌───┐│
└────────────────┘                  │  │ 1 │ │ 2 │ │ 3 ││
┌────────────────┐                  │  └───┘ └───┘ └───┘│
│ Request 2      │                  └────────────────────┘
│ new Object()   │─→ GC                  ↓ Rent  ↑ Return
└────────────────┘                  ┌────────────────┐
┌────────────────┐                  │ Request 1      │
│ Request 3      │                  │ Use obj #1     │
│ new Object()   │─→ GC             └────────────────┘
└────────────────┘                  ┌────────────────┐
                                    │ Request 2      │
Result: Many allocations            │ Use obj #2     │
        GC pressure                 └────────────────┘
        Slower                      ┌────────────────┐
                                    │ Request 3      │
                                    │ Reuse obj #1   │
                                    └────────────────┘
                                    
                                    Result: Few allocations
                                            Less GC pressure
                                            Faster
```

## When to Use ObjectPool

### ✅ Good Use Cases

1. **Expensive Object Creation**
   - Large buffers (byte arrays, StringBuilder with capacity)
   - Objects that make system calls during initialization
   - Objects with complex initialization logic

2. **High Allocation Rate**
   - Thousands+ objects per second
   - Short-lived objects (used and discarded quickly)
   - Predictable usage patterns

3. **Clear Ownership/Lifetime**
   - You control when object is acquired
   - You control when object is returned
   - No shared references that outlive the usage

### Example: byte[] Buffers (Perfect Use Case)

```csharp
// Bad: Allocates 4KB every request
public async Task ProcessRequest(Stream stream) {
    byte[] buffer = new byte[4096]; // ❌ Allocation
    await stream.ReadAsync(buffer, 0, buffer.Length);
    // Process buffer
    // buffer goes out of scope → GC
}

// Good: Reuses buffers from pool
public async Task ProcessRequest(Stream stream) {
    byte[] buffer = ArrayPool<byte>.Shared.Rent(4096); // ✅ Rent
    try {
        await stream.ReadAsync(buffer, 0, buffer.Length);
        // Process buffer
    } finally {
        ArrayPool<byte>.Shared.Return(buffer); // ✅ Return
    }
}
```

**Why this works:**
- Clear lifetime: Acquire → Use → Return
- No escaping references
- High frequency (many requests/second)
- Expensive allocation (4KB each time)

### ❌ Bad Use Cases

1. **Unclear Lifetime**
   - Object passed to async methods
   - Object stored in fields
   - Object shared between components
   - Return timing unknown

2. **Complex State**
   - Object holds resources (connections, files)
   - State that's hard to reset
   - Mutable state that could leak between uses

3. **Minimal Benefit**
   - Small objects (<100 bytes)
   - Low allocation rate (<1000/sec)
   - Creation already fast (<10ns)

## Why ObjectPool is Wrong for Your Context

Let's analyze your specific case:

### Your Context Object

```csharp
internal sealed class Context : IContext {
    private readonly IPublisher _publisher;
    public Guid CorrelationId { get; }           // 16 bytes
    public DateTimeOffset OccuredAt { get; }     // 16 bytes
    // + object header (8-16 bytes)
    // Total: ~40-48 bytes
}
```

### The Problems

#### Problem 1: Unclear Return Point

```csharp
// In Sender
public ValueTask<Result<TResponse>> SendAsync<TRequest, TResponse>(...) {
    var ctx = _contextFactory.Create(); // Get from pool
    var result = await handler.HandleAsync(ctx, request, ct);
    // ❌ Can we return ctx to pool now?
    // What if handler stored it?
    // What if it's in an async continuation?
    return result;
}

// In a Handler
public async ValueTask<Result<int>> HandleAsync(IContext ctx, MyRequest req, CancellationToken ct) {
    await DoSomethingAsync();
    await ctx.PublishEventAsync(new MyEvent(), ct); // Using context after await
    // Context might be returned to pool already!
    return Result.Success(42);
}
```

#### Problem 2: Small Object

```csharp
Size: ~48 bytes (small!)
Cost: 
  - Allocation: ~15ns
  - Guid.CreateVersion7(): ~30ns  ← Real bottleneck!
  - DateTimeOffset.UtcNow: ~15ns  ← Real bottleneck!
  
Total: ~60ns (45ns is NOT from allocation)

Pooling saves: ~15ns (allocation only)
Pooling costs: Complexity, safety risks
Net benefit: Not worth it!
```

#### Problem 3: State Must Be Fresh

```csharp
// Each request MUST have unique values
public Context(IPublisher publisher) {
    CorrelationId = Guid.CreateVersion7(); // MUST be unique per request
    OccuredAt = DateTimeOffset.UtcNow;     // MUST be accurate per request
}

// Pooling doesn't help - you still call these expensive operations!
public bool Return(Context obj) {
    obj.Reset(Guid.CreateVersion7(), DateTimeOffset.UtcNow); // Same cost!
    return true;
}
```

## Real-World ObjectPool Examples

### Example 1: ASP.NET Core StringBuilder Pool

```csharp
// ASP.NET Core uses this for formatting
var builder = _stringBuilderPool.Get();
try {
    builder.AppendFormat("Hello {0}", name);
    return builder.ToString();
} finally {
    builder.Clear();
    _stringBuilderPool.Return(builder);
}
```

**Why it works:**
- ✅ Clear lifetime (try-finally)
- ✅ Easy to reset (Clear())
- ✅ Expensive allocation (initial capacity)
- ✅ High frequency

### Example 2: HttpClient Handler Pool

```csharp
// HttpClientFactory internally pools HttpMessageHandler
var handler = _pool.Get(); // Reuse TCP connections
try {
    var response = await handler.SendAsync(request);
    return response;
} finally {
    _pool.Return(handler); // Keep connection alive
}
```

**Why it works:**
- ✅ Very expensive to create (TCP connection)
- ✅ Clear ownership
- ✅ Stateless between uses
- ✅ Huge performance benefit

### Example 3: JSON Serialization Buffers

```csharp
// Newtonsoft.Json uses ArrayPool internally
var buffer = ArrayPool<char>.Shared.Rent(1024);
try {
    // Serialize JSON into buffer
    JsonConvert.Serialize(obj, buffer);
} finally {
    ArrayPool<char>.Shared.Return(buffer);
}
```

**Why it works:**
- ✅ Large allocation (1KB+)
- ✅ Very high frequency (every JSON operation)
- ✅ Simple to reset (overwrite data)
- ✅ Clear lifetime

## Better Alternatives for Your Context

### Option 1: Lazy Creation (Best)

```csharp
// Don't create context unless handler needs it
if (handler is IContextlessHandler contextless) {
    return contextless.HandleAsync(request, ct); // No context!
} else {
    var ctx = _contextFactory.Create();
    return handler.HandleAsync(ctx, request, ct);
}
```

**Saves:** 60ns for handlers that don't need context

### Option 2: Struct Context

```csharp
public readonly struct Context : IContext {
    // ...fields
}
```

**Saves:** ~15ns (allocation only)

### Option 3: Cached Timestamps

```csharp
// Reuse DateTimeOffset.UtcNow for 100ms window
if (now - _cachedTime < 100ms) {
    return _cachedTime; // Save ~15ns
}
```

**Saves:** ~15ns on DateTimeOffset.UtcNow

## Summary

| Approach | Complexity | Benefit | Risk |
|----------|-----------|---------|------|
| **ObjectPool** | High | ~15ns | High (use-after-return) |
| **Lazy Creation** | Medium | ~60ns | Low |
| **Struct Context** | Low | ~15ns | Low |
| **Cached Timestamps** | Low | ~15ns | None |

**Recommendation for your code:**
1. Use **Lazy Creation** (Option A in QUICK_WINS.md)
2. Consider **Struct Context** if you need more
3. Avoid **ObjectPool** for Context

## Key Takeaways

1. **ObjectPool is not a silver bullet** - use only when justified
2. **Profile first** - know where the cost actually is
3. **Measure the benefit** - is complexity worth the gain?
4. **Consider alternatives** - often simpler solutions work better
5. **Lifetime matters** - pooling only works with clear ownership

ObjectPool is powerful but complex. In your case, the simpler alternatives (lazy creation, struct) provide better results with less risk.

