using Oleexo.UnambitiousFx.Core;

namespace Oleexo.UnambitiousFx.Mediator;

internal sealed class ProxyMutationHandler<TMutationHandler, TMutation, TResponse>
    : ProxyRequestHandler<TMutationHandler, TMutation, TResponse>,
      IMutationHandler<TMutation, TResponse>
    where TMutationHandler : class, IMutationHandler<TMutation, TResponse>
    where TMutation : IMutation<TResponse>
    where TResponse : notnull {
    private readonly IEnumerable<IMutationPipelineBehavior<TMutation, TResponse>> _behaviors;

    public ProxyMutationHandler(TMutationHandler                                             handler,
                                IEnumerable<IMutationPipelineBehavior<TMutation, TResponse>> mutationBehaviors,
                                IEnumerable<IRequestPipelineBehavior<TMutation, TResponse>>  requestBehaviors)
        : base(handler, requestBehaviors) {
        _behaviors = mutationBehaviors;
    }

    public override ValueTask<IResult<TResponse>> HandleAsync(TMutation         request,
                                                              CancellationToken cancellationToken = default) {
        return ExecutePipelineAsync(request, _behaviors.ToArray(), 0, cancellationToken);
    }

    private ValueTask<IResult<TResponse>> ExecutePipelineAsync(TMutation                                         request,
                                                               IMutationPipelineBehavior<TMutation, TResponse>[] behaviors,
                                                               int                                               index,
                                                               CancellationToken                                 cancellationToken) {
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
