using Oleexo.UnambitiousFx.Core.Abstractions;

namespace Oleexo.UnambitiousFx.Mediator.Abstractions;

public interface ISender {
    ValueTask<IResult<TResponse>> SendAsync<TRequest, TResponse>(TRequest          request,
                                                                 CancellationToken cancellationToken = default)
        where TResponse : notnull
        where TRequest : IRequest<TResponse>;

    ValueTask<IResult> SendAsync<TRequest>(TRequest          request,
                                           CancellationToken cancellationToken = default)
        where TRequest : IRequest;
}
