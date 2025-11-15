using Application.Domain.Events;
using Microsoft.Extensions.Logging;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Mediator.Abstractions;

namespace Application.Application.Inventory;

/// <summary>
/// Handler for external InventoryUpdated events received from transport
/// </summary>
[EventHandler<InventoryUpdated>]
public sealed class InventoryUpdatedHandler : IEventHandler<InventoryUpdated>
{
    private readonly ILogger<InventoryUpdatedHandler> _logger;

    public InventoryUpdatedHandler(ILogger<InventoryUpdatedHandler> logger)
    {
        _logger = logger;
    }

    public ValueTask<Result> HandleAsync(InventoryUpdated @event, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Inventory updated for product {ProductId}: {QuantityChange:+#;-#;0} (New quantity: {NewQuantity})",
            @event.ProductId,
            @event.QuantityChange,
            @event.NewQuantity);

        return ValueTask.FromResult(Result.Success());
    }
}
