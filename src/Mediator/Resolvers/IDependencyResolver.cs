using Oleexo.UnambitiousFx.Core;

namespace Oleexo.UnambitiousFx.Mediator.Resolvers;

public interface IDependencyResolver {
    Option<TService> GetService<TService>()
        where TService : class;
}
