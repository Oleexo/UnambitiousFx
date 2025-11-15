using UnambitiousFx.Mediator.Abstractions;

namespace Application.Application.Todos;

public record DeleteTodoCommand : IRequest
{
    public required Guid Id { get; init; }
}
