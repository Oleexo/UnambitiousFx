using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace Oleexo.UnambitiousFx.Mediator;

public static class ServiceCollectionExtensions {
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

    internal static IServiceCollection RegisterMutationHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler, TMutation, TResponse>(
        this IServiceCollection services,
        ServiceLifetime         lifetime = ServiceLifetime.Scoped)
        where TResponse : notnull
        where TMutation : IMutation<TResponse>
        where THandler : class, IMutationHandler<TMutation, TResponse> {
        services.Add(new ServiceDescriptor(typeof(THandler),                               typeof(THandler),                                                      lifetime));
        services.Add(new ServiceDescriptor(typeof(IMutationHandler<TMutation, TResponse>), typeof(ProxyMutationHandler<THandler, TMutation, TResponse>),          lifetime));
        services.Add(new ServiceDescriptor(typeof(IRequestHandler<TMutation, TResponse>),  sp => sp.GetRequiredService<IMutationHandler<TMutation, TResponse>>(), lifetime));

        return services;
    }

    internal static IServiceCollection RegisterQueryHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler, TQuery, TResponse>(
        this IServiceCollection services,
        ServiceLifetime         lifetime = ServiceLifetime.Scoped)
        where TResponse : notnull
        where TQuery : IQuery<TResponse>
        where THandler : class, IQueryHandler<TQuery, TResponse> {
        services.Add(new ServiceDescriptor(typeof(THandler),                           typeof(THandler),                                                lifetime));
        services.Add(new ServiceDescriptor(typeof(IQueryHandler<TQuery, TResponse>),   typeof(ProxyQueryHandler<THandler, TQuery, TResponse>),          lifetime));
        services.Add(new ServiceDescriptor(typeof(IRequestHandler<TQuery, TResponse>), sp => sp.GetRequiredService<IQueryHandler<TQuery, TResponse>>(), lifetime));
        return services;
    }

    internal static IServiceCollection RegisterRequestPipelineBehavior(this IServiceCollection services,
                                                                       [DynamicallyAccessedMembers(
                                                                           DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.Interfaces)]
                                                                       Type requestBehaviorType,
                                                                       ServiceLifetime lifetime = ServiceLifetime.Scoped) {
        if (!requestBehaviorType.IsGenericType ||
            !requestBehaviorType.GetInterfaces()
                                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestPipelineBehavior<,>))) {
            throw new ArgumentException($"Type {requestBehaviorType.Name} must implement IRequestPipelineBehavior<,>", nameof(requestBehaviorType));
        }

        services.Add(new ServiceDescriptor(typeof(IRequestPipelineBehavior<,>), requestBehaviorType, lifetime));
        return services;
    }

    internal static IServiceCollection RegisterMutationPipelineBehavior(this IServiceCollection services,
                                                                        [DynamicallyAccessedMembers(
                                                                            DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.Interfaces)]
                                                                        Type mutationBehaviorType,
                                                                        ServiceLifetime lifetime = ServiceLifetime.Scoped) {
        if (!mutationBehaviorType.IsGenericType ||
            !mutationBehaviorType.GetInterfaces()
                                 .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMutationPipelineBehavior<,>))) {
            throw new ArgumentException($"Type {mutationBehaviorType.Name} must implement IMutationPipelineBehavior<,>", nameof(mutationBehaviorType));
        }

        services.Add(new ServiceDescriptor(typeof(IMutationPipelineBehavior<,>), mutationBehaviorType, lifetime));
        return services;
    }

    internal static IServiceCollection RegisterQueryPipelineBehavior(this IServiceCollection services,
                                                                     [DynamicallyAccessedMembers(
                                                                         DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.Interfaces)]
                                                                     Type queryBehaviorType,
                                                                     ServiceLifetime lifetime = ServiceLifetime.Scoped) {
        if (!queryBehaviorType.IsGenericType ||
            !queryBehaviorType.GetInterfaces()
                              .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryPipelineBehavior<,>))) {
            throw new ArgumentException($"Type {queryBehaviorType.Name} must implement IQueryPipelineBehavior<,>", nameof(queryBehaviorType));
        }

        services.Add(new ServiceDescriptor(typeof(IQueryPipelineBehavior<,>), queryBehaviorType, lifetime));
        return services;
    }
}
