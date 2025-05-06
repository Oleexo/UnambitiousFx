namespace UnambitiousFx.Mediator.Abstractions;

/// <summary>
///     Represents a factory responsible for creating <see cref="IContext" /> instances.
/// </summary>
public interface IContextFactory<TContext>
    where TContext : IContext {
    /// <summary>
    ///     Creates a new instance of <see cref="IContext" />.
    /// </summary>
    /// <returns>A new instance of <see cref="IContext" />.</returns>
    TContext Create();
}
