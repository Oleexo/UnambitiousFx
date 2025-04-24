using Oleexo.UnambitiousFx.Core;

namespace Oleexo.UnambitiousFx.Mediator.Abstractions;

public interface IEventPipelineBehavior {
    ValueTask<Result> HandleAsync<TEvent>(TEvent               @event,
                                          EventHandlerDelegate next,
                                          CancellationToken    cancellationToken = default);
}
