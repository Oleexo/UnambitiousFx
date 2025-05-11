using Common.Domain.Events;
using Microsoft.Extensions.Logging;
using UnambitiousFx.Core;
using UnambitiousFx.Mediator;
using UnambitiousFx.Mediator.Abstractions;

namespace Common.Application.Todos;

[EventHandler<TodoDeleted>]
public sealed class TodoDeletedHandler : IEventHandler<IAppContext, TodoDeleted> {
    private readonly ILogger<TodoDeletedHandler> _logger;

    public TodoDeletedHandler(ILogger<TodoDeletedHandler> logger) {
        _logger = logger;
    }

    public ValueTask<Result> HandleAsync(IAppContext       context,
                                         TodoDeleted       @event,
                                         CancellationToken cancellationToken = default) {
        _logger.LogInformation("Todo deleted {TodoId}", @event.Todo.Id);

        return ValueTask.FromResult(Result.Success());
    }
}
