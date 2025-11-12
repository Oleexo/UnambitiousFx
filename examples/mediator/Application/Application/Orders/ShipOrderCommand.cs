using UnambitiousFx.Mediator.Abstractions;

namespace Application.Application.Orders;

[RequestHandler<ShipOrderCommandHandler, ShipOrderCommand>]
public sealed record ShipOrderCommand : IRequest
{
    public required Guid OrderId { get; init; }
}
