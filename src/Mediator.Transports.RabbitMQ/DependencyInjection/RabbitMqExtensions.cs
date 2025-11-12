using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using UnambitiousFx.Mediator.Transports.Abstractions;

namespace UnambitiousFx.Mediator.Transports.RabbitMQ.DependencyInjection;

/// <summary>
///     Extension methods for configuring RabbitMQ transport.
/// </summary>
public static class RabbitMqExtensions
{
    /// <summary>
    ///     Adds RabbitMQ transport to the distributed messaging builder.
    /// </summary>
    /// <param name="builder">The distributed messaging builder.</param>
    /// <param name="configure">Optional action to configure RabbitMQ options.</param>
    /// <returns>The builder for fluent chaining.</returns>
    public static IDistributedMessagingBuilder AddRabbitMq(
        this IDistributedMessagingBuilder builder,
        Action<RabbitMqOptions>? configure = null)
    {
        var options = new RabbitMqOptions();
        configure?.Invoke(options);

        // Register options
        builder.Services.TryAddSingleton(options);

        // Register ConnectionFactory
        builder.Services.TryAddSingleton<IConnectionFactory>(sp =>
        {
            var opts = sp.GetRequiredService<RabbitMqOptions>();
            return new ConnectionFactory
            {
                HostName = opts.HostName,
                Port = opts.Port,
                UserName = opts.UserName,
                Password = opts.Password,
                VirtualHost = opts.VirtualHost,
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10),
                RequestedHeartbeat = TimeSpan.FromSeconds(60)
            };
        });

        // Register RabbitMQ transport
        builder.Services.TryAddSingleton<RabbitMqTransport>(sp =>
        {
            var connectionFactory = sp.GetRequiredService<IConnectionFactory>();
            var serializer = sp.GetRequiredService<IMessageSerializer>();
            var opts = sp.GetRequiredService<RabbitMqOptions>();
            var logger = sp.GetRequiredService<ILogger<RabbitMqTransport>>();

            return new RabbitMqTransport(connectionFactory, serializer, opts, logger);
        });

        // Register as IMessageTransport with optional name
        var transportName = options.TransportName ?? "rabbitmq";
        builder.AddTransport<RabbitMqTransport>(transportName);

        return builder;
    }
}

/// <summary>
///     Marker interface to enable extension method discovery.
///     This interface should match the actual IDistributedMessagingBuilder from Mediator.Transports.
/// </summary>
public interface IDistributedMessagingBuilder
{
    /// <summary>
    ///     Gets the service collection for registering additional services.
    /// </summary>
    IServiceCollection Services { get; }

    /// <summary>
    ///     Adds a built-in transport implementation.
    /// </summary>
    /// <typeparam name="TTransport">The transport type implementing IMessageTransport.</typeparam>
    /// <param name="name">Optional name for the transport.</param>
    /// <returns>The builder for fluent chaining.</returns>
    IDistributedMessagingBuilder AddTransport<TTransport>(string? name = null)
        where TTransport : class, IMessageTransport;
}
