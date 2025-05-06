using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using UnambitiousFx.Mediator.Abstractions;

namespace UnambitiousFx.Mediator;

internal sealed class DefaultDependencyInjectionBuilder : IDependencyInjectionBuilder {
    private readonly List<Action<IServiceCollection, ServiceLifetime>> _actions = [];

    public IDependencyInjectionBuilder RegisterRequestHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TRequestHandler, TContext, TRequest,
                                                              TResponse>()
        where TRequestHandler : class, IRequestHandler<TContext, TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : notnull
        where TContext : IContext {
        _actions.Add((services,
                      lifetime) => services.RegisterRequestHandler<TRequestHandler, TContext, TRequest, TResponse>(lifetime));
        return this;
    }

    public IDependencyInjectionBuilder RegisterRequestHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TRequestHandler, TContext, TRequest>()
        where TRequestHandler : class, IRequestHandler<TContext, TRequest>
        where TRequest : IRequest
        where TContext : IContext {
        _actions.Add((services,
                      lifetime) => services.RegisterRequestHandler<TRequestHandler, TContext, TRequest>(lifetime));
        return this;
    }

    public IDependencyInjectionBuilder RegisterEventHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TEventHandler, TContext, TEvent>()
        where TEventHandler : class, IEventHandler<TContext, TEvent>
        where TEvent : class, IEvent
        where TContext : IContext {
        _actions.Add((services,
                      lifetime) => services.RegisterEventHandler<TEventHandler, TContext, TEvent>(lifetime));
        return this;
    }

    public void Apply(IServiceCollection services,
                      ServiceLifetime    lifetime) {
        foreach (var action in _actions) {
            action(services, lifetime);
        }
    }
}
