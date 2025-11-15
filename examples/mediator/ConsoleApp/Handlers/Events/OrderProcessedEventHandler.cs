using Microsoft.Extensions.Logging;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Examples.ConsoleApp.Events;
using UnambitiousFx.Mediator.Abstractions;

namespace UnambitiousFx.Examples.ConsoleApp.Handlers.Events;

[EventHandler<OrderProcessedEvent>]
public class OrderProcessedEventHandler : IEventHandler<OrderProcessedEvent>
{
    private readonly ILogger<OrderProcessedEventHandler> _logger;

    public OrderProcessedEventHandler(ILogger<OrderProcessedEventHandler> logger)
    {
        _logger = logger;
    }

    public ValueTask<Result> HandleAsync(OrderProcessedEvent @event,
                                         CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Order processed event received for OrderId: {OrderId} at {ProcessedAt}", @event.OrderId, @event.ProcessedAt);
        return ValueTask.FromResult(Result.Success());
    }
}
