using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Oleexo.UnambitiousFx.Mediator.Abstractions;
using Oleexo.UnambitiousFx.Mediator.Resolvers;

namespace Oleexo.UnambitiousFx.Mediator;

public interface IMediatorConfig {
    IMediatorConfig SetLifetime(ServiceLifetime lifetime);

    IMediatorConfig RegisterHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler, TRequest, TResponse>()
        where TResponse : notnull
        where TRequest : IRequest<TResponse>
        where THandler : class, IRequestHandler<TRequest, TResponse>;

    IMediatorConfig RegisterHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler, TRequest>()
        where TRequest : IRequest
        where THandler : class, IRequestHandler<TRequest>;

    IMediatorConfig RegisterRequestPipelineBehavior<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TRequestPipelineBehavior>()
        where TRequestPipelineBehavior : class, IRequestPipelineBehavior;
}

internal sealed class MediatorConfig : IMediatorConfig {
    private readonly List<Action<IServiceCollection, ServiceLifetime>> _actions = new();
    private readonly IServiceCollection                                _services;
    private          ServiceLifetime                                   _lifetime;

    public MediatorConfig(IServiceCollection services) {
        _services = services;
        _lifetime = ServiceLifetime.Scoped;
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

    public void Apply() {
        foreach (var action in _actions) {
            action(_services, _lifetime);
        }
    }
}

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddMediator(this IServiceCollection services,
                                                 Action<IMediatorConfig> configure) {
        var cfg = new MediatorConfig(services);
        configure(cfg);
        cfg.Apply();
        return services.AddSingleton<IDependencyResolver, DefaultDependencyResolver>()
                       .AddScoped<Sender>()
                       .AddScoped<ISender>(x => x.GetRequiredService<Sender>());
    }

    internal static IServiceCollection RegisterHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler, TRequest, TResponse>(
        this IServiceCollection services,
        ServiceLifetime         lifetime = ServiceLifetime.Scoped)
        where TResponse : notnull
        where TRequest : IRequest<TResponse>
        where THandler : class, IRequestHandler<TRequest, TResponse> {
        services.Add(new ServiceDescriptor(typeof(THandler),                             typeof(THandler),                                           lifetime));
        services.Add(new ServiceDescriptor(typeof(IRequestHandler<TRequest, TResponse>), typeof(ProxyRequestHandler<THandler, TRequest, TResponse>), lifetime));
        return services;
    }

    internal static IServiceCollection RegisterHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler, TRequest>(
        this IServiceCollection services,
        ServiceLifetime         lifetime = ServiceLifetime.Scoped)
        where TRequest : IRequest
        where THandler : class, IRequestHandler<TRequest> {
        services.Add(new ServiceDescriptor(typeof(THandler),                  typeof(THandler),                                lifetime));
        services.Add(new ServiceDescriptor(typeof(IRequestHandler<TRequest>), typeof(ProxyRequestHandler<THandler, TRequest>), lifetime));
        return services;
    }

    internal static IServiceCollection RegisterRequestPipelineBehavior<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TRequestPipelineBehavior>(
        this IServiceCollection services,
        ServiceLifetime         lifetime = ServiceLifetime.Scoped)
        where TRequestPipelineBehavior : class, IRequestPipelineBehavior {
        services.Add(new ServiceDescriptor(typeof(IRequestPipelineBehavior), typeof(TRequestPipelineBehavior), lifetime));
        return services;
    }
}
