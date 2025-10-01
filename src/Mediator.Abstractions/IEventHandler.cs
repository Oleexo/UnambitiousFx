﻿using UnambitiousFx.Core.Results;

namespace UnambitiousFx.Mediator.Abstractions;

/// <summary>
///     Defines a handler for a specific event type.
/// </summary>
/// <typeparam name="TEvent">
///     The type of event that this handler will process.
///     Must implement the <see cref="IEvent" /> interface.
/// </typeparam>
public interface IEventHandler<in TEvent>
    where TEvent : class, IEvent {
    /// Handles the asynchronous processing of an event.
    /// <param name="context">The context providing information for the handling process.</param>
    /// <param name="event">The event to be processed.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <return>A task that, when completed, contains the result of the operation.</return>
    ValueTask<Result> HandleAsync(IContext          context,
                                  TEvent            @event,
                                  CancellationToken cancellationToken = default);
}
