using Oleexo.UnambitiousFx.Core.Abstractions;

namespace Oleexo.UnambitiousFx.Mediator.Abstractions;

public interface IRequestHandler<in TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : notnull {
    ValueTask<IResult<TResponse>> HandleAsync(IContext          context,
                                              TRequest          request,
                                              CancellationToken cancellationToken = default);
}

public interface IRequestHandler<in TRequest>
    where TRequest : IRequest {
    ValueTask<IResult> HandleAsync(IContext          context,
                                   TRequest          request,
                                   CancellationToken cancellationToken = default);
}
