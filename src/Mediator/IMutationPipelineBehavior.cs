namespace Oleexo.UnambitiousFx.Mediator;

public interface IMutationPipelineBehavior<in TMutation, TResponse>
    where TMutation : IMutation<TResponse>
{
}