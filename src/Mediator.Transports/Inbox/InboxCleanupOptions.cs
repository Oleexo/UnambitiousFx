namespace UnambitiousFx.Mediator.Transports.Inbox;

/// <summary>
///     Configuration options for inbox cleanup service.
/// </summary>
public sealed class InboxCleanupOptions
{
    /// <summary>
    ///     Gets or sets the interval between cleanup operations.
    /// </summary>
    /// <remarks>
    ///     Default is 1 hour.
    /// </remarks>
    public TimeSpan CleanupInterval { get; set; } = TimeSpan.FromHours(1);

    /// <summary>
    ///     Gets or sets the retention period for inbox entries.
    /// </summary>
    /// <remarks>
    ///     Default is 7 days. Entries older than this will be cleaned up.
    /// </remarks>
    public TimeSpan RetentionPeriod { get; set; } = TimeSpan.FromDays(7);
}
