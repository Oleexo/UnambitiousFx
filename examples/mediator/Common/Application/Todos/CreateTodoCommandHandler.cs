using Common.Domain.Entities;
using Common.Domain.Events;
using Common.Domain.Repositories;
using UnambitiousFx.Core;
using UnambitiousFx.Mediator;
using UnambitiousFx.Mediator.Abstractions;

namespace Common.Application.Todos;

[RequestHandler<CreateTodoCommand, Guid>]
public sealed class CreateTodoCommandHandler : ICommandHandler<CreateTodoCommand, Guid> {
    private readonly IPublisher<IAppContext> _publisher;
    private readonly ITodoRepository         _todoRepository;

    public CreateTodoCommandHandler(ITodoRepository         todoRepository,
                                    IPublisher<IAppContext> publisher) {
        _todoRepository = todoRepository;
        _publisher      = publisher;
    }

    public async ValueTask<Result<Guid>> HandleAsync(IAppContext       context,
                                                     CreateTodoCommand request,
                                                     CancellationToken cancellationToken = default) {
        var todo = new Todo {
            Id   = Guid.CreateVersion7(),
            Name = request.Name
        };

        await _todoRepository.CreateAsync(todo, cancellationToken);

        await _publisher.PublishAsync(context, new TodoCreated {
            Todo = todo
        }, cancellationToken);
        return Result<Guid>.Success(todo.Id);
    }
}
