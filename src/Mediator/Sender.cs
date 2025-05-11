using UnambitiousFx.Core;
using UnambitiousFx.Mediator.Abstractions;
using UnambitiousFx.Mediator.Resolvers;

namespace UnambitiousFx.Mediator;

internal class Sender<TContext> : ISender<TContext>
    where TContext : IContext {
    private readonly IDependencyResolver _resolver;

    public Sender(IDependencyResolver resolver) {
        _resolver = resolver;
    }

    public ValueTask<Result<TResponse>> SendAsync<TRequest, TResponse>(TContext          context,
                                                                       TRequest          request,
                                                                       CancellationToken cancellationToken = default)
        where TRequest : IRequest<TResponse>
        where TResponse : notnull {
        return _resolver.GetService<IRequestHandler<TContext, TRequest, TResponse>>()
                        .Match(handler => handler.HandleAsync(context, request, cancellationToken),
                               () => throw new MissingHandlerException(typeof(IRequestHandler<TContext, TRequest, TResponse>)));
    }

    public ValueTask<Result> SendAsync<TRequest>(TContext          context,
                                                 TRequest          request,
                                                 CancellationToken cancellationToken = default)
        where TRequest : IRequest {
        return _resolver.GetService<IRequestHandler<TContext, TRequest>>()
                        .Match(handler => handler.HandleAsync(context, request, cancellationToken),
                               () => throw new MissingHandlerException(typeof(IRequestHandler<TContext, TRequest>)));
    }
}

internal sealed class Sender : Sender<IContext>, ISender {
    private readonly IContextFactory<IContext> _contextFactory;

    public Sender(IDependencyResolver       resolver,
                  IContextFactory<IContext> contextFactory)
        : base(resolver) {
        _contextFactory = contextFactory;
    }

    public ValueTask<Result<TResponse>> SendAsync<TRequest, TResponse>(TRequest          request,
                                                                       CancellationToken cancellationToken = default)
        where TResponse : notnull
        where TRequest : IRequest<TResponse> {
        return SendAsync<TRequest, TResponse>(_contextFactory.Create(), request, cancellationToken);
    }

    public ValueTask<Result> SendAsync<TRequest>(TRequest          request,
                                                 CancellationToken cancellationToken = default)
        where TRequest : IRequest {
        return SendAsync(_contextFactory.Create(), request, cancellationToken);
    }
}
