using Oleexo.UnambitiousFx.Core;
using Oleexo.UnambitiousFx.Mediator.Abstractions;
using WebApi.Domain.Repositories;
using IResult = Oleexo.UnambitiousFx.Core.Abstractions.IResult;

namespace WebApi.Application.Todos;

public sealed class DeleteTodoCommandHandler : IRequestHandler<DeleteTodoCommand> {
    private readonly ITodoRepository _todoRepository;

    public DeleteTodoCommandHandler(ITodoRepository todoRepository) {
        _todoRepository = todoRepository;
    }

    public async ValueTask<IResult> HandleAsync(IContext          context,
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
