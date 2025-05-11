using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using UnambitiousFx.Mediator.Abstractions;
using UnambitiousFx.Mediator.Resolvers;

namespace UnambitiousFx.Mediator;

/// <summary>
///     Provides extension methods for registering mediator services and related components
///     within an <see cref="IServiceCollection" />.
/// </summary>
public static class DependencyInjectionExtensions {
    /// <summary>
    ///     Adds the mediator services to the specified IServiceCollection.
    /// </summary>
    /// <param name="services">The service collection to add the mediator services to.</param>
    /// <param name="configure">A delegate to configure the mediator services.</param>
    /// <returns>The IServiceCollection with the mediator services added.</returns>
    public static IServiceCollection AddMediator<TContext>(this IServiceCollection           services,
                                                           Action<IMediatorConfig<TContext>> configure)
        where TContext : IContext {
        var cfg = new MediatorConfig<TContext>(services);
        configure(cfg);
        cfg.Apply();
        services.TryAddScoped<IEventDispatcher<TContext>, EventDispatcher<TContext>>();
        services.TryAddScoped<IDependencyResolver, DefaultDependencyResolver>();
        services.TryAddScoped<ISender, Sender>();
        services.TryAddScoped<IPublisher, Publisher>();
        return services;
    }

    /// <summary>
    ///     Adds the mediator services to the specified IServiceCollection.
    /// </summary>
    /// <param name="services">The service collection to add the mediator services to.</param>
    /// <param name="configure">A delegate to configure the mediator services.</param>
    /// <returns>The IServiceCollection with the mediator services added.</returns>
    public static IServiceCollection AddMediator(this IServiceCollection           services,
                                                 Action<IMediatorConfig<IContext>> configure) {
        services.TryAddScoped<IContextFactory<IContext>, DefaultContextFactory>();
        return AddMediator<IContext>(services, configure);
    }

    internal static IServiceCollection RegisterRequestHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler, TContext, TRequest,
                                                              TResponse>(this IServiceCollection services,
                                                                         ServiceLifetime         lifetime = ServiceLifetime.Scoped)
        where TResponse : notnull
        where TRequest : IRequest<TResponse>
        where THandler : class, IRequestHandler<TContext, TRequest, TResponse>
        where TContext : IContext {
        services.Add(new ServiceDescriptor(typeof(THandler),                                       typeof(THandler),                                                     lifetime));
        services.Add(new ServiceDescriptor(typeof(IRequestHandler<TContext, TRequest, TResponse>), typeof(ProxyRequestHandler<TContext, THandler, TRequest, TResponse>), lifetime));
        return services;
    }

    internal static IServiceCollection RegisterRequestHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler, TContext, TRequest>(
        this IServiceCollection services,
        ServiceLifetime         lifetime = ServiceLifetime.Scoped)
        where TRequest : IRequest
        where THandler : class, IRequestHandler<TContext, TRequest>
        where TContext : IContext {
        services.Add(new ServiceDescriptor(typeof(THandler),                            typeof(THandler),                                          lifetime));
        services.Add(new ServiceDescriptor(typeof(IRequestHandler<TContext, TRequest>), typeof(ProxyRequestHandler<TContext, THandler, TRequest>), lifetime));
        return services;
    }

    internal static IServiceCollection RegisterEventHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler, TContext, TEvent>(
        this IServiceCollection services,
        ServiceLifetime         lifetime = ServiceLifetime.Scoped)
        where THandler : class, IEventHandler<TContext, TEvent>
        where TEvent : IEvent
        where TContext : IContext {
        services.Add(new ServiceDescriptor(typeof(IEventHandler<TContext, TEvent>), typeof(THandler), lifetime));
        return services;
    }

    internal static IServiceCollection RegisterRequestPipelineBehavior<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TRequestPipelineBehavior,
                                                                       TContext>(this IServiceCollection services,
                                                                                 ServiceLifetime         lifetime = ServiceLifetime.Scoped)
        where TRequestPipelineBehavior : class, IRequestPipelineBehavior<TContext>
        where TContext : IContext {
        services.Add(new ServiceDescriptor(typeof(IRequestPipelineBehavior<TContext>), typeof(TRequestPipelineBehavior), lifetime));
        return services;
    }

    internal static IServiceCollection RegisterEventPipelineBehavior<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TEventPipelineBehavior,
                                                                     TContext>(this IServiceCollection services,
                                                                               ServiceLifetime         lifetime = ServiceLifetime.Scoped)
        where TEventPipelineBehavior : class, IEventPipelineBehavior<TContext>
        where TContext : IContext {
        services.Add(new ServiceDescriptor(typeof(IEventPipelineBehavior<TContext>), typeof(TEventPipelineBehavior), lifetime));
        return services;
    }
}
