using UnambitiousFx.Core;

namespace UnambitiousFx.Mediator.Abstractions;

/// Provides a mechanism to define custom behavior for handling requests in a processing pipeline.
/// Implementers of this interface can encapsulate logic that needs to be executed before or after
/// the request handler is invoked.
/// The interface supports two forms of request handling:
/// 1. A request without a response.
/// 2. A request that yields a specific response.
/// Implementations can utilize the provided `context`, `request`, and the `next` delegate to
/// customize or extend the handling process. The `next` delegate represents the subsequent
/// middleware or the actual request handler in the pipeline.
/// Generic Type Parameters:
/// - TRequest: Represents the type of the request being handled.
/// - TResponse: Represents the type of the response produced by the request.
public interface IRequestPipelineBehavior<TContext>
    where TContext : IContext {
    /// Handles the asynchronous processing of a request in the context of a pipeline behavior.
    /// <typeparam name="TRequest">The type of the request object being processed.</typeparam>
    /// <param name="context">The execution context for the current request.</param>
    /// <param name="request">The request object to be processed.</param>
    /// <param name="next">The delegate to invoke the next step in the processing pipeline.</param>
    /// <param name="cancellationToken">
    ///     A token to observe while waiting for the operation to complete, allowing cancellation if needed.
    /// </param>
    /// <returns>
    ///     A ValueTask representing the asynchronous operation, containing a result object that indicates the
    ///     success or failure of the process.
    /// </returns>
    ValueTask<Result> HandleAsync<TRequest>(TContext               context,
                                            TRequest               request,
                                            RequestHandlerDelegate next,
                                            CancellationToken      cancellationToken = default)
        where TRequest : IRequest;

    /// Handles the asynchronous execution of a request pipeline behavior within a specific context.
    /// <typeparam name="TRequest">The type of the request being processed. Must implement IRequest{TResponse}.</typeparam>
    /// <typeparam name="TResponse">The type of the response expected from the request handling. Must be a non-nullable type.</typeparam>
    /// <param name="context">The context in which the request is being executed.</param>
    /// <param name="request">The request instance to be processed in the pipeline.</param>
    /// <param name="next">A delegate that represents the subsequent step in the request pipeline.</param>
    /// <param name="cancellationToken">
    ///     A token to observe for cancellation requests during the asynchronous operation.
    /// </param>
    /// <returns>
    ///     A ValueTask containing a Result{TResponse} representing the outcome of the request pipeline execution.
    /// </returns>
    ValueTask<Result<TResponse>> HandleAsync<TRequest, TResponse>(TContext                          context,
                                                                  TRequest                          request,
                                                                  RequestHandlerDelegate<TResponse> next,
                                                                  CancellationToken                 cancellationToken = default)
        where TResponse : notnull
        where TRequest : IRequest<TResponse>;
}
