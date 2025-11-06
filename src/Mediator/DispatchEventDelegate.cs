using UnambitiousFx.Core.Results;
using UnambitiousFx.Mediator.Abstractions;

namespace UnambitiousFx.Mediator;

internal delegate ValueTask<Result> DispatchEventDelegate(IEvent            @event,
                                                          IEventDispatcher  dispatcher,
                                                          CancellationToken cancellationToken);
