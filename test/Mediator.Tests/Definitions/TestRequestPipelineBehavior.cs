using UnambitiousFx.Core;
using UnambitiousFx.Mediator.Abstractions;

namespace UnambitiousFx.Mediator.Tests.Definitions;

public sealed class TestRequestPipelineBehavior : IRequestPipelineBehavior {
    public bool    Executed        { get; private set; }
    public object? RequestExecuted { get; private set; }
    public int     ExecutionCount  { get; private set; }
    public Action? OnExecuted      { get; set; }

    public ValueTask<Result> HandleAsync<TContext, TRequest>(TContext               context,
                                                             TRequest               request,
                                                             RequestHandlerDelegate next,
                                                             CancellationToken      cancellationToken = default)
        where TRequest : IRequest
        where TContext : IContext {
        Executed        = true;
        RequestExecuted = request;
        ExecutionCount++;
        OnExecuted?.Invoke();
        return next();
    }

    public ValueTask<Result<TResponse>> HandleAsync<TContext, TRequest, TResponse>(TContext                          context,
                                                                                   TRequest                          request,
                                                                                   RequestHandlerDelegate<TResponse> next,
                                                                                   CancellationToken                 cancellationToken = default)
        where TRequest : IRequest<TResponse>
        where TResponse : notnull
        where TContext : IContext {
        Executed        = true;
        RequestExecuted = request;
        ExecutionCount++;
        OnExecuted?.Invoke();
        return next();
    }
}
