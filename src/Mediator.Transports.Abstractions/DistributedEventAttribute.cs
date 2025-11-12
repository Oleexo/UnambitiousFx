namespace UnambitiousFx.Mediator.Transports.Abstractions;

/// <summary>
///     Attribute for configuring distributed messaging behavior on message types.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class DistributedEventAttribute : Attribute
{
    /// <summary>
    ///     Gets or sets the distribution mode for the message.
    /// </summary>
    public DistributionMode Mode { get; set; } = DistributionMode.Hybrid;

    /// <summary>
    ///     Gets or sets the topic or queue name for external dispatch.
    /// </summary>
    public string? Topic { get; set; }

    /// <summary>
    ///     Gets or sets the name of the transport to use for external dispatch.
    /// </summary>
    public string? TransportName { get; set; }

    /// <summary>
    ///     Gets or sets whether to fail fast on transport errors.
    /// </summary>
    public bool FailFast { get; set; }

    /// <summary>
    ///     Gets or sets whether to use the outbox pattern for reliable delivery.
    /// </summary>
    public bool UseOutbox { get; set; }
}
