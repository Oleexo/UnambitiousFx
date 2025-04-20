namespace Oleexo.UnambitiousFx.Mediator;

public interface IQueryPipelineBehavior<in TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
}