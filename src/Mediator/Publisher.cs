using Microsoft.Extensions.Options;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Mediator.Abstractions;

namespace UnambitiousFx.Mediator;

internal sealed class Publisher : IPublisher {
    private readonly PublishMode         _defaultMode;
    private readonly IEventDispatcher    _eventDispatcher;
    private readonly IEventOutboxStorage _outboxStorage;

    public Publisher(IEventDispatcher           eventDispatcher,
                     IEventOutboxStorage        outboxStorage,
                     IOptions<PublisherOptions> options) {
        _eventDispatcher = eventDispatcher;
        _outboxStorage   = outboxStorage;
        _defaultMode     = options.Value.DefaultMode;
    }

    public ValueTask<Result> PublishAsync<TEvent>(IContext          context,
                                                  TEvent            @event,
                                                  CancellationToken cancellationToken = default)
        where TEvent : class, IEvent {
        return PublishAsync(context, @event, _defaultMode, cancellationToken);
    }

    public ValueTask<Result> PublishAsync<TEvent>(IContext          context,
                                                  TEvent            @event,
                                                  PublishMode       mode,
                                                  CancellationToken cancellationToken = default)
        where TEvent : class, IEvent {
        return mode switch {
            PublishMode.Now     => _eventDispatcher.DispatchAsync(context, @event, cancellationToken),
            PublishMode.Outbox  => _outboxStorage.AddAsync(@event, cancellationToken),
            PublishMode.Default => PublishAsync(context, @event, _defaultMode, cancellationToken),
            _                   => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
        };
    }

    public async ValueTask<Result> CommitAsync(IContext          context,
                                               CancellationToken cancellationToken = default) {
        var events  = await _outboxStorage.GetPendingEventsAsync(cancellationToken);
        var results = new List<Result>();

        foreach (var @event in events) {
            results.Add(await _eventDispatcher.DispatchAsync(context, @event, cancellationToken));
        }

        return results.ToResult();
    }
}
