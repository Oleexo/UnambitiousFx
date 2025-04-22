using Oleexo.UnambitiousFx.Mediator.Abstractions;
using WebApi.Domain.Entities;

namespace WebApi.Application.Todos;

public record TodoQuery : IRequest<Todo> {
    public required Guid Id { get; init; }
}
