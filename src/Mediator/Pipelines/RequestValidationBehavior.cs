using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions.Collections.ValueTasks;
using UnambitiousFx.Mediator.Abstractions;

namespace UnambitiousFx.Mediator.Pipelines;

public class RequestValidationBehavior<TRequest, TResponse> : IRequestPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : notnull {
    private readonly IEnumerable<IRequestValidator<TRequest>> _validators;

    public RequestValidationBehavior(IEnumerable<IRequestValidator<TRequest>> validators) {
        _validators = validators;
    }

    public async ValueTask<Result<TResponse>> HandleAsync(TRequest                          request,
                                                          RequestHandlerDelegate<TResponse> next,
                                                          CancellationToken                 cancellationToken) {
        var result = await _validators.Select(x => x.ValidateAsync(request, cancellationToken))
                                      .CombineAsync();

        if (result.IsFaulted) {
            return Result.Failure<TResponse>(result.Errors);
        }

        return await next();
    }
}
