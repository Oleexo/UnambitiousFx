using Microsoft.Extensions.Logging;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Examples.ConsoleApp.Events;
using UnambitiousFx.Mediator.Abstractions;

namespace UnambitiousFx.Examples.ConsoleApp.Handlers.Events;

[EventHandler<OrderShippedEvent>]
public sealed class UpdateInventoryHandler : IEventHandler<OrderShippedEvent> {
    private readonly ILogger<UpdateInventoryHandler> _logger;

    public UpdateInventoryHandler(ILogger<UpdateInventoryHandler> logger) {
        _logger = logger;
    }

    public ValueTask<Result> HandleAsync(OrderShippedEvent @event,
                                         CancellationToken cancellationToken = default) {
        _logger.LogDebug("Updating inventory for shipped order: {OrderId}", @event.OrderId);
        return ValueTask.FromResult(Result.Success());
    }
}
