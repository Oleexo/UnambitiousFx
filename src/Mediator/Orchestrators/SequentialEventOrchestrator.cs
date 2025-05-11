using UnambitiousFx.Core;
using UnambitiousFx.Mediator.Abstractions;

namespace UnambitiousFx.Mediator.Orchestrators;

/// Represents an orchestrator responsible for executing event handlers sequentially.
/// This class is specifically designed to handle events by invoking each corresponding
/// event handler in sequence. It ensures that every handler processes the event in the
/// specified order, and their individual results are aggregated into a single final result.
public sealed class SequentialEventOrchestrator<TContext> : IEventOrchestrator<TContext>
    where TContext : IContext {
    /// <inheritdoc />
    public async ValueTask<Result> RunAsync<TEvent>(TContext                                     context,
                                                    IEnumerable<IEventHandler<TContext, TEvent>> handlers,
                                                    TEvent                                       @event,
                                                    CancellationToken                            cancellationToken = default)
        where TEvent : IEvent {
        var results = new List<Result>();
        foreach (var eventHandler in handlers) {
            var result = await eventHandler.HandleAsync(context, @event, cancellationToken);
            results.Add(result);
        }

        return results.ToResult();
    }
}
