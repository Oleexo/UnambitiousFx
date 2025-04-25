using UnambitiousFx.Mediator.Abstractions;

namespace Application.Application.Todos;

public sealed record CreateTodoCommand : IRequest<Guid> {
    public required string Name { get; init; }
}
