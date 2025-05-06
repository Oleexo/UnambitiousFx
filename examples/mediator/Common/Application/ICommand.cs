using UnambitiousFx.Mediator.Abstractions;

namespace Common.Application;

public interface ICommand<TResult> : IRequest<TResult> {
}

public interface ICommand : IRequest {
}
