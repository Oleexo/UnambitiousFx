namespace Oleexo.UnambitiousFx.Mediator;

public interface IMutationHandler<TMutation, TResponse> : IRequestHandler<TMutation, TResponse>
    where TMutation : IMutation<TResponse> where TResponse : notnull;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse> where TResponse : notnull;