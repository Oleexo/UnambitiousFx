using Oleexo.UnambitiousFx.Core;

namespace Oleexo.UnambitiousFx.Mediator;

public interface IMutationPipelineBehavior<in TMutation, TResponse>
    where TMutation : IMutation<TResponse>
    where TResponse : notnull {
    ValueTask<IResult<TResponse>> HandleAsync(TMutation                         request,
                                              RequestHandlerDelegate<TResponse> next,
                                              CancellationToken                 cancellationToken = default);
}
