using Oleexo.UnambitiousFx.Core;

namespace Oleexo.UnambitiousFx.Mediator;

internal sealed class ProxyQueryHandler<TQueryHandler, TQuery, TResponse>
    : ProxyRequestHandler<TQueryHandler, TQuery, TResponse>,
      IQueryHandler<TQuery, TResponse>
    where TQueryHandler : class, IQueryHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
    where TResponse : notnull {
    private readonly IEnumerable<IQueryPipelineBehavior<TQuery, TResponse>> _behaviors;

    public ProxyQueryHandler(TQueryHandler                                            handler,
                             IEnumerable<IQueryPipelineBehavior<TQuery, TResponse>>   queryBehaviors,
                             IEnumerable<IRequestPipelineBehavior<TQuery, TResponse>> requestBehaviors)
        : base(handler, requestBehaviors) {
        _behaviors = queryBehaviors;
    }

    public override ValueTask<IResult<TResponse>> HandleAsync(TQuery            request,
                                                              CancellationToken cancellationToken = default) {
        return ExecutePipelineAsync(request, _behaviors.ToArray(), 0, cancellationToken);
    }

    private ValueTask<IResult<TResponse>> ExecutePipelineAsync(TQuery                                      request,
                                                               IQueryPipelineBehavior<TQuery, TResponse>[] behaviors,
                                                               int                                         index,
                                                               CancellationToken                           cancellationToken) {
        if (index >= behaviors.Length)
            // When we reach the end of the pipeline, execute the actual handler
        {
            return base.HandleAsync(request, cancellationToken);
        }

        // Create a delegate for the next behavior in the pipeline
        RequestHandlerDelegate<TResponse> next = () =>
            ExecutePipelineAsync(request, behaviors, index + 1, cancellationToken);

        // Execute the current behavior with the delegate for the next one
        return behaviors[index]
           .HandleAsync(request, next, cancellationToken);
    }
}
