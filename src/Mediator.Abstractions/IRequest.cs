namespace UnambitiousFx.Mediator.Abstractions;

/// <summary>
///     Represents a marker interface that defines a request within the mediator pattern.
/// </summary>
public interface IRequest<TResponse>;

/// Represents a command or query that can be sent through the mediator for processing.
/// This interface sets the base contract for requests that do not return a response.
public interface IRequest;
