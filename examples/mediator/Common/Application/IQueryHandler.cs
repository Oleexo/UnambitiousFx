using UnambitiousFx.Mediator.Abstractions;

namespace Common.Application;

public interface IQueryHandler<TRequest, TResult> : IRequestHandler<IAppContext, TRequest, TResult>
    where TRequest : IQuery<TResult>
    where TResult : notnull {
}
