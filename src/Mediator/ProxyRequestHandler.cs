using Oleexo.UnambitiousFx.Core;

namespace Oleexo.UnambitiousFx.Mediator;

internal sealed class ProxyRequestHandler<TRequestHandler, TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequestHandler : class, IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : notnull
{
    private readonly TRequestHandler _handler;
    private readonly IEnumerable<IRequestPipelineBehavior<TRequest, TResponse>> _behaviors;

    public ProxyRequestHandler(TRequestHandler handler,
        IEnumerable<IRequestPipelineBehavior<TRequest, TResponse>> behaviors)
    {
        _handler = handler;
        _behaviors = behaviors;
    }

    public ValueTask<IResult<TResponse>> HandleAsync(TRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}