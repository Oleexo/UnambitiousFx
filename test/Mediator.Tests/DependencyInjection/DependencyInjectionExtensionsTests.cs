using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using UnambitiousFx.Mediator.Abstractions;
using UnambitiousFx.Mediator.Tests.Definitions;

namespace UnambitiousFx.Mediator.Tests.DependencyInjection;

[TestSubject(typeof(DependencyInjectionExtensions))]
public sealed class DependencyInjectionExtensionsTests {
    [Fact]
    public async Task GivenRequest_WhenResolve_ThenReturnResult() {
        var services = new ServiceCollection()
                      .AddMediator(cfg => { cfg.RegisterRequestHandler<RequestWithResponseExampleHandler, RequestWithResponseExample, int>(); })
                      .BuildServiceProvider();

        var handler = services.GetRequiredService<IRequestHandler<RequestWithResponseExample, int>>();
        var ctx = services.GetRequiredService<IContextFactory>()
                          .Create();

        var result = await handler.HandleAsync(ctx, new RequestWithResponseExample(), CancellationToken.None);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task GivenRequestWithBehavior_WhenResolve_ThenReturnResult() {
        var services = new ServiceCollection()
                      .AddMediator(cfg => {
                           cfg.RegisterRequestHandler<RequestWithResponseExampleHandler, RequestWithResponseExample, int>();
                           cfg.RegisterRequestPipelineBehavior<TestRequestPipelineBehavior>();
                       })
                      .BuildServiceProvider();

        var handler = services.GetRequiredService<IRequestHandler<RequestWithResponseExample, int>>();
        var ctx = services.GetRequiredService<IContextFactory>()
                          .Create();

        var result = await handler.HandleAsync(ctx, new RequestWithResponseExample(), CancellationToken.None);

        Assert.True(result.IsSuccess);
    }
}
