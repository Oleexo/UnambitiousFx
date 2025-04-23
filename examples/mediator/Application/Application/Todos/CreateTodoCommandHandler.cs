using Application.Domain.Entities;
using Application.Domain.Repositories;
using Oleexo.UnambitiousFx.Core;
using Oleexo.UnambitiousFx.Mediator.Abstractions;

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

        return Result<Guid>.Success(todo.Id);
    }
}
