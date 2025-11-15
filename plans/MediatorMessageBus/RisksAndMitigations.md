# Risks and Mitigations

## Infinite Message Loops

- **Risk**: Events trigger commands that publish events, leading to uncontrolled recursion.
- **Mitigation**: Provide guidance on idempotency and loop detection; optionally implement recursion depth tracking or duplicate detection policies.

## Handler Non-Idempotency

- **Risk**: At-least-once delivery causes side effects to execute multiple times.
- **Mitigation**: Supply best practices for idempotent handler design; offer deduplication tooling via inbox feature.

## Transport Outages

- **Risk**: Broker downtime prevents message delivery, causing backlog or data loss.
- **Mitigation**: Implement outbox with retry policies and health monitoring; surface alerts when backlog thresholds are exceeded.

## Configuration Complexity

- **Risk**: Distributed messaging configuration becomes difficult to reason about, slowing adoption.
- **Mitigation**: Keep fluent APIs concise, provide templates, and validate configuration at startup with clear error messages.

## Performance Regression

- **Risk**: Additional pipeline stages degrade performance for in-memory operations.
- **Mitigation**: Ensure noop transport adds minimal overhead; benchmark critical paths and gate on acceptable latency increases.

## Security Exposure

- **Risk**: Misconfigured transports or message payloads leak sensitive data.
- **Mitigation**: Rely on operator-managed broker security (TLS, network isolation, secrets rotation) and document least-privilege recommendations; expose configuration checks that highlight insecure transport settings.

## Sensitive Data Governance

- **Risk**: Personally identifiable or regulated data traverses transports without appropriate masking or access controls.
- **Mitigation**: Introduce attribute-based tagging for sensitive payload members, provide policy hooks to validate consuming roles or redact fields before transmission, and rely on secured broker channels managed by platform teams.

## NativeAOT & Trimming Compatibility

- **Risk**: Reflection-heavy patterns or unsupported broker SDKs break NativeAOT compilation or trimming scenarios.
- **Mitigation**: Design core libraries with trimming-friendly patterns, add analyzer checks in CI, and clearly document any transport adapters that cannot support AOT due to third-party limitations.

## Observability Gaps

- **Risk**: Lack of visibility into distributed flows makes troubleshooting difficult.
- **Mitigation**: Mandate OpenTelemetry instrumentation alongside structured logging/metrics, provide default dashboards, and require observability checks in CI/CD.

## Breaking Changes

- **Risk**: Distributed messaging alters existing mediator behavior unexpectedly.
- **Mitigation**: Communicate breaking changes early, provide migration tooling and guides, and supply upgrade verification suites for adopters.

## Embedded Hosting Constraints

- **Risk**: Running inbound message pumps inside the main application may complicate scaling, isolation, or deployment metrics for certain workloads.
- **Mitigation**: Offer clear guidance on when to migrate to dedicated worker services and provide configuration patterns that make the transition low friction.

## Adapter Fragmentation

- **Risk**: Third-party adapters diverge in behavior and quality.
- **Mitigation**: Publish contract tests, authoring guides, and certification criteria for official adapters.
