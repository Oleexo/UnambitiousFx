using Oleexo.UnambitiousFx.Mediator.Tests.Definitions;

namespace Oleexo.UnambitiousFx.Mediator.Tests;

public sealed class ProxyRequestHandlerTests {
    [Fact]
    public async Task GivenARequestPipelineBehavior_WhenProxyHandle_ShouldCallTheBehavior() {
        var handler  = new RequestExampleHandler();
        var behavior = new TestRequestPipelineBehavior<RequestExample, int>();
        var proxy    = new ProxyRequestHandler<RequestExampleHandler, RequestExample, int>(handler, [behavior]);
        var request  = new RequestExample();

        var result = await proxy.HandleAsync(request, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.True(behavior.IsCalled);
    }

    [Fact]
    public async Task GivenAMutationPipelineBehavior_WhenProxyHandle_ShouldCallTheBehavior() {
        var handler  = new MutationExampleHandler();
        var behavior = new TestMutationPipelineBehavior<MutationExample, Unit>();
        var proxy    = new ProxyMutationHandler<MutationExampleHandler, MutationExample, Unit>(handler, [behavior], []);
        var request  = new MutationExample();

        var result = await proxy.HandleAsync(request, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.True(behavior.IsCalled);
    }

    [Fact]
    public async Task GivenAQueryPipelineBehavior_WhenProxyHandle_ShouldCallTheBehavior() {
        var handler  = new QueryExampleHandler();
        var behavior = new TestQueryPipelineBehavior<QueryExample, int>();
        var proxy    = new ProxyQueryHandler<QueryExampleHandler, QueryExample, int>(handler, [behavior], []);
        var request  = new QueryExample();

        var result = await proxy.HandleAsync(request, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.True(behavior.IsCalled);
    }

    [Fact]
    public async Task GivenMutationAndRequestPipelineBehavior_WhenProxyHandle_ShouldCallTheBehavior() {
        var handler          = new MutationExampleHandler();
        var mutationBehavior = new TestMutationPipelineBehavior<MutationExample, Unit>();
        var requestBehavior  = new TestRequestPipelineBehavior<MutationExample, Unit>();
        var proxy = new ProxyMutationHandler<MutationExampleHandler, MutationExample, Unit>(handler, [mutationBehavior],
                                                                                            [requestBehavior]);
        var request = new MutationExample();

        var result = await proxy.HandleAsync(request, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.True(mutationBehavior.IsCalled);
        Assert.True(requestBehavior.IsCalled);
    }
}
