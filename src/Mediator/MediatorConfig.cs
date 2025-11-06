using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using UnambitiousFx.Mediator.Abstractions;
using UnambitiousFx.Mediator.Orchestrators;
using UnambitiousFx.Mediator.Pipelines;

namespace UnambitiousFx.Mediator;

internal sealed class MediatorConfig : IMediatorConfig {
    private readonly List<Action<IServiceCollection, ServiceLifetime>> _actions          = new();
    private readonly Dictionary<Type, DispatchEventDelegate>           _eventDispatchers = new();
    private readonly IServiceCollection                                _services;
    private          DefaultDependencyInjectionBuilder                 _builder;
    private          PublishMode                                       _defaultPublisherMode;

    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
    private Type _eventOrchestrator;

    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
    private Type _eventOutBoxStorage;

    private ServiceLifetime       _lifetime;
    private Action<OutboxOptions> _outboxConfigure = _ => { };

    public MediatorConfig(IServiceCollection services) {
        _services             = services;
        _lifetime             = ServiceLifetime.Scoped;
        _eventOrchestrator    = typeof(SequentialEventOrchestrator);
        _builder              = new DefaultDependencyInjectionBuilder();
        _eventOutBoxStorage   = typeof(InMemoryEventOutboxStorage);
        _defaultPublisherMode = PublishMode.Now;
    }

    public IMediatorConfig SetLifetime(ServiceLifetime lifetime) {
        _lifetime = lifetime;
        return this;
    }

    public IMediatorConfig RegisterRequestPipelineBehavior<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TRequestPipelineBehavior>()
        where TRequestPipelineBehavior : class, IRequestPipelineBehavior {
        _actions.Add((services,
                      lifetime) => services.RegisterRequestPipelineBehavior<TRequestPipelineBehavior>(lifetime));
        return this;
    }

    public IMediatorConfig RegisterRequestPipelineBehavior<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TBehavior, TRequest>()
        where TBehavior : class, IRequestPipelineBehavior<TRequest>
        where TRequest : IRequest {
        _actions.Add((services,
                      lifetime) => services.RegisterTypedRequestPipelineBehavior<TBehavior, TRequest>(lifetime));
        return this;
    }

    public IMediatorConfig RegisterRequestPipelineBehavior<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TBehavior, TRequest, TResponse>()
        where TBehavior : class, IRequestPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : notnull {
        _actions.Add((services,
                      lifetime) => services.RegisterTypedRequestPipelineBehavior<TBehavior, TRequest, TResponse>(lifetime));
        return this;
    }

    public IMediatorConfig RegisterConditionalRequestPipelineBehavior<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TBehavior>(
        Func<object, bool> predicate)
        where TBehavior : class, IRequestPipelineBehavior {
        _actions.Add((services,
                      lifetime) => services.RegisterConditionalRequestPipelineBehavior<TBehavior>(predicate, lifetime));
        return this;
    }

    public IMediatorConfig RegisterConditionalRequestPipelineBehavior<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TBehavior, TRequest>(
        Func<TRequest, bool> predicate)
        where TBehavior : class, IRequestPipelineBehavior<TRequest>
        where TRequest : IRequest {
        _actions.Add((services,
                      lifetime) => services.RegisterConditionalTypedRequestPipelineBehavior<TBehavior, TRequest>(predicate, lifetime));
        return this;
    }

    public IMediatorConfig RegisterConditionalRequestPipelineBehavior<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TBehavior, TRequest,
                                                                      TResponse>(Func<TRequest, bool> predicate)
        where TBehavior : class, IRequestPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : notnull {
        _actions.Add((services,
                      lifetime) => services.RegisterConditionalTypedRequestPipelineBehavior<TBehavior, TRequest, TResponse>(predicate, lifetime));
        return this;
    }

    public IMediatorConfig RegisterEventPipelineBehavior<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TEventPipelineBehavior>()
        where TEventPipelineBehavior : class, IEventPipelineBehavior {
        _actions.Add((services,
                      lifetime) => services.RegisterEventPipelineBehavior<TEventPipelineBehavior>(lifetime));
        return this;
    }

    public IMediatorConfig SetEventOrchestrator<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TEventOrchestrator>()
        where TEventOrchestrator : class, IEventOrchestrator {
        _eventOrchestrator = typeof(TEventOrchestrator);
        return this;
    }

    public IMediatorConfig AddRegisterGroup(IRegisterGroup group) {
        _builder = new DefaultDependencyInjectionBuilder();
        group.Register(_builder);
        return this;
    }

    public IMediatorConfig RegisterRequestHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler, TRequest, TResponse>()
        where TResponse : notnull
        where TRequest : IRequest<TResponse>
        where THandler : class, IRequestHandler<TRequest, TResponse> {
        _actions.Add((services,
                      lifetime) => services.RegisterRequestHandler<THandler, TRequest, TResponse>(lifetime));
        return this;
    }

    public IMediatorConfig RegisterRequestHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler, TRequest>()
        where TRequest : IRequest
        where THandler : class, IRequestHandler<TRequest> {
        _actions.Add((services,
                      lifetime) => services.RegisterRequestHandler<THandler, TRequest>(lifetime));
        return this;
    }

    public IMediatorConfig RegisterEventHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler, TEvent>()
        where THandler : class, IEventHandler<TEvent>
        where TEvent : class, IEvent {
        _actions.Add((services,
                      lifetime) => services.RegisterEventHandler<THandler, TEvent>(lifetime));
        // Ensure only one dispatcher per event type; multiple handler registrations should not create duplicate dictionary entries
        _eventDispatchers.TryAdd(typeof(TEvent), (@event,
                                                  dispatcher,
                                                  cancellationToken) => dispatcher.DispatchAsync(Unsafe.As<TEvent>(@event), cancellationToken));
        return this;
    }

    public IMediatorConfig SetEventOutboxStorage<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TEventOutboxStorage>()
        where TEventOutboxStorage : class, IEventOutboxStorage {
        _eventOutBoxStorage = typeof(TEventOutboxStorage);
        return this;
    }

    public IMediatorConfig SetDefaultPublishingMode(PublishMode mode) {
        _defaultPublisherMode = mode;
        return this;
    }

    public IMediatorConfig ConfigureOutbox(Action<OutboxOptions> configure) {
        _outboxConfigure = configure; // delegate expected non-null by contract
        return this;
    }

    public IMediatorConfig EnableCqrsBoundaryEnforcement(bool enable = true) {
        if (!enable) {
            return this;
        }

        return RegisterRequestPipelineBehavior<CqrsBoundaryEnforcementBehavior>();
    }

    public void Apply() {
        _builder.Apply(_services, _lifetime);

        foreach (var action in _actions) {
            action(_services, _lifetime);
        }

        _services.AddScoped(typeof(IEventOrchestrator),  _eventOrchestrator);
        _services.AddScoped(typeof(IEventOutboxStorage), _eventOutBoxStorage);

        _services.Configure<PublisherOptions>(options => { options.DefaultMode       = _defaultPublisherMode; });
        _services.Configure<EventDispatcherOptions>(options => { options.Dispatchers = _eventDispatchers; });
        _services.Configure<OutboxOptions>(options => { _outboxConfigure(options); });
    }
}
