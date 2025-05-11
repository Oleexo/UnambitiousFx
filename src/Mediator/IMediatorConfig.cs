using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using UnambitiousFx.Mediator.Abstractions;
using UnambitiousFx.Mediator.Orchestrators;

namespace UnambitiousFx.Mediator;

/// <summary>
///     Represents the configuration provider for the mediator, allowing the setup of different
///     components such as handlers, pipelines, and orchestrators.
/// </summary>
public interface IMediatorConfig<TContext>
    where TContext : IContext {
    /// <summary>
    ///     Configures the service lifetime for the mediator.
    /// </summary>
    /// <param name="lifetime">The service lifetime to configure, such as Singleton, Scoped, or Transient.</param>
    /// <returns>The current <see cref="IMediatorConfig{TContext}" /> instance for chaining additional configurations.</returns>
    IMediatorConfig<TContext> SetLifetime(ServiceLifetime lifetime);

    /// <summary>
    ///     Configures the factory responsible for creating instances of the context.
    /// </summary>
    /// <typeparam name="TContextFactory">
    ///     The type of the context factory to configure, which must implement
    ///     <see cref="IContextFactory{TContext}" />.
    /// </typeparam>
    /// <returns>The current <see cref="IMediatorConfig{TContext}" /> instance for chaining additional configurations.</returns>
    IMediatorConfig<TContext> SetContextFactory<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TContextFactory>()
        where TContextFactory : class, IContextFactory<TContext>;

    /// <summary>
    ///     Registers a request handler for processing specific types of requests and producing responses.
    /// </summary>
    /// <typeparam name="TRequestHandler">The type of the request handler to register.</typeparam>
    /// <typeparam name="TRequest">The type of the request that the handler processes.</typeparam>
    /// <typeparam name="TResponse">The type of the response produced by the handler.</typeparam>
    /// <returns>The current instance of <see cref="IMediatorConfig{TContext}" /> for chaining additional configurations.</returns>
    IMediatorConfig<TContext> RegisterRequestHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TRequestHandler, TRequest,
                                                     TResponse>()
        where TRequestHandler : class, IRequestHandler<TContext, TRequest, TResponse>
        where TResponse : notnull
        where TRequest : IRequest<TResponse>;

    /// <summary>
    ///     Registers a request handler for a specific request type with the mediator configuration.
    /// </summary>
    /// <typeparam name="TRequestHandler">
    ///     The type of the request handler to register, implementing
    ///     <see cref="IRequestHandler{TContext, TRequest}" />.
    /// </typeparam>
    /// <typeparam name="TRequest">The type of the request handled by <typeparamref name="TRequestHandler" />.</typeparam>
    /// <returns>The current <see cref="IMediatorConfig{TContext}" /> instance for chaining additional configurations.</returns>
    IMediatorConfig<TContext> RegisterRequestHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TRequestHandler, TRequest>()
        where TRequestHandler : class, IRequestHandler<TContext, TRequest>
        where TRequest : IRequest;

    /// <summary>
    ///     Registers an event handler for the mediator that handles a specific event type.
    /// </summary>
    /// <typeparam name="TEventHandler">
    ///     The type of the event handler to register, implementing <see cref="IEventHandler{TContext, TEvent}" />.
    /// </typeparam>
    /// <typeparam name="TEvent">
    ///     The type of the event to be handled, implementing <see cref="IEvent" />.
    /// </typeparam>
    /// <returns>
    ///     The current <see cref="IMediatorConfig{TContext}" /> instance for chaining additional configurations.
    /// </returns>
    IMediatorConfig<TContext> RegisterEventHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TEventHandler, TEvent>()
        where TEventHandler : class, IEventHandler<TContext, TEvent>
        where TEvent : class, IEvent;

    /// Registers a request pipeline behavior to be utilized in the mediator pipeline configuration.
    /// The behavior is applied to every request that goes through the mediator, providing a mechanism
    /// to extend or modify the processing of requests.
    /// <typeparam name="TRequestPipelineBehavior">
    ///     The type of the request pipeline behavior to register. It must be a class implementing
    ///     the <see cref="IRequestPipelineBehavior{TContext}" /> interface and have public constructors.
    /// </typeparam>
    /// <returns>
    ///     The current instance of <see cref="IMediatorConfig{TContext}" />, enabling method chaining for further
    ///     configuration.
    /// </returns>
    IMediatorConfig<TContext> RegisterRequestPipelineBehavior<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TRequestPipelineBehavior>()
        where TRequestPipelineBehavior : class, IRequestPipelineBehavior<TContext>;

    /// Registers a custom event pipeline behavior in the mediator configuration pipeline.
    /// Event pipeline behaviors can be used to define and encapsulate custom logic
    /// that is executed before or after event handling by event handlers.
    /// <typeparam name="TEventPipelineBehavior">The type of the event pipeline behavior to register.</typeparam>
    /// <returns>The updated mediator configuration instance.</returns>
    IMediatorConfig<TContext> RegisterEventPipelineBehavior<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TEventPipelineBehavior>()
        where TEventPipelineBehavior : class, IEventPipelineBehavior<TContext>;

    /// Configures the event orchestrator to be used for handling events in the mediator.
    /// This method allows the user to specify a custom implementation of the `IEventOrchestrator`
    /// to handle the orchestration of events.
    /// <typeparam name="TEventOrchestrator">
    ///     The type of the event orchestrator that implements `IEventOrchestrator`.
    /// </typeparam>
    /// <returns>
    ///     The instance of `IMediatorConfig` for method chaining.
    /// </returns>
    IMediatorConfig<TContext> SetEventOrchestrator<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TEventOrchestrator>()
        where TEventOrchestrator : class, IEventOrchestrator<TContext>;

    /// <summary>
    ///     Configures the event outbox storage implementation to be used by the mediator.
    /// </summary>
    /// <typeparam name="TEventOutboxStorage">
    ///     The type of the event outbox storage, which must implement <see cref="IEventOutboxStorage" />.
    /// </typeparam>
    /// <returns>The current <see cref="IMediatorConfig{TContext}" /> instance for chaining additional configurations.</returns>
    IMediatorConfig<TContext> SetEventOutboxStorage<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TEventOutboxStorage>()
        where TEventOutboxStorage : class, IEventOutboxStorage;

    /// <summary>
    ///     Adds a register group to the mediator configuration.
    /// </summary>
    /// <param name="group">The register group to add, implementing <see cref="IRegisterGroup" />.</param>
    /// <returns>The current <see cref="IMediatorConfig{TContext}" /> instance to enable further configuration chaining.</returns>
    IMediatorConfig<TContext> AddRegisterGroup(IRegisterGroup group);

    /// <summary>
    ///     Configures the default publishing mode for events handled by the mediator.
    /// </summary>
    /// <param name="mode">The publishing mode to set, such as Now or Outbox.</param>
    /// <returns>The current <see cref="IMediatorConfig{TContext}" /> instance for chaining additional configurations.</returns>
    IMediatorConfig<TContext> SetDefaultPublishingMode(PublishMode mode);
}
