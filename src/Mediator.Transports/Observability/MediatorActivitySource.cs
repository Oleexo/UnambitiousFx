using System.Diagnostics;

namespace UnambitiousFx.Mediator.Transports.Observability;

/// <summary>
///     Provides the ActivitySource for OpenTelemetry distributed tracing in the mediator transport system.
/// </summary>
public static class MediatorActivitySource
{
    /// <summary>
    ///     The ActivitySource for creating activity spans for distributed tracing.
    /// </summary>
    public static readonly ActivitySource Source = new("Unambitious.Mediator", "1.0.0");
}
