using Oleexo.UnambitiousFx.Core.Abstractions;

namespace Oleexo.UnambitiousFx.Mediator.Abstractions;

public interface IContext {
    Guid           CorrelationId { get; }
    DateTimeOffset OccuredAt     { get; }

    void Set<TValue>(string key,
                     TValue value)
        where TValue : notnull;

    IOption<TValue> Get<TValue>(string key)
        where TValue : notnull;
}
