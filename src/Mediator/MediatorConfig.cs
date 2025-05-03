using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using UnambitiousFx.Mediator.Abstractions;
using UnambitiousFx.Mediator.Orchestrators;

namespace UnambitiousFx.Mediator;

internal sealed class MediatorConfig : IMediatorConfig {
    private readonly List<Action<IServiceCollection, ServiceLifetime>> _actions = new();
    private readonly IServiceCollection                                _services;
    private          DefaultDependencyInjectionBuilder                 _builder;

    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
    private Type _eventOrchestrator;

    private ServiceLifetime _lifetime;

    public MediatorConfig(IServiceCollection services) {
        _services          = services;
        _lifetime          = ServiceLifetime.Scoped;
        _eventOrchestrator = typeof(SequentialEventOrchestrator);
        _builder           = new DefaultDependencyInjectionBuilder();
    }

    public IMediatorConfig SetLifetime(ServiceLifetime lifetime) {
        _lifetime = lifetime;
        return this;
    }

    public IDependencyInjectionBuilder RegisterRequestHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler, TRequest, TResponse>()
        where TResponse : notnull
        where TRequest : IRequest<TResponse>
        where THandler : class, IRequestHandler<TRequest, TResponse> {
        _actions.Add((services,
                      lifetime) => services.RegisterRequestHandler<THandler, TRequest, TResponse>(lifetime));
        return this;
    }

    public IDependencyInjectionBuilder RegisterRequestHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler, TRequest>()
        where TRequest : IRequest
        where THandler : class, IRequestHandler<TRequest> {
        _actions.Add((services,
                      lifetime) => services.RegisterRequestHandler<THandler, TRequest>(lifetime));
        return this;
    }

    public IDependencyInjectionBuilder RegisterEventHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler, TEvent>()
        where THandler : class, IEventHandler<TEvent>
        where TEvent : IEvent {
        _actions.Add((services,
                      lifetime) => services.RegisterEventHandler<THandler, TEvent>(lifetime));
        return this;
    }

    public IMediatorConfig RegisterRequestPipelineBehavior<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TRequestPipelineBehavior>()
        where TRequestPipelineBehavior : class, IRequestPipelineBehavior {
        _actions.Add((services,
                      lifetime) => services.RegisterRequestPipelineBehavior<TRequestPipelineBehavior>(lifetime));
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

    public void Apply() {
        _builder.Apply(_services, _lifetime);

        foreach (var action in _actions) {
            action(_services, _lifetime);
        }

        _services.AddScoped(typeof(IEventOrchestrator), _eventOrchestrator);
    }
}
