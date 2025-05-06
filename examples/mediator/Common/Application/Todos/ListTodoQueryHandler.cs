using Common.Domain.Entities;
using Common.Domain.Repositories;
using UnambitiousFx.Core;
using UnambitiousFx.Mediator;

namespace Common.Application.Todos;

[RequestHandler<ListTodoQuery, IEnumerable<Todo>>]
public sealed class ListTodoQueryHandler : IQueryHandler<ListTodoQuery, IEnumerable<Todo>> {
    private readonly ITodoRepository _todoRepository;

    public ListTodoQueryHandler(ITodoRepository todoRepository) {
        _todoRepository = todoRepository;
    }

    public async ValueTask<Result<IEnumerable<Todo>>> HandleAsync(IAppContext       context,
                                                                  ListTodoQuery     request,
                                                                  CancellationToken cancellationToken = default) {
        var todos = await _todoRepository.GetAllAsync(cancellationToken);

        return Result<IEnumerable<Todo>>.Success(todos);
    }
}
