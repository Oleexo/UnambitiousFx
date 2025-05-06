namespace Common.Application.Todos;

public record DeleteTodoCommand : ICommand {
    public required Guid Id { get; init; }
}
