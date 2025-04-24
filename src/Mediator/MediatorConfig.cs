using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Oleexo.UnambitiousFx.Mediator.Abstractions;
using Oleexo.UnambitiousFx.Mediator.Orchestrators;

namespace Oleexo.UnambitiousFx.Mediator;

internal sealed class MediatorConfig : IMediatorConfig {
    private readonly List<Action<IServiceCollection, ServiceLifetime>> _actions = new();
    private readonly IServiceCollection                                _services;
    private          Type                                              _eventOrchestrator;
    private          ServiceLifetime                                   _lifetime;

    public MediatorConfig(IServiceCollection services) {
        _services          = services;
        _lifetime          = ServiceLifetime.Scoped;
        _eventOrchestrator = typeof(SequentialEventOrchestrator);
    }

    public IMediatorConfig SetLifetime(ServiceLifetime lifetime) {
        _lifetime = lifetime;
        return this;
    }

    public IMediatorConfig RegisterHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler, TRequest, TResponse>()
        where TResponse : notnull
        where TRequest : IRequest<TResponse>
        where THandler : class, IRequestHandler<TRequest, TResponse> {
        _actions.Add((services,
                      lifetime) => services.RegisterHandler<THandler, TRequest, TResponse>(lifetime));
        return this;
    }

    public IMediatorConfig RegisterHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler, TRequest>()
        where TRequest : IRequest
        where THandler : class, IRequestHandler<TRequest> {
        _actions.Add((services,
                      lifetime) => services.RegisterHandler<THandler, TRequest>(lifetime));
        return this;
    }

    public IMediatorConfig RegisterRequestPipelineBehavior<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TRequestPipelineBehavior>()
        where TRequestPipelineBehavior : class, IRequestPipelineBehavior {
        _actions.Add((services,
                      lifetime) => services.RegisterRequestPipelineBehavior<TRequestPipelineBehavior>(lifetime));
        return this;
    }

    public IMediatorConfig SetEventOrchestrator<TEventOrchestrator>()
        where TEventOrchestrator : class, IEventOrchestrator {
        _eventOrchestrator = typeof(TEventOrchestrator);
        return this;
    }

    public void Apply() {
        foreach (var action in _actions) {
            action(_services, _lifetime);
        }

        _services.AddScoped<IEventOrchestrator, SequentialEventOrchestrator>();
    }
}
