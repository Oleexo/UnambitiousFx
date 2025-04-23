namespace Oleexo.UnambitiousFx.Mediator.Resolvers;

public interface IDependencyResolver {
    TService? GetService<TService>()
        where TService : class;

    IEnumerable<TService> GetServices<TService>()
        where TService : class;
}
