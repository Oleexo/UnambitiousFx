using System.Diagnostics;
using UnambitiousFx.Mediator.Abstractions;
using UnambitiousFx.Mediator.Transports.Abstractions;

namespace UnambitiousFx.Mediator.Transports.Core;

/// <summary>
///     Builds message envelopes from mediator messages, populating metadata from the current context.
/// </summary>
public sealed class EnvelopeBuilder : IEnvelopeBuilder
{
    private readonly IContextAccessor _contextAccessor;

    /// <summary>
    ///     Initializes a new instance of the <see cref="EnvelopeBuilder" /> class.
    /// </summary>
    /// <param name="contextAccessor">The context accessor for retrieving the current context.</param>
    public EnvelopeBuilder(IContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    /// <inheritdoc />
    public MessageEnvelope Build<TMessage>(TMessage message)
    {
        var context = _contextAccessor.Context;
        var messageId = Guid.NewGuid().ToString();

        return new MessageEnvelope
        {
            MessageId = messageId,
            CorrelationId = context?.CorrelationId ?? Guid.NewGuid().ToString(),
            CausationId = context?.GetMetadata<string>("MessageId"),
            TenantId = context?.GetMetadata<string>("TenantId"),
            Timestamp = DateTimeOffset.UtcNow,
            PayloadType = typeof(TMessage).AssemblyQualifiedName!,
            Payload = message!,
            Headers = BuildHeaders()
        };
    }

    private static MessageHeaders BuildHeaders()
    {
        var headers = new MessageHeaders
        {
            ContentType = "application/json",
            SchemaVersion = "1.0"
        };

        // Propagate trace context using W3C Trace Context standard
        var activity = Activity.Current;
        if (activity != null)
        {
            headers.TraceParent = activity.Id;
            headers.TraceState = activity.TraceStateString;
        }

        return headers;
    }
}
