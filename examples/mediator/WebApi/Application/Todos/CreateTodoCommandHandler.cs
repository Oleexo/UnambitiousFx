using Oleexo.UnambitiousFx.Core;
using Oleexo.UnambitiousFx.Core.Abstractions;
using Oleexo.UnambitiousFx.Mediator.Abstractions;
using WebApi.Domain.Entities;
using WebApi.Domain.Repositories;

namespace WebApi.Application.Todos;

public sealed class CreateTodoCommandHandler : IRequestHandler<CreateTodoCommand, Guid> {
    private readonly ITodoRepository _todoRepository;

    public CreateTodoCommandHandler(ITodoRepository todoRepository) {
        _todoRepository = todoRepository;
    }

    public async ValueTask<IResult<Guid>> HandleAsync(IContext          context,
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
