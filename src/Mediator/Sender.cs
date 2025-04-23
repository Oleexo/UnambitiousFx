using Oleexo.UnambitiousFx.Core;
using Oleexo.UnambitiousFx.Mediator.Abstractions;
using Oleexo.UnambitiousFx.Mediator.Resolvers;

namespace Oleexo.UnambitiousFx.Mediator;

internal sealed class Sender : ISender {
    private readonly IContextFactory     _contextFactory;
    private readonly IDependencyResolver _resolver;

    public Sender(IDependencyResolver resolver,
                  IContextFactory     contextFactory) {
        _resolver       = resolver;
        _contextFactory = contextFactory;
    }

    public ValueTask<Result<TResponse>> SendAsync<TRequest, TResponse>(TRequest          request,
                                                                       CancellationToken cancellationToken = default)
        where TResponse : notnull
        where TRequest : IRequest<TResponse> {
        var handler = _resolver.GetService<IRequestHandler<TRequest, TResponse>>();
        if (handler is null) {
            throw new MissingHandlerException(typeof(IRequestHandler<TRequest, TResponse>));
        }

        var ctx = _contextFactory.Create();
        return handler.HandleAsync(ctx, request, cancellationToken);
    }

    public ValueTask<Result> SendAsync<TRequest>(TRequest          request,
                                                 CancellationToken cancellationToken = default)
        where TRequest : IRequest {
        var handler = _resolver.GetService<IRequestHandler<TRequest>>();
        if (handler is null) {
            throw new MissingHandlerException(typeof(IRequestHandler<TRequest>));
        }

        var ctx = _contextFactory.Create();
        return handler.HandleAsync(ctx, request, cancellationToken);
    }
}
