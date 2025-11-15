using Application.Domain.Events;
using Microsoft.Extensions.Logging;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Mediator.Abstractions;

namespace Application.Application.Inventory;

public sealed class UpdateInventoryCommandHandler : IRequestHandler<UpdateInventoryCommand>
{
    private readonly ILogger<UpdateInventoryCommandHandler> _logger;
    private readonly IPublisher _publisher;

    public UpdateInventoryCommandHandler(ILogger<UpdateInventoryCommandHandler> logger, IPublisher publisher)
    {
        _logger = logger;
        _publisher = publisher;
    }

    public async ValueTask<Result> HandleAsync(UpdateInventoryCommand request, CancellationToken cancellationToken = default)
    {
        // Simulate current inventory (in real app, would query from database)
        var currentQuantity = 100;
        var newQuantity = currentQuantity + request.QuantityChange;
        
        _logger.LogInformation("Updating inventory for product {ProductId}: {QuantityChange:+#;-#;0}", 
            request.ProductId, request.QuantityChange);

        // Publish EXTERNAL event - will be sent through transport layer
        await _publisher.PublishAsync(new InventoryUpdated
        {
            ProductId = request.ProductId,
            QuantityChange = request.QuantityChange,
            NewQuantity = newQuantity,
            UpdatedAt = DateTime.UtcNow
        }, cancellationToken);

        return Result.Success();
    }
}
