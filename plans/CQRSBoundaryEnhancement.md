# Plan: Allow Commands from Event Handlers in CQRS Boundary Enforcement

## Current State Analysis

### Current Behavior

The `CqrsBoundaryEnforcementBehavior` currently:

- Prevents **any request** (command/query) from being sent within a **request handler**
- Uses `IContext` metadata to track if we're inside a request handler
- Metadata keys used:
    - `__CQRSBoundaryEnforcement` (bool) - marks that we're inside a request handler
    - `__CQRSBoundaryEnforcement_Name` (string) - stores the originating request name
- Only applies to **request pipeline** (`IRequestPipelineBehavior`)
- Does **NOT** apply to event handlers (no equivalent behavior for `IEventPipelineBehavior`)

### Desired Behavior

Allow the following flow:

```
Command A → Request Handler A → Publish Event 1 → Event Handler 1 → Command B
```

This means:

- Event handlers should be able to send commands/requests
- The CQRS boundary should be "reset" or "cleared" when entering an event handler context
- However, we still want to prevent nested requests within request handlers themselves

## Solution Strategy

### Recommended Approach: Automatic Boundary Reset in Event Pipeline

**Approach**: Automatically register an `EventCqrsBoundaryResetBehavior` when `EnableCqrsBoundaryEnforcement()` is called. This makes the event handler boundary reset the default behavior without requiring additional configuration.

**Pros**:

- Clean separation of concerns
- Event handlers get a fresh CQRS context automatically
- Original request context is preserved for logging/tracing
- Minimal changes to existing code
- Easy to understand and maintain
- No additional configuration needed - "just works"
- Intuitive default behavior (events can trigger commands)

**Cons**:

- None significant - this is the expected behavior for most CQRS patterns

**Implementation**:

1. Create new `EventCqrsBoundaryResetBehavior : IEventPipelineBehavior`
2. In `HandleAsync`:
    - Save current boundary metadata (if present)
    - Clear boundary metadata before calling `next()`
    - Restore boundary metadata after `next()` completes
3. Modify `EnableCqrsBoundaryEnforcement()` to automatically register both behaviors
4. Update documentation

## Recommended Implementation Plan

### Phase 1: Core Implementation

#### 1.1 Create EventCqrsBoundaryResetBehavior

**File**: `src/Mediator/Pipelines/EventCqrsBoundaryResetBehavior.cs`

```csharp
/// <summary>
/// Pipeline behavior that resets CQRS boundary enforcement when entering event handlers.
/// This allows event handlers to send commands/requests, enabling the pattern:
/// Command A → Event 1 → Event Handler → Command B
/// This behavior is automatically registered when EnableCqrsBoundaryEnforcement() is called.
/// </summary>
public sealed class EventCqrsBoundaryResetBehavior : IEventPipelineBehavior
{
    private const string CQRSBoundaryEnforcementKey = "__CQRSBoundaryEnforcement";
    private const string CQRSBoundaryEnforcementNameKey = "__CQRSBoundaryEnforcement_Name";
    
    private readonly IContext _context;

    public EventCqrsBoundaryResetBehavior(IContext context)
    {
        _context = context;
    }

    public async ValueTask<Result> HandleAsync<TEvent>(
        TEvent @event,
        EventHandlerDelegate next,
        CancellationToken cancellationToken = default)
        where TEvent : IEvent
    {
        // Save current boundary state
        bool hadBoundary = _context.TryGetMetadata<bool>(CQRSBoundaryEnforcementKey, out var wasInRequest);
        string? previousRequestName = hadBoundary 
            ? _context.TryGetMetadata<string>(CQRSBoundaryEnforcementNameKey, out var name) ? name : null
            : null;

        // Clear boundary metadata to allow event handlers to send requests
        if (hadBoundary)
        {
            _context.RemoveMetadata(CQRSBoundaryEnforcementKey);
            _context.RemoveMetadata(CQRSBoundaryEnforcementNameKey);
        }

        try
        {
            // Execute event handlers with cleared boundary
            return await next();
        }
        finally
        {
            // Restore original boundary state
            if (hadBoundary)
            {
                _context.SetMetadata(CQRSBoundaryEnforcementKey, true);
                if (previousRequestName != null)
                {
                    _context.SetMetadata(CQRSBoundaryEnforcementNameKey, previousRequestName);
                }
            }
        }
    }
}
```

#### 1.2 Update EnableCqrsBoundaryEnforcement Method

**File**: `src/Mediator/MediatorConfig.cs`

Modify the existing method to automatically register both behaviors:

```csharp
public IMediatorConfig EnableCqrsBoundaryEnforcement(bool enable = true)
{
    if (!enable)
        return this;

    // Register the request pipeline behavior to enforce boundaries
    RegisterRequestPipelineBehavior<CqrsBoundaryEnforcementBehavior>();
    
    // Automatically register the event pipeline behavior to reset boundaries for event handlers
    RegisterEventPipelineBehavior<EventCqrsBoundaryResetBehavior>();
    
    return this;
}
```

**Note**: No changes needed to `IMediatorConfig.cs` interface - the method signature remains the same.

### Phase 2: Testing

#### 2.1 Create Comprehensive Tests

**File**: `test/Mediator.Tests/CqrsBoundaryWithEventsTests.cs`

Test scenarios:

1. ✅ Command A → Publish Event → Event Handler sends Command B (should succeed when enforcement enabled)
2. ✅ Command A → Publish Event → Event Handler sends Query (should succeed when enforcement enabled)
3. ✅ Command A → Command B (should still fail - direct nesting)
4. ✅ Event Handler → Publish Another Event → Event Handler 2 sends Command (should succeed)
5. ✅ Verify boundary is restored after event handling completes
6. ✅ Multiple event handlers can each send their own commands
7. ✅ Event handler sends command that publishes event that sends command (nested events work)
8. ✅ Event handlers can send commands even without enforcement enabled (no boundary to reset)
9. ✅ Exception in event handler properly restores boundary state

#### 2.2 Update Existing Tests

- Ensure existing `CqrsBoundaryEnforcementTests` still pass
- No breaking changes to existing behavior

### Phase 3: Documentation

#### 3.1 Update CQRS Boundary Enforcement Doc

**File**: `docs/docs/mediator/advanced/cqrs-boundary-enforcement.markdown`

Update existing documentation to clarify:

- "Event Handlers and CQRS Boundaries" section
- Explain that event handlers automatically get a fresh CQRS context
- Show the Command → Event → Command pattern
- Add code example demonstrating the flow
- Note that this is the default behavior when enforcement is enabled

#### 3.2 Update Configuration Reference

**File**: `docs/docs/mediator/advanced/mediator-config-reference.markdown`

Update `EnableCqrsBoundaryEnforcement()` documentation:

- Clarify that it automatically allows event handlers to send commands
- Mention that both request and event pipeline behaviors are registered
- No additional configuration needed

#### 3.3 Create Example

**File**: `examples/ConsoleApp/EventToCommandExample/`

Create a practical example showing:

- Command that publishes domain event
- Event handler that sends follow-up command
- Demonstrates real-world use case (e.g., order processing)

### Phase 4: Optional Enhancements

#### 4.1 Add Logging/Diagnostics

- Optional: Add diagnostic logging when boundary is cleared/restored
- Could use `ILogger` if available

#### 4.2 Add Metadata Tracking

- Optional: Track the event that cleared the boundary
- Could help with debugging complex event chains

## Migration Guide for Users

### Before (Events couldn't be tested with commands)

```csharp
services.AddMediator(cfg =>
{
    cfg.RegisterRequestHandler<CreateOrderHandler, CreateOrderCommand>();
    cfg.RegisterEventHandler<OrderCreatedHandler, OrderCreatedEvent>();
    cfg.EnableCqrsBoundaryEnforcement();
});
```

**Behavior**:

- Request handlers CANNOT send nested requests (enforced)
- Event handlers COULD send commands (but behavior was undefined/untested)

### After (Automatic support for Event → Command pattern)

```csharp
services.AddMediator(cfg =>
{
    cfg.RegisterRequestHandler<CreateOrderHandler, CreateOrderCommand>();
    cfg.RegisterEventHandler<OrderCreatedHandler, OrderCreatedEvent>();
    
    // Same configuration - now automatically handles event → command pattern
    cfg.EnableCqrsBoundaryEnforcement();
});
```

**Behavior**:

- Request handlers CANNOT send nested requests (enforced)
- Event handlers CAN send commands/queries (automatically supported)
- No additional configuration needed!

### Key Points

- **No breaking changes** - existing code works the same way
- **Automatic behavior** - event handlers can now reliably send commands when CQRS enforcement is enabled
- **Still opt-in** - only active when `EnableCqrsBoundaryEnforcement()` is called
- **No migration required** - if you weren't sending commands from events, nothing changes

## Implementation Checklist

- [ ] Create `EventCqrsBoundaryResetBehavior` class
- [ ] Modify `EnableCqrsBoundaryEnforcement()` in `MediatorConfig` to auto-register event behavior
- [ ] Create `CqrsBoundaryWithEventsTests` test file
- [ ] Write all test scenarios
- [ ] Run all tests and ensure they pass
- [ ] Update `cqrs-boundary-enforcement.markdown` documentation
- [ ] Update `mediator-config-reference.markdown` documentation
- [ ] Create example in `examples/ConsoleApp/`
- [ ] Review and test end-to-end
- [ ] Update CHANGELOG if applicable

## Potential Issues & Mitigations

### Issue 1: Infinite Event Loops

**Risk**: Event A triggers Command B, which publishes Event A again
**Mitigation**:

- Document this risk clearly
- Users should design events to be idempotent
- Could add optional recursion depth tracking in future

### Issue 2: Context Corruption

**Risk**: Exception during event handling leaves boundary in wrong state
**Mitigation**:

- Use try/finally to ensure restoration
- Already implemented in the proposed solution

### Issue 3: Thread Safety

**Risk**: Concurrent event handling might interfere
**Mitigation**:

- `IContext` should be scoped per request
- Each request gets its own context instance
- No shared state concerns

## Alternative Patterns to Consider

Users could also achieve similar results without this feature by:

1. **Injecting ISender into Event Handler** - This already works! Events don't go through boundary enforcement
2. **Using Application Services** - Call a service that sends the command
3. **Publishing Follow-up Events** - Chain events instead of mixing events and commands

However, the direct pattern (Event → Command) is more intuitive and cleaner for many scenarios.

## Conclusion

This plan provides a clean, backward-compatible way to enable the common pattern of event handlers sending commands. The implementation is minimal, focused, and leverages the existing metadata system. The key insight is that event handlers should get a "fresh" CQRS boundary context, allowing them to send requests without triggering violations.
