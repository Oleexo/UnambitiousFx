using Application.Domain.Events;
using Microsoft.Extensions.Logging;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Mediator.Abstractions;

namespace Application.Application.Payments;

public sealed class ProcessPaymentCommandHandler : IRequestHandler<ProcessPaymentCommand>
{
    private readonly ILogger<ProcessPaymentCommandHandler> _logger;
    private readonly IPublisher _publisher;

    public ProcessPaymentCommandHandler(ILogger<ProcessPaymentCommandHandler> logger, IPublisher publisher)
    {
        _logger = logger;
        _publisher = publisher;
    }

    public async ValueTask<Result> HandleAsync(ProcessPaymentCommand request, CancellationToken cancellationToken = default)
    {
        var paymentId = Guid.NewGuid();
        
        _logger.LogInformation("Processing payment {PaymentId} for order {OrderId}", paymentId, request.OrderId);

        // Publish EXTERNAL event - will be sent through transport layer
        await _publisher.PublishAsync(new PaymentProcessed
        {
            PaymentId = paymentId,
            OrderId = request.OrderId,
            Amount = request.Amount,
            PaymentMethod = request.PaymentMethod,
            ProcessedAt = DateTime.UtcNow
        }, cancellationToken);

        return Result.Success();
    }
}
