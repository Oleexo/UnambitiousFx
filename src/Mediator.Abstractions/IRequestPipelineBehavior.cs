using Oleexo.UnambitiousFx.Core.Abstractions;

namespace Oleexo.UnambitiousFx.Mediator.Abstractions;

public interface IRequestPipelineBehavior<in TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : notnull {
    ValueTask<IResult<TResponse>> HandleAsync(TRequest                          request,
                                              RequestHandlerDelegate<TResponse> next,
                                              CancellationToken                 cancellationToken = default);
}

public interface IRequestPipelineBehavior<in TRequest>
    where TRequest : IRequest {
    ValueTask<IResult> HandleAsync(TRequest               request,
                                   RequestHandlerDelegate next,
                                   CancellationToken      cancellationToken = default);
}
