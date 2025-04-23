using Oleexo.UnambitiousFx.Core;

namespace Oleexo.UnambitiousFx.Mediator.Abstractions;

public interface IPublisher {
    ValueTask<Result> PublishAsync<TEvent>(IContext          context,
                                           TEvent            @event,
                                           CancellationToken cancellationToken = default)
        where TEvent : IEvent;
}
