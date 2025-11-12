using Application.Domain.Events;
using Microsoft.Extensions.Logging;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Mediator.Abstractions;

namespace Application.Application.Orders;

/// <summary>
/// Local handler - processes order creation within the same process
/// </summary>
[EventHandler<OrderCreated>]
public sealed class OrderCreatedHandler : IEventHandler<OrderCreated>
{
    private readonly ILogger<OrderCreatedHandler> _logger;

    public OrderCreatedHandler(ILogger<OrderCreatedHandler> logger)
    {
        _logger = logger;
    }

    public ValueTask<Result> HandleAsync(OrderCreated @event, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Order {OrderId} created for customer {CustomerName} with total {TotalAmount:C}",
            @event.OrderId,
            @event.CustomerName,
            @event.TotalAmount);

        return ValueTask.FromResult(Result.Success());
    }
}
