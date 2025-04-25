using Application.Domain.Entities;
using Application.Domain.Events;
using Application.Domain.Repositories;
using UnambitiousFx.Core;
using UnambitiousFx.Mediator.Abstractions;

namespace Application.Application.Todos;

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

        await context.PublishAsync(new TodoCreated {
            Todo = todo
        }, cancellationToken);
        return Result<Guid>.Success(todo.Id);
    }
}
