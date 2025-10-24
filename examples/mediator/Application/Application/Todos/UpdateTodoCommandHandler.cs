using Application.Domain.Events;
using Application.Domain.Repositories;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Mediator.Abstractions;
using UnambitiousFx.Mediator;

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
            return Result.Failure("Not found");
        }

        todo.Name = request.Name;

        await _todoRepository.UpdateAsync(todo, cancellationToken);
        await context.PublishEventAsync(new TodoUpdated {
            Todo = todo
        }, cancellationToken);

        return Result.Success();
    }
}
