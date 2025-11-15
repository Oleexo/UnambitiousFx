using UnambitiousFx.Mediator.Transports.Abstractions;

namespace UnambitiousFx.Mediator.Transports.Configuration;

/// <summary>
/// Builder interface for configuring message traits for a specific message type.
/// </summary>
/// <typeparam name="TMessage">The message type being configured.</typeparam>
public interface IMessageTraitsBuilder<TMessage> where TMessage : class
{
    /// <summary>
    /// Configures the message to use local-only distribution (no external transport).
    /// </summary>
    /// <returns>The builder for fluent chaining.</returns>
    IMessageTraitsBuilder<TMessage> UseLocalOnly();

    /// <summary>
    /// Configures the message to use hybrid mode (both local handlers and external transport).
    /// </summary>
    /// <returns>The builder for fluent chaining.</returns>
    IMessageTraitsBuilder<TMessage> UseHybridMode();

    /// <summary>
    /// Configures the message to route externally only (skip local handlers).
    /// </summary>
    /// <returns>The builder for fluent chaining.</returns>
    IMessageTraitsBuilder<TMessage> RouteExternally();

    /// <summary>
    /// Specifies the transport to use for this message.
    /// </summary>
    /// <param name="transportName">The name of the transport.</param>
    /// <returns>The builder for fluent chaining.</returns>
    IMessageTraitsBuilder<TMessage> WithTransport(string transportName);

    /// <summary>
    /// Specifies the topic/queue name for this message.
    /// </summary>
    /// <param name="topic">The topic or queue name.</param>
    /// <returns>The builder for fluent chaining.</returns>
    IMessageTraitsBuilder<TMessage> ToTopic(string topic);

    /// <summary>
    /// Specifies the partition key for this message (for partitioned transports).
    /// </summary>
    /// <param name="partitionKey">The partition key.</param>
    /// <returns>The builder for fluent chaining.</returns>
    IMessageTraitsBuilder<TMessage> WithPartitionKey(string partitionKey);

    /// <summary>
    /// Configures the message to fail fast on transport errors (propagate exceptions).
    /// </summary>
    /// <returns>The builder for fluent chaining.</returns>
    IMessageTraitsBuilder<TMessage> FailFastOnTransportErrors();

    /// <summary>
    /// Configures the message to use the outbox pattern for reliable delivery.
    /// </summary>
    /// <returns>The builder for fluent chaining.</returns>
    IMessageTraitsBuilder<TMessage> UseOutbox();

    /// <summary>
    /// Configures the retry policy for this message.
    /// </summary>
    /// <param name="policy">The retry policy.</param>
    /// <returns>The builder for fluent chaining.</returns>
    IMessageTraitsBuilder<TMessage> WithRetryPolicy(RetryPolicy policy);

    /// <summary>
    /// Configures the maximum concurrency for processing this message.
    /// </summary>
    /// <param name="maxConcurrency">The maximum number of concurrent message processors.</param>
    /// <returns>The builder for fluent chaining.</returns>
    IMessageTraitsBuilder<TMessage> WithMaxConcurrency(int maxConcurrency);

    /// <summary>
    /// Configures a sensitive field on the message for encryption.
    /// </summary>
    /// <param name="fieldName">The name of the field.</param>
    /// <param name="getter">The delegate to get the field value.</param>
    /// <param name="setter">The delegate to set the field value.</param>
    /// <param name="level">The sensitivity level (default: Confidential).</param>
    /// <param name="requireEncryption">Whether encryption is required (default: true).</param>
    /// <returns>The builder for fluent chaining.</returns>
    IMessageTraitsBuilder<TMessage> WithSensitiveField(
        string fieldName,
        Func<TMessage, string?> getter,
        Action<TMessage, string?> setter,
        Security.SensitivityLevel level = Security.SensitivityLevel.Confidential,
        bool requireEncryption = true);

    /// <summary>
    /// Configures multiple sensitive fields on the message for encryption.
    /// </summary>
    /// <param name="configure">Action to configure sensitive fields.</param>
    /// <returns>The builder for fluent chaining.</returns>
    IMessageTraitsBuilder<TMessage> WithSensitiveFields(
        Action<ISensitiveFieldsBuilder<TMessage>> configure);
}

/// <summary>
/// Builder interface for configuring sensitive fields on a message type.
/// </summary>
/// <typeparam name="TMessage">The message type being configured.</typeparam>
public interface ISensitiveFieldsBuilder<TMessage> where TMessage : class
{
    /// <summary>
    /// Adds a sensitive field configuration.
    /// </summary>
    /// <param name="fieldName">The name of the field.</param>
    /// <param name="getter">The delegate to get the field value.</param>
    /// <param name="setter">The delegate to set the field value.</param>
    /// <param name="level">The sensitivity level (default: Confidential).</param>
    /// <param name="requireEncryption">Whether encryption is required (default: true).</param>
    /// <returns>The builder for fluent chaining.</returns>
    ISensitiveFieldsBuilder<TMessage> Add(
        string fieldName,
        Func<TMessage, string?> getter,
        Action<TMessage, string?> setter,
        Security.SensitivityLevel level = Security.SensitivityLevel.Confidential,
        bool requireEncryption = true);
}
