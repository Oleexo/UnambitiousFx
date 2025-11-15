using Amazon;
using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using Amazon.SQS;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using UnambitiousFx.Mediator.Transports.Abstractions;

namespace UnambitiousFx.Mediator.Transports.AwsSns.DependencyInjection;

/// <summary>
///     Extension methods for configuring AWS SNS/SQS transport.
/// </summary>
public static class AwsSnsExtensions
{
    /// <summary>
    ///     Adds AWS SNS/SQS transport to the distributed messaging builder.
    /// </summary>
    /// <param name="builder">The distributed messaging builder.</param>
    /// <param name="configure">Optional action to configure AWS SNS/SQS options.</param>
    /// <returns>The builder for fluent chaining.</returns>
    public static IDistributedMessagingBuilder AddAwsSns(
        this IDistributedMessagingBuilder builder,
        Action<AwsSnsOptions>? configure = null)
    {
        var options = new AwsSnsOptions();
        configure?.Invoke(options);

        // Register options
        builder.Services.TryAddSingleton(options);

        // Register AWS credentials
        AWSCredentials? credentials = null;
        if (!string.IsNullOrEmpty(options.AccessKeyId) && !string.IsNullOrEmpty(options.SecretAccessKey))
        {
            credentials = new BasicAWSCredentials(options.AccessKeyId, options.SecretAccessKey);
        }

        // Register SNS client
        builder.Services.TryAddSingleton<IAmazonSimpleNotificationService>(sp =>
        {
            var opts = sp.GetRequiredService<AwsSnsOptions>();
            var region = RegionEndpoint.GetBySystemName(opts.Region);

            if (credentials != null)
            {
                return new AmazonSimpleNotificationServiceClient(credentials, region);
            }

            // Use default credential chain (environment variables, IAM role, etc.)
            return new AmazonSimpleNotificationServiceClient(region);
        });

        // Register SQS client
        builder.Services.TryAddSingleton<IAmazonSQS>(sp =>
        {
            var opts = sp.GetRequiredService<AwsSnsOptions>();
            var region = RegionEndpoint.GetBySystemName(opts.Region);

            if (credentials != null)
            {
                return new AmazonSQSClient(credentials, region);
            }

            // Use default credential chain (environment variables, IAM role, etc.)
            return new AmazonSQSClient(region);
        });

        // Register AWS SNS/SQS transport
        builder.Services.TryAddSingleton<AwsSnsTransport>(sp =>
        {
            var snsClient = sp.GetRequiredService<IAmazonSimpleNotificationService>();
            var sqsClient = sp.GetRequiredService<IAmazonSQS>();
            var serializer = sp.GetRequiredService<IMessageSerializer>();
            var opts = sp.GetRequiredService<AwsSnsOptions>();
            var logger = sp.GetRequiredService<ILogger<AwsSnsTransport>>();

            return new AwsSnsTransport(snsClient, sqsClient, serializer, opts, logger);
        });

        // Register as IMessageTransport with optional name
        var transportName = options.TransportName ?? "awssns";
        builder.AddTransport<AwsSnsTransport>(transportName);

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
