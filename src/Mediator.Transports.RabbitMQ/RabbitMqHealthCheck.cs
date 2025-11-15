using Microsoft.Extensions.Diagnostics.HealthChecks;
using RabbitMQ.Client;

namespace UnambitiousFx.Mediator.Transports.RabbitMQ;

/// <summary>
///     Health check for RabbitMQ transport connectivity.
/// </summary>
public sealed class RabbitMqHealthCheck : IHealthCheck
{
    private readonly IConnectionFactory _connectionFactory;
    private readonly RabbitMqOptions _options;

    /// <summary>
    ///     Initializes a new instance of the <see cref="RabbitMqHealthCheck" /> class.
    /// </summary>
    /// <param name="connectionFactory">The RabbitMQ connection factory.</param>
    /// <param name="options">The RabbitMQ configuration options.</param>
    public RabbitMqHealthCheck(IConnectionFactory connectionFactory, RabbitMqOptions options)
    {
        _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    /// <inheritdoc />
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Attempt to create a connection to verify connectivity
            await using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

            if (connection.IsOpen)
            {
                // Optionally create a channel to verify full connectivity
                await using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

                if (channel.IsOpen)
                    return HealthCheckResult.Healthy(
                        "RabbitMQ connection is healthy",
                        new Dictionary<string, object>
                        {
                            ["host"] = _options.HostName,
                            ["port"] = _options.Port,
                            ["virtualHost"] = _options.VirtualHost
                        });
            }

            return HealthCheckResult.Unhealthy(
                "RabbitMQ connection or channel is not open",
                data: new Dictionary<string, object>
                {
                    ["host"] = _options.HostName,
                    ["port"] = _options.Port
                });
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(
                "Failed to connect to RabbitMQ",
                ex,
                new Dictionary<string, object>
                {
                    ["host"] = _options.HostName,
                    ["port"] = _options.Port,
                    ["error"] = ex.Message
                });
        }
    }
}
