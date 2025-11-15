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
public sealed class DistributionModeTests
{
    private readonly IEventOutboxStorage _outboxStorage;
    private readonly IEnvelopeBuilder _envelopeBuilder;
    private readonly ITransportDispatcher _transportDispatcher;
    private readonly IEventOrchestrator _eventOrchestrator;
    private readonly IEventDispatcher _eventDispatcher;
    private readonly MediatorMetrics _metrics;

    public DistributionModeTests()
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
    public async Task ExecuteDirectAsync_WithLocalOnlyMode_ExecutesOnlyLocalHandlers()
    {
        // Arrange
        var dispatcherOptions = Options.Create(new EventDispatcherOptions());
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
        var handler1 = new EventExampleHandler1();
        var handlers = new IEventHandler<EventExample>[] { handler1 };

        // Act
        var result = await manager.ExecuteDirectAsync(
            @event,
            handlers,
            DistributionMode.LocalOnly,
            null,
            CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        await _eventOrchestrator.Received(1).RunAsync(handlers, @event, Arg.Any<CancellationToken>());
        await _transportDispatcher.DidNotReceive().DispatchAsync(Arg.Any<MessageEnvelope>(), Arg.Any<MessageTraits>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteDirectAsync_WithExternalOnlyMode_ExecutesOnlyExternalDispatch()
    {
        // Arrange
        var dispatcherOptions = Options.Create(new EventDispatcherOptions());
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
        var traits = new MessageTraits { MessageType = typeof(EventExample) };

        // Act
        var result = await manager.ExecuteDirectAsync(
            @event,
            handlers,
            DistributionMode.ExternalOnly,
            traits,
            CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        await _eventOrchestrator.DidNotReceive().RunAsync(Arg.Any<IEventHandler<EventExample>[]>(), Arg.Any<EventExample>(), Arg.Any<CancellationToken>());
        await _transportDispatcher.Received(1).DispatchAsync(Arg.Any<MessageEnvelope>(), traits, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteDirectAsync_WithHybridMode_ExecutesBothLocalAndExternal()
    {
        // Arrange
        var dispatcherOptions = Options.Create(new EventDispatcherOptions());
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
        var handler1 = new EventExampleHandler1();
        var handlers = new IEventHandler<EventExample>[] { handler1 };
        var traits = new MessageTraits { MessageType = typeof(EventExample) };

        // Act
        var result = await manager.ExecuteDirectAsync(
            @event,
            handlers,
            DistributionMode.Hybrid,
            traits,
            CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        await _eventOrchestrator.Received(1).RunAsync(handlers, @event, Arg.Any<CancellationToken>());
        await _transportDispatcher.Received(1).DispatchAsync(Arg.Any<MessageEnvelope>(), traits, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteDirectAsync_WithHybridMode_ExecutesInParallel()
    {
        // Arrange
        var localExecuted = false;
        var externalExecuted = false;
        var localStartTime = DateTimeOffset.MinValue;
        var externalStartTime = DateTimeOffset.MinValue;
        
        _eventOrchestrator.RunAsync(Arg.Any<IEventHandler<EventExample>[]>(), Arg.Any<EventExample>(), Arg.Any<CancellationToken>())
            .Returns(callInfo => new ValueTask<Result>(Task.Run(async () =>
            {
                localStartTime = DateTimeOffset.UtcNow;
                await Task.Delay(50); // Simulate work
                localExecuted = true;
                return Result.Success();
            })));
        
        _transportDispatcher.DispatchAsync(Arg.Any<MessageEnvelope>(), Arg.Any<MessageTraits>(), Arg.Any<CancellationToken>())
            .Returns(callInfo => new ValueTask(Task.Run(async () =>
            {
                externalStartTime = DateTimeOffset.UtcNow;
                await Task.Delay(50); // Simulate work
                externalExecuted = true;
            })));
        
        var dispatcherOptions = Options.Create(new EventDispatcherOptions());
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
        var traits = new MessageTraits { MessageType = typeof(EventExample) };

        // Act
        var result = await manager.ExecuteDirectAsync(
            @event,
            handlers,
            DistributionMode.Hybrid,
            traits,
            CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(localExecuted);
        Assert.True(externalExecuted);
        
        // Verify parallel execution - start times should be very close (within 20ms)
        var timeDifference = Math.Abs((localStartTime - externalStartTime).TotalMilliseconds);
        Assert.True(timeDifference < 20, $"Expected parallel execution but time difference was {timeDifference}ms");
    }

    [Fact]
    public async Task StoreAndDispatchAsync_WithLocalOnlyMode_StoresCorrectDistributionMode()
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
        await manager.StoreAndDispatchAsync(
            @event,
            handlers,
            DistributionMode.LocalOnly,
            null,
            CancellationToken.None);

        // Assert - Verify distribution mode is stored correctly
        await _outboxStorage.Received(1).AddAsync(@event, DistributionMode.LocalOnly, Arg.Any<CancellationToken>());
    }
}
