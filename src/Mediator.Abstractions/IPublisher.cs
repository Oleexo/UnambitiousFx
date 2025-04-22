using Oleexo.UnambitiousFx.Core.Abstractions;

namespace Oleexo.UnambitiousFx.Mediator.Abstractions;

public interface IPublisher {
    ValueTask<IOption<IError>> PublishAsync<TEvent>(IContext          context,
                                                    TEvent            @event,
                                                    CancellationToken cancellationToken = default)
        where TEvent : IEvent;
}
