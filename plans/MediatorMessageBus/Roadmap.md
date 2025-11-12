# Delivery Roadmap

## Phase 0 – Research & Alignment

- Inventory existing mediator features that will be affected (pipelines, outbox, context).
- Interview current users to understand distributed messaging expectations and preferred brokers.
- Document success metrics and non-goals to align stakeholders.
- Produce architecture decision records (ADRs) for core concepts (envelope, transport abstraction).

## Phase 1 – Core Abstractions

- Implement message envelope, header schema, and serialization interfaces.
- Introduce transport abstractions with noop transport default.
- Extend configuration APIs to register transports and mark distributed messages.
- Prototype sensitive-data tagging (attribute + policy evaluation) within the traits registry.
- Design async-first developer ergonomics, including generation of AsyncAPI artefacts from message contracts.
- Ensure core abstractions compile cleanly under NativeAOT and trimming analyzers (no warning regressions).
- Introduce semantic schema headers and generate AsyncAPI specs per message version, publishing them to the shared catalog.
- Provide initial documentation for internal review.

**Exit Criteria**
- Unit tests validate envelope creation and transport registration.
- Noop transport keeps existing apps functioning without changes.

## Phase 2 – Outbound Distribution

- Add publishing pipeline behavior supporting local, hybrid, and external dispatch.
- Integrate telemetry for outbound operations.
- Support fail-fast vs. best-effort policies and outbox staging for deferred dispatch.
- Create contract tests with mock transport.
- Validate sensitivity tags during outbound dispatch (encryption, redaction, access policies).
- Enforce OpenTelemetry instrumentation as part of outbound pipeline work.

**Exit Criteria**
- Hybrid dispatch validated via automated tests.
- Documentation updated with configuration examples for outbound scenarios.

## Phase 3 – Inbound Consumption

- Build hosted service infrastructure for subscriptions and message pumps, embedding the default host inside the primary application.
- Implement deserialization, handler invocation, and acknowledgement logic.
- Support per-subscription concurrency, retry, and dead-letter policies.
- Publish sample worker host showing configuration and deployment pattern.
- Enforce sensitive-data policies on inbound messages (role validation, field masking before handler invocation).
- Demonstrate AsyncAPI documentation flow for inbound subscriptions.
- Instrument inbound processing with OpenTelemetry spans and context propagation.
- Validate consumer support for multi-version schemas and contract-test against catalogued AsyncAPI definitions.

**Exit Criteria**
- Integration tests demonstrate end-to-end external publish/receive.
- Sample project successfully processes messages from mock transport.

## Phase 4 – Reliability Enhancements

- Extend outbox, introduce optional inbox, and wire retry policies.
- Provide poison handling and dead-letter routing capabilities.
- Benchmark performance impact and optimize hot paths.

**Exit Criteria**
- Reliability features configurable per message type.
- Benchmarks show acceptable overhead under expected load.

## Phase 5 – Transport Adapters & Samples

- Deliver first-party adapters (prioritize Azure Service Bus, RabbitMQ, AWS SNS/SQS based on research).
- Create sample solutions demonstrating cross-service workflows.
- Document adapter authoring guide for community contributions.
- Validate NativeAOT/trimming compatibility for first-party transports where SDKs permit; document limitations when broker libraries do not support AOT.

**Exit Criteria**
- At least one official adapter passes contract tests and sample scenario.
- Documentation includes end-to-end walkthrough with chosen adapter.

## Phase 6 – Documentation & Enablement

- Publish comprehensive docs covering architecture, configuration, troubleshooting, and migration.
- Update marketing/README materials to reflect new positioning.
- Provide migration checklist for existing users.
- Deliver AsyncAPI documentation toolkit and publish sample schema bundles.
- Ship OpenTelemetry integration guide and reference dashboards as part of documentation.

**Exit Criteria**
- Documentation reviewed by beta users.
- Migration guide validated through pilot implementation.

## Phase 7 – Observability & Advanced Features (Optional)

- Complete OpenTelemetry instrumentation and metrics exporters.
- Introduce management surfaces (diagnostics endpoints, dashboards).
- Explore advanced features such as sagas, schema registry integration, multi-broker publishing.

**Exit Criteria**
- Observability stack integrates with reference monitoring tooling.
- Decision made on scope of advanced features for GA vs. post-GA.

## Tracking & Governance

- Maintain progress board pointing to detailed tasks from each phase.
- Update ADRs when major decisions change.
- Schedule regular stakeholder reviews at phase boundaries.
