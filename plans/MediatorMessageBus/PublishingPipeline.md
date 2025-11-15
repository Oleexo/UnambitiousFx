# Publishing Pipeline Plan

## Scope

Extend the mediator send and publish flows to support hybrid delivery (local + external) and transport-only scenarios while preserving existing handler execution semantics and keeping the local-only path unchanged by default.

## Pipeline Stages

1. **Message Preparation**
   - Build envelope using context metadata and message traits.
   - Enrich with tracing, correlation, and tenant information.
2. **Policy Evaluation**
   - Determine distribution mode (local, hybrid, external-only) based on attributes/configuration, defaulting every message to local mode until explicitly overridden.
   - Apply reliability policy (synchronous dispatch, outbox deferred dispatch, fire-and-forget with background worker) only when the application opts in.
3. **Local Dispatch**
   - Execute existing mediator handler pipeline when policy requires local processing.
4. **External Dispatch**
   - Forward envelope to transport dispatcher only for messages that have opted into hybrid or external policies.
   - Handle synchronous success/failure or queue to outbox for eventual dispatch.
5. **Result Aggregation**
   - Merge results/errors from local and external operations according to fail-fast or best-effort configuration.
   - Update context metadata to reflect dispatch outcome for downstream behaviors.

## Configuration Design

- Fluent API example:

```csharp
cfg.EnableDistributedMessaging(options =>
{
    options.ForEvent<OrderCreatedEvent>()
        .UseHybridMode()
        .WithTransport("ServiceBus")
        .FailFastOnTransportErrors();

    options.ForCommand<ShipOrderCommand>()
        .RouteExternally()
        .UseOutbox();
});
```

- Support convention-based registration (assembly scanning) with manual overrides for specific message types.

## Error Handling Strategies

- **Fail-Fast**: Exception or failure result bubbles up immediately, halting local handler execution when configured.
- **Best-Effort**: Local result returned even if transport fails; failure logged and surfaced via metrics.
- **Deferred**: Outbox ensures transport dispatch executes in background, decoupling from request thread.

## Default Experience

- Upgrading to the new pipeline without calling `EnableDistributedMessaging()` leaves behaviour identical to the current mediator implementation.
- Enabling distributed messaging without further options keeps dispatch local while wiring up infrastructure for future external delivery.
- Configuration diagnostics should highlight when advanced policies are partially configured to prevent surprises during incremental adoption.

## Tasks

- [ ] Implement pipeline behavior that encapsulates preparation, policy evaluation, and dual dispatch.
- [ ] Integrate with existing outbox infrastructure; define contract for adapters to record envelopes.
- [ ] Provide configuration objects and builder methods for defining distribution policies.
- [ ] Add telemetry hooks (logging/tracing) for each stage with consistent event naming.
- [ ] Write unit tests covering policy combinations, error paths, and result aggregation.

## Acceptance Criteria

- Hybrid messages invoke local handlers and transport dispatch exactly once each.
- Configuration allows per-message overrides for fail-fast, outbox, and transport choice.
- Errors in transport dispatch are surfaced according to policy without corrupting mediator state.
- Observability hooks emit structured information for success and failure scenarios.
