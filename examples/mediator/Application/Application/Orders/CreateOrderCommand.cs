using UnambitiousFx.Mediator.Abstractions;

namespace Application.Application.Orders;

[RequestHandler<CreateOrderCommandHandler, CreateOrderCommand, Guid>]
public sealed record CreateOrderCommand : IRequest<Guid>
{
    public required string CustomerName { get; init; }
    public required decimal TotalAmount { get; init; }
}
