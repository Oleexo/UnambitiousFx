using Oleexo.UnambitiousFx.Core;
using Oleexo.UnambitiousFx.Core.Abstractions;
using Oleexo.UnambitiousFx.Mediator.Abstractions;
using WebApi.Domain.Entities;
using WebApi.Domain.Repositories;

namespace WebApi.Application.Todos;

public sealed class ListTodoQueryHandler : IRequestHandler<ListTodoQuery, IEnumerable<Todo>> {
    private readonly ITodoRepository _todoRepository;

    public ListTodoQueryHandler(ITodoRepository todoRepository) {
        _todoRepository = todoRepository;
    }

    public async ValueTask<IResult<IEnumerable<Todo>>> HandleAsync(IContext          context,
                                                                   ListTodoQuery     request,
                                                                   CancellationToken cancellationToken = default) {
        var todos = await _todoRepository.GetAllAsync(cancellationToken);

        return Result<IEnumerable<Todo>>.Success(todos);
    }
}
