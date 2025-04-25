using Application.Domain.Events;
using Application.Domain.Repositories;
using UnambitiousFx.Core;
using UnambitiousFx.Mediator;
using UnambitiousFx.Mediator.Abstractions;

namespace Application.Application.Todos;

[RequestHandler<UpdateTodoCommand>]
public sealed class UpdateTodoCommandHandler : IRequestHandler<UpdateTodoCommand> {
    private readonly ITodoRepository _todoRepository;

    public UpdateTodoCommandHandler(ITodoRepository todoRepository) {
        _todoRepository = todoRepository;
    }

    public async ValueTask<Result> HandleAsync(IContext          context,
                                               UpdateTodoCommand request,
                                               CancellationToken cancellationToken = default) {
        var todoOpt = await _todoRepository.GetAsync(request.Id, cancellationToken);

        if (!todoOpt.Some(out var todo)) {
            return Result.Failure(new Error("Not found"));
        }

        todo.Name = request.Name;

        await _todoRepository.UpdateAsync(todo, cancellationToken);
        await context.PublishAsync(new TodoUpdated {
            Todo = todo
        }, cancellationToken);

        return Result.Success();
    }
}
