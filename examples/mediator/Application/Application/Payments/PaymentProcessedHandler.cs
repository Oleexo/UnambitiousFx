using Application.Domain.Events;
using Microsoft.Extensions.Logging;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Mediator.Abstractions;

namespace Application.Application.Payments;

/// <summary>
/// Handler for external PaymentProcessed events received from transport
/// </summary>
[EventHandler<PaymentProcessed>]
public sealed class PaymentProcessedHandler : IEventHandler<PaymentProcessed>
{
    private readonly ILogger<PaymentProcessedHandler> _logger;

    public PaymentProcessedHandler(ILogger<PaymentProcessedHandler> logger)
    {
        _logger = logger;
    }

    public ValueTask<Result> HandleAsync(PaymentProcessed @event, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Payment {PaymentId} processed for order {OrderId}: {Amount:C} via {PaymentMethod}",
            @event.PaymentId,
            @event.OrderId,
            @event.Amount,
            @event.PaymentMethod);

        return ValueTask.FromResult(Result.Success());
    }
}
