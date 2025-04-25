namespace UnambitiousFx.Mediator.Abstractions;

/// <summary>
///     Defines an executor for handling events by delegating to one or more event handlers.
/// </summary>
/// <typeparam name="TEvent">
///     The type of event being handled. Must implement the <see cref="IEvent" /> interface.
/// </typeparam>
public interface IEventHandlerExecutor<in TEvent> : IEventHandler<TEvent>
    where TEvent : IEvent {
}
