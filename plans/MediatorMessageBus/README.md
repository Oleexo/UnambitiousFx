# Mediator Message Bus Pivot

## Purpose

Provide a structured roadmap for evolving the existing in-process mediator into a distributed messaging hub that combines mediator ergonomics with broker-backed delivery while keeping the experience simple to configure and understand.

## Vision

- Deliver a distributed-first mediator experience tailored for hybrid and cross-service workloads without sacrificing the ease of the current mediator.
- Simplify handler development while accepting breaking changes that streamline the new architecture.
- Remain broker-agnostic while offering first-class integration hooks for Azure Service Bus, RabbitMQ, AWS SNS/SQS, Kafka, and future transports.
- Ship with opinionated reliability, observability, and tooling so teams can trust behaviour in production from day one.
- Ensure teams can adopt distributed features incrementally, avoiding a "monster of complexity" when only mediator functionality is required.

## Guiding Principles

- **Transport Agnostic**: Handlers speak to mediator abstractions; adapters encapsulate transport-specific details including serialization, security, and topology.
- **Simplicity First**: Default configuration mirrors today's mediator; distributed features remain opt-in with safe, opinionated defaults.
- **Distributed-First**: Prioritise the new distributed messaging model even when it requires breaking changes to existing contracts.
- **Deterministic & Idempotent**: Provide primitives and guidance that make deduplication, replay, and saga orchestration predictable.
- **Observability First**: Logs, traces, and metrics accompany every publish and receive operation, with correlation data preserved end-to-end.
- **Low Ceremony**: Message distribution rules are declared through attributes or fluent configuration, avoiding repetitive boilerplate in handlers.
- **Runtime Ready**: Core libraries must support .NET NativeAOT and trimming, with clear documentation when third-party broker SDKs impose limits.

## Document Map

| File                             | Focus                                                 |
| -------------------------------- | ----------------------------------------------------- |
| `Architecture.md`                | End-to-end architecture and key components            |
| `CoreMessagingModel.md`          | Envelope design, metadata, message types              |
| `TransportAbstractions.md`       | Transport contracts, adapters, broker neutrality      |
| `PublishingPipeline.md`          | Outbound flow, hybrid dispatch, configuration knobs   |
| `InboundHosting.md`              | Message pump, subscriptions, host lifecycle           |
| `ReliabilityAndDelivery.md`      | Outbox, retries, deduplication, poison handling       |
| `DeveloperExperience.md`         | Configuration API, annotations, templates             |
| `ObservabilityAndDiagnostics.md` | Logging, tracing, metrics, troubleshooting            |
| `Roadmap.md`                     | Phased delivery plan with milestones and dependencies |
| `OpenQuestions.md`               | Outstanding decisions and research topics             |
| `RisksAndMitigations.md`         | Known risks with mitigation strategies                |
| `SuccessCriteria.md`             | Measurable outcomes required for launch               |

Use these documents together to plan sequencing, align stakeholders, and communicate progress as the mediator evolves into a distributed message platform.
