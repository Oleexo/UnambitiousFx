using System.Collections.Concurrent;
using UnambitiousFx.Mediator.Transports.Abstractions;

namespace UnambitiousFx.Mediator.Transports.Core;

/// <summary>
///     Thread-safe registry for storing and retrieving message traits.
/// </summary>
public sealed class MessageTraitsRegistry : IMessageTraitsRegistry
{
    private readonly ConcurrentDictionary<string, MessageTraits> _traits = new();

    /// <inheritdoc />
    public void Register<TMessage>(MessageTraits traits)
    {
        var fullName = typeof(TMessage).FullName;
        ArgumentNullException.ThrowIfNull(fullName);
        _traits[fullName] = traits;
    }

    /// <inheritdoc />
    public MessageTraits? GetTraits(string messageType)
    {
        return _traits.GetValueOrDefault(messageType);
    }

    /// <inheritdoc />
    public MessageTraits? GetTraits<TMessage>()
    {
        var fullName = typeof(TMessage).FullName;
        ArgumentNullException.ThrowIfNull(fullName);
        return _traits.GetValueOrDefault(fullName);
    }

    /// <inheritdoc />
    public IEnumerable<MessageTraits> GetAllTraits()
    {
        return _traits.Values;
    }
}
