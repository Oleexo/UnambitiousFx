using Oleexo.UnambitiousFx.Core;

namespace Oleexo.UnambitiousFx.Mediator;

public interface ISender
{
    ValueTask<IResult<TResponse>> SendAsync<TRequest, TResponse>(TRequest request,
        CancellationToken cancellationToken = default)
        where TResponse : notnull
        where TRequest : IRequest<TResponse>;

    ValueTask<IResult<TResponse>> SendMutationAsync<TMutation, TResponse>(TMutation mutation,
        CancellationToken cancellationToken = default)
        where TMutation : IMutation<TResponse>
        where TResponse : notnull;

    ValueTask<IResult<TResponse>> SendQueryAsync<TQuery, TResponse>(TQuery query,
        CancellationToken cancellationToken = default)
        where TQuery : IQuery<TResponse>
        where TResponse : notnull;
}