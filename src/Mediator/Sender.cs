using Oleexo.UnambitiousFx.Core;

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

        return handler.HandleAsync(request, cancellationToken);
    }

    public ValueTask<IResult<TResponse>> SendMutationAsync<TMutation, TResponse>(TMutation         mutation,
                                                                                 CancellationToken cancellationToken = default)
        where TMutation : IMutation<TResponse>
        where TResponse : notnull {
        var handler = _resolver.Resolve<IMutationHandler<TMutation, TResponse>>();
        if (handler is null) {
            throw new MissingHandlerException(typeof(IMutationHandler<TMutation, TResponse>));
        }

        return handler.HandleAsync(mutation, cancellationToken);
    }

    public ValueTask<IResult<TResponse>> SendQueryAsync<TQuery, TResponse>(TQuery            query,
                                                                           CancellationToken cancellationToken = default)
        where TQuery : IQuery<TResponse>
        where TResponse : notnull {
        var handler = _resolver.Resolve<IQueryHandler<TQuery, TResponse>>();
        if (handler is null) {
            throw new MissingHandlerException(typeof(IQueryHandler<TQuery, TResponse>));
        }

        return handler.HandleAsync(query, cancellationToken);
    }
}
