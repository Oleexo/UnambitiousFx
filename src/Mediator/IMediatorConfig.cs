using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using UnambitiousFx.Mediator.Abstractions;
using UnambitiousFx.Mediator.Orchestrators;

namespace UnambitiousFx.Mediator;

/// <summary>
///     Represents the configuration provider for the mediator, allowing the setup of different
///     components such as handlers, pipelines, and orchestrators.
/// </summary>
public interface IMediatorConfig {
    /// <summary>
    ///     Configures the service lifetime for the mediator.
    /// </summary>
    /// <param name="lifetime">The service lifetime to configure, such as Singleton, Scoped, or Transient.</param>
    /// <returns>The current <see cref="IMediatorConfig" /> instance for chaining additional configurations.</returns>
    IMediatorConfig SetLifetime(ServiceLifetime lifetime);

    /// Registers a request handler for a specific request and response type.
    /// This method allows you to specify the type of request handler,
    /// the request type, and the corresponding response type to handle it.
    /// <typeparam name="THandler">
    ///     The type of the request handler to register, which must implement
    ///     <see cref="IRequestHandler{TRequest, TResponse}" />.
    /// </typeparam>
    /// <typeparam name="TRequest">The type of the request object, which must implement <see cref="IRequest{TResponse}" />.</typeparam>
    /// <typeparam name="TResponse">The type of the response object, which must be a non-nullable type.</typeparam>
    /// <returns>
    ///     The <see cref="IMediatorConfig" /> instance for chaining further configurations.
    /// </returns>
    IMediatorConfig RegisterRequestHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler, TRequest, TResponse>()
        where TResponse : notnull
        where TRequest : IRequest<TResponse>
        where THandler : class, IRequestHandler<TRequest, TResponse>;

    /// Registers a request handler for a specific request type.
    /// This method allows registering a handler that will be responsible for processing
    /// instances of a specific request type.
    /// <typeparam name="THandler">The type of the handler that processes the request.</typeparam>
    /// <typeparam name="TRequest">The type of the request to be processed by the handler.</typeparam>
    /// <returns>The current configuration instance to support fluent configuration chaining.</returns>
    IMediatorConfig RegisterRequestHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler, TRequest>()
        where TRequest : IRequest
        where THandler : class, IRequestHandler<TRequest>;

    /// Registers an event handler for a specific event type in the mediator configuration.
    /// This method allows you to associate a handler class with a specific event type
    /// such that the handler will be invoked when the corresponding event is published.
    /// <typeparam name="THandler">
    ///     The type of the event handler that implements the <see cref="IEventHandler{TEvent}" /> interface.
    /// </typeparam>
    /// <typeparam name="TEvent">
    ///     The type of the event that implements the IEvent interface.
    /// </typeparam>
    /// <returns>
    ///     Returns the current instance of IMediatorConfig, allowing for method chaining.
    /// </returns>
    IMediatorConfig RegisterEventHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler, TEvent>()
        where TEvent : IEvent
        where THandler : class, IEventHandler<TEvent>;

    /// Registers a request pipeline behavior to be utilized in the mediator pipeline configuration.
    /// The behavior is applied to every request that goes through the mediator, providing a mechanism
    /// to extend or modify the processing of requests.
    /// <typeparam name="TRequestPipelineBehavior">
    ///     The type of the request pipeline behavior to register. It must be a class implementing
    ///     the <see cref="IRequestPipelineBehavior" /> interface and have public constructors.
    /// </typeparam>
    /// <returns>
    ///     The current instance of <see cref="IMediatorConfig" />, enabling method chaining for further configuration.
    /// </returns>
    IMediatorConfig RegisterRequestPipelineBehavior<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TRequestPipelineBehavior>()
        where TRequestPipelineBehavior : class, IRequestPipelineBehavior;

    /// Registers a custom event pipeline behavior in the mediator configuration pipeline.
    /// Event pipeline behaviors can be used to define and encapsulate custom logic
    /// that is executed before or after event handling by event handlers.
    /// <typeparam name="TEventPipelineBehavior">The type of the event pipeline behavior to register.</typeparam>
    /// <returns>The updated mediator configuration instance.</returns>
    IMediatorConfig RegisterEventPipelineBehavior<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TEventPipelineBehavior>()
        where TEventPipelineBehavior : class, IEventPipelineBehavior;

    /// Configures the event orchestrator to be used for handling events in the mediator.
    /// This method allows the user to specify a custom implementation of the `IEventOrchestrator`
    /// to handle the orchestration of events.
    /// <typeparam name="TEventOrchestrator">
    ///     The type of the event orchestrator that implements `IEventOrchestrator`.
    /// </typeparam>
    /// <returns>
    ///     The instance of `IMediatorConfig` for method chaining.
    /// </returns>
    IMediatorConfig SetEventOrchestrator<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TEventOrchestrator>()
        where TEventOrchestrator : class, IEventOrchestrator;
}
