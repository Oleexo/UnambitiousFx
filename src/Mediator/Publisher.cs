using Microsoft.Extensions.Options;
using UnambitiousFx.Core;
using UnambitiousFx.Mediator.Abstractions;

namespace UnambitiousFx.Mediator;

internal class Publisher<TContext> : IPublisher<TContext>
    where TContext : IContext {
    private readonly PublishMode                _defaultMode;
    private readonly IEventDispatcher<TContext> _eventDispatcher;
    private readonly IEventOutboxStorage        _outboxStorage;

    public Publisher(IEventDispatcher<TContext> eventDispatcher,
                     IEventOutboxStorage        outboxStorage,
                     IOptions<PublisherOptions> options) {
        _eventDispatcher = eventDispatcher;
        _outboxStorage   = outboxStorage;
        _defaultMode     = options.Value.DefaultMode;
    }

    public ValueTask<Result> PublishAsync<TEvent>(TContext          context,
                                                  TEvent            @event,
                                                  CancellationToken cancellationToken = default)
        where TEvent : IEvent {
        return PublishAsync(context, @event, _defaultMode, cancellationToken);
    }

    public ValueTask<Result> PublishAsync<TEvent>(TContext          context,
                                                  TEvent            @event,
                                                  PublishMode       mode,
                                                  CancellationToken cancellationToken = default)
        where TEvent : IEvent {
        if (mode == PublishMode.Default) {
            mode = _defaultMode;
        }

        switch (mode) {
            case PublishMode.Now:
                return _eventDispatcher.DispatchAsync(context, @event, cancellationToken);
            case PublishMode.Outbox:
                return _outboxStorage.AddAsync(@event, cancellationToken);
            default:
                throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
        }
    }

    public async ValueTask<Result> CommitAsync(TContext          context,
                                               CancellationToken cancellationToken = default) {
        var events  = await _outboxStorage.GetPendingEventsAsync(cancellationToken);
        var results = new List<Result>();

        foreach (var @event in events) {
            results.Add(await _eventDispatcher.DispatchAsync(context, @event, cancellationToken));
        }

        return results.ToResult();
    }
}

internal sealed class Publisher : Publisher<IContext>, IPublisher {
    public Publisher(IEventDispatcher<IContext> eventDispatcher,
                     IEventOutboxStorage        outboxStorage,
                     IOptions<PublisherOptions> options)
        : base(eventDispatcher, outboxStorage, options) {
    }
}
