using UnambitiousFx.Core;
using UnambitiousFx.Mediator.Abstractions;

namespace UnambitiousFx.Mediator.Orchestrators;

/// <summary>
///     An orchestrator that concurrently executes multiple event handlers for a specified event type.
/// </summary>
/// <remarks>
///     The <see cref="ConcurrentEventOrchestrator{TContext}" /> is responsible for running all event handlers concurrently
///     for
///     a given event.
/// </remarks>
public sealed class ConcurrentEventOrchestrator<TContext> : IEventOrchestrator<TContext>
    where TContext : IContext {
    /// <inheritdoc />
    public async ValueTask<Result> RunAsync<TEvent>(TContext                                     context,
                                                    IEnumerable<IEventHandler<TContext, TEvent>> handlers,
                                                    TEvent                                       @event,
                                                    CancellationToken                            cancellationToken = default)
        where TEvent : IEvent {
        var tasks = handlers.Select(eventHandler => eventHandler.HandleAsync(context, @event, cancellationToken)
                                                                .AsTask())
                            .ToList();

        var results = await Task.WhenAll(tasks);
        return results.ToResult();
    }
}
