using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using UnambitiousFx.Mediator.Transports.Abstractions;
using UnambitiousFx.Mediator.Transports.Inbox;
using UnambitiousFx.Mediator.Transports.Outbox;

namespace UnambitiousFx.Mediator.Transports.Configuration;

/// <summary>
///     Builder interface for configuring distributed messaging.
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
    /// <param name="name">Optional name for the transport. If not provided, uses the transport's Name property.</param>
    /// <returns>The builder for fluent chaining.</returns>
    IDistributedMessagingBuilder AddTransport<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        TTransport>(string? name = null)
        where TTransport : class, IMessageTransport;

    /// <summary>
    ///     Configures message traits for a specific message type.
    /// </summary>
    /// <typeparam name="TMessage">The message type to configure.</typeparam>
    /// <returns>A message traits builder for fluent configuration.</returns>
    IMessageTraitsBuilder<TMessage> ForMessage<TMessage>() where TMessage : class;

    /// <summary>
    ///     Configures the outbox storage implementation.
    /// </summary>
    /// <typeparam name="TOutboxStorage">The outbox storage type implementing IDistributedOutboxStorage.</typeparam>
    /// <returns>The builder for fluent chaining.</returns>
    IDistributedMessagingBuilder UseOutbox<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        TOutboxStorage>()
        where TOutboxStorage : class, IDistributedOutboxStorage;

    /// <summary>
    ///     Configures the inbox storage implementation for deduplication.
    /// </summary>
    /// <typeparam name="TInboxStorage">The inbox storage type implementing IInboxStorage.</typeparam>
    /// <returns>The builder for fluent chaining.</returns>
    IDistributedMessagingBuilder UseInbox<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        TInboxStorage>()
        where TInboxStorage : class, IInboxStorage;


    /// <summary>
    ///     Replaces the default transport provider with a custom implementation.
    /// </summary>
    /// <typeparam name="TProvider">The custom provider type implementing ITransportProvider.</typeparam>
    /// <returns>The builder for fluent chaining.</returns>
    IDistributedMessagingBuilder UseCustomProvider<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        TProvider>()
        where TProvider : class, ITransportProvider;
}
