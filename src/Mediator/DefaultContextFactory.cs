using UnambitiousFx.Mediator.Abstractions;

namespace UnambitiousFx.Mediator;

internal sealed class DefaultContextFactory(IPublisher publisher) : IContextFactory
{
    public IContext Create()
    {
        var correlationId = Guid.CreateVersion7()
            .ToString();
        var metadata = new Dictionary<string, object> { { "OccuredAt", DateTimeOffset.UtcNow } };
        return new Context(publisher, correlationId,
            metadata: metadata);
    }
}
