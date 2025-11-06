using System.Collections.Immutable;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Mediator.Abstractions;

namespace UnambitiousFx.Mediator;

internal sealed class ProxyRequestHandler<TRequestHandler, TRequest> : IRequestHandler<TRequest>
    where TRequestHandler : class, IRequestHandler<TRequest>
    where TRequest : IRequest {
    private readonly ImmutableArray<IRequestPipelineBehavior> _behaviors;
    private readonly TRequestHandler                          _handler;

    public ProxyRequestHandler(TRequestHandler                       handler,
                               IEnumerable<IRequestPipelineBehavior> behaviors) {
        _handler   = handler;
        _behaviors = [..behaviors];
    }

    public ValueTask<Result> HandleAsync(TRequest          request,
                                         CancellationToken cancellationToken = default) {
        return ExecutePipelineAsync(request, 0, cancellationToken);
    }

    private ValueTask<Result> ExecutePipelineAsync(TRequest          request,
                                                   int               index,
                                                   CancellationToken cancellationToken) {
        if (index >= _behaviors.Length) {
            return _handler.HandleAsync(request, cancellationToken);
        }

        return _behaviors[index]
           .HandleAsync(request, Next, cancellationToken);

        ValueTask<Result> Next() {
            return ExecutePipelineAsync(request, index + 1, cancellationToken);
        }
    }
}

internal class ProxyRequestHandler<TRequestHandler, TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequestHandler : class, IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : notnull {
    private readonly ImmutableArray<IRequestPipelineBehavior> _behaviors;
    private readonly TRequestHandler                          _handler;

    public ProxyRequestHandler(TRequestHandler                       handler,
                               IEnumerable<IRequestPipelineBehavior> behaviors) {
        _handler   = handler;
        _behaviors = [..behaviors];
    }

    public virtual ValueTask<Result<TResponse>> HandleAsync(TRequest          request,
                                                            CancellationToken cancellationToken = default) {
        return ExecutePipelineAsync(request, 0, cancellationToken);
    }

    private ValueTask<Result<TResponse>> ExecutePipelineAsync(TRequest          request,
                                                              int               index,
                                                              CancellationToken cancellationToken) {
        if (index >= _behaviors.Length) {
            return _handler.HandleAsync(request, cancellationToken);
        }

        return _behaviors[index]
           .HandleAsync(request, Next, cancellationToken);

        ValueTask<Result<TResponse>> Next() {
            return ExecutePipelineAsync(request, index + 1, cancellationToken);
        }
    }
}
