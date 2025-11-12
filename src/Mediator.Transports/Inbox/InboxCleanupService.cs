using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace UnambitiousFx.Mediator.Transports.Inbox;

/// <summary>
///     Background service that periodically cleans up expired inbox entries.
/// </summary>
public sealed class InboxCleanupService : BackgroundService
{
    private readonly IInboxStorage _inboxStorage;
    private readonly InboxCleanupOptions _options;
    private readonly ILogger<InboxCleanupService> _logger;

    /// <summary>
    ///     Initializes a new instance of the <see cref="InboxCleanupService" /> class.
    /// </summary>
    /// <param name="inboxStorage">The inbox storage instance.</param>
    /// <param name="options">The cleanup options.</param>
    /// <param name="logger">The logger instance.</param>
    public InboxCleanupService(
        IInboxStorage inboxStorage,
        IOptions<InboxCleanupOptions> options,
        ILogger<InboxCleanupService> logger)
    {
        _inboxStorage = inboxStorage;
        _options = options.Value;
        _logger = logger;
    }

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "Inbox cleanup service started with interval {CleanupInterval}",
            _options.CleanupInterval);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(_options.CleanupInterval, stoppingToken);

                _logger.LogDebug("Starting inbox cleanup");

                await _inboxStorage.CleanupExpiredAsync(stoppingToken);

                _logger.LogDebug("Inbox cleanup completed");
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                // Expected during shutdown
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during inbox cleanup");
            }
        }

        _logger.LogInformation("Inbox cleanup service stopped");
    }
}
