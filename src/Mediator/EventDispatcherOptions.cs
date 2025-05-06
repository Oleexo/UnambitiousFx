using UnambitiousFx.Mediator.Abstractions;

namespace UnambitiousFx.Mediator;

internal sealed record EventDispatcherOptions<TContext>
    where TContext : IContext {
    public IReadOnlyDictionary<Type, DispatchEventDelegate<TContext>> Dispatchers { get; set; } = new Dictionary<Type, DispatchEventDelegate<TContext>>();
}
