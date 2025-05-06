using Common.Domain.Events;
using Microsoft.Extensions.Logging;
using UnambitiousFx.Core;
using UnambitiousFx.Mediator;
using UnambitiousFx.Mediator.Abstractions;

namespace Common.Application.Todos;

[EventHandler<TodoUpdated>]
public sealed class TodoUpdatedHandler : IEventHandler<IAppContext, TodoUpdated> {
    private readonly ILogger<TodoUpdatedHandler> _logger;

    public TodoUpdatedHandler(ILogger<TodoUpdatedHandler> logger) {
        _logger = logger;
    }

    public ValueTask<Result> HandleAsync(IAppContext       context,
                                         TodoUpdated       @event,
                                         CancellationToken cancellationToken = default) {
        _logger.LogInformation("Todo updated {TodoId}", @event.Todo.Id);

        return ValueTask.FromResult(Result.Success());
    }
}
