using NSubstitute;
using UnambitiousFx.Core.Options;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Mediator.Abstractions;
using UnambitiousFx.Mediator.Resolvers;
using UnambitiousFx.Mediator.Tests.Definitions;

namespace UnambitiousFx.Mediator.Tests;

public sealed class SenderTests {
    private readonly IContextFactory     _contextFactory;
    private readonly IDependencyResolver _resolver;
    private readonly Sender              _sender;

    public SenderTests() {
        _resolver       = Substitute.For<IDependencyResolver>();
        _contextFactory = Substitute.For<IContextFactory>();

        _sender = new Sender(_resolver, _contextFactory);
    }

    [Fact]
    public async Task GivenAValidHandlerWithResponse_WhenHandleAsync_ShouldReturnAResponse() {
        // Arrange
        var request = new RequestWithResponseExample();
        var handler = Substitute.For<IRequestHandler<RequestWithResponseExample, int>>();
        var context = Substitute.For<IContext>();

        _resolver.GetService<IRequestHandler<RequestWithResponseExample, int>>()
                 .Returns(Option.Some(handler));
        _contextFactory.Create()
                       .Returns(context);
        handler.HandleAsync(context, request, CancellationToken.None)
               .Returns(Result.Success(42));

        // Act
        var result = await _sender.SendAsync<RequestWithResponseExample, int>(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        if (result.Ok(out var value)) {
            Assert.Equal(42, value);
        }
        else {
            Assert.Fail("Result should be successful but was marked as failed");
        }
    }

    [Fact]
    public async Task GivenNoHandlerWithResponse_WhenHandleAsync_ShouldThrowAnException() {
        // Arrange
        var request = new RequestWithResponseExample();
        _resolver.GetService<IRequestHandler<RequestWithResponseExample, int>>()
                 .Returns(Option<IRequestHandler<RequestWithResponseExample, int>>.None());

        // Act & Assert
        await Assert.ThrowsAsync<MissingHandlerException>(() => _sender.SendAsync<RequestWithResponseExample, int>(request, CancellationToken.None)
                                                                       .AsTask());
    }

    [Fact]
    public async Task GivenAValidHandlerWithoutResponse_WhenHandleAsync_ShouldReturnAResponse() {
        // Arrange
        var request = new RequestExample();
        var handler = Substitute.For<IRequestHandler<RequestExample>>();
        var context = Substitute.For<IContext>();

        _resolver.GetService<IRequestHandler<RequestExample>>()
                 .Returns(Option.Some(handler));
        _contextFactory.Create()
                       .Returns(context);
        handler.HandleAsync(context, request, CancellationToken.None)
               .Returns(Result.Success());

        // Act
        var result = await _sender.SendAsync(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task GivenNoHandlerWithoutResponse_WhenHandleAsync_ShouldThrowAnException() {
        // Arrange
        var request = new RequestWithResponseExample();
        _resolver.GetService<IRequestHandler<RequestWithResponseExample, int>>()
                 .Returns(Option.None<IRequestHandler<RequestWithResponseExample, int>>());

        // Act & Assert
        await Assert.ThrowsAsync<MissingHandlerException>(() => _sender.SendAsync<RequestWithResponseExample, int>(request, CancellationToken.None)
                                                                       .AsTask());
    }
}
