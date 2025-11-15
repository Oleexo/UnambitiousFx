using UnambitiousFx.Mediator.Abstractions;

namespace Application.Application.Payments;

[RequestHandler<ProcessPaymentCommandHandler, ProcessPaymentCommand>]
public sealed record ProcessPaymentCommand : IRequest
{
    public required Guid OrderId { get; init; }
    public required decimal Amount { get; init; }
    public required string PaymentMethod { get; init; }
}
