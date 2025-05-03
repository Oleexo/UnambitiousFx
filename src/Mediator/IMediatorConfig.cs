using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using UnambitiousFx.Mediator.Abstractions;
using UnambitiousFx.Mediator.Orchestrators;

namespace UnambitiousFx.Mediator;

/// <summary>
///     Represents the configuration provider for the mediator, allowing the setup of different
///     components such as handlers, pipelines, and orchestrators.
/// </summary>
public interface IMediatorConfig : IDependencyInjectionBuilder {
    /// <summary>
    ///     Configures the service lifetime for the mediator.
    /// </summary>
    /// <param name="lifetime">The service lifetime to configure, such as Singleton, Scoped, or Transient.</param>
    /// <returns>The current <see cref="IMediatorConfig" /> instance for chaining additional configurations.</returns>
    IMediatorConfig SetLifetime(ServiceLifetime lifetime);

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

    /// <summary>
    ///     Adds a register group to the mediator configuration.
    /// </summary>
    /// <param name="group">The register group to add, implementing <see cref="IRegisterGroup" />.</param>
    /// <returns>The current <see cref="IMediatorConfig" /> instance to enable further configuration chaining.</returns>
    IMediatorConfig AddRegisterGroup(IRegisterGroup group);
}
