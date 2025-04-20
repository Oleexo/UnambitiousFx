using Oleexo.UnambitiousFx.Core;

namespace Oleexo.UnambitiousFx.Mediator.Tests.Definitions;

public sealed class TestRequestPipelineBehavior<TRequest, TResponse> : IRequestPipelineBehavior<TRequest, TResponse>
    where TResponse : notnull
    where TRequest : IRequest<TResponse> {
    public bool IsCalled { get; private set; }

    public ValueTask<IResult<TResponse>> HandleAsync(TRequest                          request,
                                                     RequestHandlerDelegate<TResponse> next,
                                                     CancellationToken                 cancellationToken = default) {
        IsCalled = true;
        return next();
    }
}
