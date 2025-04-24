using Oleexo.UnambitiousFx.Core;
using Oleexo.UnambitiousFx.Mediator.Abstractions;

namespace Oleexo.UnambitiousFx.Mediator.Orchestrators;

public sealed class ConcurrentEventOrchestrator : IEventOrchestrator {
    public async ValueTask<IEnumerable<Result>> RunAsync<TEvent>(IContext                           context,
                                                                 IEnumerable<IEventHandler<TEvent>> handlers,
                                                                 TEvent                             @event,
                                                                 CancellationToken                  cancellationToken = default)
        where TEvent : IEvent {
        var tasks = new List<Task<Result>>();

        foreach (var eventHandler in handlers) {
            tasks.Add(eventHandler.HandleAsync(context, @event, cancellationToken)
                                  .AsTask());
        }

        return await Task.WhenAll(tasks);
    }
}