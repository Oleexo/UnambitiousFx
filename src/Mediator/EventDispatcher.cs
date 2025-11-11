using Microsoft.Extensions.Options;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Mediator.Abstractions;
using UnambitiousFx.Mediator.Orchestrators;
using UnambitiousFx.Mediator.Resolvers;

namespace UnambitiousFx.Mediator;

internal sealed class EventDispatcher(
    IDependencyResolver dependencyResolver,
    IEventOrchestrator eventOrchestrator,
    IOptions<EventDispatcherOptions> options)
    : IEventDispatcher
{
    private readonly IReadOnlyDictionary<Type, DispatchEventDelegate> _dispatchers = options.Value.Dispatchers;

    public ValueTask<Result> DispatchAsync(IEvent @event,
        CancellationToken cancellationToken)
    {
        var eventType = @event.GetType();

        if (_dispatchers.TryGetValue(eventType, out var dispatcher)) return dispatcher(@event, this, cancellationToken);

        return ValueTask.FromResult(Result.Failure($"No dispatcher registered for event type {eventType.Name}"));
    }

    public ValueTask<Result> DispatchAsync<TEvent>(TEvent @event,
        CancellationToken cancellationToken)
        where TEvent : class, IEvent
    {
        var handlers = dependencyResolver.GetServices<IEventHandler<TEvent>>();
        var behaviors = dependencyResolver.GetServices<IEventPipelineBehavior>();

        return ExecutePipelineAsync(@event, handlers.ToArray(), behaviors.ToArray(), 0, cancellationToken);
    }

    private ValueTask<Result> ExecutePipelineAsync<TEvent>(TEvent @event,
        IEventHandler<TEvent>[] handlers,
        IEventPipelineBehavior[] behaviors,
        int index,
        CancellationToken cancellationToken)
        where TEvent : class, IEvent
    {
        if (index >= behaviors.Length) return eventOrchestrator.RunAsync(handlers, @event, cancellationToken);

        return behaviors[index]
            .HandleAsync(@event, Next, cancellationToken);

        ValueTask<Result> Next()
        {
            return ExecutePipelineAsync(@event, handlers, behaviors, index + 1, cancellationToken);
        }
    }
}
