namespace Oleexo.UnambitiousFx.Mediator;

internal sealed class DefaultDependencyResolver : IDependencyResolver {
    private readonly IServiceProvider _serviceProvider;

    public DefaultDependencyResolver(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
    }

    public TService? Resolve<TService>()
        where TService : class {
        return _serviceProvider.GetService(typeof(TService)) as TService;
    }
}
