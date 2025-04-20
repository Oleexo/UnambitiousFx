namespace Oleexo.UnambitiousFx.Mediator;

public interface IDependencyResolver {
    TService? Resolve<TService>()
        where TService : class;
}
