using Oleexo.UnambitiousFx.Core;
using Oleexo.UnambitiousFx.Mediator.Abstractions;

namespace Oleexo.UnambitiousFx.Mediator.Orchestrators;

public sealed class ConcurrentEventOrchestrator : IEventOrchestrator {
    public async ValueTask<Result> RunAsync<TEvent>(IContext                           context,
                                                    IEnumerable<IEventHandler<TEvent>> handlers,
                                                    TEvent                             @event,
                                                    CancellationToken                  cancellationToken = default)
        where TEvent : IEvent {
        var tasks = handlers.Select(eventHandler => eventHandler.HandleAsync(context, @event, cancellationToken)
                                                                .AsTask())
                            .ToList();

        var results = await Task.WhenAll(tasks);
        return results.ToResult();
    }
}
