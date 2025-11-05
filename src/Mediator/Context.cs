using UnambitiousFx.Core.Results;
using UnambitiousFx.Mediator.Abstractions;

namespace UnambitiousFx.Mediator;

internal readonly struct Context : IContext {
    private readonly IPublisher _publisher;

    public Context(IPublisher publisher) {
        _publisher    = publisher;
        CorrelationId = Guid.CreateVersion7();
        OccuredAt     = DateTimeOffset.UtcNow;
    }

    private Context(Context context) {
        _publisher = context._publisher;
    }

    public Guid           CorrelationId { get; }
    public DateTimeOffset OccuredAt     { get; }

    public ValueTask<Result> PublishEventAsync<TEvent>(TEvent            @event,
                                                       CancellationToken cancellationToken = default)
        where TEvent : class, IEvent {
        return _publisher.PublishAsync(this, @event, cancellationToken);
    }

    public ValueTask<Result> PublishEventAsync<TEvent>(TEvent            @event,
                                                       PublishMode       mode,
                                                       CancellationToken cancellationToken = default)
        where TEvent : class, IEvent {
        return _publisher.PublishAsync(this, @event, mode, cancellationToken);
    }

    public ValueTask<Result> CommitEventsAsync(CancellationToken cancellationToken = default) {
        return _publisher.CommitAsync(this, cancellationToken);
    }
}
