namespace UnambitiousFx.Mediator;

internal sealed record EventDispatcherOptions
{
    public IReadOnlyDictionary<Type, DispatchEventDelegate> Dispatchers { get; set; } = new Dictionary<Type, DispatchEventDelegate>();
}
