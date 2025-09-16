using Microsoft.Extensions.DependencyInjection;
using UnambitiousFx.Core.Options;

namespace UnambitiousFx.Mediator.Resolvers;

internal sealed class DefaultDependencyResolver : IDependencyResolver {
    private readonly IServiceProvider _serviceProvider;

    public DefaultDependencyResolver(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
    }

    public Option<TService> GetService<TService>()
        where TService : class {
        var service = _serviceProvider.GetService<TService>();
        return service is not null
                   ? Option.Some(service)
                   : Option.None<TService>();
    }

    public IEnumerable<TService> GetServices<TService>()
        where TService : class {
        return _serviceProvider.GetServices<TService>();
    }
}
