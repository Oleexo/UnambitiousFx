using Oleexo.UnambitiousFx.Core.Abstractions;
using Oleexo.UnambitiousFx.Mediator.Abstractions;

namespace Oleexo.UnambitiousFx.Mediator;

internal sealed class ProxyRequestHandler<TRequestHandler, TRequest> : IRequestHandler<TRequest>
    where TRequestHandler : class, IRequestHandler<TRequest>
    where TRequest : IRequest {
    private readonly IEnumerable<IRequestPipelineBehavior> _behaviors;
    private readonly TRequestHandler                       _handler;

    public ProxyRequestHandler(TRequestHandler                       handler,
                               IEnumerable<IRequestPipelineBehavior> behaviors) {
        _handler   = handler;
        _behaviors = behaviors;
    }

    public ValueTask<IResult> HandleAsync(IContext          context,
                                          TRequest          request,
                                          CancellationToken cancellationToken = default) {
        return ExecutePipelineAsync(context, request, _behaviors.ToArray(), 0, cancellationToken);
    }

    private ValueTask<IResult> ExecutePipelineAsync(IContext                   context,
                                                    TRequest                   request,
                                                    IRequestPipelineBehavior[] behaviors,
                                                    int                        index,
                                                    CancellationToken          cancellationToken) {
        if (index >= behaviors.Length) {
            return _handler.HandleAsync(context, request, cancellationToken);
        }

        return behaviors[index]
           .HandleAsync(request, Next, cancellationToken);

        ValueTask<IResult> Next() {
            return ExecutePipelineAsync(context, request, behaviors, index + 1, cancellationToken);
        }
    }
}

internal class ProxyRequestHandler<TRequestHandler, TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequestHandler : class, IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : notnull {
    private readonly IEnumerable<IRequestPipelineBehavior> _behaviors;
    private readonly TRequestHandler                       _handler;

    public ProxyRequestHandler(TRequestHandler                       handler,
                               IEnumerable<IRequestPipelineBehavior> behaviors) {
        _handler   = handler;
        _behaviors = behaviors;
    }

    public virtual ValueTask<IResult<TResponse>> HandleAsync(IContext          context,
                                                             TRequest          request,
                                                             CancellationToken cancellationToken = default) {
        return ExecutePipelineAsync(context, request, _behaviors.ToArray(), 0, cancellationToken);
    }

    private ValueTask<IResult<TResponse>> ExecutePipelineAsync(IContext                   context,
                                                               TRequest                   request,
                                                               IRequestPipelineBehavior[] behaviors,
                                                               int                        index,
                                                               CancellationToken          cancellationToken) {
        if (index >= behaviors.Length) {
            return _handler.HandleAsync(context, request, cancellationToken);
        }

        return behaviors[index]
           .HandleAsync(request, Next, cancellationToken);

        ValueTask<IResult<TResponse>> Next() {
            return ExecutePipelineAsync(context, request, behaviors, index + 1, cancellationToken);
        }
    }
}
