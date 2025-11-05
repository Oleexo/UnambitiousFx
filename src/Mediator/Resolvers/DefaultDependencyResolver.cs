using Microsoft.Extensions.DependencyInjection;

namespace UnambitiousFx.Mediator.Resolvers;

internal sealed class DefaultDependencyResolver : IDependencyResolver {
    private readonly IServiceProvider _serviceProvider;

    public DefaultDependencyResolver(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
    }

    public TService? GetService<TService>()
        where TService : class {
        return _serviceProvider.GetService<TService>();
    }

    public TService GetRequiredService<TService>()
        where TService : class {
        // maybe improve error message here later
        return _serviceProvider.GetRequiredService<TService>();
    }

    public IEnumerable<TService> GetServices<TService>()
        where TService : class {
        return _serviceProvider.GetServices<TService>();
    }
}
