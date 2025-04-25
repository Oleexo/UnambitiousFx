using Application.Domain.Events;
using Microsoft.Extensions.Logging;
using UnambitiousFx.Core;
using UnambitiousFx.Mediator.Abstractions;

namespace Application.Application.Todos;

public sealed class TodoUpdatedHandler : IEventHandler<TodoUpdated> {
    private readonly ILogger<TodoUpdatedHandler> _logger;

    public TodoUpdatedHandler(ILogger<TodoUpdatedHandler> logger) {
        _logger = logger;
    }

    public ValueTask<Result> HandleAsync(IContext          context,
                                         TodoUpdated       @event,
                                         CancellationToken cancellationToken = default) {
        _logger.LogInformation("Todo updated {TodoId}", @event.Todo.Id);

        return ValueTask.FromResult(Result.Success());
    }
}
