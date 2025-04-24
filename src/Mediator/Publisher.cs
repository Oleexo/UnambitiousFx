using Oleexo.UnambitiousFx.Core;
using Oleexo.UnambitiousFx.Mediator.Abstractions;
using Oleexo.UnambitiousFx.Mediator.Orchestrators;
using Oleexo.UnambitiousFx.Mediator.Resolvers;

namespace Oleexo.UnambitiousFx.Mediator;

internal sealed class Publisher : IPublisher {
    private readonly IDependencyResolver _dependencyResolver;
    private readonly IEventOrchestrator  _eventOrchestrator;

    public Publisher(IDependencyResolver dependencyResolver,
                     IEventOrchestrator  eventOrchestrator) {
        _dependencyResolver = dependencyResolver;
        _eventOrchestrator  = eventOrchestrator;
    }

    public async ValueTask<Result> PublishAsync<TEvent>(IContext          context,
                                                        TEvent            @event,
                                                        CancellationToken cancellationToken = default)
        where TEvent : IEvent {
        var handlers = _dependencyResolver.GetServices<IEventHandler<TEvent>>();
        var results  = await _eventOrchestrator.RunAsync(context, handlers, @event, cancellationToken);
        return results.ToResult();
    }
}
