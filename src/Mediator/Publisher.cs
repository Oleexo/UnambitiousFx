using Oleexo.UnambitiousFx.Core;
using Oleexo.UnambitiousFx.Mediator.Abstractions;
using Oleexo.UnambitiousFx.Mediator.Resolvers;

namespace Oleexo.UnambitiousFx.Mediator;

internal sealed class Publisher : IPublisher {
    private readonly IDependencyResolver _dependencyResolver;

    public Publisher(IDependencyResolver dependencyResolver) {
        _dependencyResolver = dependencyResolver;
    }

    public ValueTask<Result> PublishAsync<TEvent>(IContext          context,
                                                  TEvent            @event,
                                                  CancellationToken cancellationToken = default)
        where TEvent : IEvent {
        return _dependencyResolver.GetService<IEventHandlerExecutor<TEvent>>()
                                  .Match(handler => handler.HandleAsync(context, @event, cancellationToken),
                                         () => throw new MissingHandlerException(typeof(EventHandlerExecutor<TEvent>)));
    }
}
