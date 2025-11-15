using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using UnambitiousFx.Mediator.Transports.Abstractions;
using UnambitiousFx.Mediator.Transports.Inbox;
using UnambitiousFx.Mediator.Transports.Outbox;
using UnambitiousFx.Mediator.Transports.Security;

namespace UnambitiousFx.Mediator.Transports.Configuration;

/// <summary>
///     Builder implementation for configuring distributed messaging.
/// </summary>
internal sealed class DistributedMessagingBuilder : IDistributedMessagingBuilder
{
    private readonly IMessageTraitsRegistry _traitsRegistry;
    private readonly IMessageTypeRegistry _typeRegistry;
    private readonly ISensitiveDataRegistry _sensitiveDataRegistry;

    public DistributedMessagingBuilder(
        IServiceCollection services,
        IMessageTraitsRegistry traitsRegistry,
        IMessageTypeRegistry typeRegistry,
        ISensitiveDataRegistry sensitiveDataRegistry)
    {
        Services = services;
        _traitsRegistry = traitsRegistry;
        _typeRegistry = typeRegistry;
        _sensitiveDataRegistry = sensitiveDataRegistry;
    }

    public IServiceCollection Services { get; }

    public IDistributedMessagingBuilder AddTransport<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        TTransport>(string? name = null)
        where TTransport : class, IMessageTransport
    {
        if (name != null)
            // Register as keyed service with the specified name
            Services.AddKeyedSingleton<IMessageTransport, TTransport>(name);
        else
            // Register as regular singleton
            Services.TryAddSingleton<IMessageTransport, TTransport>();

        return this;
    }

    public IMessageTraitsBuilder<TMessage> ForMessage<TMessage>() where TMessage : class
    {
        return new MessageTraitsBuilder<TMessage>(_traitsRegistry, _typeRegistry, _sensitiveDataRegistry);
    }

    public IDistributedMessagingBuilder UseOutbox<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        TOutboxStorage>()
        where TOutboxStorage : class, IDistributedOutboxStorage
    {
        Services.TryAddSingleton<IDistributedOutboxStorage, TOutboxStorage>();
        Services.AddHostedService<OutboxBackgroundService>();
        return this;
    }

    public IDistributedMessagingBuilder UseInbox<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        TInboxStorage>()
        where TInboxStorage : class, IInboxStorage
    {
        Services.TryAddSingleton<IInboxStorage, TInboxStorage>();
        Services.AddHostedService<InboxCleanupService>();
        return this;
    }

    public IDistributedMessagingBuilder UseCustomProvider<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        TProvider>()
        where TProvider : class, ITransportProvider
    {
        // Replace the default transport provider registration
        Services.RemoveAll<ITransportProvider>();
        Services.AddSingleton<ITransportProvider, TProvider>();
        return this;
    }
}
