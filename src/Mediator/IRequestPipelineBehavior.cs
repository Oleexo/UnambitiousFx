namespace Oleexo.UnambitiousFx.Mediator;

public interface IRequestPipelineBehavior<in TRequest, TResponse>
    where TRequest : IRequest<TResponse>;