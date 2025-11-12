using Microsoft.Extensions.Options;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions.Collections;
using UnambitiousFx.Mediator.Abstractions;

namespace UnambitiousFx.Mediator;

internal sealed class Publisher(
    IEventDispatcher eventDispatcher,
    OutboxManager outboxManager,
    IOptions<PublisherOptions> options)
    : IPublisher
{
    private readonly PublishMode _defaultMode = options.Value.DefaultMode;

    public ValueTask<Result> PublishAsync<TEvent>(TEvent @event,
        CancellationToken cancellationToken = default)
        where TEvent : class, IEvent
    {
        return PublishAsync(@event, _defaultMode, cancellationToken);
    }

    public ValueTask<Result> PublishAsync<TEvent>(TEvent @event,
        PublishMode mode,
        CancellationToken cancellationToken = default)
        where TEvent : class, IEvent
    {
        return mode switch
        {
            PublishMode.Now => eventDispatcher.DispatchAsync(@event, cancellationToken),
            PublishMode.Outbox => eventDispatcher.DispatchAsync(@event, cancellationToken),
            PublishMode.Default => PublishAsync(@event, _defaultMode, cancellationToken),
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
        };
    }

    public ValueTask<Result> CommitAsync(CancellationToken cancellationToken = default)
    {
        return outboxManager.ProcessPendingAsync(cancellationToken);
    }
}
