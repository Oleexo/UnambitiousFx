using Oleexo.UnambitiousFx.Core;

namespace Oleexo.UnambitiousFx.Mediator.Abstractions;

public interface IEventHandler<in TEvent>
    where TEvent : IEvent {
    ValueTask<Result> HandleAsync(IContext          context,
                                  TEvent            @event,
                                  CancellationToken cancellationToken = default);
}
