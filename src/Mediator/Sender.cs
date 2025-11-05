using UnambitiousFx.Core.Results;
using UnambitiousFx.Mediator.Abstractions;
using UnambitiousFx.Mediator.Resolvers;

namespace UnambitiousFx.Mediator;

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
        var handler = _resolver.GetRequiredService<IRequestHandler<TRequest, TResponse>>();
        var ctx     = _contextFactory.Create();
        return handler.HandleAsync(ctx, request, cancellationToken);
    }

    public ValueTask<Result> SendAsync<TRequest>(TRequest          request,
                                                 CancellationToken cancellationToken = default)
        where TRequest : IRequest {
        var handler = _resolver.GetRequiredService<IRequestHandler<TRequest>>();
        var ctx     = _contextFactory.Create();
        return handler.HandleAsync(ctx, request, cancellationToken);
    }
}
