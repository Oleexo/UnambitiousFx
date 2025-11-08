using UnambitiousFx.Mediator.Abstractions;

namespace UnambitiousFx.Mediator;

internal sealed class ContextFactory : IContextFactory
{
    private readonly IPublisher _publisher;

    public ContextFactory(IPublisher publisher)
    {
        _publisher = publisher;
    }

    public IContext Create()
    {
        return new Context(_publisher);
    }
}
