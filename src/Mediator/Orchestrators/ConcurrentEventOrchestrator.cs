using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions.Collections;
using UnambitiousFx.Mediator.Abstractions;

namespace UnambitiousFx.Mediator.Orchestrators;

/// <summary>
///     An orchestrator that concurrently executes multiple event handlers for a specified event type.
/// </summary>
/// <remarks>
///     The <see cref="ConcurrentEventOrchestrator" /> is responsible for running all event handlers concurrently for
///     a given event.
/// </remarks>
public sealed class ConcurrentEventOrchestrator : IEventOrchestrator {
    /// <inheritdoc />
    public async ValueTask<Result> RunAsync<TEvent>(IEnumerable<IEventHandler<TEvent>> handlers,
                                                    TEvent                             @event,
                                                    CancellationToken                  cancellationToken = default)
        where TEvent : class, IEvent {
        var tasks = handlers.Select(eventHandler => eventHandler.HandleAsync(@event, cancellationToken)
                                                                .AsTask())
                            .ToList();

        var results = await Task.WhenAll(tasks);
        return results.Combine();
    }
}
