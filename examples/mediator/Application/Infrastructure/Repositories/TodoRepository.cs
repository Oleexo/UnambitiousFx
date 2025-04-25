using Application.Domain.Entities;
using Application.Domain.Repositories;
using Oleexo.UnambitiousFx.Core;

namespace Application.Infrastructure.Repositories;

public sealed class TodoRepository : ITodoRepository {
    private static readonly Dictionary<Guid, Todo> Todos = new();

    public ValueTask CreateAsync(Todo              todo,
                                 CancellationToken cancellationToken = default) {
        Todos.Add(todo.Id, todo);
        return ValueTask.CompletedTask;
    }

    public ValueTask UpdateAsync(Todo              todo,
                                 CancellationToken cancellationToken = default) {
        Todos[todo.Id] = todo;
        return ValueTask.CompletedTask;
    }

    public ValueTask<Option<Todo>> GetAsync(Guid              id,
                                            CancellationToken cancellationToken = default) {
        if (Todos.TryGetValue(id, out var todo)) {
            return ValueTask.FromResult(Option<Todo>.Some(todo));
        }

        return ValueTask.FromResult(Option<Todo>.None());
    }

    public ValueTask<IEnumerable<Todo>> GetAllAsync(CancellationToken cancellationToken = default) {
        return ValueTask.FromResult<IEnumerable<Todo>>(Todos.Values);
    }

    public ValueTask DeleteAsync(Guid              id,
                                 CancellationToken cancellationToken) {
        Todos.Remove(id);

        return ValueTask.CompletedTask;
    }
}
