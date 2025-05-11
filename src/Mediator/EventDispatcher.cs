using Microsoft.Extensions.Options;
using UnambitiousFx.Core;
using UnambitiousFx.Mediator.Abstractions;
using UnambitiousFx.Mediator.Orchestrators;
using UnambitiousFx.Mediator.Resolvers;

namespace UnambitiousFx.Mediator;

internal delegate ValueTask<Result> DispatchEventDelegate<TContext>(TContext                   context,
                                                                    IEvent                     @event,
                                                                    IEventDispatcher<TContext> dispatcher,
                                                                    CancellationToken          cancellationToken)
    where TContext : IContext;

internal sealed class EventDispatcher<TContext> : IEventDispatcher<TContext>
    where TContext : IContext {
    private readonly IDependencyResolver                                        _dependencyResolver;
    private readonly IReadOnlyDictionary<Type, DispatchEventDelegate<TContext>> _dispatchers;
    private readonly IEventOrchestrator<TContext>                               _eventOrchestrator;

    public EventDispatcher(IDependencyResolver                        dependencyResolver,
                           IEventOrchestrator<TContext>               eventOrchestrator,
                           IOptions<EventDispatcherOptions<TContext>> options) {
        _dependencyResolver = dependencyResolver;
        _eventOrchestrator  = eventOrchestrator;
        _dispatchers        = options.Value.Dispatchers;
    }

    public ValueTask<Result> DispatchAsync(TContext          context,
                                           IEvent            @event,
                                           CancellationToken cancellationToken) {
        var eventType = @event.GetType();

        if (_dispatchers.TryGetValue(eventType, out var dispatcher)) {
            return dispatcher(context, @event, this, cancellationToken);
        }

        return ValueTask.FromResult(Result.Failure($"No dispatcher registered for event type {eventType.Name}"));
    }

    public ValueTask<Result> DispatchAsync<TEvent>(TContext          context,
                                                   TEvent            @event,
                                                   CancellationToken cancellationToken)
        where TEvent : IEvent {
        var handlers  = _dependencyResolver.GetServices<IEventHandler<TContext, TEvent>>();
        var behaviors = _dependencyResolver.GetServices<IEventPipelineBehavior<TContext>>();

        return ExecutePipelineAsync(context, @event, handlers.ToArray(), behaviors.ToArray(), 0, cancellationToken);
    }

    private ValueTask<Result> ExecutePipelineAsync<TEvent>(TContext                           context,
                                                           TEvent                             @event,
                                                           IEventHandler<TContext, TEvent>[]  handlers,
                                                           IEventPipelineBehavior<TContext>[] behaviors,
                                                           int                                index,
                                                           CancellationToken                  cancellationToken)
        where TEvent : IEvent {
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
