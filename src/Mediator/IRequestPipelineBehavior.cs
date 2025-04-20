using Oleexo.UnambitiousFx.Core;

namespace Oleexo.UnambitiousFx.Mediator;

public interface IRequestPipelineBehavior<in TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : notnull {
    ValueTask<IResult<TResponse>> HandleAsync(TRequest                          request,
                                              RequestHandlerDelegate<TResponse> next,
                                              CancellationToken                 cancellationToken = default);
}
