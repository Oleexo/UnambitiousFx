using UnambitiousFx.Core;
using UnambitiousFx.Mediator.Abstractions;

namespace UnambitiousFx.Mediator.Tests.Definitions;

public sealed class EventExampleHandler2 : IEventHandler<ITestContext, EventExample> {
    public bool          Executed       { get; private set; }
    public EventExample? EventExecuted  { get; private set; }
    public int           ExecutionCount { get; private set; }
    public Action?       OnExecuted     { get; set; }

    public ValueTask<Result> HandleAsync(ITestContext      context,
                                         EventExample      @event,
                                         CancellationToken cancellationToken = default) {
        Executed      = true;
        EventExecuted = @event;
        ExecutionCount++;
        OnExecuted?.Invoke();
        return new ValueTask<Result>(Result.Success());
    }
}
