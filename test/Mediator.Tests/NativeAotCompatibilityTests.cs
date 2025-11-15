using JetBrains.Annotations;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NSubstitute;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Mediator.Abstractions;
using UnambitiousFx.Mediator.Orchestrators;
using UnambitiousFx.Mediator.Resolvers;
using UnambitiousFx.Mediator.Tests.Definitions;
using UnambitiousFx.Mediator.Transports.Abstractions;
using UnambitiousFx.Mediator.Transports.Observability;

namespace UnambitiousFx.Mediator.Tests;

[TestSubject(typeof(EventDispatcher))]
public sealed class NativeAotCompatibilityTests
{
    [Fact]
    public void DispatchEventDelegate_CanBeRegisteredWithoutReflection()
    {
        // Arrange
        var options = new EventDispatcherOptions();
        
        // Act - Register dispatcher delegate without using reflection
        options.Dispatchers[typeof(EventExample)] = async (@event, dispatcher, distributionMode, ct) =>
        {
            var typedEvent = (EventExample)@event;
            return await dispatcher.DispatchFromOutboxAsync(typedEvent, distributionMode, ct);
        };

        // Assert
        Assert.Single(options.Dispatchers);
        Assert.True(options.Dispatchers.ContainsKey(typeof(EventExample)));
    }

    [Fact]
    public async Task DispatchEventDelegate_MaintainsGenericTypeInformation()
    {
        // Arrange
        var dependencyResolver = Substitute.For<IDependencyResolver>();
        var eventOrchestrator = Substitute.For<IEventOrchestrator>();
        var traitsRegistry = Substitute.For<IMessageTraitsRegistry>();
        var outboxManager = Substitute.For<OutboxManager>();
        var metrics = Substitute.For<MediatorMetrics>();
        
        dependencyResolver.GetServices<IEventHandler<EventExample>>()
            .Returns(Array.Empty<IEventHandler<EventExample>>());
        dependencyResolver.GetServices<IEventPipelineBehavior>()
            .Returns(Array.Empty<IEventPipelineBehavior>());
        outboxManager.ExecuteDirectAsync(Arg.Any<EventExample>(), Arg.Any<IEventHandler<EventExample>[]>(), 
            Arg.Any<DistributionMode>(), Arg.Any<MessageTraits?>(), Arg.Any<CancellationToken>())
            .Returns(Result.Success());
        
        var options = Options.Create(new EventDispatcherOptions
        {
            Dispatchers = new Dictionary<Type, DispatchEventDelegate>
            {
                [typeof(EventExample)] = async (@event, dispatcher, distributionMode, ct) =>
                {
                    var typedEvent = (EventExample)@event;
                    return await dispatcher.DispatchFromOutboxAsync(typedEvent, distributionMode, ct);
                }
            }
        });
        
        var eventDispatcher = new EventDispatcher(
            dependencyResolver,
            eventOrchestrator,
            traitsRegistry,
            Array.Empty<IEventRoutingFilter>(),
            outboxManager,
            options,
            NullLogger<EventDispatcher>.Instance,
            metrics);
        
        var @event = new EventExample("Test Event");

        // Act - Call through delegate (simulating outbox replay)
        var dispatcher = options.Value.Dispatchers[typeof(EventExample)];
        var result = await dispatcher(@event, eventDispatcher, DistributionMode.LocalOnly, CancellationToken.None);

        // Assert - Verify generic method was called with correct type
        Assert.True(result.IsSuccess);
        await outboxManager.Received(1).ExecuteDirectAsync(
            Arg.Is<EventExample>(e => e.Name == "Test Event"),
            Arg.Any<IEventHandler<EventExample>[]>(),
            DistributionMode.LocalOnly,
            Arg.Any<MessageTraits?>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task OutboxManager_ProcessPendingAsync_UsesDispatcherDelegateWithoutReflection()
    {
        // Arrange
        var outboxStorage = Substitute.For<IEventOutboxStorage>();
        var envelopeBuilder = Substitute.For<IEnvelopeBuilder>();
        var transportDispatcher = Substitute.For<ITransportDispatcher>();
        var eventOrchestrator = Substitute.For<IEventOrchestrator>();
        var eventDispatcher = Substitute.For<IEventDispatcher>();
        var metrics = Substitute.For<MediatorMetrics>();
        
        var @event = new EventExample("Test Event");
        outboxStorage.GetPendingEventsAsync(Arg.Any<CancellationToken>())
            .Returns(new[] { @event });
        outboxStorage.GetDistributionModeAsync(@event, Arg.Any<CancellationToken>())
            .Returns(DistributionMode.LocalOnly);
        outboxStorage.MarkAsProcessedAsync(Arg.Any<IEvent>(), Arg.Any<CancellationToken>())
            .Returns(Result.Success());
        
        var dispatcherCalled = false;
        var dispatcherOptions = Options.Create(new EventDispatcherOptions
        {
            Dispatchers = new Dictionary<Type, DispatchEventDelegate>
            {
                [typeof(EventExample)] = async (evt, dispatcher, distributionMode, ct) =>
                {
                    dispatcherCalled = true;
                    var typedEvent = (EventExample)evt;
                    Assert.Equal("Test Event", typedEvent.Name);
                    Assert.Equal(DistributionMode.LocalOnly, distributionMode);
                    return Result.Success();
                }
            }
        });
        var outboxOptions = Options.Create(new OutboxOptions());
        
        var manager = new OutboxManager(
            outboxStorage,
            envelopeBuilder,
            transportDispatcher,
            eventOrchestrator,
            eventDispatcher,
            dispatcherOptions,
            outboxOptions,
            NullLogger<OutboxManager>.Instance,
            metrics);

        // Act
        var result = await manager.ProcessPendingAsync(CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(dispatcherCalled, "Dispatcher delegate should have been called");
        await outboxStorage.Received(1).MarkAsProcessedAsync(@event, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task OutboxManager_ProcessPendingAsync_WithoutRegisteredDispatcher_ReturnsFailure()
    {
        // Arrange
        var outboxStorage = Substitute.For<IEventOutboxStorage>();
        var envelopeBuilder = Substitute.For<IEnvelopeBuilder>();
        var transportDispatcher = Substitute.For<ITransportDispatcher>();
        var eventOrchestrator = Substitute.For<IEventOrchestrator>();
        var eventDispatcher = Substitute.For<IEventDispatcher>();
        var metrics = Substitute.For<MediatorMetrics>();
        
        var @event = new EventExample("Test Event");
        outboxStorage.GetPendingEventsAsync(Arg.Any<CancellationToken>())
            .Returns(new[] { @event });
        outboxStorage.GetDistributionModeAsync(@event, Arg.Any<CancellationToken>())
            .Returns(DistributionMode.LocalOnly);
        
        var dispatcherOptions = Options.Create(new EventDispatcherOptions
        {
            Dispatchers = new Dictionary<Type, DispatchEventDelegate>() // Empty - no dispatcher registered
        });
        var outboxOptions = Options.Create(new OutboxOptions());
        
        var manager = new OutboxManager(
            outboxStorage,
            envelopeBuilder,
            transportDispatcher,
            eventOrchestrator,
            eventDispatcher,
            dispatcherOptions,
            outboxOptions,
            NullLogger<OutboxManager>.Instance,
            metrics);

        // Act
        var result = await manager.ProcessPendingAsync(CancellationToken.None);

        // Assert
        Assert.True(result.IsFaulted);
        Assert.Contains("No dispatcher registered", result.ToString());
        await outboxStorage.DidNotReceive().MarkAsProcessedAsync(Arg.Any<IEvent>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public void DispatchEventDelegate_SupportsMultipleEventTypes()
    {
        // Arrange
        var options = new EventDispatcherOptions();
        
        // Define a second event type for testing
        var secondEventType = typeof(EventExample);

        // Act - Register multiple event types
        options.Dispatchers[typeof(EventExample)] = async (@event, dispatcher, distributionMode, ct) =>
        {
            var typedEvent = (EventExample)@event;
            return await dispatcher.DispatchFromOutboxAsync(typedEvent, distributionMode, ct);
        };
        
        options.Dispatchers[secondEventType] = async (@event, dispatcher, distributionMode, ct) =>
        {
            var typedEvent = (EventExample)@event;
            return await dispatcher.DispatchFromOutboxAsync(typedEvent, distributionMode, ct);
        };

        // Assert
        Assert.Equal(2, options.Dispatchers.Count);
        Assert.All(options.Dispatchers.Values, dispatcher => Assert.NotNull(dispatcher));
    }

    [Fact]
    public async Task DispatchFromOutboxAsync_SkipsOutboxStorage()
    {
        // Arrange
        var dependencyResolver = Substitute.For<IDependencyResolver>();
        var eventOrchestrator = Substitute.For<IEventOrchestrator>();
        var traitsRegistry = Substitute.For<IMessageTraitsRegistry>();
        var outboxManager = Substitute.For<OutboxManager>();
        var metrics = Substitute.For<MediatorMetrics>();
        
        dependencyResolver.GetServices<IEventHandler<EventExample>>()
            .Returns(Array.Empty<IEventHandler<EventExample>>());
        dependencyResolver.GetServices<IEventPipelineBehavior>()
            .Returns(Array.Empty<IEventPipelineBehavior>());
        outboxManager.ExecuteDirectAsync(Arg.Any<EventExample>(), Arg.Any<IEventHandler<EventExample>[]>(), 
            Arg.Any<DistributionMode>(), Arg.Any<MessageTraits?>(), Arg.Any<CancellationToken>())
            .Returns(Result.Success());
        
        var options = Options.Create(new EventDispatcherOptions());
        
        var eventDispatcher = new EventDispatcher(
            dependencyResolver,
            eventOrchestrator,
            traitsRegistry,
            Array.Empty<IEventRoutingFilter>(),
            outboxManager,
            options,
            NullLogger<EventDispatcher>.Instance,
            metrics);
        
        var @event = new EventExample("Test Event");

        // Act - Call DispatchFromOutboxAsync directly
        var result = await eventDispatcher.DispatchFromOutboxAsync(
            @event,
            DistributionMode.LocalOnly,
            CancellationToken.None);

        // Assert - Should call ExecuteDirectAsync (not StoreAndDispatchAsync)
        Assert.True(result.IsSuccess);
        await outboxManager.Received(1).ExecuteDirectAsync(
            @event,
            Arg.Any<IEventHandler<EventExample>[]>(),
            DistributionMode.LocalOnly,
            Arg.Any<MessageTraits?>(),
            Arg.Any<CancellationToken>());
        await outboxManager.DidNotReceive().StoreAndDispatchAsync(
            Arg.Any<EventExample>(),
            Arg.Any<IEventHandler<EventExample>[]>(),
            Arg.Any<DistributionMode>(),
            Arg.Any<MessageTraits?>(),
            Arg.Any<CancellationToken>());
    }
}
