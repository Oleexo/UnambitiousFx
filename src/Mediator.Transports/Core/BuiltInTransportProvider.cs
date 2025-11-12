using Microsoft.Extensions.DependencyInjection;
using UnambitiousFx.Mediator.Transports.Abstractions;

namespace UnambitiousFx.Mediator.Transports.Core;

/// <summary>
///     Built-in transport provider that delegates to registered IMessageTransport implementations.
/// </summary>
public sealed class BuiltInTransportProvider : ITransportProvider
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    ///     Initializes a new instance of the <see cref="BuiltInTransportProvider" /> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider for resolving transport implementations.</param>
    public BuiltInTransportProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <inheritdoc />
    public TransportProviderType ProviderType => TransportProviderType.BuiltIn;

    /// <inheritdoc />
    public async ValueTask PublishAsync(
        MessageEnvelope envelope,
        MessageTraits traits,
        CancellationToken cancellationToken = default)
    {
        var transport = ResolveTransport(traits.TransportName);
        await transport.PublishAsync(envelope, cancellationToken);
    }

    /// <inheritdoc />
    public async ValueTask SubscribeAsync(
        Type messageType,
        MessageTraits traits,
        Func<MessageEnvelope, CancellationToken, ValueTask> handler,
        CancellationToken cancellationToken = default)
    {
        var transport = ResolveTransport(traits.TransportName);
        var descriptor = new SubscriptionDescriptor
        {
            MessageType = messageType.Name,
            Topic = traits.Topic ?? messageType.Name,
            Filter = null,
            MaxConcurrency = traits.MaxConcurrency,
            RetryPolicy = traits.RetryPolicy,
            Handler = handler
        };

        await transport.SubscribeAsync(descriptor, cancellationToken);
    }

    /// <inheritdoc />
    public async ValueTask UnsubscribeAsync(
        Type messageType,
        CancellationToken cancellationToken = default)
    {
        // For unsubscribe, we need to resolve all transports and unsubscribe from each
        // This is a simplified implementation - in production, we'd track which transport was used
        var transports = _serviceProvider.GetServices<IMessageTransport>();
        foreach (var transport in transports)
        {
            var descriptor = new SubscriptionDescriptor
            {
                MessageType = messageType.Name,
                Topic = messageType.Name,
                Handler = (_, _) => ValueTask.CompletedTask
            };

            await transport.UnsubscribeAsync(descriptor, cancellationToken);
        }
    }

    /// <inheritdoc />
    public async ValueTask<bool> CheckHealthAsync(CancellationToken cancellationToken = default)
    {
        // Check health of all registered transports
        var transports = _serviceProvider.GetServices<IMessageTransport>();
        var allHealthy = true;

        foreach (var transport in transports)
        {
            try
            {
                // For now, we assume transports are healthy if they can be resolved
                // Individual transport implementations can provide more sophisticated health checks
                allHealthy &= transport != null;
            }
            catch
            {
                allHealthy = false;
            }
        }

        return await ValueTask.FromResult(allHealthy);
    }

    private IMessageTransport ResolveTransport(string? transportName)
    {
        if (string.IsNullOrEmpty(transportName))
        {
            // Return the first registered transport (typically noop)
            var transport = _serviceProvider.GetService<IMessageTransport>();
            if (transport == null)
            {
                throw new InvalidOperationException(
                    "No message transport is registered. Register at least one transport implementation.");
            }

            return transport;
        }

        // Try to resolve named transport using keyed services
        var namedTransport = _serviceProvider.GetKeyedService<IMessageTransport>(transportName);
        if (namedTransport != null)
        {
            return namedTransport;
        }

        // Fall back to searching all transports by name
        var transports = _serviceProvider.GetServices<IMessageTransport>();
        var matchingTransport = transports.FirstOrDefault(t => t.Name == transportName);

        if (matchingTransport == null)
        {
            throw new InvalidOperationException(
                $"Transport '{transportName}' is not registered. Available transports: {string.Join(", ", transports.Select(t => t.Name))}");
        }

        return matchingTransport;
    }
}
