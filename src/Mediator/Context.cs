using UnambitiousFx.Core.Results;
using UnambitiousFx.Mediator.Abstractions;

namespace UnambitiousFx.Mediator;

internal sealed class ContextAccessor : IContextAccessor {
    public ContextAccessor(IContext context) {
        Context = context;
    }

    public IContext Context { get; set; }
}

internal readonly struct Context : IContext {
    private readonly IPublisher                 _publisher;
    private readonly Dictionary<string, object> _metadata;

    public Context(IPublisher                  publisher,
                   string?                     correlationId = null,
                   DateTimeOffset?             occuredAt     = null,
                   Dictionary<string, object>? metadata      = null) {
        _publisher = publisher;
        CorrelationId = correlationId ??
                        Guid.CreateVersion7()
                            .ToString();
        OccuredAt = occuredAt ?? DateTimeOffset.UtcNow;
        _metadata = metadata  ?? new Dictionary<string, object>();
    }

    private Context(Context context) {
        _publisher    = context._publisher;
        CorrelationId = context.CorrelationId;
        OccuredAt     = context.OccuredAt;
        _metadata     = new Dictionary<string, object>(context._metadata);
    }

    public string         CorrelationId { get; }
    public DateTimeOffset OccuredAt     { get; }

    public void SetMetadata(string key,
                            object value) {
        _metadata[key] = value;
    }

    public bool RemoveMetadata(string key) {
        return _metadata.Remove(key);
    }

    public bool TryGetMetadata<T>(string key,
                                  out T? value) {
        if (_metadata.TryGetValue(key, out var obj) &&
            obj is T tValue) {
            value = tValue;
            return true;
        }

        value = default;
        return false;
    }

    public T? GetMetadata<T>(string key) {
        if (_metadata.TryGetValue(key, out var obj) &&
            obj is T tValue) {
            return tValue;
        }

        return default;
    }

    public IReadOnlyDictionary<string, object> Metadata => _metadata;

    public ValueTask<Result> PublishEventAsync<TEvent>(TEvent            @event,
                                                       CancellationToken cancellationToken = default)
        where TEvent : class, IEvent {
        return _publisher.PublishAsync(@event, cancellationToken);
    }

    public ValueTask<Result> PublishEventAsync<TEvent>(TEvent            @event,
                                                       PublishMode       mode,
                                                       CancellationToken cancellationToken = default)
        where TEvent : class, IEvent {
        return _publisher.PublishAsync(@event, mode, cancellationToken);
    }

    public ValueTask<Result> CommitEventsAsync(CancellationToken cancellationToken = default) {
        return _publisher.CommitAsync(cancellationToken);
    }
}
