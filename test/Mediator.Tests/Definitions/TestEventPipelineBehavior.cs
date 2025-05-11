using UnambitiousFx.Core;
using UnambitiousFx.Mediator.Abstractions;

namespace UnambitiousFx.Mediator.Tests.Definitions;

public sealed class TestEventPipelineBehavior : IEventPipelineBehavior {
    public bool    Executed       { get; private set; }
    public object? EventExecuted  { get; private set; }
    public int     ExecutionCount { get; private set; }
    public Action? OnExecuted     { get; set; }

    public ValueTask<Result> HandleAsync<TContext, TEvent>(TContext             context,
                                                           TEvent               @event,
                                                           EventHandlerDelegate next,
                                                           CancellationToken    cancellationToken = default)
        where TEvent : IEvent
        where TContext : IContext {
        Executed      = true;
        EventExecuted = @event;
        ExecutionCount++;
        OnExecuted?.Invoke();
        return next();
    }
}
