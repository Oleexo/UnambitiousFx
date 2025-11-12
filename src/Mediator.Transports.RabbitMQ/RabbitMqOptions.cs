namespace UnambitiousFx.Mediator.Transports.RabbitMQ;

/// <summary>
///     Configuration options for RabbitMQ transport.
/// </summary>
public sealed class RabbitMqOptions
{
    /// <summary>
    ///     Gets or sets the RabbitMQ host name.
    /// </summary>
    public string HostName { get; set; } = "localhost";

    /// <summary>
    ///     Gets or sets the RabbitMQ port.
    /// </summary>
    public int Port { get; set; } = 5672;

    /// <summary>
    ///     Gets or sets the username for authentication.
    /// </summary>
    public string UserName { get; set; } = "guest";

    /// <summary>
    ///     Gets or sets the password for authentication.
    /// </summary>
    public string Password { get; set; } = "guest";

    /// <summary>
    ///     Gets or sets the virtual host.
    /// </summary>
    public string VirtualHost { get; set; } = "/";

    /// <summary>
    ///     Gets or sets the exchange name for publishing messages.
    /// </summary>
    public string ExchangeName { get; set; } = "mediator-exchange";

    /// <summary>
    ///     Gets or sets the exchange type (e.g., "topic", "direct", "fanout").
    /// </summary>
    public string ExchangeType { get; set; } = "topic";

    /// <summary>
    ///     Gets or sets the optional transport name. If null, defaults to "rabbitmq".
    /// </summary>
    public string? TransportName { get; set; }

    /// <summary>
    ///     Gets or sets whether the exchange should be durable.
    /// </summary>
    public bool DurableExchange { get; set; } = true;

    /// <summary>
    ///     Gets or sets whether queues should be durable.
    /// </summary>
    public bool DurableQueues { get; set; } = true;

    /// <summary>
    ///     Gets or sets whether to auto-delete queues when no consumers are connected.
    /// </summary>
    public bool AutoDeleteQueues { get; set; } = false;
}
