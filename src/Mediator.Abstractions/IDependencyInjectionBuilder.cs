using System.Diagnostics.CodeAnalysis;

namespace UnambitiousFx.Mediator.Abstractions;

/// Provides a contract for configuring and registering dependencies related to request and event handling within a dependency injection container.
public interface IDependencyInjectionBuilder {
    /// <summary>
    ///     Registers a request handler implementation for a specific request and response type with the dependency injection
    ///     system.
    /// </summary>
    /// <typeparam name="TRequestHandler">
    ///     The type of the request handler to be registered, which must implement
    ///     <see cref="IRequestHandler{TContext, TRequest, TResponse}" />.
    /// </typeparam>
    /// <typeparam name="TContext">
    ///     The type of the context in which the request is handled, which must implement <see cref="IContext" />.
    /// </typeparam>
    /// <typeparam name="TRequest">
    ///     The type of the request being handled, which must implement <see cref="IRequest{TResponse}" />.
    /// </typeparam>
    /// <typeparam name="TResponse">
    ///     The type of the response returned by the handler, ensuring it is not null.
    /// </typeparam>
    /// <remarks>
    ///     This method is typically used to register a request handler that processes a specific request and returns a typed
    ///     response.
    ///     It enables decoupling of the request processing logic and promotes testability and maintainability.
    /// </remarks>
    IDependencyInjectionBuilder RegisterRequestHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TRequestHandler, TContext, TRequest,
                                                       TResponse>()
        where TRequestHandler : class, IRequestHandler<TContext, TRequest, TResponse>
        where TResponse : notnull
        where TRequest : IRequest<TResponse>
        where TContext : IContext;

    /// <summary>
    ///     Registers a request handler implementation for a specific request type within the dependency injection system.
    /// </summary>
    /// <typeparam name="TRequestHandler">
    ///     The type of the request handler to be registered, which must implement
    ///     <see cref="IRequestHandler{TContext, TRequest}" />.
    /// </typeparam>
    /// <typeparam name="TContext">
    ///     The type of the context in which the request is handled, which must implement <see cref="IContext" />.
    /// </typeparam>
    /// <typeparam name="TRequest">
    ///     The type of the request being handled, which must implement <see cref="IRequest" />.
    /// </typeparam>
    /// <returns>
    ///     The current instance of <see cref="IDependencyInjectionBuilder" />, enabling method chaining.
    /// </returns>
    /// <remarks>
    ///     This method is used to dynamically register a request handler for processing specific request types. It allows
    ///     dependency injection of handlers, promoting modular design and enhancing maintainability.
    /// </remarks>
    IDependencyInjectionBuilder RegisterRequestHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TRequestHandler, TContext, TRequest>()
        where TRequestHandler : class, IRequestHandler<TContext, TRequest>
        where TRequest : IRequest
        where TContext : IContext;

    /// <summary>
    ///     Registers an event handler implementation for a specific event and context type with the dependency injection
    ///     system.
    /// </summary>
    /// <typeparam name="TEventHandler">
    ///     The type of the event handler to be registered, which must implement
    ///     <see cref="IEventHandler{TContext, TEvent}" />.
    /// </typeparam>
    /// <typeparam name="TContext">
    ///     The type of the context in which the event is handled, which must implement <see cref="IContext" />.
    /// </typeparam>
    /// <typeparam name="TEvent">
    ///     The type of the event being handled, which must implement <see cref="IEvent" />.
    /// </typeparam>
    /// <remarks>
    ///     This method is mainly used to integrate event handling mechanisms into the dependency injection container,
    ///     allowing automatic resolution and invocation of appropriate handlers when events occur.
    /// </remarks>
    /// <returns>
    ///     The dependency injection builder, enabling method chaining in the context of dependency registration.
    /// </returns>
    IDependencyInjectionBuilder RegisterEventHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TEventHandler, TContext, TEvent>()
        where TEventHandler : class, IEventHandler<TContext, TEvent>
        where TEvent : class, IEvent
        where TContext : IContext;
}
