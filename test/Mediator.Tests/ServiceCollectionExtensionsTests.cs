using Microsoft.Extensions.DependencyInjection;
using Oleexo.UnambitiousFx.Mediator.Abstractions;
using Oleexo.UnambitiousFx.Mediator.Tests.Definitions;

namespace Oleexo.UnambitiousFx.Mediator.Tests;

public sealed class ServiceCollectionExtensionsTests {
    [Fact]
    public async Task GivenRequest_WhenResolve_ThenReturnResult() {
        var services = new ServiceCollection()
                      .AddMediator(cfg => { cfg.RegisterHandler<RequestExampleHandler, RequestExample, int>(); })
                      .BuildServiceProvider();

        var handler = services.GetRequiredService<IRequestHandler<RequestExample, int>>();
        var ctx = services.GetRequiredService<IContextFactory>()
                          .Create();

        var result = await handler.HandleAsync(ctx, new RequestExample(), CancellationToken.None);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task GivenRequestWithBehavior_WhenResolve_ThenReturnResult() {
        var services = new ServiceCollection()
                      .AddMediator(cfg => {
                           cfg.RegisterHandler<RequestExampleHandler, RequestExample, int>();
                           cfg.RegisterRequestPipelineBehavior<TestRequestPipelineBehavior>();
                       })
                      .BuildServiceProvider();

        var handler = services.GetRequiredService<IRequestHandler<RequestExample, int>>();
        var ctx = services.GetRequiredService<IContextFactory>()
                          .Create();

        var result = await handler.HandleAsync(ctx, new RequestExample(), CancellationToken.None);

        Assert.True(result.IsSuccess);
    }
}
