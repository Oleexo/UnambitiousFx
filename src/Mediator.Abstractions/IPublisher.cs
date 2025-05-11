using UnambitiousFx.Core;

namespace UnambitiousFx.Mediator.Abstractions;

/// <summary>
///     Represents a publisher capable of handling and publishing events to their respective subscribers or processors.
/// </summary>
public interface IPublisher {
    /// <summary>
    ///     Publishes an event asynchronously within the given context and provides feedback about success or failure.
    /// </summary>
    /// <typeparam name="TEvent">
    ///     The type of the event to be published. This must implement the <see cref="IEvent" />
    ///     interface.
    /// </typeparam>
    /// <param name="context">
    ///     The context providing additional metadata or state information relevant to the publishing
    ///     process.
    /// </param>
    /// <param name="event">The event instance to be published.</param>
    /// <param name="cancellationToken">An optional cancellation token to cancel the operation.</param>
    /// <returns>A <see cref="ValueTask{TResult}" /> containing the result of the publish operation.</returns>
    ValueTask<Result> PublishAsync<TEvent>(IContext          context,
                                           TEvent            @event,
                                           CancellationToken cancellationToken = default)
        where TEvent : class, IEvent;

    /// <summary>
    ///     Publishes an event asynchronously within the specified context using the given publish mode
    ///     and provides feedback about success or failure.
    /// </summary>
    /// <typeparam name="TEvent">
    ///     The type of the event to be published. This must implement the <see cref="IEvent" />
    ///     interface.
    /// </typeparam>
    /// <param name="context">
    ///     The context providing additional metadata or state information relevant
    ///     to the publishing process.
    /// </param>
    /// <param name="event">The event instance to be published.</param>
    /// <param name="mode">
    ///     Specifies the mode in which the event should be published, such as immediately (Now),
    ///     through an outbox mechanism, or using the default behavior.
    /// </param>
    /// <param name="cancellationToken">
    ///     An optional cancellation token to cancel the operation.
    /// </param>
    /// <returns>A <see cref="ValueTask{TResult}" /> containing the result of the publish operation.</returns>
    ValueTask<Result> PublishAsync<TEvent>(IContext          context,
                                           TEvent            @event,
                                           PublishMode       mode,
                                           CancellationToken cancellationToken = default)
        where TEvent : class, IEvent;

    /// <summary>
    ///     Commits the current operation within the given context and ensures its completion.
    /// </summary>
    /// <param name="context">
    ///     The context providing metadata or state information relevant to the commit process.
    /// </param>
    /// <param name="cancellationToken">
    ///     An optional cancellation token to cancel the operation before completion if needed.
    /// </param>
    /// <returns>
    ///     A <see cref="ValueTask{TResult}" /> containing the result of the commit operation.
    /// </returns>
    ValueTask<Result> CommitAsync(IContext          context,
                                  CancellationToken cancellationToken = default);
}
