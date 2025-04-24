using Oleexo.UnambitiousFx.Core;
using Oleexo.UnambitiousFx.Mediator.Abstractions;

namespace Oleexo.UnambitiousFx.Mediator.Orchestrators;

public interface IEventOrchestrator {
    ValueTask<IEnumerable<Result>> RunAsync<TEvent>(IContext                           context,
                                                    IEnumerable<IEventHandler<TEvent>> handlers,
                                                    TEvent                             @event,
                                                    CancellationToken                  cancellationToken = default)
        where TEvent : IEvent;
}