using Application.Domain.Entities;
using Application.Domain.Events;
using Application.Domain.Repositories;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Mediator.Abstractions;
using UnambitiousFx.Mediator;

namespace Application.Application.Todos;

[RequestHandler<CreateTodoCommand, Guid>]
public sealed class CreateTodoCommandHandler : IRequestHandler<CreateTodoCommand, Guid> {
    private readonly ITodoRepository _todoRepository;

    public CreateTodoCommandHandler(ITodoRepository todoRepository) {
        _todoRepository = todoRepository;
    }

    public async ValueTask<Result<Guid>> HandleAsync(IContext          context,
                                                     CreateTodoCommand request,
                                                     CancellationToken cancellationToken = default) {
        var todo = new Todo {
            Id   = Guid.CreateVersion7(),
            Name = request.Name
        };

        await _todoRepository.CreateAsync(todo, cancellationToken);

        await context.PublishEventAsync(new TodoCreated {
            Todo = todo
        }, cancellationToken);
        return Result.Success(todo.Id);
    }
}
