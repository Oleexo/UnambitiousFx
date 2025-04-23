using Oleexo.UnambitiousFx.Core;

namespace Oleexo.UnambitiousFx.Mediator.Abstractions;

public interface IEventPipelineBehavior<in TEvent>
    where TEvent : IEvent {
    ValueTask<Option<IError>> HandleAsync(TEvent               @event,
                                          EventHandlerDelegate next,
                                          CancellationToken    cancellationToken = default);
}
