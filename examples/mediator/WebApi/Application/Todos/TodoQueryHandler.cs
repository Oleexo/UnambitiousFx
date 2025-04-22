using Oleexo.UnambitiousFx.Core;
using Oleexo.UnambitiousFx.Core.Abstractions;
using Oleexo.UnambitiousFx.Mediator.Abstractions;
using WebApi.Domain.Entities;
using WebApi.Domain.Repositories;

namespace WebApi.Application.Todos;

public sealed class TodoQueryHandler : IRequestHandler<TodoQuery, Todo> {
    private readonly ITodoRepository _todoRepository;

    public TodoQueryHandler(ITodoRepository todoRepository) {
        _todoRepository = todoRepository;
    }

    public async ValueTask<IResult<Todo>> HandleAsync(IContext          context,
                                                      TodoQuery         request,
                                                      CancellationToken cancellationToken = default) {
        var todoOpt = await _todoRepository.GetAsync(request.Id, cancellationToken);

        return todoOpt.Some(out var todo)
                   ? Result<Todo>.Success(todo)
                   : Result<Todo>.Failure(new Error("Not found"));
    }
}
