namespace UnambitiousFx.Mediator.Transports.Outbox;

/// <summary>
///     Provides configuration options for the distributed outbox background service.
/// </summary>
public sealed class OutboxOptions
{
    /// <summary>
    ///     Gets or sets the maximum number of outbox entries to process in a single batch.
    ///     Default is 100.
    /// </summary>
    public int? BatchSize { get; set; } = 100;

    /// <summary>
    ///     Gets or sets the interval between outbox polling operations.
    ///     Default is 5 seconds.
    /// </summary>
    public TimeSpan PollingInterval { get; set; } = TimeSpan.FromSeconds(5);
}
