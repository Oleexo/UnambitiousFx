using UnambitiousFx.Core;
using UnambitiousFx.Mediator.Abstractions;
using UnambitiousFx.Mediator.Orchestrators;

namespace UnambitiousFx.Mediator;

internal sealed class EventHandlerExecutor<TEvent> : IEventHandlerExecutor<TEvent>
    where TEvent : IEvent {
    private readonly IEnumerable<IEventPipelineBehavior> _behaviors;
    private readonly IEventOrchestrator                  _eventOrchestrator;
    private readonly IEnumerable<IEventHandler<TEvent>>  _handlers;

    public EventHandlerExecutor(IEnumerable<IEventHandler<TEvent>>  handlers,
                                IEnumerable<IEventPipelineBehavior> behaviors,
                                IEventOrchestrator                  eventOrchestrator) {
        _handlers          = handlers;
        _behaviors         = behaviors;
        _eventOrchestrator = eventOrchestrator;
    }

    public ValueTask<Result> HandleAsync(IContext          context,
                                         TEvent            @event,
                                         CancellationToken cancellationToken = default) {
        return ExecutePipelineAsync(context, @event, _behaviors.ToArray(), 0, cancellationToken);
    }

    private ValueTask<Result> ExecutePipelineAsync(IContext                 context,
                                                   TEvent                   @event,
                                                   IEventPipelineBehavior[] behaviors,
                                                   int                      index,
                                                   CancellationToken        cancellationToken) {
        if (index >= behaviors.Length) {
            return _eventOrchestrator.RunAsync(context, _handlers, @event, cancellationToken);
        }

        return behaviors[index]
           .HandleAsync(context, @event, Next, cancellationToken);

        ValueTask<Result> Next() {
            return ExecutePipelineAsync(context, @event, behaviors, index + 1, cancellationToken);
        }
    }
}
