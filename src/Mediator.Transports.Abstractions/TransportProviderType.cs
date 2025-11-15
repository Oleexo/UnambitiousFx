namespace UnambitiousFx.Mediator.Transports.Abstractions;

/// <summary>
///     Defines the type of transport provider implementation.
/// </summary>
public enum TransportProviderType
{
    /// <summary>
    ///     Built-in transport provider using direct broker integrations.
    /// </summary>
    BuiltIn = 0,

    /// <summary>
    ///     MassTransit-based transport provider.
    /// </summary>
    MassTransit = 1,

    /// <summary>
    ///     Wolverine-based transport provider.
    /// </summary>
    Wolverine = 2,

    /// <summary>
    ///     Custom transport provider implementation.
    /// </summary>
    Custom = 3
}
