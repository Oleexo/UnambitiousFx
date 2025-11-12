using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using UnambitiousFx.Mediator.Transports.Outbox;

namespace UnambitiousFx.Mediator.Transports.Observability;

/// <summary>
///     Health check for outbox backlog monitoring.
///     Reports degraded health when the outbox backlog exceeds the configured threshold.
/// </summary>
public sealed class OutboxHealthCheck : IHealthCheck
{
    private readonly IDistributedOutboxStorage _outboxStorage;
    private readonly OutboxHealthCheckOptions _options;

    /// <summary>
    ///     Initializes a new instance of the <see cref="OutboxHealthCheck" /> class.
    /// </summary>
    /// <param name="outboxStorage">The outbox storage to check.</param>
    /// <param name="options">The health check options.</param>
    public OutboxHealthCheck(
        IDistributedOutboxStorage outboxStorage,
        IOptions<OutboxHealthCheckOptions> options)
    {
        _outboxStorage = outboxStorage;
        _options = options.Value;
    }

    /// <inheritdoc />
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Get pending entries to count backlog
            var pendingEntries = await _outboxStorage.GetPendingAsync(
                int.MaxValue,
                cancellationToken);

            var backlogSize = pendingEntries.Count;
            var threshold = _options.BacklogThreshold;

            var data = new Dictionary<string, object>
            {
                ["BacklogSize"] = backlogSize,
                ["Threshold"] = threshold
            };

            if (backlogSize < threshold)
                return HealthCheckResult.Healthy(
                    $"Outbox backlog is {backlogSize} (threshold: {threshold})",
                    data);

            return HealthCheckResult.Degraded(
                $"Outbox backlog is {backlogSize}, exceeding threshold of {threshold}",
                data: data);
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(
                "Failed to check outbox health",
                ex,
                new Dictionary<string, object>
                {
                    ["Error"] = ex.Message
                });
        }
    }
}

/// <summary>
///     Configuration options for the outbox health check.
/// </summary>
public sealed class OutboxHealthCheckOptions
{
    /// <summary>
    ///     Gets or sets the backlog threshold for reporting degraded health.
    ///     Default is 1000 messages.
    /// </summary>
    public int BacklogThreshold { get; set; } = 1000;
}
