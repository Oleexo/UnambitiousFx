using UnambitiousFx.Core;
using UnambitiousFx.Mediator.Abstractions;

namespace UnambitiousFx.Mediator.Tests.Definitions;

public sealed class RequestWithResponseExampleHandler : IRequestHandler<ITestContext, RequestWithResponseExample, int> {
    public bool                        Executed        { get; private set; }
    public RequestWithResponseExample? RequestExecuted { get; private set; }
    public int                         ExecutionCount  { get; private set; }
    public Action?                     OnExecuted      { get; set; }

    public ValueTask<Result<int>> HandleAsync(ITestContext               ctx,
                                              RequestWithResponseExample request,
                                              CancellationToken          cancellationToken = default) {
        Executed        = true;
        RequestExecuted = request;
        ExecutionCount++;
        OnExecuted?.Invoke();
        return new ValueTask<Result<int>>(Result<int>.Success(0));
    }
}
