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
public sealed class EventDispatcherRoutingTests
{
    private readonly IDependencyResolver _dependencyResolver;
    private readonly IEventOrchestrator _eventOrchestrator;
    private readonly IMessageTraitsRegistry _traitsRegistry;
    private readonly OutboxManager _outboxManager;
    private readonly MediatorMetrics _metrics;

    public EventDispatcherRoutingTests()
    {
        _dependencyResolver = Substitute.For<IDependencyResolver>();
        _eventOrchestrator = Substitute.For<IEventOrchestrator>();
        _traitsRegistry = Substitute.For<IMessageTraitsRegistry>();
        _outboxManager = Substitute.For<OutboxManager>();
        _metrics = Substitute.For<MediatorMetrics>();
        
        // Setup default returns
        _dependencyResolver.GetServices<IEventHandler<EventExample>>()
            .Returns(Array.Empty<IEventHandler<EventExample>>());
        _dependencyResolver.GetServices<IEventPipelineBehavior>()
            .Returns(Array.Empty<IEventPipelineBehavior>());
        _eventOrchestrator.RunAsync(Arg.Any<IEventHandler<EventExample>[]>(), Arg.Any<EventExample>(), Arg.Any<CancellationToken>())
            .Returns(Result.Success());
        _outboxManager.StoreAndDispatchAsync(Arg.Any<EventExample>(), Arg.Any<IEventHandler<EventExample>[]>(), 
            Arg.Any<DistributionMode>(), Arg.Any<MessageTraits?>(), Arg.Any<CancellationToken>())
            .Returns(Result.Success());
    }

    [Fact]
    public async Task DispatchAsync_WithRoutingFilter_UsesFilterDistributionMode()
    {
        // Arrange
        var contextAccessor = CreateContextAccessorWithTenant("premium-tenant");
        var filter = new TenantBasedRoutingFilter(contextAccessor);
        var options = Options.Create(new EventDispatcherOptions
        {
            DefaultDistributionMode = DistributionMode.LocalOnly
        });
        
        var dispatcher = new EventDispatcher(
            _dependencyResolver,
            _eventOrchestrator,
            _traitsRegistry,
            new[] { filter },
            _outboxManager,
            options,
            NullLogger<EventDispatcher>.Instance,
            _metrics);
        
        var @event = new EventExample("Test Event");

        // Act
        await dispatcher.DispatchAsync(@event, CancellationToken.None);

        // Assert - Filter should return Hybrid mode for premium-tenant
        await _outboxManager.Received(1).StoreAndDispatchAsync(
            @event,
            Arg.Any<IEventHandler<EventExample>[]>(),
            DistributionMode.Hybrid,
            Arg.Any<MessageTraits?>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task DispatchAsync_WithMultipleFilters_UsesFirstNonNullResult()
    {
        // Arrange
        var contextAccessor = CreateContextAccessorWithTenant("premium-tenant");
        var environmentFilter = new EnvironmentBasedRoutingFilter("Development"); // Order 50, returns LocalOnly
        var tenantFilter = new TenantBasedRoutingFilter(contextAccessor); // Order 100, returns Hybrid
        
        var options = Options.Create(new EventDispatcherOptions
        {
            DefaultDistributionMode = DistributionMode.ExternalOnly
        });
        
        var dispatcher = new EventDispatcher(
            _dependencyResolver,
            _eventOrchestrator,
            _traitsRegistry,
            new IEventRoutingFilter[] { environmentFilter, tenantFilter },
            _outboxManager,
            options,
            NullLogger<EventDispatcher>.Instance,
            _metrics);
        
        var @event = new EventExample("Test Event");

        // Act
        await dispatcher.DispatchAsync(@event, CancellationToken.None);

        // Assert - Environment filter executes first and returns LocalOnly
        await _outboxManager.Received(1).StoreAndDispatchAsync(
            @event,
            Arg.Any<IEventHandler<EventExample>[]>(),
            DistributionMode.LocalOnly,
            Arg.Any<MessageTraits?>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task DispatchAsync_WithFilterReturningNull_FallsBackToMessageTraits()
    {
        // Arrange
        var contextAccessor = CreateContextAccessorWithTenant("unknown-tenant");
        var filter = new TenantBasedRoutingFilter(contextAccessor); // Returns null for unknown tenant
        
        var messageTraits = new MessageTraits
        {
            MessageType = typeof(EventExample),
            DistributionMode = DistributionMode.ExternalOnly
        };
        _traitsRegistry.GetTraits<EventExample>().Returns(messageTraits);
        
        var options = Options.Create(new EventDispatcherOptions
        {
            DefaultDistributionMode = DistributionMode.LocalOnly
        });
        
        var dispatcher = new EventDispatcher(
            _dependencyResolver,
            _eventOrchestrator,
            _traitsRegistry,
            new[] { filter },
            _outboxManager,
            options,
            NullLogger<EventDispatcher>.Instance,
            _metrics);
        
        var @event = new EventExample("Test Event");

        // Act
        await dispatcher.DispatchAsync(@event, CancellationToken.None);

        // Assert - Should use message traits distribution mode
        await _outboxManager.Received(1).StoreAndDispatchAsync(
            @event,
            Arg.Any<IEventHandler<EventExample>[]>(),
            DistributionMode.ExternalOnly,
            Arg.Any<MessageTraits?>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task DispatchAsync_WithNoFilterAndNoTraits_UsesDefaultDistributionMode()
    {
        // Arrange
        _traitsRegistry.GetTraits<EventExample>().Returns((MessageTraits?)null);
        
        var options = Options.Create(new EventDispatcherOptions
        {
            DefaultDistributionMode = DistributionMode.Hybrid
        });
        
        var dispatcher = new EventDispatcher(
            _dependencyResolver,
            _eventOrchestrator,
            _traitsRegistry,
            Array.Empty<IEventRoutingFilter>(),
            _outboxManager,
            options,
            NullLogger<EventDispatcher>.Instance,
            _metrics);
        
        var @event = new EventExample("Test Event");

        // Act
        await dispatcher.DispatchAsync(@event, CancellationToken.None);

        // Assert - Should use default distribution mode from options
        await _outboxManager.Received(1).StoreAndDispatchAsync(
            @event,
            Arg.Any<IEventHandler<EventExample>[]>(),
            DistributionMode.Hybrid,
            Arg.Any<MessageTraits?>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task DispatchAsync_CachesRoutingDecisions_ForSameEventType()
    {
        // Arrange
        var contextAccessor = CreateContextAccessorWithTenant("premium-tenant");
        var filter = Substitute.For<IEventRoutingFilter>();
        filter.Order.Returns(100);
        filter.GetDistributionMode(Arg.Any<EventExample>()).Returns(DistributionMode.Hybrid);
        
        var options = Options.Create(new EventDispatcherOptions
        {
            DefaultDistributionMode = DistributionMode.LocalOnly
        });
        
        var dispatcher = new EventDispatcher(
            _dependencyResolver,
            _eventOrchestrator,
            _traitsRegistry,
            new[] { filter },
            _outboxManager,
            options,
            NullLogger<EventDispatcher>.Instance,
            _metrics);
        
        var event1 = new EventExample("Event 1");
        var event2 = new EventExample("Event 2");

        // Act - Dispatch two events of the same type
        await dispatcher.DispatchAsync(event1, CancellationToken.None);
        await dispatcher.DispatchAsync(event2, CancellationToken.None);

        // Assert - Filter should only be called once (cached for second call)
        filter.Received(1).GetDistributionMode(Arg.Any<EventExample>());
        
        // Both events should use the cached distribution mode
        await _outboxManager.Received(2).StoreAndDispatchAsync(
            Arg.Any<EventExample>(),
            Arg.Any<IEventHandler<EventExample>[]>(),
            DistributionMode.Hybrid,
            Arg.Any<MessageTraits?>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task DispatchAsync_FiltersOrderedByPriority_LowerOrderExecutesFirst()
    {
        // Arrange
        var filter1 = Substitute.For<IEventRoutingFilter>();
        filter1.Order.Returns(200);
        filter1.GetDistributionMode(Arg.Any<EventExample>()).Returns(DistributionMode.ExternalOnly);
        
        var filter2 = Substitute.For<IEventRoutingFilter>();
        filter2.Order.Returns(50);
        filter2.GetDistributionMode(Arg.Any<EventExample>()).Returns(DistributionMode.LocalOnly);
        
        var filter3 = Substitute.For<IEventRoutingFilter>();
        filter3.Order.Returns(100);
        filter3.GetDistributionMode(Arg.Any<EventExample>()).Returns(DistributionMode.Hybrid);
        
        var options = Options.Create(new EventDispatcherOptions
        {
            DefaultDistributionMode = DistributionMode.LocalOnly
        });
        
        // Pass filters in random order - dispatcher should sort by Order
        var dispatcher = new EventDispatcher(
            _dependencyResolver,
            _eventOrchestrator,
            _traitsRegistry,
            new[] { filter1, filter3, filter2 },
            _outboxManager,
            options,
            NullLogger<EventDispatcher>.Instance,
            _metrics);
        
        var @event = new EventExample("Test Event");

        // Act
        await dispatcher.DispatchAsync(@event, CancellationToken.None);

        // Assert - Filter2 (Order 50) should execute first and return LocalOnly
        await _outboxManager.Received(1).StoreAndDispatchAsync(
            @event,
            Arg.Any<IEventHandler<EventExample>[]>(),
            DistributionMode.LocalOnly,
            Arg.Any<MessageTraits?>(),
            Arg.Any<CancellationToken>());
    }

    private static IContextAccessor CreateContextAccessorWithTenant(string tenantId)
    {
        var context = Substitute.For<IContext>();
        context.GetMetadata<string>("TenantId").Returns(tenantId);
        
        var contextAccessor = Substitute.For<IContextAccessor>();
        contextAccessor.Context.Returns(context);
        
        return contextAccessor;
    }
}
