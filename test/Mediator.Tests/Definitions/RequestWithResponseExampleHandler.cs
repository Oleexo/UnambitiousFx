using UnambitiousFx.Core.Results;
using UnambitiousFx.Mediator.Abstractions;

namespace UnambitiousFx.Mediator.Tests.Definitions;

public sealed class RequestWithResponseExampleHandler : IRequestHandler<RequestWithResponseExample, int> {
    public bool                        Executed        { get; private set; }
    public RequestWithResponseExample? RequestExecuted { get; private set; }
    public int                         ExecutionCount  { get; private set; }
    public Action?                     OnExecuted      { get; set; }

    public ValueTask<Result<int>> HandleAsync(IContext                   ctx,
                                              RequestWithResponseExample request,
                                              CancellationToken          cancellationToken = default) {
        Executed        = true;
        RequestExecuted = request;
        ExecutionCount++;
        OnExecuted?.Invoke();
        return new ValueTask<Result<int>>(Result.Success(0));
    }
}
