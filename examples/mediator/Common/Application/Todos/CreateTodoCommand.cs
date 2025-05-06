namespace Common.Application.Todos;

public sealed record CreateTodoCommand : ICommand<Guid> {
    public required string Name { get; init; }
}
