using Microsoft.Extensions.DependencyInjection;
using Oleexo.UnambitiousFx.Mediator.Abstractions;
using Oleexo.UnambitiousFx.Mediator.Tests.Definitions;

namespace Oleexo.UnambitiousFx.Mediator.Tests;

public sealed class ServiceCollectionExtensionsTests {
    [Fact]
    public async Task GivenRequest_WhenResolve_ThenReturnResult() {
        var services = new ServiceCollection()
                      .RegisterHandler<RequestExampleHandler, RequestExample, int>()
                      .BuildServiceProvider();

        var handler = services.GetRequiredService<IRequestHandler<RequestExample, int>>();
        var ctx     = new Context();

        var result = await handler.HandleAsync(ctx, new RequestExample(), CancellationToken.None);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task GivenRequestWithBehavior_WhenResolve_ThenReturnResult() {
        var services = new ServiceCollection()
                      .RegisterHandler<RequestExampleHandler, RequestExample, int>(ServiceLifetime.Transient)
                      .RegisterRequestPipelineBehavior(typeof(TestRequestPipelineBehavior<,>))
                      .BuildServiceProvider();

        var handler = services.GetRequiredService<IRequestHandler<RequestExample, int>>();
        var ctx     = new Context();

        var result = await handler.HandleAsync(ctx, new RequestExample(), CancellationToken.None);

        Assert.True(result.IsSuccess);
    }
}
