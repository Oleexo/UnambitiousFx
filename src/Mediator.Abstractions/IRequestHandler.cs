using UnambitiousFx.Core;

namespace UnambitiousFx.Mediator.Abstractions;

/// <summary>
///     Represents a contract for handling requests within a specified context, where the request and response types
///     are explicitly defined.
/// </summary>
/// <typeparam name="TContext">
///     The type of the context in which the request is processed. Must implement
///     <see cref="IContext" />.
/// </typeparam>
/// <typeparam name="TRequest">
///     The type of the request being handled. Must implement <see cref="IRequest{TResponse}" />.
/// </typeparam>
/// <typeparam name="TResponse">The type of the response produced by the handler. Must be a non-nullable type.</typeparam>
public interface IRequestHandler<in TContext, in TRequest, TResponse>
    where TContext : IContext
    where TRequest : IRequest<TResponse>
    where TResponse : notnull {
    /// <summary>
    ///     Handles the given request asynchronously.
    /// </summary>
    /// <param name="context">The context in which the request is being processed.</param>
    /// <param name="request">The request object containing all required data.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation and contains the result of the request.</returns>
    ValueTask<Result<TResponse>> HandleAsync(TContext          context,
                                             TRequest          request,
                                             CancellationToken cancellationToken = default);
}

/// <summary>
///     Defines a contract for processing a request asynchronously within an execution context.
/// </summary>
/// <typeparam name="TContext">
///     The type of the context in which the request is processed. Must implement
///     <see cref="IContext" />.
/// </typeparam>
/// <typeparam name="TRequest">The type of the request being handled. Must implement <see cref="IRequest" />.</typeparam>
public interface IRequestHandler<in TContext, in TRequest>
    where TContext : IContext
    where TRequest : IRequest {
    /// Handles an incoming request asynchronously and produces a result.
    /// <param name="context">The execution context containing relevant information for handling the request.</param>
    /// <param name="request">The request to be processed by the handler.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests, with a default value of None.</param>
    /// <return>A task that represents the asynchronous operation, containing a result of the processing.</return>
    ValueTask<Result> HandleAsync(TContext          context,
                                  TRequest          request,
                                  CancellationToken cancellationToken = default);
}
