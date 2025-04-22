using Oleexo.UnambitiousFx.Mediator.Abstractions;

namespace WebApi.Application.Todos;

public record DeleteTodoCommand : IRequest {
    public required Guid Id { get; init; }
}
