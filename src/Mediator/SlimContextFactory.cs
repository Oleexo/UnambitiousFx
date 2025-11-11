using UnambitiousFx.Mediator.Abstractions;

namespace UnambitiousFx.Mediator;

internal sealed class SlimContextFactory(IPublisher publisher) : IContextFactory
{
    public IContext Create()
    {
        var correlationId = Guid.NewGuid().ToString();
        return new Context(publisher, correlationId);
    }
}
