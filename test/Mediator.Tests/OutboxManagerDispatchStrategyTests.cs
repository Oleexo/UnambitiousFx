using JetBrains.Annotations;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NSubstitute;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Mediator.Abstractions;
using UnambitiousFx.Mediator.Orchestrators;
using UnambitiousFx.Mediator.Tests.Definitions;
using UnambitiousFx.Mediator.Transports.Abstractions;
using UnambitiousFx.Mediator.Transports.Observability;

namespace UnambitiousFx.Mediator.Tests;

[TestSubject(typeof(OutboxManager))]
public sealed class OutboxManagerDispatchStrategyTests
{
    private readonly IEventOutboxStorage _outboxStorage;
    private readonly IEnvelopeBuilder _envelopeBuilder;
    private readonly ITransportDispatcher _transportDispatcher;
    private readonly IEventOrchestrator _eventOrchestrator;
    private readonly IEventDispatcher _eventDispatcher;
    private readonly MediatorMetrics _metrics;

    public OutboxManagerDispatchStrategyTests()
    {
        _outboxStorage = Substitute.For<IEventOutboxStorage>();
        _envelopeBuilder = Substitute.For<IEnvelopeBuilder>();
        _transportDispatcher = Substitute.For<ITransportDispatcher>();
        _eventOrchestrator = Substitute.For<IEventOrchestrator>();
        _eventDispatcher = Substitute.For<IEventDispatcher>();
        _metrics = Substitute.For<MediatorMetrics>();
        
        // Setup default returns
        _outboxStorage.AddAsync(Arg.Any<EventExample>(), Arg.Any<DistributionMode>(), Arg.Any<CancellationToken>())
            .Returns(Result.Success());
        _outboxStorage.MarkAsProcessedAsync(Arg.Any<IEvent>(), Arg.Any<CancellationToken>())
            .Returns(Result.Success());
        _eventOrchestrator.RunAsync(Arg.Any<IEventHandler<EventExample>[]>(), Arg.Any<EventExample>(), Arg.Any<CancellationToken>())
            .Returns(Result.Success());
        _envelopeBuilder.Build(Arg.Any<EventExample>())
            .Returns(new MessageEnvelope
            {
                MessageId = Guid.NewGuid().ToString(),
                CorrelationId = Guid.NewGuid().ToString(),
                Timestamp = DateTimeOffset.UtcNow,
                PayloadType = typeof(EventExample).FullName!,
                Payload = new EventExample("Test"),
                Headers = new MessageHeaders()
            });
    }

    [Fact]
    public async Task StoreAndDispatchAsync_WithImmediateStrategy_DispatchesImmediately()
    {
        // Arrange
        var dispatcherOptions = Options.Create(new EventDispatcherOptions
        {
            DispatchStrategy = DispatchStrategy.Immediate
        });
        var outboxOptions = Options.Create(new OutboxOptions());
        
        var manager = new OutboxManager(
            _outboxStorage,
            _envelopeBuilder,
            _transportDispatcher,
            _eventOrchestrator,
            _eventDispatcher,
            dispatcherOptions,
            outboxOptions,
            NullLogger<OutboxManager>.Instance,
            _metrics);
        
        var @event = new EventExample("Test Event");
        var handlers = Array.Empty<IEventHandler<EventExample>>();

        // Act
        var result = await manager.StoreAndDispatchAsync(
            @event,
            handlers,
            DistributionMode.LocalOnly,
            null,
            CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        await _outboxStorage.Received(1).AddAsync(@event, DistributionMode.LocalOnly, Arg.Any<CancellationToken>());
        await _eventOrchestrator.Received(1).RunAsync(handlers, @event, Arg.Any<CancellationToken>());
        await _outboxStorage.Received(1).MarkAsProcessedAsync(@event, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task StoreAndDispatchAsync_WithDeferredStrategy_ReturnsImmediately()
    {
        // Arrange
        var dispatcherOptions = Options.Create(new EventDispatcherOptions
        {
            DispatchStrategy = DispatchStrategy.Deferred
        });
        var outboxOptions = Options.Create(new OutboxOptions());
        
        var manager = new OutboxManager(
            _outboxStorage,
            _envelopeBuilder,
            _transportDispatcher,
            _eventOrchestrator,
            _eventDispatcher,
            dispatcherOptions,
            outboxOptions,
            NullLogger<OutboxManager>.Instance,
            _metrics);
        
        var @event = new EventExample("Test Event");
        var handlers = Array.Empty<IEventHandler<EventExample>>();

        // Act
        var result = await manager.StoreAndDispatchAsync(
            @event,
            handlers,
            DistributionMode.LocalOnly,
            null,
            CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        await _outboxStorage.Received(1).AddAsync(@event, DistributionMode.LocalOnly, Arg.Any<CancellationToken>());
        // Should NOT dispatch immediately
        await _eventOrchestrator.DidNotReceive().RunAsync(Arg.Any<IEventHandler<EventExample>[]>(), Arg.Any<EventExample>(), Arg.Any<CancellationToken>());
        await _outboxStorage.DidNotReceive().MarkAsProcessedAsync(Arg.Any<IEvent>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task StoreAndDispatchAsync_WithBatchedStrategy_ReturnsImmediately()
    {
        // Arrange
        var dispatcherOptions = Options.Create(new EventDispatcherOptions
        {
            DispatchStrategy = DispatchStrategy.Batched,
            BatchSize = 10
        });
        var outboxOptions = Options.Create(new OutboxOptions());
        
        var manager = new OutboxManager(
            _outboxStorage,
            _envelopeBuilder,
            _transportDispatcher,
            _eventOrchestrator,
            _eventDispatcher,
            dispatcherOptions,
            outboxOptions,
            NullLogger<OutboxManager>.Instance,
            _metrics);
        
        var @event = new EventExample("Test Event");
        var handlers = Array.Empty<IEventHandler<EventExample>>();

        // Act
        var result = await manager.StoreAndDispatchAsync(
            @event,
            handlers,
            DistributionMode.LocalOnly,
            null,
            CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        await _outboxStorage.Received(1).AddAsync(@event, DistributionMode.LocalOnly, Arg.Any<CancellationToken>());
        // Should NOT dispatch immediately (batched for later)
        await _eventOrchestrator.DidNotReceive().RunAsync(Arg.Any<IEventHandler<EventExample>[]>(), Arg.Any<EventExample>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task StoreAndDispatchAsync_WhenStorageFails_ReturnsFailure()
    {
        // Arrange
        _outboxStorage.AddAsync(Arg.Any<EventExample>(), Arg.Any<DistributionMode>(), Arg.Any<CancellationToken>())
            .Returns(Result.Failure("Storage error"));
        
        var dispatcherOptions = Options.Create(new EventDispatcherOptions
        {
            DispatchStrategy = DispatchStrategy.Immediate
        });
        var outboxOptions = Options.Create(new OutboxOptions());
        
        var manager = new OutboxManager(
            _outboxStorage,
            _envelopeBuilder,
            _transportDispatcher,
            _eventOrchestrator,
            _eventDispatcher,
            dispatcherOptions,
            outboxOptions,
            NullLogger<OutboxManager>.Instance,
            _metrics);
        
        var @event = new EventExample("Test Event");
        var handlers = Array.Empty<IEventHandler<EventExample>>();

        // Act
        var result = await manager.StoreAndDispatchAsync(
            @event,
            handlers,
            DistributionMode.LocalOnly,
            null,
            CancellationToken.None);

        // Assert
        Assert.True(result.IsFaulted);
        // Should NOT attempt dispatch if storage fails
        await _eventOrchestrator.DidNotReceive().RunAsync(Arg.Any<IEventHandler<EventExample>[]>(), Arg.Any<EventExample>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task StoreAndDispatchAsync_WhenDispatchFails_MarksAsFailed()
    {
        // Arrange
        _eventOrchestrator.RunAsync(Arg.Any<IEventHandler<EventExample>[]>(), Arg.Any<EventExample>(), Arg.Any<CancellationToken>())
            .Returns(Result.Failure("Handler error"));
        _outboxStorage.GetAttemptCountAsync(Arg.Any<IEvent>(), Arg.Any<CancellationToken>())
            .Returns(0);
        _outboxStorage.MarkAsFailedAsync(Arg.Any<IEvent>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<DateTimeOffset?>(), Arg.Any<CancellationToken>())
            .Returns(Result.Success());
        
        var dispatcherOptions = Options.Create(new EventDispatcherOptions
        {
            DispatchStrategy = DispatchStrategy.Immediate
        });
        var outboxOptions = Options.Create(new OutboxOptions
        {
            MaxRetryAttempts = 3,
            InitialRetryDelay = TimeSpan.FromSeconds(1),
            BackoffFactor = 2.0
        });
        
        var manager = new OutboxManager(
            _outboxStorage,
            _envelopeBuilder,
            _transportDispatcher,
            _eventOrchestrator,
            _eventDispatcher,
            dispatcherOptions,
            outboxOptions,
            NullLogger<OutboxManager>.Instance,
            _metrics);
        
        var @event = new EventExample("Test Event");
        var handlers = Array.Empty<IEventHandler<EventExample>>();

        // Act
        var result = await manager.StoreAndDispatchAsync(
            @event,
            handlers,
            DistributionMode.LocalOnly,
            null,
            CancellationToken.None);

        // Assert
        Assert.True(result.IsFaulted);
        await _outboxStorage.Received(1).MarkAsFailedAsync(
            @event,
            Arg.Any<string>(),
            false, // Should not dead-letter on first failure
            Arg.Is<DateTimeOffset?>(dt => dt.HasValue), // Should schedule retry
            Arg.Any<CancellationToken>());
        await _outboxStorage.DidNotReceive().MarkAsProcessedAsync(Arg.Any<IEvent>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ProcessPendingAsync_WithRetryLogic_CalculatesExponentialBackoff()
    {
        // Arrange
        var @event = new EventExample("Test Event");
        _outboxStorage.GetPendingEventsAsync(Arg.Any<CancellationToken>())
            .Returns(new[] { @event });
        _outboxStorage.GetDistributionModeAsync(@event, Arg.Any<CancellationToken>())
            .Returns(DistributionMode.LocalOnly);
        _outboxStorage.GetAttemptCountAsync(@event, Arg.Any<CancellationToken>())
            .Returns(2); // Third attempt (0-indexed: 0, 1, 2)
        
        var dispatcherOptions = Options.Create(new EventDispatcherOptions
        {
            Dispatchers = new Dictionary<Type, DispatchEventDelegate>
            {
                [typeof(EventExample)] = async (evt, dispatcher, mode, ct) =>
                {
                    return Result.Failure("Simulated failure");
                }
            }
        });
        var outboxOptions = Options.Create(new OutboxOptions
        {
            MaxRetryAttempts = 5,
            InitialRetryDelay = TimeSpan.FromSeconds(1),
            BackoffFactor = 2.0,
            BatchSize = 10
        });
        
        var manager = new OutboxManager(
            _outboxStorage,
            _envelopeBuilder,
            _transportDispatcher,
            _eventOrchestrator,
            _eventDispatcher,
            dispatcherOptions,
            outboxOptions,
            NullLogger<OutboxManager>.Instance,
            _metrics);

        // Act
        await manager.ProcessPendingAsync(CancellationToken.None);

        // Assert - Exponential backoff: 1000ms * (2.0 ^ 2) = 4000ms
        await _outboxStorage.Received(1).MarkAsFailedAsync(
            @event,
            Arg.Any<string>(),
            false,
            Arg.Is<DateTimeOffset?>(dt => 
                dt.HasValue && 
                dt.Value > DateTimeOffset.UtcNow.AddMilliseconds(3900) && 
                dt.Value < DateTimeOffset.UtcNow.AddMilliseconds(4100)),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ProcessPendingAsync_WhenMaxRetriesExceeded_MovesToDeadLetter()
    {
        // Arrange
        var @event = new EventExample("Test Event");
        _outboxStorage.GetPendingEventsAsync(Arg.Any<CancellationToken>())
            .Returns(new[] { @event });
        _outboxStorage.GetDistributionModeAsync(@event, Arg.Any<CancellationToken>())
            .Returns(DistributionMode.LocalOnly);
        _outboxStorage.GetAttemptCountAsync(@event, Arg.Any<CancellationToken>())
            .Returns(2); // This will be attempt 3, which equals MaxRetryAttempts
        
        var dispatcherOptions = Options.Create(new EventDispatcherOptions
        {
            Dispatchers = new Dictionary<Type, DispatchEventDelegate>
            {
                [typeof(EventExample)] = async (evt, dispatcher, mode, ct) =>
                {
                    return Result.Failure("Simulated failure");
                }
            }
        });
        var outboxOptions = Options.Create(new OutboxOptions
        {
            MaxRetryAttempts = 3,
            InitialRetryDelay = TimeSpan.FromSeconds(1),
            BackoffFactor = 2.0,
            BatchSize = 10
        });
        
        var manager = new OutboxManager(
            _outboxStorage,
            _envelopeBuilder,
            _transportDispatcher,
            _eventOrchestrator,
            _eventDispatcher,
            dispatcherOptions,
            outboxOptions,
            NullLogger<OutboxManager>.Instance,
            _metrics);

        // Act
        await manager.ProcessPendingAsync(CancellationToken.None);

        // Assert - Should move to dead-letter
        await _outboxStorage.Received(1).MarkAsFailedAsync(
            @event,
            Arg.Any<string>(),
            true, // Should dead-letter
            null, // No next attempt when dead-lettered
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ProcessPendingAsync_WithBatchSize_LimitsProcessedEvents()
    {
        // Arrange
        var events = Enumerable.Range(1, 20)
            .Select(i => new EventExample($"Event {i}") as IEvent)
            .ToList();
        
        _outboxStorage.GetPendingEventsAsync(Arg.Any<CancellationToken>())
            .Returns(events);
        
        foreach (var evt in events)
        {
            _outboxStorage.GetDistributionModeAsync(evt, Arg.Any<CancellationToken>())
                .Returns(DistributionMode.LocalOnly);
        }
        
        var dispatcherOptions = Options.Create(new EventDispatcherOptions
        {
            Dispatchers = new Dictionary<Type, DispatchEventDelegate>
            {
                [typeof(EventExample)] = async (evt, dispatcher, mode, ct) =>
                {
                    return Result.Success();
                }
            }
        });
        var outboxOptions = Options.Create(new OutboxOptions
        {
            BatchSize = 5 // Only process 5 events
        });
        
        var manager = new OutboxManager(
            _outboxStorage,
            _envelopeBuilder,
            _transportDispatcher,
            _eventOrchestrator,
            _eventDispatcher,
            dispatcherOptions,
            outboxOptions,
            NullLogger<OutboxManager>.Instance,
            _metrics);

        // Act
        await manager.ProcessPendingAsync(CancellationToken.None);

        // Assert - Should only process 5 events
        await _outboxStorage.Received(5).MarkAsProcessedAsync(Arg.Any<IEvent>(), Arg.Any<CancellationToken>());
    }
}
