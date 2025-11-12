using UnambitiousFx.Mediator.Abstractions;

namespace UnambitiousFx.Mediator;

internal sealed class ContextBuilder : IContextBuilder
{
    private readonly Dictionary<string, object> _metadata;
    private readonly IPublisher _publisher;
    private string? _correlationId;

    public ContextBuilder(IPublisher publisher)
    {
        _publisher = publisher;
        _metadata = new Dictionary<string, object>();
    }

    public IContextBuilder WithCorrelationId(string correlationId)
    {
        _correlationId = correlationId;
        return this;
    }

    public IContextBuilder WithMetadata(Dictionary<string, object> metadata)
    {
        foreach (var kv in metadata)
            _metadata[kv.Key] = kv.Value;
        return this;
    }

    public IContextBuilder WithMetadata(string key, object value)
    {
        _metadata[key] = value;
        return this;
    }

    public IContext Build()
    {
        ArgumentException.ThrowIfNullOrEmpty(_correlationId);
        return new Context(_publisher, _correlationId, null, _metadata);
    }
}
