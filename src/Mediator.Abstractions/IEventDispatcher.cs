using UnambitiousFx.Core.Results;

namespace UnambitiousFx.Mediator.Abstractions;

/// <summary>
///     Provides functionality to dispatch events in an asynchronous manner.
/// </summary>
public interface IEventDispatcher
{
    /// Dispatches an event asynchronously, handling the specified context and cancellation token.
    /// <param name="event">
    ///     The event to be handled asynchronously.
    /// </param>
    /// <param name="cancellationToken">
    ///     A token to monitor for cancellation requests.
    /// </param>
    /// <returns>
    ///     A ValueTask containing the result of the dispatch operation, which may indicate success or a failure.
    /// </returns>
    ValueTask<Result> DispatchAsync(IEvent @event,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Dispatches an event asynchronously within the specified context.
    /// </summary>
    /// <typeparam name="TEvent">The type of the event to dispatch, which must implement <see cref="IEvent" />.</typeparam>
    /// <param name="event">The event to be dispatched.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to request cancellation of the operation.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task's result contains a <see cref="Result" />
    ///     indicating the outcome of the operation.
    /// </returns>
    ValueTask<Result> DispatchAsync<TEvent>(TEvent @event,
        CancellationToken cancellationToken = default)
        where TEvent : class, IEvent;
}
