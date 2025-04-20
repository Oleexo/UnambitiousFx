using Oleexo.UnambitiousFx.Core;

namespace Oleexo.UnambitiousFx.Mediator.Tests.Definitions;

public sealed class TestMutationPipelineBehavior<TMutation, TResponse> : IMutationPipelineBehavior<TMutation, TResponse>
    where TMutation : IMutation<TResponse>
    where TResponse : notnull {
    public bool IsCalled { get; private set; }

    public ValueTask<IResult<TResponse>> HandleAsync(TMutation                         request,
                                                     RequestHandlerDelegate<TResponse> next,
                                                     CancellationToken                 cancellationToken = default) {
        IsCalled = true;
        return next();
    }
}
