namespace Common.Application.Todos;

public record UpdateTodoCommand : ICommand {
    public required Guid   Id   { get; init; }
    public required string Name { get; init; }
}
