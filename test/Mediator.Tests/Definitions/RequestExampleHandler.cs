using Oleexo.UnambitiousFx.Core;
using Oleexo.UnambitiousFx.Mediator.Abstractions;

namespace Oleexo.UnambitiousFx.Mediator.Tests.Definitions;

public sealed class RequestExampleHandler : IRequestHandler<RequestExample, int> {
    public bool            Executed        { get; private set; }
    public RequestExample? RequestExecuted { get; private set; }
    public int             ExecutionCount  { get; private set; }
    public Action?         OnExecuted      { get; set; }

    public ValueTask<Result<int>> HandleAsync(IContext          ctx,
                                              RequestExample    request,
                                              CancellationToken cancellationToken = default) {
        Executed        = true;
        RequestExecuted = request;
        ExecutionCount++;
        OnExecuted?.Invoke();
        return new ValueTask<Result<int>>(Result<int>.Success(0));
    }
}
