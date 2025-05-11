using System.Diagnostics.CodeAnalysis;
using UnambitiousFx.Mediator.Abstractions;

namespace UnambitiousFx.Mediator;

/// <summary>
///     Represents the default context implementation for operations within the mediator framework.
/// </summary>
/// <remarks>
///     This class provides a unique correlation identifier and a timestamp indicating when the context was created.
///     It supports cloning to create a copy of the current context with the same values.
/// </remarks>
[SuppressMessage("ReSharper", "ClassCanBeSealed.Global")]
public class DefaultContext : IContext {
    /// <summary>
    ///     Represents the default context implementation for handling correlation and timing data within the mediator
    ///     framework.
    /// </summary>
    /// <remarks>
    ///     Provides a correlation identifier and a timestamp indicating the moment the context was initialized.
    ///     Allows cloning to generate a new instance replicating the current context's data.
    /// </remarks>
    public DefaultContext() {
        CorrelationId = Guid.CreateVersion7();
        OccuredAt     = DateTimeOffset.UtcNow;
    }

    private DefaultContext(DefaultContext context) {
        CorrelationId = context.CorrelationId;
        OccuredAt     = context.OccuredAt;
    }

    /// <inheritdoc />
    public Guid CorrelationId { get; }

    /// <inheritdoc />
    public DateTimeOffset OccuredAt { get; }

    /// <summary>
    ///     Creates a new instance of <see cref="DefaultContext" /> with the same properties as the current instance.
    /// </summary>
    /// <returns>
    ///     A new <see cref="DefaultContext" /> instance containing the same correlation identifier and timestamp as the
    ///     original instance.
    /// </returns>
    public DefaultContext Clone() {
        return new DefaultContext(this);
    }
}
