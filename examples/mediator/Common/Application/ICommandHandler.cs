using UnambitiousFx.Mediator.Abstractions;

namespace Common.Application;

public interface ICommandHandler<in TRequest> : IRequestHandler<IAppContext, TRequest>
    where TRequest : ICommand {
}

public interface ICommandHandler<in TRequest, TResult> : IRequestHandler<IAppContext, TRequest, TResult>
    where TRequest : ICommand<TResult>
    where TResult : notnull {
}
