namespace Oleexo.UnambitiousFx.Mediator;

public interface IMutation<TResponse> : IRequest<TResponse> {
}

public interface IMutation : IMutation<Unit> {
}
