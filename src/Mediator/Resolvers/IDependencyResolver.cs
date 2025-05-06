using UnambitiousFx.Core;

namespace UnambitiousFx.Mediator.Resolvers;

/// <summary>
///     Represents a dependency resolver used to retrieve implementations of services.
/// </summary>
public interface IDependencyResolver {
    /// Retrieves a service of the specified type from the dependency resolver.
    /// <typeparam name="TService">The type of the service to retrieve.</typeparam>
    /// <returns>
    ///     An Option that contains the retrieved service if it exists; otherwise, an empty Option.
    /// </returns>
    Option<TService> GetService<TService>()
        where TService : class;

    /// Retrieves all services of the specified type from the dependency resolver.
    /// <typeparam name="TService">The type of the services to retrieve.</typeparam>
    /// <returns>
    ///     An enumerable of the retrieved services of the specified type.
    /// </returns>
    IEnumerable<TService> GetServices<TService>()
        where TService : class;
}
