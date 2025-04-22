using Oleexo.UnambitiousFx.Core.Abstractions;

namespace Oleexo.UnambitiousFx.Mediator.Abstractions;

public interface IEventPipelineBehavior<in TEvent>
    where TEvent : IEvent {
    ValueTask<IOption<IError>> HandleAsync(TEvent               @event,
                                           EventHandlerDelegate next,
                                           CancellationToken    cancellationToken = default);
}
