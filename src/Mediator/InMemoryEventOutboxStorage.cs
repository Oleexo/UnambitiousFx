using UnambitiousFx.Core.Results;
using UnambitiousFx.Mediator.Abstractions;

namespace UnambitiousFx.Mediator;

/// <summary>
///     Provides an in-memory implementation of the <see cref="IEventOutboxStorage" /> interface.
///     This class is designed to store and manage events transiently within the application process.
/// </summary>
/// <remarks>
///     This implementation is useful for development and testing scenarios where a persistent storage mechanism is not
///     required.
///     Since the storage is in-memory, all data will be lost when the application process is terminated.
/// </remarks>
/// <threadsafety>
///     This class is not guaranteed to be thread-safe. It is recommended to ensure proper synchronization
///     if accessed concurrently from multiple threads.
/// </threadsafety>
public sealed class InMemoryEventOutboxStorage : IEventOutboxStorage {
    private readonly List<Item> _items = [];

    /// <inheritdoc />
    public ValueTask<IEnumerable<IEvent>> GetPendingEventsAsync(CancellationToken cancellationToken = default) {
        return new ValueTask<IEnumerable<IEvent>>(_items.Where(item => !item.Processed)
                                                        .Select(item => item.Event));
    }

    /// <inheritdoc />
    public ValueTask<Result> MarkAsProcessedAsync(IEvent            @event,
                                                  CancellationToken cancellationToken = default) {
        var item = _items.FirstOrDefault(i => i.Event.Equals(@event));
        if (item == null) {
            return new ValueTask<Result>(Result.Failure($"Event '{@event}' was not found in the outbox storage"));
        }

        item.Processed = true;
        return new ValueTask<Result>(Result.Success());
    }

    /// <inheritdoc />
    public ValueTask<Result> ClearAsync(CancellationToken cancellationToken = default) {
        _items.Clear();
        return new ValueTask<Result>(Result.Success());
    }

    /// <inheritdoc />
    public ValueTask<Result> AddAsync<TEvent>(TEvent            @event,
                                              CancellationToken cancellationToken = default)
        where TEvent : class, IEvent {
        _items.Add(new Item(@event));

        return new ValueTask<Result>(Result.Success());
    }

    private sealed record Item {
        public Item(IEvent @event) {
            Event     = @event;
            Processed = false;
        }

        public IEvent Event     { get; }
        public bool   Processed { get; set; }
    }
}
