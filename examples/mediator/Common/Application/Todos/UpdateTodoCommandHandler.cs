using Common.Domain.Events;
using Common.Domain.Repositories;
using UnambitiousFx.Core;
using UnambitiousFx.Mediator;
using UnambitiousFx.Mediator.Abstractions;

namespace Common.Application.Todos;

[RequestHandler<UpdateTodoCommand>]
public sealed class UpdateTodoCommandHandler : ICommandHandler<UpdateTodoCommand> {
    private readonly IPublisher<IAppContext> _publisher;
    private readonly ITodoRepository         _todoRepository;

    public UpdateTodoCommandHandler(ITodoRepository         todoRepository,
                                    IPublisher<IAppContext> publisher) {
        _todoRepository = todoRepository;
        _publisher      = publisher;
    }

    public async ValueTask<Result> HandleAsync(IAppContext       context,
                                               UpdateTodoCommand request,
                                               CancellationToken cancellationToken = default) {
        var todoOpt = await _todoRepository.GetAsync(request.Id, cancellationToken);

        if (!todoOpt.Some(out var todo)) {
            return Result.Failure(new Error("Not found"));
        }

        todo.Name = request.Name;

        await _todoRepository.UpdateAsync(todo, cancellationToken);
        await _publisher.PublishAsync(context, new TodoUpdated {
            Todo = todo
        }, cancellationToken);

        return Result.Success();
    }
}
