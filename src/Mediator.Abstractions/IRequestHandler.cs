using Oleexo.UnambitiousFx.Core;

namespace Oleexo.UnambitiousFx.Mediator.Abstractions;

public interface IRequestHandler<in TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : notnull {
    ValueTask<Result<TResponse>> HandleAsync(IContext          context,
                                             TRequest          request,
                                             CancellationToken cancellationToken = default);
}

public interface IRequestHandler<in TRequest>
    where TRequest : IRequest {
    ValueTask<Result> HandleAsync(IContext          context,
                                  TRequest          request,
                                  CancellationToken cancellationToken = default);
}
