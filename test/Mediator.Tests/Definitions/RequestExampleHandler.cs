using UnambitiousFx.Core.Results;
using UnambitiousFx.Mediator.Abstractions;

namespace UnambitiousFx.Mediator.Tests.Definitions;

public sealed class RequestExampleHandler : IRequestHandler<RequestExample> {
    public bool            Executed        { get; private set; }
    public RequestExample? RequestExecuted { get; private set; }
    public int             ExecutionCount  { get; private set; }
    public Action?         OnExecuted      { get; set; }

    public ValueTask<Result> HandleAsync(IContext          context,
                                         RequestExample    request,
                                         CancellationToken cancellationToken = default) {
        Executed        = true;
        RequestExecuted = request;
        ExecutionCount++;
        OnExecuted?.Invoke();
        return new ValueTask<Result>(Result.Success());
    }
}
