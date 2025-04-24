using Oleexo.UnambitiousFx.Core;
using Oleexo.UnambitiousFx.Mediator.Abstractions;

namespace Oleexo.UnambitiousFx.Mediator.Orchestrators;

public sealed class SequentialEventOrchestrator : IEventOrchestrator {
    public async ValueTask<Result> RunAsync<TEvent>(IContext                           context,
                                                    IEnumerable<IEventHandler<TEvent>> handlers,
                                                    TEvent                             @event,
                                                    CancellationToken                  cancellationToken = default)
        where TEvent : IEvent {
        var results = new List<Result>();
        foreach (var eventHandler in handlers) {
            var result = await eventHandler.HandleAsync(context, @event, cancellationToken);
            results.Add(result);
        }

        return results.ToResult();
    }
}
