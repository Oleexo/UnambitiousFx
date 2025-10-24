using Microsoft.Extensions.DependencyInjection;
using UnambitiousFx.Mediator.Abstractions;
using UnambitiousFx.Mediator.Tests.Definitions;

namespace UnambitiousFx.Mediator.Tests;

public sealed class ConditionalRequestPipelineBehaviorTests {
    private sealed class UntypedConditionalBehavior : IRequestPipelineBehavior {
        public int ExecutionCount { get; private set; }
        public ValueTask<Core.Results.Result> HandleAsync<TRequest>(IContext context, TRequest request, RequestHandlerDelegate next, CancellationToken cancellationToken = default) where TRequest : IRequest {
            ExecutionCount++;
            return next();
        }
        public ValueTask<Core.Results.Result<TResponse>> HandleAsync<TRequest, TResponse>(IContext context, TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken = default) where TResponse : notnull where TRequest : IRequest<TResponse> {
            ExecutionCount++;
            return next();
        }
    }

    [Fact]
    public async Task Conditional_untyped_behavior_executes_when_predicate_true() {
        var services = new ServiceCollection();
        services.AddMediator(cfg => {
            cfg.RegisterRequestHandler<RequestExampleHandler, RequestExample>();
            cfg.RegisterConditionalRequestPipelineBehavior<UntypedConditionalBehavior>((_, _) => true);
        });
        var provider = services.BuildServiceProvider();
        var sender = provider.GetRequiredService<ISender>();
        await sender.SendAsync(new RequestExample());
        var behavior = provider.GetRequiredService<UntypedConditionalBehavior>();
        Assert.Equal(1, behavior.ExecutionCount);
    }

    [Fact]
    public async Task Conditional_untyped_behavior_skips_when_predicate_false() {
        var services = new ServiceCollection();
        services.AddMediator(cfg => {
            cfg.RegisterRequestHandler<RequestExampleHandler, RequestExample>();
            cfg.RegisterConditionalRequestPipelineBehavior<UntypedConditionalBehavior>((_, _) => false);
        });
        var provider = services.BuildServiceProvider();
        var sender = provider.GetRequiredService<ISender>();
        await sender.SendAsync(new RequestExample());
        var behavior = provider.GetRequiredService<UntypedConditionalBehavior>();
        Assert.Equal(0, behavior.ExecutionCount);
    }
}

