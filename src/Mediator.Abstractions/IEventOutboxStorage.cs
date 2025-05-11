﻿using UnambitiousFx.Core;

namespace UnambitiousFx.Mediator.Abstractions;

/// <summary>
///     Represents the contract for an event outbox storage mechanism to manage and store events reliably.
/// </summary>
/// <remarks>
///     This interface serves as the foundation for implementing event storage systems designed to support
///     dependable event handling and dispatch. It ensures the persistence and retrieval of events
///     during scenarios requiring guaranteed delivery or ordered processing.
///     Implementations may vary from in-memory storage to fully persistent systems, catering to
///     different operational and performance considerations.
/// </remarks>
public interface IEventOutboxStorage {
    /// <summary>
    ///     Adds an event to the outbox storage for later processing.
    /// </summary>
    /// <typeparam name="TEvent">The type of the event being added. Must implement the <see cref="IEvent" /> interface.</typeparam>
    /// <param name="event">The event to be added to the outbox storage.</param>
    /// <param name="cancellationToken">
    ///     A token to observe while waiting for the task to complete. Defaults to
    ///     <see cref="CancellationToken.None" />.
    /// </param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a <see cref="Result" /> indicating
    ///     whether the event was successfully added.
    /// </returns>
    ValueTask<Result> AddAsync<TEvent>(TEvent            @event,
                                       CancellationToken cancellationToken = default)
        where TEvent : class, IEvent;

    /// <summary>
    ///     Retrieves all pending events that have not yet been marked as processed.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a collection of pending events.
    /// </returns>
    ValueTask<IEnumerable<IEvent>> GetPendingEventsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    ///     Marks the specified event as processed in the event outbox storage.
    /// </summary>
    /// <param name="event">
    ///     The event to be marked as processed.
    /// </param>
    /// <param name="cancellationToken">
    ///     A token to monitor for cancellation requests.
    /// </param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a <see cref="Result" />
    ///     indicating whether the event was successfully marked as processed.
    /// </returns>
    ValueTask<Result> MarkAsProcessedAsync(IEvent            @event,
                                           CancellationToken cancellationToken = default);

    /// <summary>
    ///     Clears all events in the event outbox storage.
    /// </summary>
    /// <param name="cancellationToken">
    ///     A token to monitor for cancellation requests.
    /// </param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a <see cref="Result" />
    ///     object indicating whether the operation was successful.
    /// </returns>
    ValueTask<Result> ClearAsync(CancellationToken cancellationToken = default);
}
