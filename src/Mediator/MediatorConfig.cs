using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using UnambitiousFx.Mediator.Abstractions;
using UnambitiousFx.Mediator.Orchestrators;
using UnambitiousFx.Mediator.Pipelines;
using UnambitiousFx.Mediator.Transports.Abstractions;
using UnambitiousFx.Mediator.Transports.Configuration;
using UnambitiousFx.Mediator.Transports.Core;
using UnambitiousFx.Mediator.Transports.Pipeline;
using UnambitiousFx.Mediator.Transports.Security;
using UnambitiousFx.Mediator.Transports.Serialization;

namespace UnambitiousFx.Mediator;

internal sealed class MediatorConfig(IServiceCollection services) : IMediatorConfig
{
    private readonly List<Action<IServiceCollection, ServiceLifetime>> _actions = new();
    private readonly Dictionary<Type, DispatchEventDelegate> _eventDispatchers = new();
    private DefaultDependencyInjectionBuilder _builder = new();

    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
    private Type _contextFactory = typeof(DefaultContextFactory);

    private PublishMode _defaultPublisherMode = PublishMode.Now;

    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
    private Type _eventOrchestrator = typeof(SequentialEventOrchestrator);

    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
    private Type _eventOutBoxStorage = typeof(InMemoryEventOutboxStorage);

    private ServiceLifetime _lifetime = ServiceLifetime.Scoped;
    private Action<OutboxOptions> _outboxConfigure = _ => { };

    public IMediatorConfig SetLifetime(ServiceLifetime lifetime)
    {
        _lifetime = lifetime;
        return this;
    }

    public IMediatorConfig RegisterRequestPipelineBehavior<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        TRequestPipelineBehavior>()
        where TRequestPipelineBehavior : class, IRequestPipelineBehavior
    {
        _actions.Add((svc,
            lifetime) => svc.RegisterRequestPipelineBehavior<TRequestPipelineBehavior>(lifetime));
        return this;
    }

    public IMediatorConfig RegisterRequestPipelineBehavior<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        TBehavior, TRequest>()
        where TBehavior : class, IRequestPipelineBehavior<TRequest>
        where TRequest : IRequest
    {
        _actions.Add((scv,
            lifetime) => scv.RegisterTypedRequestPipelineBehavior<TBehavior, TRequest>(lifetime));
        return this;
    }

    public IMediatorConfig RegisterRequestPipelineBehavior<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        TBehavior, TRequest,
        TResponse>()
        where TBehavior : class, IRequestPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : notnull
    {
        _actions.Add((scv,
            lifetime) => scv.RegisterTypedRequestPipelineBehavior<TBehavior, TRequest, TResponse>(lifetime));
        return this;
    }

    public IMediatorConfig RegisterConditionalRequestPipelineBehavior<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        TBehavior>(
        Func<object, bool> predicate)
        where TBehavior : class, IRequestPipelineBehavior
    {
        _actions.Add((scv,
            lifetime) => scv.RegisterConditionalRequestPipelineBehavior<TBehavior>(predicate, lifetime));
        return this;
    }

    public IMediatorConfig RegisterConditionalRequestPipelineBehavior<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        TBehavior, TRequest>(
        Func<TRequest, bool> predicate)
        where TBehavior : class, IRequestPipelineBehavior<TRequest>
        where TRequest : IRequest
    {
        _actions.Add((scv,
                lifetime) =>
            scv.RegisterConditionalTypedRequestPipelineBehavior<TBehavior, TRequest>(predicate, lifetime));
        return this;
    }

    public IMediatorConfig RegisterConditionalRequestPipelineBehavior<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        TBehavior, TRequest,
        TResponse>(Func<TRequest, bool> predicate)
        where TBehavior : class, IRequestPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : notnull
    {
        _actions.Add((scv,
                lifetime) =>
            scv.RegisterConditionalTypedRequestPipelineBehavior<TBehavior, TRequest, TResponse>(predicate,
                lifetime));
        return this;
    }

    public IMediatorConfig RegisterEventPipelineBehavior<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        TEventPipelineBehavior>()
        where TEventPipelineBehavior : class, IEventPipelineBehavior
    {
        _actions.Add((scv,
            lifetime) => scv.RegisterEventPipelineBehavior<TEventPipelineBehavior>(lifetime));
        return this;
    }

    public IMediatorConfig SetEventOrchestrator<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        TEventOrchestrator>()
        where TEventOrchestrator : class, IEventOrchestrator
    {
        _eventOrchestrator = typeof(TEventOrchestrator);
        return this;
    }

    public IMediatorConfig AddRegisterGroup(IRegisterGroup group)
    {
        _builder = new DefaultDependencyInjectionBuilder();
        group.Register(_builder);
        return this;
    }

    public IMediatorConfig RegisterRequestHandler<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        THandler, TRequest, TResponse>()
        where TResponse : notnull
        where TRequest : IRequest<TResponse>
        where THandler : class, IRequestHandler<TRequest, TResponse>
    {
        _actions.Add((scv,
            lifetime) => scv.RegisterRequestHandler<THandler, TRequest, TResponse>(lifetime));
        return this;
    }

    public IMediatorConfig RegisterRequestHandler<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        THandler, TRequest>()
        where TRequest : IRequest
        where THandler : class, IRequestHandler<TRequest>
    {
        _actions.Add((scv,
            lifetime) => scv.RegisterRequestHandler<THandler, TRequest>(lifetime));
        return this;
    }

    public IMediatorConfig RegisterEventHandler<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        THandler, TEvent>()
        where THandler : class, IEventHandler<TEvent>
        where TEvent : class, IEvent
    {
        _actions.Add((scv,
            lifetime) => scv.RegisterEventHandler<THandler, TEvent>(lifetime));
        // Ensure only one dispatcher per event type; multiple handler registrations should not create duplicate dictionary entries
        _eventDispatchers.TryAdd(typeof(TEvent), (@event,
            dispatcher,
            distributionMode,
            cancellationToken) => dispatcher.DispatchAsync(Unsafe.As<TEvent>(@event), cancellationToken));
        return this;
    }

    public IMediatorConfig SetEventOutboxStorage<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        TEventOutboxStorage>()
        where TEventOutboxStorage : class, IEventOutboxStorage
    {
        _eventOutBoxStorage = typeof(TEventOutboxStorage);
        return this;
    }

    public IMediatorConfig SetDefaultPublishingMode(PublishMode mode)
    {
        _defaultPublisherMode = mode;
        return this;
    }

    public IMediatorConfig ConfigureOutbox(Action<OutboxOptions> configure)
    {
        _outboxConfigure = configure;
        return this;
    }

    public IMediatorConfig EnableCqrsBoundaryEnforcement(bool enable = true)
    {
        if (!enable) return this;

        return RegisterRequestPipelineBehavior<CqrsBoundaryEnforcementBehavior>();
    }

    public IMediatorConfig AddValidator<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        TValidator, TRequest>()
        where TValidator : class, IRequestValidator<TRequest>
        where TRequest : IRequest
    {
        _actions.Add((scv,
            _) => scv.AddScoped<IRequestValidator<TRequest>, TValidator>());
        return this;
    }

    public IMediatorConfig UseDefaultContextFactory()
    {
        return UseContextFactory<DefaultContextFactory>();
    }

    public IMediatorConfig UseSlimContextFactory()
    {
        return UseContextFactory<SlimContextFactory>();
    }

    public IMediatorConfig UseContextFactory<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        TContextFactory>()
        where TContextFactory : class, IContextFactory
    {
        _contextFactory = typeof(TContextFactory);
        return this;
    }

    public IMediatorConfig EnableDistributedMessaging(Action<IDistributedMessagingBuilder>? configure = null)
    {
        // Register core transport services

        // Register NoopTransport as the default transport
        services.TryAddSingleton<IMessageTransport, NoopTransport>();

        // Register BuiltInTransportProvider as the default provider
        services.TryAddSingleton<ITransportProvider, BuiltInTransportProvider>();

        // Register MessageTraitsRegistry as singleton
        var traitsRegistry = new MessageTraitsRegistry();
        services.TryAddSingleton<IMessageTraitsRegistry>(traitsRegistry);

        // Register MessageTypeRegistry as singleton for NativeAOT compatibility
        var typeRegistry = new MessageTypeRegistry();
        services.TryAddSingleton<IMessageTypeRegistry>(typeRegistry);

        // Register SensitiveDataRegistry as singleton for NativeAOT-compatible field encryption
        var sensitiveDataRegistry = new SensitiveDataRegistry();
        services.TryAddSingleton<ISensitiveDataRegistry>(sensitiveDataRegistry);

        // Register EnvelopeBuilder as scoped
        services.TryAddScoped<IEnvelopeBuilder, EnvelopeBuilder>();

        // Register JsonMessageSerializer as singleton
        services.TryAddSingleton<IMessageSerializer, JsonMessageSerializer>();

        // Register TransportDispatcher as scoped
        services.TryAddScoped<ITransportDispatcher, TransportDispatcher>();

        // MIGRATION NOTE: DistributedEventDispatchBehavior has been removed
        // The functionality has been integrated into the unified EventDispatcher and OutboxManager.
        // Distribution mode routing is now handled automatically by EventDispatcher based on:
        //   1. IEventRoutingFilter implementations (evaluated first)
        //   2. MessageTraits configuration (fallback)
        //   3. EventDispatcherOptions.DefaultDistributionMode (default)
        //
        // To migrate:
        //   - Remove any manual registration of DistributedEventDispatchBehavior
        //   - Use RegisterEventRoutingFilter<T>() for custom routing logic
        //   - Configure distribution modes via MessageTraits or EventDispatcherOptions
        //
        // The DistributedEventDispatchBehavior class is marked as obsolete and will be removed in a future version.

        // Configure options
        services.Configure<DistributedMessagingOptions>(options => { options.Enabled = true; });

        // Create and configure the builder
        var builder = new DistributedMessagingBuilder(services, traitsRegistry, typeRegistry, sensitiveDataRegistry);

        // Allow user to configure the builder
        configure?.Invoke(builder);

        return this;
    }

    public IMediatorConfig RegisterEventRoutingFilter<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        TEventRoutingFilter>()
        where TEventRoutingFilter : class, IEventRoutingFilter
    {
        _actions.Add((scv, lifetime) => scv.AddScoped<IEventRoutingFilter, TEventRoutingFilter>());
        return this;
    }

    public IMediatorConfig UseEventDispatcherRegistration<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        TRegistration>()
        where TRegistration : class, IEventDispatcherRegistration, new()
    {
        var registration = new TRegistration();
        registration.RegisterDispatchers((type, del) =>
        {
            if (del is DispatchEventDelegate dispatchDelegate)
            {
                _eventDispatchers[type] = dispatchDelegate;
            }
        });
        return this;
    }

    public void Apply()
    {
        _builder.Apply(services, _lifetime);

        foreach (var action in _actions) action(services, _lifetime);

        services.AddScoped(typeof(IContextFactory), _contextFactory);
        services.AddScoped(typeof(IEventOrchestrator), _eventOrchestrator);
        services.AddScoped(typeof(IEventOutboxStorage), _eventOutBoxStorage);

        services.Configure<PublisherOptions>(options => { options.DefaultMode = _defaultPublisherMode; });
        services.Configure<EventDispatcherOptions>(options =>
        {
            // Set default values (already set in EventDispatcherOptions class, but can be overridden here)
            options.DefaultDistributionMode = DistributionMode.LocalOnly;
            options.DispatchStrategy = DispatchStrategy.Immediate;
            options.BatchSize = 100;
            options.BatchFlushInterval = TimeSpan.FromSeconds(5);
            
            // Register event dispatchers from manual registrations
            options.Dispatchers = _eventDispatchers;
        });
        services.Configure<OutboxOptions>(options => { _outboxConfigure(options); });
    }
}
