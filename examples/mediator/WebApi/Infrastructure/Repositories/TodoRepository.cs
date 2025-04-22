using Oleexo.UnambitiousFx.Core;
using Oleexo.UnambitiousFx.Core.Abstractions;
using WebApi.Domain.Entities;
using WebApi.Domain.Repositories;

namespace WebApi.Infrastructure.Repositories;

public class TodoRepository : ITodoRepository {
    private readonly Dictionary<Guid, Todo> _todos = new();

    public ValueTask CreateAsync(Todo              todo,
                                 CancellationToken cancellationToken = default) {
        _todos.Add(todo.Id, todo);
        return ValueTask.CompletedTask;
    }

    public ValueTask UpdateAsync(Todo              todo,
                                 CancellationToken cancellationToken = default) {
        _todos[todo.Id] = todo;
        return ValueTask.CompletedTask;
    }

    public ValueTask<IOption<Todo>> GetAsync(Guid              id,
                                             CancellationToken cancellationToken = default) {
        if (_todos.TryGetValue(id, out var todo)) {
            return ValueTask.FromResult<IOption<Todo>>(Option<Todo>.Some(todo));
        }

        return ValueTask.FromResult<IOption<Todo>>(Option<Todo>.None);
    }

    public ValueTask<IEnumerable<Todo>> GetAllAsync(CancellationToken cancellationToken = default) {
        return ValueTask.FromResult<IEnumerable<Todo>>(_todos.Values);
    }

    public ValueTask DeleteAsync(Guid              id,
                                 CancellationToken cancellationToken) {
        _todos.Remove(id);

        return ValueTask.CompletedTask;
    }
}
