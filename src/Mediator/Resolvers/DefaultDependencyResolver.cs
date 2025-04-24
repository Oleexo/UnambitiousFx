using Microsoft.Extensions.DependencyInjection;
using Oleexo.UnambitiousFx.Core;

namespace Oleexo.UnambitiousFx.Mediator.Resolvers;

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
}
