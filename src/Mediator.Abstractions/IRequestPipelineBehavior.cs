using Oleexo.UnambitiousFx.Core;

namespace Oleexo.UnambitiousFx.Mediator.Abstractions;

public interface IRequestPipelineBehavior {
    ValueTask<Result> HandleAsync<TRequest>(TRequest               request,
                                            RequestHandlerDelegate next,
                                            CancellationToken      cancellationToken = default)
        where TRequest : IRequest;

    ValueTask<Result<TResponse>> HandleAsync<TRequest, TResponse>(TRequest                          request,
                                                                  RequestHandlerDelegate<TResponse> next,
                                                                  CancellationToken                 cancellationToken = default)
        where TResponse : notnull
        where TRequest : IRequest<TResponse>;
}
