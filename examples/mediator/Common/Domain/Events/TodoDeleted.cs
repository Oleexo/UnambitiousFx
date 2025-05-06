using Common.Domain.Entities;
using UnambitiousFx.Mediator.Abstractions;

namespace Common.Domain.Events;

public sealed record TodoDeleted : IEvent {
    public required Todo Todo { get; init; }
}
