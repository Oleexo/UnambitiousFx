using Oleexo.UnambitiousFx.Core;

namespace Oleexo.UnambitiousFx.Mediator;

internal class ProxyRequestHandler<TRequestHandler, TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequestHandler : class, IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : notnull {
    private readonly IEnumerable<IRequestPipelineBehavior<TRequest, TResponse>> _behaviors;
    private readonly TRequestHandler                                            _handler;

    public ProxyRequestHandler(TRequestHandler                                            handler,
                               IEnumerable<IRequestPipelineBehavior<TRequest, TResponse>> behaviors) {
        _handler   = handler;
        _behaviors = behaviors;
    }

    public virtual ValueTask<IResult<TResponse>> HandleAsync(TRequest          request,
                                                             CancellationToken cancellationToken = default) {
        return ExecutePipelineAsync(request, _behaviors.ToArray(), 0, cancellationToken);
    }

    private ValueTask<IResult<TResponse>> ExecutePipelineAsync(TRequest                                        request,
                                                               IRequestPipelineBehavior<TRequest, TResponse>[] behaviors,
                                                               int                                             index,
                                                               CancellationToken                               cancellationToken) {
        if (index >= behaviors.Length)
            // When we reach the end of the pipeline, execute the actual handler
        {
            return _handler.HandleAsync(request, cancellationToken);
        }

        // Create a delegate for the next behavior in the pipeline
        RequestHandlerDelegate<TResponse> next = () =>
            ExecutePipelineAsync(request, behaviors, index + 1, cancellationToken);

        // Execute the current behavior with the delegate for the next one
        return behaviors[index]
           .HandleAsync(request, next, cancellationToken);
    }
}
