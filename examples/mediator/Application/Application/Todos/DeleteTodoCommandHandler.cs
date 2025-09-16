using Application.Domain.Events;
using Application.Domain.Repositories;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Mediator.Abstractions;

namespace Application.Application.Todos;

public sealed class DeleteTodoCommandHandler : IRequestHandler<DeleteTodoCommand> {
    private readonly ITodoRepository _todoRepository;

    public DeleteTodoCommandHandler(ITodoRepository todoRepository) {
        _todoRepository = todoRepository;
    }

    public async ValueTask<Result> HandleAsync(IContext          context,
                                               DeleteTodoCommand request,
                                               CancellationToken cancellationToken = default) {
        var todoOpt = await _todoRepository.GetAsync(request.Id, cancellationToken);

        if (!todoOpt.Some(out var todo)) {
            return Result.Failure("Not found");
        }

        await _todoRepository.DeleteAsync(todo.Id, cancellationToken);

        await context.PublishEventAsync(new TodoDeleted {
            Todo = todo
        }, cancellationToken);

        return Result.Success();
    }
}
