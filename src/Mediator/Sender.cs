using Oleexo.UnambitiousFx.Core.Abstractions;
using Oleexo.UnambitiousFx.Mediator.Abstractions;
using Oleexo.UnambitiousFx.Mediator.Resolvers;

namespace Oleexo.UnambitiousFx.Mediator;

internal sealed class Sender : ISender {
    private readonly IDependencyResolver _resolver;

    public Sender(IDependencyResolver resolver) {
        _resolver = resolver;
    }

    public ValueTask<IResult<TResponse>> SendAsync<TRequest, TResponse>(TRequest          request,
                                                                        CancellationToken cancellationToken = default)
        where TResponse : notnull
        where TRequest : IRequest<TResponse> {
        var handler = _resolver.Resolve<IRequestHandler<TRequest, TResponse>>();
        if (handler is null) {
            throw new MissingHandlerException(typeof(IRequestHandler<TRequest, TResponse>));
        }

        var ctx = new Context();
        return handler.HandleAsync(ctx, request, cancellationToken);
    }

    public ValueTask<IResult> SendAsync<TRequest>(TRequest          request,
                                                  CancellationToken cancellationToken = default)
        where TRequest : IRequest {
        var handler = _resolver.Resolve<IRequestHandler<TRequest>>();
        if (handler is null) {
            throw new MissingHandlerException(typeof(IRequestHandler<TRequest>));
        }

        var ctx = new Context();
        return handler.HandleAsync(ctx, request, cancellationToken);
    }
}
