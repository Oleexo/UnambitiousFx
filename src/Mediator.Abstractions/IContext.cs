using Oleexo.UnambitiousFx.Core;

namespace Oleexo.UnambitiousFx.Mediator.Abstractions;

public interface IContext {
    Guid           CorrelationId { get; }
    DateTimeOffset OccuredAt     { get; }

    void Set<TValue>(string key,
                     TValue value)
        where TValue : notnull;

    Option<TValue> Get<TValue>(string key)
        where TValue : notnull;

    ValueTask<Result> PublishAsync<TEvent>(TEvent            @event,
                                           CancellationToken cancellationToken = default)
        where TEvent : IEvent;
}
