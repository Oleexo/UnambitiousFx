# Observability and Diagnostics Plan

## Objectives

- Ensure every publish and receive action is observable through logs, metrics, and traces without burdening the local-only upgrade path.
- Preserve correlation across service boundaries to simplify debugging distributed flows.
- Provide actionable diagnostics and health endpoints for operators with sensible defaults and optional advanced sinks.

## Logging Strategy

- Structured logs emitted at key stages: envelope creation, transport dispatch start/end, handler invocation result, retry events, dead-letter routing.
- Include message id, correlation id, transport name, message type, sensitivity flags, and latency measurements.
- Ship with a minimal logging configuration that works with the platform default logger; support log level customization per transport or message type when distributed messaging is enabled.

## Tracing Plan

- Integrate with OpenTelemetry to create spans for outbound and inbound operations, while keeping instrumentation opt-in so local-only adopters are not forced to depend on tracing packages.
- Propagate trace context through envelope headers, supporting W3C Trace Context.
- Use span attributes for message metadata and outcomes (success, retry, failure).
- Link spans between publisher and consumer when correlation data is available.

## Metrics

- Counters: messages published, messages consumed, retries attempted, dead-lettered messages.
- Counters: sensitivity rule violations, redactions performed, encryption failures.
- Histograms: publish latency, consume latency, handler execution duration.
- Gauges: outbox backlog size, in-flight message count, subscription health.
- Provide a lightweight metrics collector that can emit to an in-memory or logging-based sink by default; expose Prometheus-friendly exporters and other backends through optional packages.
- Track schema-version adoption metrics (per version publish/consume counts) to monitor migration progress.

## Diagnostics & Health

- Health checks reporting transport connectivity, subscription readiness, outbox backlog thresholds with defaults that degrade gracefully when transports are not enabled.
- Diagnostic commands or endpoints to dump current subscriptions, pending messages, and recent failures.
- Correlation ID echo endpoints to assist with distributed tracing in support scenarios.

## Implementation Tasks

- [ ] Instrument pipeline behaviors and message pumps with structured logging.
- [ ] Add OpenTelemetry instrumentation package or custom activity sources (required for release).
- [ ] Implement metrics collectors with multi-provider support.
- [ ] Wire health checks into ASP.NET health endpoint extension and document usage.
- [ ] Provide troubleshooting guide covering common failure scenarios and diagnostic steps.

## Acceptance Criteria

- Operators can trace a message from publish through consumption across services using logs or traces.
- Metrics enable alerting on delivery failures or backlog growth.
- Health checks expose actionable status with clear remediation guidance.
- Documentation includes sample dashboards and alerting recommendations.
