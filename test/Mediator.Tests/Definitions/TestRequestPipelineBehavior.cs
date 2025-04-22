using Oleexo.UnambitiousFx.Core.Abstractions;
using Oleexo.UnambitiousFx.Mediator.Abstractions;

namespace Oleexo.UnambitiousFx.Mediator.Tests.Definitions;

public sealed class TestRequestPipelineBehavior<TRequest, TResponse> : IRequestPipelineBehavior<TRequest, TResponse>
    where TResponse : notnull
    where TRequest : IRequest<TResponse> {
    public bool      Executed        { get; private set; }
    public TRequest? RequestExecuted { get; private set; }
    public int       ExecutionCount  { get; private set; }
    public Action?   OnExecuted      { get; set; }

    public ValueTask<IResult<TResponse>> HandleAsync(TRequest                          request,
                                                     RequestHandlerDelegate<TResponse> next,
                                                     CancellationToken                 cancellationToken = default) {
        Executed        = true;
        RequestExecuted = request;
        ExecutionCount++;
        OnExecuted?.Invoke();
        return next();
    }
}
