using UnambitiousFx.Core;
using UnambitiousFx.Mediator.Abstractions;

namespace UnambitiousFx.Mediator;

internal sealed class ProxyRequestHandler<TContext, TRequestHandler, TRequest> : IRequestHandler<TContext, TRequest>
    where TRequestHandler : class, IRequestHandler<TContext, TRequest>
    where TRequest : IRequest
    where TContext : IContext {
    private readonly IEnumerable<IRequestPipelineBehavior<TContext>> _behaviors;
    private readonly TRequestHandler                                 _handler;

    public ProxyRequestHandler(TRequestHandler                                 handler,
                               IEnumerable<IRequestPipelineBehavior<TContext>> behaviors) {
        _handler   = handler;
        _behaviors = behaviors;
    }

    public ValueTask<Result> HandleAsync(TContext          context,
                                         TRequest          request,
                                         CancellationToken cancellationToken = default) {
        return ExecutePipelineAsync(context, request, _behaviors.ToArray(), 0, cancellationToken);
    }

    private ValueTask<Result> ExecutePipelineAsync(TContext                             context,
                                                   TRequest                             request,
                                                   IRequestPipelineBehavior<TContext>[] behaviors,
                                                   int                                  index,
                                                   CancellationToken                    cancellationToken) {
        if (index >= behaviors.Length) {
            return _handler.HandleAsync(context, request, cancellationToken);
        }

        return behaviors[index]
           .HandleAsync(context, request, Next, cancellationToken);

        ValueTask<Result> Next() {
            return ExecutePipelineAsync(context, request, behaviors, index + 1, cancellationToken);
        }
    }
}

internal class ProxyRequestHandler<TContext, TRequestHandler, TRequest, TResponse> : IRequestHandler<TContext, TRequest, TResponse>
    where TRequestHandler : class, IRequestHandler<TContext, TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : notnull
    where TContext : IContext {
    private readonly IEnumerable<IRequestPipelineBehavior<TContext>> _behaviors;
    private readonly TRequestHandler                                 _handler;

    public ProxyRequestHandler(TRequestHandler                                 handler,
                               IEnumerable<IRequestPipelineBehavior<TContext>> behaviors) {
        _handler   = handler;
        _behaviors = behaviors;
    }

    public virtual ValueTask<Result<TResponse>> HandleAsync(TContext          context,
                                                            TRequest          request,
                                                            CancellationToken cancellationToken = default) {
        return ExecutePipelineAsync(context, request, _behaviors.ToArray(), 0, cancellationToken);
    }

    private ValueTask<Result<TResponse>> ExecutePipelineAsync(TContext                             context,
                                                              TRequest                             request,
                                                              IRequestPipelineBehavior<TContext>[] behaviors,
                                                              int                                  index,
                                                              CancellationToken                    cancellationToken) {
        if (index >= behaviors.Length) {
            return _handler.HandleAsync(context, request, cancellationToken);
        }

        return behaviors[index]
           .HandleAsync(context, request, Next, cancellationToken);

        ValueTask<Result<TResponse>> Next() {
            return ExecutePipelineAsync(context, request, behaviors, index + 1, cancellationToken);
        }
    }
}
