using Application.Domain.Entities;
using Application.Domain.Repositories;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Mediator.Abstractions;

namespace Application.Application.Todos;

public sealed class TodoQueryHandler : IRequestHandler<TodoQuery, Todo>
{
    private readonly ITodoRepository _todoRepository;

    public TodoQueryHandler(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }

    public async ValueTask<Result<Todo>> HandleAsync(TodoQuery request,
                                                     CancellationToken cancellationToken = default)
    {
        var todoOpt = await _todoRepository.GetAsync(request.Id, cancellationToken);

        return todoOpt.Some(out var todo)
                   ? Result.Success(todo)
                   : Result.Failure<Todo>(new Exception("Not found"));
    }
}
