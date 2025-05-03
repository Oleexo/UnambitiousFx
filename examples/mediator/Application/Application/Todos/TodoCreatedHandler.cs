using Application.Domain.Events;
using Microsoft.Extensions.Logging;
using UnambitiousFx.Core;
using UnambitiousFx.Mediator.Abstractions;

namespace Application.Application.Todos;

public sealed class TodoCreatedHandler : IEventHandler<TodoCreated> {
    private readonly ILogger<TodoCreatedHandler> _logger;

    public TodoCreatedHandler(ILogger<TodoCreatedHandler> logger) {
        _logger = logger;
    }

    public ValueTask<Result> HandleAsync(IContext          context,
                                         TodoCreated       @event,
                                         CancellationToken cancellationToken = default) {
        _logger.LogInformation("New todo created {TodoId}", @event.Todo.Id);

        return ValueTask.FromResult(Result.Success());
    }
}
