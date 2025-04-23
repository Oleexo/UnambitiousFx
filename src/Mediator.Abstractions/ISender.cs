using Oleexo.UnambitiousFx.Core;

namespace Oleexo.UnambitiousFx.Mediator.Abstractions;

public interface ISender {
    ValueTask<Result<TResponse>> SendAsync<TRequest, TResponse>(TRequest          request,
                                                                CancellationToken cancellationToken = default)
        where TResponse : notnull
        where TRequest : IRequest<TResponse>;

    ValueTask<Result> SendAsync<TRequest>(TRequest          request,
                                          CancellationToken cancellationToken = default)
        where TRequest : IRequest;
}
