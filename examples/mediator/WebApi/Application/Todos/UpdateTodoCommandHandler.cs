using Oleexo.UnambitiousFx.Core;
using Oleexo.UnambitiousFx.Mediator.Abstractions;
using WebApi.Domain.Repositories;
using IResult = Oleexo.UnambitiousFx.Core.Abstractions.IResult;

namespace WebApi.Application.Todos;

public sealed class UpdateTodoCommandHandler : IRequestHandler<UpdateTodoCommand> {
    private readonly ITodoRepository _todoRepository;

    public UpdateTodoCommandHandler(ITodoRepository todoRepository) {
        _todoRepository = todoRepository;
    }

    public async ValueTask<IResult> HandleAsync(IContext          context,
                                                UpdateTodoCommand request,
                                                CancellationToken cancellationToken = default) {
        var todoOpt = await _todoRepository.GetAsync(request.Id, cancellationToken);

        if (!todoOpt.Some(out var todo)) {
            return Result.Failure(new Error("Not found"));
        }

        todo.Name = request.Name;

        await _todoRepository.UpdateAsync(todo, cancellationToken);

        return Result.Success();
    }
}
