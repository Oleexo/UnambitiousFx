using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Oleexo.UnambitiousFx.Mediator.Abstractions;
using Oleexo.UnambitiousFx.Mediator.Resolvers;

namespace Oleexo.UnambitiousFx.Mediator;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddMediator(this IServiceCollection services,
                                                 Action<IMediatorConfig> configure) {
        var cfg = new MediatorConfig(services);
        configure(cfg);
        cfg.Apply();
        return services.AddScoped<IDependencyResolver, DefaultDependencyResolver>()
                       .AddScoped<ISender, Sender>()
                       .AddScoped<IPublisher, Publisher>()
                       .AddScoped(typeof(ProxyEventHandler<>))
                       .AddScoped<IContextFactory, ContextFactory>();
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
