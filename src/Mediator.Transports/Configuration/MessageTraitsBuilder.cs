using UnambitiousFx.Mediator.Transports.Abstractions;
using UnambitiousFx.Mediator.Transports.Security;

namespace UnambitiousFx.Mediator.Transports.Configuration;

/// <summary>
/// Builder implementation for configuring message traits for a specific message type.
/// </summary>
/// <typeparam name="TMessage">The message type being configured.</typeparam>
internal sealed class MessageTraitsBuilder<TMessage> : IMessageTraitsBuilder<TMessage>
    where TMessage : class
{
    private readonly IMessageTraitsRegistry _traitsRegistry;
    private readonly IMessageTypeRegistry _typeRegistry;
    private readonly ISensitiveDataRegistry _sensitiveDataRegistry;
    private DistributionMode _distributionMode = DistributionMode.LocalOnly;
    private string? _transportName;
    private string? _topic;
    private string? _partitionKey;
    private bool _failFast;
    private bool _useOutbox;
    private RetryPolicy? _retryPolicy;
    private int _maxConcurrency = 1;

    public MessageTraitsBuilder(
        IMessageTraitsRegistry traitsRegistry,
        IMessageTypeRegistry typeRegistry,
        ISensitiveDataRegistry sensitiveDataRegistry)
    {
        _traitsRegistry = traitsRegistry;
        _typeRegistry = typeRegistry;
        _sensitiveDataRegistry = sensitiveDataRegistry;

        // Automatically register the message type for NativeAOT compatibility
        _typeRegistry.Register<TMessage>();
    }

    public IMessageTraitsBuilder<TMessage> UseLocalOnly()
    {
        _distributionMode = DistributionMode.LocalOnly;
        RegisterTraits();
        return this;
    }

    public IMessageTraitsBuilder<TMessage> UseHybridMode()
    {
        _distributionMode = DistributionMode.Hybrid;
        RegisterTraits();
        return this;
    }

    public IMessageTraitsBuilder<TMessage> RouteExternally()
    {
        _distributionMode = DistributionMode.ExternalOnly;
        RegisterTraits();
        return this;
    }

    public IMessageTraitsBuilder<TMessage> WithTransport(string transportName)
    {
        _transportName = transportName;
        RegisterTraits();
        return this;
    }

    public IMessageTraitsBuilder<TMessage> ToTopic(string topic)
    {
        _topic = topic;
        RegisterTraits();
        return this;
    }

    public IMessageTraitsBuilder<TMessage> WithPartitionKey(string partitionKey)
    {
        _partitionKey = partitionKey;
        RegisterTraits();
        return this;
    }

    public IMessageTraitsBuilder<TMessage> FailFastOnTransportErrors()
    {
        _failFast = true;
        RegisterTraits();
        return this;
    }

    public IMessageTraitsBuilder<TMessage> UseOutbox()
    {
        _useOutbox = true;
        RegisterTraits();
        return this;
    }

    public IMessageTraitsBuilder<TMessage> WithRetryPolicy(RetryPolicy policy)
    {
        _retryPolicy = policy;
        RegisterTraits();
        return this;
    }

    public IMessageTraitsBuilder<TMessage> WithMaxConcurrency(int maxConcurrency)
    {
        _maxConcurrency = maxConcurrency;
        RegisterTraits();
        return this;
    }

    public IMessageTraitsBuilder<TMessage> WithSensitiveField(
        string fieldName,
        Func<TMessage, string?> getter,
        Action<TMessage, string?> setter,
        SensitivityLevel level = SensitivityLevel.Confidential,
        bool requireEncryption = true)
    {
        var descriptor = new SensitiveFieldDescriptor
        {
            FieldName = fieldName,
            Level = level,
            RequireEncryption = requireEncryption,
            Getter = obj => getter((TMessage)obj),
            Setter = (obj, value) => setter((TMessage)obj, value)
        };

        _sensitiveDataRegistry.RegisterSensitiveFields<TMessage>(new[] { descriptor });
        return this;
    }

    public IMessageTraitsBuilder<TMessage> WithSensitiveFields(
        Action<ISensitiveFieldsBuilder<TMessage>> configure)
    {
        var builder = new SensitiveFieldsBuilder<TMessage>();
        configure(builder);
        var descriptors = builder.Build();

        if (descriptors.Count > 0)
        {
            _sensitiveDataRegistry.RegisterSensitiveFields<TMessage>(descriptors);
        }

        return this;
    }

    private void RegisterTraits()
    {
        var traits = new MessageTraits
        {
            MessageType = typeof(TMessage),
            DistributionMode = _distributionMode,
            TransportName = _transportName,
            Topic = _topic,
            PartitionKey = _partitionKey,
            FailFast = _failFast,
            UseOutbox = _useOutbox,
            RetryPolicy = _retryPolicy,
            MaxConcurrency = _maxConcurrency
        };

        _traitsRegistry.Register<TMessage>(traits);
    }
}
