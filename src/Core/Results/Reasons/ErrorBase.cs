namespace UnambitiousFx.Core.Results.Reasons;

/// <summary>
///     Standard base implementation for an error. Immutable and value-based.
/// </summary>
public abstract record ErrorBase : IError
{
    protected ErrorBase(string code,
                        string message,
                        Exception? exception = null,
                        IReadOnlyDictionary<string, object?>? metadata = null)
    {
        Code = code;
        Message = message;
        Exception = exception;
        Metadata = metadata is null
                       ? new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase)
                       : new Dictionary<string, object?>(metadata, StringComparer.OrdinalIgnoreCase);
    }

    public string Code { get; }
    public string Message { get; private init; }
    public Exception? Exception { get; }
    public IError WithMessage(string message)
    {
        return this with
        {
            Message = message
        };
    }

    public IReadOnlyDictionary<string, object?> Metadata { get; init; }

    protected static IReadOnlyDictionary<string, object?> Merge(IReadOnlyDictionary<string, object?>? left,
                                                                IEnumerable<KeyValuePair<string, object?>> right)
    {
        var d = left is null
                    ? new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase)
                    : new Dictionary<string, object?>(left, StringComparer.OrdinalIgnoreCase);
        foreach (var kv in right)
        {
            d[kv.Key] = kv.Value;
        }

        return d;
    }
}
