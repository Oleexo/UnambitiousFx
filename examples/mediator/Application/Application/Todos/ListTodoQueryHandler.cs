using Application.Domain.Entities;
using Application.Domain.Repositories;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Mediator.Abstractions;

namespace Application.Application.Todos;

[RequestHandler<ListTodoQuery, IEnumerable<Todo>>]
public sealed class ListTodoQueryHandler : IRequestHandler<ListTodoQuery, IEnumerable<Todo>>
{
    private readonly ITodoRepository _todoRepository;

    public ListTodoQueryHandler(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }

    public async ValueTask<Result<IEnumerable<Todo>>> HandleAsync(ListTodoQuery request,
                                                                  CancellationToken cancellationToken = default)
    {
        var todos = await _todoRepository.GetAllAsync(cancellationToken);

        return Result.Success(todos);
    }
}
