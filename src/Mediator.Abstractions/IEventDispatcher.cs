using UnambitiousFx.Core.Results;
using UnambitiousFx.Mediator.Transports.Abstractions;

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

    /// <summary>
    ///     Dispatches an event from the outbox, skipping outbox storage.
    ///     This method is NativeAOT-friendly as it maintains generic type information.
    /// </summary>
    /// <typeparam name="TEvent">The type of the event to dispatch, which must implement <see cref="IEvent" />.</typeparam>
    /// <param name="event">The event to be dispatched from the outbox.</param>
    /// <param name="distributionMode">The distribution mode determining how the event should be processed.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to request cancellation of the operation.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task's result contains a <see cref="Result" />
    ///     indicating the outcome of the operation.
    /// </returns>
    /// <remarks>
    ///     This method is used by the OutboxManager to replay events from the outbox without re-storing them.
    ///     It maintains generic type information through the dispatcher delegate pattern, enabling NativeAOT compatibility.
    /// </remarks>
    ValueTask<Result> DispatchFromOutboxAsync<TEvent>(
        TEvent @event,
        DistributionMode distributionMode,
        CancellationToken cancellationToken = default)
        where TEvent : class, IEvent;
}
