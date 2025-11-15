using Microsoft.Extensions.Diagnostics.HealthChecks;
using UnambitiousFx.Mediator.Transports.Abstractions;

namespace UnambitiousFx.Mediator.Transports.Observability;

/// <summary>
///     Health check for transport provider connectivity.
///     Checks all registered transport providers and reports their health status.
/// </summary>
public sealed class TransportHealthCheck : IHealthCheck
{
    private readonly IEnumerable<ITransportProvider> _transportProviders;

    /// <summary>
    ///     Initializes a new instance of the <see cref="TransportHealthCheck" /> class.
    /// </summary>
    /// <param name="transportProviders">The collection of transport providers to check.</param>
    public TransportHealthCheck(IEnumerable<ITransportProvider> transportProviders)
    {
        _transportProviders = transportProviders;
    }

    /// <inheritdoc />
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        var providers = _transportProviders.ToList();

        if (providers.Count == 0)
            return HealthCheckResult.Healthy("No transport providers registered");

        var results = new Dictionary<string, object>();
        var healthyCount = 0;
        var unhealthyCount = 0;

        foreach (var provider in providers)
        {
            var providerName = provider.ProviderType.ToString();

            try
            {
                var isHealthy = await provider.CheckHealthAsync(cancellationToken);

                if (isHealthy)
                {
                    healthyCount++;
                    results[providerName] = "Healthy";
                }
                else
                {
                    unhealthyCount++;
                    results[providerName] = "Unhealthy";
                }
            }
            catch (Exception ex)
            {
                unhealthyCount++;
                results[providerName] = $"Unhealthy: {ex.Message}";
            }
        }

        results["HealthyCount"] = healthyCount;
        results["UnhealthyCount"] = unhealthyCount;
        results["TotalCount"] = providers.Count;

        // Determine overall health status
        if (unhealthyCount == 0)
            return HealthCheckResult.Healthy(
                $"All {providers.Count} transport provider(s) are healthy",
                results);

        if (healthyCount == 0)
            return HealthCheckResult.Unhealthy(
                $"All {providers.Count} transport provider(s) are unhealthy",
                data: results);

        return HealthCheckResult.Degraded(
            $"{healthyCount} of {providers.Count} transport provider(s) are healthy",
            data: results);
    }
}
