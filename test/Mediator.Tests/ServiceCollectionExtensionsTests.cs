using Microsoft.Extensions.DependencyInjection;
using Oleexo.UnambitiousFx.Mediator.Tests.Definitions;

namespace Oleexo.UnambitiousFx.Mediator.Tests;

public sealed class ServiceCollectionExtensionsTests {
    [Fact]
    public async Task GivenRequest_WhenResolve_ThenReturnResult() {
        var services = new ServiceCollection()
                      .RegisterHandler<RequestExampleHandler, RequestExample, int>()
                      .BuildServiceProvider();

        var handler = services.GetRequiredService<IRequestHandler<RequestExample, int>>();

        var result = await handler.HandleAsync(new RequestExample(), CancellationToken.None);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task GivenMutation_WhenResolve_ThenReturnResult() {
        var services = new ServiceCollection()
                      .RegisterMutationHandler<MutationExampleHandler, MutationExample, Unit>()
                      .BuildServiceProvider();

        var handler = services.GetRequiredService<IMutationHandler<MutationExample, Unit>>();

        var result = await handler.HandleAsync(new MutationExample(), CancellationToken.None);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task GivenQuery_WhenResolve_ThenReturnResult() {
        var services = new ServiceCollection()
                      .RegisterQueryHandler<QueryExampleHandler, QueryExample, int>()
                      .BuildServiceProvider();

        var handler = services.GetRequiredService<IQueryHandler<QueryExample, int>>();

        var result = await handler.HandleAsync(new QueryExample(), CancellationToken.None);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task GivenRequestWithBehavior_WhenResolve_ThenReturnResult() {
        var services = new ServiceCollection()
                      .RegisterHandler<RequestExampleHandler, RequestExample, int>(ServiceLifetime.Transient)
                      .RegisterRequestPipelineBehavior(typeof(TestRequestPipelineBehavior<,>))
                      .BuildServiceProvider();

        var handler = services.GetRequiredService<IRequestHandler<RequestExample, int>>();

        var result = await handler.HandleAsync(new RequestExample(), CancellationToken.None);

        Assert.True(result.IsSuccess);
    }
}
