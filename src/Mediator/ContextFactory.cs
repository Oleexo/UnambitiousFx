using Oleexo.UnambitiousFx.Mediator.Abstractions;

namespace Oleexo.UnambitiousFx.Mediator;

internal sealed class ContextFactory : IContextFactory {
    private readonly IPublisher _publisher;

    public ContextFactory(IPublisher publisher) {
        _publisher = publisher;
    }

    public IContext Create() {
        return new Context(_publisher);
    }
}
