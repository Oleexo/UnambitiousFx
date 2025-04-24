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
        return _resolver.GetService<IRequestHandler<TRequest, TResponse>>()
                        .Match(handler => {
                             var ctx = _contextFactory.Create();
                             return handler.HandleAsync(ctx, request, cancellationToken);
                         }, () => throw new MissingHandlerException(typeof(IRequestHandler<TRequest, TResponse>)));
    }

    public ValueTask<Result> SendAsync<TRequest>(TRequest          request,
                                                 CancellationToken cancellationToken = default)
        where TRequest : IRequest {
        return _resolver.GetService<IRequestHandler<TRequest>>()
                        .Match(handler => {
                                   var ctx = _contextFactory.Create();
                                   return handler.HandleAsync(ctx, request, cancellationToken);
                               },
                               () => throw new MissingHandlerException(typeof(IRequestHandler<TRequest>)));
    }
}
