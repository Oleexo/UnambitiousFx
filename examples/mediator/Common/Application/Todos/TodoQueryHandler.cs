using Common.Domain.Entities;
using Common.Domain.Repositories;
using UnambitiousFx.Core;

namespace Common.Application.Todos;

public sealed class TodoQueryHandler : IQueryHandler<TodoQuery, Todo> {
    private readonly ITodoRepository _todoRepository;

    public TodoQueryHandler(ITodoRepository todoRepository) {
        _todoRepository = todoRepository;
    }

    public async ValueTask<Result<Todo>> HandleAsync(IAppContext       context,
                                                     TodoQuery         request,
                                                     CancellationToken cancellationToken = default) {
        var todoOpt = await _todoRepository.GetAsync(request.Id, cancellationToken);

        return todoOpt.Some(out var todo)
                   ? Result<Todo>.Success(todo)
                   : Result<Todo>.Failure(new Error("Not found"));
    }
}
