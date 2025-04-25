using Application.Domain.Events;
using Microsoft.Extensions.Logging;
using UnambitiousFx.Core;
using UnambitiousFx.Mediator.Abstractions;

namespace Application.Application.Todos;

public sealed class TodoDeletedHandler : IEventHandler<TodoDeleted> {
    private readonly ILogger<TodoDeletedHandler> _logger;

    public TodoDeletedHandler(ILogger<TodoDeletedHandler> logger) {
        _logger = logger;
    }

    public ValueTask<Result> HandleAsync(IContext          context,
                                         TodoDeleted       @event,
                                         CancellationToken cancellationToken = default) {
        _logger.LogInformation("Todo deleted {TodoId}", @event.Todo.Id);

        return ValueTask.FromResult(Result.Success());
    }
}
