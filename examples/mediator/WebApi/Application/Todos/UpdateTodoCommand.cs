using Oleexo.UnambitiousFx.Mediator.Abstractions;

namespace WebApi.Application.Todos;

public record UpdateTodoCommand : IRequest {
    public required Guid   Id   { get; init; }
    public required string Name { get; init; }
}
