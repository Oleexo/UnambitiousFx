using UnambitiousFx.Mediator.Abstractions;

namespace UnambitiousFx.Examples.ConsoleApp.Events;

public sealed record PaymentCompletedEvent : IEvent {
    public required Guid     PaymentId   { get; init; }
    public required decimal  Amount      { get; init; }
    public required DateTime CompletedAt { get; init; }
}
