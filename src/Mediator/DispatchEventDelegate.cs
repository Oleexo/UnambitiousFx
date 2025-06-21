using UnambitiousFx.Core;
using UnambitiousFx.Mediator.Abstractions;

namespace UnambitiousFx.Mediator;

internal delegate ValueTask<Result> DispatchEventDelegate(IContext          context,
                                                          IEvent            @event,
                                                          IEventDispatcher  dispatcher,
                                                          CancellationToken cancellationToken);
