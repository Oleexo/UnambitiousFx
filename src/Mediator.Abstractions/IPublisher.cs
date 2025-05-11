using UnambitiousFx.Core;

namespace UnambitiousFx.Mediator.Abstractions;

/// <summary>
///     Represents a publisher responsible for publishing events within a defined context and propagating
///     them to appropriate handlers or subscribers.
/// </summary>
public interface IPublisher<in TContext>
    where TContext : IContext {
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
    ValueTask<Result> PublishAsync<TEvent>(TContext          context,
                                           TEvent            @event,
                                           CancellationToken cancellationToken = default)
        where TEvent : IEvent;

    /// <summary>
    ///     Publishes an event asynchronously within the given context, offering control over the publishing mode,
    ///     and provides feedback about success or failure.
    /// </summary>
    /// <typeparam name="TEvent">
    ///     The type of the event to be published. This must implement the <see cref="IEvent" />
    ///     interface.
    /// </typeparam>
    /// <param name="context">
    ///     The context providing additional metadata or state information relevant to the publishing process.
    /// </param>
    /// <param name="event">The event instance to be published.</param>
    /// <param name="mode">
    ///     Specifies the publishing mode which determines how the event should be processed.
    ///     It can be immediate or processed through an outbox mechanism.
    /// </param>
    /// <param name="cancellationToken">An optional cancellation token to cancel the operation.</param>
    /// <returns>A <see cref="ValueTask{TResult}" /> containing the result of the publish operation.</returns>
    ValueTask<Result> PublishAsync<TEvent>(TContext          context,
                                           TEvent            @event,
                                           PublishMode       mode,
                                           CancellationToken cancellationToken = default)
        where TEvent : IEvent;

    /// <summary>
    ///     Commits all events that have been published using the outbox pattern within the provided context.
    /// </summary>
    /// <param name="context">
    ///     The contextual information associated with the commit operation, providing any necessary metadata or state.
    /// </param>
    /// <param name="cancellationToken">
    ///     An optional cancellation token to signal the request for cancellation of the commit operation.
    /// </param>
    /// <returns>
    ///     A <see cref="ValueTask{TResult}" /> containing the result of the commit operation.
    /// </returns>
    ValueTask<Result> CommitAsync(TContext          context,
                                  CancellationToken cancellationToken = default);
}

/// <summary>
///     Represents a publisher capable of handling and publishing events to their respective subscribers or processors.
/// </summary>
public interface IPublisher : IPublisher<IContext>;
