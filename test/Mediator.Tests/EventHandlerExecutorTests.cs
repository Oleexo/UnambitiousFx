using NSubstitute;
using Oleexo.UnambitiousFx.Mediator.Abstractions;
using Oleexo.UnambitiousFx.Mediator.Orchestrators;
using Oleexo.UnambitiousFx.Mediator.Tests.Definitions;

namespace Oleexo.UnambitiousFx.Mediator.Tests;

public sealed class EventHandlerExecutorTests {
    [Fact]
    public async Task GivenAEventPipelineBehavior_WhenProxyHandle_ShouldCallTheBehavior() {
        var order     = 0;
        var publisher = Substitute.For<IPublisher>();
        var handler   = new EventExampleHandler1();
        handler.OnExecuted = () => {
            Assert.Equal(1, order);
            order++;
        };
        var behavior = new TestEventPipelineBehavior();
        behavior.OnExecuted = () => {
            Assert.Equal(0, order);
            order++;
        };
        var proxy  = new EventHandlerExecutor<EventExample>([handler], [behavior], new SequentialEventOrchestrator());
        var @event = new EventExample();

        var result = await proxy.HandleAsync(new Context(publisher), @event, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.True(handler.Executed);
        Assert.Equal(1, handler.ExecutionCount);
        Assert.True(behavior.Executed);
        Assert.Equal(1, behavior.ExecutionCount);
    }

    [Fact]
    public async Task GivenAEventPipelineBehaviors_WhenProxyHandle_ShouldCallTheBehavior() {
        var order     = 0;
        var publisher = Substitute.For<IPublisher>();
        var handler   = new EventExampleHandler1();
        handler.OnExecuted = () => {
            Assert.Equal(2, order);
            order++;
        };
        var behaviors = new List<IEventPipelineBehavior> {
            new TestEventPipelineBehavior(),
            new TestEventPipelineBehavior()
        };
        behaviors.ForEach(x => {
            if (x is TestEventPipelineBehavior behavior) {
                behavior.OnExecuted = () => { order++; };
            }
        });

        var proxy  = new EventHandlerExecutor<EventExample>([handler], behaviors, new SequentialEventOrchestrator());
        var @event = new EventExample();

        var result = await proxy.HandleAsync(new Context(publisher), @event, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.True(handler.Executed);
        Assert.Equal(1, handler.ExecutionCount);
    }
}
