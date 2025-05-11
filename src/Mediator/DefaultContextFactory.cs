using UnambitiousFx.Mediator.Abstractions;

namespace UnambitiousFx.Mediator;

internal sealed class DefaultContextFactory : IContextFactory<IContext> {
    public IContext Create() {
        return new DefaultContext();
    }
}
