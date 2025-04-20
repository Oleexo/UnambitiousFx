using Oleexo.UnambitiousFx.Core;

namespace Oleexo.UnambitiousFx.Mediator.Tests.Definitions;

public sealed class TestQueryPipelineBehavior<TQuery, TResponse> : IQueryPipelineBehavior<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
    where TResponse : notnull {
    public bool IsCalled { get; private set; }

    public ValueTask<IResult<TResponse>> HandleAsync(TQuery                            request,
                                                     RequestHandlerDelegate<TResponse> next,
                                                     CancellationToken                 cancellationToken = default) {
        IsCalled = true;
        return next();
    }
}
