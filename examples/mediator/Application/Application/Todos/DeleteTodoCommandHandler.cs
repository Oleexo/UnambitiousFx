using Application.Domain.Repositories;
using Oleexo.UnambitiousFx.Core;
using Oleexo.UnambitiousFx.Mediator.Abstractions;

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
            return Result.Failure(new Error("Not found"));
        }

        await _todoRepository.DeleteAsync(todo.Id, cancellationToken);

        return Result.Success();
    }
}
