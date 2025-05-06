using UnambitiousFx.Core;

namespace UnambitiousFx.Mediator.Abstractions;

/// <summary>
///     Defines a handler for a specific event type.
/// </summary>
/// <typeparam name="TContext">
///     The context type that provides information for the handling process.
///     Must implement the <see cref="IContext" /> interface.
/// </typeparam>
/// <typeparam name="TEvent">
///     The type of event that this handler will process.
///     Must implement the <see cref="IEvent" /> interface.
/// </typeparam>
public interface IEventHandler<in TContext, in TEvent>
    where TContext : IContext
    where TEvent : IEvent {
    /// <summary>
    ///     Handles the asynchronous processing of an event.
    /// </summary>
    /// <param name="context">The context providing information for the handling process.</param>
    /// <param name="event">The event to be processed.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that, when completed, contains the result of the operation.</returns>
    ValueTask<Result> HandleAsync(TContext          context,
                                  TEvent            @event,
                                  CancellationToken cancellationToken = default);
}
