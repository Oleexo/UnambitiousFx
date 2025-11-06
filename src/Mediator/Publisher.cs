using Microsoft.Extensions.Options;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions.Collections;
using UnambitiousFx.Mediator.Abstractions;

namespace UnambitiousFx.Mediator;

internal sealed class Publisher : IPublisher {
    private readonly PublishMode         _defaultMode;
    private readonly IEventDispatcher    _eventDispatcher;
    private readonly OutboxOptions       _outboxOptions;
    private readonly IEventOutboxStorage _outboxStorage;

    public Publisher(IEventDispatcher           eventDispatcher,
                     IEventOutboxStorage        outboxStorage,
                     IOptions<PublisherOptions> options,
                     IOptions<OutboxOptions>?   outboxOptions = null) {
        _eventDispatcher = eventDispatcher;
        _outboxStorage   = outboxStorage;
        _defaultMode     = options.Value.DefaultMode;
        _outboxOptions   = outboxOptions?.Value ?? new OutboxOptions();
    }

    public ValueTask<Result> PublishAsync<TEvent>(TEvent            @event,
                                                  CancellationToken cancellationToken = default)
        where TEvent : class, IEvent {
        return PublishAsync(@event, _defaultMode, cancellationToken);
    }

    public ValueTask<Result> PublishAsync<TEvent>(TEvent            @event,
                                                  PublishMode       mode,
                                                  CancellationToken cancellationToken = default)
        where TEvent : class, IEvent {
        return mode switch {
            PublishMode.Now     => _eventDispatcher.DispatchAsync(@event, cancellationToken),
            PublishMode.Outbox  => _outboxStorage.AddAsync(@event, cancellationToken),
            PublishMode.Default => PublishAsync(@event, _defaultMode, cancellationToken),
            _                   => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
        };
    }

    public async ValueTask<Result> CommitAsync(CancellationToken cancellationToken = default) {
        var          pendingEvents = await _outboxStorage.GetPendingEventsAsync(cancellationToken);
        List<IEvent> events;
        if (_outboxOptions.BatchSize.HasValue &&
            _outboxOptions.BatchSize.Value > 0) {
            events = pendingEvents.Take(_outboxOptions.BatchSize.Value)
                                  .ToList();
        }
        else {
            events = pendingEvents.ToList();
        }

        var results = new List<Result>();

        foreach (var @event in events) {
            var result = await _eventDispatcher.DispatchAsync(@event, cancellationToken);
            if (result.IsSuccess) {
                var processed = await _outboxStorage.MarkAsProcessedAsync(@event, cancellationToken);
                results.Add(processed.IsSuccess
                                ? result
                                : processed);
                continue;
            }

            var attemptCount      = await _outboxStorage.GetAttemptCountAsync(@event, cancellationToken) ?? 0;
            var nextAttemptNumber = attemptCount + 1; // the attempt we are recording now
            var shouldDeadLetter  = nextAttemptNumber >= _outboxOptions.MaxRetryAttempts;

            DateTimeOffset? nextAttemptAt = null;
            if (!shouldDeadLetter &&
                _outboxOptions.InitialRetryDelay > TimeSpan.Zero) {
                // exponential backoff like delay * (factor ^ (attemptCount))
                var factorPower = Math.Pow(_outboxOptions.BackoffFactor <= 0
                                               ? 1
                                               : _outboxOptions.BackoffFactor, attemptCount);
                var delay = TimeSpan.FromMilliseconds(_outboxOptions.InitialRetryDelay.TotalMilliseconds * factorPower);
                nextAttemptAt = DateTimeOffset.UtcNow + delay;
            }

            var failureReason = "Dispatch failed"; // Minimal reason placeholder
            await _outboxStorage.MarkAsFailedAsync(@event,
                                                   failureReason,
                                                   shouldDeadLetter,
                                                   nextAttemptAt,
                                                   cancellationToken);
            results.Add(result);
        }

        return results.Combine();
    }
}
