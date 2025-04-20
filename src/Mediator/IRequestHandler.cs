using Oleexo.UnambitiousFx.Core;

namespace Oleexo.UnambitiousFx.Mediator;

public interface IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse> where TResponse : notnull
{
    ValueTask<IResult<TResponse>> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}