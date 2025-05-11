using UnambitiousFx.Core;

namespace UnambitiousFx.Mediator.Abstractions;

/// <summary>
///     Represents a context with properties and methods for managing data during a process or operation.
/// </summary>
public interface IContext {
    /// <summary>
    ///     Gets the unique identifier that represents a correlation ID for tracing or tracking purposes within the context.
    ///     This property is immutable and ensures consistent identification of a specific operation or event flow.
    /// </summary>
    Guid CorrelationId { get; }

    /// Gets the exact date and time at which the operation or context occurred.
    /// This property provides a timestamp that can be used for logging,
    /// debugging, or tracking purposes.
    DateTimeOffset OccuredAt { get; }

    /// <summary>
    ///     Publishes an event asynchronously.
    /// </summary>
    /// <typeparam name="TEvent">The type of the event to be published, which must implement <see cref="IEvent" />.</typeparam>
    /// <param name="event">The event instance to be published.</param>
    /// <param name="cancellationToken">
    ///     A cancellation token to observe while waiting for the task to complete. Defaults to
    ///     <see cref="CancellationToken.None" /> if not provided.
    /// </param>
    /// <returns>
    ///     A <see cref="ValueTask" /> containing a <see cref="Result" /> that indicates the success or failure of the
    ///     operation.
    /// </returns>
    ValueTask<Result> PublishEventAsync<TEvent>(TEvent            @event,
                                                CancellationToken cancellationToken = default)
        where TEvent : class, IEvent;

    /// <summary>
    ///     Publishes an event asynchronously with the specified publish mode.
    /// </summary>
    /// <typeparam name="TEvent">
    ///     The type of the event to be published, which must implement <see cref="IEvent" />.
    /// </typeparam>
    /// <param name="event">
    ///     The event instance to be published.
    /// </param>
    /// <param name="mode">
    ///     The mode in which the event should be published, specified as a <see cref="PublishMode" />.
    /// </param>
    /// <param name="cancellationToken">
    ///     A cancellation token to observe while waiting for the task to complete. Defaults to
    ///     <see cref="CancellationToken.None" /> if not provided.
    /// </param>
    /// <returns>
    ///     A <see cref="ValueTask" /> containing a <see cref="Result" /> that indicates the success or failure of the
    ///     operation.
    /// </returns>
    ValueTask<Result> PublishEventAsync<TEvent>(TEvent            @event,
                                                PublishMode       mode,
                                                CancellationToken cancellationToken = default)
        where TEvent : class, IEvent;

    /// <summary>
    ///     Commits all pending events asynchronously within the specified context.
    /// </summary>
    /// <param name="cancellationToken">
    ///     A cancellation token to observe while waiting for the task to complete. Defaults to
    ///     <see cref="CancellationToken.None" /> if not provided.
    /// </param>
    /// <returns>
    ///     A <see cref="ValueTask" /> containing a <see cref="Result" /> that indicates the success or failure of the
    ///     operation.
    /// </returns>
    ValueTask<Result> CommitEventsAsync(CancellationToken cancellationToken = default);
}
