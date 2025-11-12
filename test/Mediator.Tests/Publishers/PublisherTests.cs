using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using NSubstitute;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Mediator.Abstractions;
using UnambitiousFx.Mediator.Tests.Definitions;

namespace UnambitiousFx.Mediator.Tests.Publishers;

[TestSubject(typeof(Publisher))]
public sealed class PublisherTests
{
    private readonly IEventDispatcher _eventDispatcher;
    private readonly OutboxManager _outboxManager;
    private readonly Publisher _publisher;

    public PublisherTests()
    {
        _eventDispatcher = Substitute.For<IEventDispatcher>();
        _outboxManager = Substitute.For<OutboxManager>();

        _publisher = new Publisher(
            _eventDispatcher,
            _outboxManager,
            Options.Create(new PublisherOptions())
        );
    }

    [Fact]
    public async Task GivenAnEvent_WhenPublish_ShouldDispatchEvent()
    {
        var @event = new EventExample("Event 1");
        _eventDispatcher.DispatchAsync(@event, Arg.Any<CancellationToken>())
                        .Returns(Result.Success());

        var result = await _publisher.PublishAsync(
                         @event,
                         CancellationToken.None);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task GivenAnEvent_WhenPublishWithOutbox_ShouldDispatchEvent()
    {
        var @event = new EventExample("Event 1");
        _eventDispatcher.DispatchAsync(@event, Arg.Any<CancellationToken>())
                        .Returns(Result.Success());

        var result = await _publisher.PublishAsync(
                         @event,
                         PublishMode.Outbox,
                         CancellationToken.None);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task GivenOutboxEvent_WhenCommit_ShouldDelegateToOutboxManager()
    {
        var @event = new EventExample("Event 1");
        _outboxManager.ProcessPendingAsync(Arg.Any<CancellationToken>())
                      .Returns(Result.Success());

        var result = await _publisher.CommitAsync(CancellationToken.None);

        Assert.True(result.IsSuccess);
        await _outboxManager.Received(1).ProcessPendingAsync(Arg.Any<CancellationToken>());
    }
}
