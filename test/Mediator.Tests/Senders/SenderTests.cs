using NSubstitute;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Mediator.Abstractions;
using UnambitiousFx.Mediator.Resolvers;
using UnambitiousFx.Mediator.Tests.Definitions;

namespace UnambitiousFx.Mediator.Tests.Senders;

public sealed class SenderTests {
    private readonly IDependencyResolver _resolver;
    private readonly Sender              _sender;

    public SenderTests() {
        _resolver = Substitute.For<IDependencyResolver>();

        _sender = new Sender(_resolver);
    }

    [Fact]
    public async Task GivenAValidHandlerWithResponse_WhenHandleAsync_ShouldReturnAResponse() {
        // Arrange
        var request = new RequestWithResponseExample();
        var handler = Substitute.For<IRequestHandler<RequestWithResponseExample, int>>();

        _resolver.GetRequiredService<IRequestHandler<RequestWithResponseExample, int>>()
                 .Returns(handler);
        handler.HandleAsync(request, CancellationToken.None)
               .Returns(Result.Success(42));

        // Act
        var result = await _sender.SendAsync<RequestWithResponseExample, int>(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        if (result.TryGet(out var value)) {
            Assert.Equal(42, value);
        }
        else {
            Assert.Fail("Result should be successful but was marked as failed");
        }
    }

    [Fact]
    public async Task GivenAValidHandlerWithoutResponse_WhenHandleAsync_ShouldReturnAResponse() {
        // Arrange
        var request = new RequestExample();
        var handler = Substitute.For<IRequestHandler<RequestExample>>();

        _resolver.GetRequiredService<IRequestHandler<RequestExample>>()
                 .Returns(handler);
        handler.HandleAsync(request, CancellationToken.None)
               .Returns(Result.Success());

        // Act
        var result = await _sender.SendAsync(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
    }
}
