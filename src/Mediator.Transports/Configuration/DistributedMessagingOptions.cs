namespace UnambitiousFx.Mediator.Transports.Configuration;

/// <summary>
/// Configuration options for distributed messaging.
/// </summary>
public sealed class DistributedMessagingOptions
{
    /// <summary>
    /// Gets or sets the default transport name to use when not explicitly specified.
    /// </summary>
    public string? DefaultTransportName { get; set; }

    /// <summary>
    /// Gets or sets whether to enable distributed messaging by default.
    /// Default is true when EnableDistributedMessaging is called.
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Gets or sets whether to validate configuration on startup.
    /// Default is true.
    /// </summary>
    public bool ValidateOnStartup { get; set; } = true;
}
