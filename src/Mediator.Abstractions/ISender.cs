using UnambitiousFx.Core;

namespace UnambitiousFx.Mediator.Abstractions;

/// <summary>
///     Represents a sender responsible for dispatching requests within a given context and receiving their corresponding
///     results.
/// </summary>
public interface ISender<in TContext>
    where TContext : IContext {
    /// Sends a request to the appropriate handler and returns the result.
    /// <typeparam name="TRequest">
    ///     The type of the request message. Must implement <see cref="IRequest{TResponse}" />.
    /// </typeparam>
    /// <typeparam name="TResponse">
    ///     The type of the response message. The response must be a non-nullable type.
    /// </typeparam>
    /// <param name="context">
    ///     The context in which the request is being sent. This parameter cannot be null.
    /// </param>
    /// <param name="request">
    ///     The request object to be sent. This parameter cannot be null.
    /// </param>
    /// <param name="cancellationToken">
    ///     A cancellation token that can be used to cancel the operation. Defaults to <see cref="CancellationToken.None" />.
    /// </param>
    /// <returns>
    ///     A task representing the asynchronous operation. The task result contains a <see cref="Result{TValue}" />
    ///     holding the response of type <typeparamref name="TResponse" />.
    /// </returns>
    ValueTask<Result<TResponse>> SendAsync<TRequest, TResponse>(TContext          context,
                                                                TRequest          request,
                                                                CancellationToken cancellationToken = default)
        where TResponse : notnull
        where TRequest : IRequest<TResponse>;

    /// Sends a request through the appropriate handler and processes the result.
    /// <typeparam name="TRequest">
    ///     The type of the request message. Must implement <see cref="IRequest" />.
    /// </typeparam>
    /// <param name="context">
    ///     The context in which the request is being executed. This parameter cannot be null and must implement
    ///     <see cref="IContext" />.
    /// </param>
    /// <param name="request">
    ///     The request object to be sent. This parameter cannot be null.
    /// </param>
    /// <param name="cancellationToken">
    ///     A cancellation token that can be used to cancel the operation. Defaults to <see cref="CancellationToken.None" />.
    /// </param>
    /// <returns>
    ///     A <see cref="ValueTask{Result}" /> representing the asynchronous operation. The task result contains a
    ///     <see cref="Result{TValue}" />
    /// </returns>
    ValueTask<Result> SendAsync<TRequest>(TContext          context,
                                          TRequest          request,
                                          CancellationToken cancellationToken = default)
        where TRequest : IRequest;
}

/// <summary>
///     Represents a sender that dispatches requests to their corresponding handlers.
/// </summary>
public interface ISender : ISender<IContext> {
    /// Sends a request to the appropriate handler and returns the result.
    /// <typeparam name="TRequest">
    ///     The type of the request message. Must implement <see cref="IRequest{TResponse}" />.
    /// </typeparam>
    /// <typeparam name="TResponse">
    ///     The type of the response message. The response must be a non-nullable type.
    /// </typeparam>
    /// <param name="request">
    ///     The request object to be sent. This parameter cannot be null.
    /// </param>
    /// <param name="cancellationToken">
    ///     A cancellation token that can be used to cancel the operation. Defaults to <see cref="CancellationToken.None" />.
    /// </param>
    /// <returns>
    ///     A task representing the asynchronous operation. The task result contains a <see cref="Result{TValue}" />
    ///     holding the response of type <typeparamref name="TResponse" />.
    /// </returns>
    ValueTask<Result<TResponse>> SendAsync<TRequest, TResponse>(TRequest          request,
                                                                CancellationToken cancellationToken = default)
        where TResponse : notnull
        where TRequest : IRequest<TResponse>;

    /// Sends a request asynchronously and returns a result.
    /// <typeparam name="TRequest">The type of the request object. It must implement the <see cref="IRequest" /> interface.</typeparam>
    /// <param name="request">The request object to be processed.</param>
    /// <param name="cancellationToken">An optional cancellation token to cancel the operation.</param>
    /// <returns>A <see cref="ValueTask{Result}" /> that represents the result of the operation.</returns>
    ValueTask<Result> SendAsync<TRequest>(TRequest          request,
                                          CancellationToken cancellationToken = default)
        where TRequest : IRequest;
}
