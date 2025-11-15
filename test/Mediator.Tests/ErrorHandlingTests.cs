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
public sealed class ErrorHandlingTests
{
    private readonly IEventOutboxStorage _outboxStorage;
    private readonly IEnvelopeBuilder _envelopeBuilder;
    private readonly ITransportDispatcher _transportDispatcher;
    private readonly IEventOrchestrator _eventOrchestrator;
    private readonly IEventDispatcher _eventDispatcher;
    private readonly MediatorMetrics _metrics;

    public ErrorHandlingTests()
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
        _outboxStorage.MarkAsFailedAsync(Arg.Any<IEvent>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<DateTimeOffset?>(), Arg.Any<CancellationToken>())
            .Returns(Result.Success());
        _outboxStorage.GetAttemptCountAsync(Arg.Any<IEvent>(), Arg.Any<CancellationToken>())
            .Returns(0);
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
    public async Task StoreAndDispatchAsync_WhenLocalHandlerFails_MarksAsFailedAndSchedulesRetry()
    {
        // Arrange
        _eventOrchestrator.RunAsync(Arg.Any<IEventHandler<EventExample>[]>(), Arg.Any<EventExample>(), Arg.Any<CancellationToken>())
            .Returns(Result.Failure("Handler execution failed"));
        
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
            Arg.Is<string>(s => s.Contains("Handler execution failed")),
            false,
            Arg.Is<DateTimeOffset?>(dt => dt.HasValue),
            Arg.Any<CancellationToken>());
        await _outboxStorage.DidNotReceive().MarkAsProcessedAsync(Arg.Any<IEvent>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task StoreAndDispatchAsync_WhenExternalDispatchFails_MarksAsFailedAndSchedulesRetry()
    {
        // Arrange
        _transportDispatcher.DispatchAsync(Arg.Any<MessageEnvelope>(), Arg.Any<MessageTraits>(), Arg.Any<CancellationToken>())
            .Returns(ValueTask.FromException(new InvalidOperationException("Transport error")));
        
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
        var traits = new MessageTraits { MessageType = typeof(EventExample) };

        // Act
        var result = await manager.StoreAndDispatchAsync(
            @event,
            handlers,
            DistributionMode.ExternalOnly,
            traits,
            CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess); // Returns success for best-effort (no FailFast)
        await _outboxStorage.Received(1).MarkAsFailedAsync(
            @event,
            Arg.Is<string>(s => s.Contains("Transport error")),
            false,
            Arg.Is<DateTimeOffset?>(dt => dt.HasValue),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task StoreAndDispatchAsync_WhenOutboxStorageFails_ReturnsFailureImmediately()
    {
        // Arrange
        _outboxStorage.AddAsync(Arg.Any<EventExample>(), Arg.Any<DistributionMode>(), Arg.Any<CancellationToken>())
            .Returns(Result.Failure("Database connection failed"));
        
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
        Assert.Contains("Database connection failed", result.ToString());
        // Should not attempt dispatch if storage fails
        await _eventOrchestrator.DidNotReceive().RunAsync(Arg.Any<IEventHandler<EventExample>[]>(), Arg.Any<EventExample>(), Arg.Any<CancellationToken>());
        await _outboxStorage.DidNotReceive().MarkAsFailedAsync(Arg.Any<IEvent>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<DateTimeOffset?>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task StoreAndDispatchAsync_WithFailFastTrue_PropagatesException()
    {
        // Arrange
        var expectedException = new InvalidOperationException("Critical transport error");
        _transportDispatcher.DispatchAsync(Arg.Any<MessageEnvelope>(), Arg.Any<MessageTraits>(), Arg.Any<CancellationToken>())
            .Returns(ValueTask.FromException(expectedException));
        
        var dispatcherOptions = Options.Create(new EventDispatcherOptions
        {
            DispatchStrategy = DispatchStrategy.Immediate
        });
        var outboxOptions = Options.Create(new OutboxOptions
        {
            MaxRetryAttempts = 3
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
        var traits = new MessageTraits 
        { 
            MessageType = typeof(EventExample),
            FailFast = true // Enable fail-fast
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await manager.StoreAndDispatchAsync(
                @event,
                handlers,
                DistributionMode.ExternalOnly,
                traits,
                CancellationToken.None);
        });
        
        Assert.Equal("Critical transport error", exception.Message);
        // Should still mark as failed before throwing
        await _outboxStorage.Received(1).MarkAsFailedAsync(
            @event,
            Arg.Any<string>(),
            Arg.Any<bool>(),
            Arg.Any<DateTimeOffset?>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task StoreAndDispatchAsync_WithFailFastFalse_ReturnsSuccessOnError()
    {
        // Arrange
        _transportDispatcher.DispatchAsync(Arg.Any<MessageEnvelope>(), Arg.Any<MessageTraits>(), Arg.Any<CancellationToken>())
            .Returns(ValueTask.FromException(new InvalidOperationException("Transport error")));
        
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
        var traits = new MessageTraits 
        { 
            MessageType = typeof(EventExample),
            FailFast = false // Best-effort mode
        };

        // Act
        var result = await manager.StoreAndDispatchAsync(
            @event,
            handlers,
            DistributionMode.ExternalOnly,
            traits,
            CancellationToken.None);

        // Assert - Should return success for best-effort
        Assert.True(result.IsSuccess);
        await _outboxStorage.Received(1).MarkAsFailedAsync(
            @event,
            Arg.Any<string>(),
            Arg.Any<bool>(),
            Arg.Any<DateTimeOffset?>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task StoreAndDispatchAsync_WithHybridMode_WhenLocalFailsButExternalSucceeds_MarksAsFailed()
    {
        // Arrange
        _eventOrchestrator.RunAsync(Arg.Any<IEventHandler<EventExample>[]>(), Arg.Any<EventExample>(), Arg.Any<CancellationToken>())
            .Returns(Result.Failure("Local handler failed"));
        // External succeeds (no exception)
        
        var dispatcherOptions = Options.Create(new EventDispatcherOptions
        {
            DispatchStrategy = DispatchStrategy.Immediate
        });
        var outboxOptions = Options.Create(new OutboxOptions
        {
            MaxRetryAttempts = 3,
            InitialRetryDelay = TimeSpan.FromSeconds(1)
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
        var traits = new MessageTraits { MessageType = typeof(EventExample) };

        // Act
        var result = await manager.StoreAndDispatchAsync(
            @event,
            handlers,
            DistributionMode.Hybrid,
            traits,
            CancellationToken.None);

        // Assert - Combined result should be failure
        Assert.True(result.IsFaulted);
        await _outboxStorage.Received(1).MarkAsFailedAsync(
            @event,
            Arg.Any<string>(),
            false,
            Arg.Is<DateTimeOffset?>(dt => dt.HasValue),
            Arg.Any<CancellationToken>());
    }
}
