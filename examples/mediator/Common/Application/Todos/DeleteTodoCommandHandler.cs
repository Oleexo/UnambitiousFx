using Common.Domain.Events;
using Common.Domain.Repositories;
using UnambitiousFx.Core;
using UnambitiousFx.Mediator.Abstractions;

namespace Common.Application.Todos;

public sealed class DeleteTodoCommandHandler : ICommandHandler<DeleteTodoCommand> {
    private readonly IPublisher<IAppContext> _publisher;
    private readonly ITodoRepository         _todoRepository;

    public DeleteTodoCommandHandler(ITodoRepository         todoRepository,
                                    IPublisher<IAppContext> publisher) {
        _todoRepository = todoRepository;
        _publisher      = publisher;
    }

    public async ValueTask<Result> HandleAsync(IAppContext       context,
                                               DeleteTodoCommand request,
                                               CancellationToken cancellationToken = default) {
        var todoOpt = await _todoRepository.GetAsync(request.Id, cancellationToken);

        if (!todoOpt.Some(out var todo)) {
            return Result.Failure(new Error("Not found"));
        }

        await _todoRepository.DeleteAsync(todo.Id, cancellationToken);

        await _publisher.PublishAsync(context, new TodoDeleted {
            Todo = todo
        }, cancellationToken);

        return Result.Success();
    }
}
