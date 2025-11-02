using Microsoft.Extensions.DependencyInjection;
using UnambitiousFx.Core.Maybe;

namespace UnambitiousFx.Mediator.Resolvers;

internal sealed class DefaultDependencyResolver : IDependencyResolver {
    private readonly IServiceProvider _serviceProvider;

    public DefaultDependencyResolver(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
    }

    public Maybe<TService> GetService<TService>()
        where TService : class {
        var service = _serviceProvider.GetService<TService>();
        return service is not null
                   ? Maybe.Some(service)
                   : Maybe.None<TService>();
    }

    public IEnumerable<TService> GetServices<TService>()
        where TService : class {
        return _serviceProvider.GetServices<TService>();
    }
}
