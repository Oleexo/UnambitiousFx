using UnambitiousFx.Core.Results;

namespace UnambitiousFx.Mediator.Abstractions;

public interface IRequestValidator<in TRequest>
{
    ValueTask<Result> ValidateAsync(TRequest request, CancellationToken cancellationToken = default);
}
