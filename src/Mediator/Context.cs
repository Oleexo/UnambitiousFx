using Oleexo.UnambitiousFx.Core;
using Oleexo.UnambitiousFx.Mediator.Abstractions;

namespace Oleexo.UnambitiousFx.Mediator;

internal sealed class Context : IContext {
    private readonly Dictionary<string, object> _data = new();
    private readonly IPublisher                 _publisher;

    public Context(IPublisher publisher) {
        _publisher    = publisher;
        CorrelationId = Guid.CreateVersion7();
        OccuredAt     = DateTimeOffset.UtcNow;
    }

    private Context(Context context) {
        _data      = context._data;
        _publisher = context._publisher;
    }

    public Guid           CorrelationId { get; }
    public DateTimeOffset OccuredAt     { get; }

    public void Set<TValue>(string key,
                            TValue value)
        where TValue : notnull {
        _data[key] = value;
    }

    public Option<TValue> Get<TValue>(string key)
        where TValue : notnull {
        if (_data.TryGetValue(key, out var value) &&
            value is TValue valueAsTValue) {
            return Option<TValue>.Some(valueAsTValue);
        }

        return Option<TValue>.None;
    }

    public ValueTask<Result> PublishAsync<TEvent>(TEvent            @event,
                                                  CancellationToken cancellationToken = default)
        where TEvent : IEvent {
        return _publisher.PublishAsync(Clone(), @event, cancellationToken);
    }

    private Context Clone() {
        return new Context(this);
    }
}
