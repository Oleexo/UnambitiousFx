using System.Diagnostics.CodeAnalysis;
using UnambitiousFx.Core;

namespace UnambitiousFx.Mediator.Abstractions;

/// <summary>
///     Represents a context with properties and methods for managing data during a process or operation.
/// </summary>
public interface IContext {
    /// <summary>
    ///     Gets the unique identifier that represents a correlation ID for tracing or tracking purposes within the context.
    ///     This property is immutable and ensures consistent identification of a specific operation or event flow.
    /// </summary>
    Guid CorrelationId { get; }

    /// Gets the exact date and time at which the operation or context occurred.
    /// This property provides a timestamp that can be used for logging,
    /// debugging, or tracking purposes.
    DateTimeOffset OccuredAt { get; }

    /// <summary>
    ///     Sets a value in the context with the specified key.
    /// </summary>
    /// <typeparam name="TValue">The type of value being set.</typeparam>
    /// <param name="key">The unique key to associate with the value.</param>
    /// <param name="value">The value to store in the context.</param>
    void Set<TValue>(string key,
                     TValue value)
        where TValue : notnull;

    /// <summary>
    ///     Retrieves a value associated with the specified key, if it exists.
    /// </summary>
    /// <typeparam name="TValue">The type of the value to retrieve.</typeparam>
    /// <param name="key">The key for which the value should be retrieved.</param>
    /// <returns>
    ///     An <see cref="Option{TValue}" /> containing the value if the key is found and the value
    ///     is of type <typeparamref name="TValue" />; otherwise, it returns a None option.
    /// </returns>
    Option<TValue> Get<TValue>(string key)
        where TValue : notnull;

    /// <summary>
    ///     Attempts to retrieve a value associated with the specified key.
    /// </summary>
    /// <typeparam name="TValue">The type of the value to retrieve.</typeparam>
    /// <param name="key">The key associated with the value to retrieve.</param>
    /// <param name="value">
    ///     When this method returns, contains the value associated with the specified key,
    ///     if the key is found and the value is of the requested type; otherwise, the default value for the type of the out
    ///     parameter.
    ///     This parameter is passed uninitialized.
    /// </param>
    /// <returns>
    ///     <c>true</c> if the key exists and the value is of the requested type; otherwise, <c>false</c>.
    /// </returns>
    bool TryGet<TValue>(string                          key,
                        [NotNullWhen(true)] out TValue? value);

    /// <summary>
    ///     Publishes an event asynchronously.
    /// </summary>
    /// <typeparam name="TEvent">The type of the event to be published, which must implement <see cref="IEvent" />.</typeparam>
    /// <param name="event">The event instance to be published.</param>
    /// <param name="cancellationToken">
    ///     A cancellation token to observe while waiting for the task to complete. Defaults to
    ///     <see cref="CancellationToken.None" /> if not provided.
    /// </param>
    /// <returns>
    ///     A <see cref="ValueTask" /> containing a <see cref="Result" /> that indicates the success or failure of the
    ///     operation.
    /// </returns>
    ValueTask<Result> PublishAsync<TEvent>(TEvent            @event,
                                           CancellationToken cancellationToken = default)
        where TEvent : IEvent;
}
