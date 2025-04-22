using Oleexo.UnambitiousFx.Core.Abstractions;

namespace Oleexo.UnambitiousFx.Mediator.Abstractions;

public interface IRequestPipelineBehavior
     {
    ValueTask<IResult> HandleAsync<TRequest>(TRequest               request,
                                                RequestHandlerDelegate next,
                                                CancellationToken      cancellationToken = default)
        where TRequest : IRequest;
    ValueTask<IResult<TResponse>> HandleAsync<TRequest,TResponse>(TRequest                          request,
                                              RequestHandlerDelegate<TResponse> next,
                                              CancellationToken                 cancellationToken = default)
        where TResponse : notnull
    where TRequest : IRequest<TResponse>;
}
