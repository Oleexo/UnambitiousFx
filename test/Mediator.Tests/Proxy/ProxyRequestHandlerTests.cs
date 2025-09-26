using NSubstitute;
using UnambitiousFx.Mediator.Abstractions;
using UnambitiousFx.Mediator.Tests.Definitions;

namespace UnambitiousFx.Mediator.Tests;

public sealed class ProxyRequestHandlerTests {
    [Fact]
    public async Task GivenARequestPipelineBehavior_WhenProxyHandle_ShouldCallTheBehavior() {
        var order     = 0;
        var publisher = Substitute.For<IPublisher>();
        var handler   = new RequestWithResponseExampleHandler();
        handler.OnExecuted = () => {
            Assert.Equal(1, order);
            order++;
        };
        var behavior = new TestRequestPipelineBehavior();
        behavior.OnExecuted = () => {
            Assert.Equal(0, order);
            order++;
        };
        var proxy   = new ProxyRequestHandler<RequestWithResponseExampleHandler, RequestWithResponseExample, int>(handler, [behavior]);
        var request = new RequestWithResponseExample();

        var result = await proxy.HandleAsync(new Context(publisher), request, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.True(handler.Executed);
        Assert.Equal(1, handler.ExecutionCount);
        Assert.True(behavior.Executed);
        Assert.Equal(1, behavior.ExecutionCount);
    }
}
