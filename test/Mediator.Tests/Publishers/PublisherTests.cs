using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using NSubstitute;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Mediator.Abstractions;
using UnambitiousFx.Mediator.Tests.Definitions;

namespace UnambitiousFx.Mediator.Tests.Publishers;

[TestSubject(typeof(Publisher))]
public sealed class PublisherTests {
    private readonly IEventDispatcher    _eventDispatcher;
    private readonly IEventOutboxStorage _eventOutboxStorage;
    private readonly Publisher           _publisher;

    public PublisherTests() {
        _eventDispatcher    = Substitute.For<IEventDispatcher>();
        _eventOutboxStorage = Substitute.For<IEventOutboxStorage>();

        _publisher = new Publisher(
            _eventDispatcher,
            _eventOutboxStorage,
            Options.Create(new PublisherOptions())
        );
    }

    [Fact]
    public async Task GivenAnEvent_WhenPublish_ShouldDispatchEvent() {
        var @event = new EventExample();
        _eventDispatcher.DispatchAsync(Arg.Any<IContext>(), @event, Arg.Any<CancellationToken>())
                        .Returns(Result.Success());

        var result = await _publisher.PublishAsync(Substitute.For<IContext>(),
                                                   @event,
                                                   CancellationToken.None);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task GivenAnEvent_WhenPublishWithOutbox_ShouldStoreTheEvent() {
        var @event = new EventExample();
        _eventOutboxStorage.AddAsync(@event, Arg.Any<CancellationToken>())
                           .Returns(Result.Success());

        var result = await _publisher.PublishAsync(Substitute.For<IContext>(),
                                                   @event,
                                                   PublishMode.Outbox,
                                                   CancellationToken.None);

        Assert.True(result.IsSuccess);
    }
}
