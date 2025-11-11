using UnambitiousFx.Core.Results;
using UnambitiousFx.Mediator.Abstractions;
using UnambitiousFx.Mediator.Abstractions.Exceptions;

namespace UnambitiousFx.Mediator;

internal readonly record struct Context : IContext
{
    private readonly Dictionary<Type, IContextFeature> _features;
    private readonly Dictionary<string, object> _metadata;
    private readonly IPublisher _publisher;

    public Context(IPublisher publisher,
        string correlationId,
        IReadOnlyDictionary<Type, IContextFeature>? features = null,
        IReadOnlyDictionary<string, object>? metadata = null)
    {
        _publisher = publisher;
        CorrelationId = correlationId ??
                        Guid.CreateVersion7()
                            .ToString();
        _metadata = metadata?.ToDictionary() ?? new Dictionary<string, object>();
        _features = features?.ToDictionary() ?? new Dictionary<Type, IContextFeature>();
    }

    public string CorrelationId { get; }

    public void SetMetadata(string key,
        object value)
    {
        _metadata[key] = value;
    }

    public bool RemoveMetadata(string key)
    {
        return _metadata.Remove(key);
    }

    public bool TryGetMetadata<T>(string key,
        out T? value)
    {
        if (_metadata.TryGetValue(key, out var obj) &&
            obj is T tValue)
        {
            value = tValue;
            return true;
        }

        value = default;
        return false;
    }

    public T? GetMetadata<T>(string key)
    {
        if (_metadata.TryGetValue(key, out var obj) &&
            obj is T tValue)
            return tValue;

        return default;
    }

    public IReadOnlyDictionary<string, object> Metadata => _metadata;

    public ValueTask<Result> PublishEventAsync<TEvent>(TEvent @event,
        CancellationToken cancellationToken = default)
        where TEvent : class, IEvent
    {
        return _publisher.PublishAsync(@event, cancellationToken);
    }

    public ValueTask<Result> PublishEventAsync<TEvent>(TEvent @event,
        PublishMode mode,
        CancellationToken cancellationToken = default)
        where TEvent : class, IEvent
    {
        return _publisher.PublishAsync(@event, mode, cancellationToken);
    }

    public ValueTask<Result> CommitEventsAsync(CancellationToken cancellationToken = default)
    {
        return _publisher.CommitAsync(cancellationToken);
    }

    public bool TryGetFeature<TFeature>(out TFeature? feature) where TFeature : class, IContextFeature
    {
        feature = GetFeature<TFeature>();
        return feature != null;
    }

    public TFeature? GetFeature<TFeature>() where TFeature : class, IContextFeature
    {
        return _features.TryGetValue(typeof(TFeature), out var value)
            ? (TFeature)value
            : null;
    }

    public TFeature MustGetFeature<TFeature>() where TFeature : class, IContextFeature
    {
        var feature = GetFeature<TFeature>();
        return feature ?? throw new MissingContextFeatureException(typeof(TFeature));
    }
}
