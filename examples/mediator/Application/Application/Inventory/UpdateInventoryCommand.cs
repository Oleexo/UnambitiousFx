using UnambitiousFx.Mediator.Abstractions;

namespace Application.Application.Inventory;

[RequestHandler<UpdateInventoryCommandHandler, UpdateInventoryCommand>]
public sealed record UpdateInventoryCommand : IRequest
{
    public required Guid ProductId { get; init; }
    public required int QuantityChange { get; init; }
}
