using Oleexo.UnambitiousFx.Core;

namespace Oleexo.UnambitiousFx.Mediator;

public interface IQueryPipelineBehavior<in TQuery, TResponse>
    where TQuery : IQuery<TResponse>
    where TResponse : notnull {
    ValueTask<IResult<TResponse>> HandleAsync(TQuery                            request,
                                              RequestHandlerDelegate<TResponse> next,
                                              CancellationToken                 cancellationToken = default);
}
