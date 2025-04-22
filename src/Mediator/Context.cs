using Oleexo.UnambitiousFx.Core;
using Oleexo.UnambitiousFx.Core.Abstractions;
using Oleexo.UnambitiousFx.Mediator.Abstractions;

namespace Oleexo.UnambitiousFx.Mediator;

public sealed class Context : IContext {
    private readonly Dictionary<string, object> _data = new();

    public Context() {
        CorrelationId = Guid.CreateVersion7();
        OccuredAt     = DateTimeOffset.UtcNow;
    }

    public Guid           CorrelationId { get; }
    public DateTimeOffset OccuredAt     { get; }

    public void Set<TValue>(string key,
                            TValue value)
        where TValue : notnull {
        _data[key] = value;
    }

    public IOption<TValue> Get<TValue>(string key)
        where TValue : notnull {
        if (_data.TryGetValue(key, out var value) &&
            value is TValue valueAsTValue) {
            return Option<TValue>.Some(valueAsTValue);
        }

        return Option<TValue>.None;
    }
}
