---
layout: default
title: CQRS Boundary Enforcement
parent: Advanced
nav_order: 3
---

# CQRS boundary enforcement (Advanced)

CQRS boundary enforcement is an optional request pipeline behavior that prevents nested mediator requests from executing inside a request handler. It helps you keep a clean separation between commands and queries and avoid accidental reentrancy or hidden coupling between features.

When enabled, attempting to call `ISender.SendAsync(...)` (for any request type) from within another request handler throws a `CqrsBoundaryViolationException` with a clear message showing the offending request names.

- Scope: applies to request handling (`IRequest` and `IRequest<TResponse>`). It does not affect event publishing or stream requests.
- Default: disabled (opt‑in).
- AOT: fully AOT/trimming‑friendly; no runtime reflection.

## How it works

The behavior is implemented by `CqrsBoundaryEnforcementBehavior` and uses `IContext` to carry lightweight metadata across a request execution. The metadata keys are:

- `"__CQRSBoundaryEnforcement"` — marks that a request is currently being handled
- `"__CQRSBoundaryEnforcement_Name"` — stores the current request type name

On entry, the behavior checks the context:
- If no marker is present, it sets the metadata and continues.
- If the marker is already present, it throws `CqrsBoundaryViolationException` similar to:

```
CQRS boundary violation: Cannot send request 'SecondRequest' within a request handler. Boundary was previously crossed by 'FirstRequest'.
```

On exit, the behavior removes the metadata. If removal fails (marker missing), the behavior throws a `CqrsBoundaryViolationException` indicating inconsistent metadata (which likely means the contract was violated).

Notes:
- Because this uses `IContext`, it works across the same logical mediator execution scope (e.g., the same DI scope/scoped context).
- The enforcement is a request pipeline behavior only; it does not run for events or streaming requests.

## Enabling the guard

Enable it via `IMediatorConfig.EnableCqrsBoundaryEnforcement(bool enable = true)` during mediator registration:

```csharp
using Microsoft.Extensions.DependencyInjection;
using UnambitiousFx.Mediator;

services.AddMediator(cfg =>
{
    // Other registrations…
    cfg.RegisterRequestHandler<CreateInvoiceHandler, CreateInvoiceCommand>();
    cfg.RegisterRequestHandler<GetInvoiceHandler, GetInvoiceQuery, Invoice>();

    // Enable the boundary guard (opt‑in)
    cfg.EnableCqrsBoundaryEnforcement();
});
```

- To explicitly disable (useful for tests or selective scenarios): `cfg.EnableCqrsBoundaryEnforcement(false);`
- Ordering: it behaves like any other request behavior. Register it early if you want it to wrap other behaviors as well.

## What a violation looks like

Violations throw `UnambitiousFx.Mediator.Abstractions.CqrsBoundaryViolationException` (subclass of `InvalidOperationException`) with a descriptive message including both request names. Example message:

```
CQRS boundary violation: Cannot send request 'GetCustomerQuery' within a request handler. Boundary was previously crossed by 'CreateOrderCommand'.
```

## Example: triggering a violation

```csharp
using Microsoft.Extensions.DependencyInjection;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Mediator;
using UnambitiousFx.Mediator.Abstractions;

// Requests
public sealed record FirstRequest : IRequest;
public sealed record SecondRequest : IRequest;

// Handlers
public sealed class FirstRequestHandlerThatSendsSecondRequest(ISender sender) : IRequestHandler<FirstRequest>
{
    public async ValueTask<Result> HandleAsync(FirstRequest request, CancellationToken ct = default)
    {
        // This nested send will be blocked when enforcement is enabled
        return await sender.SendAsync(new SecondRequest(), ct);
    }
}

public sealed class ValidSecondRequestHandler : IRequestHandler<SecondRequest>
{
    public ValueTask<Result> HandleAsync(SecondRequest request, CancellationToken ct = default)
        => ValueTask.FromResult(Result.Success());
}

// DI setup
var services = new ServiceCollection();
services.AddMediator(cfg =>
{
    cfg.RegisterRequestHandler<FirstRequestHandlerThatSendsSecondRequest, FirstRequest>();
    cfg.RegisterRequestHandler<ValidSecondRequestHandler, SecondRequest>();

    // Enforce CQRS boundary
    cfg.EnableCqrsBoundaryEnforcement();
});

var provider = services.BuildServiceProvider();
var sender   = provider.GetRequiredService<ISender>();

// This call will throw CqrsBoundaryViolationException
await sender.SendAsync(new FirstRequest());
```

## FAQ and guidance

- Does it distinguish between commands and queries? The behavior is generic: it forbids sending any mediator request from within a request handler, regardless of whether you treat it as a command or a query. This helps keep handlers single‑responsibility and avoids hidden synchronous orchestration.
- Does it affect events? No. It only wraps request handling (`IRequest` / `IRequest<TResponse>`). Event publishing uses event pipeline behaviors and an orchestrator and is unaffected.
- What about streaming requests? The guard does not run for `IStreamRequest<TItem>`; use separate patterns to control streaming composition.
- Can I still compose operations? Prefer publishing domain events, using orchestrators, or invoking application services directly instead of nested mediator sends inside handlers.

## See also
- Configuration reference: ./mediator-config-reference.html (look for `EnableCqrsBoundaryEnforcement`)
- Behaviors overview: ../behaviors/index.html
- Event orchestrator: ./event-orchestrator.html
