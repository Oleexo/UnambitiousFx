# Developer Experience Plan

## Goals

- Deliver a configuration model that keeps the mediator setup approachable for teams of varying maturity and preserves the zero-config in-process experience.
- Provide tooling, templates, and documentation that reduce friction when enabling distributed messaging in deliberate, incremental steps.
- Ensure tests remain simple, with an in-memory transport for unit and integration scenarios.
- Provide async-first ergonomics, including AsyncAPI documentation workflows for distributed contracts, without forcing these workflows on teams that stay local-only.

## Configuration Surface

1. **Fluent API**
   - Extend `IMediatorConfig` with a single `EnableDistributedMessaging()` entry point that activates defaults (noop transport, no outbox) until further configuration is provided.
   - Support optional chaining policies (transport selection, mode, fail-fast, outbox usage) that remain hidden until the developer opts into them.
2. **Attribute-Based Markers**
   - Introduce `[DistributedEvent]`, `[DistributedCommand]`, etc. as optional sugar over the fluent configuration so simple scenarios avoid duplicating rules.
   - Allow additional parameters (topic, partition key, fail-fast, sensitivity/role metadata) via attribute properties while providing validation that prevents over-specification when defaults suffice.
3. **Convention Support**
   - Provide hooks to scan assemblies for known naming conventions (e.g., events ending with `Event`).
   - Enable custom convention registrars that can enrich configuration without replacing the default, developer-friendly behaviour.
4. **Options Binding**
   - Integrate with `IOptions<T>` for environment-specific configuration (connection strings, topics) only when distributed messaging is enabled.
   - Support JSON/YAML configuration templates with conservative defaults to keep onboarding simple.

## Progressive Disclosure

- Keep the out-of-the-box upgrade path to a single `EnableDistributedMessaging()` call that preserves current in-memory behaviour.
- Gate advanced transports, reliability policies, and AsyncAPI generation behind explicit configuration sections or package references.
- Provide configuration diagnostics that highlight when optional features are partially configured, helping teams roll forward gradually.

## Tooling & Templates

- **Project Templates**: Provide sample solutions demonstrating hybrid mediator usage (web API + worker).
- **CLI Helpers**: Optionally add tooling to scaffold transport registrations or outbox database migrations.
- **Snippet Library**: Document common setup patterns for different brokers, starting with the default single-transport story.
- **AsyncAPI Tooling**: Generate AsyncAPI definitions from mediator message traits and surface automation hooks for publishing them.
- **Analyzer Support (Future)**: Consider Roslyn analyzer that warns when distributed messages lack attributes or configuration.
- **Embedded Hosting Defaults**: Ship configuration templates that wire inbound hosting into the main app, with optional guidance for scaling out to dedicated workers.

## Testing Guidance

- Default to in-memory transport for unit tests to avoid broker dependencies.
- Offer contract test harness to validate custom transport implementations in CI.
- Provide examples for integration testing with docker-compose for supported brokers.
- Run NativeAOT and trimming validation builds to ensure developer workflows surface incompatible patterns early.

## Documentation Tasks

- [ ] Update README and docs landing page to reflect distributed messaging capability.
- [ ] Author step-by-step guide for enabling the feature in an existing project.
- [ ] Produce FAQ covering common misconfigurations and troubleshooting steps.
- [ ] Include DX-focused best practices (e.g., naming conventions, attribute usage).
- [ ] Document AsyncAPI generation workflow and distribution strategy for downstream consumers.

## Acceptance Criteria

- Developers can enable distributed messaging with minimal code changes.
- Documentation includes at least one end-to-end tutorial and reference section for configuration options, with quick-start content showing the minimal configuration path.
- Samples demonstrate both basic and advanced configurations.
- Testing guidance covers unit, integration, and contract testing scenarios.
