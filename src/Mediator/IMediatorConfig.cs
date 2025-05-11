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
    ///     Sets the lifetime of the mediator's components within the dependency injection container.
    /// </summary>
    /// <param name="lifetime">
    ///     The <see cref="ServiceLifetime" /> value that determines the lifetime of the components.
    /// </param>
    /// <returns>
    ///     The current instance of <see cref="IMediatorConfig" />, allowing for fluent configuration.
    /// </returns>
    IMediatorConfig SetLifetime(ServiceLifetime lifetime);

    /// <summary>
    ///     Registers a request pipeline behavior to be included in the mediator's processing pipeline.
    /// </summary>
    /// <typeparam name="TRequestPipelineBehavior">
    ///     The type of the request pipeline behavior that implements <see cref="IRequestPipelineBehavior" />.
    ///     This type must have a public constructor to be resolved at runtime.
    /// </typeparam>
    /// <returns>
    ///     The current instance of <see cref="IMediatorConfig" />, enabling further configuration chaining.
    /// </returns>
    IMediatorConfig RegisterRequestPipelineBehavior<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TRequestPipelineBehavior>()
        where TRequestPipelineBehavior : class, IRequestPipelineBehavior;

    /// <summary>
    ///     Registers a custom event pipeline behavior to be included in the mediator configuration.
    /// </summary>
    /// <typeparam name="TEventPipelineBehavior">
    ///     The type of the event pipeline behavior to register. The type must implement <see cref="IEventPipelineBehavior" />
    ///     and
    ///     must have a public parameterless constructor.
    /// </typeparam>
    /// <returns>
    ///     The instance of <see cref="IMediatorConfig" />, enabling method chaining for further configuration.
    /// </returns>
    IMediatorConfig RegisterEventPipelineBehavior<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TEventPipelineBehavior>()
        where TEventPipelineBehavior : class, IEventPipelineBehavior;

    /// <summary>
    ///     Specifies the event orchestrator implementation to be used for coordinating the execution
    ///     of event handlers. The specified type must implement the <see cref="IEventOrchestrator" /> interface.
    /// </summary>
    /// <typeparam name="TEventOrchestrator">
    ///     The type of the event orchestrator to be registered. Must have accessible public constructors
    ///     and implement <see cref="IEventOrchestrator" />.
    /// </typeparam>
    /// <returns>
    ///     The current <see cref="IMediatorConfig" /> instance to allow method chaining.
    /// </returns>
    IMediatorConfig SetEventOrchestrator<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TEventOrchestrator>()
        where TEventOrchestrator : class, IEventOrchestrator;

    /// <summary>
    ///     Adds a register group to the mediator configuration.
    /// </summary>
    /// <param name="group">
    ///     The instance of <see cref="IRegisterGroup" /> that contains the registrations to be added.
    /// </param>
    /// <returns>
    ///     The current instance of <see cref="IMediatorConfig" />, enabling fluent configuration.
    /// </returns>
    IMediatorConfig AddRegisterGroup(IRegisterGroup group);

    /// <summary>
    ///     Registers a request handler within the mediator configuration to handle requests of a specific type and produce
    ///     responses of a specific type.
    /// </summary>
    /// <typeparam name="THandler">
    ///     The type of the handler that processes the request. Must implement
    ///     <see cref="IRequestHandler{TRequest, TResponse}" />.
    /// </typeparam>
    /// <typeparam name="TRequest">
    ///     The type of the request that the handler processes. Must implement <see cref="IRequest{TResponse}" />.
    /// </typeparam>
    /// <typeparam name="TResponse">
    ///     The type of the response that the handler generates. Must be a non-nullable type.
    /// </typeparam>
    /// <returns>
    ///     The current instance of <see cref="IMediatorConfig" />, enabling method chaining for additional configuration.
    /// </returns>
    IMediatorConfig RegisterRequestHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler, TRequest, TResponse>()
        where TResponse : notnull
        where TRequest : IRequest<TResponse>
        where THandler : class, IRequestHandler<TRequest, TResponse>;

    /// <summary>
    ///     Registers a request handler and its associated request type with the mediator's configuration.
    /// </summary>
    /// <typeparam name="THandler">
    ///     The type of the request handler to be registered. Must implement <see cref="IRequestHandler{TRequest}" />.
    /// </typeparam>
    /// <typeparam name="TRequest">
    ///     The type of the request that the handler processes. Must implement <see cref="IRequest" />.
    /// </typeparam>
    /// <returns>
    ///     The current instance of <see cref="IMediatorConfig" />, enabling chained configuration.
    /// </returns>
    IMediatorConfig RegisterRequestHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler, TRequest>()
        where TRequest : IRequest
        where THandler : class, IRequestHandler<TRequest>;

    /// <summary>
    ///     Registers an event handler for a specific event type.
    /// </summary>
    /// <typeparam name="THandler">
    ///     The handler type that implements <see cref="IEventHandler{TEvent}" />.
    /// </typeparam>
    /// <typeparam name="TEvent">
    ///     The event type that the handler will process, implementing <see cref="IEvent" />.
    /// </typeparam>
    /// <returns>
    ///     The current instance of <see cref="IMediatorConfig" />, allowing for fluent configuration.
    /// </returns>
    IMediatorConfig RegisterEventHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler, TEvent>()
        where THandler : class, IEventHandler<TEvent>
        where TEvent : class, IEvent;

    /// <summary>
    ///     Configures the mediator to use the specified implementation for event outbox storage.
    ///     The event outbox storage is responsible for persisting events and tracking their delivery status.
    /// </summary>
    /// <typeparam name="TEventOutboxStorage">
    ///     The type of the event outbox storage implementation. Must implement <see cref="IEventOutboxStorage" />.
    /// </typeparam>
    /// <returns>
    ///     The current <see cref="IMediatorConfig" /> instance, allowing for method chaining.
    /// </returns>
    IMediatorConfig SetEventOutboxStorage<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TEventOutboxStorage>()
        where TEventOutboxStorage : class, IEventOutboxStorage;

    /// <summary>
    ///     Sets the default publishing mode for events in the mediator configuration.
    /// </summary>
    /// <param name="mode">The <see cref="PublishMode" /> to set as the default publishing mode.</param>
    /// <returns>An instance of <see cref="IMediatorConfig" /> to allow for method chaining.</returns>
    IMediatorConfig SetDefaultPublishingMode(PublishMode mode);

    /// <summary>
    ///     Applies the current configuration to set up the mediator with the provided services and options.
    ///     This method finalizes the configuration by ensuring that all registered components such as
    ///     handlers, orchestrators, and storage are added to the service collection. The configurations
    ///     for publishing and event dispatching are also initialized during this process.
    /// </summary>
    void Apply();
}
