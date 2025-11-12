namespace UnambitiousFx.Mediator.Transports.Abstractions;

/// <summary>
///     Represents message headers for cross-boundary serialization with support for custom extensions.
/// </summary>
public sealed class MessageHeaders
{
    private static readonly HashSet<string> ReservedKeys = new(StringComparer.OrdinalIgnoreCase)
    {
        "ContentType",
        "SchemaVersion",
        "TraceParent",
        "TraceState",
        "DeliveryMode",
        "Priority"
    };

    private readonly Dictionary<string, string> _customHeaders = new();

    /// <summary>
    ///     Gets or sets the content type of the message payload (e.g., "application/json").
    /// </summary>
    public string? ContentType { get; set; }

    /// <summary>
    ///     Gets or sets the schema version for the message payload.
    /// </summary>
    public string? SchemaVersion { get; set; }

    /// <summary>
    ///     Gets or sets the W3C Trace Context traceparent header for distributed tracing.
    /// </summary>
    public string? TraceParent { get; set; }

    /// <summary>
    ///     Gets or sets the W3C Trace Context tracestate header for distributed tracing.
    /// </summary>
    public string? TraceState { get; set; }

    /// <summary>
    ///     Gets or sets the delivery mode for the message.
    /// </summary>
    public DeliveryMode DeliveryMode { get; set; } = DeliveryMode.BestEffort;

    /// <summary>
    ///     Gets or sets the message priority (higher values indicate higher priority).
    /// </summary>
    public int Priority { get; set; }

    /// <summary>
    ///     Sets a custom header value. Reserved keys cannot be set through this method.
    /// </summary>
    /// <param name="key">The header key.</param>
    /// <param name="value">The header value.</param>
    /// <exception cref="ArgumentException">Thrown when attempting to set a reserved key.</exception>
    public void Set(string key, string value)
    {
        if (ReservedKeys.Contains(key))
            throw new ArgumentException($"Cannot set reserved header key: {key}", nameof(key));

        _customHeaders[key] = value;
    }

    /// <summary>
    ///     Gets a custom header value by key.
    /// </summary>
    /// <param name="key">The header key.</param>
    /// <returns>The header value if found; otherwise, null.</returns>
    public string? Get(string key)
    {
        return _customHeaders.TryGetValue(key, out var value) ? value : null;
    }

    /// <summary>
    ///     Gets all custom headers as a read-only dictionary.
    /// </summary>
    /// <returns>A read-only dictionary of all custom headers.</returns>
    public IReadOnlyDictionary<string, string> GetAll()
    {
        return _customHeaders;
    }
}
