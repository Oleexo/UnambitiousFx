using Common.Domain.Entities;

namespace Common.Application.Todos;

public record ListTodoQuery : IQuery<IEnumerable<Todo>>;
