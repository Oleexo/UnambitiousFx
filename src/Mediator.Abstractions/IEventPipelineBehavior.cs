using UnambitiousFx.Core;

namespace UnambitiousFx.Mediator.Abstractions;

/// <summary>
///     Defines an interface for implementing a pipeline behavior that can be executed
///     around the handling of events in an event-driven mediator pattern.
/// </summary>
public interface IEventPipelineBehavior<TContext>
    where TContext : IContext {
    /// Handles the given event by executing the next delegate in the event pipeline behavior chain.
    /// <typeparam name="TEvent">
    ///     The type of the event being processed. Must implement the <see cref="IEvent" /> interface.
    /// </typeparam>
    /// <param name="context">
    ///     The execution context associated with the event, containing relevant information like correlation ID and event
    ///     time.
    /// </param>
    /// <param name="event">
    ///     The event instance that is currently being handled by the pipeline behavior.
    /// </param>
    /// <param name="next">
    ///     A delegate representing the subsequent behavior in the pipeline or the ultimate handler when this is the final
    ///     behavior.
    /// </param>
    /// <param name="cancellationToken">
    ///     A token to signal cancellation of the operation.
    /// </param>
    /// <returns>
    ///     A <see cref="ValueTask{Result}" /> representing the execution result of the pipeline behavior and the following
    ///     chain.
    ///     A successful result indicates the event was processed correctly, while a failure result represents processing
    ///     issues.
    /// </returns>
    ValueTask<Result> HandleAsync<TEvent>(TContext             context,
                                          TEvent               @event,
                                          EventHandlerDelegate next,
                                          CancellationToken    cancellationToken = default)
        where TEvent : IEvent;
}
