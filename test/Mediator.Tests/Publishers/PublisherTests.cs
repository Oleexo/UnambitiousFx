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
    private readonly IEventOutboxStorage _eventOutboxStorage;
    private readonly Publisher _publisher;

    public PublisherTests()
    {
        _eventDispatcher = Substitute.For<IEventDispatcher>();
        _eventOutboxStorage = Substitute.For<IEventOutboxStorage>();

        _publisher = new Publisher(
            _eventDispatcher,
            _eventOutboxStorage,
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
    public async Task GivenAnEvent_WhenPublishWithOutbox_ShouldStoreTheEvent()
    {
        var @event = new EventExample("Event 1");
        _eventOutboxStorage.AddAsync(@event, Arg.Any<CancellationToken>())
                           .Returns(Result.Success());

        var result = await _publisher.PublishAsync(
                         @event,
                         PublishMode.Outbox,
                         CancellationToken.None);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task GivenOutboxEvent_WhenDispatchFails_ShouldRetryUntilDeadLetter()
    {
        var failingDispatcher = Substitute.For<IEventDispatcher>();
        failingDispatcher.DispatchAsync(Arg.Any<IEvent>(), Arg.Any<CancellationToken>())
                         .Returns(Result.Failure(new Exception("fail")));
        var storage = new InMemoryEventOutboxStorage();
        var publisher = new Publisher(
            failingDispatcher,
            storage,
            Options.Create(new PublisherOptions { DefaultMode = PublishMode.Outbox }),
            Options.Create(new OutboxOptions { MaxRetryAttempts = 2, InitialRetryDelay = TimeSpan.Zero })
        );
        var ev = new EventExample("Event 1");

        await publisher.PublishAsync(ev, PublishMode.Outbox, CancellationToken.None);

        // first commit -> attempt 1
        await publisher.CommitAsync(CancellationToken.None);
        var attempt1 = await storage.GetAttemptCountAsync(ev, CancellationToken.None);
        Assert.Equal(1, attempt1);
        var deadLettersAfterFirst = await storage.GetDeadLetterEventsAsync(CancellationToken.None);
        Assert.DoesNotContain(ev, deadLettersAfterFirst);

        // second commit -> attempt 2 (dead letter)
        await publisher.CommitAsync(CancellationToken.None);
        var attempt2 = await storage.GetAttemptCountAsync(ev, CancellationToken.None);
        Assert.Equal(2, attempt2);
        var deadLettersAfterSecond = await storage.GetDeadLetterEventsAsync(CancellationToken.None);
        Assert.Contains(ev, deadLettersAfterSecond);
    }

    [Fact]
    public async Task GivenMultipleOutboxEvents_WhenBatchSizeIsOne_ShouldProcessOnePerCommit()
    {
        var dispatcher = Substitute.For<IEventDispatcher>();
        dispatcher.DispatchAsync(Arg.Any<IEvent>(), Arg.Any<CancellationToken>())
                  .Returns(Result.Success());
        var storage = new InMemoryEventOutboxStorage();
        var publisher = new Publisher(
            dispatcher,
            storage,
            Options.Create(new PublisherOptions { DefaultMode = PublishMode.Outbox }),
            Options.Create(new OutboxOptions { BatchSize = 1 })
        );
        var ev1 = new EventExample("Event 1");
        var ev2 = new EventExample("Event 2");
        await publisher.PublishAsync(ev1, PublishMode.Outbox, CancellationToken.None);
        await publisher.PublishAsync(ev2, PublishMode.Outbox, CancellationToken.None);

        await publisher.CommitAsync(CancellationToken.None); // processes first only
        var pendingAfterFirst = (await storage.GetPendingEventsAsync(CancellationToken.None)).ToList();
        Assert.Contains(ev2, pendingAfterFirst);
        Assert.DoesNotContain(ev1, pendingAfterFirst); // ev1 processed

        await publisher.CommitAsync(CancellationToken.None); // processes second
        var pendingAfterSecond = (await storage.GetPendingEventsAsync(CancellationToken.None)).ToList();
        Assert.DoesNotContain(ev2, pendingAfterSecond);
    }
}
