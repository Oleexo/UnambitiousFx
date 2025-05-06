using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using UnambitiousFx.Mediator.Abstractions;
using UnambitiousFx.Mediator.Orchestrators;

namespace UnambitiousFx.Mediator;

internal sealed class MediatorConfig<TContext> : IMediatorConfig<TContext>
    where TContext : IContext {
    private readonly List<Action<IServiceCollection, ServiceLifetime>> _actions          = new();
    private readonly Dictionary<Type, DispatchEventDelegate<TContext>> _eventDispatchers = new();
    private readonly IServiceCollection                                _services;
    private          DefaultDependencyInjectionBuilder                 _builder;

    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
    private Type? _contextFactory;

    private PublishMode _defaultPublisherMode;

    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
    private Type _eventOrchestrator;

    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
    private Type _eventOutBoxStorage;

    private ServiceLifetime _lifetime;

    public MediatorConfig(IServiceCollection services) {
        _services           = services;
        _lifetime           = ServiceLifetime.Scoped;
        _eventOrchestrator  = typeof(SequentialEventOrchestrator<TContext>);
        _eventOutBoxStorage = typeof(InMemoryEventOutboxStorage);
        _builder            = new DefaultDependencyInjectionBuilder();
    }

    public IMediatorConfig<TContext> SetLifetime(ServiceLifetime lifetime) {
        _lifetime = lifetime;
        return this;
    }

    public IMediatorConfig<TContext> SetContextFactory<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TContextFactory>()
        where TContextFactory : class, IContextFactory<TContext> {
        _contextFactory = typeof(TContextFactory);
        return this;
    }

    public IMediatorConfig<TContext> RegisterRequestHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler, TRequest, TResponse>()
        where TResponse : notnull
        where TRequest : IRequest<TResponse>
        where THandler : class, IRequestHandler<TContext, TRequest, TResponse> {
        _actions.Add((services,
                      lifetime) => services.RegisterRequestHandler<THandler, TContext, TRequest, TResponse>(lifetime));
        return this;
    }

    public IMediatorConfig<TContext> RegisterRequestPipelineBehavior<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TRequestPipelineBehavior>()
        where TRequestPipelineBehavior : class, IRequestPipelineBehavior<TContext> {
        _actions.Add((services,
                      lifetime) => services.RegisterRequestPipelineBehavior<TRequestPipelineBehavior, TContext>(lifetime));
        return this;
    }

    public IMediatorConfig<TContext> RegisterEventPipelineBehavior<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TEventPipelineBehavior>()
        where TEventPipelineBehavior : class, IEventPipelineBehavior<TContext> {
        _actions.Add((services,
                      lifetime) => services.RegisterEventPipelineBehavior<TEventPipelineBehavior, TContext>(lifetime));
        return this;
    }

    public IMediatorConfig<TContext> SetEventOrchestrator<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TEventOrchestrator>()
        where TEventOrchestrator : class, IEventOrchestrator<TContext> {
        _eventOrchestrator = typeof(TEventOrchestrator);
        return this;
    }

    public IMediatorConfig<TContext> SetEventOutboxStorage<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TEventOutboxStorage>()
        where TEventOutboxStorage : class, IEventOutboxStorage {
        _eventOutBoxStorage = typeof(TEventOutboxStorage);
        return this;
    }

    public IMediatorConfig<TContext> AddRegisterGroup(IRegisterGroup group) {
        _builder = new DefaultDependencyInjectionBuilder();
        group.Register(_builder);
        return this;
    }

    public IMediatorConfig<TContext> SetDefaultPublishingMode(PublishMode mode) {
        _defaultPublisherMode = mode;
        return this;
    }

    public IMediatorConfig<TContext> RegisterRequestHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler, TRequest>()
        where TRequest : IRequest
        where THandler : class, IRequestHandler<TContext, TRequest> {
        _actions.Add((services,
                      lifetime) => services.RegisterRequestHandler<THandler, TContext, TRequest>(lifetime));
        return this;
    }

    public IMediatorConfig<TContext> RegisterEventHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler, TEvent>()
        where THandler : class, IEventHandler<TContext, TEvent>
        where TEvent : class, IEvent {
        _actions.Add((services,
                      lifetime) => services.RegisterEventHandler<THandler, TContext, TEvent>(lifetime));
        _eventDispatchers.Add(typeof(TEvent), (context,
                                               @event,
                                               dispatcher,
                                               cancellationToken) => dispatcher.DispatchAsync(context, Unsafe.As<TEvent>(@event), cancellationToken));
        return this;
    }

    public void Apply() {
        _builder.Apply(_services, _lifetime);

        foreach (var action in _actions) {
            action(_services, _lifetime);
        }

        _services.AddScoped(typeof(IEventOrchestrator<TContext>), _eventOrchestrator);
        _services.AddScoped(typeof(IEventOutboxStorage),          _eventOutBoxStorage);
        if (_contextFactory != null) {
            _services.AddScoped(typeof(IContextFactory<TContext>), _contextFactory);
        }

        _services.Configure<PublisherOptions>(options => { options.DefaultMode                 = _defaultPublisherMode; });
        _services.Configure<EventDispatcherOptions<TContext>>(options => { options.Dispatchers = _eventDispatchers; });
    }
}
