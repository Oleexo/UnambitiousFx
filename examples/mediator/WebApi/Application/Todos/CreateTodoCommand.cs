using Oleexo.UnambitiousFx.Mediator.Abstractions;

namespace WebApi.Application.Todos;

public sealed record CreateTodoCommand : IRequest<Guid> {
    public required string Name { get; init; }
}
