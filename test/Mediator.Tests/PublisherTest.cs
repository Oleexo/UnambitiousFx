using NSubstitute;
using Oleexo.UnambitiousFx.Core;
using Oleexo.UnambitiousFx.Mediator.Abstractions;
using Oleexo.UnambitiousFx.Mediator.Resolvers;
using Oleexo.UnambitiousFx.Mediator.Tests.Definitions;

namespace Oleexo.UnambitiousFx.Mediator.Tests;

public sealed class PublisherTest {
    private readonly Publisher           _publisher;
    private readonly IDependencyResolver _resolver;

    public PublisherTest() {
        _resolver  = Substitute.For<IDependencyResolver>();
        _publisher = new Publisher(_resolver);
    }

    [Fact]
    public async Task GivenValidEventHandler_WhenPublishAsync_ShouldSucceed() {
        // Arrange
        var eventData    = new EventExample();
        var proxyHandler = Substitute.For<IEventHandlerExecutor<EventExample>>();
        var context      = Substitute.For<IContext>();

        _resolver.GetService<IEventHandlerExecutor<EventExample>>()
                 .Returns(Option.Some(proxyHandler));

        proxyHandler.HandleAsync(context, eventData, CancellationToken.None)
                    .Returns(Result.Success());

        // Act
        var result = await _publisher.PublishAsync(context, eventData, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        await proxyHandler.Received(1)
                          .HandleAsync(context, eventData, CancellationToken.None);
    }

    [Fact]
    public async Task GivenNoEventHandler_WhenPublishAsync_ShouldThrowException() {
        // Arrange
        var eventData = new EventExample();
        var context   = Substitute.For<IContext>();

        _resolver.GetService<IEventHandlerExecutor<EventExample>>()
                 .Returns(Option.None<IEventHandlerExecutor<EventExample>>());

        // Act & Assert
        await Assert.ThrowsAsync<MissingHandlerException>(() =>
                                                              _publisher.PublishAsync(context, eventData, CancellationToken.None)
                                                                        .AsTask());
    }
}
