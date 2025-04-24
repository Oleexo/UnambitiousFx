using Oleexo.UnambitiousFx.Core;
using Oleexo.UnambitiousFx.Mediator.Abstractions;
using Oleexo.UnambitiousFx.Mediator.Orchestrators;

namespace Oleexo.UnambitiousFx.Mediator;

internal sealed class ProxyEventHandler<TEvent> : IEventHandler<TEvent>
    where TEvent : IEvent {
    private readonly IEnumerable<IEventPipelineBehavior<TEvent>> _behaviors;
    private readonly IEventOrchestrator                          _eventOrchestrator;
    private readonly IEnumerable<IEventHandler<TEvent>>          _handlers;

    public ProxyEventHandler(IEnumerable<IEventHandler<TEvent>>          handlers,
                             IEnumerable<IEventPipelineBehavior<TEvent>> behaviors,
                             IEventOrchestrator                          eventOrchestrator) {
        _handlers          = handlers;
        _behaviors         = behaviors;
        _eventOrchestrator = eventOrchestrator;
    }

    public ValueTask<Result> HandleAsync(IContext          context,
                                         TEvent            @event,
                                         CancellationToken cancellationToken = default) {
        return ExecutePipelineAsync(context, @event, _behaviors.ToArray(), 0, cancellationToken);
    }

    private ValueTask<Result> ExecutePipelineAsync(IContext                         context,
                                                   TEvent                           @event,
                                                   IEventPipelineBehavior<TEvent>[] behaviors,
                                                   int                              index,
                                                   CancellationToken                cancellationToken) {
        if (index >= behaviors.Length) {
            return _eventOrchestrator.RunAsync(context, _handlers, @event, cancellationToken);
        }

        return behaviors[index]
           .HandleAsync(@event, Next, cancellationToken);

        ValueTask<Result> Next() {
            return ExecutePipelineAsync(context, @event, behaviors, index + 1, cancellationToken);
        }
    }
}
