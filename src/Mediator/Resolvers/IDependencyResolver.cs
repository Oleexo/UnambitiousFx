namespace Oleexo.UnambitiousFx.Mediator.Resolvers;

public interface IDependencyResolver {
    TService? Resolve<TService>()
        where TService : class;
}
