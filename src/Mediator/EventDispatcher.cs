using Microsoft.Extensions.Options;
using UnambitiousFx.Core;
using UnambitiousFx.Mediator.Abstractions;
using UnambitiousFx.Mediator.Orchestrators;
using UnambitiousFx.Mediator.Resolvers;

namespace UnambitiousFx.Mediator;

internal sealed class EventDispatcher : IEventDispatcher {
    private readonly IDependencyResolver                              _dependencyResolver;
    private readonly IReadOnlyDictionary<Type, DispatchEventDelegate> _dispatchers;
    private readonly IEventOrchestrator                               _eventOrchestrator;

    public EventDispatcher(IDependencyResolver              dependencyResolver,
                           IEventOrchestrator               eventOrchestrator,
                           IOptions<EventDispatcherOptions> options) {
        _dependencyResolver = dependencyResolver;
        _eventOrchestrator  = eventOrchestrator;
        _dispatchers        = options.Value.Dispatchers;
    }

    public ValueTask<Result> DispatchAsync(IContext          context,
                                           IEvent            @event,
                                           CancellationToken cancellationToken) {
        var eventType = @event.GetType();

        if (_dispatchers.TryGetValue(eventType, out var dispatcher)) {
            return dispatcher(context, @event, this, cancellationToken);
        }

        return ValueTask.FromResult(Result.Failure($"No dispatcher registered for event type {eventType.Name}"));
    }

    public ValueTask<Result> DispatchAsync<TEvent>(IContext          context,
                                                   TEvent            @event,
                                                   CancellationToken cancellationToken)
        where TEvent : class, IEvent {
        var handlers  = _dependencyResolver.GetServices<IEventHandler<TEvent>>();
        var behaviors = _dependencyResolver.GetServices<IEventPipelineBehavior>();

        return ExecutePipelineAsync(context, @event, handlers.ToArray(), behaviors.ToArray(), 0, cancellationToken);
    }

    private ValueTask<Result> ExecutePipelineAsync<TEvent>(IContext                 context,
                                                           TEvent                   @event,
                                                           IEventHandler<TEvent>[]  handlers,
                                                           IEventPipelineBehavior[] behaviors,
                                                           int                      index,
                                                           CancellationToken        cancellationToken)
        where TEvent : class, IEvent {
        if (index >= behaviors.Length) {
            return _eventOrchestrator.RunAsync(context, handlers, @event, cancellationToken);
        }

        return behaviors[index]
           .HandleAsync(context, @event, Next, cancellationToken);

        ValueTask<Result> Next() {
            return ExecutePipelineAsync(context, @event, handlers, behaviors, index + 1, cancellationToken);
        }
    }
}
