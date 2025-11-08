using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions.Collections.ValueTasks;
using UnambitiousFx.Mediator.Abstractions;

namespace UnambitiousFx.Mediator.Pipelines;

/// <summary>
/// Implements a request pipeline behavior that validates an incoming request before delegating
/// the request to the next handler in the pipeline. If validation fails, it returns a result
/// containing the validation errors and avoids further processing.
/// </summary>
/// <typeparam name="TRequest">The type of the request being validated.</typeparam>
/// <typeparam name="TResponse">The type of the response expected from the pipeline handling.</typeparam>
public class RequestValidationBehavior<TRequest, TResponse> : IRequestPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : notnull
{
    private readonly IEnumerable<IRequestValidator<TRequest>> _validators;

    /// <summary>
    /// Represents a pipeline behavior that validates requests before they are processed by the appropriate request handler.
    /// </summary>
    public RequestValidationBehavior(IEnumerable<IRequestValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    /// <summary>
    /// Handles the current request by validating it using the configured validators.
    /// If validation succeeds, the request is passed to the next behavior in the pipeline.
    /// If validation fails, a failed result is returned.
    /// </summary>
    /// <param name="request">The request object to be processed.</param>
    /// <param name="next">The delegate that represents the next behavior in the pipeline.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A <see cref="ValueTask{TResult}"/> of type <see cref="Result{TResponse}"/>.
    /// If validation is successful, the result of the next behavior is returned.
    /// If validation fails, a failed result with validation errors is returned.
    /// </returns>
    public async ValueTask<Result<TResponse>> HandleAsync(TRequest request,
                                                          RequestHandlerDelegate<TResponse> next,
                                                          CancellationToken cancellationToken)
    {
        var result = await _validators.Select(x => x.ValidateAsync(request, cancellationToken))
                                      .CombineAsync();

        if (result.IsFaulted)
        {
            return Result.Failure<TResponse>(result.Errors);
        }

        return await next();
    }
}
