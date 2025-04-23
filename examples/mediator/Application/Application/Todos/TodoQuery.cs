using Application.Domain.Entities;
using Oleexo.UnambitiousFx.Mediator.Abstractions;

namespace Application.Application.Todos;

public sealed record TodoQuery : IRequest<Todo> {
    public required Guid Id { get; init; }
}
