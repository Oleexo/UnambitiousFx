namespace UnambitiousFx.Mediator.Transports.Abstractions;

/// <summary>
///     Defines how messages are distributed across local and external boundaries.
/// </summary>
public enum DistributionMode
{
    /// <summary>
    ///     Messages are processed only by local in-process handlers.
    /// </summary>
    LocalOnly = 0,

    /// <summary>
    ///     Messages are processed by both local handlers and external transports.
    /// </summary>
    Hybrid = 1,

    /// <summary>
    ///     Messages are dispatched only to external transports, skipping local handlers.
    /// </summary>
    ExternalOnly = 2
}
