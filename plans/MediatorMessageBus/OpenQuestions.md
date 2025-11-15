# Open Questions

## Transport Prioritization

- Which brokers should receive first-class adapters for the initial release? → **SQS/SNS (AWS) and RabbitMQ**.
- Do we need to support multiple transports in a single application from day one? → **No—single transport support is sufficient initially.**

## Serialization Strategy

- Should JSON remain the default serialization format, or do we require pluggable serializer selection per message type or transport? → **JSON-only for the initial release**.
- How do we manage schema evolution across services (version headers, schema registry integration)? → **Adopt semantic schema headers, generate AsyncAPI specs per version, store them in a shared catalog, and enforce compatibility via CI contract tests**.

## Command Routing Scope

- Are cross-process commands in scope, or should distributed messaging focus on events only for the first iteration? → **Keep commands in-process; no cross-service command dispatch initially**.
- If commands are supported, how do we model responses and correlation when multiple services are involved? (out of scope for initial release)

## Hosting Options

- Should inbound hosting be embedded in existing applications by default, or should we promote dedicated worker services? → **Embed inbound hosting in the main application by default; worker services remain optional guidance.**
- What tooling is needed to help teams deploy and scale listener hosts (e.g., container templates, Helm charts)? (out of scope for initial release)

## Security Requirements

- What authentication and authorization mechanisms must the transport adapters support out of the box?
- Do we need to offer payload encryption hooks beyond transport-level security (TLS)? → **No—assume brokers are secured operationally; payload encryption can be layered later if needed.**

## Operational Boundaries

- How will teams monitor the outbox/inbox stores and respond to backlog growth? **Using OpenTelemetry metrics and alerts.**
- Do we provide management APIs for pausing/resuming subscriptions or purging queues? **No—leverage broker-native management tools initially.**

## Governance

- Do we require schema approval workflows or message catalogues for larger organizations?
- How do we prevent drift between message definitions in different services?
