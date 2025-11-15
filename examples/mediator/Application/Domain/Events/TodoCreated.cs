using Application.Domain.Entities;
using UnambitiousFx.Mediator.Abstractions;

namespace Application.Domain.Events;

public sealed record TodoCreated : IEvent
{
    public required Todo Todo { get; init; }
}
