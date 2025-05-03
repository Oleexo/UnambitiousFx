using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using UnambitiousFx.Mediator.Abstractions;
using UnambitiousFx.Mediator.Resolvers;

namespace UnambitiousFx.Mediator;

/// <summary>
///     Provides extension methods for registering mediator services and related components
///     within an <see cref="IServiceCollection" />.
/// </summary>
public static class ServiceCollectionExtensions {
    /// <summary>
    ///     Adds the mediator services to the specified IServiceCollection.
    /// </summary>
    /// <param name="services">The service collection to add the mediator services to.</param>
    /// <param name="configure">A delegate to configure the mediator services.</param>
    /// <returns>The IServiceCollection with the mediator services added.</returns>
    public static IServiceCollection AddMediator(this IServiceCollection services,
                                                 Action<IMediatorConfig> configure) {
        var cfg = new MediatorConfig(services);
        configure(cfg);
        cfg.Apply();
        return services.AddScoped<IDependencyResolver, DefaultDependencyResolver>()
                       .AddScoped<ISender, Sender>()
                       .AddScoped<IPublisher, Publisher>()
                       .AddScoped(typeof(IEventHandlerExecutor<>), typeof(EventHandlerExecutor<>))
                       .AddScoped<IContextFactory, ContextFactory>();
    }

    internal static IServiceCollection RegisterRequestHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler, TRequest, TResponse>(
        this IServiceCollection services,
        ServiceLifetime         lifetime = ServiceLifetime.Scoped)
        where TResponse : notnull
        where TRequest : IRequest<TResponse>
        where THandler : class, IRequestHandler<TRequest, TResponse> {
        services.Add(new ServiceDescriptor(typeof(THandler),                             typeof(THandler),                                           lifetime));
        services.Add(new ServiceDescriptor(typeof(IRequestHandler<TRequest, TResponse>), typeof(ProxyRequestHandler<THandler, TRequest, TResponse>), lifetime));
        return services;
    }

    internal static IServiceCollection RegisterRequestHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler, TRequest>(
        this IServiceCollection services,
        ServiceLifetime         lifetime = ServiceLifetime.Scoped)
        where TRequest : IRequest
        where THandler : class, IRequestHandler<TRequest> {
        services.Add(new ServiceDescriptor(typeof(THandler),                  typeof(THandler),                                lifetime));
        services.Add(new ServiceDescriptor(typeof(IRequestHandler<TRequest>), typeof(ProxyRequestHandler<THandler, TRequest>), lifetime));
        return services;
    }

    internal static IServiceCollection RegisterEventHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler, TEvent>(
        this IServiceCollection services,
        ServiceLifetime         lifetime = ServiceLifetime.Scoped)
        where THandler : class, IEventHandler<TEvent>
        where TEvent : IEvent {
        services.Add(new ServiceDescriptor(typeof(IEventHandler<TEvent>), typeof(THandler), lifetime));
        return services;
    }

    internal static IServiceCollection RegisterRequestPipelineBehavior<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TRequestPipelineBehavior>(
        this IServiceCollection services,
        ServiceLifetime         lifetime = ServiceLifetime.Scoped)
        where TRequestPipelineBehavior : class, IRequestPipelineBehavior {
        services.Add(new ServiceDescriptor(typeof(IRequestPipelineBehavior), typeof(TRequestPipelineBehavior), lifetime));
        return services;
    }

    internal static IServiceCollection RegisterEventPipelineBehavior<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TEventPipelineBehavior>(
        this IServiceCollection services,
        ServiceLifetime         lifetime = ServiceLifetime.Scoped)
        where TEventPipelineBehavior : class, IEventPipelineBehavior {
        services.Add(new ServiceDescriptor(typeof(IEventPipelineBehavior), typeof(TEventPipelineBehavior), lifetime));
        return services;
    }
}
